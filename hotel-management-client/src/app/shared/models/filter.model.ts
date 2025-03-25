import {ERoomType} from '../enums/eroom-type.enum';

export class Filter {
  private _roomType: string;
  private _checkinDate: Date = new Date();
  private _checkoutDate: Date = new Date();
  private _floor: number = 1

  constructor() {
    this._roomType = ERoomType.Any
    this._checkinDate = new Date();
    this._checkoutDate.setDate(this.checkinDate.getDate() + 1);
    this._floor = 1;
  }

  public get roomType(): string {
    return this._roomType;
  }

  public set roomType(value: string) {
    this._roomType = value;
  }

  public get checkinDate() {
    return this._checkinDate;
  }

  public set checkinDate(checkinDate: Date) {
    this._checkinDate = checkinDate;
    if(this._checkinDate > this._checkoutDate) {
      this._checkoutDate = checkinDate;
    }
  }
  public get checkoutDate() {
    return this._checkoutDate;
  }

  public set checkoutDate(checkoutDate: Date) {
    this._checkoutDate = checkoutDate;
    if(this._checkinDate > this._checkoutDate) {
      this._checkinDate = checkoutDate;
    }
  }

  public get floor() {
    return this._floor;
  }

  public set floor(numberOfGuests: number) {
    this._floor = numberOfGuests;
  }
}
