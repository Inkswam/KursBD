import {Component, inject, signal} from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatCard, MatCardContent, MatCardHeader } from '@angular/material/card';
import {
  MatFormFieldModule,
  MatLabel,
  MatSuffix,
} from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAnchor, MatButton, MatIconButton } from '@angular/material/button';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule, NgForm, NgModel } from '@angular/forms';
import { MatGridList, MatGridTile } from '@angular/material/grid-list';
import {
  MatDatepickerInput,
  MatDatepickerModule,
  MatDatepickerToggle,
} from '@angular/material/datepicker';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatIcon } from '@angular/material/icon';
import {AuthenticationService} from '../../authentication.service';
import {User} from '../../../../shared/models/user.model';
import {MatSnackBar} from '@angular/material/snack-bar';

@Component({
  selector: 'app-register',
  providers: [provideNativeDateAdapter()],
  imports: [
    MatToolbarModule,
    MatCard,
    MatCardContent,
    MatFormFieldModule,
    MatLabel,
    MatInputModule,
    MatButton,
    MatAnchor,
    FormsModule,
    MatGridList,
    MatGridTile,
    MatDatepickerInput,
    MatDatepickerToggle,
    MatSuffix,
    MatDatepickerModule,
    MatCardHeader,
    MatIconButton,
    MatIcon,
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  readonly _currentDate = new Date();
  hidePassword = signal(true);
  confirmPassword = '';
  private _snackBar = inject(MatSnackBar);

  constructor(
    public service: AuthenticationService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    service.formUser.password = "123456";
    service.formUser.firstName = "John";
    service.formUser.lastName = "Doe";
    service.formUser.email = "john@doe.com";
    service.formUser.birthDate = new Date(2005, 1, 6, 0, 0, 0, 0);
    service.formUser.phoneNumber = "0674527417";
  }

  changeButtonVisibility(event: MouseEvent) {
    this.hidePassword.set(!this.hidePassword());
    event.stopPropagation();
  }
  isPasswordMatch(
    event: FocusEvent,
    password: NgModel,
    confirmPassword: NgModel
  ) {
    if (password.value != confirmPassword.value) {
      password.control.setErrors({ mismatch: true });
      confirmPassword.control.setErrors({ mismatch: true });
    } else {
      password.control.setErrors(null);
      confirmPassword.control.setErrors(null);
    }
    event.stopPropagation();
  }
  onSubmit(form: NgForm) {
    this.service.isFormSubmitted = true;
    if (form.valid) {
      this.service.register().subscribe({
        next: (data) => {
          console.log(data);
          this.service.formUser = new User();
          sessionStorage.setItem('user', JSON.stringify(data));
          this.service.getUsername();

          const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') || `/${(data as User).userRole}`;
          this.router.navigateByUrl(returnUrl);
        },
        error: (error) => {
          this._snackBar.open(error.message, 'Understood');
          console.log(error);
        },
      });
    }
  }
  goToLogin(){
    const savedQuery = this.route.snapshot.queryParamMap.get('returnUrl');
    return this.router.navigate(['/login'], {queryParams: {returnUrl: savedQuery}});
  }
}
