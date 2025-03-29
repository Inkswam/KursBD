import {Service} from './service.model';

export class Reservation {
  public id: string = "";
  public guestEmail: string = "";
  public roomNumber: number = 0;
  public roomType: string = "";
  public checkinDate: Date = new Date();
  public checkoutDate: Date = new Date();
  public status: string = "";
}
