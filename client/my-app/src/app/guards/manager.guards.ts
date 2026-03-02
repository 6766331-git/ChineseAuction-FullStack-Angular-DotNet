import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth';

export const managerGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);
  if (typeof window === 'undefined') return true;

  const role = auth.getUserRole();

  if (role === 1) {
    return true;
  }
  return router.parseUrl('/login');
};
