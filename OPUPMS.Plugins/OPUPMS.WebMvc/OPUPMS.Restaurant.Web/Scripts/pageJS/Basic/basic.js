/**
 * 所有 iframe子页面 基本事件
 */

layui.use(['form','laydate','element'], function(){
  var form = layui.form(),
      laydate = layui.laydate,
      element = layui.element();
  
 
});


//iframe自定义风格加载
var skin_back=$.cookie('skin_back');
if (skin_back!=null) {
 if (skin_back!=0) {
   $('body').addClass('skin-diy');
 }
}


//获取url参数
function GetRequest() {  
   var urlz = window.location.href; //获取url中"?"符后的字串 
   url = urlz.substring(urlz.indexOf("?"),urlz.length); 
   var theRequest = new Object();  
   if (url.indexOf("?") != -1) {  
      var str = url.substr(1);  
      strs = str.split("&");  
      for(var i = 0; i < strs.length; i ++) {  
         theRequest[strs[i].split("=")[0]]=unescape(strs[i].split("=")[1]);  
      }  
   }  
   return theRequest;  
}  



//清除点击关闭热点事件
function dropdown_click() {
    $(".dropdown-menu").on("click", "[data-stopPropagation]", function(e) {
        e.stopPropagation();
    });
}

