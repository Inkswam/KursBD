import {Component, OnInit} from '@angular/core';
import {FormsModule} from "@angular/forms";
import {MatInput, MatLabel, MatSuffix} from "@angular/material/input";
import {MatToolbar} from "@angular/material/toolbar";
import {NgForOf} from "@angular/common";
import {MatFormField} from '@angular/material/form-field';
import {User} from '../../../../shared/models/user.model';
import {Reservation} from '../../../../shared/models/reservation.model';
import {ManagementService} from '../../management.service';
import {EReservationStatus} from '../../../../shared/enums/ereservation-status.enum';
import {MatOption, MatSelect} from '@angular/material/select';
import {ERoomType} from '../../../../shared/enums/eroom-type.enum';
import {MatDatepicker, MatDatepickerInput, MatDatepickerToggle} from '@angular/material/datepicker';
import {ReservationComponent} from '../../components/reservation/reservation.component';

@Component({
  selector: 'app-reservations-page',
  imports: [
    FormsModule,
    MatFormField,
    MatInput,
    MatLabel,
    MatToolbar,
    NgForOf,
    MatSelect,
    MatOption,
    MatDatepicker,
    MatDatepickerInput,
    MatDatepickerToggle,
    MatSuffix,
    ReservationComponent
  ],
  templateUrl: './reservations-page.component.html',
  styleUrl: './reservations-page.component.scss'
})
export class ReservationsPageComponent implements OnInit {

  allReservations: Reservation[];
  reservations: Reservation[];
  users: Map<string, User>;

  searchName = '';
  selectedStatus: string;
  selectedRoomType: string;
  selectedCheckinDate: Date | null;
  selectedCheckoutDate: Date | null;

  status = Object.entries(EReservationStatus);
  roomType = Object.entries(ERoomType);

  constructor(private managementService: ManagementService) {
    this.selectedStatus = "Any";
    this.selectedRoomType = 'Any';
    this.selectedCheckinDate = null;
    this.selectedCheckoutDate = null;
    this.allReservations = [];
    this.users = new Map<string, User>();
    this.reservations = [];
  }

  ngOnInit() {
    this.managementService.getAllGuestsWithReservations().subscribe({
      next: data => {
        this.allReservations = [];
        for(let reservation of data.reservations){
          reservation.checkinDate = new Date(reservation.checkinDate);
          reservation.checkoutDate = new Date(reservation.checkoutDate);
          this.allReservations.push(reservation);
        }

        this.users = new Map<string, User>();
        for(let user of data.users){
          user.birthDate = new Date(user.birthDate);

          this.users.set(user.email, user);
        }

        this.applyFilters();
      },
      error: error => console.log(error),
    })

    console.log(this.reservations.values());
  }

  applyFilters() {
    this.reservations = this.allReservations
      .filter(
        (r) =>
          (!this.searchName ||
            this.users.get(r.guestEmail)?.firstName.toLowerCase().includes(this.searchName.toLowerCase()) ||
            this.users.get(r.guestEmail)?.lastName.toLowerCase().includes(this.searchName.toLowerCase()) ||
            (
              this.users.get(r.guestEmail)?.firstName.toLowerCase()
              + " " +
              this.users.get(r.guestEmail)?.lastName.toLowerCase()
            ).includes(this.searchName.toLowerCase())) &&
          (this.selectedStatus === "Any" || r.status === this.selectedStatus) &&
          (this.selectedRoomType === "Any" || r.roomType === this.selectedRoomType) &&
          (!this.selectedCheckinDate || r.checkinDate.toDateString() === this.selectedCheckinDate.toDateString()) &&
          (!this.selectedCheckoutDate || r.checkoutDate.toDateString() === this.selectedCheckoutDate.toDateString())
      );

    console.log(this.users);
  }
}
