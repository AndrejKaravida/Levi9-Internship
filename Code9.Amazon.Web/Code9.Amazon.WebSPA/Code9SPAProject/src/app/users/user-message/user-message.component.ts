import { MessagesService } from './../../_services/messages.service';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { Message } from 'src/app/_models/message';
import { UsersService } from 'src/app/_services/users.service';
import { AuthService } from 'src/app/_services/auth.service';
import { tap } from 'rxjs/operators';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-user-message',
  templateUrl: './user-message.component.html',
  styleUrls: ['./user-message.component.scss'],
})
export class UserMessageComponent implements OnInit {
  recipientId: number;
  messages: Message[];
  newMessage: any = {};

  constructor(
    private usersService: UsersService,
    private authService: AuthService,
    private toastr: ToastrService,
    private route: ActivatedRoute
  ) {
    route.params.subscribe((p) => {
      this.recipientId = +p.id || 0;
    });
  }

  ngOnInit(): void {
    this.usersService.startConnection();
    this.usersService.addTransferMessagesListener(this.authService.getNameId());
    this.loadMessages();

    this.usersService.dataReceived.subscribe((data: Message) => {
      data.isRead = true;
      this.messages.push(data);
    });
  }

  loadMessages() {
    const currentUserId = +this.authService.getNameId();
    this.usersService
      .getMessageThread(this.authService.getNameId(), this.recipientId)
      .pipe(
        tap((messages) => {
          messages.forEach((message) => {
            if (message.isRead === false && message.recipientId === currentUserId) {
              this.usersService.markAsRead(currentUserId, message.id);
            }
          });
        })
      )
      .subscribe(
        (messages) => {
          this.messages = messages;
        },
        (error) => {
          this.toastr.error(error);
        }
      );
  }

  sendMessage() {
    this.newMessage.recipientId = this.recipientId;
    this.usersService.sendMessage(this.authService.getNameId(), this.newMessage).subscribe(
      (message: Message) => {
        this.messages.push(message);
        this.newMessage.content = '';
      },
      (error) => {
        this.toastr.error(error);
      }
    );
  }
}
