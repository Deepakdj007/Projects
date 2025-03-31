import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { LoginUserDto } from '../../../core/models/login-user.dto';
@Component({
  selector: 'app-login',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    RouterLink
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginForm!: FormGroup;
  hide = true;
  constructor(private fb: FormBuilder, private authService:AuthService, private router:Router) {
    this.loginForm = this.fb.group(
      {
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required]],
      },
    );
  }


  onSubmit() {
    if (this.loginForm.valid) {
      let request:LoginUserDto = {
        Email: this.loginForm.get('email')?.value,
        Password: this.loginForm.get('password')?.value
      }
      console.log('Form Submitted:', request);
      this.authService.loginUser(request).subscribe({
        next: (res:any) => {
          console.log('Login successful:', res);
          this.authService.login(res.token, res.user.email)
          this.router.navigate(['/']);
        },
        error: (err) => {
          console.error('Registration failed:', err);
          // Show error to user
        }
      });

    }
  }
}
