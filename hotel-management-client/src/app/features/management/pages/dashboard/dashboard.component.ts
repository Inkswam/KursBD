import {Component, OnInit} from '@angular/core';
import {MatGridList, MatGridTile} from "@angular/material/grid-list";
import {OverviewComponent} from '../../components/overview/overview.component';
import {CalendarComponent} from '../../components/calendar/calendar.component';
import {ManagementService} from '../../management.service';
import {SalesGoalsComponent} from '../../components/sales-goals/sales-goals.component';
import {GuestListComponent} from '../../components/guest-list/guest-list.component';

@Component({
  selector: 'app-dashboard',
  imports: [
    MatGridList,
    MatGridTile,
    OverviewComponent,
    CalendarComponent,
    SalesGoalsComponent,
    GuestListComponent
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {

  constructor(public managementService: ManagementService) {
  }

  ngOnInit() {
  }
}
