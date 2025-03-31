import {Component, Input} from '@angular/core';
import {Reservation} from '../../../../shared/models/reservation.model';
import {MatCard, MatCardContent, MatCardTitle} from '@angular/material/card';
import {User} from '../../../../shared/models/user.model';
import {Router} from '@angular/router';

@Component({
  selector: 'app-guest-reservation',
  imports: [
    MatCard,
    MatCardTitle,
    MatCardContent
  ],
  templateUrl: './guest-reservation.component.html',
  styleUrl: './guest-reservation.component.scss'
})
export class GuestReservationComponent {
  @Input() reservation!: Reservation;
  @Input() user!: User;

  constructor(private router: Router) {

  }

  navigateToBooking() {
    this.router.navigate(['Receptionist/reservation'], {
      queryParams: {
        email: this.user.email,
        id: this.reservation.id
      }
    });
  }
}
