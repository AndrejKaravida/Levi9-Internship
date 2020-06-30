export class Message {
  id: number;
  senderId: number;
  senderUsername: string;
  senderPhotoUrl: string;
  recipientId: number;
  recipientUsername: string;
  recipientPhotoUrl: string;
  content: string;
  isRead: boolean;
  dateRead: Date;
  messageSent: Date;

  constructor() {
    this.id = 0;
    this.senderId = 0;
    this.senderPhotoUrl = '';
    this.recipientId = 1;
    this.recipientUsername = '';
    this.recipientPhotoUrl = '';
    this.content = '';
    this.isRead = false;
  }
}
