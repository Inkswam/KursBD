import {Component, inject, signal} from '@angular/core';
import { FormsModule, NgForm, ReactiveFormsModule } from '@angular/forms';
import { MatAnchor, MatButton, MatIconButton } from '@angular/material/button';
import { MatCard, MatCardContent, MatCardHeader } from '@angular/material/card';
import {
  MatFormField,
  MatLabel,
  MatSuffix,
} from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatInput } from '@angular/material/input';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import {AuthenticationService} from '../../authentication.service';
import {User} from '../../../../shared/models/user.model';
import {MatSnackBar} from '@angular/material/snack-bar';

@Component({
  selector: 'app-login',
  imports: [
    FormsModule,
    MatAnchor,
    MatButton,
    MatCard,
    MatCardContent,
    MatCardHeader,
    MatFormField,
    MatIcon,
    MatIconButton,
    MatInput,
    MatLabel,
    MatSuffix,
    ReactiveFormsModule,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  hidePassword = signal(true);
  private _snackBar = inject(MatSnackBar);
  constructor(
    public service: AuthenticationService,
    private router: Router,
    private route: ActivatedRoute
  ) {}
  changeButtonVisibility(event: MouseEvent) {
    this.hidePassword.set(!this.hidePassword());
    event.stopPropagation();
  }
  onSubmit(form: NgForm) {
    this.service.isFormSubmitted = true;
    if (form.valid) {
      this.service.login().subscribe({
        next: (data) => {
          console.log(data);
          this.service.formUser = new User();
          sessionStorage.setItem('user', JSON.stringify(data));
          this.service.getUsername();

          const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') || `/${(data as User).userRole}`;
          this.router.navigateByUrl(returnUrl);
        },
        error: (err) => {
          this._snackBar.open(err.message, 'Understood');
          console.log(err);
        },
      });
    }
  }

  goToRegister(){
    const savedQuery = this.route.snapshot.queryParamMap.get('returnUrl');
    this.router.navigate(['/register'], {queryParams: {returnUrl: savedQuery}});
  }
}
