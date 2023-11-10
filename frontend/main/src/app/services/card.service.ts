import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CardService {

  private AUTH_API = "https://localhost:44395/api/Cards/";
  constructor(private http: HttpClient) { }

  deleteCardById(cardId: number): Observable<any> {
    return this.http.delete(this.AUTH_API + "deleteCard/"+cardId);
  }

  createCard(request: any): Observable<any> {
    return this.http.post(this.AUTH_API + "createCard", request);
  }
}
