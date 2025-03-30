import {Component, OnInit} from '@angular/core';
import {MatToolbar} from '@angular/material/toolbar';
import {MatFormField, MatLabel} from '@angular/material/form-field';
import {MatInput} from '@angular/material/input';
import {FormsModule} from '@angular/forms';
import {MatOption, MatSelect} from '@angular/material/select';
import {NgForOf} from '@angular/common';
import {User} from '../../../../shared/models/user.model';
import {ManagementService} from '../../management.service';
import {GuestComponent} from '../../components/guest/guest.component';
import {Reservation} from '../../../../shared/models/reservation.model';

@Component({
  selector: 'app-guests-page',
  imports: [
    MatToolbar,
    MatFormField,
    MatInput,
    FormsModule,
    NgForOf,
    GuestComponent,
    MatLabel
  ],
  templateUrl: './guests-page.component.html',
  styleUrl: './guests-page.component.scss'
})
export class GuestsPageComponent implements OnInit {

  allUsers: User[];
  users: User[];
  reservations: Map<string, Set<Reservation>>;
  searchName = '';
  searchPhone = '';

  constructor(private managementService: ManagementService) {
    this.allUsers = [];
    this.users = [];
    this.reservations = new Map<string, Set<Reservation>>();
  }

  ngOnInit() {
    this.managementService.getAllGuestsWithReservations().subscribe({
      next: data => {
        this.users = [];
        for(let user of data.users){
          user.birthDate = new Date(user.birthDate);
          this.allUsers.push(user);
        }

        this.reservations = new Map<string, Set<Reservation>>();
        for(let reservation of data.reservations){
          reservation.checkinDate = new Date(reservation.checkinDate);
          reservation.checkoutDate = new Date(reservation.checkoutDate);

          if(this.reservations.has(reservation.guestEmail)){
            this.reservations.get(reservation.guestEmail)?.add(reservation);
          }
          else{
            this.reservations.set(reservation.guestEmail, new Set<Reservation>([reservation]));
          }
        }

        this.applyFilters();
      },
      error: error => console.log(error),
    })

    console.log(this.reservations.values());
  }

  applyFilters() {
    this.users = this.allUsers
      .filter(
        (u) =>
          (!this.searchName ||
            u.firstName.toLowerCase().includes(this.searchName.toLowerCase()) ||
            u.lastName.toLowerCase().includes(this.searchName.toLowerCase()) ||
            (u.firstName.toLowerCase() + " " + u.lastName.toLowerCase()).includes(this.searchName.toLowerCase())) &&
          (!this.searchPhone || u.phoneNumber.toLowerCase().includes(this.searchPhone))
      );

    console.log(this.users);
  }
}
