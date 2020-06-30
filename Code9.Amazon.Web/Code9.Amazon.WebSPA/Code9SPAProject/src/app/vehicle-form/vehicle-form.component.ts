import { VehicleTosave } from './../_models/vehicleToSave';
import { FeaturesService } from './../_services/features.service';
import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { Model } from '../_models/model';
import { Make } from '../_models/make';
import { VehicleService } from '../_services/vehicles.service';
import { MakesService } from '../_services/makes.service';
import { ModelsService } from '../_services/models.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { Feature } from '../_models/feature';
import { Vehicle } from '../_models/vehicle';
import { forkJoin } from 'rxjs';
import { Image } from '../_models/image';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.scss'],
})
export class VehicleFormComponent implements OnInit {
  models: Model[] = [];
  makes: Make[] = [];
  features: Feature[];
  sources: any[];
  images: Image[];
  fuelList: string[] = ['Benzin', 'Dizel', 'Metan', 'Gas', 'Električno'];
  vehicle: VehicleTosave = new VehicleTosave();
  @ViewChild('editForm', { static: false }) editForm: NgForm;
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private vehiclesService: VehicleService,
    private makesService: MakesService,
    private modelsService: ModelsService,
    private featuresService: FeaturesService,
    private toastr: ToastrService
  ) {
    route.params.subscribe((p) => {
      this.vehicle.id = +p.id || 0;
    });
  }

  ngOnInit(): void {
    this.sources = [this.makesService.getMakes(), this.featuresService.getFeatures()];

    if (this.vehicle.id) {
      this.sources.push(this.vehiclesService.getVehicle(this.vehicle.id));
    }

    forkJoin(this.sources).subscribe((data: any) => {
      this.makes = data[0];
      this.features = data[1];
      if (this.vehicle.id) {
        this.setVehicle(data[2]);
        this.populateModels();
      }
    });
  }

  populateModels() {
    this.modelsService.getModelsByMakeId(this.vehicle.makeId).subscribe((response) => {
      this.models = response;
    });
  }

  loadMakes() {
    this.makesService.getMakes().subscribe((response) => {
      this.makes = response;
    });
  }

  loadFeatures() {
    this.featuresService.getFeatures().subscribe((response) => {
      this.features = response;
    });
  }

  setVehicle(v: Vehicle) {
    this.vehicle.id = v.id;
    this.vehicle.makeId = v.make.id;
    this.vehicle.modelId = v.model.id;
    this.vehicle.isRegistered = v.isRegistered;
    this.vehicle.price = v.price;
    this.vehicle.userId = v.userId;
    this.vehicle.city = v.city;
    this.vehicle.description = v.description;
    this.vehicle.mileage = v.mileage;
    this.vehicle.productionYear = v.productionYear;
    this.vehicle.fuelType = v.fuelType;
    this.vehicle.features = v.features.map((f) => f.id);
    this.vehicle.images = v.images.map((i) => i.id);
    this.images = v.images;
  }

  submit() {
    if (this.vehicle.id) {
      this.vehiclesService.updateVehicle(this.vehicle).subscribe(
        (response) => {
          this.toastr.success('Vehicle record has been updated', 'Success');
          this.router.navigate(['vehicle/' + response]);
        },
        (error) => {
          this.toastr.error(error);
        }
      );
    } else {
      this.vehiclesService.createVehicle(this.vehicle).subscribe(
        (response) => {
          this.toastr.success('Vehicle record has been created', 'Success');
          this.router.navigate(['vehicle/' + response.id]);
        },
        (error) => {
          this.toastr.error(error);
        }
      );
    }
  }

  cancelChanges() {
    if (this.vehicle.id == 0) {
      this.router.navigate(['home/']);
    } else {
      this.router.navigate(['vehicle/' + this.vehicle.id]);
    }
  }
}
