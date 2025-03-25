import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {Filter} from '../../shared/models/filter.model';
import {Observable} from 'rxjs';
import {Room} from '../../shared/models/room.model';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  public url: string = environment.apiBaseUrl + '/Customer';
  public isFormSubmitted: boolean = false;
  constructor(private http: HttpClient) { }


  getAvailableRooms(filter: Filter) : Observable<string[]> {
    return this.http.get<string[]>(this.url + '/GetAvailableRoomTypes', {
      params: {
        roomType: filter.roomType,
        checkIn: filter.checkinDate.toISOString().split('T')[0],
        checkOut: filter.checkinDate.toISOString().split('T')[0],
        floor: filter.floor.toString()
      },
      withCredentials: true
    });
  }

}
