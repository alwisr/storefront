import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private headers: HttpHeaders;
  public products: any;

  constructor(private http: HttpClient) {
    this.headers = new HttpHeaders({ 'Content-Type': 'application/json; charset=utf-8' });
  }

  public async get() {
    return await this.http.get(environment.baseApiUrl + 'product/getproducts', { observe: 'response' });
  }
}
