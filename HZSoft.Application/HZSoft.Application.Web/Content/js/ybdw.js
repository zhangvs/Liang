// JavaScript Document

$(document).ready(
function () {
$('.nav li').hover(function(){$(this).addClass('hover').siblings(this).removeClass('hover')});	
var aInp=$('.login_username');
var i=0;
var sArray=[];
for(i=0; i<aInp.length; i++)
{
aInp[i].index=i;
sArray.push(aInp[i].value);
aInp[i].onfocus=function()
{
if(sArray[this.index]==aInp[this.index].value)
{
aInp[this.index].value='';
}
$('.tx_user').css('color','#333');
$('.tx_sec').css('color','#333');
};
aInp[i].onblur=function()
{
if(aInp[this.index].value=='')
{
aInp[this.index].value=sArray[this.index];
}
};
}
		});
<!--公告板滚动-->
$(function(){
(function (win){
var callboarTimer;
var callboard = $('#callboard');
var callboardUl = callboard.find('ul');
var callboardLi = callboard.find('li');
var liLen = callboard.find('li').length;
var initHeight = callboardLi.first().outerHeight(true);
win.autoAnimation = function (){
if (liLen <= 1) return;
var self = arguments.callee;
var callboardLiFirst = callboard.find('li').first();
callboardLiFirst.animate({
marginTop:-initHeight
}, 500, function (){
clearTimeout(callboarTimer);
callboardLiFirst.appendTo(callboardUl).css({marginTop:0});
callboarTimer = setTimeout(self, 5000);
});
}
callboard.mouseenter(
function (){
clearTimeout(callboarTimer);
}).mouseleave(function (){
callboarTimer = setTimeout(win.autoAnimation, 5000);
});
}(window));
setTimeout(window.autoAnimation, 5000);
});


$(document).ready(function(){
	$(".setcolor tr td").mouseover(function(){
		$(this).parent().find("td").css("background-color","#ebebeb");
	});
	/* 当鼠标在表格上移动时，离开的那一行背景恢复 */
	 $(".setcolor tr td").mouseout(function(){
		var bgc = $(this).parent().attr("bg");
		$(this).parent().find("td").css("background-color",bgc);
	});
	var color="#fafafa"
	$(".setcolor tr:even td").css("background-color",color);  //改变偶数行背景色
	/* 把背景色保存到属性中 */
	$(".setcolor tr:even").attr("bg",color);
	$(".setcolor tr:odd").attr("bg","#fff");
})
$(function(){
	$(".select").hover(
			function () {
				$(this).find(".msgNav").show();
			},
			function () {
				$(this).find(".msgNav").hide();
			}
	);
	$(".title-btn").on("click",function(){
		if ( $(this).hasClass("on") ){
			$(this).removeClass("on");
			$(this).parent(".box-content").addClass("off");
		}else{
			$(this).addClass("on");
			$(this).parent(".box-content").removeClass("off");
		}
	});
});