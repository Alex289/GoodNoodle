import { Component, Input } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';

@Component({
  selector: 'app-delete-dialog',
  templateUrl: './delete-dialog.component.html',
  styleUrls: ['./delete-dialog.component.scss']
})
export class DeleteDialogComponent {
  @Input() message = '';

  constructor(private dialog: NbDialogRef<DeleteDialogComponent>) {}

  close(accepted: boolean): void {
    this.dialog.close(accepted);
  }
}
