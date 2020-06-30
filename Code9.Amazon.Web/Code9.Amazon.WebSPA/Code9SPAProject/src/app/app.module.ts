import { UserListResolver } from './_resolvers/user-list-resolver';
import { MessagesResolver } from './_resolvers/messages-resolver';
import { MessagesService } from './_services/messages.service';
import { MatRadioModule } from '@angular/material/radio';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './user/login/login.component';
import { RegisterComponent } from './user/register/register.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HomeComponent } from './home/home.component';
import { AuthService } from './_services/auth.service';
import { ToastrModule } from 'ngx-toastr';
import { AuthGuard } from './_guards/auth.guard';
import { MaterialModule } from './material.module';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { VehicleCardComponent } from './home/vehicle-card/vehicle-card.component';
import { VehicleService } from './_services/vehicles.service';
import { VehicleListResolver } from './_resolvers/vehicle-list-resolver';
import { ModelsService } from './_services/models.service';
import { MakesService } from './_services/makes.service';
import { VehicleDetailComponent } from './vehicle-detail/vehicle-detail.component';
import { NgxGalleryModule } from 'ngx-gallery-9';
import { VehicleDetailResolver } from './_resolvers/vehicle-detail-resolver';
import { RegistrationPipe } from './registration.pipe';
import { NavbarComponent } from './navbar/navbar.component';
import { FooterComponent } from './footer/footer.component';
import { CommentService } from './_services/comments.service';
import { NewCommentDialogComponent } from './_dialogs/new-comment-dialog/new-comment-dialog.component';
import { VehicleFormComponent } from './vehicle-form/vehicle-form.component';
import { FeaturesService } from './_services/features.service';
import { PhotoUploadComponent } from './photo-upload/photo-upload.component';
import { FileUploadModule } from 'ng2-file-upload';
import { ImagesService } from './_services/images.service';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes-guard';
import { MessagesComponent } from './messages/messages.component';
import { UserCardComponent } from './users/user-card/user-card.component';
import { UserListComponent } from './users/user-list/user-list.component';
import { UsersService } from './_services/users.service';
import { UserMessageComponent } from './users/user-message/user-message.component';
import { TimeAgoExtPipe } from './time-ago-ext.pipe';
import { AdminGuard } from './_guards/admin.guard';

@NgModule({
  declarations: [
    AppComponent,
    UserComponent,
    LoginComponent,
    NewCommentDialogComponent,
    RegisterComponent,
    HomeComponent,
    VehicleCardComponent,
    VehicleDetailComponent,
    RegistrationPipe,
    NavbarComponent,
    FooterComponent,
    VehicleFormComponent,
    PhotoUploadComponent,
    MessagesComponent,
    TimeAgoExtPipe,
    UserCardComponent,
    UserListComponent,
    UserMessageComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MaterialModule,
    BsDropdownModule.forRoot(),
    NgxGalleryModule,
    PaginationModule.forRoot(),
    ReactiveFormsModule,
    FormsModule,
    FileUploadModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    TabsModule.forRoot(),
    MatRadioModule,
  ],
  providers: [
    AuthService,
    AuthGuard,
    AdminGuard,
    VehicleService,
    VehicleListResolver,
    MakesService,
    ModelsService,
    ImagesService,
    VehicleDetailResolver,
    CommentService,
    PreventUnsavedChanges,
    FeaturesService,
    MessagesService,
    MessagesResolver,
    UsersService,
    UserListResolver,
  ],
  bootstrap: [AppComponent],
  entryComponents: [NewCommentDialogComponent],
})
export class AppModule {}
