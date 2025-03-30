import { Routes } from '@angular/router';
import {HomePageComponent} from './core/home/home-page/home-page.component';
import {LoginComponent} from './core/authentication/pages/login/login.component';
import {RegisterComponent} from './core/authentication/pages/register/register.component';
import {RoomSearchPageComponent} from './features/booking/pages/room-search-page/room-search-page.component';
import {CheckoutPageComponent} from './features/booking/pages/checkout-page/checkout-page.component';
import {authenticationGuard} from './core/authentication/authentication.guard';
import {
  ReceptionistMainPageComponent
} from './features/management/pages/receptionist-main-page/receptionist-main-page.component';
import {DashboardComponent} from './features/management/pages/dashboard/dashboard.component';
import {GuestsPageComponent} from './features/management/pages/guests-page/guests-page.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'Guest',
    pathMatch: 'full',
  },
  {
    path: 'Guest',
    component: HomePageComponent,
    title: 'Home',
  },
  {
    path: 'login',
    component: LoginComponent,
    title: 'Login',
  },
  {
    path: 'register',
    component: RegisterComponent,
    title: 'Register',
  },
  {
    path: 'room-search-page',
    component: RoomSearchPageComponent,
    title: 'Room Search Page',
  },
  {
    path: 'checkout-page',
    component: CheckoutPageComponent,
    canActivate: [authenticationGuard],
    title: 'Checkout Page',
  },
  {
    path: 'Receptionist',
    component: ReceptionistMainPageComponent,
    canActivate: [authenticationGuard],
    title: 'Management Home',
    children: [
      {
        path: 'dashboard',
        component: DashboardComponent,
        canActivate: [authenticationGuard],
        title: 'Dashboard',
      },
      {
        path: 'guests',
        component: GuestsPageComponent,
        canActivate: [authenticationGuard],
        title: 'Guests',
      },
    ]
  }
];
