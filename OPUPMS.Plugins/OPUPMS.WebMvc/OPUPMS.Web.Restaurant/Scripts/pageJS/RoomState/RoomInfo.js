/**
 * 开房事件处理
 */


layui.use('form', function(){
    var form = layui.form();
    //监听贵重物品是否开启
    form.on('switch(gzwp)', function(data){
      if (data.elem.checked==true) {
          $('#gzwp').show();
          $('#more_cz').addClass('xian').addClass('p-sm');
      }else{
          $('#gzwp').hide();
      }
    });
    //监听接送机是否开启
    form.on('switch(service_jsj)', function(data){
      if (data.elem.checked==true) {
          $('#service_jsj').show();
          $('#service_jsj2').show();
          $('#more_cz').addClass('xian').addClass('p-sm');
      }else{
          $('#service_jsj').hide();
          $('#service_jsj2').hide();
      }
    });
    //监听服务是否开启
    form.on('switch(service_fw)', function(data){
      if (data.elem.checked==true) {
          $('#service_fw').show();
          $('#more_cz').addClass('xian').addClass('p-sm');
      }else{
          $('#service_fw').hide();
      }
    });
    //监听叫醒是否开启
    form.on('switch(service_jx)', function(data){
      if (data.elem.checked==true) {
          $('#service_jx').show();
          $('#more_cz').addClass('xian').addClass('p-sm');
      }else{
          $('#service_jx').hide();
      }
    });
})


