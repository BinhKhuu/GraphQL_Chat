import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { GraphqlService } from './services/graphql.service';
import { ChatMessage } from '../app/model/ChatMessage';
@Component({
  selector: 'app-root',
  imports: [
    CommonModule,
    FormsModule,
    MatToolbarModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
  ],
  providers:[GraphqlService],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  messages: ChatMessage[] = [];
  newMessage = '';

  constructor(
    private graphqlService: GraphqlService
  ){

  }
  ngOnInit(): void {
    this.graphqlService.subscribeToChat()
    .subscribe({
      next: (result: any) =>{
        console.log('message received', result);
        this.messages.push({
          text: result.data.messageAdded.content,
          user: result.data.messageAdded?.from?.id,
          timestamp: result.data.messageAdded?.sentAt
        });
        //this.messages.push(`${result.data.messageAdded.content} ${result.data.messageAdded?.from?.id} ${result.data.messageAdded?.sentAt}`);
      },
      error: (error) => console.log(error)
    })
  }

  sendMessage() {
    if (this.newMessage.trim()) {
      this.graphqlService.sendMessage(this.newMessage)
    }
  }
}
