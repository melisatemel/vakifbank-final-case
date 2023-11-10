import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { HeaderComponent } from '../layouts/full/header/header.component';
import { StorageService } from './storage.service';

@Injectable({
  providedIn: 'root'
})
export class ShoppingCartService {

  private AUTH_API = "https://localhost:44395/api/ShoppingCart/";
  public numberOfItemsSubject: BehaviorSubject<number> = new BehaviorSubject<number>(0);
  numberOfItems$: Observable<number> = this.numberOfItemsSubject.asObservable();
  constructor(private http: HttpClient, private storage: StorageService) { }
  private httpOptions = {
    headers: new HttpHeaders({'Content-Type': 'application/json'})
  }


  addProductToCart(id: number, isMinus= false, isDelete = false): Observable<any> {
    const userId = this.storage.getUser().userId;
    var date = new Date();
    var createdAt = date.toISOString();
    const isCompleted = false;
    const request = {
      userId,
      createdAt,
      isCompleted,
      isMinus,
      isDelete,
      "productIds": [
        id
      ]
    }
    return this.http.post(this.AUTH_API + "createShoppingCart", request, {
      ...this.httpOptions,
      withCredentials: true
    }).pipe(
      tap(() => this.updateNumberOfItems()) // HTTP isteği başarıyla tamamlandığında numberOfItems'ı güncelle
    );
  }

  public updateNumberOfItems(): void {
    this.getUserShoppingCart(this.storage.getUser().userId).subscribe((data: any) => {
      let numberOfItems = 0;
      data.response?.productQuantities?.map((item: any) => {
        numberOfItems += item.quantity;
      });
      this.numberOfItemsSubject.next(numberOfItems);
    });
  }

  getUserShoppingCart(id: number): Observable<any>{
    return this.http.get(this.AUTH_API + "getShoppingCartById/" + id, {
      ...this.httpOptions,
      withCredentials: true
    });
  }


  getComplatedShoppingCart(id: number): Observable<any>{
    return this.http.get(this.AUTH_API + "getComplatedShoppingCartById/" + id, {
      ...this.httpOptions,
      withCredentials: true
    });
  }

  updateShoppingCart(shoppingCartId: number, selectedCartId: number, selectedAddressId: number, waitForPayment: boolean, openAccount: boolean): Observable<any>{
    return this.http.put(this.AUTH_API + "updateShoppingCartById/"+ shoppingCartId, {
      "isCompleted": true,
      "selectedAddressId": selectedAddressId,
      "selectedCardId": selectedCartId,
      "waitForPayment": waitForPayment,
      "openAccount": openAccount
    })
  }

 complateShoppingCart(shoppingCartId: number): Observable<any>{
    return this.http.put(this.AUTH_API + "updateShoppingCartById/"+ shoppingCartId, {
      "isActive": false,
    })
  }

  getAllShoppingCartItems(): Observable<any>{
    return this.http.get(this.AUTH_API + "getAllCartItems")
  }


  cancelShoppingCart(id: number): Observable<any>{
    return this.http.delete(this.AUTH_API + "deleteShoppingCart/" + id);
  }
}
