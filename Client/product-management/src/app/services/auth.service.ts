import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { config } from '../../config';
import { catchError, Observable, of, tap } from 'rxjs';



@Injectable({
  providedIn: 'root'
})
export class AuthService {
  
  private apiUrl = `${config.apiUrl}/Auth/login`; // Ensure the correct path to login
  
  constructor(private http: HttpClient, private router: Router) {}

  /**
   * Sends login request to the server and stores JWT and role if successful
   * @param username User's username
   * @param password User's password
   * @returns Observable with the login result
   */
  login(username: string, password: string): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const body = { username, password };

    return this.http.post<{ token: string; role?: string }>(this.apiUrl, body, { headers }).pipe(
      tap((response) => {
        // Save token and decode role if available
        localStorage.setItem('jwt', response.token);
        if (response.role) {
          localStorage.setItem('role', response.role);
        }
        this.router.navigate(['/']);
      }),
      catchError((error) => {
        console.error('Login failed', error);
        return of(false);
      })
    );
  }

  /**
   * Logs out the user by removing JWT and role from local storage and redirects to login page
   */
  logout(): void {
    localStorage.removeItem('jwt');
    localStorage.removeItem('role');
    this.router.navigate(['/login']);
  }

  /**
   * Extracts the user's role from the JWT token if available
   * @returns User's role as a string, or null if not available
   */
  getRole(): string | null {
    return localStorage.getItem('role');
  }
  
}
