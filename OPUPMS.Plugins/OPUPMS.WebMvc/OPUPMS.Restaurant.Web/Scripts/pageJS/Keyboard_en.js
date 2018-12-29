

var data;
  var keycache='';
  var thisfontArr=[];
  var thisleft=[];
  var languagetype='en';
  var keysdata=[
                [1]
               ,[2,'A','B','C']
               ,[3,'D','E','F']
               ,[4,'G','H','I']
               ,[5,'J','K','L']
               ,[6,'M','N','O']
               ,[7,'P','Q','R','S']
               ,[8,'T','U','V']
               ,[9,'W','X','Y','Z']
               ];
       

  function ShowKeyboardbox(inputname) {

      var input=$('input[name="'+inputname+'"]');
//    if (input.hasClass('keythis')) {
//      return false;
//    }
      $('input').removeClass('keythis');
      input.addClass('keythis');
      //构建键盘
      var keyboardkeys='';
      for (var i = 0; i < keysdata.length; i++) {
        var keylabel='',keyvalue='';
        if (keysdata[i].length==1) {
          keyvalue=keysdata[i][0];
        }else{
          keylabel=keysdata[i][0];
          for (var j = 0; j < keysdata[i].length; j++) {
            if (j!=0) {
                keyvalue+=keysdata[i][j];
            }
          }
        }
        keyboardkeys+='<li><label>'+keylabel+'</label><div class="keyboard-value">'+keyvalue+'</div></li>';
      }
       keyboardkeys+='<li class="keyboard-other keyboard-type"><label></label><div class="keyboard-value">全</div></li>';
        keyboardkeys+='<li class="keyboard-zero"><label></label><div class="keyboard-value">0</div></li>';
        if (languagetype=='en') {
           keyboardkeys+='<li data-type="en" class="keyboard-other keyboard-language"><label>中</label><div class="keyboard-value">英</div></li>';
        }else{
           keyboardkeys+='<li data-type="ch" class="keyboard-other keyboard-language"><label>英</label><div class="keyboard-value">中</div></li>';
        }
        

      var keyboardHtml='<div class="hg-keyboard" >'
                      +   '<div class="keyboard-keys"></div>'
                      +   '<div class="keyboard-head"><span></span><a href="javascript:void(0);" class="del" data-type="del"> ← </a></div>'
                      +   '<div class="keyboard-body">'
                      +     '<div class="keyboard-side-left">'
                      +     '</div>'
                      +     '<div class="keyboard-key-box">'
                      +        '<ul>'+keyboardkeys+'</ul>'
                      +     '</div>'
                      +   '</div>'
                      +   '<div class="keyboard-more-font"></div>'
                      +'</div>';
      $('#keyboard_layer').html(keyboardHtml);

      data=ChineseData();
      //键盘点击
      $('.keyboard-key-box li').click(function(event) {
          if ($(this).hasClass('keyboard-language')) {//中英文切换
              if (languagetype=='en') {
                 languagetype='ch';
                 $(this).attr('data-type','ch').find('label').text('英').next('.keyboard-value').text('中');
              }else{
                 languagetype='en';
                 $(this).attr('data-type','en').find('label').text('中').next('.keyboard-value').text('英'); 
              }
              $('.keyboard-side-left').html('');
          }else if ($(this).hasClass('keyboard-zero')) {//0
               setinputval('0');
          }else if ($(this).hasClass('keyboard-type')) {//切换全键盘
          	   layer.close(layer.index);
               Allkeyboard(input);
          }else{
               var keyindex=$(this).index();
               var keydata=keysdata[keyindex];
               if (keydata.length==1) {//数字
                     setinputval(keydata[0]);
               }else{
                     if (languagetype=='en') {//英文输入
                         var newkeydata=[];
                         for (var i = 0; i < keydata.length; i++) {
                            newkeydata.push(keydata[i]);
                            if (i>0) {
                               var xiao=keydata[i].toLowerCase();
                               newkeydata.push(xiao);
                            }
                         }
                         newhead(newkeydata);
                     }else{//数字输入
                        //更新左侧按钮
                         var side_left='';
                         for (var i = 1; i < keydata.length; i++) {
                             var active=i==1?'class="active"':''; 
                             side_left+='<a href="javascript:void(0);" '+active+'>'+keydata[i]+'</a>';
                         } 
                         side_left+='<a href="javascript:void(0);">'+keydata[0]+'</a>';
                         $('.keyboard-side-left').html('<div class="keyboard-side-left-box"><div class="keyboard-side-left-list">'+side_left+'</div></div>');
                         thisleft=keydata;
                         //更新文字
                         
                         //默认key值
                         var keyword=keydata[1];
                         var font=fontdata(keycache+keyword,keyword);
                         newhead(font);
                     }
               }
          }
          return false;
         
          
      });
      
      //左侧点击
      $('.keyboard-side-left').delegate('.keyboard-side-left-box a', 'click', function(event) {

          $(this).addClass('active').siblings('a').removeClass('active');
          var keyindex=$(this).index()+1;
          var keyword=thisleft[keyindex];
          if (!keyword) {//数字
             keyword=thisleft[0];
             setinputval(keyword);
          }else{
             if (keyword) {
               var leftArr=keycache.split('');
               var newindex=leftArr.length-1;
               leftArr[newindex]=keyword.toLowerCase();
               var newkeycache='';
               for (var i = 0; i < leftArr.length; i++) {
                   newkeycache+=leftArr[i];
               }
               console.log(newkeycache);
               var font=fontdata(newkeycache,keyword);
               newhead(font);
            }
          }
          
      });

      //文字点击
      $('.keyboard-head').delegate('a', 'click', function(event) {
          var type=$(this).attr('data-type');
          if (type=='del') {
              var leftArr=keycache.split('');
              if (leftArr==0) {//删除输入框
                var inputdom=$('input.keythis');
                var inputval=inputdom.val();
                var newval = inputval.substring(0, inputval.length - 1);
                inputdom.val(newval);
                if($('input[name="TcKeyWord"]').length>0){
				       	  TcKeyWord(newval.toUpperCase());
				       	}else if($('input[name="KeyWord"]').length>0){
				       	  KeyWord(newval.toUpperCase());
					      }
              }else{
                var newindex=leftArr.length-1;
                leftArr.splice(newindex,1);
                var newkeycache='';
                for (var i = 0; i < leftArr.length; i++) {
                    newkeycache+=leftArr[i];
                }
                var font=fontdata(newkeycache,newkeycache);
                newhead(font);
                $('.keyboard-side-left').html('');
              }

          }else if (type=='more') {
              //更多文字弹出
              var morebox=$('.keyboard-more-font');
              if (morebox.hasClass('show')) {
                  morebox.removeClass('show');
              }else{
                  morebox.addClass('show');
              }
          } else{
              var thisfont=$(this).text();
              if (thisfont) {
                 setinputval(thisfont);
              }
          }
      });

      //更多文字点击
      $('.keyboard-more-font').delegate('a', 'click', function(event) {
            var thisfont=$(this).text();
            if (thisfont) {
               setinputval(thisfont);
            }
      })
  }

  //更新文字
  function newhead(font) {
       var fontArr=font;
       thisfontArr=fontArr;
       var hrad_font='';
       var more_font='';
       for (var i = 0; i < fontArr.length; i++) {
          if (i<6) {
             hrad_font+='<a href="javascript:void(0);">'+fontArr[i]+'</a>';
          }else{
             more_font+='<a href="javascript:void(0);">'+fontArr[i]+'</a>';
          }
       }
       if (fontArr.length>6) {
          hrad_font+='<a href="javascript:void(0);" data-type="more">></a>';
       }
       
       $('.keyboard-head span').html(hrad_font);
       $('.keyboard-more-font').html('<div class="keyboard-more-box">'+more_font+'</div>');
       //$('input.keythis').focus();
       return false;
  }
 
 //取得文字
  function fontdata(keyword,keyval) {
    var keyword=keyword.toLowerCase();
    var objkeys = [];
    var font='';
    for(objkeys[objkeys.length] in data);
    for(var key in data){
        if (key==keyword) {
          font = data[key];
        }
    }
    keycache=keyword;
    if (keycache) {
       keycachehtml=keycache?'<span>'+keycache+'</span>':'';
    }else{
      keycachehtml='';
    }
    $('.keyboard-keys').html(keycachehtml);
    var fontArr=font.split('');
    return fontArr;
  }

  function setinputval(value) {
       var inputval= $('input.keythis').val();
       keycache='';
       $('input.keythis').val(inputval+value);
       $('.keyboard-head span').html('');
       $('.keyboard-keys').html('');
       $('.keyboard-side-left').html('');
       $('.keyboard-more-font').removeClass('show').html('');
//     if($('input[name="KeyWord"]').length>0){
//     	  var newval=inputval+value;
//     	  KeyWord(newval.toUpperCase());
//     }
//     if($('input[name="TcKeyWord"]').length>0){
//     	  var newval=inputval+value;
//     	  TcKeyWord(newval.toUpperCase());
//     }
       if($('input[name="TcKeyWord"]').length>0){
       		var newval=inputval+value;
       	  TcKeyWord(newval.toUpperCase());
       	}else if($('input[name="KeyWord"]').length>0){
       		var newval=inputval+value;
       	  KeyWord(newval.toUpperCase());
	      }
  }