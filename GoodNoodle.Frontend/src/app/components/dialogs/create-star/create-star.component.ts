import { Component } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';

@Component({
  selector: 'app-create-star',
  templateUrl: './create-star.component.html',
  styleUrls: ['./create-star.component.scss']
})
export class CreateStarComponent {
  reason = '';

  constructor(private dialog: NbDialogRef<CreateStarComponent>) {}

  close(accepted: boolean): void {
    this.dialog.close({ accepted, reason: this.reason });
  }
}
