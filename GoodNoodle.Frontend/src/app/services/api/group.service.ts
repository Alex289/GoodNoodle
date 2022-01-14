import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Injectable } from '@angular/core';
import { Guid } from 'guid-typescript';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../types/api/apiResponse';
import { EmptyApiResponse } from '../../types/api/emptyApiResponse';
import { Group } from '../../types/entities/group';
import { User } from '../../types/entities/user/user';

@Injectable({
  providedIn: 'root'
})
export class GroupService {
  constructor(private http: HttpClient) {}

  getAllGroups(): Observable<ApiResponse<Group[]>> {
    const url = `${environment.api.baseUrl}/group`;
    return this.http.get<ApiResponse<Group[]>>(url);
  }

  getGroupById(groupId: Guid): Observable<ApiResponse<Group>> {
    const url = `${environment.api.baseUrl}/group/${groupId}`;
    return this.http.get<ApiResponse<Group>>(url);
  }

  getGroupsByUser(userId: Guid): Observable<ApiResponse<Group[]>> {
    const url = `${environment.api.baseUrl}/user/${userId}/groups`;
    return this.http.get<ApiResponse<Group[]>>(url);
  }

  getJoinableGroups(userId: Guid): Observable<ApiResponse<Group[]>> {
    const url = `${environment.api.baseUrl}/group/joinable/${userId}`;
    return this.http.get<ApiResponse<Group[]>>(url);
  }

  getAllUsersInGroup(groupId: Guid): Observable<ApiResponse<User[]>> {
    const url = `${environment.api.baseUrl}/group/${groupId}/users`;
    return this.http.get<ApiResponse<User[]>>(url);
  }

  createNewGroup(name: string, image: string): Observable<ApiResponse<Group>> {
    const url = `${environment.api.baseUrl}/group`;
    return this.http.post<ApiResponse<Group>>(url, {
      Name: name,
      Image: image
    });
  }

  updateGroup(name: string, image: string): Observable<ApiResponse<Group>> {
    const url = `${environment.api.baseUrl}/group`;
    return this.http.put<ApiResponse<Group>>(url, {
      name: name,
      image: image
    });
  }

  removeGroup(id: Guid): Observable<ApiResponse<EmptyApiResponse>> {
    const url = `${environment.api.baseUrl}/group/${id}`;
    return this.http.delete<ApiResponse<EmptyApiResponse>>(url);
  }
}
