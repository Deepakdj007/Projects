// src/app/core/guards/auth.guard.ts
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const AuthGuard: CanActivateFn = () => {
  const router = inject(Router);
  const auth = inject(AuthService);

  if (!auth.isLoggedIn()) {
    router.navigate(['/auth/login']);
    return false;
  }

  return true;
};
