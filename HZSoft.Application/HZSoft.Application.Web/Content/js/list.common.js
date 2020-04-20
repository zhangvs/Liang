
$(document).ready(function () { 							 
	$(".com_doc tr").hover(function(){$(this).toggleClass("t3")},function(){$(this).toggleClass("t3")})
	$(".com_doc tr:even").addClass("com_doc_td");
}); 

//更改每页显示记录数
function changePageNum() {
	var pageNum = $("#sel_pageNum").val();
    var params = { 
   		"baseDTO.pageNum" : pageNum
    };   
    var actionUrl = "admin/base!changePageNum";   
    $.ajax( {   
        url : actionUrl, 
        data : params,   
        dataType : "json",
        cache : false, 
        type : "POST",
        error : function(textStatus, errorThrown) {   
    		alert("系统ajax交互错误!");  
        },   
        success : function(data, textStatus) {
        	 topage('1'); 	            
        }
    });
}

//全选、反选
function switchAll() {
	for (var i = 0; i < document.getElementsByName("id").length; i++) {
		document.getElementsByName("id")[i].checked = !document.getElementsByName("id")[i].checked;
   }
}

//全选、反选
function switchAllByName(name) {
	for (var i = 0; i < document.getElementsByName(name).length; i++) {
		document.getElementsByName(name)[i].checked = !document.getElementsByName(name)[i].checked;
   }
}

function topage(page){
	$("#currPage").val(page);
	$("#queryFrom").submit();
	$("#queryFrom2").submit();
}	

//批量删除
function deleteAll(url){
	//$("#"+butionId).attr("disabled", true);
	var aa = $('[name=id]');
	var ids = '';
 	for(var i=0;i<aa.length;i++){
        if(aa[i].checked){                             
        	if(ids == '') 
        	 	ids = aa[i].value;
        	else
         		ids = ids + "," + aa[i].value;
       	}
  	}
  	if(ids == ''){
   		alert("请选择批量操作项！");
   		//$("#"+butionId).attr("disabled", false);
   		return ;
  	}else{
  		 var params = {
	   		"ids" : ids
    	};   
  	}
	var actionUrl = url;
	
	confirm("确认要提交？", function(){
	    $.ajax( {   
	        url : actionUrl,
	        data : params,   
	        dataType : "json",
	        cache : false, 
	        type : "POST",
	        error : function(textStatus, errorThrown) {   
	    		alert("系统ajax交互错误!");  
	    		//$("#"+butionId).attr("disabled", false);
	        },   
	        success : function(data, textStatus) {   
	        	//alert(data.ajaxResult);		
	        	topage($("#currPage").val());                  
	        }
	    });  
    });	
    //$("#"+butionId).attr("disabled", false);  
}

$(document).ready(function () { 
	document.onkeydown = function(e){ 
	    var ev = document.all ? window.event : e;
	    if(ev.keyCode==13) {
	    	$("#subform").submit();
	    	$("#queryFrom").submit();
	    }
	}
});
