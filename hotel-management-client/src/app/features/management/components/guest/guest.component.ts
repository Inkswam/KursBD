import {Component, Input} from '@angular/core';
import {User} from '../../../../shared/models/user.model';
import {MatCard, MatCardContent, MatCardTitle} from '@angular/material/card';
import {
  MatExpansionPanel,
  MatExpansionPanelDescription,
  MatExpansionPanelHeader,
  MatExpansionPanelTitle
} from '@angular/material/expansion';
import {GuestReservationComponent} from '../guest-reservation/guest-reservation.component';
import {NgForOf, NgIf} from '@angular/common';
import {Reservation} from '../../../../shared/models/reservation.model';
import {Router} from '@angular/router';

@Component({
  selector: 'app-guest',
  imports: [
    MatCardTitle,
    MatCardContent,
    MatCard,
    MatExpansionPanel,
    MatExpansionPanelHeader,
    MatExpansionPanelTitle,
    GuestReservationComponent,
    MatExpansionPanelDescription,
    NgForOf,
    NgIf
  ],
  templateUrl: './guest.component.html',
  styleUrl: './guest.component.scss'
})
export class GuestComponent {
  @Input() user!: User;
  @Input() reservations!: Set<Reservation> | undefined;

  constructor(public router: Router) {

  }

  navigateToGuest() {
    this.router.navigate(['Receptionist/guest'],
      {
      queryParams: {
        email: this.user.email
      }
    });
  }
}
