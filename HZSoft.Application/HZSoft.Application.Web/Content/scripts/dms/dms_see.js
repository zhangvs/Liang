var echarts;
var see_names = [];
var see_counts = [];
var sum_counts = 0;

function clearData() {
    see_names = [];
    see_counts = [];
    sum_counts = 0;
}


//创建ECharts图表方法  
function DrawEChartSee() {
    $("#mainSee").css("height", 400);
    myChartSee = echarts.init(document.getElementById('mainSee'), 'chalk');

    optionSee = {
        title: {
            text: '靓号页面访问数据'
        },
        legend: {
            top: 20,
            data: ['访问数量'],
            selected: {
                '访问数量': true
            }
        },
        tooltip: {
        },
        toolbox: {
            show: true,
            orient: 'vertical',
            feature: {
                mark: {
                    show: true
                },
                dataView: {
                    show: true,
                    readOnly: false,
                    optionToContent: function (opt) {
                        var hh = (opt.xAxis[0].data.length + 1) * 22;
                        if (hh < 400) {
                            hh = 400;
                        }
                        $("#mainSee").css("height", hh);
                        var series = opt.series;
                        var axisData = opt.xAxis[0].data;
                        var table = '<table class="dataView" ><tbody><tr><td>公司</td>';
                        for (var i = 0, l = series.length; i < l; i++) {
                            table += '<td>' + series[i].name + '</td>'
                        };

                        table += '</tr>';
                        for (var m = 0, g = axisData.length; m < g; m++) {
                            table += '<tr><td>' + axisData[m] + '</td>';
                            for (var n = 0, l = series.length; n < l; n++) {
                                var val = series[n].data[m];

                                table += '<td>' + val + '</td>'
                            }
                            table += '</tr>';
                        };

                        table += '<tr><td>合计</td><td>' + sum_counts + '</td></tr>';
                        table += '</tbody></table>';
                        return table;
                    }
                },
                magicType: {
                    show: true,
                    type: ['line', 'bar']
                },
                restore: {
                    show: true
                },
                saveAsImage: {
                    show: true
                }
            }
        },
        yAxis: {
            type: 'value',
            scale: true,
            axisLabel: {
                formatter: function (v) {
                    return v
                }
            },
            splitArea: {
                show: true
            }

        },
        xAxis: {
            type: 'category',
            axisLabel: {
                interval: 0,
                rotate: 45,//倾斜度 -90 至 90 默认为0  
                //margin: 2,
                //textStyle: {
                //    color: "#99ff99"
                //}
            },
            data: []
        },
        visualMap: {
            top: 20,
            right: 40,
            pieces: [
            { min: 10000 }, // 不指定 max，表示 max 为无限大（Infinity）。
            { min: 2000, max: 5000 },
            { min: 1000, max: 2000 },
            { min: 500, max: 1000, },
            //{value: 123, label: '123（自定义特殊颜色）', color: 'grey'}, // 表示 value 等于 123 的情况。
            { max: 500 }     // 不指定 min，表示 min 为无限大（-Infinity）。
            ],
            textStyle: {
                color: "#ffffff"
            },
            color: ['#d94e5d', '#eac736', '#50a3ba']
        },
        series: [
            {
                type: 'bar',
                name: '订单数量',
                itemStyle: {
                    normal: {
                        label: {
                            show: true,
                            position: 'top'
                        }
                    }
                },
                markLine: {
                    data: [
                        { type: 'average', name: '平均值' }
                    ]
                },
                data: []
            }
        ]
    };
    myChartSee.setOption(optionSee);
}

