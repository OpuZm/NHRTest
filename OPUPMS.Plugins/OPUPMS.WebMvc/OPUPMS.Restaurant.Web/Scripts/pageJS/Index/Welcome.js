var pageJs;
$(function () {
    pageJs = editJs.Create();
    pageJs.init();
})

function Refresh() {
    if (pageJs) {
        pageJs.Refresh();
    }
}

var editJs = {
    Create: function () {
        var editor = {};
        var layindex;
        var width = $(window).width();
        var height = $(window).height();

        /* 餐饮实时状态更新 --start */
        var chat = $.connection.systemHub;
        chat.hubName = 'systemHub';
        chat.connection.start();

        chat.client.callResServiceRefersh = function (result) {
            if (result == true) {
                Refresh();
            }
        }
    /* 餐饮实时状态更新 --end */


        //var bindAction = function () {
        var vmControl = new Vue({
            el: "#contextmenu",
            data: {
                controls1: ["预定", "开台", "预定纪录"],
                controls2: ["点餐", "换桌", "并台", "拆台", "拼台", "结账", "取消订单", "预定纪录"],
                controls3: ["设为空置", "预定纪录"],
                statusShow: 0,
                isJoin: false,
                cancel: true
            }
        })

        var vm = new Vue({
            el: '#TabList',
            data: {
                tables: [],
                restaurants: [],
                areas: [],
                status: [],
                RestaurantSel: 0,
                AreaSel: 0,
                StatusSel: 0,
                TabClickId: 0,
                OrderId: 0,
                OrderTableId: 0,
                RestaurantId: 0,
                TableControlOrder: [],
                Control: "",
                OrderTables: [], //订单下所有OrderTable
                SelectedOrderTables: [], //结账选中的OrderTable
            },
            updated: function () {
                $('#ulList li').contextmenu({
                    target: '#contextmenu',
                    onItem: function (context, e) {
                        var url = "";
                        var para = "";
                        var text = $(e.target).text();
                        vm.Control = text;
                        switch (text) {
                            case "预定":                               
                                if (vm.TableControlOrder.length <= 1)
                                {
                                    url = "/Res/Home/Reserve?tableId=" + vm.GetTableClickId;
                                    OpenNextPage(text, url);
                                }
                                else
                                {                                   
                                    ChoseOrder();
                                }
                                break;
                            case "开台":
                                if (vm.TableControlOrder.length <= 1)
                                {
                                    url = "/Res/Home/OpenTable?tableId=" + vm.GetTableClickId;
                                    OpenNextPage(text, url);
                                }
                                else
                                {
                                    ChoseOrder();
                                }
                                break;
                            case "点餐":
                                if (vm.TableControlOrder.length <= 1) {
                                    url = "/Res/Home/ChoseProject?orderTableId=" + vm.GetOrderTableId;
                                    OpenNextPage(text, url);
                                }
                                else {
                                    ChoseOrder();
                                }

                                break;
                            case "换桌":
                                if (vm.TableControlOrder.length <= 1)
                                {
                                    para = "RestaurantId=" + vm.RestaurantId + "&TableId=" + vm.GetTableClickId + "&OrderTableId=" + vm.GetOrderTableId;
                                    url = "/Res/Home/ChangeTable?" + para;
                                    OpenNextPage(text, url);
                                }
                                else
                                {
                                    ChoseOrder();
                                }
                                break;
                            case "预定纪录":
                                url = "/Res/Home/ReserveHistory?tableId=" + vm.GetTableClickId;
                                OpenNextPage(text, url);
                                break;
                            case "设为空置":     
                                SetTableEmpty();
                                break;
                            case "并台":
                                para = "RestaurantId=" + vm.RestaurantId + "&TableId=" + vm.GetTableClickId + "&OrderTableId=" + vm.GetOrderTableId;
                                url = "/Res/Home/JoinTable?" + para + "&OrderId=" + vm.OrderId;
                                if (vm.TableControlOrder.length <= 1)
                                {
                                    OpenNextPage(text, url);
                                }
                                else
                                {
                                    ChoseOrder();
                                }
                                break;
                            case "拆台":
                                if (vm.TableControlOrder.length <= 1) {
                                    para = "RestaurantId=" + vm.RestaurantId + "&TableId=" + vm.GetTableClickId + "&OrderTableId=" + vm.GetOrderTableId;
                                    url = "/Res/Home/SeparateTable?" + para;
                                    OpenNextPage(text, url);
                                }
                                else
                                {
                                    ChoseOrder();
                                }
                                break;
                            case "拼台":
                                url = "/Res/Home/OpenUsingTable?tableId=" + vm.GetTableClickId;
                                OpenNextPage(text, url);
                                break;
                            case "取消订单":
                                if (vm.TableControlOrder.length <= 1) {
                                    CancelOrder();
                                }
                                else
                                {
                                    ChoseOrder();
                                }
                                break;
                            case "结账":
                                //如果结账的台号只属于一个订单
                                if (vm.TableControlOrder.length <= 1)
                                {                                 
                                    ChooseTable(text, vm);
                                }
                                //如果结账的台号属于多个订单，则进行选择要结账的订单
                                else
                                {
                                    ChoseOrder();
                                }
                                break;
                            default:
                                break;
                        }

              
                    }
                })
            },
            computed: {
                GetTableControlOrder: function () {
                    return this.TableControlOrder;
                },
                GetOrderTableId: function () {
                    return this.OrderTableId;
                },
                GetTableClickId: function () {
                    return this.TabClickId;
                },
                GetOrderTables: function () {
                    //alert("GetOrderTables");
                    return this.OrderTables;
                },
            },
            methods: {
                Add: function (index) {

                },
                Delete: function (index) {

                },
                //操作员选择订单之后（单选）
                OperatorSelectOrder: function (obj) {
                    this.OrderId = obj.OrderId;
                    this.OrderTableId = obj.Id;

                    AfterChoseOrder();
                },
                //操作员选择台号  (多选 选中/取消选中)
                OperatorSelectTable: function (obj, event) {
                    var target = event.target || window.event.srcElement;
                    target = $(target);
                    debugger;
                    if (target.hasClass("active"))
                    {
                        target.removeClass("active");
                        target.find("span").css("display", "none");
                        removeByValue(this.SelectedOrderTables, obj);
                    }
                    else
                    {
                        this.SelectedOrderTables.push(obj);
                        target.addClass("active");
                        target.find("span").css("display", "inline");
                    }

                  
                    //alert(this.SelectedOrderTables.length);
                },
              
                //操作员选择台号之后确定
                OperatorSelectTableConfirm: function () {

                    var SelectedOrderTables = [];
                    var orderTable = this.OrderTables;
                    var list = $("#TableChoose").find("li");
           
                    $(list).each(function () {                            
                        if ($(this).hasClass('active'))
                        {
                            var tableid = $(this).data("tableid");
                            $(orderTable).each(function () {
                              
                                if (this.R_Table_Id == tableid)
                                {
                                    SelectedOrderTables.push(this);
                                }
                            });
                        }
                    });

                    this.SelectedOrderTables = SelectedOrderTables;

                    if (this.SelectedOrderTables.length <= 0) {
                        alert("请选择结账台号！");
                        return;
                    }
                   
                    //打开结账画面之前，关闭台号选择画面
                    layer.closeAll();
                    var first = this.SelectedOrderTables[0];
      
                    //单台，不选择台号，打开结账画面
                    var checkoutDTO = new Object();
                    checkoutDTO.OrderId = first.R_Order_Id;
                    checkoutDTO.TableIds = new Array();
                    $(this.SelectedOrderTables).each(function () {
                        checkoutDTO.TableIds.push(this.R_Table_Id);
                    });
                   
                    OpenNextPageForCheckout(first.title, checkoutDTO);
                   
                },

                Search: function (index) {
                    Search();
                },
                FilterArea: function (resid) {
                    return this.areas.filter(function (area) {
                        return area.RestaurantId === resid;
                    })
                },
                FilterClass: function (status) {
                    var str = "";
                    switch (status) {
                        case 1:
                            str = "list-group-item list-group-item-success col-xs-2 li-margin-5";
                            break;
                        case 2:
                            str = "list-group-item list-group-item-danger col-xs-2 li-margin-5";
                            break;
                        case 3:
                            str = "list-group-item list-group-item-warning col-xs-2 li-margin-5";
                            break;
                        default:
                            str = "list-group-item list-group-item-success col-xs-2 li-margin-5";
                            break;
                    }
                    return str;
                },
                ContextMenu: function (index, event) {
                    var obj = this.tables[index];
                    vmControl.statusShow = obj.CythStatus;
                    vm.TabClickId = obj.Id;
                    vm.RestaurantId = obj.Restaurant;
                    if (obj.OrderNow != null) {
                        vm.TableControlOrder = obj.OrderNow;
                        //console.log(obj.OrderNow[0].Id);
                        if (obj.OrderNow.length == 1) {
                            vm.OrderTableId = obj.OrderNow[0].Id;
                            vm.OrderId = obj.OrderNow[0].OrderId;
                        }
                    }

                },
                GetStatusCount: function (status) {
                    var count = 0;
                    $(this.tables).each(function (i, o) {
                        if (o.CythStatus == status) {
                            count++;
                        }
                    })
                    return count;
                }
            }
        })


        //当前台号被多个订单占用时，操作员选择某个订单之后的后续处理
        function AfterChoseOrder() {
            var text = vm.Control;
            var url = "";
            var para = "";
            if (text != "" && text != "取消订单") {
                para = "RestaurantId=" + vm.RestaurantId + "&TableId=" + vm.TabClickId + "&OrderTableId=" + vm.GetOrderTableId;
                switch (text) {
                    case "点餐":
                        url = "/Res/Home/ChoseProject?orderTableId=" + vm.GetOrderTableId;
                        OpenNextPage(text, url);
                        break;
                    case "换桌":
                        url = "/Res/Home/ChangeTable?" + para;
                        OpenNextPage(text, url);
                        break;
                    case "并台":
                        url = "/Res/Home/JoinTable?" + para + "&OrderId=" + vm.OrderId;
                        OpenNextPage(text, url);
                        break;
                    case "拆台":
                        url = "/Res/Home/SeparateTable?" + para;
                        OpenNextPage(text, url);
                        break;
                    case "结账":                      
                        ChooseTable(text, vm);
                        break;
                }
            }
            else if (text == "取消订单") {
                CancelOrder();
            }
        }

        function OpenNextPage(title, url) {          
            layer.open({
                type: 2,
                title: title,
                area: [width + "px", height + "px"],
                content: url,
                maxmin: false
            })
        }

        function ChoseOrder() {         
            layer.open({
                type: 1,
                title: "选择操作订单",
                area: ["300px", "300px"],
                content: $("#TableOrderChose"),
                maxmin: false
            })
        }

        //结账时，如果待结账订单下有多个台号，让操作员选择要结账的台号
        function ChooseTable(title, vm) {
         
            //根据要结账的订单号，获取该订单下有哪些台号，即OrderTable begin
            $.ajax({
                type: "get",
                url: "/Res/Checkout/GetOrderTablesByOrderId?orderId=" + vm.OrderId,
                data: "",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                beforeSend: function (xhr) {
                    layindex = layer.open({
                        type: 3,
                        shadeClose: false, //点击遮罩关闭层
                    });
                },
                success: function (data, textStatus) {
                    if (data != null && (data instanceof Array)) {

                        //重新初始化数组对象，避免重复添加元素
                        vm.OrderTables =[];                      
                        vm.SelectedOrderTables = [];

                        $(data).each(function (i, o) {
                            o.title = title;
                            vm.OrderTables.push(o);
                            vm.SelectedOrderTables.push(o);//默认选中
                            
                        });

                        //多台，选择台号
                        if (vm.OrderTables.length > 1) {
                           
                            layer.open({
                                type: 1,
                                title: "选择操作台号",
                                area: ["500px", "500px"],
                                content: $("#TableChoose"),
                                maxmin: false
                            })
                        }
                        else {

                            //单台，不选择台号，打开结账画面
                            var checkoutDTO = {};
                            checkoutDTO.OrderId = vm.OrderId;
                            checkoutDTO.TableIds = [];
                            checkoutDTO.TableIds.push(vm.SelectedOrderTables[0].R_Table_Id);
                            OpenNextPageForCheckout(title,checkoutDTO);
                         
                           
                        }

                        
                    } else {
                        layer.msg(data.Info, {
                            time: 2000
                        });
                    }
                },
                complete: function (XMLHttpRequest, textStatus) {
                    layer.close(layindex);
                }
            })
            //根据要结账的订单号，获取该订单下有哪些台号，即OrderTable end

        

        }

        function OpenNextPageForCheckout(title,checkoutDTO)
        {
            layer.open({
                type: 2,
                title: "结账",
                area: [width + "px", height + "px"],
                content: "/Res/CheckOut/index?req=" + escape(JSON.stringify(checkoutDTO)),
                maxmin: false
            })
        }

        //删除数组指定元素
        function removeByValue(arr, val) {
            for (var i = 0; i < arr.length; i++) {
                if (arr[i] == val) {
                    arr.splice(i, 1);
                    break;
                }
            }
        }

        function Init() {
            GetRestaurants();
            GetAreas();
            GetTableStatus();
            if (vm.restaurants.length > 0) {
                var resId = vm.restaurants[0].Id;
                GetTables(resId, 0, 0);
            }
        }

        function GetRestaurants() {
            $.ajax({
                type: "get",
                url: "/Res/Api/GetRestaurants",
                data: "",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                beforeSend: function (xhr) {
                    layindex = layer.open({
                        type: 3,
                        shadeClose: false, //点击遮罩关闭层
                    });
                },
                success: function (data, textStatus) {
                    if (data.Data != null) {
                        $(data.Data).each(function (i, o) {
                            vm.restaurants.push(o);
                        })
                        vm.RestaurantSel = vm.restaurants[0].Id;
                    } else {
                        layer.msg('请先添加餐厅等基础信息', {
                            time: 2000
                        });
                    }
                },
                complete: function (XMLHttpRequest, textStatus) {
                    layer.close(layindex);
                }
            })
        }

        function GetAreas() {
            $.ajax({
                type: "get",
                url: "/Res/Api/GetAreas",
                data: "",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                beforeSend: function (xhr) {
                    layindex = layer.open({
                        type: 3,
                        shadeClose: false, //点击遮罩关闭层
                    });
                },
                success: function (data, textStatus) {
                    if (data.Data != null) {
                        $(data.Data).each(function (i, o) {
                            if (o.Cyct == vm.RestaurantSel) {
                                o.isShow = true;
                            }
                            vm.areas.push(o);
                        })
                    } else {

                    }
                },
                complete: function (XMLHttpRequest, textStatus) {
                    layer.close(layindex);
                }
            })
        }

        function GetTableStatus() {
            $.ajax({
                type: "get",
                url: "/Res/Api/GetTableStatus",
                data: "",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                beforeSend: function (xhr) {
                    layindex = layer.open({
                        type: 3,
                        shadeClose: false, //点击遮罩关闭层
                    });
                },
                success: function (data, textStatus) {
                    if (data.Data != null) {
                        $(data.Data).each(function (i, o) {
                            vm.status.push(o);
                        })
                    }
                },
                complete: function (XMLHttpRequest, textStatus) {
                    layer.close(layindex);
                }
            })
        }

        function GetTables(restaurantId, areaId, status) {
            var para = { RestaurantId: restaurantId, AreaId: areaId, CythStatus: status };
            vm.tables = [];
            $.ajax({
                type: "get",
                url: "/Res/Api/GetTables",
                data: para,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                beforeSend: function (xhr) {
                    layindex = layer.open({
                        type: 3,
                        shadeClose: false, //点击遮罩关闭层
                    });
                },
                success: function (data, textStatus) {
                    if (data.Data != null) {
                        $(data.Data).each(function (i, o) {
                            vm.tables.push(o);
                        })
                    }

                },
                complete: function (XMLHttpRequest, textStatus) {
                    layer.close(layindex);
                }
            })
        }

        function SetTableEmpty() {
            layer.confirm("确认空置吗？", { title: '更新餐台状态提示', btn: ['继续提交', '取消'] }, function () {

                var para = { tableId: vm.GetTableClickId };
                $.ajax({
                    type: "post",
                    url: "/Res/Home/SetTableEmpty",
                    dataType: "json",
                    data: JSON.stringify(para),
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    beforeSend: function (xhr) {
                        layindex = layer.open({
                            type: 3,
                            shadeClose: false, //点击遮罩关闭层
                        });
                    },
                    success: function (data, textStatus) {
                        var res = data.Data;
                        if (res == true) {
                            $.connection.hub.start().done(function () {
                                chat.server.notifyResServiceRefersh(true);
                            });
                            layer.alert("更新成功");
                        } else {
                            layer.alert(data["Message"]);
                        }
                    },
                    complete: function (XMLHttpRequest, textStatus) {
                        layer.close(layindex);
                    }
                });
            });
        }

        function CancelOrder() {
            if (vm.OrderId == 0)
            {
                layer.alert("请先选择操作的订单！");
                return;
            }
            layer.confirm("继续取消吗？", { title: '取消订单提示', btn: ['确认取消', '返回'] }, function () {

                var para = { orderId: vm.OrderId };
                $.ajax({
                    type: "post",
                    url: "/Res/Home/CancelOrder",
                    dataType: "json",
                    data: JSON.stringify(para),
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    beforeSend: function (xhr) {
                        layindex = layer.open({
                            type: 3,
                            shadeClose: false, //点击遮罩关闭层
                        });
                    },
                    success: function (data, textStatus) {
                        var res = data.Data;
                        if (res == true) {
                            $.connection.hub.start().done(function () {
                                chat.server.notifyResServiceRefersh(true);
                            });
                            layer.alert("取消订单成功");
                            layer.closeAll();
                        } else {
                            layer.alert(data["Message"]);
                        }
                    },
                    complete: function (XMLHttpRequest, textStatus) {
                        layer.close(layindex);
                    }
                });
            });
        }

        function Search() {
            GetTables(vm.RestaurantSel, vm.AreaSel, vm.StatusSel);
        }
        //Refresh
        function Refresh() {
            GetTables(vm.RestaurantSel, vm.AreaSel, vm.AreaSel.StatusSel);
        }


        //}

        editor.init = function () {
            //bindAction();
            Init();
        }

        editor.Refresh = function () {
            Refresh();
        }

        return editor;
    }
}