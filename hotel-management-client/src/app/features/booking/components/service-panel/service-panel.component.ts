import {Component, Input} from '@angular/core';
import {
  MatExpansionPanel,
  MatExpansionPanelDescription,
  MatExpansionPanelHeader,
  MatExpansionPanelTitle
} from '@angular/material/expansion';
import {MatCheckbox} from '@angular/material/checkbox';
import {Service} from '../../../../shared/models/service.model';
import {BookingService} from '../../booking.service';

@Component({
  selector: 'app-service-panel',
  imports: [
    MatExpansionPanel,
    MatExpansionPanelHeader,
    MatExpansionPanelTitle,
    MatExpansionPanelDescription,
    MatCheckbox
  ],
  templateUrl: './service-panel.component.html',
  styleUrl: './service-panel.component.scss'
})
export class ServicePanelComponent {
  @Input() service!: Service;

  constructor(public bookingService: BookingService) {
  }

}
