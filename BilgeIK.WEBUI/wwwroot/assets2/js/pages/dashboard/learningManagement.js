$(document).ready(function() {
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

  let apexChartElem1 = document.getElementById("welcome_progress_chart");
  console.log(apexChartElem1);
  if(apexChartElem1) {
    let options = {
      series: [70],
      chart: {
          height: '200px',
          type: 'radialBar',
          offsetX: 60,
          offsetY: -20
      },
      grid: {
        padding: {
         left: 0,
         right: 0
        }
      },
      plotOptions: {
          radialBar: {
              startAngle: -90,
              endAngle: 90,
              offsetY: 0,
              hollow: {
                  margin: 0,
                  size: "60%"
              },
              dataLabels: {
                  showOn: "always",
                  name: {
                      show: true,
                      fontSize: "13px",
                      fontWeight: "600",
                      offsetY: -5,
                      color: '#828D99'
                  },
                  value: {
                      color: '#304156',
                      fontSize: "24px",
                      fontWeight: "600",
                      offsetY: -40,
                      show: true
                  }
              },
              track: {
                  background: '#eee',
                  strokeWidth: '100%'
              }
          }
      },
      colors: ['#0081FF', '#eee'],
      stroke: {
        lineCap: "round",
      },
      labels: ["Progress"],
      responsive: [{
        breakpoint: 767,
        options: {
          chart: {
            offsetX: 0,
            offsetY: 0,
          }
        },
    }]
  };

    var chart = new ApexCharts(apexChartElem1, options);
    chart.render();
  }

  // analytic_echart-3
  let echartElemBar3 = document.getElementById("dashboard_learningManagement");
  if (echartElemBar3) {
    let echartBar = echarts.init(echartElemBar3);
    echartBar.setOption({
      barGap: 50,
      barMaxWidth: "6px",

      grid: {
        top: 24,
        left: 26,
        right: 26,
        bottom: 25
      },

      legend: {
        itemGap: 32,
        top: -4,
        left: -4,
        icon: "circle",
        width: "auto",
        data: ["Angular", "React", "Javascript"],
        textStyle: {
          color: "#a5a3b4",
          fontSize: 12,
          fontFamily: "roboto",
          align: "center"
        }
      },
      tooltip: {},
      xAxis: {
        type: "category",
        data: ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"],
        showGrid: false,
        boundaryGap: false,
        axisLine: {
          show: false
        },
        splitLine: {
          show: false
        },
        axisLabel: {
          color: "#a5a3b4",
          fontSize: 12,
          fontFamily: "roboto",
          margin: 16
        },
        axisTick: {
          show: false
        }
      },
      color: ["#7367F0", "#e95455", "#e97d23"],
      yAxis: {
        type: "value",
        show: false,
        axisLine: {
          show: false
        },
        splitLine: {
          show: false
        }
      },
      series: [
        {
          name: "Angular",
          data: [50, 50, 80, 80, 80, 60, 70],
          type: "bar",
          itemStyle: {
            barBorderRadius: [0, 0, 10, 10]
          },
          stack: "one"
        },
        {
          name: "React",
          data: [70, 80, 90, 100, 70, 80, 65],
          type: "bar",
          stack: "one"
        },
        {
          name: "Javascript",
          data: [65, 80, 70, 100, 90, 70, 55],
          type: "bar",
          itemStyle: {
            barBorderRadius: [10, 10, 0, 0]
          },
          stack: "one"
        }
      ]
    });
    $(window).on("resize", function() {
      setTimeout(() => {
        echartBar.resize();
      }, 500);
    });
  }
});
