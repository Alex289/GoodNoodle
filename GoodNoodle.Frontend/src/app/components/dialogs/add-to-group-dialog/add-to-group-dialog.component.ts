import { Component, Input, OnInit } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';
import { Guid } from 'guid-typescript';
import { ToastrService } from 'ngx-toastr';
import { GroupService } from 'src/app/services/api/group.service';
import { UserInGroupService } from 'src/app/services/api/user-in-group.service';
import { InviteToGroup } from 'src/app/types/entities/userInGroup/inviteToGroup';
import { AuthService } from 'src/app/services/auth/auth.service';
import { Group } from 'src/app/types/entities/group';

@Component({
  selector: 'app-add-to-group-dialog',
  templateUrl: './add-to-group-dialog.component.html',
  styleUrls: ['./add-to-group-dialog.component.scss']
})
export class AddToGroupDialogComponent implements OnInit {
  @Input() userId = Guid.createEmpty();
  @Input() userName = '';
  @Input() userEmail = '';
  userInGroups: Group[] = [];
  groups: Group[] = [];
  role = '1';

  constructor(
    private groupService: GroupService,
    private userInGroupService: UserInGroupService,
    private toaster: ToastrService,
    private dialog: NbDialogRef<AddToGroupDialogComponent>,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.getUserInGroups().then(() => {
      this.getGroups();
    });
  }

  async getGroups(): Promise<void> {
    await this.groupService
      .getJoinableGroups(this.userId)
      .toPromise()
      .then((response) => {
        const editedList: Group[] = [];

        response?.data.forEach((element) => {
          if (element.image != '') {
            element.image = 'data:text/plain;base64,' + element.image;
          }

          if (
            this.authService.isAdmin() ||
            this.authService.isTeacher(element.id)
          ) {
            editedList.push(element);
          }
        });

        this.groups = editedList;
      })
      .catch((error) => {
        this.toaster.error('Failed to load groups');
        return error;
      });
  }

  async getUserInGroups(): Promise<void> {
    await this.userInGroupService
      .getByUser(this.userId)
      .toPromise()
      .then((response) => {
        this.userInGroups = response?.data || [];
      })
      .catch((error) => {
        if (error.error.errors[0].key != 'ENTITY_NOT_FOUND') {
          this.toaster.error('Failed to load groups of user');
          return error;
        }
      });
  }

  async inviteToGroup(group: Group): Promise<void> {
    const inviteToGroup: InviteToGroup = {
      Id: this.userId,
      FullName: this.userName,
      Email: this.userEmail,
      Role: Number(this.role),
      GroupId: group.id,
      GroupName: group.name
    };

    await this.userInGroupService
      .inviteToGroup(inviteToGroup)
      .toPromise()
      .then(() => {
        this.toaster.success('Successfully invited to group');
      })
      .catch((error) => {
        this.toaster.error('Failed to invite user to group');
        return error;
      });
  }

  close(): void {
    this.dialog.close();
  }
}
