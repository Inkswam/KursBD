import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {User} from '../../shared/models/user.model';
import {environment} from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  public url: string = environment.apiBaseUrl + '/Administrator';
  constructor(private http: HttpClient) {

  }

  getAllUsers() : Observable<User[]> {
    return this.http.get<User[]>(`${this.url}/GetAllUsers`, {withCredentials: true});
  }

  promoteToManager(email: string) : Observable<User> {
    return this.http.put<User>(`${this.url}/PromoteToManager`, {}, {
      params: {
        email: email
      },
      withCredentials: true
    });
  }
  degradeToGuest(email: string) : Observable<User> {
    return this.http.put<User>(`${this.url}/DegradeToGuest`, {}, {
      params: {
        email: email
      },
    withCredentials: true
    });
  }
}
