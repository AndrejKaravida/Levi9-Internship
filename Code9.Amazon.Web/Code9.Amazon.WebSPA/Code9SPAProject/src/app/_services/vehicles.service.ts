import { VehicleTosave } from './../_models/vehicleToSave';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Vehicle } from '../_models/vehicle';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';
import { VehicleParams } from '../_models/vehicleParams';

@Injectable({
  providedIn: 'root',
})
export class VehicleService {
  baseUrl = `${environment.apiUrl}vehicles/`;

  constructor(private http: HttpClient) {}

  getVehicles(
    page?: number,
    itemsPerPage?: number,
    vehicleParams?: VehicleParams
  ): Observable<PaginatedResult<Vehicle[]>> {
    const paginatedResult: PaginatedResult<Vehicle[]> = new PaginatedResult<Vehicle[]>();

    let params = new HttpParams();

    if (page && itemsPerPage) {
      params = params
        .append('pageNumber', page.toString())
        .append('pageSize', itemsPerPage.toString());
    }

    if (vehicleParams) {
      params = params
        .append('make', vehicleParams.make.toString())
        .append('model', vehicleParams.model.toString())
        .append('fuelType', vehicleParams.fuel)
        .append('city', vehicleParams.city)
        .append('maxMileage', vehicleParams.maxMileage.toString())
        .append('minPrice', vehicleParams.minPrice.toString())
        .append('maxPrice', vehicleParams.maxPrice.toString())
        .append('minYear', vehicleParams.minYear.toString())
        .append('maxYear', vehicleParams.maxYear.toString())
        .append('orderBy', vehicleParams.orderBy);
    }

    return this.http
      .get<Vehicle[]>(this.baseUrl, { observe: 'response', params })
      .pipe(
        map((response) => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') !== null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
  }

  getVehicle(id: number): Observable<Vehicle> {
    return this.http.get<Vehicle>(this.baseUrl + id);
  }

  getAdminIdForVehicle(vehicleid: number): Observable<number> {
    return this.http.get<number>(this.baseUrl + 'admin/' + vehicleid);
  }

  createVehicle(vehicle: VehicleTosave): Observable<Vehicle> {
    return this.http.post<Vehicle>(this.baseUrl, vehicle);
  }

  updateVehicle(vehicle: VehicleTosave): Observable<Vehicle> {
    return this.http.put<Vehicle>(this.baseUrl + vehicle.id, vehicle);
  }

  deleteVehicle(id: number) {
    return this.http.delete(this.baseUrl + id).subscribe();
  }
}
