// src/app/layout/main-layout/main.routes.ts
import { Routes } from '@angular/router';
import { MainLayoutComponent } from './main-layout.component';
import { AuthGuard } from '../../core/guards/auth.guard';

export const MAIN_ROUTES: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      {
        path: '',
        canActivate: [AuthGuard],
        loadComponent: () =>
          import('../../features/home/home.component').then(m => m.HomeComponent)
      },
    ]
  }
];
