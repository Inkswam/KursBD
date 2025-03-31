import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from '../../../environments/environment';
import {User} from '../../shared/models/user.model';
import {UsersReservations} from '../../shared/wrappers/users-reservations.wrapper';
import {Reservation} from '../../shared/models/reservation.model';
import {Room} from '../../shared/models/room.model';
import {Booking} from '../../shared/wrappers/booking.wrapper';

@Injectable({
  providedIn: 'root'
})
export class ManagementService {

  public url: string = environment.apiBaseUrl + '/Manager';
  constructor(private http: HttpClient) { }

  getBookedDates(): Observable<Date[]> {
    return this.http.get<Date[]>(this.url + '/GetBookedDates', {withCredentials: true});
  }

  getGuestsByDate(date: Date): Observable<UsersReservations> {
    return this.http.get<UsersReservations>(`${this.url}/GetGuestsByDate`,
      {
        params:{
          date: date.toDateString(),
        },
        withCredentials: true
      });
  }
  getGuestsByCheckinDate(date: Date): Observable<UsersReservations> {
    return this.http.get<UsersReservations>(this.url + '/GetGuestsByCheckinDate',
      {
        params:{
          date: date.toDateString(),
        },
        withCredentials: true
      });
  }

  getAllGuestsWithReservations(): Observable<UsersReservations> {
    return this.http.get<UsersReservations>(this.url + '/GetAllGuestsWithReservations', {withCredentials: true});
  }


  getGuestByEmail(guestEmail: string): Observable<User> {
    return this.http.get<User>(`${this.url}/GetGuestByEmail/`,
      {
        params:{
          email: guestEmail
        },
        withCredentials: true
      });
  }

  getReservationsByGuestEmail(guestEmail: string): Observable<Reservation[]> {
    return this.http.get<Reservation[]>(`${this.url}/GetReservationsByGuestEmail`,
      {
        params:{
          email: guestEmail
        },
        withCredentials: true
      });
  }

  getBooking(reservationId: string): Observable<Booking>{
    return this.http.get<Booking>(`${this.url}/GetBooking`, {
      params: {
        reservationId: reservationId,
      },
      withCredentials: true
    });
  }
}
