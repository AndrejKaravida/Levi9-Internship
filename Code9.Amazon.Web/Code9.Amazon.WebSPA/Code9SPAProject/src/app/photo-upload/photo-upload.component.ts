import { Component, OnInit, Input } from '@angular/core';
import { Image } from '../_models/image';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { ToastrService } from 'ngx-toastr';
import { ImagesService } from '../_services/images.service';

@Component({
  selector: 'app-photo-upload',
  templateUrl: './photo-upload.component.html',
  styleUrls: ['./photo-upload.component.scss'],
})
export class PhotoUploadComponent implements OnInit {
  @Input() photos: Image[];
  @Input() vehicleId: number;
  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  currentMain: Image;

  constructor(private toastr: ToastrService, private imagesService: ImagesService) {}

  ngOnInit(): void {
    this.initializeUploader();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'images/' + this.vehicleId,
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 5 * 1024 * 1024, // 5mb
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: Image = JSON.parse(response);
        const photo = {
          id: res.id,
          fileName: res.fileName,
          isMain: res.isMain,
        };
        this.photos.push(photo);
      }
    };
  }

  setMainPhoto(photo: Image) {
    this.imagesService.setMain(photo.id, this.vehicleId).subscribe(
      (response) => {
        this.toastr.success('Main photo set!');
        this.currentMain = this.photos.filter((p) => p.isMain === true)[0];
        this.currentMain.isMain = false;
        photo.isMain = true;
      },
      (error) => {
        this.toastr.error('Error while setting the main photo!' + error);
      }
    );
  }

  deletePhoto(id: number) {
    this.imagesService.deleteImage(id).subscribe(
      (response) => {
        this.toastr.success('Successfully deleted image!');
        this.photos.splice(
          this.photos.findIndex((p) => p.id === id),
          1
        );
      },
      (error) => {
        this.toastr.error('Error deleting photo' + error);
      }
    );
  }
}
