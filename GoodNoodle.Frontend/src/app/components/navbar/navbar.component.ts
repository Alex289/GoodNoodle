import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NbAuthService, NbAuthToken } from '@nebular/auth';
import { UserService } from 'src/app/services/api/user.service';
import {
  NbSidebarService,
  NbSearchService,
  NbDialogService
} from '@nebular/theme';
import { Guid } from 'guid-typescript';
import { UserSettingsComponent } from '../dialogs/user-settings/user-settings.component';
import { User } from 'src/app/types/entities/user/user';
import { ToastrService } from 'ngx-toastr';
import { CLAIMTYPES_ID, CLAIMTYPES_STATUS } from 'src/app/lib/claimtypes';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  loggedIn = false;
  user: User | null = null;
  userName = '';
  userId = Guid.createEmpty();
  userStatus = '';

  constructor(
    private userService: UserService,
    private authService: NbAuthService,
    private sidebarService: NbSidebarService,
    private searchService: NbSearchService,
    private dialogService: NbDialogService,
    private router: Router,
    private toaster: ToastrService
  ) {
    this.searchService.onSearchSubmit().subscribe((data) => {
      this.router.navigateByUrl('/search/' + data.term.replace(' ', '-'));
    });

    this.authService.onTokenChange().subscribe((token: NbAuthToken) => {
      if (token.isValid()) {
        const user = token.getPayload();
        this.userId = user[CLAIMTYPES_ID];

        const claimStatus = user[CLAIMTYPES_STATUS];

        this.userStatus =
          claimStatus != 'Accepted' ? `Status: ${claimStatus}` : '';

        this.getUser();
      }
    });
  }

  ngOnInit(): void {
    this.getUser();
  }

  toggle(): void {
    this.sidebarService.toggle(false, 'left');
  }

  logout(): void {
    localStorage.removeItem('auth_app_token');
    window.location.pathname = '/';
  }

  async getUser(): Promise<void> {
    if (this.userId.toString() == Guid.EMPTY) {
      return;
    }
    await this.userService
      .getUserById(this.userId)
      .toPromise()
      .then((response) => {
        this.userName = response?.data.fullName || '';
        this.loggedIn = true;
        this.user = response?.data || null;
      })
      .catch((error) => {
        this.toaster.error('Failed loading current user');
        return error;
      });
  }

  openUserSettingsDialog(): void {
    this.dialogService
      .open(UserSettingsComponent, {
        context: {
          user: this.user,
          email: this.user?.email,
          fullName: this.user?.fullName
        }
      })
      .onClose.subscribe((accepted: boolean) => {
        this.getUser();
        if (accepted) {
        }
      });
  }
}
