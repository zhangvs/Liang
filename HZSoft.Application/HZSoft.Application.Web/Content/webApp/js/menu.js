
var organizeId = request('organizeId');
var city = request('city');

var css = { fontSize: '2em' };
setInterval(function () {
    $('.animate').animate(css, 3000, rowBack);
}, 3000);
function rowBack() {
    if (css.fontSize === '1em')
        css.fontSize = '2em';
    else if (css.fontSize === '2em')
        css.fontSize = '1em';
}

function f_search(telphoneId) {
    $.ajax({
        url: "/WeChatManage/Liang/SearchTelId",
        type: "POST",
        data: { telphoneId: telphoneId, organizeId: organizeId, city: city, rnd: Math.random() },
        success: function (data) {
            var dataresult = JSON.parse(data);
            var result = dataresult.entity;
            if (result == null) {
                layer.alert('无此号码', {
                    skin: 'layui-layer-molv', closeBtn: 0
                });
            }
            else {
                if (dataresult.state == 1) {
                    //询问框
                    layer.confirm('存在此号码：' + result.Telphone + '(' + result.Description + ')' + '<br/>【归属地】：' + result.City + result.Operator +
                        '<br/>【售价】：' + result.Price + '<br/>【成本】：' + result.MinPrice + '<br/>【利润】：' + result.ChaPrice + '<br/>【套餐】：' + result.Package + '<br/>是否继续编辑此号码？', {
                            btn: ['是', '否'] //按钮
                        }, function () {
                            window.location = "/WeChatManage/Liang/SearchForm?keyValue=" + result.TelphoneID + "&organizeId=" + organizeId;
                        });
                }
                else if (dataresult.state == 2) {
                    //可看低价不可编辑下架
                    layer.alert('存在此号码：' + result.Telphone + '(' + result.Description + ')' + '<br/>【归属地】：' + result.City + result.Operator +
                        '<br/>【售价】：' + result.Price + '<br/>【成本】：' + result.MinPrice + '<br/>【利润】：' + result.ChaPrice + '<br/>【套餐】：' + result.Package, {
                            skin: 'layui-layer-molv'
                            , closeBtn: 0
                        });
                }
                else {
                    //询问框
                    layer.confirm('存在此号码：' + result.Telphone + '(' + result.Description + ')' + '<br/>【归属地】：' + result.City + result.Operator +
                        '<br/>【价格】：' + result.Price + '<br/>【套餐】：' + result.Package + '<br/>是否电话咨询？', {
                            btn: ['是', '否'] //按钮
                        }, function () {
                            window.location = "tel:" + dataresult.tel;
                        });
                }
            }
        },
        error: function () {

        }
    });
}