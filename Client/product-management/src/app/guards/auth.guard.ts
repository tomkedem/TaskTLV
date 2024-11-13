import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

export const AuthGuard: CanActivateFn = (route, state) => {
  // Inject Router to enable navigation
  const router = inject(Router);

  // Check if the token exists in localStorage
  const token = localStorage.getItem('jwt');
  
  if (token) {
    // If the token exists, allow access
    return true;
  } else {
    // If no token, redirect to login page
    router.navigate(['/login']);
    return false;
  }
};
