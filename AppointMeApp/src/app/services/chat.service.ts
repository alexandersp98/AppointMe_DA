import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Chat } from '../classes/chat.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  url: string = environment.apiBaseUrl+'GetAllChats';
  list: Chat[] = [];

  constructor(private http: HttpClient) { }

  getAllChats(){
    this.http.get(this.url)
    .subscribe({
      next: res =>
        {
          this.list = res as Chat[];
        },
      error: err => {console.log(err)}
    })
  }
}
