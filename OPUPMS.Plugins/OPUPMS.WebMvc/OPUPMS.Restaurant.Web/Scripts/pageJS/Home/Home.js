/**
 * 控制台
 */
var pWinW = $(parent.window).width();
if(pWinW <= 1024)$('body').addClass('sm-window');



(function (doc, win) {
    var docEl = doc.documentElement,
        resizeEvt = 'orientationchange' in window ? 'orientationchange' : 'resize',
        recalc = function () {
            var clientWidth = docEl.clientWidth;
            if (!clientWidth) return;
            SetMealTableWidth()
        };

    if (!doc.addEventListener) return;
    win.addEventListener(resizeEvt, recalc, false);
    doc.addEventListener('DOMContentLoaded', recalc, false);
})(document, window);


$(function () {
    /* 餐饮实时状态更新 --start */
    chat = $.connection.systemHub;
    chat.hubName = 'systemHub';
    chat.connection.start();

    chat.client.callResServiceRefersh = function (result) {
        if (result == true) {
            LoadInit(laytpl);
        }
    }
    
    window.parent.chat = chat;
    /* 餐饮实时状态更新 --end */
});

var laytpl;
layui.use(['element','laytpl','layer','form'], function(){
    var element = layui.element,
        laytpl_para = layui.laytpl,
        layer=layui.layer;
    form = layui.form
    SysTime();
    laytpl = laytpl_para;
    LoadInit(laytpl);
});
var inidata='';

function LoadInit(laytpl) {
    //获取当前餐厅数据
    $.ajax({
        url: "/Res/Home/LoadPlatformInfo",
        data: {},
        type: "post",
        dataType: "json",
        beforeSend: function (xhr) {
            //parent.layindex = parent.layer.open({type: 3});
        },
        complete: function (XMLHttpRequest, textStatus) {
            //parent.layer.close(parent.layindex);
        },
        success: function (data) {
          	inidata=data;
          	//桌台风格渲染
			tableStyleInit()
			
			//桌台排序
			data.TableList.sort(function (x, y) {
			    return x.Sorted - y.Sorted || x.Id -y.Id;
			});
          	
            //渲染区域
            var TablegetTpl = AreaList_tpml.innerHTML
                ,view = document.getElementById('AreaList_view');
            laytpl(TablegetTpl).render(data.AreaList, function(html){
              view.innerHTML = html;
            });

            //渲染桌台
            var getTpl = TableList_tpml.innerHTML
                , view = document.getElementById('TableList_view');
            laytpl(getTpl).render(data.TableList, function (html) {
                view.innerHTML = html;
            });

            //渲染状态
            var getTpl = TableStatusList_tpml.innerHTML
                , view = document.getElementById('TableStatusList_view');
            laytpl(getTpl).render(data.TableStatusList, function (html) {
                view.innerHTML = html;
            });

            //渲染右侧数据
            var getTpl = MealTableData_tpml.innerHTML
                , view = document.getElementById('MealTableData_view');
            laytpl(getTpl).render(data, function (html) {
                view.innerHTML = html;
            });
			
            //渲染营业日期
            var getTpl = Date_tpml.innerHTML
                , view = document.getElementById('Date_view');
            laytpl(getTpl).render(data.BusinessDate, function (html) {
                view.innerHTML = html;
            });
            SetMealTableWidth();
            
            //餐台筛选 区域+状态
			MealTable_Screen(data.TableList,laytpl);
			//餐台点击
			TableClick(data.TableList);
			
			//桌台  高度设置
			var TableListHeight;//控制台餐桌高度
			pWinW > 1024 ? TableListHeight = Math.floor(($(window).height() - $('header.ClassTab-head').outerHeight()) / 112 ) * 112 : TableListHeight = Math.floor(($(window).height() - $('header.ClassTab-head').outerHeight()) / 68) * 68;
			$('#TableList_view').height(TableListHeight)
        }, error: function (msg) {
            console.log(msg.responseText);
        }
    });
}



function SetMealTableWidth() {
	var minW = pWinW > 1024 ? 130 : 100;

    var box=$('.MealTable-lists');
    var width = box.width()-20;
    var state_width = Math.floor(width / minW);
    var state_b = width/state_width-12;
    box.find('li').css('width', '' + state_b + 'px');
}

//时间
function SysTime() {
	setInterval(function() {
        var d = new Date();
        var month = d.getMonth()+1;
        var day = d.getDate();
        var minutes=d.getMinutes();
        var seconds=d.getSeconds();
        var time = d.getHours() + ":" + (minutes<10 ? '0' : '') + minutes + ":" + (seconds<10 ? '0' : '') + seconds;
        var date = d.getFullYear() + '-' +    (month<10 ? '0' : '') + month + '-' +    (day<10 ? '0' : '') + day;
        $('#current-date').text(date);
        $('#current-time').text(time);
	}, 1000);
}


//桌台筛选
function MealTable_Screen(data,laytpl) {
	$('#TableStatusList_view').delegate('li', 'click', function(event) {
	    $(this).addClass('layui-this').siblings('li').removeClass('layui-this');
	 	var key=$(this).attr('data-key');
	 	var area=$('#AreaList_view .layui-this').attr('data-area');
	 	Show_data(data,key,area,laytpl);
	})
	$('#AreaList_view').delegate('a', 'click', function(event) {
	 	$(this).addClass('layui-this').siblings('a').removeClass('layui-this');
	 	var area=$(this).attr('data-area');
	 	var key=$('#TableStatusList_view .layui-this').attr('data-key');
	 	Show_data(data,key,area,laytpl);
	})
}

//筛选结果渲染数据
function Show_data(data,key,area,laytpl) {

	var listdata=[];
	
    if (area == 0 && key == 0) {// 区域全部、状态全部
        listdata = data;
    } else if (area != 0 || key != 0) {// 区域全部

        for (var i = 0; i < data.length; i++) {
            var table_area = data[i]['AreaId'];
            var table_status = data[i]['CythStatus'];
            if (area == 0) {
                if (table_status == key) {
                    listdata.push(data[i]);
                }
            } else if (key == 0) {
                if (table_area == area) {
                    listdata.push(data[i]);
                }
            } else {
                if (table_area == area && table_status == key) {
                    listdata.push(data[i]);
                }
            }
        }
    }
	
	//渲染桌台1
    var getTpl = TableList_tpml.innerHTML
        ,view = document.getElementById('TableList_view');
    laytpl(getTpl).render(listdata, function(html){
      	view.innerHTML = html;
      	
    });
    SetMealTableWidth();
    
    TableClick(listdata);
}


function TableClick(data) {
    var tableBox = $('#TableList_view li a');
    tableBox.unbind("click");
    tableBox.click(function (event) {
        var no = parseInt($(this).attr('data-no'));
        var tableData = data[no];
        if (tableData.CythStatus == 1) {//空台
        	var ydstate=$(this).find('.MealTable-state').find('p');
        	if (ydstate.length>0) {//存在当前分市的预定
        		layer.confirm('该台存在当前分市的预定，您确定要开台吗？', {
				  btn: ['确认开台','取消'] //按钮
				}, function(){
					layindex = layer.open({type: 3,shadeClose: false});
					location.href = "/Res/Home/NewOpenTable/" + tableData.Id;
				}, function(){
				   layer.close(layer.index);
				});
        		
        	}else{
        		layindex = layer.open({type: 3,shadeClose: false});
        		location.href = "/Res/Home/NewOpenTable/" + tableData.Id;
        	}
            
        } else if (tableData.CythStatus == 2) {//在用
            if (tableData.OrderNow.length > 1) {//多单
     
                var aHtml = "";
                for (var i = 0; i < tableData.OrderNow.length; i++) {
                	var orderTableId = tableData.OrderNow[i].Id;
                	var $class= tableData.OrderNow[i].IsLock ? 'IsLock' : '';
                    aHtml += '<li data-id="'+ orderTableId +'" class="'+$class+'"><a href="javascript:;">'
                        + '<h4>单号：' + tableData.OrderNow[i].OrderNo;
                    if($class !='')aHtml += "<span class='LockTextColor'>( 锁定 )</span>"
                    aHtml += '<span class="time">时间：' + tableData.OrderNow[i].CreateDate + '</span></h4><p>人数：' + tableData.OrderNow[i].PersonNum;
                        
                    if(tableData.OrderNow[i].ContactPerson)aHtml += '<strong>联系人：' + tableData.OrderNow[i].ContactPerson  + '</strong>'
                    
                    if(tableData.OrderNow[i].ContactTel)aHtml += '<strong>电话：' + tableData.OrderNow[i].ContactTel  + '</strong>'
                    
                	aHtml += '<span>选中订单</span></p></a></li>';
                }
                layer.open({
                    type: 1,
                    title: "选择操作订单",
                    area: ["600px", "500px"],
                    content: '<ul class="chosen-order">' + aHtml + '</ul>',
                    maxmin: false,
                    success: function(layero, index){
                    	layero.find('li').on('click',function(){
                    		var id = $(this).attr('data-id');
                    		var $index = $(this).index();
                    		if(tableData.OrderNow[$index].IsControl){
                    			parent.layer.confirm('该桌台（ '+tableData.Name+' ）中的第'+($index + 1)+'个订单正在被操作,是否确认要进入该桌台点餐?', {icon: 3, title:'提示'}, function(index){
                    				parent.layer.close(index);
                    				parent.OpenOrder(id);
                    			});
                    		}else{
                    			parent.OpenOrder(id);
                    		}
                    	})
                    }
                });

            } else {
            	//如果当前桌台有人操作
            	if(tableData.OrderNow[0].IsControl){
            		parent.layer.confirm('该桌台（ '+tableData.Name+' ）正在被操作,是否确认要进入该桌台点餐?', {icon: 3, title:'提示'}, function(index){
				 		parent.layer.close(index);
				 		var orderTableId = tableData.OrderNow[0].Id;
		                var ordTableArr = [];
		                ordTableArr.push(orderTableId);
		                parent.OpenOrder(ordTableArr);
					});
            	}else{
            		var orderTableId = tableData.OrderNow[0].Id;
	                var ordTableArr = [];
	                ordTableArr.push(orderTableId);
	                parent.OpenOrder(ordTableArr);
            	}
            }

        } else if (tableData.CythStatus == 3) {//清洁
            
            layer.confirm("确认空置吗？", { title: '更新餐台状态提示', btn: ['确认', '取消'] }, function () {
                var tabArray = [];
                tabArray.push(tableData.Id);
                SetTableEmpty(tabArray);
            });
        }
    });
}


//弹出空置操作
function SetTableEmpty(tableIds) {
    var chat = $.connection.systemHub;
    chat.hubName = 'systemHub';
    chat.connection.start();

    var para = { tableIds: tableIds};
    $.ajax({
        type: "post",
        url: "/Res/Home/SetTableEmpty",
        dataType: "json",
        data: JSON.stringify(para),
        contentType: "application/json; charset=utf-8",
        async: false,
        beforeSend: function (xhr) {
            layindex = layer.open({type: 3});
        },
        complete: function (XMLHttpRequest, textStatus) {
            layer.close(layindex);
        },
        success: function (data, textStatus) {
            if (data.Successed == true) {
                $.connection.hub.start().done(function () {
                    chat.server.notifyResServiceRefersh(true);
                });
                parent.layer.msg("操作成功");
                layer.closeAll();
            } else {
                layer.alert(data.Message);
            }
        }
    });
}



//弹出批量置空
function BatchEmpty(){
	var str='';
	for (var i = 0; i < inidata.TableList.length; i++) {
		var item=inidata.TableList[i];
		if (item.CythStatus==3) {//空台
			str+='<li data-id="'+item.Id+'">'
			    +   '<div class="MealTable-head flex">'
			    +        '<span class="item MealTable-number flex-item">'+item.Id+'</span>'
			    +   '</div>'
			    +   '<div class="MealTable-title" style="line-height: 15px;font-size:16px;">'+item.Name+' </div>'
			    +'</li>';
		}
	}
	if(!str){
	    layer.msg('当前没有可置空的桌台!');
	    return false;
	}
	var strHtml='<div class="MealTable-lists box-sm" style="margin-right:0;"><ul id="BatchEmpty">'+str+'</ul></div>';
	

	layer.open({
	    type: 1,
	    anim: -1,
	    title: '请选择需要置空的桌台',
	    shadeClose: true,
	    skin: 'layer-header', 
	    shade: 0.8,
	    area: ['740px', '500px'],
	    content: strHtml
	    ,btn: ['确认置空', '取消']
        , yes: function (index, layero) {
            //取得所选台id
            var tableIds = [];
            var listsli = $('#BatchEmpty li.checked');
            for (var i = 0; i < listsli.length; i++) {
                var id = listsli.eq(i).attr('data-id');
                tableIds.push(id);
            }
            if (tableIds < 1) {
                layer.msg('请选择置空的桌台!');
                return false;
            }
            SetTableEmpty(tableIds);
        }
	    ,btn2: function(index, layero){
	    }
	    ,success:function(layero){
	    	var $checkbox = $('<div class="layui-form" lay-filter="empty_MealTable" style="position: absolute;bottom: 12px;left: 10px;"><input type="checkbox" name="isAllSelected" title="全选" class="isAllSelected" lay-filter="isAllSelected"></div>')
	    	layero.children('.layui-layer-btn').append($checkbox);
	    	var isAllSelected = layero.find('.isAllSelected'); //全选标签
	    	var $li = $('#BatchEmpty li');
	    	
	    	form.render('checkbox', 'empty_MealTable');
	    	
	    	form.on('checkbox(isAllSelected)', function (data) {
                if (data.elem.checked == true) {//全选
                    $li.addClass('checked')
                } else {
                    $li.removeClass('checked')
                }

            });
	    	
	    	var len = $li.length;
            $li.click(function () {
                if ($(this).hasClass('checked')) {
                    $(this).removeClass('checked');
                } else {
                    $(this).addClass('checked');
                }
                if($li.filter('.checked').length === len){
                	isAllSelected.get(0).checked = true;
                }else{
                	isAllSelected.attr("checked",false);
                }
                form.render('checkbox', 'empty_MealTable');
            });
            	    
	    }
    });
}

//餐桌风格设置
function tableStyleInit(){
	if(window.parent.inidata && window.parent.inidata.UserName){
		var userOptions = layui.data(window.parent.inidata.UserName);
		if(!isEmptyObject(userOptions)){
			tableStyle(userOptions)
		}
	}
}

//餐桌风格拼接
function tableStyle(options){
	$('#myTableStyle').remove();
	var style = document.createElement('style');
	style.id = 'myTableStyle'
	style.type = 'text/css';
	var str = "";
	for(var i in options){
		switch(i){
			case 'addBG':
				str += ".MealTable-Screen .ScreenMealTable-icon.add, .MealTable-lists li.add{background:"+ options[i] +";}"
				break;
			case 'addColor':
				str += "#TableList_view li.add a{color:"+ options[i] +";}"
				break;
			case 'addState':
				str += "#TableList_view li.add .MealTable-footer .color-green{color:"+ options[i] +";}"
				break;
			case 'lockBG':
				str += ".MealTable-Screen .ScreenMealTable-icon.lock, .MealTable-lists li.lock{background:"+ options[i] +";}"
				break;
			case 'lockColor':
				str += "#TableList_view li.lock a{color:"+ options[i] +";}"
				break;
			case 'lockState':
				str += "#TableList_view li.lock .MealTable-footer .color-green{color:"+ options[i] +";}"
				break;
			case 'dirtyBG':
				str += ".MealTable-Screen .ScreenMealTable-icon.dirty, .MealTable-lists li.dirty{background:"+ options[i] +";}"
				break;
			case 'dirtyColor':
				str += "#TableList_view li.dirty a{color:"+ options[i] +";}"
				break;
			case 'dirtyState':
				str += "#TableList_view li.dirty .MealTable-footer .color-green{color:"+ options[i] +";}"
				break;
			case 'emptyBG':
				str += ".MealTable-Screen .ScreenMealTable-icon.empty, .MealTable-lists li.empty{background:"+ options[i] +";}"
				break;
			case 'emptyColor':
				str += "#TableList_view li.empty a{color:"+ options[i] +";}"
				str += "#TableList_view li.empty a:hover{color:"+ options[i] +";}"
				break;
			case 'emptyState':
				str += "#TableList_view li.empty .MealTable-footer .color-green{color:"+ options[i] +";}"
				break;
			case 'wxBG':
				str += ".MealTable-Screen .ScreenMealTable-icon.wx, .MealTable-lists li.wx{background:"+ options[i] +";}"
				break;
			case 'wxColor':
				str += "#TableList_view li.wx a{color:"+ options[i] +";}"
				str += "#TableList_view li.wx a:hover{color:"+ options[i] +";}"
				break;
			case 'wxState':
				str += "#TableList_view li.wx .MealTable-footer .color-green{color:"+ options[i] +";}"
				break;
		}
	}
	style.innerHTML = str;
	document.getElementsByTagName('HEAD').item(0).appendChild(style);
}

//菜品向下滚动 一页
$(document).on('mousedown touchstart','.scrollBtn .scrollBottomBtn',function(){
	var list = $('#TableList_view');
	var listH = list.height();
	var h = list.scrollTop();
	
	list.scrollTop(h + listH)
})

//菜品向上滚动 一页
$(document).on('mousedown touchstart','.scrollBtn .scrollTopBtn',function(){
	var list = $('#TableList_view');
	var listH = list.height();
	var h = list.scrollTop();
	if(h < listH){
		list.scrollTop(0)
	}else{
		list.scrollTop(h - listH)
	}
	
})

//控制台  右侧详情栏  伸缩
$('.scrollBtn .menuBtn').on('click',function(){
	var $side = $('.right-side'); 
	$side.hasClass('expand') ? $side.removeClass('expand') : $side.addClass('expand');
})


//判断对象是否为空
function isEmptyObject(obj) {   
　　for (var key in obj){
　　　　return false;//返回false，不为空对象
　　}　　
　　return true;//返回true，为空对象
}

//日期 +1
function addDate(){
	var $date = inidata.BusinessDate;
	var d=new Date($date);
   	d.setDate(d.getDate()+1); 
   	var m=d.getMonth()+1;
   	if(m.toString().length == 1)m = '0' + m;
   	inidata.BusinessDate = d.getFullYear()+'-'+m+'-'+ (d.getDate().toString().length != 1 ? d.getDate() : '0' + d.getDate());
   	//渲染营业日期
    var getTpl = Date_tpml.innerHTML
        , view = document.getElementById('Date_view');
    laytpl(getTpl).render(inidata.BusinessDate, function (html) {
        view.innerHTML = html;
    });
}