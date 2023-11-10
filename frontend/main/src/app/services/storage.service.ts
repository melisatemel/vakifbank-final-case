import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StorageService {

  constructor() { }

  public saveUser(user: any): void{
    window.sessionStorage.removeItem('auth');
    window.sessionStorage.setItem('auth', JSON.stringify(user));
  }

  public clean(){
    window.sessionStorage.removeItem('auth');
  }

  public getUser(): any{
    const user = window.sessionStorage.getItem('auth');
    if(user){
      return JSON.parse(user);
    }
  }


  public getToken():string{
    const stringfyUser: any = window.sessionStorage.getItem('auth');
    const user = JSON.parse(stringfyUser)
    return user ? 'bearer ' + user.token : '';
  }
}
