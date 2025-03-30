import {User} from '../models/user.model';
import {Reservation} from '../models/reservation.model';

export class UsersReservations {
  public users: User[] = [];
  public reservations: Reservation[] = [];
}
