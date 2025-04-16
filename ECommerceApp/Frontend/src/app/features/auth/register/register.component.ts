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
    RouterLink
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  signupForm!: FormGroup;
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
  }
}
