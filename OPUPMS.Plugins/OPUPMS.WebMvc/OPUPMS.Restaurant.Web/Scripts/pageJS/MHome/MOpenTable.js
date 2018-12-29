
/**
 * 开台
 */

//fastclick
FastClick.attach(document.body);

//获取参数
isDisable = false; //开台是否可点击  控制器
layui.use(['element', 'form'], function () {
    var element = layui.element,
        form = layui.form;
	
	
	var winH = $(window).height();

	//添加桌台自适应
	$('#AddTable').parent().css('height',winH - 150);
	$('#Table_lists').parent().css('height',winH - 225);

    //添加桌台
    $('.Panel-side .MealTable-lists ul').delegate('li a', 'click', function (event) {
        if ($(this).parent('li').hasClass('layui-this')) {
            return false;
        }
        $(this).parent('li').addClass('layui-this');
        var number = $(this).find('.MealTable-number').text();
        var title = $(this).find('.MealTable-title').text();
        var str = '<li>'
            + '<div class="MealTable-head flex">'
            + '<span class="item MealTable-number flex-item">' + number + '</span>'
            + '</div>'
            + '<div class="MealTable-title">' + title + '</div>'
            + '<a href="javascript:;" class="btn-del"><i class="layui-icon">&#x1006;</i></a>'
            + '</li>';
        $('.StartDesk-form .MealTable-lists ul').append(str);
    });

    //删除桌台
    $('.StartDesk-form .MealTable-lists ul').delegate('li a.btn-del', 'click', function (event) {
        var id = $(this).parent('li').find('.MealTable-number').text();
        $('#Table_' + id + '').removeClass('layui-this');
        $(this).parent('li').remove();

    });

    //区域筛选
    $('#TableAreas').delegate('a', 'click', function (event) {
        var id = $(this).attr('data-id');
        var tablelist = $('#AddTable li');
        $(this).addClass('layui-this').siblings().removeClass('layui-this');
        if (id != 0) {
            for (var i = 0; i < tablelist.length; i++) {
                var table = tablelist.eq(i);
                if (table.attr('data-areaid') == id) {
                    table.show();
                } else {
                    table.hide();
                }
            }
        } else {
            tablelist.show();
        }
    });
    
    $('#TableAreas a:first').click()


    form.on('submit(OpenTable)', function (data) {
    	if(isDisable)return;//阻止多次提交
    	layindex = layer.open({type: 3,shadeClose: true});
    	
        var name = data.elem.name;
        var formdata = data.field;
        var Tablelists = $('#Table_lists li');
        var tabIds = [];
        for (var i = 0; i < Tablelists.length; i++) {
            var tableid = Tablelists.eq(i).find('.MealTable-number').text();
            tabIds.push(tableid);
        }

        if (formdata.R_Market_Id == 0 || formdata.R_Market_Id == null)
        {
            layer.alert("请选择分市！");
            return false;
        }

        if (tabIds.length <= 0) {
            layer.msg('请至少选择一个桌号进行开台操作', {
                time: 4000
            });
            return false;
        }

        var chat = $.connection.systemHub;
        chat.hubName = 'systemHub';
        chat.connection.start();

        var para = { req: formdata, TableIds: tabIds };
        //提交数据
        $.ajax({
            url: "/Res/Home/OpenTableCreate",
            data: JSON.stringify(para),
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: true,
            beforeSend: function (xhr) {
	            layindex = layer.open({type: 3});
	        },
	        complete: function (XMLHttpRequest, textStatus) {
	            layer.close(layindex);
	        },
            success: function (data, textStatus) {
                var res = data.Data;
                if(data.Data){
                	if (res.OrderId > 0) {
		                $.connection.hub.start().done(function () {//通知刷新工作台界面
		                      chat.server.notifyResServiceRefersh(true);
		                });
						layer.open({type: 3,shadeClose: false});
	                    layer.msg('开台成功！');
	                    
	                    if (name == 'order') {//开台点餐
	                    	layer.open({type: 3});
	                        var orderTableIds = data.Data.OrderTableIds;
	                        location.replace("/Res/MHome/BatchChoseProject?OrderTableIds=" + orderTableIds.join(','))
	                    } else {
	                    	layer.open({type: 3});
	                        location.replace("/Res/MHome/Index")
	                    }
	                } else {
	                	isDisable = false;
	                    layer.alert(data["Message"]);
	                }
                }else {
                    layer.alert(data["Message"]);
                }
            }
        });
        return false;

    });
	
	
	//更多按钮
	$('#moreInfoBtn').on('click',function(){
		if($(this).hasClass('disable'))return;
		var _this = $(this)
		var $moreDiv = $('.more-info');
		var list = $('.StartDesk-form .MealTable-lists');
		$moreDiv.slideToggle(200);
		$(this).addClass('disable');
		if($(this).hasClass('expand')){
			$(this).removeClass('expand');
			list.height(list.height() + 225);
		}else{
			$(this).addClass('expand');
			list.height(list.height() - 225);
		}
		setTimeout(function(){_this.removeClass('disable')},200)
	})
	
	//初始化动作全部完成时
	$('#loading').remove();
});



