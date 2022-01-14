import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { NbAuthService, NbAuthToken } from '@nebular/auth';
import { CLAIMTYPES_STATUS } from 'src/app/lib/claimtypes';

@Injectable({
  providedIn: 'root'
})
export class DeclinedGuard implements CanActivate {
  isDeclined = true;

  constructor(private authService: NbAuthService, private router: Router) {}

  canActivate(): boolean {
    this.authService.onTokenChange().subscribe((token: NbAuthToken) => {
      if (token.isValid()) {
        const decodedToken = token.getPayload();
        const status = decodedToken[CLAIMTYPES_STATUS];

        this.isDeclined = status === 'Declined' ? true : false;
      } else {
        this.isDeclined = false;
      }
    });

    if (this.isDeclined) {
      this.router.navigate(['/declined']);
    }

    return !this.isDeclined;
  }
}
