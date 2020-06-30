import { Component, OnInit, Input } from '@angular/core';
import { Vehicle } from 'src/app/_models/vehicle';
import { Router } from '@angular/router';

@Component({
  selector: 'app-vehicle-card',
  templateUrl: './vehicle-card.component.html',
  styleUrls: ['./vehicle-card.component.scss'],
})
export class VehicleCardComponent implements OnInit {
  @Input() vehicle: Vehicle;
  photoUrl: string;

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.vehicle.images.forEach((image) => {
      if (image.isMain) {
        this.photoUrl = image.fileName;
      }
    });
  }

  onVehicleDetails() {
    this.router.navigate(['vehicle/' + this.vehicle.id]);
  }
}
