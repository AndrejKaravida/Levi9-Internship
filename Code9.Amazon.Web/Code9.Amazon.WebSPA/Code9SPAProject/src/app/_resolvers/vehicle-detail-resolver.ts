import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Vehicle } from '../_models/vehicle';
import { VehicleService } from '../_services/vehicles.service';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class VehicleDetailResolver implements Resolve<Vehicle> {
  constructor(
    private vehicleService: VehicleService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Vehicle> {
    return this.vehicleService.getVehicle(route.params.id).pipe(
      catchError(() => {
        this.toastr.error('Problem retrieving data!');
        this.router.navigate(['/home']);
        return of(null);
      })
    );
  }
}
