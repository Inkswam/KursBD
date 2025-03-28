import { inject } from '@angular/core';
import {Router} from '@angular/router';
import {AuthenticationService} from './authentication.service';

export const authenticationGuard = async () => {
  const authService = inject(AuthenticationService);
  const router = inject(Router);

  const isAuthenticated = await authService.checkAuthenticated();
  console.log("Authentication check: ", isAuthenticated);

  if(isAuthenticated) {
    return true;
  }
  const returnUrl = router.routerState.snapshot.url;
  return router.createUrlTree(['/login'], {queryParams: {returnUrl: returnUrl}});
};
