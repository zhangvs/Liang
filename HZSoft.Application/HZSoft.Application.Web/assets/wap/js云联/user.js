/**
 * 共用
 * */
//配送方式
$(document).on("change",".expressType",function(){
	var expressType = $(this).val();
	if( expressType=='1' ){
		$.toast("温馨提示！确定本人到公司取卡办理请选择自取 ；否则请选择快递 按照要求填写收件信息！", "text");
		$(".regionArea").hide();
	}else{
		$(".regionArea").show();
	}
});
//支付方式
$(document).on("click",".payType",function(){
	var payType = $(this).val();
//	if( payType=='offline' ){
//		$(".payMemo").show();
//	}else{
//		$(".payMemo").hide();
//	}
});
/*快捷登录*/
$(document).on("click",".btnAuth",function(){
	var type = $(this).attr("data-type");
	var memo = $(this).attr("data-memo");
	$.toast(memo, "text", function(toast) {
        location.href = baseUrl +'/WeChatManage/user_index/authLogin/?type='+type;
	});
});
//退出
$(document).on("click",".btnLogOut",function(){
	$.confirm("你真的确定要退出系统吗？", "确认删除?", function() {
        $.ajax({
            type: "post", url: baseUrl +"/WeChatManage/user_index/logout/",dataType:'json',data:"ajax=1",success: function(dataObj){
			if(dataObj.status){
                window.location.href = baseUrl +"/WeChatManage/user_index/";
			}
		}});
	}, function() {
		//取消操作
	});
});
//支付提交(AJAX方式)
if( $("#paySubmit").length > 0 ){
	$(".Validform").Validform({
		btnSubmit:"#paySubmit",tiptype:1,tipSweep:true,postonce:true,ajaxPost:true,
		tiptype:function(msg,o,cssctl){
			//1：正在检测/提交数据，2：通过验证，3：验证失败，4：提示ignore状态
			if( o.type=='3' ){
				$.toptip($(o.obj).attr("errormsg"), 'warning');
			}
		},
		beforeCheck:function(curform){
			//在表单提交执行验证之前执行的函数，curform参数是当前表单对象。
			//这里明确return false的话将不会继续执行验证操作;
		},
		beforeSubmit:function(curform){
			//加载层
			$.showLoading();
			//在验证成功后，表单提交前执行的函数，curform参数是当前表单对象。
			//这里明确return false的话表单将不会提交;	
			//获取表单数据
			var formData = new Object();
			$.each(curform.serializeArray(), function (index, param) {
				if ( !(param.name in formData) ) {
					formData[param.name] = param.value;
				}
			});
			//console.log(formData);
			//微信内调起支付
			if( formData['pay_type']=='wechat' && typeof(formData['wx_active']) != "undefined" ){
				//取得当前支付页面真实url（不带参数）
				var curr_url= window.location.href;
				var arr 	= curr_url.split('?');
				var real_url = arr[0];
				//console.log(formData);
				if( typeof(formData['wx_jsApiPara']) == "undefined" || formData['wx_jsApiPara']=='' ){
					$.toast("无法支付，请稍后重试！", "cancel", function() {
						location.href = real_url;
					});
					return false;
				}
				var jsApiParameters = JSON.parse(formData['wx_jsApiPara']);
				WeixinJSBridge.invoke('getBrandWCPayRequest',jsApiParameters,function(res){
					var msg = res.err_msg;
					if ( msg.indexOf("ok")>=0 ){
						//支付成功
						$.ajax({
							type: "post",
						    url: formData['wx_query_href'],
						    data:'',
						    dataType:'json',
						    success: function (result) {
						    	if(result.status){
									$.toast("支付成功！", function() {
										location.href = formData['wx_query_over'];
									});
								}else{
									$.toast("支付失败！", "cancel", function() {
										location.href = real_url;
									});
								}
						    	return false;
						    }
						});
					}else{
						//返回失败
						if( res.err_desc.length > 0 ){
							$.toast(res.err_desc, "cancel", function(toast) {
								location.href = real_url;
							});
						}
					}
					//WeixinJSBridge.log(msg);
				});
				$.hideLoading();
				$.modal({
					  text: "请在打开的支付页面完成支付",
					  buttons: [
					    { text: "重新支付", className: "default", onClick: function(){ location.href = real_url; } },
					    { text: "支付完成", onClick: function(){ location.href = formData['wx_query_over']; } },
					  ]
				});
				return false;
			}
		},
		//返回数据data是json对象
		callback:function(resObj){
			//支付方式
    		var paytype = $("input[name='pay_type']:checked").val();
        	if( resObj.status ){
        		//头条推广转化（点击支付）
        		if( typeof(trackToutiaoPara) != 'undefined' && typeof(trackToutiaoPara.trackId) != 'undefined' && trackToutiaoPara.trackPay ){
        			trackObj.trackToutiao(trackToutiaoPara.trackId);
        		}
        		//广点通推广转化（点击支付）
        		if( typeof(tracktGdtPara) != 'undefined' && typeof(tracktGdtPara.init) != 'undefined' && tracktGdtPara.trackPay ){
        			trackObj.trackGdt(tracktGdtPara.init);
        		}
        		//快手推广转化（点击提交订单）
        		if( typeof(tracktKsPara) != 'undefined' && typeof(tracktKsPara.trackId) != 'undefined' && tracktKsPara.trackPay ){
        			trackObj.trackKs(tracktKsPara.trackId);
        		}
        		//不做处理，跳转
	        	if( resObj.data.target ){
        			location.href = resObj.data.gotoUrl;
        			return false;
	        	}
            	//站外支付方式
        		if( paytype == 'offline' ){
        			location.href = resObj.data.uploadpic_url;
        		}
        		//支付宝
        		if( paytype == 'alipay' ){
        			//输出提交表单
        			$("body").append(resObj.data.form);
        		}
        		//微信
        		if( paytype == 'wechat' ){
        			//H5支付
        			if( typeof(resObj.data.mweb_url) != "undefined" ){
        				location.href = resObj.data.mweb_url + '&redirect_url=' + encodeURIComponent(resObj.data.wx_query_over);
            			return false;
        			}
					//扫码支付
                    var qrsrc = '<img src="' + baseUrl +'/WeChatManage/user_index/getPageqr/?pageurl='+encodeURIComponent(resObj.data.code_url)+'" style="max-width:100%;" />';
					$.modal({
						text: qrsrc,
						buttons: [
						   {text: "取消支付",className: "default", onClick: function(){ $.hideLoading(); }},
                            { text: "支付完成", onClick: function () { location.href = '/WeChatManage/user_order/paymentFinish/'+resObj.data.payid; }}
						]
					});
					//检测付款状态
					setInterval(function(){
						$.ajax({
							type: "post",
						    url: resObj.data.wx_query_href,
						    data:'',
						    dataType:'json',
						    success: function (result) {
						    	if(result.status){
						    		//支付成功
									location.href = resObj.data.wx_query_over;
								}else{
									console.log("等待付款");
								}
						    }
						});
					},2000);
        		}
        	}else{
        		$.hideLoading();
        		//状态显示
        		$.toptip(resObj.msg, resObj.status?'success':'error');
        	}
        }
	});
}