var winW = $(window).width();
var winH = $(window).height();

RegKeyboard();
var allkeywordindex='';
//监听触发软键盘事件
function RegKeyboard() {

    //输入键盘唤起
    $('.layui-form').delegate('a.input-keyboard', 'click', function (event) {
        var input = $(this).prev('.layui-input');
        var type = input.attr('data-type');
        var name = input.attr('name');
        input.focus();
        var mymode = layui.data('set');
        if (mymode.mymode != 'touch') {//触摸
            Keyboard(name);
        }
    });

}

//监听输入框获得焦点是否触发软键盘
function ShowKeyboard(inputname) {
    var mymode = layui.data('set');
    if (mymode.mymode == 'touch') {//触摸
        Keyboard(inputname);
    }
}

//创建键盘
function Keyboard(inputname) {

    $('.keyboard-number li').unbind("click");
    var inputdom = $('[name="' + inputname + '"]');
    var inputtitle = inputdom.attr('title');
    var inputid = inputdom.attr('id');
    var type = inputdom.attr('data-type');
    $('.layui-input').removeClass('keythis');
    inputdom.addClass('keythis');
    var inputY = inputdom.offset().top + 40;
    var inputX = inputdom.offset().left;
    
    var keywordlayerindex='';

    if (type == 'number') {//数字键盘
        var keyboard = $('.keyboard-number');

        var str = '<div class="keyboard-number keyboard number-input">'
            + '<div class="Keyboard-input-group">'
            + '<input type="text"  class="layui-input" disabled value=""/>'
            + '</div>'
            + '<ul class="row">'
            + '<li data-val="1">1</li>'
            + '<li data-val="2">2</li>'
            + '<li data-val="3">3</li>'
            + '<li data-val="4">4</li>'
            + '<li data-val="5">5</li>'
            + '<li data-val="6">6</li>'
            + '<li data-val="7">7</li>'
            + '<li data-val="8">8</li>'
            + '<li data-val="9">9</li>'
            + '<li data-val="0" style="width: 62%;">0</li>'
            + '<li data-val=".">.</li>'
            + '</ul>'
            + '<div class="Keyboard-btn">'
            + '<a href="javascript:void(0);" data-val="del" ><i class="layui-icon" >&#xe65c;</i></a>'
            + '<a href="javascript:void(0);" data-val="success">确定</a>'
            + '</div>'
            + '</div>';
        layer.open({
            type: 1,
            title: false,
            closeBtn: 0,
            shadeClose: true,
            shade: 0,
            anim: 2,
            isOutAnim: false,
            //offset: [inputY,inputX],
            offset: 'rb',
            skin: 'keyboard-box',
            area: ['380px', '380px'],
            content: str,
            success: function(layero, index){
            	keywordlayerindex=index;
			}
        });

        $('.keyboard-number li').mousedown(function (event) {
            var value = $(this).attr('data-val');
            var inputval = inputdom.val();
            if (value == '.') {
                if (inputval.indexOf(".") >= 0) {
                    return false;
                }
            }
            if (inputval.indexOf(".") >= 0) {
	            if (inputval.toString().split(".")[1].length>=2) {
	            	return false;
	            }
            }
            var newval = inputval + value;
            inputdom.val(newval);
            //return false;
        });

        $('.Keyboard-btn a').on("click", function () {
            var value = $(this).attr('data-val');
            var inputval = inputdom.val();
            if (value == 'del') {//删除
                var newval = inputval.substring(0, inputval.length - 1);
                inputdom.val(newval);
            } else if (value == 'success') {//确定
                //跳转下一个input获得焦点
                var numberinput = $('.layui-input[data-type="number"]');
                var nxtidx = numberinput.index(inputdom);
                if (nxtidx + 1 >= numberinput.length) {
                    layer.close(layer.index);
                } else {
                    numberinput[nxtidx + 1].focus();
                }


            }
        })

        $('.keyboard-box').mousedown(function (event) {
            return false;
        })


    } else {//九宫格键盘
    	var str = '<div id="keyboard_layer" style="padding-top: 10px;"></div>';
        layer.open({
            type: 1,
            title: false,
            closeBtn: 0,
            shadeClose: true,
            shade: 0,
            anim: 2,
            isOutAnim: false,
            offset: 'rb',
            //skin: 'keyboard-box',
            area: ['410px', '380px'],
            skin: 'keyboard_layer',
            content: str,
            success: function(layero, index){
            	if ($('#keyboard_layer').length == 1) {
			        var inputname=inputdom.attr('name');
    				ShowKeyboardbox(inputname);
    			}
            	keywordlayerindex=index;
			}
        });

    }
    //禁止input失去焦点
    $('.keyboard_layer').mousedown(function (event) {
            return false;
    })

    inputdom.blur(function () {
        if ($('#softkey').length > 0) {
            VirtualKeyboard.toggle(inputid, 'softkey');
        }
        inputdom.removeClass('keythis');
        layer.close(keywordlayerindex);
        layer.close(allkeywordindex);
    })
}


function Allkeyboard(inputdom){
	    var inputid = inputdom.attr('id');
	  	var lang = inputdom.attr('data-lang');
        var str = '<div id="softkey"></div>';
        layer.open({
            type: 1,
            title: false,
            closeBtn: 0,
            shadeClose: true,
            shade: 0,
            anim: 2,
            isOutAnim: false,
            offset: 'rb',
            skin: 'keyboard-box',
            area: ['876px', '310px'],
            content: str,
            success:function(layero, index){
            	allkeywordindex=index;
            }
        });
        if(lang=='en'){
        	var vk="US US";
        }else{
        	var vk="CN Chinese Simpl. Pinyin";
        }
        DocumentCookie.set('vk_layout', vk);
        if ($('#softkey').length == 1) {
            VirtualKeyboard.toggle(inputid, 'softkey',vk);
        } else {
            layer.close(layer.index);
        }
}

//获取URL参数
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}

//layer弹窗
function layerBox(url, id, titlename,Height,Width) {
	var width = Width || 800 ;
	var winHeight = $(document).height();
    var height;
    !Height ? height = winHeight - 100 : winHeight > Height ? height = Height : height = winHeight - 10;
    layer.open({
        type: 2,
        title: titlename,
        area: [width + "px", height + "px"],
        content: url + "?id=" + id,
        maxmin: false
    });
}


//取小数后两位
function changeTwoDecimal(x) {
    var f_x = parseFloat(x);
    if (isNaN(f_x)) {
        alert('function:changeTwoDecimal->parameter error');
        return false;
    }
    var f_x = Math.round(x * 100) / 100;
    var s_x = f_x.toString();
    var pos_decimal = s_x.indexOf('.');
    if (pos_decimal < 0) {
        pos_decimal = s_x.length;
        s_x += '.';
    }
    while (s_x.length <= pos_decimal + 2) {
        s_x += '0';
    }
    return s_x;
}


//获取当前时间
function getTime() {
    var myDate = new Date();
    //获取当前年
    var year = myDate.getFullYear();
    //获取当前月
    var month = myDate.getMonth() + 1;
    //获取当前日
    var date = myDate.getDate();
    var h = myDate.getHours();       //获取当前小时数(0-23)
    var m = myDate.getMinutes();     //获取当前分钟数(0-59)
    var s = myDate.getSeconds();
    var now = year + '-' + p(month) + "-" + p(date) + " " + p(h) + ':' + p(m) + ":" + p(s);
    return now;
}
function p(s) {
    return s < 10 ? '0' + s : s;
}

//自适应  T
function T_list_auto(operation,mealTable){
	//点菜操作菜单
	if(operation){
		var minHeight = 50;  //最小高度
		var winHeight = $(window).height();
		var $operation_li = $('#operation_lists').children('li');
		var len = $operation_li.length;
		
		var $operation_li_h = ( winHeight - len * 2 - 2) / len;
		
		$operation_li_h < minHeight ? $operation_li_h = minHeight + 'px' : $operation_li_h > 100 ? $operation_li_h = '100px' : $operation_li_h += 'px';
		
		$operation_li.children('a').css({'height':$operation_li_h,'line-height':$operation_li_h});
	}
	
	//菜品列表
	if(mealTable){
		var $mealTable = $('#ProjectAndDetails_view');
		var $mealTable_w = $mealTable.width();
		var maxW = 120;
		var outW = 12;
		if(winW <= 1024){
			maxW = 100;
			outW = 2;
		}
		line_sum = Math.floor($mealTable_w / maxW);
		$mealTable_li_w = $mealTable_w / line_sum - outW;//120为最小宽度(包含padding margin border)
		$mealTable.children('li').css('width',$mealTable_li_w);
	}
	
}

//获取当前时间
function gettime(){
	var d = new Date();
    var month = d.getMonth()+1;
    var day = d.getDate();
    var minutes=d.getMinutes();
    var seconds=d.getSeconds();
    var time = d.getHours() + ":" + (minutes<10 ? '0' : '') + minutes + ":" + (seconds<10 ? '0' : '') + seconds;
    var date = d.getFullYear() + '-' +    (month<10 ? '0' : '') + month + '-' +    (day<10 ? '0' : '') + day;
    var datatime=date+' '+time;
    return datatime;
}




layui.use(['element', 'table', 'layer', 'laydate', 'form', 'laytpl'], function () {
	var element = layui.element,
	laydate = layui.laydate,
	layer = layui.layer,
	table = layui.table,
	form = layui.form,
	laytpl = layui.laytpl;
	
    $(document).on('keydown','.layui-form-select .layui-input',function(e){
		var selectDiv = $(this).closest('.layui-form-select');
		var anim = selectDiv.children('.layui-anim');
		if(e.keyCode === 13){ //确认
			anim.children('.layui-this').click();
		}else{
			var index;
			var dd = anim.children('dd:not(.layui-hide)').not(':hidden');
			var index = -1;
			$.each(dd,function(i){
				if($(this).hasClass('layui-this')){
					index = i;
				}
			})
			
			//如果没有数据  或者  点击的是删除
			if(e.keyCode === 38 || e.keyCode === 40){
				e.preventDefault();
				if(e.keyCode === 38){ //上
					if(index === 0 )return false
					index > 0 ? index-- : index = 0;
				}else if(e.keyCode === 40){ //下
					if(index >= dd.length -1)return;
					index++;
				}
				var indexDd = dd.eq(index);
				//滚动定位
				var animH = anim.outerHeight();
				var DdH = indexDd.outerHeight()
				var offsetT = indexDd[0].offsetTop;
				var scrollT = anim.scrollTop();
				var lowerBound = scrollT + animH - DdH;
				if(offsetT < scrollT){ //向上移动
					anim.scrollTop(offsetT)
				}else if (offsetT > lowerBound) { //向下移动
					anim.scrollTop(offsetT - animH + DdH);
				}
				indexDd.addClass('layui-this').siblings().removeClass('layui-this');
				$(this).val(indexDd.attr('lay-value') !== "" ? indexDd.html() : '');
			}
		}
	})
	
	
	
});

//提示信息 公共方法
function hintTips($dom,valCallback,type){
	$dom.on('mouseenter',function(e){
		var val = typeof valCallback === "function" ? valCallback($(this)) : valCallback;
		layer.tips(val, this,{time:9999999,tips:type || 1,anim: 5,isOutAnim: false,success: function(layero, index){
			layero.css('left',parseInt(layero.css('left').replace('px','')) - 10)
		}}); 
	}).on('mouseleave',function(){
		layer.closeAll('tips');
	})
}


//判断对象是否为空
function isEmptyObject(obj) {   
　　for (var key in obj){
　　　　return false;//返回false，不为空对象
　　}　　
　　return true;//返回true，为空对象
}




