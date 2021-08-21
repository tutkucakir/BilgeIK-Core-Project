$(document).ready(function(){
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
  // analytic_echart-3 
  let echartElemBar1 = document.getElementById('dashboard_sales');
  if (echartElemBar1) {
      let echartBar = echarts.init(echartElemBar1);
      echartBar.setOption({
        
        grid: {
          top: "10%",
          bottom: "10%",
          // left: "5%",
          right: "5%"
        },
        legend: {
          show: false
        },
        color:["rgba(93, 120, 255, 1)", "#fd397a"],
        barGap: 0,
        barMaxWidth: "64px",
        tooltip: {},
        dataset: {
          source: [
            ["Month", "Website", "App"],
            ["Jan", 2200, 1200],
            ["Feb", 800, 500],
            ["Mar", 700, 1350],
            ["Apr", 1500, 1250],
            ["May", 2450, 450],
            ["June", 1700, 1250]
          ]
        },
        xAxis: {
          type: "category",
          axisLine: {
            show: false
          },
          splitLine: {
            show: false
          },
          axisTick: {
            show: false
          },
          axisLabel: {
            color: "#000",
            fontSize: 13,
            fontFamily: "roboto"
          }
        },
        yAxis: {
          axisLine: {
            show: false
          },
          axisTick: {
            show: false
          },
          splitLine: {
            // show: false
            lineStyle: {
              color:"#000",
              opacity: 0.15
            }
          },
          axisLabel: {
            color: "#000",
            fontSize: 13,
            fontFamily: "roboto"
          }
        },
        // Declare several bar series, each will be mapped
        // to a column of dataset.source by default.
        series: [{ type: "bar" }, { type: "bar" }]
      
      });
      $(window).on('resize', function() {
          setTimeout(() => {
              echartBar.resize();
          }, 500);
      });
  }

  // sales_echart-2 
  let echartElemBar2 = document.getElementById('dashboard_sales-2');
  if (echartElemBar2) {
      let echartBar = echarts.init(echartElemBar2);
      echartBar.setOption({
        color: ['#473db0', '#9187f6', '#7d6cbb'],
        legend: {
          show: true,
          itemGap: 20,
          icon: "circle",
          bottom: 0,
          textStyle: {
            color: "#a5a3b4",
            fontSize: 13,
            fontFamily: "roboto"
          }
        },
        tooltip: {
          show: false,
          trigger: "item",
          formatter: "{a} <br/>{b}: {c} ({d}%)"
        },
        xAxis: [
          {
            axisLine: {
              show: false
            },
            splitLine: {
              show: false
            }
          }
        ],
        yAxis: [
          {
            axisLine: {
              show: false
            },
            splitLine: {
              show: false
            }
          }
        ],
    
        series: [
          {
            name: "Traffic Rate",
            type: "pie",
            radius: ["45%", "72.55%"],
            center: ["50%", "50%"],
            avoidLabelOverlap: false,
            hoverOffset: 5,
            stillShowZeroSum: false,
            label: {
              normal: {
                show: false,
                position: "center", // shows the description data to center, turn off to show in right side
                textStyle: {
                  color: "#a5a3b4",
                  fontSize: 13,
                  fontFamily: "roboto"
                },
                formatter: "{a}"
              },
              emphasis: {
                show: true,
                textStyle: {
                  fontSize: "14",
                  fontWeight: "normal"
                  // color: "rgba(15, 21, 77, 1)"
                },
                formatter: "{b} \n{c} ({d}%)"
              }
            },
            labelLine: {
              normal: {
                show: false
              }
            },
            data: [
              {
                value: 65,
                name: "Google"
              },
              {
                value: 20,
                name: "Facebook"
              },
              { value: 15, name: "Others" }
            ],
            itemStyle: {
              emphasis: {
                shadowBlur: 10,
                shadowOffsetX: 0,
                shadowColor: "rgba(0, 0, 0, 0.5)"
              }
            }
          }
        ]
      
      
      });
      $(window).on('resize', function() {
          setTimeout(() => {
              echartBar.resize();
          }, 500);
      });
  }


});