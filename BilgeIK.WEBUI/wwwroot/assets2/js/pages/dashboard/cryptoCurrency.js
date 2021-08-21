$(document).ready(function () {
  var legend = {
    position: "bottom",
    horizontalAlign: "center",
    offsetX: -12,
    offsetY: -10,
    fontFamily: "Roboto",
    markers: {
      height: 12,
      width: 12,
      radius: 12,
    },
    itemMargin: {
      horizontal: 0,
      vertical: 8,
    },
  };
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

  var options = {
    series: [
      {
        name: "BTC",
        data: [
          [1584144000000, 5665.57],
          [1584230400000, 5961.78],
          [1584316800000, 5364.93],
          [1584403200000, 5561.43],
          [1584489600000, 5451.02],
          [1584576000000, 6418.99],
          [1584662400000, 6932.04],
          [1584748800000, 6459.65],
          [1584835200000, 6415.14],
          [1584921600000, 6614.56],
          [1585008000000, 6857.49],
          [1585094400000, 6980.29],
          [1585180800000, 6796.12],
          [1585267200000, 6876.97],
          [1585353600000, 6384.56],
          [1585440000000, 6286.96],
          [1585526400000, 6616.94],
          [1585612800000, 6531.66],
          [1585699200000, 6716.04],
          [1585785600000, 7245.15],
          [1585872000000, 7044.01],
          [1585958400000, 7010.45],
          [1586044800000, 6910.39],
          [1586131200000, 7359.05],
          [1586217600000, 7462.51],
          [1586304000000, 7425.85],
          [1586390400000, 7374.01],
          [1586476800000, 7304.8],
          [1586563200000, 6956.27],
          [1586649600000, 7199.97],
          [1586736000000, 6913.76],
        ],
      },
      {
        name: "LTC",
        data: [
          [1584144000000, 38.03],
          [1584230400000, 38.93],
          [1584316800000, 36.35],
          [1584403200000, 36.05],
          [1584489600000, 34.92],
          [1584576000000, 40.84],
          [1584662400000, 43.81],
          [1584748800000, 39.83],
          [1584835200000, 39.7],
          [1584921600000, 40],
          [1585008000000, 41.02],
          [1585094400000, 41.15],
          [1585180800000, 40.61],
          [1585267200000, 41.37],
          [1585353600000, 39.5],
          [1585440000000, 39.18],
          [1585526400000, 39.76],
          [1585612800000, 39.39],
          [1585699200000, 39.5],
          [1585785600000, 42.97],
          [1585872000000, 41.29],
          [1585958400000, 41.22],
          [1586044800000, 41.09],
          [1586131200000, 45.12],
          [1586217600000, 47.66],
          [1586304000000, 47.23],
          [1586390400000, 46.86],
          [1586476800000, 46.43],
          [1586563200000, 43.21],
          [1586649600000, 43.71],
          [1586736000000, 42.02],
        ],
      },
      {
        name: "ETH",
        data: [
          [1584144000000, 135.54],
          [1584230400000, 133.5],
          [1584316800000, 123.9],
          [1584403200000, 121.2],
          [1584489600000, 118.84],
          [1584576000000, 143.4],
          [1584662400000, 152.97],
          [1584748800000, 137.51],
          [1584835200000, 137.26],
          [1584921600000, 137.77],
          [1585008000000, 143.81],
          [1585094400000, 142.7],
          [1585180800000, 140.14],
          [1585267200000, 141.91],
          [1585353600000, 133.36],
          [1585440000000, 132.12],
          [1585526400000, 135.32],
          [1585612800000, 135.34],
          [1585699200000, 137.26],
          [1585785600000, 150.15],
          [1585872000000, 146.86],
          [1585958400000, 146.56],
          [1586044800000, 145.93],
          [1586131200000, 172.04],
          [1586217600000, 176.41],
          [1586304000000, 174.63],
          [1586390400000, 173.55],
          [1586476800000, 170.28],
          [1586563200000, 161.48],
          [1586649600000, 165.46],
          [1586736000000, 158.83],
        ],
      },
    ],
    chart: {
      height: 350,
      type: "area",
      toolbar: {
        show: false,
      },
    },
    legend: legend,
    dataLabels: {
      enabled: false,
    },
    colors: ["#4A90E2", "#8e8da2"],
    stroke: {
      width: 0,
      curve: "smooth",
    },
    markers: {
      size: [4, 4],
    },
    grid: {
      show: true,
      strokeDashArray: 3,
    },
    fill: {
      opacity: 1,
    },
    xaxis: {
      type: "datetime",
      axisBorder: {
        show: false,
      },
      axisTicks: {
        show: false,
      },
    },
    yaxis: {
      min: 0,
      labels: {
        formatter: function (value) {
          return (value / 1000).toFixed(2) + "k";
        },
      },
    },
    tooltip: {
      x: {
        format: "dd/MM/yy HH:mm",
      },
    },
  };

  var chart = new ApexCharts(
    document.querySelector("#cryptoCurrencyOne"),
    options
  );
  chart.render();

  // cryptoCurrencyTwo

  var options = {
    series: [
      {
        name: "Set 1",
        data: [45, 52, 38, 24, 33, 26, 21, 20, 6, 8, 15, 10],
      },
      {
        name: "Set 2",
        data: [35, 41, 62, 42, 13, 18, 29, 37, 36, 51, 32, 35],
      },
      {
        name: "Set 3",
        data: [87, 57, 74, 99, 75, 38, 62, 47, 82, 56, 45, 47],
      },
    ],
    chart: {
      height: 255,
      type: "line",
      zoom: {
        enabled: false,
      },
      toolbar: {
        show: false,
      },
    },
    dataLabels: {
      enabled: false,
    },
    stroke: {
      width: [4, 4, 4],
      curve: "smooth"
    },
    colors: ["#4A90E2", "#8e8da2"],
    title: {
      text: "",
      align: "left",
    },
    legend: legend,
    markers: {
      size: 0,
      hover: {
        sizeOffset: 6,
      },
    },
    xaxis: {
      categories: [
        "01 Jan",
        "02 Jan",
        "03 Jan",
        "04 Jan",
        "05 Jan",
        "06 Jan",
        "07 Jan",
        "08 Jan",
        "09 Jan",
        "10 Jan",
        "11 Jan",
        "12 Jan",
      ],
      axisBorder: {
        show: false,
      },
      axisTicks: {
        show: false,
      },
    },
    tooltip: {
      
    },
    grid: {
      borderColor: "#f1f1f1",
    },
  };

  var chart = new ApexCharts(
    document.querySelector("#cryptoCurrencyTwo"),
    options
  );
  chart.render();
});
