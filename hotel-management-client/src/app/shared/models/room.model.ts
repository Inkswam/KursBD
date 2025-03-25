import {ERoomType} from '../enums/eroom-type.enum';

export class Room {
  private _room_type: string;
  private _capacity: number;
  private _price: number;

  constructor(room_type: string, capacity: number, price: number) {
    this._room_type = room_type;
    this._capacity = capacity;
    this._price = price;
  }

  public get room_type(){
    return this._room_type;
  }

  public set room_type(value: string) {
    this._room_type = value;
  }

  public get capacity(){
    return this._capacity;
  }

  public set capacity(value: number) {
    this._capacity = value;
  }

  public get price(){
    return this._price;
  }

  public set price(value: number) {
    this._price = value;
  }

}
