/*------------------
       Admin
   --------------------*/

//MenuToggle
let toggle = document.querySelector('.toggle');
let navigation = document.querySelector('.navigation');
let main = document.querySelector('.main');

toggle.onclick = function () {
    navigation.classList.toggle('active');
    main.classList.toggle('active');
}

// add hovered class in selected list item
let list = document.querySelectorAll('.navigation li');
function activeLink() {
    list.forEach((item) =>
        item.classList.remove('hovered'));
    this.classList.add('hovered');
}
list.forEach((item) =>
    item.addEventListener('mouseover', activeLink));


//Column

$(document).ready(function () {
    $.getJSON("/ThongKe/GetData1", function (data) {
        var Names = []
        var Qts = []

        for (var i = 0; i < data.length; i++) {
            Names.push(data[i].name);
            Qts.push(data[i].count);
        }


        Highcharts.chart('container1', {
            chart: {
                type: 'column'
            },
            title: {
                text: 'Doanh thu theo đơn hàng'
            },
            subtitle: {
                text: ''
            },
            xAxis: {
                categories:
                    Names

            },
            yAxis: {
                min: 0,
                title: {
                    text: 'Doanh thu'
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                pointFormat: /*'<tr><td style="color:{series.color};padding:0">{series.name}: </td>'+*/
                    '<td style="padding:0"><b>{point.y} VND</b></td></tr>',
                footerFormat: '</table>',
                shared: true,
                useHTML: true
            },
            plotOptions: {
                column: {
                    pointPadding: 0.2,
                    borderWidth: 0
                }
            },
            series: [{
                name: 'Đơn hàng',
                data: Qts
            }]
        });

    });
});

// Label

$(document).ready(function () {
    $.getJSON("/ThongKe/GetData", function (data) {
        var Names = []
        var Qts = []

        for (var i = 0; i < data.length; i++) {
            Names.push(data[i].name);
            Qts.push(data[i].count);
        }


        Highcharts.chart('container', {
            chart: {
                type: 'areaspline',
                zoomType: 'xy'
            },
            credits: {
                //enabled: false
                text: 'Bakers House',
                href: 'https://hufi.edu.vn/',
                style: {
                    fontSize: "15px",
                    color: "blue"
                }
            },
            title: {
                text: 'Sản phẩm đã bán'
            },
            tooltip: {
                //animation: false,
                backgroundColor: '#333333',
                borderColor: 'red',
                borderRadius: 20,
                followPointer: false,
                style: {
                    color: '#ffffff'
                }
            },
            subtitle: {
                text: ''
            },
            xAxis: {
                categories: Names
            },
            yAxis: {
                title: {
                    text: 'Số lượng'
                }
            },
            plotOptions: {
                line: {
                    dataLabels: {
                        enabled: true
                    },
                    enableMouseTracking: false
                }
            },
            series: [{
                name: 'Số sản phẩm đã bán',
                data: Qts
            }]
        });

    });
});
