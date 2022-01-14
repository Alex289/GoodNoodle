import { Component } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';
import { ToastrService } from 'ngx-toastr';
import { GroupService } from 'src/app/services/api/group.service';

@Component({
  selector: 'app-create-group-dialog',
  templateUrl: './create-group-dialog.component.html',
  styleUrls: ['./create-group-dialog.component.scss']
})
export class CreateGroupDialogComponent {
  groupName = '';
  groupImage = '';

  constructor(
    private groupService: GroupService,
    private toaster: ToastrService,
    private dialog: NbDialogRef<CreateGroupDialogComponent>
  ) {}

  onFileChanged(event: Event): void {
    const target = event.target as HTMLInputElement;
    const files = target.files as FileList;

    if (files[0]) {
      const reader = new FileReader();

      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      reader.onload = (readerEvent: any) => {
        this.groupImage = btoa(readerEvent.target.result);
      };

      reader.readAsBinaryString(files[0]);
    }
  }

  async createNewGroup(): Promise<void> {
    if (this.groupName !== '') {
      const response = await this.groupService
        .createNewGroup(this.groupName, this.groupImage)
        .toPromise()
        .catch((error) => {
          this.toaster.error(error.error.errors[0].value);
        });

      if (response?.success) {
        this.toaster.success('Successfully created group');
        this.close();
      }
    } else {
      this.toaster.error('Name must not be empty');
    }
  }

  close(): void {
    this.dialog.close();
  }
}
