$(document).ready(function() {
 

  var options1 = {
      chart: {
          type: 'bar',
          width: '100%',
          height: 130,
          sparkline: {
              enabled: true
          }
      },
      fill: {
        colors: ['#1A73E8', '#B32824']
      },
      plotOptions: {
          bar: {
              columnWidth: '20%',
              endingShape: 'rounded'
          }
      },
      colors: ['#7367F0'],
      series: [{
          data: [25, 66, 41, 89, 63, 25]
      }],
      labels: [1, 2, 3, 4, 5, 6],
      xaxis: {
          crosshairs: {
              width: 1
          },
      },
      tooltip: {
          fixed: {
              enabled: false
          },
          x: {
              show: false
          },
          y: {
              title: {
                  formatter: function(seriesName) {
                      return ''
                  }
              }
          },
          marker: {
              show: false
          }
      }
  }

 
  new ApexCharts(document.querySelector("#chart1"), options1).render();

  
  var options2 = {
    series: [44, 55, 41],
    labels: ['Apple', 'Mango', 'Orange'],
    chart: {
      type: 'donut',
      height: 150,
      width: '100%',
    },
    
    dataLabels:{
      enabled: false,
    },
    plotOptions: {
      pie: {
        donut: {
          labels: {
            show: true,
            name: {
              show: false,
            },
            value: {
              show: true,
            },
            total: {
              show: true,
              label: 'Total',
              formatter: function (w) {
                return w.globals.seriesTotals.reduce((a, b) => {
                  return a + b
                }, 0)
              }
            }
          }
        }
      }
    },
    responsive: [{
      breakpoint: 1309,
      options: {
        chart: {
          width: "100%"
        },
        legend: {
          position: 'bottom',
          show: false,
          fontSize: '14px',
        }
      }
    }]
  };

  var chart = new ApexCharts(document.querySelector("#chart2"), options2);
  chart.render();


  var randomizeArray = function(arg) {
    var array = arg.slice();
    var currentIndex = array.length,
        temporaryValue, randomIndex;

    while (0 !== currentIndex) {

        randomIndex = Math.floor(Math.random() * currentIndex);
        currentIndex -= 1;

        temporaryValue = array[currentIndex];
        array[currentIndex] = array[randomIndex];
        array[randomIndex] = temporaryValue;
    }

    return array;
  }

  var sparklineData = [30, 35, 45, 65, 35, 50, 40, 60, 35, 45];

  var spark3 = {
    chart: {
        type: 'area',
        height: 130,
        sparkline: {
            enabled: true
        },
    },
    stroke: {
        curve: 'smooth',
        width: 2
    },
    fill: {
        colors:  "#7367F0",
        gradient: {
            shadeIntensity: 1,
            opacityFrom: 0.1,
            opacityTo: 0.6,
            stops: [0, 90, 100]
          }
        
    },
    series: [{
        data: randomizeArray(sparklineData)
    }],
    xaxis: {
        crosshairs: {
            width: 1
        },
    },
    yaxis: {
        min: 0
    },
    
}

var spark3 = new ApexCharts(document.querySelector("#chart3"), spark3);
spark3.render();

 
  var spark4 = {
    chart: {
        type: 'area',
        height: 50,
        sparkline: {
            enabled: true
        },
    },
    stroke: {
        curve: 'smooth',
        width: 1
    },
    fill: {
        opacity: 0.3,
    },
    series: [{
        data: [20, 25, 20, 10, 20, 10, 20, 25, 10]
    }],
    xaxis: {
      crosshairs: {
          width: 1
        },
    },
    yaxis: {
        min: 0
    },
    colors: ['#7367F0'],
    
  }
  var spark4 = new ApexCharts(document.querySelector("#chart4"), spark4);
  spark4.render();

  var spark5 = {
    chart: {
        type: 'area',
        height: 50,
        sparkline: {
            enabled: true
        },
    },
    stroke: {
        curve: 'smooth',
        width: 1
    },
    fill: {
        opacity: 0.3,
    },
    series: [{
        data: [10, 25, 20, 10, 20, 10, 20, 25, 20]
    }],
    xaxis: {
      crosshairs: {
          width: 1
        },
    },
    yaxis: {
        min: 0
    },
    colors: ['#CB3066'],
    
  }
  var spark5 = new ApexCharts(document.querySelector("#chart5"), spark5);
  spark5.render();

  var spark6 = {
    chart: {
        type: 'area',
        height: 50,
        sparkline: {
            enabled: true
        },
    },
    stroke: {
        curve: 'smooth',
        width: 1
    },
    fill: {
        opacity: 0.3,
    },
    series: [{
        data: [20, 25, 20, 10, 20, 10, 20, 25, 10]
    }],
    xaxis: {
      crosshairs: {
          width: 1
        },
    },
    yaxis: {
        min: 0
    },
    colors: ['#28C76E'],
    
  }
  var spark6 = new ApexCharts(document.querySelector("#chart6"), spark6);
  spark6.render();

  var options7 = {
    chart: {
        height: 380,
        type: 'bar',
        stacked: true,
        toolbar: {
            show: false
        },
        zoom: {
            enabled: true
        }
    },
    
    responsive: [{
        breakpoint: 480,
        options: {
            legend: {
                position: 'bottom',
                offsetX: -10,
                offsetY: 0
            }
        }
    }],
    plotOptions: {
        bar: {
            horizontal: false,
        },
    },
    series: [{
        name: 'PRODUCT A',
        data: [44, 55, 41, 67, 22, 43]
    },{
        name: 'PRODUCT B',
        data: [13, 23, 20, 8, 13, 27]
    },{
        name: 'PRODUCT C',
        data: [11, 17, 15, 15, 21, 14]
    },{
        name: 'PRODUCT D',
        data: [21, 7, 25, 13, 22, 8]
    }],
    xaxis: {
        type: 'datetime',
        categories: ['01/01/2011 GMT', '01/02/2011 GMT', '01/03/2011 GMT', '01/04/2011 GMT', '01/05/2011 GMT', '01/06/2011 GMT'],
    },
    legend: {
        show: false,
        position: 'left',
        offsetY: 40
    },
    fill: {
        opacity: 1
    },
  }
  
  var chart = new ApexCharts(
    document.querySelector("#chart7"),
    options7
  );
  
  chart.render();


  
  var options8 = {
      chart: {
          height: 350,
          type: 'area',
          toolbar: {
              show: false
          }
      },
      dataLabels: {
          enabled: false
      },
      stroke: {
          curve: 'smooth'
      },
      series: [{
          name: 'series1',
          data: [31, 40, 28, 51, 42, 109, 100]
      }, {
          name: 'series2',
          data: [11, 32, 45, 32, 34, 52, 41]
      }],
      legend: {
          show: false,
          position: 'left',
          offsetY: 40
      },
      xaxis: {
          type: 'datetime',
          categories: ["2018-09-19T00:00:00", "2018-09-19T01:30:00", "2018-09-19T02:30:00", "2018-09-19T03:30:00", "2018-09-19T04:30:00", "2018-09-19T05:30:00", "2018-09-19T06:30:00"],                
      },
      tooltip: {
          x: {
              format: 'dd/MM/yy HH:mm'
          },
      }
  }

  var chart = new ApexCharts(
      document.querySelector("#chart8"),
      options8
  );

  chart.render();

  
  // chart mini

  var spark_sm = {
    chart: {
      type: 'line',
      width: '20%',
     
      sparkline: {
          enabled: true
      }
    },
    stroke: {
      curve: 'smooth',
      width: 1
    },
    series: [{
        data: [10, 30, 20, 50, 20, 30, 10]
    }],
    tooltip: {
        fixed: {
            enabled: false
        },
        x: {
            show: false
        },
        y: {
            title: {
                formatter: function(seriesName) {
                    return ''
                }
            }
        },
        marker: {
            show: false
        }
    }
    
  }
  var spark_sm = new ApexCharts(document.querySelector("#chart-sm1"), spark_sm);
  spark_sm.render();

  var spark_sm2 = {
    chart: {
      type: 'line',
      width: '20%',
      sparkline: {
          enabled: true
      }
    },
    stroke: {
      curve: 'smooth',
      width: 1
    },
    series: [{
        data: [10, 20, 40, 20, 20, 30, 10]
    }],
    tooltip: {
        fixed: {
            enabled: false
        },
        x: {
            show: false
        },
        y: {
            title: {
                formatter: function(seriesName) {
                    return ''
                }
            }
        },
        marker: {
            show: false
        }
    },
    colors: ['#CB3066'],
    
    
  }
  var spark_sm2 = new ApexCharts(document.querySelector("#chart-sm2"), spark_sm2);
  spark_sm2.render();

  var spark_sm3 = {
    chart: {
      type: 'line',
      width: '20%',
      sparkline: {
          enabled: true
      }
    },
    stroke: {
      curve: 'smooth',
      width: 1
    },
    series: [{
        data: [10, 20, 40, 20, 20, 30, 10]
    }],
    tooltip: {
        fixed: {
            enabled: false
        },
        x: {
            show: false
        },
        y: {
            title: {
                formatter: function(seriesName) {
                    return ''
                }
            }
        },
        marker: {
            show: false
        }
    },
    colors: ['#e97d23'],
    
    
  }
  var spark_sm3 = new ApexCharts(document.querySelector("#chart-sm3"), spark_sm3);
  spark_sm3.render();

  var spark_sm4 = {
    chart: {
      type: 'line',
      width: '20%',
      sparkline: {
          enabled: true
      }
    },
    stroke: {
      curve: 'smooth',
      width: 1
    },
    series: [{
        data: [10, 20, 40, 20, 20, 30, 10]
    }],
    tooltip: {
        fixed: {
            enabled: false
        },
        x: {
            show: false
        },
        y: {
            title: {
                formatter: function(seriesName) {
                    return ''
                }
            }
        },
        marker: {
            show: false
        }
    },
    colors: ['#28C76E'],
    
    
  }
  var spark_sm4 = new ApexCharts(document.querySelector("#chart-sm4"), spark_sm4);
  spark_sm4.render();

  var spark_sm5 = {
    chart: {
      type: 'line',
      width: '20%',
      sparkline: {
          enabled: true
      }
    },
    stroke: {
      curve: 'smooth',
      width: 1
    },
    series: [{
        data: [10, 20, 40, 20, 20, 30, 10]
    }],
    tooltip: {
        fixed: {
            enabled: false
        },
        x: {
            show: false
        },
        y: {
            title: {
                formatter: function(seriesName) {
                    return ''
                }
            }
        },
        marker: {
            show: false
        }
    },
    colors: ['#5578eb'],
    
    
  }
  var spark_sm5 = new ApexCharts(document.querySelector("#chart-sm5"), spark_sm5);
  spark_sm5.render();


  function generateDayWiseTimeSeries(baseval, count, yrange) {
    var i = 0;
    var series = [];
    while (i < count) {
      var x = baseval;
      var y = Math.floor(Math.random() * (yrange.max - yrange.min + 1)) + yrange.min;

      series.push([x, y]);
      baseval += 86400000;
      i++;
    }
    return series;
  }
  var options9 = {
    chart: {
      height: 350,
      type: 'area',
      toolbar: {
        show: false
      },
      stacked: true,
      events: {
        selection: function(chart, e) {
          console.log(new Date(e.xaxis.min) )
        }
      },

    },
    colors: ['#9e95f5', '#48da88', '#fc612c', '#a2c6f0'],
    dataLabels: {
        enabled: false
    },
    stroke: {
      curve: 'smooth'
    },

    series: [{
        name: 'South',
        data: generateDayWiseTimeSeries(new Date('11 Feb 2017 GMT').getTime(), 20, {
          min: 10,
          max: 60
        })
      },
      {
        name: 'North',
        data: generateDayWiseTimeSeries(new Date('11 Feb 2017 GMT').getTime(), 20, {
          min: 10,
          max: 20
        })
      },
      
      {
        name: 'Central',
        data: generateDayWiseTimeSeries(new Date('11 Feb 2017 GMT').getTime(), 20, {
          min: 10,
          max: 15
        })
      },
      {
        name: 'West',
        data: generateDayWiseTimeSeries(new Date('11 Feb 2017 GMT').getTime(), 20, {
          min: 10,
          max: 15
        })
      }
    ],
    fill: {
      type: 'gradient',
      gradient: {
        opacityFrom: 0.6,
        opacityTo: 0.8,
      }
    },
    legend: {
      position: 'top',
      horizontalAlign: 'left'
    },
    xaxis: {
      type: 'datetime'
    },
  }

  var chart9 = new ApexCharts(
    document.querySelector("#chart9"),
    options9
  );

  chart9.render();

  var spark_sm6 = {
    chart: {
      type: 'line',
      width: '20%',
     
      sparkline: {
          enabled: true
      }
    },
    stroke: {
      curve: 'smooth',
      width: 1
    },
    series: [{
        data: [10, 30, 20, 50, 20, 30, 10]
    }],
    tooltip: {
        fixed: {
            enabled: false
        },
        x: {
            show: false
        },
        y: {
            title: {
                formatter: function(seriesName) {
                    return ''
                }
            }
        },
        marker: {
            show: false
        }
    },
    colors: ["#7367F0"]
    
  }
  var spark_sm6 = new ApexCharts(document.querySelector("#chart-sm6"), spark_sm6);
  spark_sm6.render();

  var spark_sm7 = {
    chart: {
      type: 'line',
      width: '20%',
     
      sparkline: {
          enabled: true
      }
    },
    stroke: {
      curve: 'smooth',
      width: 1
    },
    series: [{
        data: [10, 30, 20, 50, 20, 30, 10]
    }],
    tooltip: {
        fixed: {
            enabled: false
        },
        x: {
            show: false
        },
        y: {
            title: {
                formatter: function(seriesName) {
                    return ''
                }
            }
        },
        marker: {
            show: false
        }
    },
    colors: ["#28C76E"]
    
  }
  var spark_sm7 = new ApexCharts(document.querySelector("#chart-sm7"), spark_sm7);
  spark_sm7.render();

  var spark_sm8 = {
    chart: {
      type: 'line',
      width: '20%',
     
      sparkline: {
          enabled: true
      }
    },
    stroke: {
      curve: 'smooth',
      width: 1
    },
    series: [{
        data: [10, 30, 20, 50, 20, 30, 10]
    }],
    tooltip: {
        fixed: {
            enabled: false
        },
        x: {
            show: false
        },
        y: {
            title: {
                formatter: function(seriesName) {
                    return ''
                }
            }
        },
        marker: {
            show: false
        }
    },
    colors: ["#e97d23"]
    
  }
  var spark_sm8 = new ApexCharts(document.querySelector("#chart-sm8"), spark_sm8);
  spark_sm8.render();

  var spark_sm9 = {
    chart: {
      type: 'line',
      width: '20%',
     
      sparkline: {
          enabled: true
      }
    },
    stroke: {
      curve: 'smooth',
      width: 1
    },
    series: [{
        data: [10, 30, 20, 50, 20, 30, 10]
    }],
    tooltip: {
        fixed: {
            enabled: false
        },
        x: {
            show: false
        },
        y: {
            title: {
                formatter: function(seriesName) {
                    return ''
                }
            }
        },
        marker: {
            show: false
        }
    },
    colors: ["#F14004"]
    
  }
  var spark_sm9 = new ApexCharts(document.querySelector("#chart-sm9"), spark_sm9);
  spark_sm9.render();


});