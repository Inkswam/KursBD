import {EPaymentMethod} from '../enums/epayment-method.enum';

export class Payment {
  public id: string = "";
  public reservationId: string = "";
  public date: Date = new Date();
  public amount: number = 0;
  public method: string = EPaymentMethod.Cash;

}
