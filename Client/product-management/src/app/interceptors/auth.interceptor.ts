import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { log } from 'console';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Retrieve the token from localStorage
    
   
    const token = localStorage.getItem('jwt');
   
    // Clone the request and add the authorization header if token exists
    let clonedReq = req;
    if (token) {
    
      clonedReq = req.clone({
        headers: req.headers.set('Authorization', `Bearer ${token}`)
      });
    }

    // Handle the request and check for 401 error
    return next.handle(clonedReq).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          // If 401 error, clear the token and navigate to login
          localStorage.removeItem('jwt');
          localStorage.removeItem('role'); // Clear additional auth data if stored
          this.router.navigate(['/login']); // Redirect to login page
        }
        return throwError(error); // Return the error so it can be handled if needed
      })
    );
  }
}
