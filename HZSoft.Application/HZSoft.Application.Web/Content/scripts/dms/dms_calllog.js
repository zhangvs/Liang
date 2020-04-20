var echarts;
var calllog_names = [];
var calllog_minutes = [];
var calllog_callcounts = [];
var calllog_yescallcounts = [];
var calllog_nocallcounts = [];
var calllog_jtls = [];
var wash_getcounts = [];
var wash_surpluscounts = [];

var sum_minutes = 0;
var sum_callcounts = 0;
var sum_yescallcounts = 0;
var sum_nocallcounts = 0;
var sum_jtls = 0;
var sum_getcounts = 0;
var sum_surpluscounts = 0;

function clearData() {
    calllog_names = [];
    calllog_minutes = [];
    calllog_callcounts = [];
    calllog_yescallcounts = [];
    calllog_nocallcounts = [];
    calllog_jtls = [];
    wash_getcounts = [];
    wash_surpluscounts = [];

    sum_minutes = 0;
    sum_callcounts = 0;
    sum_yescallcounts = 0;
    sum_nocallcounts = 0;
    sum_jtls = 0;
    sum_getcounts = 0;
    sum_surpluscounts = 0;
}

//创建ECharts图表方法  
function DrawEChartCallLog() {
    $("#mainCallLog").css("height", 500);
    myChartCallLog = echarts.init(document.getElementById('mainCallLog'), 'chalk');

    optionCallLog = {
        title: {
            text: '员工通话时长排名'
        },
        legend: {
            top: 20,
            data: ['通话时长', '通话数', '接通数', '未接通数', '接通率', '获取数', '剩余数'],
            selected: {
                '通话时长': true,
                '通话数': false,
                '接通数': false,
                '未接通数': false,
                '接通率': false,
                '获取数': false,
                '剩余数': false
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
                        if (hh < 500) {
                            hh = 500;
                        }
                        $("#mainCallLog").css("height", hh);
                        var series = opt.series;
                        var axisData = opt.xAxis[0].data;
                        var table = '<table class="dataView" ><tbody><tr><td>员工</td>';
                        for (var i = 0, l = series.length; i < l; i++) {
                            table += '<td>' + series[i].name + '</td>'
                        };

                        table += '</tr>';
                        for (var m = 0, g = axisData.length; m < g; m++) {
                            table += '<tr><td>' + axisData[m] + '</td>';
                            for (var n = 0, l = series.length; n < l; n++) {
                                var val = series[n].data[m];
                                if (n==4) {
                                    table += '<td>' + val + '%</td>'
                                } else {
                                    table += '<td>' + val + '</td>'
                                }
                                
                            }
                            table += '</tr>';
                        };

                        table += '<tr><td>合计</td><td>' + sum_minutes + '</td><td>' + sum_callcounts + '</td><td>' + sum_yescallcounts + '</td><td>' + sum_nocallcounts + '</td><td>' + sum_jtls + '%</td><td>' + sum_getcounts + '</td><td>' + sum_surpluscounts + '</td></tr>';
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
        //visualMap: {
        //    top: 20,
        //    right: 40,
        //    pieces: [
        //    { min: 120 }, // 不指定 max，表示 max 为无限大（Infinity）。
        //    { min: 90, max: 120 },
        //    { min: 60, max: 90 },
        //    { min: 30, max: 60, },
        //    //{value: 123, label: '123（自定义特殊颜色）', color: 'grey'}, // 表示 value 等于 123 的情况。
        //    { max: 30 }     // 不指定 min，表示 min 为无限大（-Infinity）。
        //    ],
        //    textStyle: {
        //        color: "#ffffff"
        //    },
        //    color: ['#d94e5d', '#eac736', '#50a3ba']
        //},
        series: [
            {
                type: 'bar',
                name: '通话时长',
                itemStyle: {
                    normal: {
                        label: {
                            show: true,
                            position: 'top'
                        }
                    }
                },
                markLine : {
                    data : [
                        {type : 'average', name: '平均值'}
                    ]
                },
                data: []
            },
            {
                type: 'bar',
                name: '通话数',
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
            },
            {
                type: 'bar',
                name: '接通数',
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
            },
            {
                type: 'bar',
                name: '未接通数',
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
            },
            {
                type: 'bar',
                name: '接通率',
                itemStyle: {
                    normal: {
                        label: {
                            show: true,
                            position: 'top',
                            formatter: function (params, ticket, callback) {
                                return params.data + '%'
                            },
                        }
                    }
                },
                markLine: {
                    data: [
                        { type: 'average', name: '平均值' }
                    ]
                },
                data: []
            },
            {
                type: 'bar',
                name: '获取数',
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
            },
            {
                type: 'bar',
                name: '剩余数',
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
            },
        ]
    };
    myChartCallLog.setOption(optionCallLog);
}

