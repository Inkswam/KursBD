import {Component, Input, OnInit, signal, ViewChild, ViewEncapsulation} from '@angular/core';
import {MatCard, MatCardContent, MatCardTitle} from "@angular/material/card";
import {MatCalendar} from '@angular/material/datepicker';
import {ManagementService} from '../../management.service';
import {NgForOf} from '@angular/common';
import {User} from '../../../../shared/models/user.model';
import {MatList, MatListItem} from '@angular/material/list';
import {Reservation} from '../../../../shared/models/reservation.model';

@Component({
  selector: 'app-calendar',
  imports: [
    MatCard,
    MatCardContent,
    MatCardTitle,
    MatCalendar,
    MatList,
    MatListItem,
    NgForOf
  ],
  templateUrl: './calendar.component.html',
  styleUrl: './calendar.component.scss',
  encapsulation: ViewEncapsulation.None,
})
export class CalendarComponent implements OnInit {
  @ViewChild(MatCalendar) calendar!: MatCalendar<Date>;

  currentDate = new Date();
  bookedDates: string[] | null;
  users: Set<User>;
  reservations: Reservation[];
  selectedDate: Date;

  constructor(private managementService: ManagementService) {
    this.bookedDates = null;
    this.users = new Set();
    this.reservations = [];
    this.selectedDate = new Date();
  }

  ngOnInit() {
    this.managementService.getBookedDates().subscribe({
      next: data => {
        this.bookedDates = [];
        for(let d of data){
          this.bookedDates.push(d);
        }

        this.calendar.updateTodaysDate();
      },
      error: err => {
        console.log(err);
      }
    })
  }

  getGuestsByCheckinDate(date: Date | null) {
    if(date === null){
      console.log("Date is null, pick valid date");
      return;
    }

    this.managementService.getGuestsByDate(date).subscribe({
      next: usersReservations => {
        this.users = new Set();
        for(let user of usersReservations.users){
          this.users.add(user);
        }

        this.reservations = [];
        for(let reservation of usersReservations.reservations){
          reservation.checkinDate = new Date(reservation.checkinDate);
          reservation.checkoutDate = new Date(reservation.checkoutDate);
          this.reservations.push(reservation);
        }
      },
      error: err => {console.log(err);}
    })

  }

  dateClass = (date: Date) =>
  {
    if(this.bookedDates === null) {
      return "";
    }

    let currentDate = new Date();
    if(currentDate > date){
      return ""
    }
    let dateOnly: string = date.toDateString();

    if(this.bookedDates.includes(dateOnly)) {
      return "booked-date";
    }

    return "free-date";
  };

}
