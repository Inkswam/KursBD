import { Component } from '@angular/core';
import {MatCard, MatCardContent, MatCardTitle} from '@angular/material/card';
import {MatFormField, MatHint, MatLabel, MatOption, MatSelect, MatSuffix} from '@angular/material/select';
import {ManagementService} from '../../management.service';
import {saveAs} from 'file-saver';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule} from '@angular/forms';
import {
  MatDatepickerToggle,
  MatDateRangeInput, MatDateRangePicker, MatEndDate,
  MatStartDate
} from '@angular/material/datepicker';
import {NgxChartsModule, Color} from '@swimlane/ngx-charts';

@Component({
  selector: 'app-overview',
  imports: [
    MatCard,
    MatCardTitle,
    MatCardContent,
    MatSelect,
    MatOption,
    MatFormField,
    FormsModule,
    MatDatepickerToggle,
    MatHint,
    MatLabel,
    MatSuffix,
    MatDateRangeInput,
    MatStartDate,
    MatDateRangePicker,
    MatEndDate,
    ReactiveFormsModule,
    NgxChartsModule
  ],
  templateUrl: './overview.component.html',
  styleUrl: './overview.component.scss'
})
export class OverviewComponent {

  public currentDate: Date;

  public campaignOne = new FormGroup({
    start: new FormControl(new Date()),
    end: new FormControl(new Date()),
  });

  public chartData: Date[];

  colorScheme: string | Color = '#00796b';

  constructor(public managementService: ManagementService) {
    this.chartData = [];
    this.currentDate = new Date();
    let end = this.campaignOne.get('end')?.value;
    if(end != undefined){
      let startDate = new Date();
      startDate.setDate(end?.getDate() - 7);
      let start = this.campaignOne.get('start')?.setValue(startDate);

      let date = startDate;
      while (date <= end){
        this.chartData.push(date);
        date.setDate(date.getDate() + 1);
      }
    }
  }

  downloadReport(){
    let startDate = this.campaignOne.get('start')?.value;
    let endDate = this.campaignOne.get('end')?.value;

    if(startDate == null ||
      endDate == null){
      console.log("dates are undefined or null")
      return
    }

    this.managementService.getFinancialReport(startDate, endDate).subscribe({
      next: data => {
        saveAs(data, 'Financial Report.csv');
      },
      error: err => console.log(err)
    })
  }
}
