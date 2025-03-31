import {Component, Input} from '@angular/core';
import {GuestReservationComponent} from "../guest-reservation/guest-reservation.component";
import {MatCard, MatCardContent, MatCardTitle} from "@angular/material/card";
import {
    MatExpansionPanel,
    MatExpansionPanelDescription,
    MatExpansionPanelHeader,
    MatExpansionPanelTitle
} from "@angular/material/expansion";
import {NgForOf} from "@angular/common";
import {User} from '../../../../shared/models/user.model';
import {Reservation} from '../../../../shared/models/reservation.model';
import {MatAnchor, MatButton} from '@angular/material/button';
import {Router, RouterLink, RouterLinkActive} from '@angular/router';
import {MatListItem} from '@angular/material/list';

@Component({
  selector: 'app-reservation',
  imports: [
    MatCard,
    MatCardContent,
    MatCardTitle,
    MatAnchor
  ],
  templateUrl: './reservation.component.html',
  styleUrl: './reservation.component.scss'
})
export class ReservationComponent {
  @Input() user!: User | undefined;
  @Input() reservation!: Reservation

  constructor(public router: Router, ) {

  }

  navigateToBooking() {
    this.router.navigate(['Receptionist/reservation'], {
      queryParams: {
        email: this.user?.email,
        id: this.reservation.id
      }
    });
  }

  navigateToGuest() {
    this.router.navigate(['Receptionist/guest'], {
      queryParams: {
        email: this.user?.email
      }
    });
  }
}
