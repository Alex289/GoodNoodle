import { Component, OnDestroy, OnInit } from '@angular/core';
import { GroupService } from 'src/app/services/api/group.service';
import { Guid } from 'guid-typescript';
import { User } from '../../types/entities/user/user';
import { Group } from 'src/app/types/entities/group';
import { StarService } from 'src/app/services/api/star.service';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { Star } from 'src/app/types/entities/star/star';
import { AuthService } from 'src/app/services/auth/auth.service';
import { NbDialogService } from '@nebular/theme';
import { DeleteDialogComponent } from 'src/app/components/dialogs/delete-dialog/delete-dialog.component';
import { UserInGroupService } from 'src/app/services/api/user-in-group.service';
import { CreateStarComponent } from 'src/app/components/dialogs/create-star/create-star.component';
import { CreateStar } from 'src/app/types/entities/star/createStar';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.scss']
})
export class GroupComponent implements OnInit, OnDestroy {
  currentUserId = Guid.createEmpty();
  checkedStars: Star[] = [];
  isTeacher = false;
  isAdmin = false;
  isInGroup = false;
  users: User[] = [];
  teacher: User[] = [];
  userStarList: Star[] = [];
  userInGroupStarList: Star[] = [];
  group: Group = { id: Guid.createEmpty(), name: '', image: '' };

  groupId = Guid.createEmpty();
  userId = Guid.createEmpty();
  starId = Guid.createEmpty();

  private routeSub: Subscription = new Subscription();
  constructor(
    private route: ActivatedRoute,
    private groupService: GroupService,
    private starService: StarService,
    private userInGroupService: UserInGroupService,
    private authService: AuthService,
    private dialogService: NbDialogService,
    private toaster: ToastrService
  ) {}

  ngOnInit(): void {
    this.routeSub = this.route.params.subscribe(async (params) => {
      this.groupId = params['id'];

      await this.getAllUsersInGroup();
      await this.getGroup();

      this.authService.isTeacher(this.groupId).then((isTeacher) => {
        this.isTeacher = isTeacher;
      });

      this.authService.isInGroup(this.groupId).then((isInGroup) => {
        this.isInGroup = isInGroup;
      });

      this.currentUserId = this.authService.getCurrentUserId();

      this.isAdmin = this.authService.isAdmin();
    });
  }

  ngOnDestroy(): void {
    this.routeSub.unsubscribe();
  }

  selectStar(id: Guid): void {
    this.starId = id;
  }

  async getAllUsersInGroup(): Promise<void> {
    await this.groupService
      .getAllUsersInGroup(this.groupId)
      .toPromise()
      .then((response) => {
        if (!response?.data) {
          this.users = [];
          this.teacher = [];
        } else {
          this.users = response.data.filter((user) => user.role === 1);
          this.teacher = response.data.filter((user) => user.role === 0);
        }
      })
      .catch((error) => {
        this.toaster.error('Failed loading user in group');
        return error;
      });
    await this.getStars();
  }

  async getStars(): Promise<void> {
    await this.users.forEach((user) => {
      this.getStarsOfUser(user);
    });
  }

  async getStarsOfUser(user: User): Promise<void> {
    await this.starService
      .getStarByUserId(user.id)
      .toPromise()
      .then((response) => {
        this.userStarList = response?.data || [];
      })
      .catch((error) => {
        this.toaster.error('Failed to get stars of user');
        return error;
      });

    this.userStarList.forEach((star) => {
      if (star.groupId == this.groupId) {
        this.userInGroupStarList.push(star);
      }
    });

    user.stars = this.userInGroupStarList;
    this.userInGroupStarList = [];
  }

  async getGroup(): Promise<void> {
    await this.groupService
      .getGroupById(this.groupId)
      .toPromise()
      .then((response) => {
        this.group = response?.data || {
          id: Guid.createEmpty(),
          name: '',
          image: ''
        };

        if (response?.data && response.data.image != '') {
          response.data.image = 'data:text/plain;base64,' + response.data.image;
        }
      })
      .catch((error) => {
        this.toaster.error('Failed to load group');
        return error;
      });
  }

  toggle(checked: boolean, star: Star): void {
    if (checked) {
      this.checkedStars.push(star);
    } else {
      this.checkedStars = this.checkedStars.filter(
        (item) => item.id != star.id
      );
    }
  }

  editStar(star: Star): void {
    star.beingEdited = !star.beingEdited;
  }

  async saveStar(star: Star): Promise<void> {
    await this.starService
      .editStar(star)
      .toPromise()
      .then(() => {
        this.toaster.success('Successfully saved star');
        this.getAllUsersInGroup();
        this.getGroup();
      })
      .catch((error) => {
        this.toaster.error('Failed to save star');
        return error;
      });

    star.beingEdited = false;
  }

  openAddStarDialog(userId: Guid): void {
    this.dialogService
      .open(CreateStarComponent)
      .onClose.subscribe(({ accepted, reason }) => {
        if (accepted) {
          const star: CreateStar = {
            userId: userId,
            groupId: this.groupId,
            reason: reason
          };

          this.createStar(star);
          this.getAllUsersInGroup();
          this.getGroup();
        }
      });
  }

  async createStar(star: CreateStar): Promise<void> {
    await this.starService
      .createStar(star)
      .toPromise()
      .then(() => {
        this.toaster.success('Successfully created star');
        this.getAllUsersInGroup();
        this.getGroup();
      })
      .catch((error) => {
        this.toaster.error('Failed to create star');
        return error;
      });
  }

  openDeleteStarDialog(): void {
    this.dialogService
      .open(DeleteDialogComponent, {
        context: {
          message: `Are you sure you want to delete the selected stars?`
        }
      })
      .onClose.subscribe((accepted: boolean) => {
        if (accepted) {
          this.checkedStars.forEach((star: Star) => {
            this.deleteStar(star);
          });
        }
      });
  }

  openDeleteGroupDialog(): void {
    this.dialogService
      .open(DeleteDialogComponent, {
        context: {
          message: `Are you sure you want to delete '${this.group.name}'?`
        }
      })
      .onClose.subscribe((accepted: boolean) => {
        if (accepted) {
          this.deleteGroup();
        }
      });
  }

  async deleteGroup(): Promise<void> {
    await this.groupService
      .removeGroup(this.group.id)
      .toPromise()
      .then(() => {
        this.toaster.success('Successfully deleted group');
        window.location.pathname = '';
      })
      .catch((error) => {
        this.toaster.error('Failed to delete group');
        return error;
      });
  }

  openDeleteUserFromGroupDialog(user: User): void {
    this.dialogService
      .open(DeleteDialogComponent, {
        context: {
          message: `Are you sure you want to remove '${user.fullName}' from '${this.group.name}'?`
        }
      })
      .onClose.subscribe((accepted: boolean) => {
        if (accepted) {
          this.RemoveUserFromGroup(user.id);
        }
      });
  }

  openLeaveGroupDialog(userId: Guid): void {
    this.dialogService
      .open(DeleteDialogComponent, {
        context: {
          message: `Are you sure you want to leave '${this.group.name}'?`
        }
      })
      .onClose.subscribe((accepted: boolean) => {
        if (accepted) {
          this.RemoveUserFromGroup(userId);
        }
      });
  }

  async RemoveUserFromGroup(userId: Guid): Promise<void> {
    let userInGroupId: Guid;

    await this.userInGroupService
      .getAll()
      .toPromise()
      .then(async (response) => {
        userInGroupId =
          response?.data.find(
            (item) =>
              item.noodleGroupId === this.groupId &&
              item.noodleUserId === userId
          )?.id || Guid.createEmpty();

        await this.userInGroupService
          .removeUserFromGroup(userInGroupId)
          .toPromise()
          .then(() => {
            this.toaster.success('Successfully removed user from group');
            this.getAllUsersInGroup();
            this.getGroup();
            this.authService.isInGroup(this.groupId).then((isInGroup) => {
              this.isInGroup = isInGroup;
            });
          })
          .catch((error) => {
            this.toaster.error('Failed to remove user from group');
            return error;
          });
      })
      .catch((error) => {
        this.toaster.error('Failed to remove user from group');
        return error;
      });
  }

  async deleteStar(star: Star): Promise<void> {
    await this.starService
      .deleteStar(star.id)
      .toPromise()
      .then(() => {
        this.toaster.success('Successfully deleted star');
        this.getAllUsersInGroup();
        this.getGroup();
      })
      .catch((error) => {
        this.toaster.error('Failed to delete star');
        return error;
      });
  }
}
