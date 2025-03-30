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

  getBookedDates(): Observable<string[]> {
    return this.http.get<string[]>(this.url + '/GetBookedDates', {withCredentials: true});
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
  getGuestsByCheckinDate(): Observable<User[]> {
    return this.http.get<User[]>(this.url + '/GetGuestsByCheckinDate', {withCredentials: true});
  }
}
