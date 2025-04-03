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
import {ReservationsPageComponent} from './features/management/pages/reservations-page/reservations-page.component';
import {GuestPageComponent} from './features/management/pages/guest-page/guest-page.component';
import {ReservationPageComponent} from './features/management/pages/reservation-page/reservation-page.component';
import {BlacklistPageComponent} from './features/management/pages/blacklist-page/blacklist-page.component';
import {
  AdministratorMainPageComponent
} from './features/administration/pages/administrator-main-page/administrator-main-page.component';
import {
  ReceptionistManagementPageComponent
} from './features/administration/pages/receptionist-management-page/receptionist-management-page.component';
import {AccountPageComponent} from './core/account/pages/account-page/account-page.component';
import {AccountDetailsComponent} from './core/account/pages/account-details/account-details.component';
import {
  GuestReservationsPageComponent
} from './core/account/pages/guest-reservations-page/guest-reservations-page.component';

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
      {
        path: 'guest',
        component: GuestPageComponent,
        canActivate: [authenticationGuard],
        title: 'Guest',
      },
      {
        path: 'reservations',
        component: ReservationsPageComponent,
        canActivate: [authenticationGuard],
        title: 'Bookings',
      },
      {
        path: 'reservation',
        component: ReservationPageComponent,
        canActivate: [authenticationGuard],
        title: 'Booking',
      },
      {
        path: 'blacklist',
        component: BlacklistPageComponent,
        canActivate: [authenticationGuard],
        title: 'Blacklist',
      },
    ]
  },
  {
    path: 'Administrator',
    component: AdministratorMainPageComponent,
    canActivate: [authenticationGuard],
    title: 'Administrator',
    children:[
      {
        path: 'users',
        component: ReceptionistManagementPageComponent,
        canActivate: [authenticationGuard],
        title: 'Users',
      }
    ]
  },
  {
    path: 'account',
    component: AccountPageComponent,
    canActivate: [authenticationGuard],
    title: 'Account',
    children:[
      {
        path: 'account-details',
        component: AccountDetailsComponent,
        canActivate: [authenticationGuard],
        title: 'Account Details',
      },
      {
        path: 'reservations',
        component: GuestReservationsPageComponent,
        canActivate: [authenticationGuard],
        title: 'My reservations',
      }
    ]
  }
];
