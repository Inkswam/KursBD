import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {Filter} from '../../shared/models/filter.model';
import {Observable} from 'rxjs';
import {Room} from '../../shared/models/room.model';
import {Service} from '../../shared/models/service.model';
import {Reservation} from '../../shared/models/reservation.model';
import {Payment} from '../../shared/models/payment.model';
import {Booking} from '../../shared/wrappers/booking.wrapper';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  public url: string = environment.apiBaseUrl + '/Customer';
  public isFormSubmitted: boolean = false;
  private services: Service[] = [];
  constructor(private http: HttpClient) {
  }


  getAvailableRooms(filter: Filter) : Observable<Room[]> {
    return this.http.get<Room[]>(this.url + '/GetAvailableRoomTypes', {
      params: {
        roomType: filter.roomType,
        checkIn: filter.checkinDate.toDateString(),
        checkOut: filter.checkinDate.toDateString(),
        floor: filter.floor.toString()
      },
      withCredentials: true
    });
  }

  getServices() : Observable<Service[]> {
    return this.http.get<Service[]>(this.url + '/GetServices', {withCredentials: true});
  }

  updateService(service: Service, state: boolean) {
    if (state) {
      this.services.push(service);
    }
    else{
      const index = this.services.indexOf(service);
      if (index > -1) {
        this.services.splice(index, 1);
      }
    }
  }

  getSelectedServices(){
    let returnServices: Service[] = [];
    this.services.forEach(service => {returnServices.push(service)});
    this.services =[];
    return returnServices;
  }

  bookRoom(reservation: Reservation, payment: Payment, services: Service[], roomFloor: number) {
    return this.http.post(this.url + '/PlaceReservation', {reservation, payment, roomFloor, services}, {withCredentials: true});
  }

  getReservationsByUser(email: string) : Observable<Booking[]> {
    return this.http.get<Booking[]>(`${this.url}/GetReservationsByUser/${email}`, {withCredentials: true});
  }
}
