import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { Model } from '../_models/model';

@Injectable({
  providedIn: 'root',
})
export class ModelsService {
  baseUrl = `${environment.apiUrl}models/`;

  constructor(private http: HttpClient) {}

  getModels(): Observable<Model[]> {
    return this.http.get<Model[]>(this.baseUrl);
  }

  getModelsByMakeId(makeId: number): Observable<Model[]> {
    return this.http.get<Model[]>(this.baseUrl + 'make/' + makeId);
  }
}
