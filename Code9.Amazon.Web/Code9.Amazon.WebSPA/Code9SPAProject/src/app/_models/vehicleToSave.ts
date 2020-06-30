export class VehicleTosave {
  id: number;
  modelId: number;
  makeId: number;
  price: number;
  isRegistered: boolean;
  city: string;
  userId: number;
  description: string;
  mileage: number;
  productionYear: number;
  fuelType: string;
  features: number[];
  images: number[];

  constructor() {
    this.id = 0;
    this.modelId = 0;
    this.makeId = 0;
    this.userId = 0;
    this.price = 0;
    this.isRegistered = false;
    this.city = '';
    this.description = '';
    this.mileage = 0;
    this.productionYear = 0;
    this.fuelType = '';
    this.features = [];
    this.images = [];
  }
}
