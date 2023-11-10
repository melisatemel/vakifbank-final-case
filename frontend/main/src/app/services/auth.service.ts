import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { StorageService } from './storage.service';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  private AUTH_API = "https://localhost:44395/api/";
  constructor(private http: HttpClient, private storage: StorageService) { }
  private httpOptions = {
    headers: new HttpHeaders({'Content-Type': 'application/json'})
  }


  login(email: string, password: string): Observable<any> {
    return this.http.post(this.AUTH_API + "TokenContoller", {
      email: email,
      password: password
    }, {
      ...this.httpOptions,
      withCredentials: true
    });
  }

  getRole(): boolean{
    const token = this.storage.getUser().token;
    let jwtData = token.split('.')[1]
    let decodedJwtJsonData = window.atob(jwtData)
    let decodedJwtData = JSON.parse(decodedJwtJsonData)

    const isAdmin = decodedJwtData.Role;
    return isAdmin == 'admin' ? true : false;
  }

  logOut(){
    this.storage.clean();
  }

  isLoggin(): boolean{
    if(this.storage.getUser()){
      return true;
    }
    else{ return false}
  }

  register(email: string, password:string, firstName: string, lastName:string): Observable<any>{
    return this.http.post(this.AUTH_API + "User/createUser",{
      "firstName":firstName,
      "lastName":lastName,
      "email":email,
      "password":password,
      "role" : "dealer"
    },{
      ...this.httpOptions,
      withCredentials : true
    });

  }
}
