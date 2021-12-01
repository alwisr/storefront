import { async, ComponentFixture, TestBed,inject,tick } from '@angular/core/testing';
import { Product } from 'src/app/models/products';
import { ProductService } from 'src/app/services/product.service';
import { RouterTestingModule } from '@angular/router/testing';
import { ProductsComponent } from 'src/app/components/products/products.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('ProductsComponent', () => {
  let component: ProductsComponent;
  let fixture: ComponentFixture<ProductsComponent>;
  let mockData:Product[] = [{ name: 'product name', productId: 'product id',unitPrice: 10, sellingPrice: 120, description: 'description1', maximumQuantity: 1 },
    { name: 'product name11', productId: 'product id11', unitPrice: 20, sellingPrice: 220, description: 'description1', maximumQuantity: 1 }];

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ProductsComponent],
      providers: [ProductService],
      imports: [RouterTestingModule.withRoutes([]), HttpClientTestingModule],
    })
      .compileComponents();
    fixture = TestBed.createComponent(ProductsComponent);
    component = fixture.componentInstance;
    component.products  = mockData;
    fixture.detectChanges();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should have a table on page', () => {
    fixture.whenStable().then(() => {
     let tabControl = fixture.nativeElement.querySelectorAll('table');
     // debugger;
      expect(tabControl.length).toEqual(1);
    })

  })
  it('should have first header as Name', () => {
    fixture.whenStable().then(() => {
      let headerCell = fixture.nativeElement.querySelectorAll('th');
     expect(headerCell[0].innerHTML).toBe('Name')
    })
  })
});
