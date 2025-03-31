import {Component, OnInit} from '@angular/core';
import {MatCard, MatCardContent, MatCardTitle} from '@angular/material/card';
import {MatListOption, MatSelectionList} from '@angular/material/list';
import {MatButton} from '@angular/material/button';
import {ManagementService} from '../../management.service';
import {Reservation} from '../../../../shared/models/reservation.model';
import {User} from '../../../../shared/models/user.model';
import {NgForOf} from '@angular/common';
import {MatIcon} from '@angular/material/icon';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'app-guest-list',
  imports: [
    MatButton,
    MatCard,
    MatCardContent,
    MatCardTitle,
    MatButton,
    MatButton,
    MatSelectionList,
    MatListOption,
    NgForOf,
    MatIcon
  ],
  templateUrl: './guest-list.component.html',
  styleUrl: './guest-list.component.scss'
})
export class GuestListComponent implements OnInit {
  reservations: Set<Reservation>
  users: Map<string, User>;

  constructor(private managementService: ManagementService, private router: Router, private route: ActivatedRoute) {
    this.reservations = new Set();
    this.users = new Map<string, User>();
  }

  ngOnInit(): void {
    this.managementService.getGuestsByCheckinDate(new Date()).subscribe({
      next: data => {
        this.reservations = new Set();
        for (let reservation of data.reservations) {
          reservation.checkinDate = new Date(reservation.checkinDate);
          reservation.checkoutDate = new Date(reservation.checkoutDate);
          this.reservations.add(reservation);
        }

        this.users = new Map<string, User>();
        for(let user of data.users) {
          this.users.set(user.email, user);
        }
      },
      error: error => console.log(error),
    })
  }

  protected readonly Math = Math;

  goToGuests() {
    this.router.navigate(['/Receptionist/guests']);
  }
  goToGuest(email: string) {
    this.router.navigate(['/Receptionist/guest'],
    {
      queryParams: {
        email: email
      }
    });
  }
}
