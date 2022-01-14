import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ApiResponse } from '../../types/api/apiResponse';
import { EmptyApiResponse } from '../../types/api/emptyApiResponse';
import { User } from '../../types/entities/user/user';
import { Guid } from 'guid-typescript';
import { ChangePassword } from 'src/app/types/entities/user/changePassword';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient) {}

  getAllUsers(): Observable<ApiResponse<User[]>> {
    const url = `${environment.api.baseUrl}/user`;
    return this.http.get<ApiResponse<User[]>>(url);
  }

  getUserById(userId: Guid): Observable<ApiResponse<User>> {
    const url = `${environment.api.baseUrl}/user/${userId}`;
    return this.http.get<ApiResponse<User>>(url);
  }

  searchByName(userName: string): Observable<ApiResponse<User[]>> {
    const url = `${environment.api.baseUrl}/user/search/${userName}`;
    return this.http.get<ApiResponse<User[]>>(url);
  }

  editUser(user: User): Observable<ApiResponse<User>> {
    const url = `${environment.api.baseUrl}/user/${user.id}`;
    return this.http.put<ApiResponse<User>>(url, user);
  }

  changePassword(passwords: ChangePassword): Observable<EmptyApiResponse> {
    const url = `${environment.api.baseUrl}/user/change-password`;
    return this.http.put<EmptyApiResponse>(url, passwords);
  }

  deleteUser(Id: Guid): Observable<ApiResponse<EmptyApiResponse>> {
    const url = `${environment.api.baseUrl}/user/${Id}`;
    return this.http.delete<ApiResponse<EmptyApiResponse>>(url);
  }
}
