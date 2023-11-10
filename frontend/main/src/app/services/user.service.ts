import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private AUTH_API = "https://localhost:44395/api/User/";
  constructor(private http:HttpClient) { }

  getUserById(id:number):Observable<any>{
    return this.http.get(this.AUTH_API+"getUserById/"+id)
  }
}
