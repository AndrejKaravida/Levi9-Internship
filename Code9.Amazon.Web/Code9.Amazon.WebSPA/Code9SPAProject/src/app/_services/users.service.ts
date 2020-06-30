import { AuthService } from './auth.service';
import { Injectable, EventEmitter } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { User } from '../_models/user';
import { map } from 'rxjs/operators';
import { Message } from '../_models/message';
import * as signalR from '@aspnet/signalr';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  baseUrl = `${environment.apiUrl}`;
  hubUrl = `${environment.hubUrl}chat`;

  private hubConnection: signalR.HubConnection;
  //private messages = new BehaviorSubject<Message>(new Message());
  dataReceived = new EventEmitter<Message>();
  //messageData = this.messages.asObservable();

  constructor(
    private http: HttpClient,
    private authService: AuthService,
    private toastr: ToastrService
  ) {}

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder().withUrl(this.hubUrl).build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection Started...'))
      .catch((err) => this.toastr.error('Error while starting connection: ' + err));
  };

  public addTransferMessagesListener(id: number) {
    this.hubConnection.on('RecieveMessage ' + id, (data: Message) => {
      this.markAsRead(id, data.id);
      this.dataReceived.emit(data);
    });
  }

  getUsers(
    page?: number,
    itemsPerPage?: number,
    userParams?: any
  ): Observable<PaginatedResult<User[]>> {
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();

    let params = new HttpParams();

    if (page && itemsPerPage) {
      params = params
        .append('pageNumber', page.toString())
        .append('pageSize', itemsPerPage.toString());
    }

    return this.http
      .get<User[]>(this.baseUrl + 'users', { observe: 'response', params })
      .pipe(
        map((response) => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
  }

  getUser(id): Observable<User> {
    return this.http.get<User>(this.baseUrl + 'users/' + id);
  }

  getMessages(id: number, page?, itemsPerPage?, messageContainer?) {
    const paginatedResult: PaginatedResult<Message[]> = new PaginatedResult<Message[]>();

    let params = new HttpParams();

    params = params.append('MessageContainer', messageContainer);

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    return this.http
      .get<Message[]>(this.baseUrl + 'users/' + id + '/messages', { observe: 'response', params })
      .pipe(
        map((response) => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') !== null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }

          return paginatedResult;
        })
      );
  }

  sendMessage(id: number, message: Message) {
    return this.http.post(this.baseUrl + 'users/' + id + '/messages', message);
  }

  getMessageThread(id: number, recipientId: number) {
    return this.http.get<Message[]>(
      this.baseUrl + 'users/' + id + '/messages/thread/' + recipientId
    );
  }

  markAsRead(userId: number, messageId: number) {
    this.http
      .post(this.baseUrl + 'users/' + userId + '/messages/' + messageId + '/read', {})
      .subscribe();
  }
}
