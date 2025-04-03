import {Component, inject, Input} from '@angular/core';
import {MatCard, MatCardContent, MatCardTitle} from '@angular/material/card';
import {User} from '../../../../shared/models/user.model';
import {MatIconButton} from '@angular/material/button';
import {MatIcon} from '@angular/material/icon';
import {ManagementService} from '../../management.service';
import {MatSnackBar} from '@angular/material/snack-bar';

@Component({
  selector: 'app-blacklisted-user',
  imports: [
    MatCard,
    MatCardTitle,
    MatCardContent,
    MatIconButton,
    MatIcon,
  ],
  templateUrl: './blacklisted-user.component.html',
  styleUrl: './blacklisted-user.component.scss'
})
export class BlacklistedUserComponent {
  @Input() public user!: User;
  private _snackBar = inject(MatSnackBar);

  constructor(public managementServie: ManagementService) {
  }

  removeUserFromBlacklist() {
    this.managementServie.removeUserFromBlacklist(this.user.email).subscribe({
      next: () => {
        this._snackBar.open(`User ${this.user.email} removed successfully!`, "Ok");
      },
      error: err => console.log(err),
    })
  }
}
