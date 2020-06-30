import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { VehicleService } from '../_services/vehicles.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class AdminGuard implements CanActivate {
  flag = false;

  constructor(
    private vehicleService: VehicleService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  canActivate(route: ActivatedRouteSnapshot): Observable<boolean> {
    return this.vehicleService.getAdminIdForVehicle(+route.paramMap.get('id')).pipe(
      map((response) => {
        if (response === +route.paramMap.get('id')) {
          return true;
        }
        this.toastr.error('You are not allowed to access this route!');
        this.router.navigate(['home']);
        return false;
      })
    );
  }
}
