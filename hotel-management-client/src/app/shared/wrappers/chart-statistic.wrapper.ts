import {ChartData} from '../models/chart-data.model';

export class ChartStatistic {
  public chartsData: ChartData[] = [];
  public valueSum: number = 0;
  public percentage: number = 0;
}
