import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { Make } from '../_models/make';

@Injectable({
  providedIn: 'root',
})
export class MakesService {
  baseUrl = `${environment.apiUrl}makes/`;

  constructor(private http: HttpClient) {}

  getMakes(): Observable<Make[]> {
    return this.http.get<Make[]>(this.baseUrl);
  }
}
