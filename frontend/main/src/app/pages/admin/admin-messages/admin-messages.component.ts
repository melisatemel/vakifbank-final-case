import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'src/app/services/message.service';
import { StorageService } from 'src/app/services/storage.service';

@Component({
  selector: 'app-admin-messages',
  templateUrl: './admin-messages.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class AppAdminMessagesComponent implements OnInit{

  newMessage: string;
  messages: any = [];
  selectedUserId: number
  selectedMessages: any;
  index: number;

  constructor(private messageService: MessageService, private router: Router){}


  ngOnInit(): void {
    this.getMessages();
  }

  sendMessage(){
    const request = {
      "chatId": this.selectedUserId,
      "content": this.newMessage,
      "isAdmin": true,
    }
    this.messageService.createMessage(request).subscribe((data:any)=>{
      this.getMessages(this.index);
      this.newMessage = ''
    })
  }

  getMessages(index?: number){
    this.messageService.getAdminMessages().subscribe((data: any)=>{
      this.messages = data.response;
      this.selectedUserId = this.messages[index ? index : 0].chatId;
      this.selectedMessages = this.messages[index ? index : 0].messages;
    })
  }

  selectUser(selectedMessage: any, index: number) {
    this.selectedMessages = selectedMessage.messages;
    this.selectedUserId = selectedMessage.chatId;
    this.index = index;
  }

  backToDashboard(){
    this.router.navigate(['admin/dashboard'])
  }
}
