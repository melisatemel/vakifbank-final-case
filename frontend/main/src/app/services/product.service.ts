import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private AUTH_API = "https://localhost:44395/api/Product/";

  constructor(private http: HttpClient) { }

  getAllProduct(id: number): Observable<any> {
    return this.http.get(this.AUTH_API + "gelAllProducts/"+ id);
  }

  getProductById(id: number, userId: number): Observable<any> {
    return this.http.get(this.AUTH_API + "getProductById/"+id+ "/"+ userId);
  }

  addProduct(request: any): Observable<any>{
    return this.http.post(this.AUTH_API + "createProduct", request);
  }

  updateProduct(request: any): Observable<any>{
    return this.http.put(this.AUTH_API+ "updateProduct/"+request.productId, request)
  }

  deleteProduct(id: number){
    return this.http.delete(this.AUTH_API + "deleteProduct/" + id);
  }
}
