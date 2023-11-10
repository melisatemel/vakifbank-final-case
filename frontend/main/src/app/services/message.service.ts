import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  private AUTH_API = "https://localhost:44395/api/Messages/";
  constructor(private http: HttpClient) { }

  getMessageForDealer(id: number): Observable<any> {
    return this.http.get(this.AUTH_API + "getMessageById/"+ id);
  }

  createMessage(request: any): Observable<any> {
    return this.http.post(this.AUTH_API + "createMessage",request);
  }

  getAdminMessages(): Observable<any> {
    return this.http.get(this.AUTH_API + "getAdminMessages");
  }
}
