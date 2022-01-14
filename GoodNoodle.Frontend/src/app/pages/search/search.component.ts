import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NbDialogService } from '@nebular/theme';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { AddToGroupDialogComponent } from 'src/app/components/dialogs/add-to-group-dialog/add-to-group-dialog.component';
import { UserService } from 'src/app/services/api/user.service';
import { User } from 'src/app/types/entities/user/user';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit, OnDestroy {
  private routeSub: Subscription = new Subscription();
  users: User[] = [];
  searchValue = '';

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private toaster: ToastrService,
    private dialogService: NbDialogService
  ) {}

  ngOnInit(): void {
    this.routeSub = this.route.params.subscribe((params) => {
      this.searchValue = params['name'].replace('-', ' ');

      this.searchUser();
    });
  }

  ngOnDestroy(): void {
    this.routeSub.unsubscribe();
  }

  async searchUser(): Promise<void> {
    await this.userService
      .searchByName(this.searchValue)
      .toPromise()
      .then((response) => {
        this.users = response?.data || [];
      })
      .catch((error) => {
        this.toaster.error('Failed to load users');
        return error;
      });
  }

  open(user: User): void {
    this.dialogService.open(AddToGroupDialogComponent, {
      context: {
        userId: user.id,
        userName: user.fullName,
        userEmail: user.email
      }
    });
  }
}
