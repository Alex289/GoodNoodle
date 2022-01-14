import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Guid } from 'guid-typescript';
import { ApiResponse } from '../../types/api/apiResponse';
import { Star } from '../../types/entities/star/star';
import { EmptyApiResponse } from '../../types/api/emptyApiResponse';
import { CreateStar } from '../../types/entities/star/createStar';

@Injectable({
  providedIn: 'root'
})
export class StarService {
  constructor(private http: HttpClient) {}

  getStarByUserId(userId: Guid): Observable<ApiResponse<Star[]>> {
    const url = `${environment.api.baseUrl}/star/user/${userId}`;
    return this.http.get<ApiResponse<Star[]>>(url);
  }

  createStar(star: CreateStar): Observable<ApiResponse<Star>> {
    const url = `${environment.api.baseUrl}/star`;
    return this.http.post<ApiResponse<Star>>(url, star);
  }

  editStar(star: Star): Observable<ApiResponse<EmptyApiResponse>> {
    const url = `${environment.api.baseUrl}/star/${star.id}`;
    return this.http.put<ApiResponse<EmptyApiResponse>>(url, star);
  }

  deleteStar(starId: Guid): Observable<ApiResponse<EmptyApiResponse>> {
    const url = `${environment.api.baseUrl}/star/${starId}`;
    return this.http.delete<ApiResponse<EmptyApiResponse>>(url);
  }
}
