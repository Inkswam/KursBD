import {Component, OnInit} from '@angular/core';
import {MatCard, MatCardContent, MatCardTitle} from '@angular/material/card';
import {BookingService} from '../../../booking/booking.service';
import {ActivatedRoute, Router} from '@angular/router';
import {Reservation} from '../../../../shared/models/reservation.model';
import {User} from '../../../../shared/models/user.model';
import {Payment} from '../../../../shared/models/payment.model';
import {Room} from '../../../../shared/models/room.model';
import {ERoomType} from '../../../../shared/enums/eroom-type.enum';
import {ManagementService} from '../../management.service';
import {Service} from '../../../../shared/models/service.model';
import {NgForOf, NgOptimizedImage} from '@angular/common';
import {environment} from '../../../../../environments/environment';

@Component({
  selector: 'app-reservation-page',
  imports: [
    MatCard,
    MatCardTitle,
    MatCardContent,
    NgOptimizedImage,
    NgForOf
  ],
  templateUrl: './reservation-page.component.html',
  styleUrl: './reservation-page.component.scss'
})
export class ReservationPageComponent implements OnInit {
  reservation: Reservation;
  user: User;
  payment: Payment;
  room: Room;
  services: Service[];

  constructor(public managementService: ManagementService, private router: Router, private route: ActivatedRoute,) {
    this.reservation = new Reservation();
    this.user = new User();
    this.payment = new Payment();
    this.room = new Room('', 0, 0, '');
    this.services = [];
  }

  ngOnInit() {
    let reservationId = this.route.snapshot.queryParams['id'];
    let email = this.route.snapshot.queryParams['email'];

    if(reservationId){
      this.managementService.getGuestByEmail(email).subscribe({
        next: user => {
          user.birthDate = new Date(user.birthDate);
          this.user = user;
        },
        error: err => console.error(err)
      })
      this.getBookings(reservationId)
    }
  }

  getBookings(reservationId: string) {
    this.managementService.getBooking(reservationId).subscribe({
      next: result => {
        this.room = result.room;

        result.payment.date = new Date(result.payment.date);
        this.payment = result.payment;

        result.reservation.checkinDate = new Date(result.reservation.checkinDate);
        result.reservation.checkoutDate = new Date(result.reservation.checkoutDate);

        this.reservation = result.reservation;

        this.services = result.services;

      },
      error: error => console.log(error)
    })
  }

  protected readonly environment = environment;
}
