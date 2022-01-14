import { Component, Input, OnInit } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';

@Component({
  selector: 'app-admin-dialog',
  templateUrl: './admin-dialog.component.html',
  styleUrls: ['./admin-dialog.component.scss']
})
export class AdminDialogComponent implements OnInit {
  @Input() userStatus = 0;
  @Input() userRole = 0;
  selectedStatus = '0';
  selectedRole = '0';

  constructor(private dialog: NbDialogRef<AdminDialogComponent>) {}

  ngOnInit(): void {
    this.selectedStatus = this.userStatus.toString();
    this.selectedRole = this.userRole.toString();
  }

  close(returnedStatus: string, returnedRole: string): void {
    this.dialog.close({ returnedStatus, returnedRole });
  }
}
