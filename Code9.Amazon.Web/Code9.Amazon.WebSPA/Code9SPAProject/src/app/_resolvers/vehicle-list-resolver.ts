import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Vehicle } from '../_models/vehicle';
import { VehicleService } from '../_services/vehicles.service';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class VehicleListResolver implements Resolve<Vehicle[]> {
  pageNumber = 1;
  pageSize = 8;

  constructor(
    private vehicleService: VehicleService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Vehicle[]> {
    return this.vehicleService.getVehicles(this.pageNumber, this.pageSize).pipe(
      catchError(() => {
        this.toastr.error('Problem retrieving data!');
        this.router.navigate(['/user/login']);
        return of(null);
      })
    );
  }
}
