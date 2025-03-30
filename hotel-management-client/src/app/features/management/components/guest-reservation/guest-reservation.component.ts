import {Component, Input} from '@angular/core';
import {Reservation} from '../../../../shared/models/reservation.model';
import {MatCard, MatCardContent, MatCardTitle} from '@angular/material/card';

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

}
