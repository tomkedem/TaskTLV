import { Routes } from '@angular/router';


import { LoginComponent } from './components/login/login.component';
import { AuthGuard } from './guards/auth.guard';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ProductFormComponent } from './components/product-form/product-form.component';
import { ProductGuard } from './guards/product.guard';

// Define your routes
const routes: Routes = [
    { path: '', component: ProductListComponent, canActivate: [AuthGuard] },
    { path: 'product-list', component: ProductListComponent, canActivate: [AuthGuard] },
 
  // Add other routes here as needed
  
  { path: 'login', component: LoginComponent },
  { path: 'product-list', component: ProductListComponent, canActivate: [AuthGuard]  },
  { path: 'product-form/:id', component: ProductFormComponent, canActivate: [ProductGuard]  }, // Route with ID parameter for update
  { path: 'product-form', component: ProductFormComponent, canActivate: [AuthGuard]  } // Route without ID parameter for add
];

// Export routes so it can be used elsewhere
export { routes };
