
;(function($){

//  说明：
//  表单控件


//select单选选择器选择事件
  $.fn.select = function(settings) {
    var $div = this;    
    $div.on("click", "li", function() {
        $(this).parent(".selected_group").parent(".selected_box").prev('.select_value').text($(this).text()).prev('select').val($(this).attr('data-val'));
        $(this).parent(".selected_group").parent(".selected_box").parent(".select").parent(".title_and_select").removeClass('focus');
        $(this).parent(".selected_group").parent(".selected_box").remove();
    });
  };


  //input and 单选选择器选择事件
  $.fn.andselectvalue = function(settings) {
    var $div = this;    
    $div.on("click", "li", function() {
        $(this).addClass('selected').siblings("li").removeClass("selected");
        var selectid=$(".eject_box").attr('data-id');
        $("#"+selectid+"").find("option").attr('selected',false);
        $("#"+selectid+"").prev('.select_icon').prev('input').val($(this).text());
        $("#"+selectid+"").val($(this).attr('data-val'));

        $("#"+selectid+"").find("option:selected").attr('selected',true);
        $('.eject_box').remove();
    });
  };

  

})(jQuery);


function select_click() {

//select单选框点击事件选择器弹出于隐藏

$(".select a").click(function (event) {
  event.stopPropagation();
  if ($(this).parent(".select").find('.selected_box').length>0) {
     return false;
  }else{
     var title=$(this).prev('select').attr('data-title');
     var selectid=$(this).prev('select').attr('id');
     var option='';
     var ulheight='';
     var optionnum=$(this).prev('select').find('option').length;
     for (var i =0; i < optionnum; i++) {
        var sel='';
        var option_text=$(this).prev('select').children().eq(i).text();
        var option_val=$(this).prev('select').children().eq(i).val();
        
        if ($(this).prev('select').children().eq(i).attr('selected')) {
           var sel='class="selected"';
        }
        option+='<li '+sel+' data-val="'+option_val+'"><i></i>'+option_text+'</li>';
     }
     if (optionnum>8) {
        ulheight='style="height:240px;"';
     }
     var str='<div class="selected_box" data-id="'+selectid+'">\
                 <ul class="selected_group" '+ulheight+'>'+option+'</ul>\
               </div>\
              </div>'; 
     $('.selected_box').remove();
     $('.title_and_select').removeClass('focus');
     $(this).parent(".select").append(str); 
     $(this).parent(".select").parent(".title_and_select").addClass('focus');
     $(".selected_group").select({});
     
  }
})

$(document).click(function(){
    $('.selected_box').remove();
    $('.title_and_select').removeClass('focus');
});

}






//title_and_input获得焦点
function title_input_focus() {
  $('input[type="text"]').bind('focus', function(){
    $(this).parent("div").addClass('focus');
    $(this).blur(function(){
      $(this).parent("div").removeClass('focus');
    })
  });
}

//checkbox全选/反选事件
function checkbox_all() {
  $(".Allopt").click(function(){
     var formid=$(this).attr('data-form_id');
     if ($(this).is(':checked')) {
       $('#'+formid+' input[type="checkbox"]').each(function(){this.checked=true;});
     }else{
       $('#'+formid+' input[type="checkbox"]').each(function(){this.checked=false;});
     }
     
  });
}



//回车切换input
function keydown_to_tab($input){
  if(!$input) $input = $('input:text:not(:disabled)');
  $input.bind("keydown", function(e) {
    var n = $input.length;
    if (e.which == 13){
          e.preventDefault(); //Skip default behavior of the enter key
          var nextIndex = $input.index(this) + 1;
          if(nextIndex < n)
              $input[nextIndex].focus();
          else
              $input[nextIndex-1].blur();
    }
  });
}



//textarea自适应自适应高度
jQuery.fn.extend({
    autoHeight: function(){
        return this.each(function(){
            var $this = jQuery(this);
            if( !$this.attr('_initAdjustHeight') ){
                $this.attr('_initAdjustHeight', $this.outerHeight());
            }
            _adjustH(this).on('input', function(){
                _adjustH(this);
            });
        });
        function _adjustH(elem){
            var $obj = jQuery(elem);
            return $obj.css({height: $obj.attr('_initAdjustHeight'), 'overflow-y': 'hidden'})
                    .height( elem.scrollHeight );
        }
    }
});

//修改输入框激活
function edit_input() {
   $('.edit_input .btn').click(function () {
      var div=$(this).parent('.input-group-btn').parent('.input-group').parent('.edit_input');

      if (div.hasClass('edit')) {
         div.removeClass('edit');
         $(this).parent('.input-group-btn').prev('input').attr('disabled',true);
         $(this).attr('onclick','').html('<i class="fa icon-edit"></i>');
         var roomid=$(this).attr('data-roomid');
         var number=$(this).attr('data-number');
         var price=$(this).parent('.input-group-btn').prev('input').val();
         edit_room_price(roomid,number,price);
      }else{
         var onclick=$(this).attr('data-onclick');
         div.addClass('edit');
         div.find('.input-group').find('input[type="text"]').attr('disabled',false).focus();
         $(this).attr('onclick',onclick).html('<i class="fa icon-check"></i>');
      }
   })
}

//设置房价
function edit_room_price(roomid,number,price) {
  //Ajax设置房价
  //...
  layer.msg('设置成功');
}





