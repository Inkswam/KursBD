import {Component, Input, OnInit} from '@angular/core';
import {MatCard, MatCardContent, MatCardTitle} from '@angular/material/card';
import {NgForOf, NgOptimizedImage} from '@angular/common';
import {environment} from '../../../../../environments/environment';
import {Booking} from '../../../../shared/wrappers/booking.wrapper';
import {BookingService} from '../../../../features/booking/booking.service';

@Component({
  imports: [
    MatCard,
    MatCardContent,
    MatCardTitle,
    NgForOf,
    NgOptimizedImage
  ],
  selector: 'app-guest-reservation',
  styleUrl: './guest-reservation.component.scss',
  templateUrl: './guest-reservation.component.html'
})
export class GuestReservationComponent implements OnInit {
  @Input() booking!: Booking;
  protected readonly environment = environment;

  constructor(private bookingService: BookingService) {

  }

  ngOnInit() {
    console.log(this.booking.room.image_url);
  }
}
