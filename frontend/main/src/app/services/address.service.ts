import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AddressService {
  private AUTH_API = "https://localhost:44395/api/Addresses/";
  constructor(private http: HttpClient) { }

  deleteAddressById(addressId: number): Observable<any> {
    return this.http.delete(this.AUTH_API + "deleteAddress/"+addressId);
  }

  createAddress(request: any): Observable<any> {
    return this.http.post(this.AUTH_API + "createAddress", request);
  }
}
