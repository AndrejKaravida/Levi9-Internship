import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { VehicleFormComponent } from '../vehicle-form/vehicle-form.component';

@Injectable()
export class PreventUnsavedChanges implements CanDeactivate<VehicleFormComponent> {
  canDeactivate(component: VehicleFormComponent) {
    if (component.editForm.dirty) {
      return confirm('Are you sure you want to proceed? This action cannot be undone.');
    }
    return true;
  }
}
