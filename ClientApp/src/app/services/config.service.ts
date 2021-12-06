import { Injectable } from '@angular/core';
import { HttpClient } from 'node_modules/@angular/common/http';
import { Configuration } from 'src/app/models/Config';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
private config: Configuration;
  constructor(private httpClient: HttpClient) { }

  getConfiguration(): Configuration {
    return this.config;
  }

  load(url:string){
    return new Promise((resolve) => {
      this.httpClient.get<Configuration>(url).subscribe(config => {
        this.config = config;
        debugger;
        resolve();
      });
    });
  }
}
