export class VehicleParams {
  make: number;
  model: number;
  fuel: string;
  city: string;
  maxMileage: number;
  minPrice: number;
  maxPrice: number;
  minYear: number;
  maxYear: number;
  orderBy: string;

  constructor() {
    this.make = 0;
    this.model = 0;
    this.fuel = '';
    this.city = '';
    this.maxMileage = 999999;
    this.minPrice = 0;
    this.maxPrice = 999999;
    this.minYear = 1950;
    this.maxYear = 2020;
    this.orderBy = 'None';
  }
}
