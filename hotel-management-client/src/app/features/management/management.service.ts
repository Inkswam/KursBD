import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from '../../../environments/environment';
import {User} from '../../shared/models/user.model';
import {UsersReservations} from '../../shared/wrappers/users-reservations.wrapper';
import {Reservation} from '../../shared/models/reservation.model';
import {Booking} from '../../shared/wrappers/booking.wrapper';
import {ChartData} from '../../shared/models/chart-data.model';
import {ChartStatistic} from '../../shared/wrappers/chart-statistic.wrapper';

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

  getFinancialReport(startDate: Date, endDate: Date){
    const dateRange = [startDate.toDateString(), endDate.toDateString()];
    return this.http.get(`${this.url}/GetFinancialReport`, {
      params: {
        dateRange: dateRange,
      },
      withCredentials: true,
      responseType: 'blob'
    });
  }

  getFreeRooms(startDate: Date, endDate: Date, roomType: string): Observable<number[]> {
    return this.http.get<number[]>(`${this.url}/GetFreeRooms/${startDate.toDateString()}/${endDate.toDateString()}/${roomType}`,
      {withCredentials: true});
  }

  setNewRoom(reservationId: string, roomNumber: number) : Observable<number>{
    return this.http.put<number>(`${this.url}/SetNewRoom/${reservationId}/${roomNumber}`,{},
      {withCredentials: true});
  }

  getBlacklistedUsers() : Observable<User[]> {
    return this.http.get<User[]>(`${this.url}/GetBlacklistedUsers`, {withCredentials: true});
  }

  removeUserFromBlacklist(email: string) {
    return this.http.delete(`${this.url}/RemoveFromBlacklist/${email}`, {withCredentials: true});
  }

  addUserToBlacklist(email: string) : Observable<User> {
    return this.http.post<User>(`${this.url}/AddUserToBlacklist/${email}`, {}, {withCredentials: true});
  }

  getGuestChart(period: string): Observable<ChartStatistic> {
    return this.http.get<ChartStatistic>(`${this.url}/GetGuestChart`,
    {
      params: {
        period: period,
      },
      withCredentials: true
    });
  }

  getEarningsChart(startDate: Date, endDate: Date): Observable<ChartStatistic> {
    return this.http.get<ChartStatistic>(`${this.url}/GetEarningsChart/${startDate.toDateString()}/${endDate.toDateString()}`,
    {
      withCredentials: true
    })
  }
}
