var isScrollJudge = false; //是否可以滚动
var absoluteAddressPath = '/Plugins/Web/OPUPMS.Restaurant.Web/'; //地址头

if(isScrollJudge){
	var $style = document.createElement('style');
	
	$style.type = 'text/css';
	var str = "";
	str += '.orderScrollBtn{display:none;}';
	str += '.scrollBtn{display:none;}';
	str += '.layerScrollBtn{display:none;}';
	str += '.HeaderScrollBtn{display:block;}';
	str += '.HeaderScrollBtn .layui-btn{display:none !important;}';
	str += '.HeaderScrollBtn .menuBtn{display:inline-block !important;}';
	$style.innerHTML = str;
	document.getElementsByTagName('HEAD').item(0).appendChild($style);
}
