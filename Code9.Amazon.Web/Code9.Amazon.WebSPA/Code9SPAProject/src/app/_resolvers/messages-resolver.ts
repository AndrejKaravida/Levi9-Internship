import { ToastrService } from 'ngx-toastr';
import { MessagesService } from './../_services/messages.service';
import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Message } from '../_models/message';

@Injectable()
export class MessagesResolver implements Resolve<Message[]> {
  pageNumber = 1;
  pageSize = 5;
  messageContainer = 'Unread';

  constructor(
    private messagesService: MessagesService,
    private router: Router,
    private toastr: ToastrService,
    private authService: AuthService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Message[]> {
    return this.messagesService
      .getMessages(
        this.authService.getNameId(),
        this.pageNumber,
        this.pageSize,
        this.messageContainer
      )
      .pipe(
        catchError(() => {
          this.toastr.error('Problem retrieving data!');
          this.router.navigate(['/home']);
          return of(null);
        })
      );
  }
}
