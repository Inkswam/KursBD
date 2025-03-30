import { Component } from '@angular/core';
import {MatCard, MatCardContent, MatCardTitle} from '@angular/material/card';

@Component({
  selector: 'app-sales-goals',
  imports: [
    MatCard,
    MatCardTitle,
    MatCardContent
  ],
  templateUrl: './sales-goals.component.html',
  styleUrl: './sales-goals.component.scss'
})
export class SalesGoalsComponent {

}
