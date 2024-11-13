import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AuthService } from './services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  constructor(public authService: AuthService, private router: Router) {}

  isLoginPage(): boolean {
    return this.router.url === '/login';
  }
  isLoggedIn(): boolean {
    // Check if the user is logged in by verifying if there’s a JWT token
    return !!localStorage.getItem('jwt');
  }

  logout(): void {
    this.authService.logout();
  }
}
