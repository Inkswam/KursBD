import {Component, Input} from '@angular/core';
import {MatCard} from '@angular/material/card';
import {MatChip} from '@angular/material/chips';
import {NgClass, NgForOf, NgOptimizedImage} from '@angular/common';
import {MatButton} from '@angular/material/button';
import {Room} from '../../../../shared/models/room.model';

@Component({
  selector: 'app-room-card',
  imports: [
    MatCard,
    MatChip,
    NgClass,
    MatButton,
    NgForOf,
    NgOptimizedImage
  ],
  templateUrl: './room-card.component.html',
  styleUrl: './room-card.component.scss'
})
export class RoomCardComponent {
  @Input() room!: Room;

  constructor() {
  }
}
