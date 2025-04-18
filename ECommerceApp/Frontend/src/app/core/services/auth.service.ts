import { Injectable, signal, computed } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '../../../environemnts/environment';
import { CreateUserDto } from '../models/create-user.dto';
import { HttpClient } from '@angular/common/http';
import { LoginUserDto } from '../models/login-user.dto';
import { jwtDecode } from 'jwt-decode';
import { SocialAuthService, SocialUser } from '@abacritt/angularx-social-login';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly tokenKey = 'authToken';
  private readonly userKey = 'userEmail';

  private _token = signal<string | null>(this.getStoredToken());

  token = computed(() => this._token());
  socialUser: SocialUser | null = null;
  isLoggedIn = computed(() => !!this._token());

  constructor(
    private router: Router,
    private http: HttpClient,
    private socialAuthService: SocialAuthService
  ) {}

  ngOnInit(): void {
    this.socialAuthService.authState.subscribe((user) => {
      this.socialUser = user;
      this.isLoggedIn = computed(() =>user != null);
    });
  }
  private getStoredToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  // Check if token is expired
  isTokenExpired(): boolean {
    const token = this.getStoredToken();
    if (!token) {
      return true; // Token does not exist
    }

    const decodedToken: any = jwtDecode(token);
    const expirationDate = decodedToken.exp * 1000; // Convert from seconds to milliseconds
    const currentTime = new Date().getTime();

    return currentTime >= expirationDate; // Return true if the token has expired
  }

  login(token: string, email?: string) {
    localStorage.setItem(this.tokenKey, token);
    if (email) localStorage.setItem(this.userKey, email);
    this._token.set(token);
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.userKey);
    this._token.set(null);
    if(this.socialUser) {
      this.socialAuthService.signOut();
    }
    this.router.navigate(['/auth/login']);
  }

  getUserEmail(): string | null {
    return localStorage.getItem(this.userKey);
  }

  registerUser(user: CreateUserDto) {
    const url = `${environment.apiUrl}/users/register`;
    return this.http.post(url, user);
  }
  loginUser(user: LoginUserDto) {
    const url = `${environment.apiUrl}/users/login`;
    return this.http.post(url, user);
  }
  // Google Login method
  googleLogin(token: string) {
    const url = `${environment.apiUrl}/users/google-login`; // Backend endpoint for Google login
    return this.http.post(url, { tokenId: token });
  }
  // OTP Verification method
  verifyOtp(email: string, otp: string) {
    return this.http.post(`${environment.apiUrl}/users/verify-otp`, {
      email,
      otp,
    });
  }
}
