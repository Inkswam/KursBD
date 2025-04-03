import {Component, inject, OnInit, signal} from '@angular/core';
import {FormsModule, NgForm, NgModel, ReactiveFormsModule} from "@angular/forms";
import {MatCard, MatCardContent, MatCardHeader} from "@angular/material/card";
import {MatDatepicker, MatDatepickerInput, MatDatepickerToggle} from "@angular/material/datepicker";
import {MatGridList, MatGridTile} from "@angular/material/grid-list";
import {MatInput, MatLabel, MatSuffix} from "@angular/material/input";
import {MatSnackBar} from '@angular/material/snack-bar';
import {AuthenticationService} from '../../../authentication/authentication.service';
import {ActivatedRoute, Router} from '@angular/router';
import {User} from '../../../../shared/models/user.model';
import {MatButton} from '@angular/material/button';
import {MatFormField} from '@angular/material/form-field';
import {MatHint} from '@angular/material/select';

@Component({
  selector: 'app-account-details',
    imports: [
        FormsModule,
        MatButton,
        MatCard,
        MatCardContent,
        MatCardHeader,
        MatDatepicker,
        MatDatepickerInput,
        MatDatepickerToggle,
        MatFormField,
        MatGridList,
        MatGridTile,
        MatHint,
        MatInput,
        MatLabel,
        MatSuffix,
        ReactiveFormsModule
    ],
  templateUrl: './account-details.component.html',
  styleUrl: './account-details.component.scss'
})
export class AccountDetailsComponent implements OnInit {

  readonly _currentDate = new Date();
  private _snackBar = inject(MatSnackBar);
  public user: User;

  constructor(
    public service: AuthenticationService,
    public router: Router,
  ) {
    this.user = new User();
  }

  ngOnInit() {
    let stringUser = sessionStorage.getItem('user');
    if (stringUser != null) {
      this.user = JSON.parse(stringUser);
    }

    this.service.formUser.email = this.user.email;
    this.service.formUser.phoneNumber = this.user.phoneNumber;
    this.service.formUser.firstName = this.user.firstName;
    this.service.formUser.lastName = this.user.lastName;
    this.service.formUser.birthDate = this.user.birthDate;
    this.service.formUser.userRole = this.user.userRole;
  }

  onSubmit(form: NgForm) {
    this.service.isFormSubmitted = true;
    if (form.valid) {
      this.service.updateUser().subscribe({
        next: (data) => {
          console.log(data);
          this.service.formUser = data;
          sessionStorage.setItem('user', JSON.stringify(data));
          this.service.getUsername();

          this._snackBar.open("User Updated Successfully", "Ok");
        },
        error: (error) => {
          this._snackBar.open(error.message, 'Understood');
          console.log(error);
        },
      });
    }
  }

  logOut(){
    this.service.logout()
      .then(() => {
        this.router.navigate(['/']);
        this._snackBar.open("User Logged Out", "Ok");
      })
      .catch((error) => console.log(error));
  }
}
