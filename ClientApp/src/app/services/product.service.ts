import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ConfigService } from './config.service';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  productBaseUrl:string;
  private headers: HttpHeaders;
  public products: any;

  constructor(private http: HttpClient, private configService: ConfigService) {
    this.productBaseUrl = configService.getConfiguration().webApiBaseUrl;
    this.headers = new HttpHeaders({ 'Content-Type': 'application/json; charset=utf-8' });
  }

  public async get() {
    return await this.http.get(this.productBaseUrl + 'product/getproducts', { observe: 'response' });
  }
}
