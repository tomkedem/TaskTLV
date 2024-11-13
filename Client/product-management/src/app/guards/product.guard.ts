import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { ProductService } from '../services/product.service';

@Injectable({
  providedIn: 'root'
})
export class ProductGuard implements CanActivate {
  constructor(private productService: ProductService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): Promise<boolean> {
    const productId = route.paramMap.get('id');
    return this.productService.getProductById(Number(productId)).toPromise().then(product => {
      if (product) {
        return true;
      } else {
        this.router.navigate(['/product-list']);
        return false;
      }
    }).catch(() => {
      this.router.navigate(['/product-list']);
      return false;
    });
  }
}
