import { UserMessageComponent } from './users/user-message/user-message.component';
import { UserListResolver } from './_resolvers/user-list-resolver';
import { UserListComponent } from './users/user-list/user-list.component';
import { MessagesComponent } from './messages/messages.component';
import { VehicleFormComponent } from './vehicle-form/vehicle-form.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserComponent } from './user/user.component';
import { RegisterComponent } from './user/register/register.component';
import { LoginComponent } from './user/login/login.component';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './_guards/auth.guard';
import { VehicleListResolver } from './_resolvers/vehicle-list-resolver';
import { VehicleDetailComponent } from './vehicle-detail/vehicle-detail.component';
import { VehicleDetailResolver } from './_resolvers/vehicle-detail-resolver';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { InterceptorService } from './_services/interceptor.service';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes-guard';
import { MessagesResolver } from './_resolvers/messages-resolver';
import { AdminGuard } from './_guards/admin.guard';

const routes: Routes = [
  { path: '', redirectTo: '/user/login', pathMatch: 'full' },
  {
    path: 'user',
    component: UserComponent,
    children: [
      { path: 'registration', component: RegisterComponent },
      { path: 'login', component: LoginComponent },
    ],
  },
  {
    path: 'home',
    component: HomeComponent,
    canActivate: [AuthGuard],
    resolve: { vehicles: VehicleListResolver },
  },
  {
    path: 'vehicle/:id',
    component: VehicleDetailComponent,
    canActivate: [AuthGuard],
    resolve: { vehicles: VehicleDetailResolver },
  },
  {
    path: 'new',
    canActivate: [AuthGuard],
    component: VehicleFormComponent,
  },
  {
    path: 'edit/:id',
    component: VehicleFormComponent,
    canActivate: [AuthGuard],
    resolve: { vehicles: VehicleDetailResolver },
    canDeactivate: [PreventUnsavedChanges],
  },
  {
    path: 'messages',
    component: MessagesComponent,
    canActivate: [AuthGuard],
    resolve: { messages: MessagesResolver },
  },
  {
    path: 'users',
    component: UserListComponent,
    canActivate: [AuthGuard],
    resolve: { messages: UserListResolver },
  },
  {
    path: 'chat/:id',
    component: UserMessageComponent,
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'enabled' })],
  exports: [RouterModule],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: InterceptorService,
      multi: true,
    },
  ],
})
export class AppRoutingModule {}
