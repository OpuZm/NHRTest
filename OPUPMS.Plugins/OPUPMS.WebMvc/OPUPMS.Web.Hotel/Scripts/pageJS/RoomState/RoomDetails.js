/**
 * 房间详情事件处理
 */


  layui.use(['layer','element','form'], function() {
          var $ = layui.jquery,
          element = layui.element(),
          layer = layui.layer,
          form = layui.form();
          $('.collapse').collapse();
          $('.loader-box').remove();
         
   

  })

//修改主结房激活
function set_mainroom_show() {
   if ($('#Relation').hasClass('set-mainroom')) {
      $('#Relation').removeClass('set-mainroom');
      $('#Relation .Relation ul li').unbind("click");
   }else{
      $('#Relation').addClass('set-mainroom');
      $('.set-mainroom .Relation ul li').click(function() {
          $(this).addClass('active').siblings("li").removeClass('active');
          $('input[name="all_mainroom_check"]:checkbox').each(function(){this.checked=false;});
      })

      $('input[name="all_mainroom_check"]:checkbox').click(function(){
          if($(this).is(':checked')) {
             $('.set-mainroom .Relation ul li').removeClass('active');
          }else{
             //alert(2);
          }
      });
   }
}


//确认设置主结房
function set_mainroom() {
   if ($('.set-mainroom .Relation ul li.active').length>0) {//存在主结房
      $('.set-mainroom .Relation ul li.active').addClass('main-room').removeClass('active').removeClass('next-room').find('h5').find('.room-title').text('主结房').parent('h5').parent('li').siblings('li').addClass('next-room').removeClass('main-room').find('h5').find('.room-title').text('从房');
      $('#Relation').removeClass('set-mainroom');
      $('#Relation .Relation ul li').unbind("click");
   }else{//各自结账
      if($('input[name="all_mainroom_check"]:checkbox').is(':checked')) {
         $('.set-mainroom .Relation ul li').addClass('main-room').removeClass('next-room').find('h5').find('.room-title').text('主结房');
         $('#Relation').removeClass('set-mainroom');
         $('#Relation .Relation ul li').unbind("click");
      }else{
         layer.msg('请选择主结房');
      }

   }
}



//房间详情，添加房间(联房)事件
function add_relation_room() {
    $('.Relation ul li .Relation-hove .add_Relation_room').click(function() {
       
       var data='<div class="room-selector padding ov">'
               +  '<div class="room-selector_box">'
               +    '<div class="col-sm-3 room-selector_nav">'
               +       '<div class="form-group">'
               +         '<div class="input-group border-search">'
               +           '<input type="text" name="keyword" class="form-control input-sm bg-light no-border rounded padder" placeholder="输入房号查询..." value="" onkeydown="javascript:if (event.keyCode==13) room_sel_search();">'
               +           '<span class="input-group-btn">'
               +             '<button type="submit" onclick="room_sel_search();" class="btn btn-sm bg-light rounded"><i class="fa icon-search"></i></button>'
               +           '</span>'
               +         '</div>'
               +      '</div>'
               +       '<ul class="list-group">'
               +         '<li class="list-group-item">全部<span class="badge">129</span></li>'
               +         '<li class="list-group-item">豪华大床房<span class="badge">45</span></li>'
               +         '<li class="list-group-item">普通标间<span class="badge">24</span></li>'
               +         '<li class="list-group-item">豪华三人间<span class="badge">14</span></li>'
               +         '<li class="list-group-item">榻榻米<span class="badge">11</span></li>'
               +         '<li class="list-group-item">标准套房<span class="badge">27</span></li>'
               +         '<li class="list-group-item">电脑房<span class="badge">8</span></li>'
               +       '</ul>'
               +    '</div>'
               +    '<div class="col-sm-9 room-selector_list">'
               +      '<ul>'
               +        '<li>'
               +          '<h5><span>1001</span><span>(联主)</span></h5>'
               +          '<i class="icon-user_b"></i>'
               +          '<p>客户姓名</p>'
               +        '</li>'
               +        '<li>'
               +          '<h5><span>1002</span><span></span></h5>'
               +          '<i class="icon-twouser"></i>'
               +          '<p>客户姓名</p>'
               +        '</li>'
               +        '<li>'
               +          '<h5><span>1003</span><span>(联从)</span></h5>'
               +          '<i class="icon-Customer"></i>'
               +          '<p>客户姓名</p>'
               +        '</li>'
               +        '<li>'
               +          '<h5><span>1001</span><span>(联主)</span></h5>'
               +          '<i class="icon-user_b"></i>'
               +          '<p>客户姓名</p>'
               +        '</li>'
               +      '</ul>'
               +    '</div>'
               +  '</div>'
               +  '<div class="footer-btn_group">'
               +      '<div class="col-sm-6">'
               +         '<span>已选择:<span id="room_selnum">0</span>个</span>'
               +         '<a class="btn btn-default btn-xs" onclick="all_roomsel_cancel();">取消选中</a>'
               +      '</div>'
               +      '<div class="col-sm-6 text-right">'
               +         '<a href="#" class="btn m-b-xs w-xs btn-success" style="display: none;">确认</a>'
               +         '<a href="javascript:layer.closeAll();" class="btn m-b-xs w-xs btn-default pop_close ml-sm" >取消</a>'
               +      '</div>'
               +  '</div>'
               +'</div>';
      // $.get("../../Views/HRoomManage/RoomSelector.html",function(data){
                  
            layer.open({
              type: 1,
              skin: '', //加上边框
              area: ['800px', '600px'], //宽高
              content: data
            });

            room_selector_roomcheck();
     //  });

    })

}



//房间选择器房间选择事件
function room_selector_roomcheck() {
   $('.room-selector_list li').click(function () {
     if ($(this).hasClass('active')) {
        $(this).removeClass('active');
     }else{
        $(this).addClass('active');
     }

     var activenum=$('.room-selector_list li.active').length;
     if (activenum>0) {
       $('.room-selector .footer-btn_group .btn-success').show();
     }else{
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

