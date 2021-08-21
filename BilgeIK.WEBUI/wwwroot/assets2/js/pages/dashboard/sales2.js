$(document).ready(function() {
  var legend = {
    position: "bottom",
    horizontalAlign: "left",
    offsetX: -12,
    offsetY: -10,
    fontFamily: "Roboto",
    markers: {
      height: 12,
      width: 12,
      radius: 12,
    },
    itemMargin: {
      horizontal: 4,
      vertical: 4,
    },
  };
  $(function() {
    var start = moment().subtract(29, "days");
    var end = moment();

    function cb(start, end) {
      $("#reportrange span").html(
        start.format("MMM D") + " - " + end.format("MMM D")
      );
    }

    $("#reportrange").daterangepicker(
      {
        startDate: start,
        endDate: end,
        ranges: {
          Today: [moment(), moment()],
          Yesterday: [
            moment().subtract(1, "days"),
            moment().subtract(1, "days")
          ],
          "Last 7 Days": [moment().subtract(6, "days"), moment()],
          "Last 30 Days": [moment().subtract(29, "days"), moment()],
          "This Month": [moment().startOf("month"), moment().endOf("month")],
          "Last Month": [
            moment()
              .subtract(1, "month")
              .startOf("month"),
            moment()
              .subtract(1, "month")
              .endOf("month")
          ]
        }
      },
      cb
    );

    cb(start, end);
  });

  // var trafficPieOption = {
  //   series: [74],
  //   chart: {
  //       height: 200,
  //       type: 'radialBar',
  //       offsetY: 0
  //   },
  //   plotOptions: {
  //       radialBar: {
  //           startAngle: -90,
  //           endAngle: 90,
  
  //           hollow: {
  //               margin: 0,
  //               size: "70%"
  //           },
  //           dataLabels: {
  //               showOn: "always",
  //               name: {
  //                   show: true,
  //                   fontSize: "13px",
  //                   fontWeight: "700",
  //                   offsetY: -5,
  //                   color: ['#ccc']
  //               },
  //               value: {
  //                   color: ['#ccc'],
  //                   fontSize: "30px",
  //                   fontWeight: "700",
  //                   offsetY: -40,
  //                   show: true
  //               }
  //           },
  //           track: {
  //               background: ["#0081FF"],
  //               strokeWidth: '100%'
  //           }
  //       }
  //   },
  //   colors: ["#0081FF"],
  //   stroke: {
  //       lineCap: "round",
  //   },
  //   labels: ["Progress"]
  // }
  // var chart = new ApexCharts(document.querySelector("#trafficPie"), trafficPieOption);
  // chart.render();

  var options = {
    series: [
    {
      name: "Income",
      data: [14, 29, 18, 20, 20, 40, 20]
    },
    {
      name: "Outcome",
      data: [12, 20, 14, 25, 17, 35, 30]
    }
  ],
    chart: {
    height: 350,
    type: 'line',
    // dropShadow: {
    //   enabled: true,
    //   color: '#000',
    //   top: 18,
    //   left: 7,
    //   blur: 10,
    //   opacity: 0.2
    // },
    toolbar: {
      show: false
    }
  },
  colors: ['#5d78ff', '#fbaf0f'],
  dataLabels: {
    enabled: false,
  },
  stroke: {
    curve: 'smooth'
  },
  title: {
    text: '',
    align: 'left'
  },
  grid: {
    borderColor: '#fff',  //e7e7e7
    row: {
      colors: ['#f3f3f3', 'transparent'], // takes an array which will be repeated on columns
      opacity: 0
    },
  },
  markers: {
    size: 5
  },
  xaxis: {
    categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'],
    title: {
      text: ''
    },
    axisBorder: {
        show: false,
      },
      
  },
  yaxis: {
    title: {
      text: ''
    },
    min: 5,
    max: 40
  },
  legend: {
    position: 'top',
    horizontalAlign: 'right',
    floating: true,
    offsetY: -25,
    offsetX: -5
  }
  };

  var chart = new ApexCharts(document.querySelector("#sales2"), options);
  chart.render();


  
// simple donut
var options = {
    chart: {
        type: 'pie',
        width: '100%',
        
    },
    series: [44, 55, 41, 17, 15],
    labels: ['USA', 'Brazil', 'India', 'Australia', 'Bangladesh'],
    colors: ['#5d78ff', '#fbaf0f', '#1dc9b7', '#fd397a'],
    legend: legend,
    plotOptions: {
      pie: {
        dataLabels: {
          show: false,
          offset: 0,
          minAngleToShowLabel: 1
        }
      }
    },
    responsive: [{
        breakpoint: 480,
        options: {
            chart: {
                width: 310
            },
            legend: Object.assign({}, legend, {horizontalAlign: "center"})
        }
    }]
}

var chart = new ApexCharts(
    document.querySelector("#salesByCountries"),
    options
);

chart.render();
  var options = {
    series: [{
    name: 'Organic',
    data: [11, 100, 80, 20, 51, 42, 109]
  }, {
    name: 'Direct',
    data: [31, 90, 32, 40, 90, 34, 52]
  }],
 
    chart: {
    height: 350,
    type: 'bar',
    toolbar: {
      show: false
    }
  },
  plotOptions: {
    bar: {
      startingShape: 'rounded',
      endingShape: 'rounded',
      columnWidth: '50%'
    }
  },
  dataLabels: {
    enabled: false
  },
  stroke: {
    curve: 'smooth'
  },
  legend: Object.assign({}, legend, {horizontalAlign: "center"}),
  grid: {
    borderColor: 'transparent',
    row: {
      opacity: 0
    },
  },
  colors: ['#5d78ff', '#fbaf0f', '#1dc9b7', '#fd397a'],
  xaxis: {
    categories: ["Sat", "Sun", "Mon", "Tue", "Wed", "Thu", "Fri"],
    axisBorder: {
      show: false,
    },
    axisTicks: {
      show: false,
    },
  },
  tooltip: {
    x: {
      format: 'dd/MM/yy HH:mm'
    },
  },
  };

  var chart2 = new ApexCharts(document.querySelector("#sales2-2"), options);
  chart2.render();




});





