import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/api/user.service';
import { GroupService } from 'src/app/services/api/group.service';
import { User } from 'src/app/types/entities/user/user';
import { Group } from 'src/app/types/entities/group';
import { ToastrService } from 'ngx-toastr';
import { NbDialogService } from '@nebular/theme';
import { AdminDialogComponent } from 'src/app/components/dialogs/admin-dialog/admin-dialog.component';
import { DeleteDialogComponent } from 'src/app/components/dialogs/delete-dialog/delete-dialog.component';
import { Guid } from 'guid-typescript';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {
  users: User[] = [];
  groups: Group[] = [];

  constructor(
    private userService: UserService,
    private groupService: GroupService,
    private toaster: ToastrService,
    private dialogService: NbDialogService
  ) {}

  ngOnInit(): void {
    this.getUsers();
    this.getGroups();
  }

  async getUsers(): Promise<void> {
    await this.userService
      .getAllUsers()
      .toPromise()
      .then((response) => {
        this.users = response?.data || [];
      })
      .catch((error) => {
        this.toaster.error('Failed to load users');
        return error;
      });
  }

  async getGroups(): Promise<void> {
    await this.groupService
      .getAllGroups()
      .toPromise()
      .then((response) => {
        response?.data.forEach((element) => {
          if (element.image != '') {
            element.image = 'data:text/plain;base64,' + element.image;
          }
        });
        this.groups = response?.data || [];
      })
      .catch((error) => {
        this.toaster.error('Failed to load groups');
        return error;
      });
  }

  async changeStatus(user: User): Promise<void> {
    await this.userService
      .editUser(user)
      .toPromise()
      .then(() => {
        this.getUsers();
      })
      .catch((error) => {
        this.toaster.error('Failed to update user');
        return error;
      });
  }

  async deleteUser(userId: Guid): Promise<void> {
    await this.userService
      .deleteUser(userId)
      .toPromise()
      .then(() => {
        this.getUsers();
        this.toaster.success('Deleted user successfully');
      })
      .catch((error) => {
        this.toaster.error('Failed to delete user');
        return error;
      });
  }

  async deleteGroup(groupId: Guid): Promise<void> {
    await this.groupService
      .removeGroup(groupId)
      .toPromise()
      .then(() => {
        this.getGroups();
        this.toaster.success('Deleted group successfully');
      })
      .catch((error) => {
        this.toaster.error('Failed to update group');
        return error;
      });
  }

  open(user: User): void {
    this.dialogService
      .open(AdminDialogComponent, {
        context: {
          userStatus: user.status,
          userRole: user.role
        }
      })
      .onClose.subscribe(
        (returnedObj: { returnedStatus: string; returnedRole: string }) => {
          if (returnedObj) {
            user.status = parseInt(returnedObj.returnedStatus);
            user.role = parseInt(returnedObj.returnedRole);

            this.changeStatus(user);
          }
        }
      );
  }

  openDeleteUserDialog(userId: Guid, userName: string): void {
    this.dialogService
      .open(DeleteDialogComponent, {
        context: {
          message: `Are you sure you want to delete the user '${userName}'?`
        }
      })
      .onClose.subscribe((accepted: boolean) => {
        if (accepted) {
          this.deleteUser(userId);
        }
      });
  }

  openDeleteGroupDialog(groupId: Guid, groupName: string): void {
    this.dialogService
      .open(DeleteDialogComponent, {
        context: {
          message: `Are you sure you want to delete the group '${groupName}'?`
        }
      })
      .onClose.subscribe((accepted: boolean) => {
        if (accepted) {
          this.deleteGroup(groupId);
        }
      });
  }

  formatStatus(status: number): string {
    let formattedString = '';
    switch (status) {
      case 0:
        formattedString = 'Pending';
        break;
      case 1:
        formattedString = 'Accepted';
        break;
      case 2:
        formattedString = 'Declined';
        break;
    }

    return formattedString;
  }
}
