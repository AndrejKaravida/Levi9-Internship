import { Component, OnInit, Inject } from '@angular/core';
import { CommentService } from 'src/app/_services/comments.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { CommentToMake } from 'src/app/_models/commentToMake';

@Component({
  selector: 'app-new-comment-dialog',
  templateUrl: './new-comment-dialog.component.html',
  styleUrls: ['./new-comment-dialog.component.css'],
})
export class NewCommentDialogComponent implements OnInit {
  formGroup: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<NewCommentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private commentService: CommentService,
    private formBuilder: FormBuilder,
    private toastr: ToastrService
  ) {}

  ngOnInit() {
    this.formGroup = this.formBuilder.group({
      text: ['', [Validators.minLength(10), Validators.maxLength(500), Validators.required]],
    });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  makeComment() {
    const commentToMake: CommentToMake = {
      text: this.formGroup.get('text').value,
      vehicleId: this.data.vehicleId,
    };

    this.commentService.makeNewComment(commentToMake).subscribe(
      (response) => {
        this.toastr.success('Comment saved!');
      },
      (error) => {
        this.toastr.error('Error while saving comment' + error);
      }
    );

    this.dialogRef.close();
  }
}
