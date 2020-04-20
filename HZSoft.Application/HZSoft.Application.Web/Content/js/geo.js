//市
function queryGeo2(){
	var province_id = $("#province_id").val();
	var params = {   
		"pid" : province_id
	};
	var actionUrl = "system/common!ajaxGeo";    
	$.ajax({   
		url : actionUrl,   
		data : params,   
		dataType : "json",
		cache : false,  
		type : "POST", 
		error : function(textStatus, errorThrown) {   
			alert("系统ajax交互错误!");	   
		}, 
		success : function(data, textStatus) {
			$("#city_id").empty();
			$("#county_id").empty();
        	if (data.length>0){
        		for(var i=0;i<data.length;i++) {
        			$('#city_id').append("<option id='' value='"+data[i].key+"'>"+data[i].value+"</option>");
        		}
        	}
        	//$('#county_id').append("<option id='' value='-1'>请选择</option>");
		}
	});
}

function queryGeo3(){
	var city_id = $("#city_id").val();
	var params = {   
		"pid" : city_id
	};
	var actionUrl = "system/common!ajaxGeo";    
	$.ajax({   
		url : actionUrl,   
		data : params,   
		dataType : "json",
		cache : false,  
		type : "POST", 
		error : function(textStatus, errorThrown) {   
			alert("系统ajax交互错误!");	   
		}, 
		success : function(data, textStatus) {
			$("#county_id").empty();
        	if (data.length>0){
        		for(var i=0;i<data.length;i++) {
        			$('#county_id').append("<option id='' value='"+data[i].key+"'>"+data[i].value+"</option>");
        		}
        	}
		}
	});
}