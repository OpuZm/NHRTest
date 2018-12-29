$(function () {
    /* 餐饮实时状态更新 --start */
    var chat = $.connection.systemHub;
    chat.hubName = 'systemHub';
    chat.connection.start();

    chat.client.callResServiceRefersh = function (result) {
        if (result == true) {
            LoadInit(laytpl);
        }
    }
    /* 餐饮实时状态更新 --end */
});

//fastclick
FastClick.attach(document.body);

var laytpl;
layui.use(['element', 'laytpl', 'layer', 'form'], function () {
    var element = layui.element,
        laytpl_para = layui.laytpl,
        layer = layui.layer;
    form = layui.form
//  SysTime();
    laytpl = laytpl_para;
    LoadInit(laytpl);
});
var inidata = '';
var isLoading = [];

function LoadInit(laytpl) {
    //获取当前餐厅数据
    $.ajax({
        url: "/Res/Home/LoadPlatformInfo",
        data: {},
        type: "post",
        dataType: "json",
        success: function (data) {
            inidata = data;
            //渲染区域
            var TablegetTpl = AreaList_tpml.innerHTML
                , view = document.getElementById('AreaList_view');
            laytpl(TablegetTpl).render(data.AreaList, function (html) {
                view.innerHTML = html;
            });
            
            //桌台排序
			data.TableList.sort(function (x, y) {
			    if (x.Sorted < y.Sorted) {
			        return -1;
			    } else if (x.Sorted > y.Sorted) {
			        return 1;
			    } else {
			        return 0;
			    }
			});

            //渲染桌台
//          var listdata = [];
//          for (var i = 0; i < data.TableList.length; i++) {
//	            var table_area = data.TableList[i]['AreaId'];
//	            var table_status = data.TableList[i]['CythStatus'];
//	            if (table_area == data.AreaList[0].Id) {
//                  listdata.push(data.TableList[i]);
//              }
//	        }
//          var getTpl = TableList_tpml.innerHTML
//              , view = document.getElementById('TableList_view');
//          laytpl(getTpl).render(data.TableList, function (html) {
//              view.innerHTML = html;
//          });

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
//          SetMealTableWidth();

            //餐台筛选 区域+状态
            MealTable_Screen(data.TableList, laytpl);
            //餐台点击
//          TableClick(data.TableList);
			
			isLoading.push(1);
			//初始化动作全部完成时
			if(isLoading.length == 2)$('#loading').remove();

        }, error: function (msg) {
            console.log(msg.responseText);
        }
    });
    
    //餐厅切换信息   用户信息
    //获取当前餐厅数据
    $.ajax({
        url: "/Res/Home/GetRestauantInfo",
        data: {},
        type: "post",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: true,
        success: function (data, textStatus) {
            //渲染当前餐厅名称
//          var getTpl = RestaurantName_tpml.innerHTML
//              , view = document.getElementById('RestaurantName_view');
//          laytpl(getTpl).render(data, function (html) {
//              view.innerHTML = html;
//          });
//
//          //渲染当前用户名
//          var getTpl = UserName_tpml.innerHTML
//              , view = document.getElementById('UserName_view');
//          laytpl(getTpl).render(data, function (html) {
//              view.innerHTML = html;
//          });
//
            
            //用户名
            $('#userName').html(data.UserName)
            
            
            //当前餐厅名称
            $('#SetRestaurant').html("<span>" + data.RestaurantName + " </span><i class='layui-icon'>&#xe625;</i>" );
            $('#RestaurantName_view .media-intro').html("("+data.MarketName+")")
            
            //可选餐厅列表
            SetRestaurant(data);
            
            isLoading.push(1);
			//初始化动作全部完成时
			if(isLoading.length == 2)$('#loading').remove();

        },
        complete: function (XMLHttpRequest, textStatus) {
            //layer.close(layindex);
        }
    });
}

//切换餐厅
function SetRestaurant(data) {
	var list = data.RestaurantList
    $(document).on('click','#RestaurantName_view',function (event) {
        var shoplist = '';
        if (list.length > 0) {
            for (var i = 0; i < list.length; i++) {
                var MarketList = '<option value="" >请选择</option>';
                for (var j = 0; j < list[i].MarketList.length; j++) {
                    var selected = list[i].MarketList[j].IsDefault == true ? 'selected' : '';
                    MarketList += '<option value="' + list[i].MarketList[j].Id + '" ' + selected + '>' + list[i].MarketList[j].Name + '</option>';
                }
                shoplist += '<li>'
                    + '<h4>' + list[i].Name + '</h4>'
                    + '<div>'
                    + '<div class="layui-input-inline" style="width: 60%;margin-top:6px;">'
                    + '<select name="Market" lay-verify="Market">' + MarketList + '</select>'
                    + '</div>'
                    + '<a href="javascript:void(0);" data-id="' + list[i].Id + '" class="ok" style="width:34%;text-align:center;font-size:14px;">进入餐厅</a>'
                    + '</div>'
                    + '</li>';
            }


            layer.open({
                type: 1,
                title: "选择操作的餐厅",
                area: ["600px", "500px"],
                content: '<ul class="choice-shop choose-shop layui-form">' + shoplist + '</ul>',
                maxmin: false
            });

            form.render('select');

            //进入餐厅点击
            $('.choice-shop a.ok').click(function () {
                var Marketid = $(this).prev('.layui-input-inline').find('select[name="Market"]').val();
                var shopid = $(this).attr('data-id');
                if (!Marketid) {
                    layer.msg('请选择餐厅分市!');
                } else {
                    entrySys(shopid, Marketid);
                }
            })

        }
    })
}

//请求登录 指定餐厅
function entrySys(id, marketId) {
    $.ajax({
        type: "post",
        url: "/Res/Account/SelectRestaurant",
        data: JSON.stringify({ id: id, marketId: marketId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        beforeSend: function (xhr) {
            layindex = layer.open({type: 3});
        },
        complete: function (XMLHttpRequest, textStatus) {
            layer.close(layindex);
        },
        success: function (data, textStatus) {
            if (data.Data == true) {
                $("#btnLogin").find('span').html("登录成功，正在跳转...");
                
                
                window.setTimeout(function () {
                	layindex = layer.open({type: 3});
                    window.location.href = "/Res/MHome/Index";
                }, 200);
            } else {
                layer.alert(data.Message);
            }
        }
    });
}

//
//
//function SetMealTableWidth() {
//  var box = $('.MealTable-lists');
//  var width = box.width() - 20;
//  var state_width = Math.floor(width / 150);
//  var state_b = width / state_width - 8;
//  box.find('li').css('width', '' + state_b + 'px');
//}

//时间
//function SysTime() {
//  setInterval(function () {
//      var d = new Date();
//      var month = d.getMonth() + 1;
//      var day = d.getDate();
//      var minutes = d.getMinutes();
//      var seconds = d.getSeconds();
//      var time = d.getHours() + ":" + (minutes < 10 ? '0' : '') + minutes + ":" + (seconds < 10 ? '0' : '') + seconds;
//      var date = d.getFullYear() + '-' + (month < 10 ? '0' : '') + month + '-' + (day < 10 ? '0' : '') + day;
//      $('#current-date').text(date);
//      $('#current-time').text(time);
//  }, 1000);
//}


//桌台筛选
function MealTable_Screen(data, laytpl) {
    $('#TableStatusList_view').delegate('li', 'click', function (event) {
        $(this).addClass('layui-this').siblings('li').removeClass('layui-this');
        var key = $(this).attr('data-key');
        var area = $('#AreaList_view .layui-this').attr('data-area');
        Show_data(data, key, area, laytpl);
    })
    $('#AreaList_view').delegate('a', 'click', function (event) {
        $(this).addClass('layui-this').siblings('a').removeClass('layui-this');
        var area = $(this).attr('data-area');
        var key = $('#TableStatusList_view .layui-this').attr('data-key');
        Show_data(data, key, area, laytpl);
    })
    
    $('#AreaList_view .class-group-list a:first').click();
}

//筛选结果渲染数据
function Show_data(data, key, area, laytpl) {

    var listdata = [];

//  if (area == 0 && key == 0) {// 区域全部、状态全部
//      listdata = data;
//  } else if (area != 0 || key != 0) {// 区域全部

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
//  }
    //渲染桌台1
    var getTpl = TableList_tpml.innerHTML
        , view = document.getElementById('TableList_view');
    laytpl(getTpl).render(listdata, function (html) {
        view.innerHTML = html;
    });
//  SetMealTableWidth();

    TableClick(listdata);
}


function TableClick(data) {
    var tableBox = $('#TableList_view li a');
    tableBox.unbind("click");
    tableBox.click(function (event) {
        var no = parseInt($(this).attr('data-no'));
        var tableData = data[no];
        if (tableData.CythStatus == 1) {//空台
            var ydstate = $(this).find('.MealTable-state').find('p');
            if (ydstate.length > 0) {//存在当前分市的预定
                layer.confirm('该台存在当前分市的预定，您确定要开台吗？', {
                    btn: ['确认开台', '取消'] //按钮
                }, function () {
                	layindex = layer.open({type: 3,shadeClose: false});
                    location.href = "/Res/MHome/OpenTable/" + tableData.Id;
                }, function () {
                    layer.close(layer.index);
                });

            } else {
            	layindex = layer.open({type: 3,shadeClose: false});
                location.href = "/Res/MHome/OpenTable/" + tableData.Id;
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
                    success: function(layero,index){
                    	
                    	layero.find('li').on('click',function(){
                    		var id = $(this).attr('data-id');
                    		var index = $(this).index();
                    		if(tableData.OrderNow[index].IsControl){
                    			parent.layer.confirm('该桌台（ '+tableData.Name+' ）中的第（ '+(index+1)+' ）个订单正在被操作,是否确认要进入该桌台点餐?', {icon: 3, title:'提示'}, function(index){
                    				layer.close(index);
                    				layindex = layer.open({type: 3,shadeClose: false});
                    				location.href = '/Res/MHome/BatchChoseProject?OrderTableIds='+ id
                    			});
                    		}else{
                    			location.href = '/Res/MHome/BatchChoseProject?OrderTableIds='+ id
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
		                layindex = layer.open({type: 3,shadeClose: false});//loading
		                location.href = "/Res/MHome/BatchChoseProject?OrderTableIds=" + ordTableArr.join(',');
					});
            	}else{
            		layindex = layer.open({type: 3,shadeClose: false});//loading
            		var orderTableId = tableData.OrderNow[0].Id;
	                var ordTableArr = [];
	                ordTableArr.push(orderTableId);
	                
	                location.href = "/Res/MHome/BatchChoseProject?OrderTableIds=" + ordTableArr.join(',');
            	}
            	
            	
//          	layindex = layer.open({type: 3,shadeClose: false});
//	            
//              var orderTableId = tableData.OrderNow[0].Id;
//              var ordTableArr = [];
//              ordTableArr.push(orderTableId);
//              
//              location.href = "/Res/MHome/BatchChoseProject?OrderTableIds=" + ordTableArr.join(',');
            }

        } else if (tableData.CythStatus == 3) {//清洁

            layer.confirm("确认空置吗？", { title: '更新餐台状态提示', btn: ['确认', '取消'], btnAlign: 'c' }, function () {
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

    var para = { tableIds: tableIds };
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

//退出登录
function LoginOut() {
	layindex = layer.open({
	    type: 3,
	    shadeClose: false //点击遮罩关闭层
	});
	history.go(-1);
//	location.reload()
//  window.location.href = "/Res/MAccount/login";
}




//弹出批量置空
function BatchEmpty() {
    var str = '';
    for (var i = 0; i < inidata.TableList.length; i++) {
        var item = inidata.TableList[i];
        if (item.CythStatus == 3) {//空台
            str += '<li data-id="' + item.Id + '">'
                + '<div class="MealTable-head flex">'
                + '<span class="item MealTable-number flex-item">' + item.Id + '</span>'
                + '</div>'
                + '<div class="MealTable-title" style="line-height: 15px;font-size:16px;">' + item.Name + ' </div>'
                + '</li>';
        }
    }
    if (!str) {
        layer.msg('当前没有可置空的桌台!');
        return false;
    }
    var strHtml = '<div class="MealTable-lists box-sm" style="margin-right:0;"><ul id="BatchEmpty">' + str + '</ul></div>';


    layer.open({
        type: 1,
        anim: -1,
        title: '请选择需要置空的桌台',
        shadeClose: true,
        skin: 'layer-header',
        shade: 0.8,
        area: ['80%', '80%'],
        content: strHtml
        , btn: ['确认置空', '取消']
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
        , success: function (layero) {
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
                if ($li.filter('.checked').length === len) {
                    isAllSelected.get(0).checked = true;
                } else {
                    isAllSelected.attr("checked", false);
                }
                form.render('checkbox', 'empty_MealTable');
            });

        }
    });
}


