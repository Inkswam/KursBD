import {Component, inject, OnInit} from '@angular/core';
import {MatCard, MatCardContent, MatCardTitle} from '@angular/material/card';
import {BookingService} from '../../../booking/booking.service';
import {ActivatedRoute, Router} from '@angular/router';
import {Reservation} from '../../../../shared/models/reservation.model';
import {User} from '../../../../shared/models/user.model';
import {Payment} from '../../../../shared/models/payment.model';
import {Room} from '../../../../shared/models/room.model';
import {MatSnackBar} from '@angular/material/snack-bar';
import {ManagementService} from '../../management.service';
import {Service} from '../../../../shared/models/service.model';
import {NgForOf, NgOptimizedImage} from '@angular/common';
import {environment} from '../../../../../environments/environment';
import {MatFormField, MatLabel} from '@angular/material/form-field';
import {MatOption, MatSelect} from '@angular/material/select';
import {FormsModule} from '@angular/forms';
import {MatButton} from '@angular/material/button';

@Component({
  selector: 'app-reservation-page',
  imports: [
    MatCard,
    MatCardTitle,
    MatCardContent,
    NgOptimizedImage,
    NgForOf,
    MatFormField,
    MatLabel,
    MatSelect,
    MatOption,
    FormsModule,
    MatButton,
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
  availableRooms: number[];
  desiredRoomNumber: number;

  private _snackBar = inject(MatSnackBar);

  constructor(public managementService: ManagementService, private router: Router, private route: ActivatedRoute,) {
    this.reservation = new Reservation();
    this.user = new User();
    this.payment = new Payment();
    this.room = new Room('', 0, 0, '');
    this.services = [];
    this.availableRooms = [];
    this.desiredRoomNumber = this.reservation.roomNumber;
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
        this.availableRooms.push(this.reservation.roomNumber);
        this.desiredRoomNumber = this.reservation.roomNumber;

        this.services = result.services;

        this.getFreeRooms();
      },
      error: error => console.log(error)
    })
  }

  getFreeRooms(){
    this.managementService.getFreeRooms(
      this.reservation.checkinDate,
      this.reservation.checkoutDate,
      this.reservation.roomType)
      .subscribe({
        next: roomNumbers => {
          for(let number of roomNumbers){
            this.availableRooms.push(number);
          }
        },
        error: err => console.error(err)
      })
  }

  protected readonly environment = environment;

  setNewRoom() {
    this.managementService.setNewRoom(this.reservation.id, this.desiredRoomNumber).subscribe({
      next: result => {
        this.reservation.roomNumber = result;
        this._snackBar.open(`Room has been changed to ${result}`, "Ok");
      },
      error: err => console.error(err)
    })
  }
}
