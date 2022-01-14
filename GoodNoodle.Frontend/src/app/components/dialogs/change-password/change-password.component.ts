import { Component } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/services/api/user.service';
import { ChangePassword } from 'src/app/types/entities/user/changePassword';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent {
  oldPassword = '';
  newPassword = '';

  constructor(
    private dialog: NbDialogRef<ChangePasswordComponent>,
    private userService: UserService,
    private toaster: ToastrService
  ) {}

  close(): void {
    this.dialog.close();
  }

  async changePassword(): Promise<void> {
    const body: ChangePassword = {
      oldPassword: this.oldPassword,
      newPassword: this.newPassword
    };

    await this.userService
      .changePassword(body)
      .toPromise()
      .then((response) => {
        if (response?.success) {
          this.toaster.success('Updated successfully');
          this.close();
        }
      })
      .catch((error) => {
        this.toaster.error(error.error.errors[0].value);
      });
  }
}
