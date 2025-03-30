import { Component } from '@angular/core';
import {MatCard, MatCardContent, MatCardTitle} from '@angular/material/card';
import {MatFormField, MatOption, MatSelect} from '@angular/material/select';

@Component({
  selector: 'app-overview',
  imports: [
    MatCard,
    MatCardTitle,
    MatCardContent,
    MatSelect,
    MatOption,
    MatFormField
  ],
  templateUrl: './overview.component.html',
  styleUrl: './overview.component.scss'
})
export class OverviewComponent {

}
