/**
 * 实时房态事件处理
 */

//设置房态大小当前设置值
function set_romsize() {

    if ($.cookie('room_size') == 'sm') {
        var w = 130;
        $("#room-size_sm").attr("checked", true);
    } else if ($.cookie('room_size') == 'lg') {
        var w = 160;
        $("#room-size_lg").attr("checked", true);
    } else if ($.cookie('room_size') == 'xl') {
        var w = 200;
        $("#room-size_xl").attr("checked", true);
    }
}


layui.use('laytpl', function(){
  var laytpl = layui.laytpl;

  $.ajax({
        url: "/H/RoomManage/RoomList",
        data: {},
        type: "post",
        dataType: "json",
        success: function (data) {
            console.log(data);

            //渲染房态图
            var GetRoomListTpl = RoomListTpl.innerHTML;
            laytpl(GetRoomListTpl).render(data.RoomList, function(html){
               RoomList_View.innerHTML = html;
            });
            
            //渲染房态数据
            var getTpl = RoomListDataTpl.innerHTML;
            laytpl(getTpl).render(data.RoomStatistic, function(html){
               RoomListData_View.innerHTML = html;
            });

            //筛选楼层
            var GetGalleryListTpl = GalleryListTpl.innerHTML;
            laytpl(GetGalleryListTpl).render(data.GalleryList, function(html){
               GalleryList_View.innerHTML = html;
            });


            //筛选房型
            var GetRoomTypeListTpl = RoomTypeListTpl.innerHTML;
            laytpl(GetRoomTypeListTpl).render(data.RoomTypeList, function(html){
               RoomTypeList_View.innerHTML = html;
            });

            //筛选房态
            var GetRoomStateListTpl = RoomStateListTpl.innerHTML;
            laytpl(GetRoomStateListTpl).render(data.RoomStateList, function(html){
               RoomStateList_View.innerHTML = html;
            });


            tabs_n();
            room_state_hover();
            room_click();
            $('.loader-box').remove();
            room_state_set($.cookie('room_size'));
        },
        error: function (msg) {
            console.log(msg.responseText);
        }
  })
}); 



//设置房态单行显示数量
function room_state_set(size) {
    var sizez = size;
    if (sizez) {
        if (sizez == 'sm') {
            var w = 130;
            $("#room-size_sm").attr("checked", true);
        } else if (sizez == 'lg') {
            var w = 160;
            $("#room-size_lg").attr("checked", true);
        } else if (sizez == 'xl') {
            var w = 200;
            $("#room-size_xl").attr("checked", true);
        }
        $('.room-state-list').removeClass('room_sm').removeClass('room_lg').removeClass('room_xl').addClass('room_' + sizez);
    } else {
        if ($('.room-state-list').hasClass('room_lg')) {
            var w = 160;
        } else if ($('.room-state-list').hasClass('room_xl')) {
            var w = 200;
        } else {
            $('.room-state-list').addClass('room_sm');
            var w = 130;
        }
    }


    var width = $(window).width();
    var state_width = Math.floor(width / w);
    var state_b = 100 / state_width;
    var bl = decimal(state_b, 2) - 0.55;
    $('.room-state-list li').css('width', '' + bl + '%');
}



//监听房态房间hover
function room_state_hover() {

    $(".room-state-list li").hover(function (ev) {
        $(this).addClass('hover');
        var data_number = $(this).attr('data-number');
        if (data_number) {
            $('.room-state-list li[data-number="' + data_number + '"]').addClass('hover');
        }
    },
    function () {
        $(this).removeClass('hover');
        var data_number = $(this).attr('data-number');
        if (data_number) {
            $('.room-state-list li[data-number="' + data_number + '"]').removeClass('hover');
        }
    });
}



//取小数点后两位
function decimal(num, v) {
    var vv = Math.pow(10, v);
    return Math.round(num * vv) / vv;
}

//清除点击关闭热点事件
function dropdown_click() {
    $(".dropdown-menu").on("click", "[data-stopPropagation]", function (e) {
        e.stopPropagation();
    });
}



//实时房态搜索事件
function room_search() {
    var keyword = $('input[name="keyword"]').val();
    $(".room-state-list li").each(function () {
        if ($(this).find('.room-state_title').find('.room-id').html().indexOf(keyword) != -1) {
            $(this).removeClass('hide').addClass('show');
        } else {
            $(this).removeClass('show').addClass('hide');
        }
    })
}



//实时房态条件筛选-按楼层
function room_screen_check() {
    $(".room-screen_list .screen_btn").click(function () {
        if ($(this).hasClass('active')) {
            $(this).removeClass('active');
        } else {
            $(this).addClass('active');
        }
        $(".room-state-list li").removeClass('show').addClass('hide');

        if ($('.screen_btn.active').length < 1) {
            $(".room-state-list li").removeClass('hide').removeClass('hide1').removeClass('hide2').removeClass('show1').removeClass('show2').addClass('show');
            return false;
        }
        var keytypenum = $('.tabs-n_centent .tabs .tabs-n_centent').length;

        for (var o = 0; o < keytypenum; o++) {
            var div = $('.tabs-n_centent .tabs .tabs-n_centent').eq(o).find('.screen_btn.active');
            var keywordnum = div.length;
            if (keywordnum > 0) {
                for (var i = 0; i < keywordnum; i++) {
                    var keyword = div.eq(i).attr('data-key');
                    if (o == 1) {
                        if (i == 0) { $(".room-state-list .show").addClass('hide1').removeClass('show1'); }
                        $(".room-state-list .show").each(function () {
                            if ($(this).attr('data-key').indexOf(keyword) != -1) {
                                $(this).removeClass('hide1').addClass('show1');
                            } else {

                            }
                        })
                    } else if (o >= 2) {
                        if (i == 0) { $(".room-state-list .show1").addClass('hide2').removeClass('show2').removeClass('show'); }
                        $(".room-state-list .show1").each(function () {
                            if ($(this).attr('data-key').indexOf(keyword) != -1) {
                                $(this).removeClass('hide2').addClass('show2');
                            } else {

                            }
                        })
                    } else {
                        $(".room-state-list li").each(function () {
                            $(".room-state-list li").each(function () {
                                var key = $(this).attr('data-key');
                                if ($(this).attr('data-key').indexOf(keyword) != -1) {
                                    $(this).removeClass('hide').addClass('show');
                                }
                            })
                        })
                    }
                }
            } else {
                //alert(o);
                if (o == 0) {
                    $(".room-state-list li").removeClass('hide').removeClass('show1').removeClass('show2').addClass('show');
                } else if (o == 1) {
                    if ($(".room-state-list .show").length < 1) {
                        $(".room-state-list li").removeClass('hide').addClass('show1');
                    } else {
                        //$(".room-state-list .show").removeClass('hide1').removeClass('hide').addClass('show1');
                    }

                } else if (o == 2) {

                    $(".room-state-list .show1").removeClass('hide2').removeClass('hide1').removeClass('hide').addClass('show');
                }
            }
        }
    })
}


//房间点击事件
function room_click() {

    $(".room-state-list .room-card").dblclick(function () {
        //alert(1);
        $(this).attr('id', 'room_click');
        var roomdiv = $(this).parent('li'),
            roomid = roomdiv.attr('data-id'),
            room_state = roomdiv.attr('data-state'),
            title = $(this).attr('data-title');


        parent.room_set(roomid, title, room_state);
    });

    //阻止浏览器默认右键点击事件
    $(".centent").bind("contextmenu", function () {
        return false;
    })

    $(".room-state-list .room").mousedown(function (e) {
        if (3 == e.which) {
            var winwidth = $(window).width();
            var ofr = winwidth - $(this).offset().left;
            var roomid = $(this).attr('data-id');
            console.log(roomid);

            var number = $(this).attr('data-id');
            var data = '<div class="room-click_body" >\
              <div class="media">\
                <div class="media-left">\
                    <img class="media-object" src="../images/user.jpg" >\
                </div>\
                <div class="media-body">\
                  <p class="media-heading">姓名: <span class="username">黄岗</span>性别: <span  class="sex">男</span></p>\
                  <p>手机号: <span>12345678910</span></p>\
                </div>\
              </div>\
              <div class=" no-radius text-left fz-sm">\
                  <div class="list-group-item">\
                    <span class="pull-right badge bg-info">45,000</span>\
                    总消费\
                  </div>\
                  <div class="list-group-item">\
                    <span class="pull-right badge bg-success">23,200</span>\
                    总付款\
                  </div>\
                  <div class="list-group-item">\
                    <span class="pull-right badge bg-danger">21,000</span>\
                    总余额\
                  </div>\
              </div>\
              <div class="btnlist-group ">\
              <a class="btn btn-sm btn-default btn-rounded " id="room-set_btn"  href="javascript:void(0);" data-id="'+ roomid + '" data-title="' + number + '房间详情">房间详情</a>\
              <a class="btn btn-sm btn-default btn-rounded" href="javascript:void(0);" onclick="zzz();">入账</a>\
              <a class="btn btn-sm btn-default btn-rounded">离店</a>\
              </div>\
              </div>';

            if (ofr < 370) {
                var arrow = '<span class="arrow right pull-up"></span>';
                var src = '<div class="room-click_data pop_l pop_l_animation"></div>';
            } else {
                var arrow = '<span class="arrow left pull-up"></span>';
                var src = '<div class="room-click_data pop_r pop_r_animation"></div>';
            }
            if ($(this).find('.room-click_data').length < 1) {
                $(this).append(src);
                setTimeout(function () {
                    $('.room-click_data').removeClass('pop_r_animation').removeClass('pop_l_animation').append(data + arrow);
                    $("#room-set_btn").click(function () {
                        var title = $(this).attr('data-title');
                        var roomid = $(this).attr('data-id');
                        parent.room_set(roomid, title);
                    })

                }, 200);
                $(this).siblings("li").find('.room-click_data').remove();
            }
        }
    });
    $(document).click(function () {
        $('.room-click_data.pop_l').addClass('room-click_data_overl').find('.arrow').remove();
        $('.room-click_data.pop_r').addClass('room-click_data_over').find('.arrow').remove();
        setTimeout(function () {
            $('.room-click_data').remove();
        }, 150);

    });
}




//左侧tabs导航展开收起事件
function nav_tabs_left() {
    $('.nav-tabs_left ul .nav-tabs_btn .auto').click(function () {
        var div = $(this).parent(".nav-tabs_btn");
        if (div.hasClass('active')) {
            div.find('.nav-fnav').css('height', '0px');

            setTimeout(function () { div.removeClass('active'); }, 300);
        } else {
            div.find('.nav-fnav').height(div.find('.nav-fnav').find('li').length * 35);
            setTimeout(function () { div.addClass('active'); }, 300);
        }
    })
}


//修改主结房激活
function set_mainroom_show() {
    if ($('#Relation').hasClass('set-mainroom')) {
        $('#Relation').removeClass('set-mainroom');
        $('#Relation .Relation ul li').unbind("click");
    } else {
        $('#Relation').addClass('set-mainroom');
        $('.set-mainroom .Relation ul li').click(function () {
            $(this).addClass('active').siblings("li").removeClass('active');
            $('input[name="all_mainroom_check"]:checkbox').each(function () { this.checked = false; });
        })

        $('input[name="all_mainroom_check"]:checkbox').click(function () {
            if ($(this).is(':checked')) {
                $('.set-mainroom .Relation ul li').removeClass('active');
            } else {
            }
        });
    }
}


//确认设置主结房
function set_mainroom() {
    if ($('.set-mainroom .Relation ul li.active').length > 0) {//存在主结房
        $('.set-mainroom .Relation ul li.active').addClass('main-room').removeClass('active').removeClass('next-room').find('h5').find('.room-title').text('主结房').parent('h5').parent('li').siblings('li').addClass('next-room').removeClass('main-room').find('h5').find('.room-title').text('从房');
        $('#Relation').removeClass('set-mainroom');
        $('#Relation .Relation ul li').unbind("click");
    } else {//各自结账
        if ($('input[name="all_mainroom_check"]:checkbox').is(':checked')) {
            $('.set-mainroom .Relation ul li').addClass('main-room').removeClass('next-room').find('h5').find('.room-title').text('主结房');
            $('#Relation').removeClass('set-mainroom');
            $('#Relation .Relation ul li').unbind("click");
        } else {
            layer.msg('请选择主结房');
        }
    }
}


//房间详情，添加房间(联房)事件
function add_relation_room() {
    $('.Relation ul li .Relation-hove .add_Relation_room').click(function () {

        $.get("../tpls/room_selector.html", function (data) {

            layer.open({
                type: 1,
                skin: '', //加上边框
                area: ['800px', '600px'], //宽高
                content: data
            });

            room_selector_roomcheck();
            pop_close();

        });
    })
}


//房间选择器房间选择事件
function room_selector_roomcheck() {
    $('.room-selector_list li').click(function () {
        if ($(this).hasClass('active')) {
            $(this).removeClass('active');
        } else {
            $(this).addClass('active');
        }

        var activenum = $('.room-selector_list li.active').length;
        if (activenum > 0) {
            $('.room-selector .footer-btn_group .btn-success').show();
        } else {
            $('.room-selector .footer-btn_group .btn-success').hide();
        }
        $('#room_selnum').text(activenum);

    })
}


//取消房间选择器全部选中
function all_roomsel_cancel() {
    $('.room-selector_list li').removeClass('active');
    $('#room_selnum').text('0');
    $('.room-selector .footer-btn_group .btn-success').hide();
}


//tabs-n 列表试嵌套tabs
function tabs_n() {
    $('.tabs-n_title').click(function () {
        var div = $(this).next('.tabs-n_centent');
        if (div.hasClass('show')) {
            $(this).removeClass('active');
            div.removeClass('show').addClass('hide');
        } else {
            $(this).addClass('active');
            div.removeClass('hide').addClass('show');
        }
    })
}

//房态数据收起隐藏
function room_state_data() {
    $('#room_state_data_btn').click(function () {
        if ($(this).hasClass('active')) {
            $(this).removeClass('active');
            $('.room-state_data').removeClass('open');
        } else {
            $(this).addClass('active');
            $('.room-state_data').addClass('open');
            $(".timer").each(count);
        }
    })
}
