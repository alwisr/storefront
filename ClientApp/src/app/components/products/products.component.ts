import { Component, OnInit } from '@angular/core';
import { ProductService } from 'src/app/services/product.service';
import { Product } from 'src/app/models/products';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {

  public products: Product[];

  constructor(
    private productService: ProductService
  ) { }

  async ngOnInit() {
    var data = (await this.productService.get()).subscribe(response => {
      
      this.products = <Product[]>response.body;

      console.log(this.products);

    })
  }

}
