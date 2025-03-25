import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {Filter} from '../../shared/models/filter.model';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  public url: string = environment.apiBaseUrl + '/Customer';
  public isFormSubmitted: boolean = false;
  constructor(private http: HttpClient) { }


  getAvailableRooms(filter: Filter) {

    return this.http
      .get(this.url + '/GetAvailableRoomTypes', { withCredentials: true })
  }

}
