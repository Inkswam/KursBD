import {ERoomType} from '../enums/eroom-type.enum';

export class Room {
  private _room_type: string;
  private _capacity: number;
  private _price: number;
  private _image_url: string;

  constructor(room_type: string, capacity: number, price: number, image_url: string) {
    this._room_type = room_type;
    this._capacity = capacity;
    this._price = price;
    this._image_url = image_url;
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

  public get image_url(){
    return this._image_url;
  }

  public set image_url(value: string) {
    this._image_url = value;
  }

}
