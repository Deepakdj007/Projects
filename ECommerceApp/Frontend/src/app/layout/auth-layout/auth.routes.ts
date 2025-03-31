// src/app/layout/auth-layout/auth.routes.ts
import { Routes } from '@angular/router';
import { AuthLayoutComponent } from './auth-layout.component';
import { GuestGuard } from '../../core/guards/guest.guard';

export const AUTH_ROUTES: Routes = [
  {
    path: '',
    component: AuthLayoutComponent,
    children: [
      {
        path: 'login',
        canActivate: [GuestGuard],
        loadComponent: () =>
          import('../../features/auth/login/login.component').then(m => m.LoginComponent)
      },
      {
        path: 'register',
        canActivate: [GuestGuard],
        loadComponent: () =>
          import('../../features/auth/register/register.component').then(m => m.RegisterComponent)
      }
    ]
  }
];
