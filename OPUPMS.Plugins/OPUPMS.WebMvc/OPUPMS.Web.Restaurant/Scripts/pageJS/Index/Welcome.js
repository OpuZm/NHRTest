var pageJs;
$(function () {
    pageJs = editJs.Create();
    pageJs.init();
})

function Refresh() {
    if (pageJs) {
        pageJs.Refersh();
    }
}

var editJs = {
    Create: function () {
        var editor = {};
        var layindex;
        var width = $(window).width();
        var height = $(window).height();

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
                OrderTableId: 0,
                RestaurantId: 0,
                TableControlOrder: [],
                Control: ""
            },
            updated: function () {
                $('#ulList li').contextmenu({
                    target: '#contextmenu',
                    onItem: function (context, e) {
                        var url = "";
                        var para = "";
                        var text = $(e.target).text();
                        switch (text) {
                            case "预定":
                                url = "/Res/Home/Reserve?tableId=" + vm.TabClickId;
                                break;
                            case "开台":
                                url = "/Res/Home/OpenTable?tableId=" + vm.TabClickId;
                                break;
                            case "点餐":
                                if (vm.TableControlOrder.length == 1) {
                                    url = "/Res/Home/ChoseProject?orderTableId=" + vm.OrderTableId;
                                }
                                break;
                            case "换桌":
                                if (vm.TableControlOrder.length == 1) {
                                    para = "RestaurantId=" + vm.RestaurantId + "&TableId=" + vm.TabClickId + "&OrderTableId=" + vm.OrderTableId;
                                    url = "/Res/Home/ChangeTable?" + para;
                                }
                                break;
                            case "预定纪录":
                                url = "/Res/Home/ReserveHistory?tableId=" + vm.TabClickId;
                                break;
                            case "并台":
                                break;
                            case "拆台":
                                break;
                            case "取消订单":
                                break;
                            default:
                                break;
                        }
                        vm.Control = text;
                        if (text != "预定纪录") {
                            if (vm.TableControlOrder.length <= 1) {
                                DoControl(text, url);
                            } else {
                                vm.Control = text;
                                ChoseControlOrder();
                            }
                        }
                        else {
                            DoControl(text, url);
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
                }
            },
            methods: {
                Add: function (index) {

                },
                Delete: function (index) {

                },
                ChoseOrder: function (obj) {
                    this.OrderTableId = obj.Id;
                    //layer.closeAll();
                    DoControlOrder();
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
                        console.log(obj.OrderNow[0].Id);
                        if (obj.OrderNow.length == 1) {
                            vm.OrderTableId = obj.OrderNow[0].Id;
                        }
                    }

                    //$(event.target).contextmenu({
                    //    target: '#contextmenu',
                    //    onItem: function (context, e) {
                    //        var url = "";
                    //        var para = "";
                    //        var text = $(e.target).text();
                    //        switch (text) {
                    //            case "预定":
                    //                url = "/Home/Reserve?tableId=" + vm.TabClickId;
                    //                break;
                    //            case "开台":
                    //                url = "/Home/OpenTable?tableId=" + vm.TabClickId;
                    //                break;
                    //            case "点餐":
                    //                if (vm.TableControlOrder.length == 1) {
                    //                    url = "/Home/ChoseProject?orderTableId=" + vm.OrderTableId;
                    //                }
                    //                break;
                    //            case "换桌":
                    //                if (vm.TableControlOrder.length == 1) {
                    //                    para = "RestaurantId=" + vm.RestaurantId + "&TableId=" + vm.TabClickId + "&OrderTableId=" + vm.OrderTableId;
                    //                    url = "/Home/ChangeTable?" + para;
                    //                }
                    //                break;
                    //            case "预定纪录":
                    //                break;
                    //            case "并台":
                    //                break;
                    //            case "拆台":
                    //                break;
                    //            case "取消订单":
                    //                break;
                    //            default:
                    //                break;
                    //        }
                    //        vm.Control = text;
                    //        if (vm.TableControlOrder.length == 1) {
                    //            DoControl(text, url);
                    //        } else {
                    //            vm.Control = text;
                    //            ChoseControlOrder();
                    //        }
                    //    }
                    //})

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

        function DoControlOrder() {
            var text = vm.Control;
            var url = "";
            var para = "";
            if (text != "") {
                switch (text) {
                    case "点餐":
                        url = "/Res/Home/ChoseProject?orderTableId=" + vm.OrderTableId;
                        break;
                    case "换桌":
                        para = "RestaurantId=" + vm.RestaurantId + "&TableId=" + vm.TabClickId + "&OrderTableId=" + vm.OrderTableId;
                        url = "/Res/Home/ChangeTable?" + para;
                        break;
                }

                layer.open({
                    type: 2,
                    title: text,
                    area: [width + "px", height + "px"],
                    content: url,
                    maxmin: false
                })
            }
        }

        function DoControl(title, url) {
            layer.open({
                type: 2,
                title: title,
                area: [width + "px", height + "px"],
                content: url,
                maxmin: false
            })
        }

        function ChoseControlOrder() {
            layer.open({
                type: 1,
                title: "选择操作订单",
                area: ["300px", "300px"],
                content: $("#TableOrderChose"),
                maxmin: false
            })
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

        function Search() {
            GetTables(vm.RestaurantSel, vm.AreaSel, vm.StatusSel);
        }

        function Refersh() {
            GetTables(vm.RestaurantSel, vm.AreaSel, vm.AreaSel.StatusSel);
        }


        //}

        editor.init = function () {
            //bindAction();
            Init();
        }

        editor.Refersh = function () {
            Refersh();
        }

        return editor;
    }
}