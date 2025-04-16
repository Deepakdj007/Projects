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
import {MatProgressBarModule} from '@angular/material/progress-bar';


@Component({
  selector: 'app-login',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    RouterLink,
    MatProgressBarModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginForm!: FormGroup;
  hide:boolean = true;
  showLoader:boolean = false;
  otpForm: FormGroup;
  isOtpRequired:boolean = false;
  otpControls: any[] = Array(6).fill(0);
  constructor(private fb: FormBuilder, private authService:AuthService, private router:Router) {
    this.loginForm = this.fb.group(
      {
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required]],
      },
    );
    this.otpForm = this.fb.group(
      this.otpControls.reduce((acc, _, index) => {
        acc[index] = ['', [Validators.required, Validators.pattern(/[0-9]/)]];
        return acc;
      }, {})
    );
  }


  onSubmit() {
    if (this.loginForm.valid) {
      this.showLoader = true; // Show loader
      let request:LoginUserDto = {
        Email: this.loginForm.get('email')?.value,
        Password: this.loginForm.get('password')?.value
      }
      console.log('Form Submitted:', request);
      this.authService.loginUser(request).subscribe({
        next: (res:any) => {
          if (res.requires2FA) {
            this.isOtpRequired = true; // Show OTP form
          } else {
            // Handle regular login (store JWT token)
            this.authService.login(res.token, res.user.email)
            this.router.navigate(['/']);
          }

        },
        error: (err) => {
          this.showLoader = false;
          console.error('Registration failed:', err);
          // Show error to user
        },
        complete: () => {
          this.showLoader = false;
        }
      });

    }
  }

// Handle OTP submission
onOtpSubmit() {
  if (this.otpForm.valid) {
    this.showLoader
    const otp = this.otpForm.value; // Collect OTP values
    const otpString = Object.values(otp).join(''); // Join the individual digits
    const email = this.loginForm.get('email')?.value;

    this.authService.verifyOtp(email, otpString).subscribe({
      next: (response:any) => {
        this.authService.login(response.token, response.user.email)
        this.router.navigate(['/']);
      },
      error: (err) => {
        this.showLoader = false;
        console.error('OTP verification failed', err);
      },
      complete: () => {
        this.showLoader = false;
      }
    });
  }
}
  // Handle input focus and auto tab between OTP fields
  onOtpInput(event: any, index: number) {
    // Auto-focus the next input after typing
    if (event.target.value.length === 1 && index < 5) {
      const nextInput = document.getElementsByName(`otpControl${index + 1}`)[0] as HTMLElement;
      if (nextInput) {
        nextInput.focus();
      }
    }
  }
    // Handle paste event
    onOtpPaste(event: ClipboardEvent) {
      const pastedOtp = event.clipboardData?.getData('text').trim();

      if (pastedOtp && pastedOtp.length === 6) {
        // Automatically fill in the OTP inputs
        for (let i = 0; i < 6; i++) {
          this.otpForm.controls[i].setValue(pastedOtp[i]);
        }
      }
    }
}
