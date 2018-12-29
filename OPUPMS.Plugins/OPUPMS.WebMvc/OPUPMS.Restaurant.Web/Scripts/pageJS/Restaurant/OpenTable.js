
/**
 * 开台
 */

//获取参数
isDisable = false; //开台是否可点击  控制器
layui.use(['element', 'form'], function () {
    var element = layui.element,
        form = layui.form;

    $('input[name="PersonNum"]').focus();



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


    form.on('submit(OpenTable)', function (data) {
    	if(isDisable)return false;//阻止多次提交
    	
        var name = data.elem.name;

        var formdata = data.field;
        var Tablelists = $('#Table_lists li');
        var tabIds = [];
        for (var i = 0; i < Tablelists.length; i++) {
            var tableid = Tablelists.eq(i).find('.MealTable-number').text();
            var Num = Tablelists.eq(i).find('.add_minus input').val();
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
        
        layindex = layer.open({type: 3,shadeClose: true});
		isDisable = true;
        //提交数据
        $.ajax({
            url: "/Res/Home/OpenTableCreate",
            data: JSON.stringify(para),
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: true,
	        complete: function (XMLHttpRequest, textStatus) {
	            layer.close(layindex);
	        },
            success: function (data, textStatus) {
                var res = data.Data;
                if (res) {
                    $.connection.hub.start().done(function () {//通知刷新工作台界面
                        chat.server.notifyResServiceRefersh(true);
                        layer.msg('开台成功！');
	                    if (name == 'order') {//开台点餐
							layer.open({type: 3});
	                        var orderTableIds = data.Data.OrderTableIds;
	                        location.href = "/Res/Home/NewWelcome";
	                        parent.OpenOrder(orderTableIds);
	                    } else {
	                    	layer.open({type: 3});
	                        location.href = "/Res/Home/NewWelcome";
	                    }
                    });


                } else {
                	isDisable = false;
                    layer.alert(data["Message"]);
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
                //layer.close(layindex);
            }
        });
        return false;

    });
	
	
	//更多按钮
	$('#moreInfoBtn').on('click',function(){
		if($(this).hasClass('disable'))return;
		var _this = $(this)
		var $moreDiv = $('.more-info');
		$moreDiv.slideToggle(200);
		$(this).addClass('disable');
		if($(this).hasClass('expand')){
			$(this).removeClass('expand');
		}else{
			$(this).addClass('expand');
		}
		setTimeout(function(){_this.removeClass('disable')},200)
	})
	
	//更多   提示信息
	hintTips($('#moreInfoBtn legend'),"展开更多信息");
});