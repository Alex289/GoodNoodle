import { Component, OnInit } from '@angular/core';
import { NbAuthService, NbAuthToken } from '@nebular/auth';
import { NbDialogService } from '@nebular/theme';
import { Guid } from 'guid-typescript';
import { ToastrService } from 'ngx-toastr';
import { CLAIMTYPES_ID, CLAIMTYPES_STATUS } from 'src/app/lib/claimtypes';
import { GroupService } from 'src/app/services/api/group.service';
import { Group } from 'src/app/types/entities/group';
import { CreateGroupDialogComponent } from '../dialogs/create-group-dialog/create-group-dialog.component';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
  isAuthenticated = false;
  userStatus = false;
  groupsOfUser: Group[] = [];
  joinableGroups: Group[] = [];
  groupId: Guid = Guid.create();
  userId: Guid = Guid.createEmpty();

  constructor(
    private groupService: GroupService,
    private dialogService: NbDialogService,
    private authService: NbAuthService,
    private toaster: ToastrService
  ) {
    this.authService.onTokenChange().subscribe((token: NbAuthToken) => {
      if (token && token.isValid()) {
        const decodedToken = token.getPayload();

        this.userId = decodedToken[CLAIMTYPES_ID];

        this.userStatus =
          decodedToken[CLAIMTYPES_STATUS] === 'Accepted' ? true : false;

        if (this.userStatus) {
          this.getGroupsOfUser();
          this.getJoinableGroups();
          this.isAuthenticated = true;
        }
      }
    });
  }

  ngOnInit(): void {
    this.authService
      .isAuthenticated()
      .toPromise()
      .then((authenticated) => {
        this.isAuthenticated = authenticated || false;
      });

    if (this.isAuthenticated) {
      this.getGroupsOfUser();
      this.getJoinableGroups();
    }
  }

  async getGroupsOfUser(): Promise<void> {
    await this.groupService
      .getGroupsByUser(this.userId)
      .toPromise()
      .then((response) => {
        response?.data.forEach((element) => {
          if (element.image != '') {
            element.image = 'data:text/plain;base64,' + element.image;
          }
        });
        this.groupsOfUser = response?.data || [];
      })
      .catch((error) => {
        this.toaster.error('Failed to load groups of user');
        return error;
      });
  }

  async getJoinableGroups(): Promise<void> {
    await this.groupService
      .getJoinableGroups(this.userId)
      .toPromise()
      .then((response) => {
        response?.data.forEach((element) => {
          if (element.image != '') {
            element.image = 'data:text/plain;base64,' + element.image;
          }
        });
        this.joinableGroups = response?.data || [];
      })
      .catch((error) => {
        this.toaster.error('Failed to load joinable groups');
        return error;
      });
  }

  openWindow(): void {
    this.dialogService
      .open(CreateGroupDialogComponent)
      .onClose.subscribe(() => {
        this.getGroupsOfUser();
        this.getJoinableGroups();
      });
  }
}
