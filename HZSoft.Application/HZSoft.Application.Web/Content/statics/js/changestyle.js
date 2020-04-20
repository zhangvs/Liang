// JavaScript Document

//特殊寓意数字组合
var g = new Array();
g[0] = new Array("1314520", "一生一世我爱你");
g[1] = new Array("259695", "爱我就了解我");
g[2] = new Array("74839", "其实不想走");
g[3] = new Array("829575", "被爱就是幸福");
g[4] = new Array("20863", "爱你到来生");
g[5] = new Array("594320", "我就是想爱你");
g[6] = new Array("51020", "我依然爱你");
g[7] = new Array("520", "我爱你");
g[8] = new Array("1314920", "一生一世就爱你");
g[9] = new Array("584520", "我发誓我爱你");
g[10] = new Array("04551", "你是我唯一");
g[11] = new Array("77543", "猜猜我是谁");
g[12] = new Array("2010000", "爱你一万年");
g[13] = new Array("3344520", "生生世世我爱你");

function GetMobileArray(strInput) {
    var reg = new RegExp(/1[3-9]\d{9}/ig);
    var r = strInput.match(reg); // 尝试匹配搜索字符串。
    return (r);
}

/*
function GetMobileArray(var strInput)
{
var reg = new   RegExp(/^1[3|5]\d{9}$/);
var r = strInput.match(reg); // 尝试匹配搜索字符串。
return(r); 
}
*/

function SetMobileStyle(style) {
    if (style == 2) {
        SetMobileStyle2();
    }
    else {
        SetMobileStyle1();
    }
}

function SetMobileStyle_(style, d) {
    if (style == 2) {
        return SetMobileStyle2_(d);
    }
    else {
        return SetMobileStyle1_(d);
    }
}

function SetMobileStyle1() {
    $(".TBmobileData").each(function () {
        $(this).html(SetMobileStyle1_($(this).html()));
    });
}

function SetMobileStyle1_(myData) {
    var re = GetMobileArray(myData);
    if (re != null)//手机号码数组不为空
    {
        //按规则替换
        for (var i = 0; i < re.length; i++) {
            //myData=myData.replace(re[i],'<font color=red>'+re[i]+'</font>');
            var oldvalue = re[i];
            var newvalue = re[i];

            //规则2 , 单个数字重复3次以上(含3次) , 设为绿色
            if (oldvalue == newvalue)
                newvalue = newvalue.replace(/(\d)(\1{3,})/ig, "<span style='color:#FF6600;'>$1$2</span>");

            if (oldvalue == newvalue)
                newvalue = newvalue.replace(/(01234567|12345678|23456789|98765432|87654321|76543210|0123456|1234567|2345678|3456789|9876543|8765432|7654321|6543210|012345|123456|234567|345678|456789|987654|876543|765432|654321|543210|01234|12345|23456|34567|45678|56789|98765|87654|76543|65432|54321|43210|0123|1234|2345|3456|4567|5678|6789|9876|8765|7654|6543|5432|4321|3210)/ig, "<font color=red>$1</font>")
            //规则1 , 4个数以上的顺子(含4个)

            //规则2 , 单个数字重复3次以上(含3次) , 设为绿色
            if (oldvalue == newvalue)
                newvalue = newvalue.replace(/(\d)(\1{2,})/ig, "<span style='color:#FF6600;'>$1$2</span>");

            //规则3 , 组合-重复 ABABAB
            if (oldvalue == newvalue)
                newvalue = newvalue.replace(/(\d)([^\1])(\1)(\2)(\1)(\2)/ig, "<font color=red>$1</font><font color=blue>$2</font><font color=red>$3</font><font color=blue>$4</font><font color=red>$5</font><font color=blue>$6</font>");

            //规则4 , 组合-重复 ABABA
            if (oldvalue == newvalue)
                newvalue = newvalue.replace(/(\d)([^\1])(\1)(\2)(\1)/ig, "<font color=red>$1</font><font color=blue>$2</font><font color=red>$3</font><font color=blue>$4</font><font color=red>$5</font>");

            //规则5 , 组合-重复 ABAB
            if (oldvalue == newvalue)
                newvalue = newvalue.replace(/(\d)([^\1])(\1)(\2)/ig, "<font color=red>$1</font><font color=blue>$2</font><font color=red>$3</font><font color=blue>$4</font>");

            //规则6 , 组合-重复 AABBCC|AABBAA
            if (oldvalue == newvalue)
                newvalue = newvalue.replace(/(\d)(\1)([^\1])(\3)([^\3])(\5)/ig, "<font color=red>$1$2</font><font color=blue>$3$4</font><font color=green>$5$6</font>");

            //规则7 , 组合-重复 AABB
            if (oldvalue == newvalue)
                newvalue = newvalue.replace(/(\d)(\1)([^\1])(\3)/ig, "<font color=red>$1$2</font><font color=blue>$3$4</font>");

            //规则8 , 特殊寓意数字组合
            /*
            for (var k=0; k < g.length; k++)
            {
            newvalue=newvalue.replace(g[k][0],"<font color=red title="+g[k][1]+">"+g[k][0]+"</font>"+"<font color=gray  size='1'>("+g[k][1]+")</font>");
            }
            */

            myData = myData.replace(">" + oldvalue + "<", ">" + newvalue + "<");
        }
    }
    return myData;
}

function SetMobileStyle2() {
    $(".TBmobileData").each(function () {
        $(this).html(SetMobileStyle2_($(this).html()));
    });
}

function SetMobileStyle2_(myData) {
    var re = GetMobileArray(myData);
    if (re != null) {
        for (var i = 0; i < re.length; i++) {
            var oldvalue = re[i];
            var newvalue = re[i];
            newvalue = newvalue.replace(/^(\d{3})(\d{4})(\d{4})$/ig, "$1<font color=#FF8C69>$2</font><font color=#00BFFF>$3</font>");
            myData = myData.replace(oldvalue, newvalue);
        }
    }
    return myData;
}