import {Component, Input, Output, EventEmitter} from '@angular/core';
import {MatCard} from '@angular/material/card';
import {MatChip} from '@angular/material/chips';
import {NgClass, NgForOf, NgOptimizedImage} from '@angular/common';
import {MatButton} from '@angular/material/button';
import {Room} from '../../../../shared/models/room.model';
import {environment} from '../../../../../environments/environment';

@Component({
  selector: 'app-room-card',
  imports: [
    MatCard,
    MatChip,
    NgClass,
    MatButton,
    NgForOf,
    NgOptimizedImage,
  ],
  templateUrl: './room-card.component.html',
  styleUrl: './room-card.component.scss'
})
export class RoomCardComponent {
  @Output() outRoom = new EventEmitter<Room>();
  @Input() room!: Room;

  constructor() {
  }

  returnRoom(){
    this.outRoom.emit(this.room);
  }

  protected readonly environment = environment;
}
