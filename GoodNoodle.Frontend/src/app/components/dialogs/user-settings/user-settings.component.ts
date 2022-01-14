import { Component, Input } from '@angular/core';
import { NbDialogRef, NbDialogService } from '@nebular/theme';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/services/api/user.service';
import { User } from 'src/app/types/entities/user/user';
import { ChangePasswordComponent } from '../change-password/change-password.component';
import { DeleteDialogComponent } from '../delete-dialog/delete-dialog.component';

@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
  styleUrls: ['./user-settings.component.scss']
})
export class UserSettingsComponent {
  @Input() user: User | null = null;
  @Input() email = '';
  @Input() fullName = '';

  constructor(
    private dialog: NbDialogRef<UserSettingsComponent>,
    private userService: UserService,
    private toaster: ToastrService,
    private dialogService: NbDialogService
  ) {}

  close(accepted: boolean): void {
    this.dialog.close(accepted);
  }

  async updateUser(): Promise<void> {
    if (!this.user) {
      return;
    }
    this.user.email = this.email;
    this.user.fullName = this.fullName;

    await this.userService
      .editUser(this.user)
      .toPromise()
      .then((response) => {
        if (response?.success) {
          this.toaster.success('Updated successfully');
          this.close(true);
        }
      })
      .catch((error) => {
        this.toaster.error(error.error.errors[0].value);
      });
  }

  openChangePasswordDialog(): void {
    this.dialogService.open(ChangePasswordComponent);
  }

  openDeleteDialog(): void {
    this.dialogService
      .open(DeleteDialogComponent, {
        context: {
          message: `Are you sure you want to delete your account?`
        }
      })
      .onClose.subscribe((accepted: boolean) => {
        if (accepted) {
          this.deleteUser();
        }
      });
  }

  async deleteUser(): Promise<void> {
    if (!this.user) {
      return;
    }
    await this.userService
      .deleteUser(this.user.id)
      .toPromise()
      .then((response) => {
        if (response?.success) {
          this.toaster.success('Updated deleted account');
          localStorage.removeItem('auth_app_token');
          window.location.pathname = '/';
        }
      })
      .catch((error) => {
        this.toaster.error(error.error.errors[0].value);
      });
  }
}
