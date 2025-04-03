import {Component, OnInit} from '@angular/core';
import {GuestReservationComponent} from '../../components/guest-reservation/guest-reservation.component';
import {Reservation} from '../../../../shared/models/reservation.model';
import {BookingService} from '../../../../features/booking/booking.service';
import {NgForOf} from '@angular/common';
import {Booking} from '../../../../shared/wrappers/booking.wrapper';

@Component({
  selector: 'app-guest-reservations-page',
  imports: [
    GuestReservationComponent,
    NgForOf
  ],
  templateUrl: './guest-reservations-page.component.html',
  styleUrl: './guest-reservations-page.component.scss'
})
export class GuestReservationsPageComponent implements OnInit {

  public bookingWrappers: Booking[];

  constructor(private bookingService: BookingService) {
    this.bookingWrappers = [];
  }

  ngOnInit() {
    let userString = sessionStorage.getItem('user');
    if(userString){
      let email: string = JSON.parse(userString).email;

      this.loadBookings(email);
    }

  }

  loadBookings(email: string) {
    this.bookingService.getReservationsByUser(email).subscribe({
      next: (bookings: Booking[]) => {
        this.bookingWrappers = [];

        for (let booking of bookings) {
          booking.reservation.checkinDate = new Date(booking.reservation.checkinDate);
          booking.reservation.checkoutDate = new Date(booking.reservation.checkoutDate);
          booking.payment.date = new Date(booking.payment.date);
          if(booking){
            this.bookingWrappers.push(booking);
          }
        }
      },
      error: (error) => console.log(error)
    })
  }

}
