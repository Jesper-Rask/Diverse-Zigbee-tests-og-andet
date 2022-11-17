var temperatur = [];
var fugtighed =[];
var tidsstempel =[];
var ktemperatur = [];
var kfugtighed = [];
var ktidsstempel = []
var temp;
var fugt;
var navn;
var tid;

function TransferData(tempSove, tempK, fugtSove, fugtK, tidSove, tidK) {
    temperatur = tempSove;
    fugtighed = fugtSove;
    ktemperatur = tempK;
    kfugtighed = fugtK;
    tidsstempel = tidSove;
    ktidsstempel = tidK;
    CreateChart();
}

var grafSove;
var grafVaskerum;

function CreateChart() {
    let chartStatus = Chart.getChart("GrafSoveVærelse"); // <canvas> id
    if (chartStatus != undefined) {
        chartStatus.destroy();
    }

    let chartStatusK = Chart.getChart("GrafVaskerum"); // <canvas> id
    if (chartStatusK != undefined) {
        chartStatusK.destroy();
    }

    grafSove = new Chart("GrafSoveVærelse", {
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
                label: "Temperatur Soveværelse",
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
                label: "Fugtighed Soveværelse",
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
    grafVaskerum = new Chart("GrafVaskerum", {
        type: "line",
        data: {
            labels: ktidsstempel,
            datasets: [{
                borderWidth: 1,
                pointRadius: 0,
                pointHoverRadius: 0,
                fill: false,
                lineTension: 0.4,
                backgroundColor: "rgba(0,255,0,30)",
                borderColor: "rgba(0,255,0,150)",
                data: ktemperatur,
                label: "Temperatur Vaskerum",
                yAxisID: 'ktemperature',
            }, {
                borderWidth: 1,
                fill: false,
                pointRadius: 0,
                pointHoverRadius: 0,
                lineTension: 0.4,
                backgroundColor: "rgba(150,80,0,30)",
                borderColor: "rgba(150,80,0,150)",
                data: kfugtighed,
                label: "Fugtighed Vaskerum",
                yAxisID: 'khumidity'
            }]
        },
        options: {
            scales: {
                ktemperature: {
                    type: 'linear',
                    position: 'left'
                },
                khumidity: {
                    type: 'linear',
                    position: 'right',
                    grid: {
                        drawOnChartArea: false
                    }
                }
            }
        }
    });
}

function AddDataPoint(temperatur, fugtighed, tidsstempel, name) {

    if (name == "Sensor Soveværelse") {
        this.tidsstempel.shift();
        this.temperatur.shift();
        this.fugtighed.shift();
        this.temperatur.push(temperatur);
        this.fugtighed.push(fugtighed);
        this.tidsstempel.push(tidsstempel);
    }

    if (name == "Sensor Vaskerum") {
        this.ktemperatur.shift();
        this.kfugtighed.shift();
        this.ktidsstempel.shift();
        this.ktemperatur.push(temperatur);
        this.kfugtighed.push(fugtighed);
        this.ktidsstempel.push(tidsstempel);         
    }
}

function Upd() {
    grafSove.update();
    grafVaskerum.update();
}




