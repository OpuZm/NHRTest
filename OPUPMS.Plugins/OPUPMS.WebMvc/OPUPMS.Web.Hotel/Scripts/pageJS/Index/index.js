(function (doc, win) {
    var docEl = doc.documentElement,
      resizeEvt = 'orientationchange' in window ? 'orientationchange' : 'resize',
      recalc = function () {
          var clientWidth = docEl.clientWidth;
          if (!clientWidth) return;
          if (clientWidth < 1300) {
              $('#admin-body').animate({
                  left: '50px'
              });
              $('#admin-footer').animate({
                  left: '50px'
              });
              $('#admin-side').animate({
                  width: '50px'
              }).addClass('stop');
          } else {
              $('#admin-body').animate({
                  left: '200px'
              });
              $('#admin-footer').animate({
                  left: '200px'
              });
              $('#admin-side').animate({
                  width: '200px'
              }).removeClass('stop');
          }
      };

    if (!doc.addEventListener) return;
    win.addEventListener(resizeEvt, recalc, false);
    doc.addEventListener('DOMContentLoaded', recalc, false);
})(document, window);



//初始化layui
layui.config({
    base: '/Plugins/Web/OPUPMS.Web.Hotel/Scripts/pageJS/'
}).use(['element', 'layer', 'navbar', 'tab', 'form'], function () {
    var element = layui.element(),
        $ = layui.jquery,
        form = layui.form(),
        layer = layui.layer,
        navbar = layui.navbar(),
        tab = layui.tab({
            elem: '.admin-nav-card' //设置选项卡容器
        });
    
    //iframe自适应
    $(window).on('resize', function () {
        var $content = $('.admin-nav-card .layui-tab-content');
        $content.height($(this).height() - 125);
        $content.find('iframe').each(function () {
            $(this).height($content.height());
        });
    }).resize();
    
    $.ajax({
        url: "/H/CloudPMS/GetClientMenuJson",
        data: {},
        type: "get",
        dataType: "json",
        success: function (data) {
            navs = data;// $.parseJSON(data);
            navbar.set({
                spreadOne: true,
                elem: '#admin-navbar-side',
                cached: true,
                data: navs,
                cached: true,
                url: ''
            });

            //渲染navbar
            navbar.render();
            //监听点击事件
            navbar.on('click(side)', function (data) {
                tab.tabAdd(data.field);
            });

        }
    });

    $('.loader-box').remove();

    //监听tab点击
    element.on('tab(admin-tab)', function () {

        var title = $('.layui-tab-title li.layui-this').find('cite').text();
        console.log(title);
        if (title != '首页') {
            var tab_nav = '<li class="layui-this">' + $('.layui-tab-title li.layui-this').html() + '</li>';
            var tab_content = '<div class="layui-tab-item layui-show">' + $('#' + $('.layui-tab-title li.layui-this').find('.layui-tab-close').attr('data-fid') + '').parent('.layui-show').html() + '</div>';
            $.cookie('tab_nav', tab_nav, { path: '/' });
            $.cookie('tab_content', tab_content, { path: '/' });
        } else {
            if ($('#page_home #main').length < 1) {
                $('#page_home').html('<iframe id="main" style="height:800px;" src="/H/CloudPMS/Home"></iframe>');
            }
            $.cookie('tab_nav', 0, { path: '/' });
            $.cookie('tab_content', 0, { path: '/' });
        }

        var pageid = $('.layui-tab-title li.layui-this').find('.layui-tab-close').attr('data-navid');
        if (pageid) {
            $('.beg-navbar li').removeClass('layui-nav-itemed').find('dl').find('dd').removeClass('layui-this');
            var navdiv = $('#main_nav_' + pageid + '').parent('dd').addClass('layui-this').parent('dl').parent('li').addClass('layui-nav-itemed');
        }
    });

    //设置导航默认收起
    form.on('switch(main-nav_set)', function (data) {
        if (data.elem.checked == true) {
            $('#admin-body').animate({
                left: '50px'
            });
            $('#admin-footer').animate({
                left: '50px'
            });
            $('#admin-side').animate({
                width: '50px'
            }).addClass('stop');
            $.cookie('nav_open', 1, { expires: 30 });
        } else {
            $('#admin-body').animate({
                left: '200px'
            });
            $('#admin-footer').animate({
                left: '200px'
            });
            $('#admin-side').animate({
                width: '200px'
            }).removeClass('stop');
            $.cookie('nav_open', 0);
        }
    });

    //设置风格
    $('.backpic_list li a').click(function () {
        var type = $(this).attr('data-type');
        if (type == 0 || !type) {
            type = 0;
            $('body').removeClass('skin-diy').attr('id', '');
        } else {
            $('body').addClass('skin-diy').attr('id', 'back' + type + '').height($(window).height());
        }
        $(this).parent("li").addClass('active').siblings("li").removeClass('active');
        $.cookie('skin_back', type, { expires: 30 });
    });

    //导航收起展开点击
    $('.admin-side-toggle').on('click', function () {
        var sideWidth = $('#admin-side').width();
        if (sideWidth === 200) {
            $('#admin-body').animate({
                left: '50px'
            }); //admin-footer
            $('#admin-footer').animate({
                left: '50px'
            });
            $('#admin-side').animate({
                width: '50px'
            }).addClass('stop');
        } else {
            $('#admin-body').animate({
                left: '200px'
            });
            $('#admin-footer').animate({
                left: '200px'
            });
            $('#admin-side').animate({
                width: '200px'
            }).removeClass('stop');
        }
    });

    //手机设备的简单适配
    var treeMobile = $('.site-tree-mobile'),
        shadeMobile = $('.site-mobile-shade');
    treeMobile.on('click', function () {
        $('body').addClass('site-mobile');
    });
    shadeMobile.on('click', function () {
        $('body').removeClass('site-mobile');
    });

    room_size();
    default_style();
});



//界面布局初始化
function default_style() {

    //初始化导航默认展开收起
    var nav_open = $.cookie('nav_open');
    if (nav_open == 1) {
        $('#admin-body').css('left', '50px');
        $('#admin-footer').css('left', '50px');
        $('#admin-side').width('50px').addClass('stop');
        $('#main-nav_set').each(function () { this.checked = true; });
    } else {
        $('#admin-body').css('left', '200px');
        $('#admin-footer').css('left', '200px');
        $('#admin-side').width('200px').removeClass('stop');
        $('#main-nav_set').each(function () { this.checked = false; });
    }


    //初始化房态图大小
    var roomsize = $.cookie('room_size');
    if (roomsize) {
        $('#room-size_' + roomsize + '').attr('checked', 'checked').each(function () { this.checked = true; });
    }

    //初始化风格
    var skin_back = $.cookie('skin_back');
    if (!skin_back) {
        skin_back = 0;
    }
    if (skin_back == 0) {
        $('body').removeClass('skin-diy').attr('id', '');
    } else {
        var type = skin_back;
        $('body').addClass('skin-diy').attr('id', 'back' + type + '').height($(window).height());
        $(this).parent("li").addClass('active').siblings("li").removeClass('active');
        $.cookie('skin_back', type, { expires: 30 });
    }
    $('#backbg_' + skin_back + '').addClass('active');

    //初始化操作页面
    var tab_nav = $.cookie('tab_nav');
    var tab_content = $.cookie('tab_content');
    if (tab_nav !== null && tab_content !== null && tab_nav != 0 && tab_content !== 0) {
        var page_home = '<li>'
                 + '<i class="glyphicon glyphicon-home" ></i>'
                 + '<cite>首页</cite>'
                 + '</li>';
        $('.layui-tab-title li').removeClass('layui-this');
        $('.layui-tab-content .layui-tab-item').removeClass('layui-show');
        $('.layui-tab-title').append(page_home + tab_nav);
        $('.layui-tab-content').append(tab_content);

        var navid = $('.layui-tab-title li.layui-this').find('.layui-tab-close').attr('data-fid');
        $('.layui-tab-title li.layui-this').find('.layui-tab-close').attr('data-id', '1');
        $('#' + navid + '').attr('data-id', '1').height($('.layui-tab-content').height());
        $('i.layui-tab-close[data-id=1]').on('click', function () {
            $(this).parent('li').remove();
            $('#' + navid + '').parent('.layui-tab-item').remove();
            if ($('.layui-tab-title li.layui-this').length < 1) {
                $('.layui-tab-title li:last').addClass('layui-this');
                $('.layui-tab-content .layui-tab-item:last').addClass('layui-show');
                if ($('.layui-tab-content .layui-tab-item:last iframe').length<1) {
                    $('.layui-tab-content .layui-tab-item:last').html('<iframe id="main" style="height:800px;" src="/H/CloudPMS/Home"></iframe>');
                }
            }
            if ($('.layui-tab-title li').length <= 1) {
                $.cookie('tab_nav', 0, { path: '/' });
                $.cookie('tab_content', 0, { path: '/' });
            }
        });
    } else {
        var page_home = '<li class="layui-this">'
                  + '<i class="glyphicon glyphicon-home" ></i>'
                  + '<cite>首页</cite>'
                  + '</li>';
        $('.layui-tab-title').html(page_home);
        $('#page_home').html('<iframe id="main" style="height:800px;" src="/H/CloudPMS/Home"></iframe>');
    }
}


//监听房态图大小/导航收起/展开 设置
function room_size() {
    $('input[name="room_size"]').click(function (e) {
        var size = $(this).val();
        $.cookie('room_size', size, { expires: 30, path: "/" });
        if ($("#page_110")[0]) {
            $("#page_110")[0].contentWindow.room_state_set(size);
        }
    });

    //风格切换
    $("#skin_default_set").change(function () {
        if ($(this).is(':checked')) {//极简风格
            $('body').removeClass('skin-diy').attr('id', '');
            $.cookie('skin_back', 0);
            $('.backpic_list li').removeClass('active');
        } else {
            $('.backpic_list li:first-child').addClass('active').siblings("li").removeClass('active');
            $.cookie('skin_back', 1, { expires: 30 });
        }
    });

}


//清除点击关闭热点事件
$(".dropdown-menu").on("click", "[data-stopPropagation]", function (e) {
    e.stopPropagation();
});



//自定义风格选择事件
function set_back() {
    $('.backpic_list li a').click(function () {
        var type = $(this).attr('data-type');
        $('body').addClass('skin-diy').attr('id', 'back' + type + '');
        $(this).parent("li").addClass('active').siblings("li").removeClass('active');
        $.cookie('skin_back', type, { expires: 30 });
        $("#skin_default_set").attr('checked', false);
    })
}




