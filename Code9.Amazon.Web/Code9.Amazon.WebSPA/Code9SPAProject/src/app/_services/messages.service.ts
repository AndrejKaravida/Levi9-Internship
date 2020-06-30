import { ToastrService } from 'ngx-toastr';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PaginatedResult } from '../_models/pagination';
import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { map } from 'rxjs/operators';
import * as signalR from '@aspnet/signalr';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MessagesService {
  baseUrl = `${environment.apiUrl}`;
  hubUrl = `${environment.hubUrl}chat`;
  private hubConnection: signalR.HubConnection;
  private messages = new BehaviorSubject<boolean>(false);

  constructor(private http: HttpClient, private toastr: ToastrService) {}

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder().withUrl(this.hubUrl).build();

    this.hubConnection
      .start()
      .then()
      .catch((err) => this.toastr.error('Error while starting connection: ' + err));
  };

  public addTransferMessagesListener = () => {
    this.hubConnection.on('RecieveMessage', (data) => {
      this.messages.next(true);
    });
  };

  getMessages(id: number, page?, itemsPerPage?, messageContainer?) {
    const paginatedResult: PaginatedResult<Message[]> = new PaginatedResult<Message[]>();

    let params = new HttpParams();

    params = params.append('MessageContainer', messageContainer);

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page).append('pageSize', itemsPerPage);
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

  deleteMessage(id: number, userId: number) {
    return this.http.post(this.baseUrl + 'users/' + userId + '/messages/' + id, {});
  }
}
