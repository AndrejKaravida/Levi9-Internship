import { Component, OnInit, OnDestroy } from '@angular/core';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery-9';
import { Vehicle } from '../_models/vehicle';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { NewCommentDialogComponent } from '../_dialogs/new-comment-dialog/new-comment-dialog.component';
import { CommentService } from '../_services/comments.service';
import { Comment } from '../_models/comment';
import { Subscription } from 'rxjs';
import { VehicleService } from './../_services/vehicles.service';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-vehicle-detail',
  templateUrl: './vehicle-detail.component.html',
  styleUrls: ['./vehicle-detail.component.scss'],
})
export class VehicleDetailComponent implements OnInit, OnDestroy {
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  vehicle: Vehicle;
  comments: Comment[] = [];
  id: number;
  subscription: Subscription;
  photoUrl: string;
  userId: number;

  constructor(
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private commentService: CommentService,
    private router: Router,
    private vehiclesService: VehicleService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.userId = this.authService.getNameId();
    this.commentService.startConnection();
    this.commentService.addTransferCommentsListener();

    this.galleryOptions = [
      {
        width: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageBullets: true,
        previewCloseOnClick: true,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false,
      },
    ];

    this.route.data.subscribe((response) => {
      this.vehicle = response.vehicles;
      this.loadComments();
    });

    this.subscription = this.commentService.commentData.subscribe((response) => {
      this.loadComments();
    });

    this.galleryImages = this.getImages();

    this.vehicle.images.forEach((image) => {
      if (image.isMain) {
        this.photoUrl = image.fileName;
      }
    });
  }

  loadComments() {
    this.commentService.getCommentsForVehicle(this.vehicle.id).subscribe((response) => {
      this.comments = response;
    });
  }

  getImages() {
    const imageUrls = [];
    this.vehicle.images.forEach((image) => {
      imageUrls.push({
        small: image.fileName,
        medium: image.fileName,
        big: image.fileName,
        description: '',
      });
    });

    return imageUrls;
  }

  makeNewComment() {
    this.dialog.open(NewCommentDialogComponent, {
      width: '500px',
      height: '750px',
      data: { vehicleId: this.vehicle.id },
    });
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  editVehicle() {
    this.router.navigate(['edit/' + this.vehicle.id]);
  }

  deleteVehicle() {
    this.vehiclesService.deleteVehicle(this.vehicle.id);
    this.router.navigate(['/home']);
  }
}
