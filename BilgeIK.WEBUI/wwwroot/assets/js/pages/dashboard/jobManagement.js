$(document).ready(function () {
  $(function () {
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
            moment().subtract(1, "days"),
          ],
          "Last 7 Days": [moment().subtract(6, "days"), moment()],
          "Last 30 Days": [moment().subtract(29, "days"), moment()],
          "This Month": [moment().startOf("month"), moment().endOf("month")],
          "Last Month": [
            moment().subtract(1, "month").startOf("month"),
            moment().subtract(1, "month").endOf("month"),
          ],
        },
      },
      cb
    );

    cb(start, end);
  });

  // chartOne
  var options = {
    chart: {
      type: "radialBar",
      width: 130,
      height: 150
    },
    dataLabels: {
      enabled: false,
    },
    plotOptions: {
      radialBar: {
        hollow: {
          margin: 0,
          size: "55%"
        },
        dataLabels: {
          showOn: "always",
          name: {
            show: false
          },
          value: {
              color: '#828D99',
              fontSize: "18px",
              fontWeight: "600",
              offsetY: 8,
              show: true
          }
        }
      }
    },
    colors: ["#5d78ff", "#fff"],

    series: [70],
    legend: {
      show: false,
    },
    stroke: {
      lineCap: "round",
    },
    
    responsive: [
      {
        breakpoint: 480,
        options: {
          chart: {
            width: 130,
          },
          legend: {
            show: false,
          },
        },
      },
    ],
  };

  var chart = new ApexCharts(
    document.querySelector("#management-chart"),
    options
  );
  chart.render();

  var options = {
    chart: {
      type: "radialBar",
      width: 130,
      height: 150
    },
    dataLabels: {
      enabled: false,
    },
    plotOptions: {
      radialBar: {
        hollow: {
          margin: 0,
          size: "55%"
        },
        dataLabels: {
          showOn: "always",
          name: {
            show: false
          },
          value: {
              color: '#828D99',
              fontSize: "18px",
              fontWeight: "600",
              offsetY: 8,
              show: true
          }
        }
      }
    },
    colors: ["#e97d23", "#fff"],

    series: [45],
    legend: {
      show: false,
    },
    stroke: {
      lineCap: "round",
    },
    
    responsive: [
      {
        breakpoint: 480,
        options: {
          chart: {
            width: 130,
          },
          legend: {
            show: false,
          },
        },
      },
    ],
  };

  var chart2 = new ApexCharts(
    document.querySelector("#management-chartTwo"),
    options
  );
  chart2.render();

  // var options = {
  //   dataLabels: {
  //     enabled: false,
  //   },
  //   plotOptions: {
  //     pie: {
  //       donut: {
  //         size: "80%",
  //         labels: {
  //           show: false,
  //           name: {
  //             show: false,
  //           },
  //         },
  //       },
  //     },
  //   },
  //   colors: ["#28C76E", "#fff"],

  //   series: [74, 26],
  //   legend: {
  //     show: false,
  //   },

  //   chart: {
  //     type: "donut",
  //     width: 130,
  //   },

  //   responsive: [
  //     {
  //       breakpoint: 480,
  //       options: {
  //         chart: {
  //           width: 130,
  //         },
  //         legend: {
  //           show: false,
  //         },
  //       },
  //     },
  //   ],
  // };

  // var chart3 = new ApexCharts(
  //   document.querySelector("#management-chartThree"),
  //   options
  // );
  // chart3.render();

  let echartElemBar4 = document.getElementById("jobManagement_chart4");
  if (echartElemBar4) {
    let echartBar = echarts.init(echartElemBar4);
    echartBar.setOption({
      barGap: 50,
      barMaxWidth: "6px",

      grid: {
        top: 24,
        left: 26,
        right: 26,
        bottom: 25,
      },

      legend: {
        itemGap: 12,
        top: -4,
        left: -4,
        icon: "circle",
        width: "auto",
        data: ["Manager", "Marketer", "Developer"],
        textStyle: {
          color: "#a5a3b4",
          fontSize: 12,
          fontFamily: "roboto",
          align: "center",
        },
      },
      tooltip: {},
      xAxis: {
        type: "category",
        data: ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"],
        showGrid: false,
        boundaryGap: false,
        axisLine: {
          show: false,
        },
        splitLine: {
          show: false,
        },
        axisLabel: {
          color: "#a5a3b4",
          fontSize: 12,
          fontFamily: "roboto",
          margin: 16,
        },
        axisTick: {
          show: false,
        },
      },
      color: ["#e97d23", "#CB3066", "#7367F0"],
      yAxis: {
        type: "value",
        show: false,
        axisLine: {
          show: false,
        },
        splitLine: {
          show: false,
        },
      },
      series: [
        {
          name: "Manager",
          data: [80, 70, 40, 30, 40, 20, 40],
          type: "bar",
          itemStyle: {
            barBorderRadius: [0, 0, 10, 10],
          },
          stack: "one",
        },
        {
          name: "Marketer",
          data: [70, 80, 90, 100, 70, 80, 65],
          type: "bar",
          stack: "one",
        },
        {
          name: "Developer",
          data: [65, 80, 70, 100, 90, 70, 55],
          type: "bar",
          itemStyle: {
            barBorderRadius: [10, 10, 0, 0],
          },
          stack: "one",
        },
      ],
    });
    $(window).on("resize", function () {
      setTimeout(() => {
        echartBar.resize();
      }, 500);
    });
  }

  // chart-5
  var options = {
    series: [
      {
        name: "Applications",
        data: [14, 29, 18, 20, 20, 40, 20, 30, 24, 18, 30, 15],
      },
    ],
    chart: {
      height: 350,
      type: "line",
      dropShadow: {
        enabled: true,
        color: "#000",
        top: 18,
        left: 7,
        blur: 10,
        opacity: 0.2,
      },
      toolbar: {
        show: false,
      },
    },
    colors: ["#5d78ff", "#fbaf0f"],
    dataLabels: {
      enabled: false,
    },
    stroke: {
      curve: "smooth",
    },
    title: {
      text: "",
      align: "left",
    },
    grid: {
      borderColor: "#fff",
      row: {
        colors: ["#f3f3f3", "transparent"], // takes an array which will be repeated on columns
        opacity: 0,
      },
    },
    markers: {
      size: 5,
    },
    xaxis: {
      categories: [
        "12am",
        "1am",
        "2am",
        "3am",
        "4am",
        "5am",
        "6am",
        "12pm",
        "1pm",
        "2pm",
        "3pm",
        "4pm",
        "5pm",
        "6pm",
      ],
      title: {
        text: "",
      },
      axisBorder: {
        show: false,
      },
    },
    yaxis: {
      title: {
        text: "",
      },
      min: 5,
      max: 40,
    },
    legend: {
      position: "top",
      horizontalAlign: "right",
      floating: true,
      offsetY: -25,
      offsetX: -5,
    },
  };

  var chart5 = new ApexCharts(
    document.querySelector("#jobManagement_chart5"),
    options
  );
  chart5.render();

  // chartSeven
  var options = {
    dataLabels: {
      enabled: false,
    },
    plotOptions: {
      pie: {
        donut: {
          size: "70%",
          labels: {
            show: true,
            name: {
              show: true,
            },
          },
        },
      },
    },
    colors: ["#CB3066", "#5d78ff"],

    series: [74, 26],
    labels: ["Male", "Female"],
    legend: {
      show: false,
    },

    chart: {
      type: "donut",
      width: "100%",
    },

    responsive: [
      {
        breakpoint: 480,
        options: {
          chart: {
            width: 150,
          },
          legend: {
            show: false,
          },
        },
      },
    ],
  };

  var chart = new ApexCharts(
    document.querySelector("#jobManagement_chart6"),
    options
  );
  chart.render();
});
