
var winW = $(window).width();
var winH = $(window).height();


//iframe自适应
function iframe_height() {
    $(window).on('resize', function () {
        var $content = $('#admin-body');
        $content.height($(this).height() - 70);
        $content.find('iframe').each(function () {
            $(this).height($content.height());
        });
    }).resize();
}




var classdata = [{
    "Id": "Welcome",
    "Name": "工作台",
    "Url": "javascript:void(0);",
    "Lists": [{
        "Id": "Page_Welcome",
        "Name": "餐厅",
        "Icon": "&#xe64e;",
        "Url": "/Res/Home/NewWelcome",
        "Lists": []
    },
    {
        "Id": "Page_Check",
        "Name": "查单",
        "Icon": "&#xe607;",
        "Url": "/Res/Order/OrderSearchList",
        "Lists": []
    },
    {
        "Id": "Page_Shift",
        "Name": "交班",
        "Icon": "&#xe600;",
        "Url": "/Res/Statistics/TurnDuty",
        "Lists": []
    },
    {
        "Id": "Page_Estimate",
        "Name": "估清",
        "Icon": "&#xe713;",
        "Url": "/Res/Home/BatchCLearProject",
        "Lists": []
    },
    {
        "Id": "Page_ProjectRecommend",
        "Name": "推荐",
        "Icon": "&#xe73d;",
        "Url": "/Res/Project/ProjectRecommend",
        "Lists": []
    },
    {
        "Id": "Nocturnal_Audit",
        "Name": "夜审",
        "Icon": "&#xe665;",
        "Url": "",
        "Lists": []
    }]
},
{
    "Id": "Reserve",
    "Name": "预定管理",
    "Url": "javascript:void(0);",
    "Lists": [{
        "Id": "Page_Reserve",
        "Name": "预定管理",
        "Icon": "&#xe607;",
        "Url": "/Res/Order/NewOrderReserveList",
        "Lists": []
    },
    {
        "Id": "Page_AddReserve",
        "Name": "新增预定",
        "Icon": "&#xe64e;",
        "Url": "/Res/Order/NewReserve",
        "Lists": []
    },
    //{
    //    "Id": "Page_ReserveNav",
    //    "Name": "预定导航",
    //    "Icon": "&#xe600;",
    //    "Url": "/Res/Home/NewWelcome",
    //    "Lists": []
    //},
    {
        "Id": "Page_ReserveLists",
        "Name": "预定预测",
        "Icon": "&#xe665;",
        "Url": "/Res/Order/NewForecast",
        "Lists": []
    }]
},
{
    "Id": "DataChats",
    "Name": "数据统计",
    "Url": "javascript:void(0);",
    "Lists": [{
        "Id": "Page_Produced",
        "Name": "出品统计",
        "Icon": "&#xe64e;",
        "Url": "/Res/Statistics/Produced",
        "Lists": []
    }, {
        "Id": "Page_Produced",
        "Name": "报表查询",
        "Icon": "&#xe64e;",
        "Url": "/Res/Statistics/ReportList",
        "Lists": []
    }]
},
{
    "Id": "Sys",
    "Name": "系统设置",
    "Url": "javascript:void(0);",
    "Lists": [{
        "Id": "Page_Sys1",
        "Name": "餐厅设施管理",
        "Icon": "&#xe64e;",
        "Url": "javascript:void(0);",
        "Lists": [
            {
                "Id": "Page_Sys11",
                "Name": "餐厅管理",
                "Icon": "&#xe64e;",
                "Url": "/Res/Restaurant/NewIndex"
            },
            {
                "Id": "Page_Sys12",
                "Name": "区域管理",
                "Icon": "&#xe64e;",
                "Url": "/Res/Area/NewIndex"
            },
            {
                "Id": "Page_Sys12",
                "Name": "包厢管理",
                "Icon": "&#xe64e;",
                "Url": "/Res/Box/NewIndex"
            },
            {
                "Id": "Page_Sys12",
                "Name": "台号管理",
                "Icon": "&#xe64e;",
                "Url": "/Res/Table/NewIndex"
            },
            {
                "Id": "Page_Sys12",
                "Name": "档口管理",
                "Icon": "&#xe64e;",
                "Url": "/Res/Stalls/NewIndex"
            }]
    },
    {
        "Id": "Page_Reserve",
        "Name": "餐饮管理",
        "Icon": "&#xe607;",
        "Url": "javascript:void(0);",
        "Lists": [{
            "Id": "Page_Sys12",
            "Name": "分市管理",
            "Icon": "&#xe64e;",
            "Url": "/Res/Market/NewIndex"
        },
        {
            "Id": "Page_Sys12",
            "Name": "类别管理",
            "Icon": "&#xe64e;",
            "Url": "/Res/Category/NewIndex"
        }, {
            "Id": "Page_Sys12",
            "Name": "商品特殊要求管理",
            "Icon": "&#xe64e;",
            "Url": "/Res/Extend/NewIndex"
        },
        {
            "Id": "Page_Sys12",
            "Name": "餐饮扩展类别",
            "Icon": "&#xe64e;",
            "Url": "/Res/ExtendType/NewIndex"
        },
        {
            "Id": "Page_Sys12",
            "Name": "餐饮项目管理",
            "Icon": "&#xe64e;",
            "Url": "/Res/Project/NewIndex"
        },
        {
            "Id": "Page_Sys12",
            "Name": "套餐管理",
            "Icon": "&#xe64e;",
            "Url": "/Res/Package/NewIndex"
        },
        {
            "Id": "Page_Sys12",
            "Name": "自定义折扣管理",
            "Icon": "&#xe64e;",
            "Url": "/Res/Discount/NewIndex"
        },
        {
            "Id": "Page_Sys12",
            "Name": "订单类型管理",
            "Icon": "&#xe64e;",
            "Url": "/Res/CommonExtend/NewIndex?typeId=10001"
        },
        {
            "Id": "Page_Sys12",
            "Name": "客源管理",
            "Icon": "&#xe64e;",
            "Url": "/Res/CommonExtend/NewIndex?typeId=10002"
        },
        {
            "Id": "Page_Sys12",
            "Name": "支付方式管理",
            "Icon": "&#xe64e;",
            "Url": "/Res/PayMethod/Index"
        }, {
            "Id": "Page_Sys12",
            "Name": "赠退菜理由",
            "Icon": "&#xe64e;",
            "Url": "/Res/OrderDetailCause/Index"
        }]
    }, {
        "Id": "Page_Sys1",
        "Name": "硬件设施管理",
        "Icon": "&#xe64e;",
        "Url": "javascript:void(0);",
        "Lists": [{
            "Id": "Page_Sys11",
            "Name": "打印机管理",
            "Icon": "&#xe64e;",
            "Url": "/Res/Printer/NewIndex"
        },
        {
            "Id": "Page_Sys11",
            "Name": "微信出单区域管理",
            "Icon": "&#xe64e;",
            "Url": "/Res/AreaPrint/NewIndex"
        },
        {
            "Id": "Page_Sys11",
            "Name": "换台出单区域管理",
            "Icon": "&#xe64e;",
            "Url": "/Res/AreaPrint/ChangeTableIndex"
        },
        {
            "Id": "Page_Sys11",
            "Name": "总单出单区域管理",
            "Icon": "&#xe64e;",
            "Url": "/Res/AreaPrint/GeneralOrderIndex"
        }
        ]
    },
    {
        "Id": "Page_Sys1",
        "Name": "自定义设置",
        "Icon": "&#xe64e;",
        "Url": "javascript:void(0);",
        "Lists": [{
            "Id": "Page_Sys11",
            "Name": "点餐按钮设置",
            "Icon": "&#xe64e;",
            "Url": "/Res/CustomConfig/OrderChoseProjectConfig"
        }]
    }
    ]
}
];

var laytpl, element, form;
var Page_Check_layerindex = '';

layui.use(['element', 'laytpl', 'layer', 'form'], function () {
    element = layui.element;
    laytpl = layui.laytpl;
    form = layui.form;
    var layer = layui.layer;


    //获取当前餐厅数据
    $.ajax({
        url: "/Res/Home/GetRestauantInfo",
        data: {},
        type: "post",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: true,
        beforeSend: function (xhr) {
            layindex = layer.open({ type: 3 });
        },
        complete: function (XMLHttpRequest, textStatus) {
            layer.close(layindex);
        },
        success: function (data, textStatus) {
            inidata = data;


            if (!inidata.NightTrial) {
                classdata[0].Lists.splice(4, 1);
            }

            iframe_height();

            ClassData('0', 0);

            //渲染主菜单
            var getTpl = MainNav_tpml.innerHTML
                , view = document.getElementById('MainNav_view');
            laytpl(getTpl).render(classdata, function (html) {
                view.innerHTML = html;
            });

            //一级导航点击切换二级(左侧)
            $('#MainNav_view').delegate('li.layui-nav-item', 'click', function (event) {
                var no = $(this).attr('data-no');
                var index = $(this).index();
                $(this).addClass('layui-this').siblings('li.layui-nav-item').removeClass('layui-this');
                ClassData(no, index);

            });




            //初始化操作状态
            var mymode = layui.data('set');
            if (mymode.mymode == 'touch') {//鼠标
                $('#mymode').html('<i class="icon iconfont">&#xe605;</i>').attr({
                    'data-mode': 'touch',
                    'title': '触摸操作'
                });
                layui.data('set', {
                    key: 'mymode'
                    , value: 'touch'
                });
            } else {
                $('#mymode').html('<i class="icon iconfont">&#xe601;</i>').attr({
                    'data-mode': 'mouse',
                    'title': '鼠标操作'
                });
                layui.data('set', {
                    key: 'mymode'
                    , value: 'mouse'
                });
            }



            //左侧导航点击事件
            $('.layui-nav-tree').delegate('a', 'click', function (event) {
                event.preventDefault();
                var url = $(this).attr("href");
                var navtitle = $(this).html();
                var navtype = $(this).attr('type');
                var navid = $(this).attr('id');

                if (url == 'javascript:void(0);') {
                    return false;
                }
                //查单
                if (navid === "Page_Check") {
                    layer.open({
                        type: 2,
                        anim: -1,
                        title: navtitle.trim(),
                        shadeClose: true,
                        skin: 'layer-header Page_Check_layer',
                        shade: 0.8,
                        area: ['100%', '100%'],
                        content: url
                    });
                    return false
                }
                //估清   || 推荐
                if (navid === "Page_Estimate" || navid === 'Page_ProjectRecommend') {
                    layer.open({
                        type: 2,
                        anim: -1,
                        title: navtitle.trim(),
                        shadeClose: true,
                        skin: 'layer-header',
                        shade: 0.8,
                        area: ['100%', '100%'],
                        content: url
                    });
                    return false
                }
                //夜审
                if (navid === "Nocturnal_Audit") {
                    $.ajax({
                        url: "/Res/Order/BeforeNightTrial",
                        type: "post",
                        dataType: "json",
                        async: false,
                        beforeSend: function (xhr) {
                            layindex = layer.open({ type: 3 });
                        },
                        complete: function (XMLHttpRequest, textStatus) {
                            layer.close(layindex);
                        },
                        success: function (data, textStatus) {
                            console.log(data)
                            if (data.Data) {
                                layer.confirm(data.Message + '是否确认执行夜审操作', { icon: 3, title: '提示' }, function (index) {
                                    layer.close(index);
                                    $.ajax({
                                        url: "/Res/Order/NightTrial",
                                        type: "post",
                                        dataType: "json",
                                        beforeSend: function (xhr) {
                                            layindex = layer.open({ type: 3 });
                                        },
                                        complete: function (XMLHttpRequest, textStatus) {
                                            layer.close(layindex);
                                        },
                                        success: function (data, textStatus) {
                                            if (data.Data) {
                                                var id = $('#main').attr('data-obj');
                                                if (id == 1) {
                                                    document.getElementById('main').contentWindow.addDate()
                                                }
                                                layer.msg('操作成功');

                                            } else {
                                                layer.msg(data.Message)
                                            }
                                        }
                                    });
                                });
                            } else {
                                layer.msg(data.Message)
                            }
                        }
                    });
                    return false
                }

                $('#main').removeAttr('data-obj')

                if (navtype == 'tab') {
                    //tab页签
                    var isnav = $('.layui-tab-title').find('li[lay-id="' + navid + '"]');
                    if (isnav.length < 1) {
                        element.tabAdd('admintab', {
                            title: navtitle
                            , content: '<iframe frameBorder=0 id="admin_body" name="admin_body" style="width: 100%;height: 100%;" scrolling=yes src="' + url + '" ></iframe>'
                            , id: navid
                        })
                    }
                    element.tabChange('admintab', navid);
                    iframe_height();
                } else {
                    if (navid == 'Page_Welcome') $('#main').attr('data-obj', '1');
                    $('#main').attr('src', url);
                }
            });

            //操作模式切换
            $('.layui-layout-right').delegate('a#mymode', 'click', function (event) {
                var div = $(this);
                var mode = div.attr('data-mode');
                if (mode == 'mouse') {//改为触摸
                    div.attr('data-mode', 'touch').html('<i class="icon iconfont">&#xe605;</i>');
                    layui.data('set', {
                        key: 'mymode'
                        , value: 'touch'
                    });

                } else {//改为鼠标
                    div.attr('data-mode', 'mouse').html('<i class="icon iconfont">&#xe601;</i>');
                    layui.data('set', {
                        key: 'mymode'
                        , value: 'mouse'
                    });
                }

            });


            //渲染当前餐厅名称
            var getTpl = RestaurantName_tpml.innerHTML
                , view = document.getElementById('RestaurantName_view');
            laytpl(getTpl).render(data, function (html) {
                view.innerHTML = html;
            });

            //渲染当前用户名
            var getTpl = UserName_tpml.innerHTML
                , view = document.getElementById('UserName_view');
            laytpl(getTpl).render(data, function (html) {
                view.innerHTML = html;
            });

            //可选餐厅列表
            SetRestaurant(data.RestaurantList);

            //风格设置  窗口渲染
            myStyleSetting(form, data);

            //控制台  餐桌  风格颜色渲染
            document.getElementById('main').contentWindow.tableStyleInit && document.getElementById('main').contentWindow.tableStyleInit();

        }
    });
});



//切换餐厅
function SetRestaurant(list) {
    $('#RestaurantName_view').delegate('#SetRestaurant', 'click', function (event) {
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
                content: '<ul class="choice-shop layui-form">' + shoplist + '</ul>',
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


//选择餐厅切换事件
function entrySys(id, marketId) {
    $.ajax({
        type: "post",
        url: "/Res/Account/SelectRestaurant",
        data: JSON.stringify({ id: id, marketId: marketId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        beforeSend: function (xhr) {
            layindex = layer.open({ type: 3 });
        },
        success: function (data, textStatus) {
            if (data.Data == true) {
                // $("#btnLogin").find('span').html("登录成功，正在跳转...");
                window.setTimeout(function () {
                    window.location.href = "/Res/Home/NewIndex";
                }, 200);

            } else {
                layer.alert(data.Message);
            }
        }
    });
}



var orderTableIds;
//弹出点餐页面
function OpenOrder(OrderTableIds) {
    orderTableIds = OrderTableIds;
    var arr = [];
    OrderTableIds instanceof Array ? arr = OrderTableIds : arr.push(OrderTableIds);
    $("#main")[0].contentWindow.layer.closeAll('page');
    layer.open({
        type: 2,
        anim: -1,
        title: '点餐',
        shadeClose: true,
        skin: 'layer-header chooseProject-layer',
        shade: 0.8,
        area: ['100%', '100%'],
        content: "/Res/Home/NewBatchChoseProject?OrderTableIds=" + arr.join(','),
        cancel: function (index, layero) {
            var layertitle = $(layero).find('.layui-layer-title').text();
            if (layertitle == '点餐') {
                //判断是否存在未保存的菜品
                var layerIframeWindow = (layero).find('.layui-layer-content').find('iframe')[0].contentWindow;
                var is = layerIframeWindow.isNewProject();
                if (is) {
                    layer.confirm('有未保存的菜品，是否保存？', {
                        btn: ['保存', '退出'] //按钮
                    }, function (index) {
                        $('#actionsbtn_view', layerIframeWindow.document).find('button[name=Keep]').click();
                        layer.close(index);
                    }, function () {
                        layer.closeAll();
                        parent.layer.closeAll();
                    });
                    //阻止关闭
                    return false;
                }
            } else if (layertitle == '结账') {
            }
        },
        end: function (layero) {
            if (orderTableIds.length > 0) {
                $.ajax({
                    type: "post",
                    url: "/Res/Order/UpdateOrderTableIsControl",
                    dataType: "json",
                    data: { ordertableIds: orderTableIds, isControl: false },
                    beforeSend: function (xhr) {
                        layindex = layer.open({ type: 3 });
                    },
                    complete: function (XMLHttpRequest, textStatus) {
                        layer.close(layindex);
                    },
                    success: function (data, textStatus) {
                        if (data.Data == true) {
                            //$.connection.hub.start().done(function() {
                            //	chat.server.notifyResServiceRefersh(true);
                            //                         layer.closeAll();
                            //                     });
                            layer.closeAll();
                        } else {
                            layer.alert(data.Message);
                        }
                    }
                });
            } else {
                //$.connection.hub.start().done(function () {
                //            	chat.server.notifyResServiceRefersh(true);
                //});
            }
        }
    });
}


//初始化菜单(左侧)
function ClassData(no, index) {
    var classnavdata = classdata[no].Lists
    var getTpl = ClassNav_tpml.innerHTML
        , view = document.getElementById('ClassNav_view');
    laytpl(getTpl).render(classnavdata, function (html) {
        view.innerHTML = html;
    });

    element.init();
    if (index == 0 && $(window).width() < 1280) {
        $('body').addClass('telescopic');
        $('.layui-nav-tree#ClassNav_view li').on('mouseenter', function (e) {
            var val = $(this).children('a').children('span').html();
            layer.tips(val, this);
        }).on('mouseleave', function () {
            layer.closeAll('tips');
        })
    } else {
        $('body').removeClass('telescopic');
    }
    $('.layui-nav-tree#ClassNav_view li').eq(0).find('a').eq(0).click();
    $('.layui-nav-tree#ClassNav_view li').eq(0).find('dl.layui-nav-child').find('dd').eq(0).find('a').click();
}

//修改密码 => 弹窗
function revisePwdWindow() {
    var html = '<form class="layui-form form-col-two" id="revisePwdForm" style="padding:20px 40px 0 0">' +
        '<div class="layui-form-item" style="width: 100%;">' +
        '<label class="layui-form-label">现密码：</label>' +
        '<div class="layui-input-block">' +
        '<input type="password" class="layui-input" id="oldPassword" name="oldPassword" placeholder="请输入密码" lay-required-msg="请输入密码">' +
        '</div>' +
        '</div>' +
        '<div class="layui-form-item" style="width: 100%;">' +
        '<label class="layui-form-label">新密码：</label>' +
        '<div class="layui-input-block">' +
        '<input type="password" class="layui-input" id="newPassword" name="newPassword" placeholder="请输入新密码" lay-required-msg="请输入新密码">' +
        '</div>' +
        '<button class="layui-btn layui-btn-normal" id="revisepwdSubmitBtn" lay-submit lay-filter="revisepwdSubmit" style="display:none;">立即提交</button>' +
        '</div>' +
        '</form>'
    layer.open({
        type: 1,
        shade: 0,
        title: '修改密码',
        closeBtn: 0,
        skin: 'layer-practice ProjectLayer',
        area: ['400px', '230px'],
        content: html,
        btn: ['确定', '取消'],
        yes: function (layero, index) {
            $('#revisepwdSubmitBtn').click();
        },
        success: function (layero, index) {
            $('#oldPassword').focus();
        }
    });

    form.on('submit(revisepwdSubmit)', function (data) {
        console.log(data.field) //当前容器的全部表单字段，名值对形式：{name: value}
        $.ajax({
            type: "post",
            url: "/Res/Account/ChangePassword",
            data: JSON.stringify(data.field),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            beforeSend: function (xhr) {
                layindex = layer.open({ type: 3 });
            },
            complete: function (XMLHttpRequest, textStatus) {
                layer.close(parent.layindex);
            },
            success: function (data, textStatus) {
                if (data.Data == true) {
                    layer.closeAll();
                    layer.msg('修改成功')
                } else {
                    layer.alert(data.Message);
                }
            }
        });
        return false; //阻止表单跳转。如果需要表单跳转，去掉这段即可。
    });
}

//修改密码	=> 提交
function revisePwdSubmit(data) {
}

//风格设置
function myStyleSetting(form, data) {
    var userName = data.UserName;
    var userOptions = layui.data(userName);//当前风格数据
    var defaultOptions = { //默认风格数据
		/*
		 * add    在用
		 * dirty  脏台
		 * empty  空置
		 * lock  锁定
		 */
        addBG: "#F5A187",
        addColor: "#ffffff",
        addState: "#009688",
        lockBG: "#F5A187",
        lockColor: "#ffffff",
        lockState: "#009688",
        dirtyBG: "#999999",
        dirtyColor: "#ffffff",
        dirtyState: "#009688",
        emptyBG: "#ffffff",
        emptyColor: "#000000",
        emptyState: "#009688",
        wxBG: "#44b549",
        wxColor: "#ffffff",
        wxState: "#ffffff",
		/*
		* 菜单
		* ChoseProjectOrderDefault 	可选
		* ChoseProjectOrderActive  	选中
		* ChoseProjectOrderDisabled	禁用
		*/
        ChoseProjectOrderDefaultBG: "#f1f1f1",
        ChoseProjectOrderDefaultColor: "#333333",
        ChoseProjectOrderDefaultIcon: "#1E9FFF",
        ChoseProjectOrderActiveBG: "#ffffff",
        ChoseProjectOrderActiveColor: "#333333",
        ChoseProjectOrderActiveIcon: "#1E9FFF",
        ChoseProjectOrderDisabledBG: "#dddddd",
        ChoseProjectOrderDisabledColor: "#333333",
        ChoseProjectOrderDisabledIcon: "#1E9FFF",
        ChoseProjectBG: "#efefef",
        ChoseProjectPrice: "#efefef",
    }
    var StyleSettingDiv = $('#myStyleSetting');
    //打开风格设置窗口
    $('#myStyleBtn').on('click', function () {
        StyleSettingDiv.show();
    })

    //点击空白处   窗口 隐藏
    //	StyleSettingDiv.children('.myStyleSettingBG').on('click',function(){
    //		StyleSettingDiv.hide();
    //	})

    //取消
    StyleSettingDiv.find('.layui-btn-primary').on('click', function () {
        //		StyleSettingDiv.hide();
        var id = $('#main').attr('data-obj');
        if (id == 1) {
            if (!isEmptyObject(userOptions)) {
                userOptions = layui.data(userName);
                for (var i in userOptions) {
                    StyleSettingDiv.find('input[name="' + i + '"]').val(userOptions[i]).prev().colpickSetColor(userOptions[i]).css('background', userOptions[i]);
                }
                document.getElementById('main').contentWindow.tableStyle(userOptions);
            } else {
                for (var i in defaultOptions) {
                    StyleSettingDiv.find('input[name="' + i + '"]').val(defaultOptions[i]).prev().colpickSetColor(defaultOptions[i]).css('background', defaultOptions[i]);
                }
                $('#myTableStyle', document.getElementById('main').contentWindow.document).remove();
            }
        }
        StyleSettingDiv.hide();
    })

    //还原
    StyleSettingDiv.find('.layui-btn').eq(2).on('click', function () {
        var id = $('#main').attr('data-obj');
        if (id == 1) {
            layui.data(userName, null);

            for (var i in defaultOptions) {
                StyleSettingDiv.find('input[name="' + i + '"]').val(defaultOptions[i]).prev().colpickSetColor(defaultOptions[i]).css('background', defaultOptions[i]);
            }
            $('#myTableStyle', document.getElementById('main').contentWindow.document).remove();

            StyleSettingDiv.hide();
        }
    })

    if (!isEmptyObject(userOptions)) {
        //有
        for (var i in userOptions) {
            StyleSettingDiv.find('input[name=' + i + ']').val(userOptions[i])
        }
    } else {
        //没有
        temporaryOptions = [{}, {}];//临时 options
    }

    //初始化颜色拾取器
    $.each(StyleSettingDiv.find('.color-box'), function () {
        var val = $(this).next().val();
        $(this).css('background-color', val);
        $(this).colpick({
            colorScheme: 'dark',
            submit: 0,
            color: val,
            layout: 'hex',
            onChange: function (hsb, hex, rgb, el, bySetColor) {
                if (!bySetColor) {
                    var $main = $('#main');
                    var $input = $(el).next();
                    var index = $(el).closest('.layui-tab-item').index();
                    $(el).css('background-color', '#' + hex);
                    $input.val('#' + hex);
                    var id = $main.attr('data-obj');
                    if (id == 1 && index == 0) {
                        if (!isEmptyObject(userOptions)) {
                            userOptions[$input.attr('name')] = '#' + hex;
                            document.getElementById('main').contentWindow.tableStyle && document.getElementById('main').contentWindow.tableStyle(userOptions);
                        } else {
                            temporaryOptions[$input.attr('name')] = '#' + hex;
                            document.getElementById('main').contentWindow.tableStyle && document.getElementById('main').contentWindow.tableStyle(temporaryOptions);
                        }
                    }

                }
            }
        })
    })

    form.on('submit(myStyleSave)', function (data) {
        for (var i in data.field) {
            layui.data(userName, {
                key: i
                , value: data.field[i]
            });
        }
        StyleSettingDiv.hide();
        return false;
    })
}

//退出登录
function LoginOut() {
    window.location.href = "/Res/Account/NewLogout";
}


//更新反结弹窗index
function newPage_Check_layerindex(index) {
    Page_Check_layerindex = index;
}
function RefreshOrdeInfo_index(id) {
    $(".Page_Check_layer").find('iframe')[0].contentWindow.RefreshOrdeInfo(id);
    layer.close(Page_Check_layerindex);
}

//更新大图模式	内容 并且显示
function bigPhotoUpdateAndShow(data, index) {
    var getTpl = bigPhoto_tpml.innerHTML
        , view = document.getElementById('bigPhoto_view');
    $(view).fadeIn();
    laytpl(getTpl).render(data, function (html) {
        view.innerHTML = html;
        new Swiper('#bigPhoto_view .swiper-container', {
            zoom: true,
            loop: true,
            initialSlide: parseFloat(index),
            autoplayDisableOnInteraction: false,
            lazy: {
                loadPrevNext: true,
            }
        });

        //关闭大图模式
        $('#bigPhoto_view .back').one('click', function () {
            $('#bigPhoto_view').empty().fadeOut();
        })
    });

}




//判断对象是否为空
function isEmptyObject(obj) {
    　　for (var key in obj) {
        　　　　return false;//返回false，不为空对象
    　　}
    　　return true;//返回true，为空对象
}

