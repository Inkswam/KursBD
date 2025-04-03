import {Component, inject, OnInit} from '@angular/core';
import {MatToolbar} from '@angular/material/toolbar';
import {MatButton, MatIconButton} from '@angular/material/button';
import {MatIcon} from '@angular/material/icon';
import {MatCard, MatCardContent, MatCardTitle} from '@angular/material/card';
import {NgForOf} from '@angular/common';
import {Reservation} from '../../../../shared/models/reservation.model';
import {ActivatedRoute, Router} from '@angular/router';
import {User} from '../../../../shared/models/user.model';
import {ManagementService} from '../../management.service';
import {MatSnackBar} from '@angular/material/snack-bar';

@Component({
  selector: 'app-guest-page',
    imports: [
        MatToolbar,
        MatIconButton,
        MatIcon,
        MatCard,
        MatCardTitle,
        MatCardContent,
        NgForOf,
        MatButton
    ],
  templateUrl: './guest-page.component.html',
  styleUrl: './guest-page.component.scss'
})
export class GuestPageComponent implements OnInit {
  guest: User;
  reservations: Reservation[] = [];
  guestEmail: string;
  private _snackBar = inject(MatSnackBar);

  constructor(
    private managementService: ManagementService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.guest = new User();
    this.guestEmail = "";
  }

  ngOnInit(): void {
    // Retrieve the guest email from URL parameters
    this.guestEmail = this.route.snapshot.queryParams['email'];

    if (this.guestEmail !== "") {
      this.loadGuestData();
      this.loadReservations();
    }
  }

  loadGuestData(): void {
    this.managementService.getGuestByEmail(this.guestEmail).subscribe(
      (data: User) => {
        data.birthDate = new Date(data.birthDate);
        this.guest = data;
      },
      (error) => {
        console.error('Error loading guest data', error);
      }
    );
  }

  loadReservations(): void {
    this.managementService.getReservationsByGuestEmail(this.guestEmail).subscribe({
      next: (reservations: Reservation[]) => {
        this.reservations = [];

        for(let reservation of reservations) {
          reservation.checkinDate = new Date(reservation.checkinDate);
          reservation.checkoutDate = new Date(reservation.checkoutDate);
          this.reservations.push(reservation);
        }
      },
      error: (error) => {
        console.error('Error loading reservations', error);}
    });
  }

  goBack(): void {
    this.router.navigate(['Receptionist/guests']); // Adjust the route according to your routing setup
  }

  navigateToBooking(reservation: Reservation) {
    this.router.navigate(['Receptionist/reservation'], {
      queryParams: {
        email: this.guestEmail,
        id: reservation.id
      }
    });
  }

  addUserToBlacklist() {
    this.managementService.addUserToBlacklist(this.guest.email).subscribe({
      next: user => {
        this._snackBar.open(`User ${user.email} added to Blacklist.`, "Ok");
      },
      error: err => console.log(err),
    })
  }
}
