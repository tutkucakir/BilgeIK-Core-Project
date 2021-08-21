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
  // analytic_eChart  
  let echartElemBar = document.getElementById('anlaytic_eChart');
  if (echartElemBar) {
      let echartBar = echarts.init(echartElemBar);
      echartBar.setOption({
        color:["#7467ef"],
        grid: {
          left: 0,
          top: 0,
          right: 0,
          bottom: 0
        },
        legend: {},
        tooltip: {},
        xAxis: {
          show: false,
          type: "category",
          showGrid: false,
          boundaryGap: false
        },
        yAxis: {
          show: false,
          type: "value",
          splitLine: {
            show: false
          }
        },
        series: [
          {
            //   data: [13, 4, 15, 6, 30, 8, 43],
            data: [25, 18, 20, 30, 40, 43],
            type: "line",
            areaStyle: {},
            smooth: true
          }
        ]
   
      
      });
      $(window).on('resize', function() {
          setTimeout(() => {
              echartBar.resize();
          }, 500);
      });
  }
  // analytic_eChart -2
  let echartElemBar2 = document.getElementById('anlaytic_eChart-2');
  if (echartElemBar2) {
      let echartBar = echarts.init(echartElemBar2);
      echartBar.setOption({
        color:["#e97d23"],
        grid: {
          left: 0,
          top: 0,
          right: 0,
          bottom: 0
        },
        legend: {},
        tooltip: {},
        xAxis: {
          show: false,
          type: "category",
          showGrid: false,
          boundaryGap: false
        },
        yAxis: {
          show: false,
          type: "value",
          splitLine: {
            show: false
          }
        },
        series: [
          {
            //   data: [13, 4, 15, 6, 30, 8, 43],
            data: [25, 18, 20, 30, 40, 43],
            type: "line",
            areaStyle: {},
            smooth: true
          }
        ]
   
      
      });
      $(window).on('resize', function() {
          setTimeout(() => {
              echartBar.resize();
          }, 500);
      });
  }

// analytic_echart-3 
  let echartElemBar3 = document.getElementById('anlaytic_eChart-3');
  if (echartElemBar3) {
      let echartBar = echarts.init(echartElemBar3);
      echartBar.setOption({
        grid: {
          top: 16,
          left: 24,
          right: 0,
          bottom: 24
        },
        color:["#7467ef"],
        series: [
          {
            //   data: [13, 4, 15, 6, 30, 8, 43],
            data: [30, 45,30, 45,30, 45,30, 45,30 ],
            type: "line",
            areaStyle: {},
            smooth: true
          }
        ],
      
        legend: {},
        tooltip: {},
        xAxis: {
          show: false,
          type: "category",
          showGrid: false,
          boundaryGap: false
        },
        yAxis: {
          type: "value",
          min: 10,
          max: 60,
          splitLine: {
            show: false
          },
          axisLine: {
            show: false
          },
          axisTick: {
            show: false
          },
          axisLabel: {
            color: "rgba(0,0,0,0.54)",
            fontSize: 13,
            fontFamily: "roboto",
          }
        }
      
      });
      $(window).on('resize', function() {
          setTimeout(() => {
              echartBar.resize();
          }, 500);
      });
  }


});