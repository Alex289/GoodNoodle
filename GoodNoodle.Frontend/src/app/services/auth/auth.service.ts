import { Injectable } from '@angular/core';
import { NbAuthService, NbAuthToken } from '@nebular/auth';
import { Guid } from 'guid-typescript';
import { CLAIMTYPES_ID, CLAIMTYPES_ROLE } from 'src/app/lib/claimtypes';
import { User } from '../../types/entities/user/user';
import { UserInGroupService } from '../api/user-in-group.service';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(
    private authService: NbAuthService,
    private userInGroupService: UserInGroupService
  ) {}

  static getToken(): string {
    const token = localStorage.getItem('auth_app_token');

    if (token == null) {
      return '';
    }

    const parsedToken = JSON.parse(token);
    return parsedToken?.value;
  }

  async isTeacher(groupId: Guid): Promise<boolean> {
    let isTeacher = false;
    let userInGroups: User[];

    await this.userInGroupService
      .getByGroup(groupId)
      .toPromise()
      .then((response) => {
        userInGroups = response?.data || [];
      })
      .catch(() => {
        return false;
      });

    this.authService.onTokenChange().subscribe((token: NbAuthToken) => {
      if (token.isValid()) {
        const decodedToken = token.getPayload();
        const userId = decodedToken[CLAIMTYPES_ID];

        const user = userInGroups.find((user) => user.id == userId);

        isTeacher = user?.groupRole === 0 ? true : false;
      }
    });

    return isTeacher;
  }

  async isInGroup(groupId: Guid): Promise<boolean> {
    let isInGroup = false;
    let userInGroups: User[];

    await this.userInGroupService
      .getByGroup(groupId)
      .toPromise()
      .then((response) => {
        userInGroups = response?.data || [];
      })
      .catch(() => {
        return false;
      });

    this.authService.onTokenChange().subscribe((token: NbAuthToken) => {
      if (token.isValid()) {
        const decodedToken = token.getPayload();
        const userId = decodedToken[CLAIMTYPES_ID];

        const user = userInGroups.find((user) => user.id == userId);

        if (user) {
          isInGroup = true;
        }
      }
    });

    return isInGroup;
  }

  isAdmin(): boolean {
    let isAdmin = false;

    this.authService.onTokenChange().subscribe((token: NbAuthToken) => {
      if (token.isValid()) {
        const decodedToken = token.getPayload();
        const role = decodedToken[CLAIMTYPES_ROLE];

        isAdmin = role === 'Admin' ? true : false;
      }
    });

    return isAdmin;
  }

  getCurrentUserId(): Guid {
    let id = Guid.createEmpty();

    this.authService.onTokenChange().subscribe((token: NbAuthToken) => {
      if (token.isValid()) {
        const decodedToken = token.getPayload();
        id = decodedToken[CLAIMTYPES_ID];
      }
    });

    return id;
  }
}
