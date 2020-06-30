import { KeyValuePair } from './keyvaluepair';
import { Feature } from './feature';
import { Comment } from './comment';
import { Image } from './image';

export interface Vehicle {
  id: number;
  model: KeyValuePair;
  make: KeyValuePair;
  price: number;
  isRegistered: boolean;
  city: string;
  description: string;
  mileage: number;
  userId: number;
  productionYear: number;
  fuelType: string;
  lastUpdated: Date;
  features: KeyValuePair[];
  comments: Comment[];
  images: Image[];
}
