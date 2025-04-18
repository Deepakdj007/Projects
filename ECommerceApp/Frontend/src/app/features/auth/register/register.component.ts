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
import { CreateUserDto } from '../../../core/models/create-user.dto';
import { MatCheckbox } from '@angular/material/checkbox';
import { environment } from '../../../../environemnts/environment';
import {MatProgressBarModule} from '@angular/material/progress-bar';

@Component({
  selector: 'app-register',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatCheckbox,
    RouterLink,
    MatProgressBarModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  signupForm!: FormGroup;
  showLoader:boolean = false;
  hide = true;
  constructor(private fb: FormBuilder, private authService:AuthService, private router:Router) {
    this.signupForm = this.fb.group(
      {
        fullName: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(8)]],
        confirmPassword: ['', Validators.required],
        is2FAEnabled: [false],
      },
      { validator: this.passwordsMatchValidator }
    );
  }
  ngOnInit() {
    // Render Google Sign-In Button
    google.accounts.id.initialize({
      client_id: environment.CLINET_ID,
      callback: (response: any) => {
        this.googleLogin(response.credential); // Get the Google ID token and handle login
      },
    });

    google.accounts.id.renderButton(
      document.getElementById('google-signin-button')!,
      { theme: 'outline', size: 'large' } // Customize button style
    );

    // Also set up automatic login for already authenticated users
    google.accounts.id.prompt();
  }
  // Google Login
  googleLogin(token: string) {
    this.showLoader = true;
      this.authService.googleLogin(token).subscribe({
        next: (res: any) => {
          this.authService.login(res.token, res.user.email);
          this.router.navigate(['/']);
        },
        error: (err) => {
          this.showLoader = false;
          console.error('Google login failed:', err);
        },
        complete:() => {
          this.showLoader = false;
        },
      });
  }

  passwordsMatchValidator(form: FormGroup) {
    const password = form.get('password')?.value;
    const confirm = form.get('confirmPassword')?.value;
    return password === confirm ? null : { mismatch: true };
  }


  onSubmit() {
    if (this.signupForm.valid) {
      let request:CreateUserDto = {
        FullName: this.signupForm.get('fullName')?.value,
        Email: this.signupForm.get('email')?.value,
        Password: this.signupForm.get('password')?.value,
        is2FAEnabled:this.signupForm.get('is2FAEnabled')?.value,
      }
      console.log('Form Submitted:', request);
      this.authService.registerUser(request).subscribe({
        next: (res) => {
          console.log('Registration successful:', res);
          this.router.navigate(['/auth/login']);
        },
        error: (err) => {
          console.error('Registration failed:', err);
          // Show error to user
        }
      });

    }
    else{
      this.signupForm.markAllAsTouched(); // Mark all fields as touched to show validation errors
    }
  }
}
