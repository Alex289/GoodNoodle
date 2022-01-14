import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Guid } from 'guid-typescript';
import { Observable } from 'rxjs';
import { EmptyApiResponse } from 'src/app/types/api/emptyApiResponse';
import { InviteToGroup } from 'src/app/types/entities/userInGroup/inviteToGroup';
import { JoinGroup } from 'src/app/types/entities/userInGroup/joinGroup';
import { environment } from 'src/environments/environment';
import { ApiResponse } from '../../types/api/apiResponse';
import { Group } from '../../types/entities/group';
import { User } from '../../types/entities/user/user';
import { UserInGroup } from '../../types/entities/userInGroup';

@Injectable({
  providedIn: 'root'
})
export class UserInGroupService {
  constructor(private http: HttpClient) {}

  getAll(): Observable<ApiResponse<UserInGroup[]>> {
    const url = `${environment.api.baseUrl}/group/user/all`;
    return this.http.get<ApiResponse<UserInGroup[]>>(url);
  }

  getByUser(userid: Guid): Observable<ApiResponse<Group[]>> {
    const url = `${environment.api.baseUrl}/user/${userid}/groups`;
    return this.http.get<ApiResponse<Group[]>>(url);
  }

  getByGroup(groupid: Guid): Observable<ApiResponse<User[]>> {
    const url = `${environment.api.baseUrl}/group/${groupid}/users`;
    return this.http.get<ApiResponse<User[]>>(url);
  }

  joinGroup(joinGroup: JoinGroup): Observable<ApiResponse<UserInGroup>> {
    const url = `${environment.api.baseUrl}/group/join`;
    return this.http.post<ApiResponse<UserInGroup>>(url, joinGroup);
  }

  inviteToGroup(
    inviteToGroup: InviteToGroup
  ): Observable<ApiResponse<EmptyApiResponse>> {
    const url = `${environment.api.baseUrl}/group/invite`;
    return this.http.post<ApiResponse<EmptyApiResponse>>(url, inviteToGroup);
  }

  removeUserFromGroup(id: Guid): Observable<ApiResponse<User[]>> {
    const url = `${environment.api.baseUrl}/group/user/leave/${id}`;
    return this.http.delete<ApiResponse<User[]>>(url);
  }
}
