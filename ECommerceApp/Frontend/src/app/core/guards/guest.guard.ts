// src/app/core/guards/guest.guard.ts
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const GuestGuard: CanActivateFn = () => {
  const router = inject(Router);
  const auth = inject(AuthService);

  if (auth.isLoggedIn()) {
    router.navigate(['/']);
    return false;
  }

  return true;
};
