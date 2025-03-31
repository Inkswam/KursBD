import {Reservation} from '../models/reservation.model';
import {Payment} from '../models/payment.model';
import {Room} from '../models/room.model';
import {Service} from '../models/service.model';

export class Booking {
  public reservation: Reservation;
  public payment: Payment;
  public room: Room;
  public services: Service[];

  constructor() {
    this.services = [];
    this.reservation = new Reservation();
    this.payment = new Payment();
    this.room = new Room('', 0, 0, '');
  }
}
