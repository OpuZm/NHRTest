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


//加载房态图（json）
function RoomList_data() {

    $.ajax({
        url: "/RoomManage/RoomList",
        data: {},
        type: "post",
        dataType: "json",
        success: function (data) {
            console.log(data);
            //return false;
            var $jsontip = $(".room-state-list");
            var strHtml = "<ul>";//存储数据的变量  
            var room_name = '';
            $jsontip.empty();//清空内容  
            for (var i = 0; i < data.length; i++) {//遍历房间

                room_type = '空净房';
                // var room_style='';
                // var room_icon='';

                // if (info["RoomType"]=='0') {//空净房
                //     room_style='empty_clean';
                //     room_name='空净房';
                // }else if (info["RoomType"]=='1') {//占净房
                //     room_style='occupy_clean';
                //     room_icon='icon-user_b';
                //     room_name=info["UserName"];
                // }else if (info["RoomType"]=='2') {//空脏房
                //     room_style='empty_dirty';
                //     room_name='空脏房';
                // }else if (info["RoomType"]=='3') {//占脏房
                //     room_style='occupy_dirty';
                //     room_name='占脏房';
                // }else if (info["RoomType"]=='4') {//维修房
                //     room_style='repair';
                //     room_icon='icon-tool';
                //     room_name='维修房';
                // }else if (info["RoomType"]=='5') {//预抵房
                //     room_style='arrivals';
                //     room_name='预抵房';
                // }
                // var room_state=room_name;
                // if (info["Master"]==1) {//主结房
                //     room_name='<span class="red">主结房</span>';
                //     room_state='占净房';
                // }
                strHtml += '<li class="empty_clean room" data-id="' + data[i]["code"] + '" data-state="' + data[i]["state"] + '" data-number="">'
                        + '<div class="room-card" data-title="' + data[i]["code"] + '房间详情">'
                        + '<h5 class="room-state_title"><span class="room-id">' + data[i]["code"] + '</span><span class="room-title">' + data[i]["title"] + '</span></h5>'
                        + '<i class="icon-user_b"></i>'
                        + '<p class="room-state_name">'
                        + '<span class="room-username">' + data[i]["title"] + '</span>'
                        + '<span class="room-type">' + room_type + '</span>'
                        + '</p>'
                        + '<div class="room-state_icon">'
                        + '<span class="icon-time_room"></span>'
                        + '<span class="icon-clean"></span>'
                        + '<span class="icon-Arrow_skew_rt"></span>'
                        + '<span class="icon-msg"></span>'
                        + '<span class="icon-vip"></span>'
                        + '</div>'
                        + '</div>'
                        + '</li>';
            }
            strHtml += '</ul>';
            $jsontip.html(strHtml);



            room_state_hover();
            room_click();
            room_state_set($.cookie('room_size'));



        }
    })
}


//设置房态单行显示数量
function room_state_set(size) {
    var sizez = size;
    if (sizez) {
        //$.cookie('room_size', sizez, { expires: 30 ,path:"/"});
        //console.log(sizez);
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
   
   $(".room-state-list li").hover(function(ev) {
      $(this).addClass('hover');
      var data_number=$(this).attr('data-number');
      if (data_number) {
        $('.room-state-list li[data-number="'+data_number+'"]').addClass('hover');
      }
   },
   function() {
      $(this).removeClass('hover');
      var data_number=$(this).attr('data-number');
      if (data_number) {
        $('.room-state-list li[data-number="'+data_number+'"]').removeClass('hover');
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
    $(".dropdown-menu").on("click", "[data-stopPropagation]", function(e) {
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

//房间操作弹窗引入数据
// function room_set(roomid,title) {
//     var newno=parseFloat($('.layui-tab-title li').length)+1;
//     var roomurl='tpls/room.html?roomid='+roomid;
//     var iframeheight=$('.layui-tab-content').height();
    

//     if ($('#roomid_'+roomid+'').length<1) {//打开新窗口
//       var nav_btn='<li class="layui-this">'+
//                      '<i class="layui-icon"></i>'+
//                      '<cite>'+roomid+'房间</cite>'+
//                      '<i class="layui-icon layui-unselect layui-tab-close" data-id="'+newno+'">ဆ</i>'+
//                   '</li>';
//       var content = '<div class="layui-tab-item layui-show">'+
//                       '<iframe src="' + roomurl + '" id="roomid_'+roomid+'" style="height:'+iframeheight+'px" data-id="' + newno + '"></iframe>'+
//                     '</div>';
//       $('.layui-tab-title li').removeClass('layui-this');
//       $('.layui-tab-content .layui-tab-item').removeClass('layui-show');
//       $('.layui-tab-title').append(nav_btn);
//       $('.layui-tab-content').append(content);
//       $.cookie('tab_nav', nav_btn, { path:'/' });
//       $.cookie('tab_content', content, { path:'/' });
//       $('i.layui-tab-close[data-id=' + newno + ']').on('click', function() {
//            $(this).parent('li').remove();
//            $('#roomid_'+roomid+'').parent('.layui-tab-item').remove();
//            if ($('.layui-tab-title li.layui-this').length<1) {
//               $('.layui-tab-title li:last').addClass('layui-this');
//               $('.layui-tab-content .layui-tab-item:last').addClass('layui-show');
//            } 
//       });
//     }else{//跳转窗口
//       var navno=$('#roomid_'+roomid+'').attr('data-id');
//       $('.layui-tab-title li i.layui-tab-close[data-id=' + navno + ']').parent('li').addClass('layui-this').siblings("li").removeClass('layui-this');
//       $('#roomid_'+roomid+'').parent('.layui-tab-item').addClass('layui-show').siblings("li").removeClass('layui-show');

//     }

// }




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
                //alert(2);
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

