import {Component, inject, Input} from '@angular/core';
import {MatCard, MatCardContent, MatCardTitle} from "@angular/material/card";
import {User} from '../../../../shared/models/user.model';
import {MatSnackBar} from '@angular/material/snack-bar';
import {MatButton} from '@angular/material/button';
import {NgIf} from '@angular/common';
import {AdminService} from '../../admin.service';
import {EUserRole} from '../../../../shared/enums/euser-role.enum';

@Component({
  selector: 'app-assignable-user',
  imports: [
    MatCard,
    MatCardContent,
    MatCardTitle,
    MatButton,
    NgIf
  ],
  templateUrl: './assignable-user.component.html',
  styleUrl: './assignable-user.component.scss'
})
export class AssignableUserComponent {
  @Input() public user!: User;
  private _snackBar = inject(MatSnackBar);

  constructor(public adminService: AdminService) {
  }

  promoteToManager() {
    this.adminService.promoteToManager(this.user.email).subscribe({
      next: () => {
        this._snackBar.open(`User ${this.user.email} promoted to manager successfully!`, "Ok");
      },
      error: err => console.log(err),
    })
  }

  degradeToGuest() {
    this.adminService.degradeToGuest(this.user.email).subscribe({
      next: () => {
        this._snackBar.open(`User ${this.user.email} degraded to guest successfully!`, "Ok");
      },
      error: err => console.log(err),
    })
  }

  protected readonly EUserRole = EUserRole;
}
