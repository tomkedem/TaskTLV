import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username = '';
  password = '';
  errorMessage = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  login(): void {
    this.authService.login(this.username, this.password).subscribe(
      (response) => {
        // Save JWT and role to local storage
        localStorage.setItem('token', response.token);
        localStorage.setItem('role', response.role);
        // Navigate to home after successful login
        this.router.navigate(['/product-list']);
      },
      (error) => {
        this.errorMessage = 'Invalid username or password';
        console.error('Login failed', error);
      }
    );
  }
}
