import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Guid } from 'guid-typescript';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { UserInGroupService } from 'src/app/services/api/user-in-group.service';
import { JoinGroup } from 'src/app/types/entities/userInGroup/joinGroup';

@Component({
  selector: 'app-join',
  templateUrl: './join.component.html'
})
export class JoinComponent implements OnInit, OnDestroy {
  private routeSub: Subscription = new Subscription();
  Id = Guid.createEmpty();

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private userInGroupService: UserInGroupService,
    private toaster: ToastrService
  ) {}

  ngOnInit(): void {
    this.routeSub = this.route.params.subscribe(async (params) => {
      this.Id = params['id'];

      this.joinGroup();
    });
  }

  ngOnDestroy(): void {
    this.routeSub.unsubscribe();
  }

  async joinGroup(): Promise<void> {
    const joinGroup: JoinGroup = {
      Id: this.Id
    };

    await this.userInGroupService
      .joinGroup(joinGroup)
      .toPromise()
      .then((response) => {
        if (response?.success) {
          this.toaster.success('Successfully joined group');
          this.router.navigateByUrl('/');
        }
      })
      .catch((error) => {
        this.toaster.error('Failed to join group');
        return error;
      });
  }
}
