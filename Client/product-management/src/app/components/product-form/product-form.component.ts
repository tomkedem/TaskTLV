import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE, NativeDateAdapter } from '@angular/material/core';
import { MatNativeDateModule } from '@angular/material/core';
import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product.model';

export const CUSTOM_DATE_FORMATS = {
  parse: {
    dateInput: 'DD/MM/YYYY',
  },
  display: {
    dateInput: 'DD/MM/YYYY',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatDatepickerModule,
    MatInputModule,
    MatCheckboxModule,
    MatNativeDateModule // Add MatNativeDateModule here
  ],
  templateUrl: './product-form.component.html',
  styleUrls: ['./product-form.component.css'],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter }, // Use NativeDateAdapter instead of MatNativeDateAdapter
    { provide: MAT_DATE_FORMATS, useValue: CUSTOM_DATE_FORMATS },
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' } // Set locale for DD/MM/YYYY format
  ]
})
export class ProductFormComponent implements OnInit {
  productForm: FormGroup;
  productId: number | null = null;
  isEditMode = false;

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private route: ActivatedRoute,
    private router: Router,
    private dateAdapter: DateAdapter<Date>
  ) {
    this.dateAdapter.setLocale('en-GB'); // Set date format to DD/MM/YYYY
    this.productForm = this.fb.group({
      productId: [],
      productName: ['', Validators.required],
      inStock: [Validators.required],
      arrivalDate: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.productId = Number(this.route.snapshot.paramMap.get('id'));
    this.isEditMode = !!this.productId;

    if (this.isEditMode) {
      this.loadProductDetails();
    }
  }

  loadProductDetails() {
    this.productService.getProductById(this.productId!).subscribe(
      product => this.productForm.patchValue(product),
      error => console.error('Failed to load product', error)
    );
  }

  onSubmit(): void {
    if (this.productForm.invalid) return;

    const productData: Product = this.productForm.value;    
   
    productData.arrivalDate.setDate(productData.arrivalDate.getDate() + 1);
   
    if (this.isEditMode) {
      this.productService.updateProduct(productData).subscribe(
        () => {
          alert('Product updated successfully');
          this.router.navigate(['/']);
        },
        error => console.error('Failed to update product', error)
      );
    } else {
      this.productService.addProduct(productData).subscribe(
        () => {
          alert('Product added successfully');
          this.router.navigate(['/']);
        },
        error => console.error('Failed to add product', error)
      );
    }
  }

 
}
