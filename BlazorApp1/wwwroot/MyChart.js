var temperatur = [];
var fugtighed =[];
var tidsstempel =[];
var graf;
var temp;
var fugt;
var navn;
var tid;

function TransferData(chartName, tempList, fugtList, tidList) {
    temperatur = tempList;
    fugtighed = fugtList;
    tidsstempel = tidList;
    navn = chartName;

    CreateChart();
}

function CreateChart() {
    let chartStatus = Chart.getChart("Graf");
    if (chartStatus != undefined) {
        chartStatus.destroy();
    }

    graf = new Chart("Graf", {
        type: "line",
        data: {
            labels: tidsstempel,
            datasets: [{
                borderWidth: 1,
                pointRadius: 0,
                pointHoverRadius: 0,
                fill: false,
                lineTension: 0.4,
                backgroundColor: "rgba(200,40,0,30)",
                borderColor: "rgba(200,80,0,150)",
                data: temperatur,
                label: "Temperatur",
                yAxisID: 'temperature',
            }, {
                borderWidth: 1,
                pointRadius: 0,
                pointHoverRadius: 0,
                fill: false,
                lineTension: 0.4,
                backgroundColor: "rgba(0,40,150,30)",
                borderColor: "rgba(0,80,150,150)",
                data: fugtighed,
                label: "Fugt",
                yAxisID: 'humidity',
            }]
        },
        options: {
            scales: {
                temperature: {
                    type: 'linear',
                    position: 'left'
                },
                humidity: {
                    type: 'linear',
                    position: 'right',
                    grid: {
                        drawOnChartArea: false
                    },      
                }
            }
        }
    });
}

function AddDataPoint(temperatur, fugtighed, tidsstempel, name) {
    if (name == navn) {
        this.tidsstempel.shift();
        this.temperatur.shift();
        this.fugtighed.shift();
        this.temperatur.push(temperatur);
        this.fugtighed.push(fugtighed);
        this.tidsstempel.push(tidsstempel);
        Upd();
    }
}

function Upd() {
    graf.update();
}




