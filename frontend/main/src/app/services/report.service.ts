import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  private AUTH_API = "https://localhost:44395/api/OrderReport/";

  constructor(private http: HttpClient) { }

  getOrderReports(id: number): Observable<any> {
    return this.http.get(this.AUTH_API + "getOrderReport/"+ id);
  }
}

