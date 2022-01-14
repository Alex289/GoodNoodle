import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { NbAuthService, NbAuthToken } from '@nebular/auth';
import { CLAIMTYPES_ROLE } from 'src/app/lib/claimtypes';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  isAuthorized = false;

  constructor(private authService: NbAuthService, private router: Router) {}

  canActivate(): boolean {
    this.authService.onTokenChange().subscribe((token: NbAuthToken) => {
      if (token.isValid()) {
        const decodedToken = token.getPayload();
        const role = decodedToken[CLAIMTYPES_ROLE];

        this.isAuthorized = role === 'Admin' ? true : false;
      }
    });

    if (!this.isAuthorized) {
      this.router.navigate(['/login']);
    }

    return this.isAuthorized;
  }
}
