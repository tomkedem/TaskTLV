import { Component } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product.model';

@Component({
  selector: 'app-add-product',
  standalone: true,
  imports: [],
  templateUrl: './add-product.component.html',
  styleUrl: './add-product.component.css'
})
export class AddProductComponent {
  newProduct: Product = {
    productId: 0,
    productName: '',
    inStock: true,
    arrivalDate: new Date()
  };

  constructor(private productService: ProductService) {}

  addProduct(): void {
    this.productService.addProduct(this.newProduct).subscribe((product) => {
      console.log('Product added:', product);
    });
  }
}
