$(document).ready(function() {
  var legend = {
      position: "top",
      horizontalAlign: "left",
      offsetX: -12,
      offsetY: -5,
      fontFamily: 'Roboto',
      markers: {
        height: 12,
        width: 12,
        radius: 12
      },
      itemMargin: {
        horizontal: 0,
        vertical: 8
      }
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
  $(function() {
    var start = moment().subtract(29, "days");
    var end = moment();

    function cb(start, end) {
      $("#reportrange-2 span").html(
        start.format("MMM D") + " - " + end.format("MMM D")
      );
    }

    $("#reportrange-2").daterangepicker(
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

  var options = {
    series: [
      {
        name: "Follower Growth",
        data: [4, 3, 10, 9, 29, 19, 22, 9, 12]
      }
    ],
    chart: {
      height: 350,
      type: "line",
      toolbar: {
        show: false
      }
    },
    stroke: {
      width: 4,
      curve: "smooth"
    },
    grid: {
      show: true,
      strokeDashArray: 3
    },
    xaxis: {
      type: "datetime",
      categories: [
        "1/11/2000",
        "2/11/2000",
        "3/11/2000",
        "4/11/2000",
        "5/11/2000",
        "6/11/2000",
        "7/11/2000",
        "8/11/2000",
        "9/11/2000",
        
      ],
      labels: {
        style: {
          colors: '#8e8da2',
          fontSize: '12px',
          fontFamily: 'Roboto',
        },
      },
      tooltip: {
        enabled: false
      },
      axisBorder: {
        show: false
      },
      axisTicks: {
        show: false
      }
    },

    colors: ["#7367F0"],

    markers: {
      size: 4,
      colors: ["#7367F0"],
      strokeColors: "#fff",
      strokeWidth: 2,
      hover: {
        size: 7
      }
    },
    yaxis: {
      min: 0,
      max: 30,
      title: {
        text: ""
      }
    }
  };

  var chart = new ApexCharts(document.querySelector("#analayticOne"), options);
  chart.render();

  var optionsTwo = {
    series: [
      {
        name: "Series 1",
        data: [10, 80, 15, 71, 18, 80, 100]
      },
      {
        name: "Series 2",
        data: [80, 12, 75, 45, 84, 15, 74]
      }
    ],
    chart: {
      height: 350,
      type: "area",
      toolbar: {
        show: false
      }
    },
    legend: legend,
    dataLabels: {
      enabled: false
    },
    colors: ["#4A90E2", "#8e8da2"],
    stroke: {
      width: 0,
      curve: "smooth"
    },
    markers: {
      size: [4, 4]
    },
    grid: {
      show: true,
      strokeDashArray: 3
    },
    fill: {
      opacity: 1,
    },
    xaxis: {
      type: "datetime",
      categories: [
        "2018-09-19T00:00:00.000Z",
        "2018-09-19T01:30:00.000Z",
        "2018-09-19T02:30:00.000Z",
        "2018-09-19T03:30:00.000Z",
        "2018-09-19T04:30:00.000Z",
        "2018-09-19T05:30:00.000Z",
        "2018-09-19T06:30:00.000Z"
      ],
      axisBorder: {
        show: false
      },
      axisTicks: {
        show: false
      }
    },
    tooltip: {
      x: {
        format: "dd/MM/yy HH:mm"
      }
    }
  };

  var chart = new ApexCharts(document.querySelector("#analayticTwo"), optionsTwo);
  chart.render();

  function generateData(count, yrange) {
    var i = 0;
    var series = [];
    while (i < count) {
      var x = (i + 1).toString();
      var y =
        Math.floor(Math.random() * (yrange.max - yrange.min + 1)) + yrange.min;

      series.push({
        x: x,
        y: y
      });
      i++;
    }
    return series;
  }
  var optionsThree = {
    series: [
      {
        name: "Mon",
        data: generateData(18, {
          min: 0,
          max: 90
        })
      },
      {
        name: "Tue",
        data: generateData(18, {
          min: 0,
          max: 90
        })
      },
      {
        name: "Wed",
        data: generateData(18, {
          min: 0,
          max: 90
        })
      },
      {
        name: "Thu",
        data: generateData(18, {
          min: 0,
          max: 90
        })
      },
      {
        name: "Fri",
        data: generateData(18, {
          min: 0,
          max: 90
        })
      },
      {
        name: "Sat",
        data: generateData(18, {
          min: 0,
          max: 90
        })
      },
      {
        name: "Sun",
        data: generateData(18, {
          min: 0,
          max: 90
        })
      }
    ],
    chart: {
      height: 350,
      type: "heatmap",
      toolbar: {
        show: false
      }
    },
    grid: {
      xaxis: {
        lines: {
            show: false
        }
    },   
      yaxis: {
          lines: {
              show: false
          }
      },  
    },
    plotOptions: {
      heatmap: {
        radius: 10
      }
    },
    dataLabels: {
      enabled: false
    },
    colors: ["#008FFB"],
    xaxis: {
      categories: [
        "12am",
        "1am",
        "3am",
        "4am",
        "5am",
        "6am",
        "7am",
        "8am",
        "9am",
        "10am",
        "11am",
        "12pm",
        "1pm",
        "2pm",
        "3pm",
        "4pm",
        "5pm",
        "6pm",
        "7pm",
        "8pm",
        "9pm",
        "10pm",
        "11pm"
      ],
      axisBorder: {
        show: false
      },
      axisTicks: {
        show: false
      }
    }
  };

  var chart = new ApexCharts(
    document.querySelector("#analayticThree"),
    optionsThree
  );
  chart.render();

  var options = {
    colors: ["#F14004", "#8e8da2"],
    series: [
      {
        name: "Like",
        data: [120, 90, 130, 140, 49, 62, 100, 91, 148]
      },
      {
        name: "Avg Likes per day",
        data: [120, 120, 110, 110, 35, 90, 130, 80, 100]
      }
    ],
    stroke: {
      width: [4, 3],
      curve: "straight"
    },
    markers: {
      size: [6, 0]
    },
    legend: legend,
    chart: {
      height: 350,
      type: "line",
      toolbar: {
        show: false
      },
      zoom: {
        enabled: false
      }
    },
    dataLabels: {
      enabled: false
    },
    grid: {
      strokeDashArray: 3
    },
    xaxis: {
      categories: [
        "Jan",
        "Feb",
        "Mar",
        "Apr",
        "May",
        "Jun",
        "Jul",
        "Aug",
        "Sep"
      ],
      axisBorder: {
        show: false
      },
      axisTicks: {
        show: false
      }
    }
  };

  var chart = new ApexCharts(document.querySelector("#analayticFour"), options);
  chart.render();

  var options = {
    colors: ["#28C76E", "#8e8da2"],
    series: [
      {
        name: "Comments",
        data: [120, 90, 130, 140, 49, 62, 100, 91, 148]
      },
      {
        name: "Avg Comments per day",
        data: [120, 120, 110, 110, 35, 90, 130, 80, 100]
      }
    ],
    legend: legend,
    stroke: {
      width: [4, 3],
      curve: "straight"
    },
    markers: {
      size: [6, 0]
    },
    chart: {
      height: 350,
      type: "line",
      toolbar: {
        show: false
      },
      zoom: {
        enabled: false
      }
    },
    dataLabels: {
      enabled: false
    },
    grid: {
      strokeDashArray: 3
    },
    xaxis: {
      categories: [
        "Jan",
        "Feb",
        "Mar",
        "Apr",
        "May",
        "Jun",
        "Jul",
        "Aug",
        "Sep"
      ],
      axisBorder: {
        show: false
      },
      axisTicks: {
        show: false
      }
    }
  };

  var chart = new ApexCharts(document.querySelector("#analayticFive"), options);
  chart.render();
});
