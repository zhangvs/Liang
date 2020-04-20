/// <reference path="~/Scripts/jquery-easyui-1.5.2/jquery.min.js" />
/// <reference path="~/Scripts/jquery-easyui-1.5.2/jquery.easyui.min.js" />

$.ajaxSetup({
    cache: false
});


$headparameters = {};
if (window.top.EnterpriseRoleId) {
    $headparameters.EnterpriseRoleId = window.top.EnterpriseRoleId;
    delete window.top.EnterpriseRoleId;
    $.ajaxSetup({
        beforeSend: function (xhr, options) {
            xhr.setRequestHeader("EnterpriseRoleId", $headparameters.EnterpriseRoleId);
        }
    });
}


$top = window.top.$;
//设置默认的分页参数
if ($.fn.datagrid) {
    $.fn.datagrid.defaults.pageSize = 20;//这里一定要用datagrid.defaults.pageSize，用pagination.defaults.pageSize一直不行
    $.fn.datagrid.defaults.pageList = [20, 40, 60, 100];//这里一定要有，不然上面的也不起效
    $.fn.datagrid.defaults.loadFilter = function (data) {
        try {
            if (data.success === false) {
                if (data.errorcode === 1000) {
                    $top.messager.alert("提示", "用户长时间未操作,已掉线", "info", function () {
                        window.top.location = "/Login";
                    });
                    return { total: 0, rows: [] };
                }
                if (data.errorcode === 1001) {
                    $top.messager.alert("提示", "权限不足！", "info");
                    return { total: 0, rows: [] };
                }
                if (data.errorcode === 500) {
                    $top.messager.alert("提示", "发生内部错误，请联系客服！", "info");
                    return { total: 0, rows: [] };
                }
                $top.messager.alert("提示", data.message, "info");
                return { total: 0, rows: [] };
            }
        } catch (err) { return data; }
        return data;
    };
}
if ($.fn.form) {
    $.fn.form.defaults.iframe = false;

    $.extend($.fn.validatebox.defaults.rules, {
        //添加正则表达式验证
        regex: {
            validator: function (value, param) {
                var reg = new RegExp(param[0]);
                return reg.test(value);
            },
            message: '{1}'
        }
    });
}
$ajax = {
    post: function (url, param, func) {
        $.post(url, param, function (data) {
            var json = eval("(" + data + ")");
            if (json.success === false) {
                if (json.errorcode === 1000) {
                    $.messager.alert("提示", "用户长时间未操作,已掉线", "info", function () {
                        window.top.location = "/Login";
                    });
                    return;
                }
                if (json.errorcode === 1001) {
                    $.messager.alert("提示", "权限不足！", "info");
                    return;
                }
                if (json.errorcode === 500) {
                    $.messager.alert("提示", "发生内部错误，请联系客服！", "info");
                    return;
                }
            }
            func(json);
        });
    },
    get: function (url, param, func) {
        $.get(url, param, function (data) {
            var json = eval("(" + data + ")");
            if (json.success === false) {
                if (json.errorcode === 1000) {
                    $top.messager.alert("提示", "用户长时间未操作,已掉线", "info", function () {
                        window.top.location = "/Login";
                    });
                    return;
                }
                if (json.errorcode === 1001) {
                    $top.messager.alert("提示", "权限不足！", "info");
                    return;
                }
                if (json.errorcode === 500) {
                    $top.messager.alert("提示", "发生内部错误，请联系客服！", "info");
                    return;
                }
            }
            func(json);
        });
    }
};

$formatter = {
    datetime: function (val, row, index) {
        return val;
    },
    date: function (val, row, index) {
        if (val) {
            return val.split(' ')[0];
        } else {
            return val;
        }
    },
    month: function (val, row, index) {
        if (val) {
            return val.split(' ')[0].substr(0, 7);
        } else {
            return val;
        }
    },
    bool: function (val, row, index) {
        if (val || val > 0) {
            return "√";
        } else {
            return "×";
        }
    },
    boolfalsenull: function (val, row, index) {
        if (val || val > 0) {
            return "√";
        } else {
            return "";
        }
    }
};

$util = {
    padleft: function (str, n, c) {
        return (Array(n).join(c) + str).slice(-n);
    },
    openTab: function (subtitle, url, paramJson, icon) {
        window.top.paramData = paramJson;

        if (url.indexOf("?") > -1) {
            url += "&openbyutil=1";
        } else {
            url += "?openbyutil=1";
        }
        if ($headparameters.EnterpriseRoleId) {
            window.top.EnterpriseRoleId = $headparameters.EnterpriseRoleId;
            url += "&EnterpriseRoleId=" + $headparameters.EnterpriseRoleId;
        }
        window.top.addTab(subtitle, url, icon);
    },
    openEnterpriseTab: function (subtitle, url, paramJson, icon, header) {
        window.top.paramData = paramJson;
        window.top.EnterpriseRoleId = header.EnterpriseRoleId;
        if (url.indexOf("?") > -1) {
            url += "&openbyutil=1";
        } else {
            url += "?openbyutil=1";
        }
        url += "&EnterpriseRoleId=" + header.EnterpriseRoleId;
        window.top.addTab(subtitle, url, icon);
    },
    getTabParamData: function () {
        if (this.IsOpenbyUtil()) {
            return this.getParamData();
        }
        return null;
    },
    IsOpenbyUtil: function () {
        return this.request("openbyutil") === "1";
    },
    openWindow: function (config, paramJson, callback, paramFlag) {
        var defaultConfig = {
            width: 800,
            height: 600,
            title: '新窗口',
            showCloseMessage: true,
            closeMessage: '编辑内容尚未保存，是否关闭窗口?',
            url: '',
            modal: true,
            // toolbar: "#wintb",
            minimizable: false,
            maximizable: false,
            collapsible: false
        };
        $.extend(defaultConfig, config);
        if ($headparameters.EnterpriseRoleId) {
            window.top.EnterpriseRoleId = $headparameters.EnterpriseRoleId;
        }
        if (defaultConfig.url.indexOf("?") > -1) {
            defaultConfig.url += "&EnterpriseRoleId=" + $headparameters.EnterpriseRoleId;
        } else {
            defaultConfig.url += "?EnterpriseRoleId=" + $headparameters.EnterpriseRoleId;
        }

        window.top.openWindow(defaultConfig, paramJson, callback, paramFlag);
    },
    closeWindow: function (showCloseMessage, params) {
        window.top.closeWindow(showCloseMessage, params);
    },
    cancelWindow: function () {
        window.top.cancelWindow();
    },
    getParamData: function () {
        return window.top.paramData;
    },
    getParamFlag: function () {
        return window.top.paramFlag;
    },
    //遮罩
    showmask: function (msg) {
        if ($("#mask_div_mask-msg")) {
            $("#mask_div_mask-msg").remove();
            $("#loading_div_mask-msg").remove();
        }
        var arrry = [];
        var v = "正在加载.....";
        if (msg)
            v = msg;
        arrry.push('<div id="mask_div_mask-msg"  style="display:block; filter: alpha(opacity=75);opacity: 0.75;-moz-opacity:0.75;-khtml-opacity: 0.75;height:100%;width:100%; position:fixed;z-index:1000;top: 0px;left:0px;background-color:#000;">');
        arrry.push('</div>');
        arrry.push('<div id="loading_div_mask-msg" class="datagrid-mask-msg" style=" position:fixed;display: block;z-index:1001; top: 30%;left: 40%;">');
        arrry.push(v);
        arrry.push('</div>');
        $("body").append(arrry.join(""));
    },
    //隐藏遮罩
    hidemask: function () {
        if ($("#mask_div_mask-msg")) {
            $("#mask_div_mask-msg").remove();
            $("#loading_div_mask-msg").remove();
        }
    },
    request: function (name)// 调用方法 var 参数值1 = GetQueryString("参数名1"); 
    {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r !== null)
            return unescape(r[2]);
        return null;
    },
    exportexcel: function (url, par) {
        //创建表单对象
        var _form = $("<form></form>", {
            'method': 'post',
            'action': url,
            'target': '_blank',
            'style': 'display:none'
        }).appendTo($("body"));

        //将隐藏域加入表单
        for (var i in par) {
            _form.append($("<input>", { 'type': 'hidden', 'name': i, 'value': par[i] }));
        }
        for (var j in $headparameters) {
            _form.append($("<input>", { 'type': 'hidden', 'name': j, 'value': $headparameters[j] }));
        }
        _form.submit();
        _form.remove();
    },
    sendcall: function (target, apptoken, callnumber, data) {
        if ($(target).prop("disabled")) {
            return;
        }
        $(target).prop("disabled", "true");

        var link = "jihu://www.92hu.net?apptoken=" + apptoken + "&callnumber=" + callnumber;
        if (data) {
            for (var i in data) {
                link += "&" + i + "=" + encodeURI(data[i]);
            }
        }
        var $a = $('<a href="' + link + '">呼叫</a>');
        $('body').append($a);
        $a[0].click();
        $a.remove();

        setTimeout(function () {
            $(target).removeProp("disabled");
        }, 2000);
    },
    checkpermission: function (perm) {
        return $("#" + perm).length > 0;
    },
    addmuiltiselectbutton: function (jq) {
        $(".layout-panel-center .panel-title").append($("<a id='mselectbtn' href='javascript:void(0)'style='margin-left:10px;' class='fline-sysbtn' onclick='$util.showmuiltiselect(\"" + jq + "\")'>多选</a>"));
    },
    showmuiltiselect: function (jq) {
        if ($("#mselectbtn").html() === "多选") {
            $("#mselectbtn").html("单选");
            $("#" + jq).datagrid({
                singleSelect: false
            });
            $("#" + jq).datagrid("showColumn", "chk");
        } else {
            $("#mselectbtn").html("多选");
            $("#" + jq).datagrid({
                singleSelect: true
            });
            $("#" + jq).datagrid("hideColumn", "chk");
        }
    }
};

$convert = {
    datetostring: function (date) {
        return date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();
    }
};