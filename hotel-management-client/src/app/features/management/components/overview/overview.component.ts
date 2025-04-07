import {AfterViewInit, Component, ElementRef, ViewChild} from '@angular/core';
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
import {Chart, ChartDataset} from 'chart.js/auto';
import {ChartData} from '../../../../shared/models/chart-data.model';
import {ChartStatistic} from '../../../../shared/wrappers/chart-statistic.wrapper';

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
export class OverviewComponent implements AfterViewInit {
  @ViewChild('guestsCanvas') guestsCanvas: ElementRef | undefined;
  @ViewChild('earningsCanvas') earningsCanvas: ElementRef | undefined;

  guestsChart: any;
  earningsChart: any;

  guestChartData: ChartStatistic;
  earningsChartData: ChartStatistic;

  public currentDate: Date;

  public campaignOne = new FormGroup({
    start: new FormControl(new Date()),
    end: new FormControl(new Date()),
  });

  public chartData: Date[];

  constructor(public managementService: ManagementService) {
    this.guestChartData = new ChartStatistic();
    this.earningsChartData = new ChartStatistic();

    this.chartData = [];
    this.currentDate = new Date();
    let end = this.campaignOne.get('end')?.value;
    if(end != undefined){
      let startDate = new Date();
      startDate.setDate(end?.getDate() - 7);
      this.campaignOne.get('start')?.setValue(startDate);

      let date = new Date(startDate);
      while (date <= end){
        this.chartData.push(date);
        date.setDate(date.getDate() + 1);
      }
    }
  }

  ngAfterViewInit(): void {
    this.guestsBarChartMethod();
    this.earningsBarChartMethod();
    this.updateGuestChart("Week");
    this.updateEarningsChart();
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

  guestsBarChartMethod(){
    this.guestsChart = new Chart(this.guestsCanvas?.nativeElement, {
      type: 'bar',
      options: {
        animation: false,
        plugins: {
          legend: {
            display: false
          },
          tooltip: {
            enabled: false
          }
        }
      },
      data: {
        labels: this.guestChartData.chartsData.map(row => row.type),
        datasets: [
          {
            label: 'Acquisitions by year',
            data: this.guestChartData.chartsData.map(row => row.value),
            backgroundColor: 'rgba(54, 162, 235, 0.7)'
          }
        ]
      }
    })
  }

  earningsBarChartMethod(){
    this.earningsChart = new Chart(this.earningsCanvas?.nativeElement, {
      type: 'bar',
      options: {
        animation: false,
        plugins: {
          legend: {
            display: false
          },
          tooltip: {
            enabled: false
          }
        }
      },
      data: {
        labels: this.earningsChartData.chartsData.map(row => row.type),
        datasets: [
          {
            label: '',
            data: this.earningsChartData.chartsData.map(row => row.value),
            backgroundColor: 'rgba(75, 192, 192, 0.7)'
          }
        ]
      }
    })
  }

  updateGuestChart(value: string) {
    this.managementService.getGuestChart(value).subscribe({
      next: data => {
        this.guestChartData.chartsData = [];
        for(let cd of data.chartsData) {
          this.guestChartData.chartsData.push(cd);
        }
        this.guestChartData.valueSum = data.valueSum;
        this.guestChartData.percentage = data.percentage;

        this.guestsChart.destroy()  ;
        console.log(this.guestChartData);

        this.guestsBarChartMethod()
      },
      error: err => console.log(err)
    })
  }

  updateEarningsChart() {
    let startDate = this.campaignOne.get('start')?.value;
    let endDate = this.campaignOne.get('end')?.value;

    if(startDate == null ||
      endDate == null){
      console.log("dates are undefined or null")
      return
    }

    this.managementService.getEarningsChart(startDate, endDate).subscribe({
      next: data => {
        this.earningsChartData.chartsData = [];
        for(let cd of data.chartsData) {
          this.earningsChartData.chartsData.push(cd);
        }
        this.earningsChartData.valueSum = data.valueSum;
        this.earningsChartData.percentage = data.percentage;

        this.earningsChart.destroy();
        console.log(this.earningsChartData);

        this.earningsBarChartMethod();
      },
      error: err => console.log(err)
    })
  }
}
