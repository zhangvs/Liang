
var myChartPieB2;
var myChartPieB3;
var myChartPieOperate;
var myChartPieWH;

var echarts;


//前二前三
var b2_datas = [];
var b3_datas = [];
//运营商
var operate_datas = [];
//尾号
var w1_datas = [];
var w2_datas = [];
var w3_datas = [];
var w4_datas = [];

function clearData() {
    b2_datas = [];
    b3_datas = [];
    operate_datas = [];
    w1_datas = [];
    w2_datas = [];
    w3_datas = [];
    w4_datas = [];

}

//创建ECharts图表方法  
function DrawEChartPieB2() {
    myChartPieB2 = echarts.init(document.getElementById('mainPieB2'), 'chalk');
    optionPieB2 = {
        title: {
            text: '号码前2位分析',
            x: 'center'
        },
        tooltip: {
            trigger: 'item',
            formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        toolbox: {
            show: true,
            feature: {
                mark: { show: true },
                dataView: { show: true, readOnly: false },
                magicType: {
                    show: true,
                    type: ['pie', 'funnel']
                },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        series: [
            {
                name: '',
                type: 'pie',
                radius: '65%',
                center: ['50%', '50%'],
                data: [],
                itemStyle: {
                    emphasis: {
                        shadowBlur: 10,
                        shadowOffsetX: 0,
                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                    }
                }
            }
        ]
    };
    myChartPieB2.setOption(optionPieB2);
}


//创建ECharts图表方法  
function DrawEChartPieB3() {
    myChartPieB3 = echarts.init(document.getElementById('mainPieB3'), 'chalk');
    optionPieB3 = {
        title: {
            text: '号码前3位分析',
            x: 'center'
        },
        tooltip: {
            trigger: 'item',
            formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        toolbox: {
            show: true,
            feature: {
                mark: { show: true },
                dataView: { show: true, readOnly: false },
                magicType: {
                    show: true,
                    type: ['pie', 'funnel']
                },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        series: [
            {
                name: '',
                type: 'pie',
                radius: '65%',
                center: ['50%', '50%'],
                data: [],
                itemStyle: {
                    emphasis: {
                        shadowBlur: 10,
                        shadowOffsetX: 0,
                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                    }
                }
            }
        ]
    };
    myChartPieB3.setOption(optionPieB3);
}


//创建ECharts图表方法  
function DrawEChartPieOperate() {
    myChartPieOperate = echarts.init(document.getElementById('mainPieOperate'), 'chalk');
    optionPieOperate = {
        title: {
            text: '运营商分析',
            x: 'center'
        },
        tooltip: {
            trigger: 'item',
            formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        toolbox: {
            show: true,
            feature: {
                mark: { show: true },
                dataView: { show: true, readOnly: false },
                magicType: {
                    show: true,
                    type: ['pie', 'funnel']
                },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        series: [
            {
                name: '',
                type: 'pie',
                radius: '65%',
                center: ['50%', '50%'],
                data: [],
                itemStyle: {
                    emphasis: {
                        shadowBlur: 10,
                        shadowOffsetX: 0,
                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                    }
                }
            }
        ]
    };
    myChartPieOperate.setOption(optionPieOperate);
}

//创建ECharts图表方法  
function DrawEChartPieWH() {
    myChartPieWH = echarts.init(document.getElementById('mainPieWH'), 'chalk');
    optionPieWH = {
        title: {
            text: '尾号后四位分析',
            x: 'center'
        },
        tooltip: {
            trigger: 'item',
            formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        toolbox: {
            show: true,
            feature: {
                mark: { show: true },
                dataView: { show: true, readOnly: false },
                magicType: {
                    show: true,
                    type: ['pie', 'funnel']
                },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        series: [
            {
                name: '',
                type: 'pie',
                radius: '35%',
                center: ['20%', '50%'],
                data: [],
                itemStyle: {
                    emphasis: {
                        shadowBlur: 10,
                        shadowOffsetX: 0,
                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                    }
                }
            },
            {
                name: '',
                type: 'pie',
                radius: '35%',
                center: ['40%', '50%'],
                data: [],
                itemStyle: {
                    emphasis: {
                        shadowBlur: 10,
                        shadowOffsetX: 0,
                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                    }
                }
            },
            {
                name: '',
                type: 'pie',
                radius: '35%',
                center: ['60%', '50%'],
                data: [],
                itemStyle: {
                    emphasis: {
                        shadowBlur: 10,
                        shadowOffsetX: 0,
                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                    }
                }
            },
            {
                name: '',
                type: 'pie',
                radius: '35%',
                center: ['80%', '50%'],
                data: [],
                itemStyle: {
                    emphasis: {
                        shadowBlur: 10,
                        shadowOffsetX: 0,
                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                    }
                }
            }
        ]
    };
    myChartPieWH.setOption(optionPieWH);
}