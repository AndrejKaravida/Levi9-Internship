import { Component, OnInit } from '@angular/core';
import { Pagination, PaginatedResult } from '../_models/pagination';
import { ActivatedRoute, Router } from '@angular/router';
import { Vehicle } from '../_models/vehicle';
import { VehicleService } from '../_services/vehicles.service';
import { MakesService } from '../_services/makes.service';
import { ModelsService } from '../_services/models.service';
import { Model } from '../_models/model';
import { Make } from '../_models/make';
import { ToastrService } from 'ngx-toastr';
import { VehicleParams } from '../_models/vehicleParams';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  pagination: Pagination;
  vehicles: Vehicle[];
  models: Model[];
  makes: Make[];
  fuelList: string[] = ['Benzin', 'Dizel', 'Metan', 'Gas', 'Električno'];
  citiesList: string[] = ['Novi Sad', 'Beograd', 'Kragujevac', 'Kraljevo', 'Niš', 'Sombor'];
  vehicleParams: VehicleParams = new VehicleParams();

  constructor(
    private route: ActivatedRoute,
    private vehicleService: VehicleService,
    private makesService: MakesService,
    private modelsService: ModelsService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.vehicles = data.vehicles.result;
      this.pagination = data.vehicles.pagination;
    });
    this.loadMakes();
    this.loadVehicles();
  }

  loadMakes() {
    this.makesService.getMakes().subscribe((response) => {
      this.makes = response;
    });
  }

  populateModels() {
    this.modelsService.getModelsByMakeId(this.vehicleParams.make).subscribe((response) => {
      this.models = response;
    });
  }

  loadVehicles() {
    this.vehicleService
      .getVehicles(this.pagination.currentPage, this.pagination.itemsPerPage, this.vehicleParams)
      .subscribe(
        (response: PaginatedResult<Vehicle[]>) => {
          this.vehicles = response.result;
          this.pagination = response.pagination;
        },
        (error) => {
          this.toastr.error('Error while loading vehicles!' + error);
        }
      );
  }

  reset() {
    this.vehicleParams = new VehicleParams();
    this.loadVehicles();
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadVehicles();
  }

  addNewVehicle() {
    this.router.navigate(['new']);
  }
}
