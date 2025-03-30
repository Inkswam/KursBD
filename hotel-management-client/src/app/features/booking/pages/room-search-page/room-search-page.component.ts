import {Component, Inject, inject, LOCALE_ID, OnInit} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {MatFormField, MatLabel, MatSuffix} from '@angular/material/form-field';
import {MatDatepickerModule, MatDatepickerInput, MatDatepickerToggle} from '@angular/material/datepicker';
import {FormsModule, NgForm} from '@angular/forms';
import {MatInput} from '@angular/material/input';
import {MatOption, MatSelect} from '@angular/material/select';
import {formatDate, NgForOf} from '@angular/common';
import {MatButton, MatIconButton} from '@angular/material/button';
import {MatSidenav, MatSidenavContainer, MatSidenavContent} from '@angular/material/sidenav';
import {RoomCardComponent} from '../../components/room-card/room-card.component';
import {MatIcon} from '@angular/material/icon';
import {Filter} from '../../../../shared/models/filter.model';
import {ActivatedRoute, Router} from '@angular/router';
import {BookingService} from '../../booking.service';
import {Room} from '../../../../shared/models/room.model';
import {ERoomType} from '../../../../shared/enums/eroom-type.enum';
import {MatAccordion} from '@angular/material/expansion';
import {ServicePanelComponent} from '../../components/service-panel/service-panel.component';
import {Service} from '../../../../shared/models/service.model';
import {MatDividerModule} from '@angular/material/divider';

@Component({
  selector: 'app-room-search-page',
  templateUrl: './room-search-page.component.html',
  imports: [
    MatFormField,
    MatLabel,
    MatDatepickerToggle,
    MatDatepickerModule,
    MatDatepickerInput,
    FormsModule,
    MatInput,
    MatSelect,
    MatOption,
    NgForOf,
    MatButton,
    MatSidenavContainer,
    MatIcon,
    MatIconButton,
    RoomCardComponent,
    MatSidenav,
    MatSidenavContent,
    MatSuffix,
    MatAccordion,
    ServicePanelComponent,
    MatDividerModule
  ],
  styleUrls: ['./room-search-page.component.scss']
})
export class RoomSearchPageComponent implements OnInit {
  private router: Router = inject(Router);
  private route: ActivatedRoute = inject(ActivatedRoute);

  public availableRooms: Room[] = [];
  public services: Service[] = [];
  public roomTypeEnum = Object.entries(ERoomType);
  public role: string | null;
  public _currentDate: Date;
  public _tomorrowDate: Date;
  public filter: Filter;

  constructor(private http: HttpClient, private bookingService: BookingService, @Inject(LOCALE_ID) private locale: string) {
    this.role = null;
    this._currentDate = new Date();
    this._tomorrowDate = new Date();
    this._tomorrowDate.setDate(this._currentDate.getDate() + 1);
    this.filter = new Filter();
  }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.filter.roomType = params['roomType'];
      this.filter.checkinDate = new Date(params['checkinDate']);
      this.filter.checkoutDate = new Date(params['checkoutDate']);
      this.filter.floor = +params['floor'];
    })

    this.getServices();
    this.searchRooms();
  }
  searchRooms() {
    if (!this.filter.checkinDate || !this.filter.checkoutDate || this.filter.floor === undefined) {
      console.log("Wrong filter for search");
      return; // Handle validation
    }

    this.bookingService.getAvailableRooms(this.filter).subscribe( {
      next: rooms => {
        for(let room of rooms) {
          this.availableRooms.push(room);
        }
      },
      error: error => {
        console.log(error);
      }
    });

  }

  getServices(){
    this.bookingService.getServices().subscribe( {
      next: services => {
        for(let service of services) {
          this.services.push(service);
        }
      },
      error: error => {
        console.log(error);
      }
    })
  }

  onSubmit(form: NgForm) {

    let checkinDateString = formatDate(this.filter.checkinDate, 'YYYY-MM-ddTHH:mm:ss.sssZ', this.locale);
    let checkoutDateString = formatDate(this.filter.checkoutDate, 'YYYY-MM-ddTHH:mm:ss.sssZ', this.locale);

    if (form.valid) {
      this.availableRooms = [];
      this.router.navigate(['/room-search-page'], {
        queryParams: {
          roomType: this.filter.roomType,
          checkinDate: checkinDateString,
          checkoutDate: checkoutDateString,
          floor: this.filter.floor
        },
      });
    }

    this.searchRooms();
  }

  proceedToBooking(room: Room){
    let servicesString = this.bookingService.getSelectedServices();

    this.router.navigate(['/checkout-page'], {
      queryParams: {
        room: JSON.stringify(room),
        checkinDate: this.filter.checkinDate.toDateString(),
        checkoutDate: this.filter.checkoutDate.toDateString(),
        services: JSON.stringify(servicesString),
        floor: this.filter.floor
      }
    });
  }
}
