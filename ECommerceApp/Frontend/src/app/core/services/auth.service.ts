import { Injectable, signal, computed } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '../../../environemnts/environment';
import { CreateUserDto } from '../models/create-user.dto';
import { HttpClient } from '@angular/common/http';
import { LoginUserDto } from '../models/login-user.dto';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly tokenKey = 'authToken';
  private readonly userKey = 'userEmail';

  private _token = signal<string | null>(this.getStoredToken());

  token = computed(() => this._token());

  isLoggedIn = computed(() => !!this._token());

  constructor(private router: Router, private http: HttpClient) {}

  private getStoredToken(): string | null {
    return localStorage.getItem(this.tokenKey);
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
}
