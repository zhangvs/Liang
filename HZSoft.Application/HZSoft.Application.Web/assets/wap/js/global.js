/**
 * 共用
 * */
$(function() {
    FastClick.attach(document.body);
});
//全局变量
var loading = false,page = 1;

var globalObj = {
	//倒计时
	codeTime:function(obj,wait) {
		if (wait == 0) {
			obj.removeClass(DISABLED).attr("disabled",false);obj.text("获取验证码");wait = 60;
		} else {
			obj.addClass(DISABLED).attr("disabled",true);obj.text("重新发送(" + wait + ")");
			wait--;
			setTimeout(function() { globalObj.codeTime(obj,wait); },1000);
		}
	},
	//禁止点击倒计时
	limitTime:function(obj,wait,speclass) {
		if (wait == 0) {
			obj.addClass(speclass);
			obj.removeClass(speclass+'_no');
			obj.find("i").text("");
		} else {
			obj.removeClass(speclass);
			obj.addClass(speclass+'_no');
			obj.find("i").text("等待(" + wait + ")");
			wait--;
			setTimeout(function() { global.limitTime(obj,wait,speclass); },1000);
		}
	},
	//暂停
	delayURL:function(wait,href) {
		if (wait == 0) {
			window.location.href = href;
		} else {
			wait--;
			setTimeout(function() { globalObj.delayURL(wait,href); },1000);
		}
	},
	//禁止底层滚动
	scrollLock:function(elemScroll) {
		window.ontouchmove=function(e){
			e.preventDefault && e.preventDefault();
	        e.returnValue=false;
	        e.stopPropagation && e.stopPropagation();
	        return false;
		}
		document.querySelector(elemScroll).addEventListener('touchmove', function(e) {
			e.preventDefault && e.preventDefault();
	        e.returnValue=true;
	        e.stopPropagation && e.stopPropagation();
			return true;
		})
	},
	//解除禁止滚动
	scrollUnlock:function() {
		window.ontouchmove=function(e){
			e.preventDefault && e.preventDefault();
	        e.returnValue=true;
	        e.stopPropagation && e.stopPropagation();
	        return true;
		}
	},
	//滚动固定
	scrollFix:function(fix,fixTop) {
		//页面与顶部高度
		var docTop = Math.max(document.body.scrollTop, document.documentElement.scrollTop);
        if (fixTop < docTop) {
            fix.css({'position': 'fixed','top': '0','left': '0'});
        } else {
            fix.css({'position': 'relative'});
        }
	},
	//取得加载更多
	loadListMore:function(url,elemId,etcPara) {
		page = page+1;
		//console.log(url+'?page='+page);
		$.ajax({type:"post",url: url,data: 'page='+page,dataType: "json",
			beforeSend: function() {
				$(".weui-loadmore").html('<i class="weui-loading"></i><span class="weui-loadmore__tips">正在加载下一页</span>');
			},
			success: function(dataObj){
				$("#"+elemId).append(dataObj.data.html)
				$(".infinite-scroll-preloader").hide();
				//倒计时
//				if( etcPara == 'countDown' ){
//					countDownInt();
//				}
				if( dataObj.status ){
					loading = false;
				}else{
					$(".weui-loadmore").html(dataObj.msg);
				}
			}
		});
	    return false;
	},
	//筛选
	listFilter:function (elemId) {
		$("#"+elemId+" .screen-card .card").click(function(){
			//对应标题
			$("#"+elemId+" .screen-card .card").eq($(this).index()).toggleClass("active").siblings().removeClass("active");
			//对应内容
			$("#"+elemId+" .screen-cont .cont").eq($(this).index()).toggleClass("active").siblings().removeClass("active");
			//设置内容滚蛋区高度
			var heightAll 	 = $(window).innerHeight ();
			var heightFooter = $('.weui-tabbar').outerHeight();
			var filterOffset = $("#"+elemId)[0].scrollTop;
			var filterHeight = heightAll - heightFooter - filterOffset - 40;
			if( $("#"+elemId+" .screen-card .card").hasClass("active") ){
				$('.screen-cont').show();
				$("#"+elemId+" .screen-cont .cont").eq($(this).index()).show().css("height",filterHeight).find(".grade").css("height",filterHeight);
			}else{
				$('.screen-cont').hide();
			}
//			//锁定底层滚动
//			if( $("#"+elemId+" .screen-cont .cont").hasClass('active') ){
//				$('body').addClass('ovh');
//				globalObj.scrollLock('.screen-cont');
//			}else{
//				$('body').removeClass('ovh');
//				globalObj.scrollUnlock();
//			}
		});
	},
	//区域选择
	cityPicker:function (elemId,title,showDistrict) {
		$(elemId).cityPicker({
			title: title,
			onChange: function(res){
				var codes = res.value.join(',')
				$(elemId+'Code').val(codes);
			}
		});
	},
	//微信分享
	wxShare:function () {
		var sharetit = document.title;
		var sharetxt = $('meta[name="description"]').attr("content");
		var shareimg = $('meta[name="shareimg"]').attr("content");
		var shareurl = location.href;
			
		if( shareimg ){
			$.ajax({type:"post",url: baseUrl+"/public/getWxShare",data: "pageurl="+encodeURI(shareurl),dataType: "json",async:true,success: function(dataObj){
				if( dataObj.status ){
					wx.config({
					    debug: false,
					    appId: dataObj.data.appId,
					    timestamp: dataObj.data.timestamp,
					    nonceStr: dataObj.data.nonceStr,
					    signature: dataObj.data.signature,
					    jsApiList: ['previewImage','getNetworkType','checkJsApi','onMenuShareTimeline','onMenuShareAppMessage','onMenuShareQQ','onMenuShareWeibo','onMenuShareQZone',]
					});
					wx.ready(function () {
						var shareData = {
							title:sharetit,
							desc: sharetxt,
							link: shareurl,
							imgUrl: shareimg
						};
						wx.onMenuShareAppMessage(shareData);//分享给朋友
						wx.onMenuShareTimeline(shareData);	//分享到朋友圈
						wx.onMenuShareQQ(shareData);		//分享到QQ
						wx.onMenuShareWeibo(shareData);		//分享到腾讯微博
						wx.onMenuShareQZone(shareData);		//分享到QQ空间
					});
					wx.error(function(res){
						//layer.msg('分享接口故障');
					});
				}else{
					console.log('微信分享数据获取失败！');
				}
			}});
		}
	},
	fileUpload:function(event,option) {
    	var files 		= event.target.files;
    	var thumb  		= typeof($(event.target).attr('thumb'))!='undefined'?1:0;
    	var thumb_w  	= typeof($(event.target).attr('thumb_w'))!='undefined'?$(event.target).attr('thumb_w'):'100';
    	var thumb_h  	= typeof($(event.target).attr('thumb_h'))!='undefined'?$(event.target).attr('thumb_h'):'100';
    	var acceptType  = typeof($(event.target).attr('acceptType'))!='undefined'?$(event.target).attr('acceptType'):'images';
    	var multiple  	= typeof($(event.target).attr('multiple'))!='undefined'?true:false;
    	var maxCount  	= typeof($(event.target).attr('maxCount'))!='undefined'?$(event.target).attr('maxCount'):'10';
    	var saveField  	= typeof($(event.target).attr('saveField'))!='undefined'?$(event.target).attr('saveField'):'savefile';
    	var that 		= $(event.target);
        // 如果没有选中文件，直接返回  
        if (files.length === 0) {
            return false;
        }
        if ( files.length > maxCount) {
        	$.toptip('最多只能上传' + maxCount + '张图片!', 'warning');
        	return false;
        }
        if(typeof FileReader == "undefined"){
        	$.toptip('您的浏览器不支持FileReader对象！', 'warning');
        	return false;
        }
		//开启loadding
		$.showLoading();
        //开始处理
        for (var i = 0, len = files.length; i < len; i++) {
        	var file = files[i];
        	
//        	//每循环一次都要重新new一个FileReader实例
//        	var reader = new FileReader();
//            reader.onload = function(loadEvent) {
            	//上传表单处理
                var formData = new FormData();
                	formData.append('upFile',file);
                	formData.append('accept',acceptType);
                	formData.append('thumb',thumb);
                	formData.append('thumb_w',thumb_w);
                	formData.append('thumb_h',thumb_h);
                //上传至服务器
                $.ajax({type:'post',url: option.urlUpl,data:formData,contentType:false,processData:false,async: false,
            		beforeSend: function() { },
            		success:function(dataObj){
                		//将json字符串转换成json对象
                	    var dataObj = JSON.parse(dataObj);
    					if ( dataObj.status ){
        					//关闭loadding
        					$.hideLoading();
	    					//返回文件
	    					var filedir  = dataObj.data.filedir;
	    					if( dataObj.data.thumbdir !='' ){
	    						var thumbdir = dataObj.data.thumbdir; //缩略图
	    					}else{
	    						var thumbdir = dataObj.data.filedir;
	    					}
	    					var fileExt= filedir.substring(filedir.lastIndexOf(".")+1,filedir.length);//后缀名  
	    					if( acceptType == 'images' ){ //图片时
	    						var fileDisplay = baseUrl + thumbdir;
	    					}else{
	    						var fileDisplay = baseUrl + '/assets/res/images/' + fileExt + '.png';
	    					}
	    					if( multiple ){
	    						//多文件
	    						var $preview = $('<li class="weui-uploader__file" style="background-image:url(' + fileDisplay + ')"><input type="hidden" name="'+saveField+'[]" value="' + filedir + '" /><i class="iconfont close">&#xe609;</i></li>');
	    						//加入文件预览
	    						that.parent().siblings(".uploadFileShow").append($preview);
	    					}else{
	    						//删除旧文件
	    						var $preview_old = '';
	    						var oldFile = $(event.target).parent().siblings().find("input[type='hidden']").val();
				    			if( oldFile !='' ){
				    				$preview_old= '<input type="hidden" name="'+saveField+'_old" value="' + oldFile + '" />';
				    			}
	    						//新文件
	    						var $preview_new = $('<li class="weui-uploader__file" style="background-image:url(' + fileDisplay + ')">'+
				    								 '	<input type="hidden" name="'+saveField+'" value="' + filedir + '" />'+$preview_old+'<i class="iconfont close">&#xe609;</i>'+
	    											 '</li>');
	    						//$.ajax({type:"post",url: option.urlDel,dataType:'json',data:"filedir="+oldFile,success: function(dataObj){ }});
	    						//加入文件预览
	    						that.parent().siblings(".uploadFileShow").html($preview_new);
	    					}
    					}else{
        					//关闭loadding
        					$.hideLoading();
        					$.toast(dataObj.msg, "cancel");
    					}
                	},
        			error: function(XMLHttpRequest, textStatus, errorThrown){
    					//关闭loadding
    					$.hideLoading();
    					$.toast("上传不成功，建议切换至WiFi网络环境重新尝试", "text");
        			}
                });
//            };
//            reader.readAsDataURL(file);
        }
    }
};
//手机验证码
$(document).on("click",".btnMobileCode",function(){
	var btnObj 	= $(this);
	var mobile = btnObj.closest('form').find(".mobile").val();
	if( mobile == '' ){
		$.toptip('请先填写手机号!', 'error');
	}
    $.ajax({type:'post', url: baseUrl+'/user/getMobileCode/', dataType: 'json',data:'ajax=1&mobile='+mobile,success: function(resObj) {
    	if( resObj.data.code == '' ){
    		$.toast(resObj.msg, resObj.status?'success':'cancel');
            if( resObj.status ){
            	globalObj.codeTime($('.btnMobileCode'),'60');
            }
    	}else{
    		//直接显示
            if( resObj.status ){
            	btnObj.text(resObj.data.code).css({"font-size":"18px","font-weight":"bold","color":"#FF5722"});
            }else{
            	$.toptip(resObj.msg, 'error');
            }
    	}
    }});
    return false;
});
//邮箱验证码
$(document).on("click",".btnEmailCode",function(){
	var form = $(this).closest('form');
	var email = $(form).find(".email").val();
    $.ajax({type: 'post', url: baseUrl+'/user/getEmailCode/', dataType: 'json',data:'ajax=1&email='+email,success: function(resObj) {
    	if( resObj.data.code == '' ){
    		$.toast(resObj.msg, resObj.status?'success':'cancel');
            if( resObj.status ){
            	globalObj.codeTime($('.btnMobileCode'),'60');
            }
    	}else{
    		//直接显示
            if( resObj.status ){
            	btnObj.text(resObj.data.code).css({"font-size":"18px","font-weight":"bold","color":"#FF5722"});
            }else{
            	$.toptip(resObj.msg, 'error');
            }
    	}
    }});
    return false;
});
//图片验证码
$(document).on("click",".btnCaptchaCode",function(){
	$(this).attr('src',baseUrl+'/user/getCaptchaCode/?ajax=1&time='+Math.round(Math.random()*10));
});
//返回上页
$(document).on("click", ".goBack", function(e) {
	if(window.history.length > 1){
		window.history.back();
	}else{
		window.location.href = "/";
	}
});
/*快捷登录绑定*/
$(".authLogin").on('click', function(){
	var type = $(this).attr("data-type");
	var memo = $(this).attr("data-memo");
	$.showLoading(memo);
    setTimeout(function() {
    	$.hideLoading();
	}, 3000)
	location.href = baseUrl+'/user/authLogin/?type='+type;
});
/*解除绑定*/
$(".clearAuth").on('click', function(){
	var type = $(this).attr("data-type");
	var memo = $(this).attr("data-memo");
	$.modal({
        title: "注意",text: '确定'+memo+'吗？解绑后需要使用账户、密码登陆。',
        buttons: [
          { text: "确定", onClick: function(){
    		$.ajax({type:"post",url:baseUrl+"/user_basic/auth/",dataType:'json',data:"cmd=clear&type="+type,success: function(dataObj){
    			if(dataObj.status){
    				//刷新当前页面
    				location.reload();
    			}else{
    				console.log(dataObj);
    			}
    		}});
          }},
          { text: "取消", className: "default"},
        ]
	});
});
//pop
$(document).on("click", ".btnKefu", function(e) {
	var popId = $(this).attr('data-id');
	$("#"+popId).popup();
});
//键盘遮挡
var windheight = $(window).height();  /*未唤起键盘时当前窗口高度*/    
$(window).resize(function(){
	var docheight = $(window).height();  /*唤起键盘时当前窗口高度*/  
	if(docheight < windheight){            /*当唤起键盘高度小于未唤起键盘高度时执行*/
	   if( $(".weui-tabbar").length > 0 ){
		   $(".weui-tabbar").css('position','static');
	   }
	   if( $(".weui-tabbar").length > 0 ){
		   $(".vision_bottom").css('position','static');
	   }
	}else{
	   if( $(".weui-tabbar").length > 0 ){
		   $(".weui-tabbar").css('position','fixed');
	   }
	   if( $(".weui-tabbar").length > 0 ){
		   $(".vision_bottom").css('position','fixed');
	   }
	}
});
if (navigator.userAgent.toLocaleLowerCase().includes('iphone')) {
	$(":text").focus(function(){
		this.scrollIntoView(true);
	}).blur(function(){
		this.scrollIntoView(false);
	});  
}
//微信分享
$(document).ready(function(){
	globalObj.wxShare();
});

//$(document).on("click", "#formSubmit", function(e) {
//	$(".Validform").submit();
//});
//通用表单提交(AJAX方式)
if( $("#formSubmit").length > 0 ){
	$(".Validform").Validform({
		btnSubmit:"#formSubmit",tiptype:1,tipSweep:true,postonce:true,ajaxPost:true,
		datatype:{
			"*6-20": /^[^\s]{6,20}$/,
			"z2-4" : /^[\u4E00-\u9FA5\uf900-\ufa2d]{2,4}$/,
			"pass": /^[\S]{6,12}$/,
			"username": function(gets,obj,curform,regxp){
				//参数gets是获取到的表单元素值，obj为当前表单元素，curform为当前验证的表单，regxp为内置的一些正则表达式的引用;
				if(!new RegExp("^[a-zA-Z0-9_\u4e00-\u9fa5\\s·]+$").test(gets)){
					return false; //'用户名不能有特殊字符';
				}
				if(/(^\_)|(\__)|(\_+$)/.test(gets)){
					return false; //'用户名首尾不能出现下划线\'_\'';
				}
				if(/^\d+\d+\d$/.test(gets)){
					return false; //'用户名不能全为数字';
				}
			}
		},
		tiptype:function(msg,o,cssctl){
			//1：正在检测/提交数据，2：通过验证，3：验证失败，4：提示ignore状态
			if( o.type=='3' ){	
				$.toptip($(o.obj).attr("errormsg"), 'warning');
			}
			if( o.type=='1' ){
				$.showLoading();
			}
		},
		beforeCheck:function(curform){
			//在表单提交执行验证之前执行的函数，curform参数是当前表单对象。
			//这里明确return false的话将不会继续执行验证操作;
		},
		beforeSubmit:function(curform){
			//在验证成功后，表单提交前执行的函数，curform参数是当前表单对象。
			//这里明确return false的话表单将不会提交
		},
		callback:function(resObj){	//返回数据data是json对象
			$.hideLoading();
			//状态显示
			if( resObj.status ){
				$.toast(resObj.msg,30000);
			}else{
				$.toptip(resObj.msg, resObj.status?'success':'error',30000);
			}
    		//成功处理
	    	if( resObj ){
				//推广转化
				if( $("#orderSubmit").val() == '1' ){
					//头条推广转化（点击提交订单）
		    		if( typeof(trackToutiaoPara) != 'undefined' && typeof(trackToutiaoPara.trackId) != 'undefined' && trackToutiaoPara.trackOrder ){
		    			trackObj.trackToutiao(trackToutiaoPara.trackId);
		    		}
	        		//广点通推广转化（点击提交订单）
	        		if( typeof(tracktGdtPara) != 'undefined' && typeof(tracktGdtPara.init) != 'undefined' && tracktGdtPara.trackOrder ){
	        			trackObj.trackKuaishou(tracktGdtPara.init);
	        		}
	        		//快手推广转化（点击提交订单）
	        		if( typeof(tracktKsPara) != 'undefined' && typeof(tracktKsPara.trackId) != 'undefined' && tracktKsPara.trackOrder ){
	        			trackObj.trackKs(tracktKsPara.trackId);
	        		}
				}
	    		switch(resObj.data.code){
	    			case '1':
	    				//3秒后跳转
	    				globalObj.delayURL(3,resObj.data.gotoUrl)
	    				break;
	    			case '2':
	    				//刷新当前页面
	    				location.reload();
	    				break;
	    		}
	    		return false;
	    	}
		}
	});
}