import { Component, OnInit, inject } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product.model';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-product-list',
  standalone: true,
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css'],
  imports: [CommonModule, MatTableModule, MatButtonModule],
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  isEditor: boolean = false;
  
  displayedColumns = [
    { key: 'productId', header: 'ID' },
    { key: 'productName', header: 'Name' },
    { key: 'inStock', header: 'In Stock', format: (value: boolean) => (value ? 'Yes' : 'No') },
    { key: 'dateAdded', header: 'Added Date', pipe: 'date' },
    { key: 'arrivalDate', header: 'Arrival Date', pipe: 'date' }
  ];

  columnKeys: string[] = [];

  productService = inject(ProductService);
  authService = inject(AuthService);
  router = inject(Router);

  ngOnInit(): void {
    this.getProducts();
    this.isEditor = this.authService.getRole() === 'Editor';

    this.columnKeys = this.displayedColumns.map(col => col.key);

    if (this.isEditor) {
      this.displayedColumns.push({ key: 'actions', header: 'Actions' });
      this.columnKeys.push('actions');
    }
  }

  getProducts(): void {
    this.productService.getProducts().subscribe(
      (data) => {
        this.products = data;
      },
      (error) => {
        console.error('Error fetching products:', error);
      }
    );
  }

  navigateToAdd(): void {
    this.router.navigate(['/product-form']);
  }

  navigateToUpdate(productId: number): void {
    this.router.navigate(['/product-form', productId]);
  }
}
