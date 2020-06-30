import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { CommentToMake } from '../_models/commentToMake';
import * as signalR from '@aspnet/signalr';
import { Observable, BehaviorSubject } from 'rxjs';
import { Comment } from '../_models/comment';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class CommentService {
  baseUrl = `${environment.apiUrl}comments/`;
  hubUrl = `${environment.hubUrl}comments`;
  private comments = new BehaviorSubject<boolean>(false);
  commentData = this.comments.asObservable();

  private hubConnection: signalR.HubConnection;

  constructor(private http: HttpClient, private toastr: ToastrService) {}

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder().withUrl(this.hubUrl).build();

    this.hubConnection
      .start()
      .then()
      .catch((err) => this.toastr.error('Error while starting connection: ' + err));
  };

  public addTransferCommentsListener = () => {
    this.hubConnection.on('transfercomments', (data) => {
      this.comments.next(true);
    });
  };

  makeNewComment(commentToMake: CommentToMake) {
    return this.http.post(this.baseUrl, commentToMake);
  }

  getCommentsForVehicle(vehicleId: number): Observable<Comment[]> {
    return this.http.get<Comment[]>(this.baseUrl + 'vehicle/' + vehicleId);
  }
}
