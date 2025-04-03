import {Injectable, signal} from '@angular/core';
import {environment} from '../../../environments/environment';
import {User} from '../../shared/models/user.model';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  public url: string = environment.apiBaseUrl + '/Authentication';
  public formUser: User = new User();
  public isFormSubmitted: boolean = false;
  public username = signal<string>('');

  constructor(private http: HttpClient) {
  }

  checkAuthenticated(): Promise<boolean> {
    return new Promise((resolve) => {
      this.http
        .get<{ isAuthenticated: boolean }>(this.url + '/IsAuthenticated', {
          withCredentials: true,
        })
        .subscribe({
          next: (data) => {
            resolve(data.isAuthenticated);
          },
          error: (err) => {
            if(err.status === 401 && sessionStorage.getItem('user') != null) {
              sessionStorage.removeItem('user');
              this.getUsername();
            }
            resolve(false);
          },
        });
    });
  }

  register() {
    console.log(JSON.stringify(this.formUser));
    return this.http
      .post(this.url + '/RegisterAsGuest', this.formUser, { withCredentials: true })
  }

  updateUser() : Observable<User> {
    console.log(JSON.stringify(this.formUser));
    return this.http.put<User>(this.url + '/UpdateUser', this.formUser, { withCredentials: true })
  }

  login() {
    console.log(JSON.stringify(this.formUser));
    return this.http
      .post(this.url + '/Login', this.formUser, { withCredentials: true })
  }



  logout(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.http.delete(this.url + '/Logout', { withCredentials: true }).subscribe({
        next: () => {
          if(sessionStorage.getItem('user') != null)
          {
            sessionStorage.removeItem('user');
            this.getUsername();
            resolve();
          }
        },
        error: (err) => {
          console.log("Logout error: ", err);
          reject(err);
        }
      })
    })
  }

  getUserWithToken(): Promise<string | null>{
    return new Promise((resolve) => {
      this.http.get(this.url + '/GetUser', {withCredentials: true}).subscribe({
        next: (data) => {
          sessionStorage.setItem('user', JSON.stringify(data));
          resolve ((data as User).userRole);
        },
        error: (err) => {
          if(err.status === 401 && sessionStorage.getItem('user') != null) {
            sessionStorage.removeItem('user');
            this.getUsername();
          }
          resolve(null);
          console.log(err);
        }
      })
    })
  }

  getUsername(){
    let user = sessionStorage.getItem('user');
    if(user === null){
      this.username.set("");
      console.log('failed to get user from session storage');
    }
    else{
      const firstName = JSON.parse(user).firstName;
      const lastName = JSON.parse(user).lastName;
      this.username.set(`${firstName} ${lastName}`);
    }
  }
}
