import {afterNextRender, Component, Inject, inject, LOCALE_ID, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {AuthenticationService} from '../../authentication/authentication.service';
import {User} from '../../../shared/models/user.model';
import {MatFormField, MatInput, MatLabel, MatSuffix} from '@angular/material/input';
import {
  MatDatepickerInput,
  MatDatepickerModule,
  MatDatepickerToggle
} from '@angular/material/datepicker';
import {MatFabButton} from '@angular/material/button';
import {MatIcon} from '@angular/material/icon';
import {Filter} from '../../../shared/models/filter.model';
import {FormsModule, NgForm} from '@angular/forms';
import {MatOption, MatSelect} from '@angular/material/select';
import {ERoomType} from '../../../shared/enums/eroom-type.enum';
import {formatDate, NgForOf} from '@angular/common';
import {BookingService} from '../../../features/booking/booking.service';

@Component({
  selector: 'app-home-page',
  imports: [
    MatFormField,
    MatLabel,
    MatFormField,
    MatInput,
    MatDatepickerInput,
    MatDatepickerToggle,
    MatDatepickerModule,
    MatSuffix,
    MatFabButton,
    MatIcon,
    FormsModule,
    MatSelect,
    MatOption,
    NgForOf
  ],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent implements OnInit {

  private authenticationService = inject(AuthenticationService);
  public bookingService = inject(BookingService);
  private router = inject(Router);
  //private route: ActivatedRoute = inject(ActivatedRoute);
  roomTypeEnum = Object.entries(ERoomType);
  private role: string | null;
  public _currentDate: Date;
  public _tomorrowDate: Date;
  public filter: Filter;

  constructor(@Inject(LOCALE_ID) private locale: string) {

    this.role = null;
    this._currentDate = new Date();
    this._tomorrowDate = new Date();
    this._tomorrowDate.setDate(this._currentDate.getDate() + 1);
    this.filter = new Filter();

    afterNextRender(() =>{
      this.role = this.checkUserRole();
      if(this.role === null) {
        this.authenticationService.getUserWithToken().then(role => {
          if(role != null) {
            console.log("Role was null but now dont");
            this.authenticationService.getUsername();
            this.router.navigate(['/' + role]);
          }else{
            console.log("role was null and still null");
            this.router.navigate(['/Guest']);
          }
        });
      }else{
        console.log("role was not null");
        this.authenticationService.getUsername();
        this.router.navigate(['/' + this.role]);
      }
    })

    this.filter.checkinDate = this._currentDate;
    this.filter.checkoutDate = this._tomorrowDate;
    this.filter.floor = 1;
  }
  ngOnInit() {

  }
  checkUserRole(){
    let userString = sessionStorage.getItem('user');
    if(userString === null) return null;
    return (JSON.parse(userString) as User).userRole;
  }

  onSubmit(form: NgForm) {
    this.bookingService.isFormSubmitted = true;
    let checkinDateString = formatDate(this.filter.checkinDate, 'YYYY-MM-ddTHH:mm:ss.sssZ', this.locale);
    let checkoutDateString = formatDate(this.filter.checkoutDate, 'YYYY-MM-ddTHH:mm:ss.sssZ', this.locale);

    if (form.valid) {
      this.router.navigate(['/room-search-page'], {
        queryParams: {
          roomType: this.filter.roomType,
          checkinDate: checkinDateString,
          checkoutDate: checkoutDateString,
          floor: this.filter.floor
        },
      });
    }
  }
}
