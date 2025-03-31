// src/app/app.routes.ts
import { Routes } from '@angular/router';

export const appRoutes: Routes = [
  {
    path: '',
    loadChildren: () =>
      import('./layout/main-layout/main.routes').then(m => m.MAIN_ROUTES)
  },
  {
    path: 'auth',
    loadChildren: () =>
      import('./layout/auth-layout/auth.routes').then(m => m.AUTH_ROUTES)
  },
  {
    path: '**',
    redirectTo: ''
  }
];
