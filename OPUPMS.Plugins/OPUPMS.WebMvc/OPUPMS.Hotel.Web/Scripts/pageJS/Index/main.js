/**
 * 处理接收iframe子页面 对 最外层index.html 容器的事件
 */

//房间操作弹窗引入数据
function room_set(roomid, title, opentype) {
    var newno = parseFloat($('.layui-tab-title li').length);

    var iframeheight = $('.layui-tab-content').height();

    if (opentype == 'V') {
        var tab_title = '开房';
        var roomurl = '/RoomManage/RoomInfo?roomid=' + roomid;
    } else {
        var tab_title = roomid + '房间';
        var roomurl = '/RoomManage/Details?roomid=' + roomid;
    }


    if ($('#roomid_' + roomid + '').length < 1) {//打开新窗口
        var nav_btn = '<li class="layui-this">' +
                       '<i class="layui-icon"></i>' +
                       '<cite>' + tab_title + '</cite>' +
                       '<i class="layui-icon layui-unselect layui-tab-close" data-id="' + newno + '"  data-fid="roomid_' + roomid + '">ဆ</i>' +
                    '</li>';
        var content = '<div class="layui-tab-item layui-show">' +
                        '<iframe src="' + roomurl + '" id="roomid_' + roomid + '" style="height:' + iframeheight + 'px" data-id="' + newno + '"></iframe>' +
                      '</div>';
        $('.layui-tab-title li').removeClass('layui-this');
        $('.layui-tab-content .layui-tab-item').removeClass('layui-show');
        $('.layui-tab-title').append(nav_btn);
        $('.layui-tab-content').append(content);
        $.cookie('tab_nav', nav_btn, { path: '/' });
        $.cookie('tab_content', content, { path: '/' });
        $('i.layui-tab-close[data-id=' + newno + ']').on('click', function () {
            $(this).parent('li').remove();
            $('#roomid_' + roomid + '').parent('.layui-tab-item').remove();
            if ($('.layui-tab-title li.layui-this').length < 1) {
                $('.layui-tab-title li:last').addClass('layui-this');
                $('.layui-tab-content .layui-tab-item:last').addClass('layui-show');
                if ($('.layui-tab-content .layui-tab-item:last iframe').length<1) {
                    $('.layui-tab-content .layui-tab-item:last').html('<iframe id="main" style="height:800px;" src="/CloudPMS/Home"></iframe>');
                }
            }
        });
    } else {//跳转窗口
        var navno = $('#roomid_' + roomid + '').attr('data-id');
        $('.layui-tab-title li i.layui-tab-close[data-id=' + navno + ']').parent('li').addClass('layui-this').siblings("li").removeClass('layui-this');
        $('#roomid_' + roomid + '').parent('.layui-tab-item').addClass('layui-show').siblings(".layui-tab-item").removeClass('layui-show');
    }
}