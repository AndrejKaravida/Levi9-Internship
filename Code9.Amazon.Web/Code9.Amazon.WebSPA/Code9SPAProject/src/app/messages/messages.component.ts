import { MessagesService } from './../_services/messages.service';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { Pagination } from '../_models/pagination';
import { Message } from '../_models/message';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss'],
})
export class MessagesComponent implements OnInit {
  messages: Message[];
  pagination: Pagination;
  messageContainer = 'Unread';

  constructor(
    private messagesService: MessagesService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.messages = data.messages.result;
      this.pagination = data.messages.pagination;
    });
  }

  loadUnreadMessages() {
    this.messageContainer = 'Unread';
    this.loadMessages();
  }

  loadInboxMessages() {
    this.messageContainer = 'Inbox';
    this.loadMessages();
  }

  loadOutboxMessages() {
    this.messageContainer = 'Outbox';
    this.loadMessages();
  }

  loadMessages() {
    this.messagesService
      .getMessages(
        this.authService.getNameId(),
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        this.messageContainer
      )
      .subscribe(
        (res: any) => {
          this.messages = res.result;
          this.pagination = res.pagination;
        },
        (error) => {
          this.toastr.error(error);
        }
      );
  }

  deleteMessage(id: any) {
    this.messagesService.deleteMessage(id, this.authService.getNameId()).subscribe(
      () => {
        this.messages.splice(
          this.messages.findIndex((m) => m.id === id),
          1
        );
        this.toastr.success('Message has been deleted');
      },
      (error) => {
        this.toastr.error('Failed to delete the message');
      }
    );
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadMessages();
  }
}
