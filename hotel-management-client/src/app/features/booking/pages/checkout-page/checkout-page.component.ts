import {Component, inject, OnInit} from '@angular/core';
import {MatCard, MatCardContent, MatCardTitle} from '@angular/material/card';
import {MatFormField, MatLabel} from '@angular/material/form-field';
import {MatInput} from '@angular/material/input';
import {Room} from '../../../../shared/models/room.model';
import {ActivatedRoute, Router} from '@angular/router';
import {NgForOf, NgOptimizedImage, NgStyle} from '@angular/common';
import {environment} from '../../../../../environments/environment';
import {Reservation} from '../../../../shared/models/reservation.model';
import {Service} from '../../../../shared/models/service.model';
import {MatDivider} from '@angular/material/divider';
import {MatAccordion, MatExpansionPanel, MatExpansionPanelHeader} from '@angular/material/expansion';
import {MatRadioButton, MatRadioGroup} from '@angular/material/radio';
import {MatButton} from '@angular/material/button';
import {User} from '../../../../shared/models/user.model';
import {Form, FormsModule} from '@angular/forms';
import {EReservationStatus} from '../../../../shared/enums/ereservation-status.enum';
import {Payment} from '../../../../shared/models/payment.model';
import {EPaymentMethod} from '../../../../shared/enums/epayment-method.enum';
import * as uuid from 'uuid';
import {BookingService} from '../../booking.service';
import {MatSnackBar} from '@angular/material/snack-bar';

@Component({
  selector: 'app-checkout-page',
  imports: [
    MatCard,
    MatCardTitle,
    MatFormField,
    MatLabel,
    MatInput,
    NgOptimizedImage,
    MatCardContent,
    NgForOf,
    MatDivider,
    MatExpansionPanel,
    MatExpansionPanelHeader,
    MatRadioGroup,
    MatRadioButton,
    MatAccordion,
    MatButton,
    FormsModule,
  ],
  templateUrl: './checkout-page.component.html',
  styleUrl: './checkout-page.component.scss'
})
export class CheckoutPageComponent implements OnInit {
  private router: Router = inject(Router);
  private route: ActivatedRoute = inject(ActivatedRoute);
  public room: Room;
  public reservation: Reservation;
  public services: Service[];
  public total: number;
  public floor: number;
  public user: User;
  public payment: Payment;
  protected readonly EPaymentMethod = EPaymentMethod;
  private _snackBar = inject(MatSnackBar);

  constructor(private bookingService: BookingService) {
    this.floor = 0;
    this.total = 0;
    this.reservation = new Reservation();
    this.room = new Room("Single", 0, 0, "single.jpg");
    this.services = [];
    this.user = new User();
    this.payment = new Payment();
    this.payment.method = EPaymentMethod.CreditCard;
  }

  ngOnInit() {

    this.route.queryParams.subscribe(params => {
      this.room = JSON.parse(params['room']);
      this.reservation.checkinDate = new Date(params['checkinDate']);
      this.reservation.checkoutDate = new Date(params['checkoutDate']);
      this.services = JSON.parse(params['services']);
      this.floor = +params['floor'];
    })
    this.reservation.roomType = this.room.room_type;

    let runningDate = new Date(this.reservation.checkinDate);
    while (runningDate < this.reservation.checkoutDate) {
      this.total += this.room.price;
      runningDate.setDate(runningDate.getDate() + 1);
    }

    for(let service of this.services){
      this.total += service.price;
    }

    const userData = sessionStorage.getItem("user");
    if(userData){
      this.user = JSON.parse(userData);
    }
  }

  protected readonly environment = environment;

  bookNow(personalDataForm: HTMLFormElement, cardDataForm: HTMLFormElement) {
    if(!personalDataForm.checkValidity()){
      this._snackBar.open("Please, fill forms properly", "Ok");
      return;
    }
    if(!cardDataForm.checkValidity() && this.payment.method === EPaymentMethod.CreditCard) {
      this._snackBar.open("Please, fill forms properly", "Ok");
      return;
    }
    this.reservation.id = uuid.v4();
    this.reservation.guestEmail = this.user.email;
    this.reservation.status = EReservationStatus.Reserved;

    this.payment.id = uuid.v4();
    this.payment.reservationId = this.reservation.id;
    this.payment.date = new Date();
    this.payment.amount = this.total;

    this.bookingService.bookRoom(this.reservation, this.payment, this.services, this.floor).subscribe({
      next: (result) => {
        this._snackBar.open("You made reservation successfully.", "Ok");
        this.router.navigate(['/Guest']);
        console.log(result);
      },
      error: (err) => {
        this._snackBar.open(err.message, "Ok");
        console.log(err);
      }
    })
  }
}
