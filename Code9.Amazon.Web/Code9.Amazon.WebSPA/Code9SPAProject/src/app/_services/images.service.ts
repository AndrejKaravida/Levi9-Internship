import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class ImagesService {
  baseUrl = `${environment.apiUrl}images/`;

  constructor(private http: HttpClient) {}

  setMain(photoId: number, vehicleId: number) {
    return this.http.post(`${this.baseUrl}setMain/${vehicleId}/${photoId}`, {});
  }

  deleteImage(photoId: number) {
    return this.http.delete(this.baseUrl + photoId);
  }
}
