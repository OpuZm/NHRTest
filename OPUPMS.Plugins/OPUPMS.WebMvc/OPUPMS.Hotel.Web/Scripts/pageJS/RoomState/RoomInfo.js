/**
 * 开房事件处理
 */


layui.use('form', function () {
    var form = layui.form();


    //监听贵重物品是否开启
    form.on('switch(gzwp)', function (data) {
        if (data.elem.checked == true) {
            $('#gzwp').show();
            $('#more_cz').addClass('xian').addClass('p-sm');
        } else {
            $('#gzwp').hide();
        }
    });
    //监听接送机是否开启
    form.on('switch(service_jsj)', function (data) {
        if (data.elem.checked == true) {
            $('#service_jsj').show();
            $('#service_jsj2').show();
            $('#more_cz').addClass('xian').addClass('p-sm');
        } else {
            $('#service_jsj').hide();
            $('#service_jsj2').hide();
        }
    });
    //监听服务是否开启
    form.on('switch(service_fw)', function (data) {
        if (data.elem.checked == true) {
            $('#service_fw').show();
            $('#more_cz').addClass('xian').addClass('p-sm');
        } else {
            $('#service_fw').hide();
        }
    });
    //监听叫醒是否开启
    form.on('switch(service_jx)', function (data) {
        if (data.elem.checked == true) {
            $('#service_jx').show();
            $('#more_cz').addClass('xian').addClass('p-sm');
        } else {
            $('#service_jx').hide();
        }
    });


})


//房间选择器
//房间详情，添加房间(联房)事件
function add_relation_room() {
    var str = '<div class="room-selector padding ov">'
           + '<div class="room-selector_box">'
           + '<div class="col-sm-3 room-selector_nav">'
           + '<div class="form-group">'
           + '<div class="input-group border-search">'
           + '<input type="text" name="keyword" class="form-control input-sm bg-light no-border rounded padder" placeholder="输入房号查询..." value="" onkeydown="javascript:if (event.keyCode==13) room_sel_search();">'
           + '<span class="input-group-btn">'
           + '<button type="submit" onclick="room_sel_search();" class="btn btn-sm bg-light rounded"><i class="fa icon-search"></i></button>'
           + '</span>'
           + '</div>'
           + '</div>'
           + '<ul class="list-group">'
           + '<li class="list-group-item">全部<span class="badge">129</span></li>'
           + '<li class="list-group-item">豪华大床房<span class="badge">45</span></li>'
           + '<li class="list-group-item">普通标间<span class="badge">24</span></li>'
           + '<li class="list-group-item">豪华三人间<span class="badge">14</span></li>'
           + '<li class="list-group-item">榻榻米<span class="badge">11</span></li>'
           + '<li class="list-group-item">标准套房<span class="badge">27</span></li>'
           + '<li class="list-group-item">电脑房<span class="badge">8</span></li>'
           + '</ul>'
           + '</div>'
           + '<div class="col-sm-9 room-selector_list">'
           + '<ul>'
           + '<li>'
           + '<h5><span>1001</span><span>(联主)</span></h5>'
           + '<i class="icon-user_b"></i>'
           + '<p>客户姓名</p>'
           + '</li>'
           + '<li>'
           + '<h5><span>1002</span><span></span></h5>'
           + '<i class="icon-twouser"></i>'
           + '<p>客户姓名</p>'
           + '</li>'
           + '<li>'
           + '<h5><span>1003</span><span>(联从)</span></h5>'
           + '<i class="icon-Customer"></i>'
           + '<p>客户姓名</p>'
           + '</li>'
           + '<li>'
           + '<h5><span>1001</span><span>(联主)</span></h5>'
           + '<i class="icon-user_b"></i>'
           + '<p>客户姓名</p>'
           + '</li>'
           + '<li>'
           + '<h5><span>1002</span><span></span></h5>'
           + '<i class="icon-twouser"></i>'
           + '<p>客户姓名</p>'
           + '</li>'
           + '</ul>'
           + '</div>'
           + '</div>'
           + '<div class="footer-btn_group">'
           + '<div class="col-sm-6">'
           + '<span>已选择:<span id="room_selnum">0</span>个</span>'
           + '<a class="btn btn-default btn-xs" onclick="all_roomsel_cancel();">取消选中</a>'
           + '</div>'
           + '<div class="col-sm-6 text-right">'
           + '<a href="javascript:void(0);" class="btn m-b-xs w-xs btn-success" style="display: none;">确认</a>'
           + '<a href="javascript:void(0);" class="btn m-b-xs w-xs ml-xs btn-default pop_close">取消</a>'
           + '</div>'
           + '</div>'
           + '</div>';


    layer.open({
        type: 1,
        skin: '', //加上边框
        area: ['800px', '600px'], //宽高
        content: str
    });

    room_selector_roomcheck();
    // pop_close();



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
