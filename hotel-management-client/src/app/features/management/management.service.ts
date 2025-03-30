import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from '../../../environments/environment';
import {User} from '../../shared/models/user.model';
import {UsersReservations} from '../../shared/wrappers/users-reservations.wrapper';

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
}
