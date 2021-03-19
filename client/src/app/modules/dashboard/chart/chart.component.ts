import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { BaseChartDirective, Color, Label } from 'ng2-charts';


@Component({
    selector: 'app-chart',
    templateUrl: './chart.component.html',
    styleUrls: ['./chart.component.scss'],
})
export class ChartComponent implements OnInit {

    @Input() data: any[] = [];
    @Input() labels: any[] = [];
    @ViewChild(BaseChartDirective, { static: true }) chart: BaseChartDirective;

    public lineChartType: ChartType = 'line';
    public lineChartData: ChartDataSets[] = [];
    public lineChartLabels: Label[] = [];
    public lineChartOptions: (ChartOptions) = {
        legend: {
            display: false,
        },
        scales: {
            xAxes: [{
                display: false,
            }],
            yAxes: [{
                display: false,
            }],
        },
        elements: {
            point: {
                radius: 0,
            },
        },
        tooltips: {
            enabled: true,
            displayColors: false,
        },
        responsive: false,
    };

    public lineChartColors: Color[] = [];


    constructor() { }

    ngOnInit(): void {
        this.lineChartData = [{ data: this.data }];
        this.lineChartLabels = this.labels;
        if (this.data[this.data.length - 1] > 0) {
            this.setChartPositive();
        } else {
            this.setChartNegative();
        }
    }

    public chartHovered({ event, active }: { event: MouseEvent, active: {}[] }): void {
        console.log(event, active);
    }


    private setChartPositive() {
        this.lineChartColors = [
            {
                backgroundColor: 'rgb(125,183,129)',
                borderColor: 'rgb(31,124,39)',
                pointBackgroundColor: 'rgba(148,159,177,1)',
                pointBorderColor: '#fff',
                pointHoverBackgroundColor: '#fff',
                pointHoverBorderColor: 'rgba(148,159,177,0.8)',
            },
        ];
    }

    private setChartNegative() {
        this.lineChartColors = [
            {
                backgroundColor: 'rgb(217,114,114)',
                borderColor: 'rgba(227,46,46)',
                pointBackgroundColor: 'rgba(148,159,177,1)',
                pointBorderColor: '#fff',
                pointHoverBackgroundColor: '#fff',
                pointHoverBorderColor: 'rgba(148,159,177,0.8)',
            },
        ];
    }

}
