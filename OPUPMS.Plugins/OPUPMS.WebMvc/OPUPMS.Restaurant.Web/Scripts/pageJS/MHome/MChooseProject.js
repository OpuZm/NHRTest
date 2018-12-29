/**
 * 点菜
 */
var inidata; //初始化得到的数据
var OrderTableIds;
var OrderTableProjectsdata = []; //已选订单的数组
var thisOrderProjectArr, thisProjectArr;
var laytpl;
var thisProjectsIndex;
var element;
var form;
var userInfo; //登陆用户信息
var enter_type = 1;
var allStatus = 1;

var btnOption = {
	26:{Name:'保存',Click:"AddOrderBefore('Keep');",IsLock:true},
	27:{Name:'全送',Click:"AddOrderBefore('Print');",IsLock:true,CyddStatus:1},
	28:{Name:'全免送',Click:"AddOrderBefore('NoPrint');",IsLock:true,CyddStatus:1},
	29:{Name:'关闭',Click:"cancelOut();"},
	30:{Name:'单品即起',Click:"EditDishesStatus(1);",IsLock:true,CyddStatus:1},
	31:{Name:'单品叫起',Click:"EditDishesStatus(2);",IsLock:true,CyddStatus:1},
	32:{Name:'手写做法',Click:"EditTypeName(this,'2');",IsLock:true,CyddStatus:1},
	33:{Name:'全单做法',Click:"openExtendMore('All','1');",IsLock:true,CyddStatus:1},
	34:{Name:'全单即起',Click:"DishesStatus('1');",IsLock:true,CyddStatus:1},
	35:{Name:'全单叫起',Click:"DishesStatus('2');",IsLock:true,CyddStatus:1},
	36:{Name:'取消赠送',Click:"cancelRetire(1);",IsLock:true,CyddStatus:1},
	37:{Name:'取消退菜',Click:"cancelRetire(2);",IsLock:true,CyddStatus:1},
	38:{Name:'催菜',Click:"UrgeOrder();",IsLock:true,CyddStatus:1,isOnly:true},
	39:{Name:'换台',Click:"changeTab(this);",IsLock:true,CyddStatus:1,isOnly:true},
	40:{Name:'加台',Click:"addTab(this);",IsLock:true,CyddStatus:1,isOnly:true},
	41:{Name:'辙台',Click:"revokeTab(this);",IsLock:true,CyddStatus:1,isOnly:true},
	42:{Name:'并台',Click:"combineTab(this);",IsLock:true,CyddStatus:1,isOnly:true},
	43:{Name:'拼台',Click:"spellTab(this);",IsLock:true,CyddStatus:1,isOnly:true},
	44:{Name:'菜品转台',Click:"openChangeTable();",IsLock:true,CyddStatus:1},
	45:{Name:'多桌点餐',Click:"openChoseTable();",IsLock:true,CyddStatus:1},
	46:{Name:'打厨单',Click:"InitCookOrder();",IsLock:true,CyddStatus:1,isOnly:true},
	47:{Name:'解锁',Click:"UnLock(this);",CyddStatus:1},
	48:{Name:'人数修改',Click:"NumberKeyboard('editSum',this);",IsLock:true,CyddStatus:1,isOnly:true},
	49:{Name:'订单信息查看',Click:"orderInfoCheck(this);",IsLock:true,CyddStatus:1},
	50:{Name:'列印全单',Click:"PrintLXDALL()",IsLock:true,CyddStatus:1},
}

/*
 * 0 表示默认没有任何弹窗
 * 1 表示左下套餐界面弹出
 * 2 表示套餐修改界面弹出
 */
var TlayerType = 0;

var winW = $(window).width();
var winH = $(window).height();

layui.use(['element', 'form', 'laytpl'], function () {
    element = layui.element;
    form = layui.form;
    laytpl = layui.laytpl;

    OrderTableIdString = getUrlParam('OrderTableIds');
    OrderTableIds = OrderTableIdString.split(',');
    
	
	chat = $.connection.systemHub;
	chat.hubName = 'systemHub';
	chat.connection.start();
	
    //获取参数
    $.ajax({
        url: "/Res/Project/InitFormInfo",
        data: {
            orderTableId: OrderTableIds
        },
        type: "post",
        dataType: "json",
        success: function (data) {
            inidata = data;
            //判断锁定
            if (inidata.OrderAndTables.IsLock == true) { //锁定
                var w = winW - 470;
                var h = winH - 63;
                var str = '<div class="locking" style="width:' + w + 'px;height:' + h + 'px"><div class="locking-box"><i class="iconfont">&#xe677;</i><h4>该桌已被锁定，如需操作请先解锁!</h4></div><div class="locking-bg"></div></div>';
                $('body').append(str);
                $('#operation_lists li a').not(':last').addClass('Disable');
				$('#more-btn-group').empty().append('<a class="layui-btn layui-btn-normal layui-btn-lg" onclick="UnLock(this);">解锁</a>');
            }
            
            //按钮数据重构并排序
			data.Actionsbtn = [];//点餐功能区
			data.MoreOrderbtn = [];//更多	=> 菜品操作
			data.MoreTablebtn = [];//更多	=> 桌台操作
			for(var i=0;i<inidata.CustomConfigsFlat.length;i++){
				var Id = inidata.CustomConfigsFlat[i].Id;
				var thisBtn = btnOption[Id];
				if(!thisBtn)continue;
				if(thisBtn.Click)inidata.CustomConfigsFlat[i].Click = thisBtn.Click;
				if(thisBtn.IsLock)inidata.CustomConfigsFlat[i].IsLock = thisBtn.IsLock;
				if(thisBtn.CyddStatus)inidata.CustomConfigsFlat[i].CyddStatus = thisBtn.CyddStatus;
				if(thisBtn.isOnly)inidata.CustomConfigsFlat[i].isOnly = thisBtn.isOnly;
				
				
				switch(inidata.CustomConfigsFlat[i].ModuleName){
					case '点餐功能区':
						data.Actionsbtn.push(inidata.CustomConfigsFlat[i])
						break;
					case '更多-菜品操作':
						data.MoreOrderbtn.push(inidata.CustomConfigsFlat[i])
						break;
					case '更多-桌台操作':
						data.MoreTablebtn.push(inidata.CustomConfigsFlat[i])
						break;
				}
			}
			//渲染 底部操作按钮
			var getTpl = actionsbtn_tpml.innerHTML,
				view = document.getElementById('actionsbtn_view');
			laytpl(getTpl).render(data, function(html) {
				view.innerHTML = html;
			});
			//渲染 更多操作按钮
			var getTpl = moreBtnGroup_tpml.innerHTML,
				view = document.getElementById('more-btn-group');
			laytpl(getTpl).render(data, function(html) {
				view.innerHTML = html;
			});
            
            
            //渲染 菜品分类
            var getTpl = CategoryList_tpml.innerHTML,
                view = document.getElementById('CategoryList_view');
            laytpl(getTpl).render(data.CategoryList, function (html) {
                view.innerHTML = html;
            });
			
			//渲染菜品  默认展开  第一个标签内的所有数据
			var defaultDetailsArr = [];
			var defaultCategoryList = inidata.CategoryList[0];
			if (inidata.CategoryList[0].ChildList.length > 0) { //有子分类
                for (var i = 0; i < defaultCategoryList.ChildList.length; i++) {
                    classid = defaultCategoryList.ChildList[i].Id;
                    for (var j = 0; j < inidata.ProjectAndDetails.length; j++) {
                        var item = inidata.ProjectAndDetails[j];
                        if (classid == item.Category) { //成立
                            defaultDetailsArr.push(item);
                        }
                    }
                }
            } else { //没有子分类的
                for (var j = 0; j < inidata.ProjectAndDetails.length; j++) {
                    var item = inidata.ProjectAndDetails[j];
                    if (defaultCategoryList.Id == item.Category) { //成立
                        defaultDetailsArr.push(item);
                    }
                }
            }
            
            //渲染 菜品
            var getTpl = ProjectAndDetails_tpml.innerHTML,
                view = document.getElementById('ProjectAndDetails_view');
            laytpl(getTpl).render(defaultDetailsArr, function (html) {
                view.innerHTML = html;
            });


            //渲染 订单头部信息
            var getTpl = OrderAndTables_tpml.innerHTML,
                view = document.getElementById('OrderAndTables_view');
            laytpl(getTpl).render(data.OrderAndTables, function (html) {
                view.innerHTML = html;
            });
            
            //键盘回车  失去input焦点
            $(document).on('submit','.search-form',function(){
            	$(this).find('input').blur();
            	return false;
            })

            //监听搜索
            //			$('#KeyWord').bind('input propertychange', function(e) {
            //				var value = $(this).val().toUpperCase();
            //				KeyWord(value);
            //			})

            search_input('#KeyWord', KeyWord, 64);

            //初始化数据加入 提交数组
            for (var i = 0; i < data.OrderTableProjects.length; i++) {
                //if(data.OrderTableProjects[i].CyddMxStatus==1){
                var OrderTableProjectsitem = data.OrderTableProjects[i];
                var OrderTableProjectsitemExtend = [];
                if (data.OrderTableProjects[i].Extend) {
                    for (var j = 0; j < data.OrderTableProjects[i].Extend.length; j++) {
                        OrderTableProjectsitemExtend.push(data.OrderTableProjects[i].Extend[j]);
                    }
                }
                if (data.OrderTableProjects[i].ExtendExtra) {
                    for (var j = 0; j < data.OrderTableProjects[i].ExtendExtra.length; j++) {
                        OrderTableProjectsitemExtend.push(data.OrderTableProjects[i].ExtendExtra[j]);
                    }
                }
                if (data.OrderTableProjects[i].ExtendRequire) {
                    for (var j = 0; j < data.OrderTableProjects[i].ExtendRequire.length; j++) {
                        OrderTableProjectsitemExtend.push(data.OrderTableProjects[i].ExtendRequire[j]);
                    }
                }
                OrderTableProjectsdata.push({
                	CreateDate: OrderTableProjectsitem.CreateDate,
                    CyddTh: OrderTableIds,
                    CyddMxType: OrderTableProjectsitem.CyddMxType,
                    CyddMxId: OrderTableProjectsitem.CyddMxId,
                    Price: OrderTableProjectsitem.Price,
                    Num: OrderTableProjectsitem.Num,
                    CyddMxStatus: OrderTableProjectsitem.CyddMxStatus,
                    ProjectName: OrderTableProjectsitem.ProjectName,
                    Unit: OrderTableProjectsitem.Unit,
                    Id: OrderTableProjectsitem.Id,
                    R_Project_Id: OrderTableProjectsitem.R_Project_Id,
                    CyddMxName: OrderTableProjectsitem.CyddMxName,
                    ProjectDetailList: [],
                    CostPrice: OrderTableProjectsitem.CostPrice,
                    IsChangePrice: OrderTableProjectsitem.IsChangePrice,
                    IsChangeNum: OrderTableProjectsitem.IsChangeNum,
                    IsUpdateNum: false,
                    DishesStatus: OrderTableProjectsitem.DishesStatus,
                    IsGive: OrderTableProjectsitem.IsGive,
                    Extend: OrderTableProjectsitemExtend,
                    Remark: OrderTableProjectsitem.Remark,
                    OrderDetailRecordCount: OrderTableProjectsitem.OrderDetailRecordCount,
                    PackageDetailList: OrderTableProjectsitem.PackageDetailList,
                    R_OrderTable_Id: OrderTableProjectsitem.R_OrderTable_Id,
                    OrderDetailRecord: OrderTableProjectsitem.OrderDetailRecord,
                    Property: OrderTableProjectsitem.Property,
                    IsQzdz: OrderTableProjectsitem.IsQzdz,
                    IsCustomer: OrderTableProjectsitem.IsCustomer,
                    IsRecommend: OrderTableProjectsitem.IsRecommend
                });
            }
            // }

//          $('.CategoryListTab').width(winW - 470);
            element.init();
            AddProject();
            //更新订单/统计金额
            NewsOrder();
            
            

            //监听菜品分类一 点击
            $(document).on('click','.ClassTab-head .CategoryListTab .tabList li',function(){
//          	if($(this).hasClass('layui-this'))return false;
            	var index = $(this).index();
            	var newsArr = [];
            	$(this).addClass('layui-this').siblings().removeClass('layui-this');
            	
//              if (index == 0) { //全部
//                  newsArr = inidata.ProjectAndDetails;
//              } else {
                    var CategoryList = inidata.CategoryList[index];
                    if (CategoryList.ChildList.length > 0) { //有子分类
                        for (var i = 0; i < CategoryList.ChildList.length; i++) {
                            classid = CategoryList.ChildList[i].Id;
                            for (var j = 0; j < inidata.ProjectAndDetails.length; j++) {
                                var item = inidata.ProjectAndDetails[j];
                                if (classid == item.Category) { //成立
                                    newsArr.push(item);
                                }
                            }
                        }
                    } else { //没有子分类的
                        for (var j = 0; j < inidata.ProjectAndDetails.length; j++) {
                            var item = inidata.ProjectAndDetails[j];
                            if (CategoryList.Id == item.Category) { //成立
                                newsArr.push(item);
                            }
                        }
                    }
//              }
                var getTpl = ProjectAndDetails_tpml.innerHTML,
                    view = document.getElementById('ProjectAndDetails_view');
                laytpl(getTpl).render(newsArr, function (html) {
                    view.innerHTML = html;
//                  setTimeout(projectAndDetailsAuto, 500);//菜品父元素自适应
//                  T_list_auto(false, true);//菜品宽度自适应
                });
                
                //2级分类显示
                var item = $(this).closest('.CategoryListTab').find('.layui-tab-content .layui-tab-item').eq(index);
                item.addClass('layui-show').siblings('.layui-tab-item').removeClass('layui-show');
                item.find('a').eq(0).addClass('layui-this').siblings().removeClass('layui-this');
            })
            
            //监听  套餐  菜品分类一 点击
            $(document).on('click','#TC_CategoryListTab .tabList li',function(){
            	$(this).addClass('layui-this').siblings().removeClass('layui-this');
//          	if($(this).hasClass('layui-this'))return false;
				var index = $(this).index();
				var newsArr = [];
                var CategoryList = inidata.CategoryList[index];
                if (CategoryList.ChildList.length > 0) { //有子分类
                    for (var i = 0; i < CategoryList.ChildList.length; i++) {
                        classid = CategoryList.ChildList[i].Id;
                        for (var j = 0; j < inidata.Projects.length; j++) {
                            var item = inidata.Projects[j];
                            if (classid == item.Category) { //成立
                                newsArr.push(item);
                            }
                        }
                    }
                    $('#CategoryList_view .layui-tab-content .layui-tab-item.layui-show .class-group a.layui-btn').eq(0).addClass('layui-this').siblings('a').removeClass('layui-this');
                } else { //没有子分类的
                    for (var j = 0; j < inidata.Projects.length; j++) {
                        var item = inidata.Projects[j];
                        if (CategoryList.Id == item.Category) { //成立
                            newsArr.push(item);
                        }
                    }
                }
                var ProjectsHtml = getProjectsHtml(newsArr);
                $('#Tc_ProjectsLists ul').html(ProjectsHtml);
                
                //2级分类显示
                var item = $(this).closest('#TC_CategoryListTab').find('.layui-tab-content .layui-tab-item').eq(index);
                item.addClass('layui-show').siblings('.layui-tab-item').removeClass('layui-show');
                item.find('a').eq(0).addClass('layui-this').siblings().removeClass('layui-this');
            })
            
            

            //监听二级分类点击
            $('#CategoryList_view .class-group').delegate('a.layui-btn', 'click', function (event) {
                //var Projectlists=$('#ProjectAndDetails_view li');
                var classno = $(this).parent('.class-group').parent('.layui-tab-item').index();
                $(this).addClass('layui-this').siblings('a').removeClass('layui-this');
                var newsArr = [];
//              if (classno == 0) { //全部--全部
//                  newsArr = inidata.ProjectAndDetails;
//              } else {
                    var btnno = $(this).index();
                    if (btnno == 0) { //分类下的全部
                        $('#CategoryList_view .tabList .layui-this').click();
                        return false;
                    } else {
                        var classdata = inidata.CategoryList[classno];
                        var classid = classdata.ChildList[btnno-1].Id;
                        for (var j = 0; j < inidata.ProjectAndDetails.length; j++) {
                            var item = inidata.ProjectAndDetails[j];
                            if (classid == item.Category) { //成立
                                newsArr.push(item);
                            }
                        }
                        var getTpl = ProjectAndDetails_tpml.innerHTML,
                            view = document.getElementById('ProjectAndDetails_view');
                        laytpl(getTpl).render(newsArr, function (html) {
                            view.innerHTML = html;
                        });
//                      T_list_auto(false, true);
                    }
//              }
            })
            
			//点菜菜品窗体  自适应
            projectAndDetailsAuto();
            T_list_auto(true, true);
            
            //已选菜品 窗体 高度
            orderContentH = winH - $('.Panel-side.left > .order-total').outerHeight() - $('.Panel-side.left > .order-head').outerHeight() - $('.Panel-side.left > .layui-table-header').outerHeight() - 2;
            
            
            
            //根据是否在操作状态  判断行为
			if(inidata.OrderAndTables.IsControl){ //如果是锁定状态
				//初始化动作全部完成时
				$('#loading').remove();
			}else{  //如果不是锁定状态
				$.ajax({
			        type: "post",
			        url: "/Res/Order/UpdateOrderTableIsControl",
			        dataType: "json",
			        data: {ordertableIds: OrderTableIds,isControl: true},
			        success: function (data, textStatus) {
			            if (data.Data == true) {
			                var chat = $.connection.systemHub;
							chat.hubName = 'systemHub';
							chat.connection.start();
							$.connection.hub.start().done(function() {
								chat.server.notifyResServiceRefersh(true);
			                	//初始化动作全部完成时
        						$('#loading').remove();
							});
//							$('#loading').remove();
			            } else {
			                layer.alert(data.Message);
			            }
			        },
			    });
			}
        },
        error: function (msg) {
            console.log(msg.responseText);
        }
    });

    //提交
//  form.on('submit(AddOrder)', function (data) {
//      var chat = $.connection.systemHub;
//      chat.hubName = 'systemHub';
//      chat.connection.start();
//      var name = data.elem.name;
//      if (name == 'Print') { //落单打厨
//          print = 2;
//      } else if (name == 'NoPrint') { //落单不打厨
//          print = 1;
//      } else if (name == 'Keep') { //保存
//          print = 0;
//      }
//
//      for (var i = 0; i < OrderTableProjectsdata.length; i++) {
//          if (OrderTableProjectsdata[i].Id == 0) {
//              OrderTableProjectsdata[i].CyddMxStatus = print;
//          }
//      }
//      //判断套餐是否为空
//      for(var i = 0; i < OrderTableProjectsdata.length; i++){
//      	if(OrderTableProjectsdata[i].CyddMxType == 2 && OrderTableProjectsdata[i].PackageDetailList.length < 1){
//      		layer.msg('套餐 （ ' + OrderTableProjectsdata[i].CyddMxName + " ） 中必须选择至少一个菜品");
//      		return false;
//      	}
//      }
//      
//
//      var para = {
//          req: OrderTableProjectsdata,
//          orderTableIds: OrderTableIds,
//          status: print
//      };
//      $.ajax({
//          type: "post",
//          url: "/Res/Home/OrderDetailCreate",
//          data: JSON.stringify(para),
//          contentType: "application/json; charset=utf-8",
//          dataType: "json",
//          beforeSend: function (xhr) {
//	            layindex = layer.open({type: 3});
//	        },
//	        complete: function (XMLHttpRequest, textStatus) {
//	            layer.close(layindex);
//	        },
//          success: function (data, textStatus) {
//              if (data.Data == true) {
//                  $.connection.hub.start().done(function () {
//                      chat.server.notifyResServiceRefersh(true);
//                  });
//                  if (OrderTableIds.length > 1) {
//                      layer.confirm('提交完成', {
//                          btn: ['确定'] //按钮
//                      }, function () {
//                          parent.layer.closeAll();
//                      });
//                  } else {
//                      layer.confirm('提交完成', {
//                          btn: ['继续操作', '退出'] //按钮
//                      }, function () {
//                      	layer.open({type: 3,shadeClose: false});
//                          location.reload();
//                      }, function () {
//                          history.go(-1);
//                      });
//                  }
//              } else {
//                  layer.alert(data["Message"]);
//              }
//          }
//      })
//      return false; //阻止表单跳转。如果需要表单跳转，去掉这段即可。
//  });
});

//落单打厨	提交之前
function AddOrderBefore(name){
	if(name == 'Print') { //落单打厨
		print = 2;
	} else if(name == 'NoPrint') { //落单不打厨
		print = 1;
	} else if(name == 'Keep') { //保存
		print = 0;
	}

	for(var i = 0; i < OrderTableProjectsdata.length; i++) {
		if(OrderTableProjectsdata[i].Id == 0) {
			OrderTableProjectsdata[i].CyddMxStatus = print;
		}
	}
	
	//判断套餐是否为空
    for(var i = 0; i < OrderTableProjectsdata.length; i++){
    	if(OrderTableProjectsdata[i].CyddMxType == 2 && OrderTableProjectsdata[i].PackageDetailList.length < 1){
    		layer.msg('套餐 （ ' + OrderTableProjectsdata[i].CyddMxName + " ） 中必须选择至少一个菜品");
    		return false;
    	}
    }
    
    var para = {
		req: OrderTableProjectsdata,
		orderTableIds: OrderTableIds,
		status: print
	};
	
	
	if(name == 'Print' && inidata.OrderDetailTest){
		layindex = layer.open({type: 3});
		$.ajax({
			type: "post",
			url: "/Res/Project/OrderDetailPrintTesting",
			data: JSON.stringify(para),
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			beforeSend: function (xhr) {
	        },
	        async:false,
	        complete: function (XMLHttpRequest, textStatus) {
	            layer.close(layindex);
	        },
			success: function(data, textStatus) {
				if(data.Data.length > 0) {
					var $tr = '';
					for(var i=0;i<data.Data.length;i++){
						$tr += '<tr>' +
								'<td>' + data.Data[i].Name + '</td>' +
								'<td width="13%"><div class="tc">' + data.Data[i].StallName + '</div></td>' +
								'<td width="20%"><div class="tc">' + data.Data[i].IpAddress + '</div></td>' +
								'<td width="20%"><div class="tc">' + data.Data[i].Remark + '</div></td>' +
							'</tr>';
					}
					var str = '<div style="padding:10px;position:relative;overflow:hidden;">'+
						'<table class="layui-table layui-table-header table-head" lay-skin="line" style="margin: 0;">'+
			                '<thead>'+
			                    '<tr>'+
			                        '<th width="">打印机</th>'+
			                        '<th width="13%"><div class="tc">档口名称</div></th>'+
			                        '<th width="20%"><div class="tc">IP地址</div></th>'+
			                        '<th width="20%"><div class="tc">状态</div></th>'+
			                    '</tr>'+
			                '</thead>'+
			            '</table>'+
			            '<div class="orderScrollBtn" style="top:10px;">'+
			            	'<div class="layui-btn layui-btn-normal scrollTopBtn"><i class="layui-icon">&#xe619;</i></div>'+
			            	'<div class="layui-btn layui-btn-normal scrollBottomBtn"><i class="layui-icon">&#xe61a;</i></div>'+
			            '</div>'+
			            '<div class="order-content sm-scroll-hidden" style="max-height:330px;position: initial;">'+
			                '<table class="layui-table" lay-skin="line" style="margin:0;">'+
			                    '<tbody id="InitCookOrder_lists">' + $tr + '</tbody>'+
			                '</table>'+
			            '</div>'+
			        '</div>';
			        layer.open({
						type: 1,
						title: '打印机状态',
						shadeClose: true,
						skin: 'layer-header layer-form-group',
						shade: 0,
						area: ['600px', '500px'],
						content: str,
						btn: ['确定', '取消'],
						yes: function(index, layero) {
							AddOrderBeforeType(para)
						},
						btn2: function(index, layero) {}
					});
				}else{
					AddOrderBeforeType(para)
				}
			}
		})
	}else{
		AddOrderBeforeType(para)
	}
}

//落单打厨	||  落单不打厨	|| 保存	提交前判断即起叫起
function AddOrderBeforeType(para){
	if(inidata.DefaultPromptly || para.status != 2){
		AddOrderSubmit(para)
	}else{
		layer.open({
			type: 1,
			title: false,
			shadeClose: true,
			closeBtn: 0,
			skin: 'layer-header layer-form-group',
			area: ['220px', '85px'],
			content: '<div style="padding:20px"><a href="javascript:;" class="layui-btn layui-btn-normal layui-btn-lg" data-type="1">即起</a><a href="javascript:;" class="layui-btn layui-btn-lg" data-type="2">叫起</a></div>',
			success: function(layero,index){
				$(layero).find('a').on('click',function(){
					var DishesStatus = $(this).attr('data-type');
					for(var i=0;i<para.req.length;i++){
						if(para.req[i].Id == 0){
							para.req[i].DishesStatus = DishesStatus
						}
					}
					layer.close(index)
					AddOrderSubmit(para)
				})
			}
		});
	}
}

//落单打厨	||  落单不打厨	|| 保存	提交
function AddOrderSubmit(para){
	layindex = layer.open({type: 3});
	$.ajax({
		type: "post",
		url: "/Res/Home/OrderDetailCreate",
		data: JSON.stringify(para),
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		beforeSend: function (xhr) {
        },
        async:false,
        complete: function (XMLHttpRequest, textStatus) {
            layer.close(layindex);
        },
		success: function(data, textStatus) {
			if(data.Data == true) {
				//var chat = $.connection.systemHub;
				//chat.hubName = 'systemHub';
				//chat.connection.start();
				
				//$.connection.hub.start().done(function() {
				//	chat.server.notifyResServiceRefersh(true);
				//});
				if(OrderTableIds.length > 1) {
					layer.confirm('提交完成', {
						btn: ['确定'] //按钮
					}, function() {
						cancelOut()
					});
				} else {
					layer.confirm('提交完成', {
						btn: ['继续操作', '退出'], //按钮
						closeBtn:0
					}, function() {
						layer.open({type: 3,shadeClose: false});
						location.reload();
					}, function() {
						cancelOut()
					});
				}
			} else {
				layer.alert(data["Message"]);
			}
		}
	})
}

//搜索检索
function KeyWord(value) {
    var newsArr = [];
    if (!value) {
        newsArr = inidata.ProjectAndDetails;
    } else {
        for (var i = 0; i < inidata.ProjectAndDetails.length; i++) {
            var item = inidata.ProjectAndDetails[i];
            if(item.Name.indexOf(value) >= 0){
				newsArr.push(item);
			}else if(item.CharsetCodeList) { //存在 code   
				//拼接 所有code
				var code = '';
				for(var j = 0; j < item.CharsetCodeList.length; j++) {
					code += item.CharsetCodeList[j].Code.toUpperCase();
				}
				if(code.indexOf(value) >= 0) { //成立
					newsArr.push(item);
				}
			}
        }
    }
    var getTpl = ProjectAndDetails_tpml.innerHTML,
        view = document.getElementById('ProjectAndDetails_view');
    laytpl(getTpl).render(newsArr, function (html) {
        view.innerHTML = html;
    });
    T_list_auto(false, true);
}

//搜索套餐检索
function TcKeyWord(value) {
    var newsArr = [];
    if (!value) {
        newsArr = inidata.Projects;
    } else {
        for (var i = 0; i < inidata.Projects.length; i++) {
            var item = inidata.Projects[i];
            if(item.ProjectName.indexOf(value) >= 0){
				newsArr.push(item);
			}else if (item.CharsetCodeList) { //存在 code
                //拼接 所有code
                var code = '';
                for (var j = 0; j < item.CharsetCodeList.length; j++) {
                    code += item.CharsetCodeList[j].Code.toUpperCase();
                }
                if (code.indexOf(value) >= 0) { //成立
                    newsArr.push(item);
                }
            }
        }
    }
    var ProjectsHtml = getProjectsHtml(newsArr);
    $('#Tc_ProjectsLists ul').html(ProjectsHtml);
    T_list_auto(false, true);
}

//检索内容  上下左右切换
function search_input(ele, callback, marginBottom) {
    $(ele).on('focus', function () {
        $(ele).off('input propertychange');
        $(this).on('input propertychange', function (e) {
            var value = $(this).val().toUpperCase();
            callback && callback(value)
        })
    })
}

//套餐 检索input 事件
function tcSearch_input(ele, callback, obj) {
//  var headHeight = $('#TC_CategoryListTab').outerHeight();
    //ele 监听对象  callback 回调  obj 滚动对象
    $(ele).on('focus', function () {
        $(this).off('input propertychange');
        $(this).on('input propertychange', function (e) {
            var value = $(this).val().toUpperCase();
            callback && callback(value)
        })
    })

//  $(ele).focus();
}

//套餐筛选
function Package() {
    var newsArr = [];
    for (var i = 0; i < inidata.ProjectAndDetails.length; i++) {
        var item = inidata.ProjectAndDetails[i];
        if (item.CyddMxType == 2) { //套餐
            newsArr.push(item);
        }
    }

    var getTpl = ProjectAndDetails_tpml.innerHTML,
        view = document.getElementById('ProjectAndDetails_view');
    laytpl(getTpl).render(newsArr, function (html) {
        view.innerHTML = html;
    });

//  T_list_auto(false, true);
}

//点菜
function AddProject() {
    $('#ProjectAndDetails_view').delegate('li a', 'click', function (event) {
        var $li = $(this).parent('li');

        $li.addClass('layui-this').siblings().removeClass('layui-this');
//      $('#KeyWord').focus();

        if ($li.hasClass('disabled')) {
            layer.msg('该菜品库存为空!');
            return false;
        }
        layer.closeAll('page');
        var CyddMxType = $li.attr('data-CyddMxType');
        var id = $li.attr('data-id');
        var itemdata = '';
        var inidataCopy = {};
        $.extend(true, inidataCopy, inidata);
        for (var i = 0; i < inidataCopy.ProjectAndDetails.length; i++) {
            var item = inidataCopy.ProjectAndDetails[i];
            if (item.CyddMxType == CyddMxType && item.Id == id) {
                itemdata = item;
            }
        }

        //添加数组到已选菜单
        AddSelect(itemdata);
        //选中当前
        thisProjectsIndex = OrderTableProjectsdata.length - 1;
        thisOrderProjectArr = OrderTableProjectsdata[thisProjectsIndex];
        //当前菜品数组
        thisProjectArr = itemdata;
        //更新订单/统计金额
        NewsOrder();
        
        
        //弹出做法弹窗
        ProjectLayer();
        //按钮权限
        ProjectPower();
        
        //如果是手写菜 直接弹出
        if (itemdata.IsCustomer > 0){
        	EditName(this, '1')
        	return false;
        }

        if (itemdata.ProjectDetailList && itemdata.CyddMxType == 1) {
            if (itemdata.ProjectDetailList[0].Price <= 0 && itemdata.IsChangePrice > 0) { //默认单位  价格为0，并且允许改价==弹出改价输入
                var dom = $('#operation_lists li').eq(7).find('a');
                NumberKeyboard('editprice', dom);
            }
        }

        if (itemdata.CyddMxType == 2) { //套餐
            if (itemdata.Price <= 0 && itemdata.IsChangePrice > 0) { //默认单位  价格为0，并且允许改价==弹出改价输入
                var dom = $('#operation_lists li').eq(7).find('a');
                NumberKeyboard('editprice', dom);
            }
        }

    })
}

function AddSelect(pro) {
    if (pro.CyddMxType == 1) {
        var detail = pro.ProjectDetailList[0];
        OrderTableProjectsdata.push({
            CyddTh: OrderTableIds,
            CyddMxType: pro.CyddMxType,
            CyddMxId: detail.Id,
            Price: detail.Price,
            Num: 1,
            CyddMxStatus: 0,
            ProjectName: pro.Name,
            Unit: detail.Unit,
            Id: 0,
            R_Project_Id: pro.Id,
            CyddMxName: pro.Name,
            ProjectDetailList: [pro.ProjectDetailList[0]],
            CostPrice: pro.CostPrice,
            IsChangePrice: pro.IsChangePrice,
            IsChangeNum: pro.IsChangeNum,
            IsCustomer: pro.IsCustomer,
            IsUpdataPrice: false,
            IsGive: pro.IsGive,
            OrderDetailRecordCount: [],
            DishesStatus: allStatus,
            Extend: [],
            OrderDetailRecord: pro.OrderDetailRecord
        });
    } else {
        OrderTableProjectsdata.push({
            CyddTh: OrderTableIds,
            CyddMxId: pro.Id,
            CyddMxType: pro.CyddMxType,
            Price: pro.Price,
            Num: 1,
            CyddMxStatus: 0,
            ProjectName: pro.Name,
            Unit: "份",
            Id: 0,
            R_Project_Id: pro.Id,
            CyddMxName: pro.Name,
            CostPrice: pro.CostPrice,
            IsCustomer: pro.IsCustomer,
            IsChangePrice: pro.IsChangePrice,
            IsUpdataPrice: false,
            OrderDetailRecordCount: [],
            DishesStatus: allStatus,
            Extend: [],
            PackageDetailList: pro.PackageDetailList,
            IsGive: 0,
            OrderDetailRecord: pro.OrderDetailRecord
        });
    }
}

//点击做法/要求/配菜/单位

function ProjectClick() {
    $('.practice-box .practice-lists').delegate('li', 'click', function (event) {
        var type = $(this).attr('data-type');
        var no = $(this).attr('data-no');
        var tr = $('#ProjectLists_view tr.layui-this');
        if (type == 4) { //修改单位
            var clickdata = thisProjectArr.ExtendList[no];
            $(this).addClass('layui-this').siblings('li').removeClass('layui-this');
            var clickdata = thisProjectArr.ProjectDetailList[no];
            //修改 已选菜品单位数组
            thisOrderProjectArr.ProjectDetailList = [];
            thisOrderProjectArr.ProjectDetailList.push(clickdata);
            //修改已选菜品单价
            thisOrderProjectArr.Price = clickdata.Price;
            thisOrderProjectArr.CyddMxId = clickdata.Id;
            thisOrderProjectArr.Unit = clickdata.Unit;
            if (thisOrderProjectArr.ProjectDetailList) {
                if (thisOrderProjectArr.ProjectDetailList[0].Price <= 0 && thisOrderProjectArr.IsChangePrice > 0) { //默认单位  价格为0，并且允许改价==弹出改价输入
                    var dom = $('#operation_lists li').eq(7).find('a');
                    NumberKeyboard('editprice', dom);
                }
            }
        } else {
            if (no == 'more') { //更多 做法/要求/配菜
                openExtendMore('Project', type);
            } else {
                var clickdata = thisProjectArr.ExtendList[no];
                var ClickExtend = thisOrderProjectArr.Extend;
                if ($(this).hasClass('layui-this')) { //取消 做法/要求/配菜
                    $(this).removeClass('layui-this');
                    var newExtend = [];
                    for (var i = 0; i < ClickExtend.length; i++) {
                        if (ClickExtend[i].Id != clickdata.Id && ClickExtend[i].ProjectExtendName != clickdata.ProjectExtendName) {
                            newExtend.push(ClickExtend[i]);
                        }
                    }
                    thisOrderProjectArr.Extend = newExtend;
                } else { //添加 选择
                    //添加 做法、要求、配菜的数组
                    thisOrderProjectArr.Extend.push(clickdata);
                    $(this).addClass('layui-this');
                }
            }
        }
        //更新订单/统计金额
        NewsOrder();
    })
}

//已选菜品选中

$('#ProjectLists_view').delegate('tr', 'click', function (event) {
    if (inidata.OrderAndTables.IsLock == true) { //锁定
        return false;
    }
    layer.closeAll('page');
    //	 if ($(this).hasClass('layui-this')) {
    //	 	return false;
    //	 }

    $(this).addClass('layui-this').siblings('tr').removeClass('layui-this');
    var id = parseFloat($(this).attr('data-id'));
    var CyddMxType = parseFloat($(this).attr('data-CyddMxType'));
    var thisdata;
    var thisno = 0;
    for (var i = 0; i < inidata.ProjectAndDetails.length; i++) {
        if (id == inidata.ProjectAndDetails[i].Id && CyddMxType == inidata.ProjectAndDetails[i].CyddMxType) {
            thisdata = inidata.ProjectAndDetails[i];
            thisno = i;
        }
    }

    //当前菜单选中菜品的数组
    var Projectdom = $('#ProjectLists_view tr.layui-this');
    var ProjectListsno = Projectdom.index();
    var ProjectListsarr = OrderTableProjectsdata[ProjectListsno];
    thisProjectsIndex = ProjectListsno;
    thisOrderProjectArr = ProjectListsarr;
    //当前菜品数组
    thisProjectArr = thisdata;

    if (thisOrderProjectArr.CyddMxType == 2) { //套餐--显示所含菜品
        ProjectLayer();
    }
    ProjectPower();

})

/**
 * 弹出做法/要求/配菜更多选项
 * @param {Object} type   处理类型   All==全单 ；   Project当前选中菜品
 * @param {Object} tabno  默认打开tab
 */
function openExtendMore(type, tabno) {
	
    var content1 = '',
        content2 = '',
        content3 = '';
    if (type == 'All') { //全单操作
        var is = false;
        for (var i = 0; i < OrderTableProjectsdata.length; i++) {
            if (OrderTableProjectsdata[i].Id <= 0 || OrderTableProjectsdata[i].CyddMxStatus == 0 && OrderTableProjectsdata[i].CyddMxType == 1) { //存在新菜品
                is = true;
            }
        }
        if (!is) {
            layer.msg('订单中没有可操作的菜品!');
            return false;
        }
    }
    //做法--可选项

    var moreExtendData = [];
    $.extend(true, moreExtendData, inidata.ProjectExtendSplitList);
    //数据渲染
    content1 += "<div class='layui-tab-content' data-num='1'><div class='t-select-tab-lists class-group'><a href='javascript:void(0);' class='layui-btn layui-btn-primary layui-this'>全部</a>";
    content2 += "<div class='layui-tab-content' data-num='2'><div class='t-select-tab-lists class-group'><a href='javascript:void(0);' class='layui-btn layui-btn-primary layui-this'>全部</a>";
    content3 += "<div class='layui-tab-content' data-num='3'><div class='t-select-tab-lists class-group'><a href='javascript:void(0);' class='layui-btn layui-btn-primary layui-this'>全部</a>";
    for (var i = 0; i < moreExtendData.length; i++) {
        //获取当前已经选择的 做法 | 要求 | 配菜
        var list = moreExtendData[i].ProjectExtendDTOList;
        for (var j = 0; j < list.length; j++) {
            list[j].isCheck = false; //首先统一设置为不选中
            if (type != 'All' && thisOrderProjectArr.Extend) {
                for (var o = 0; o < thisOrderProjectArr.Extend.length; o++) {
                    if (thisOrderProjectArr.Extend[o].Id == list[j].Id) { //选中当前选项
                        list[j].isCheck = true;
                    }
                }
            }
        }
        //获取二级分类
        var arr = moreExtendData[i]
        switch (arr.ExtendType) {
            case 1:
                content1 += '<a class="layui-btn layui-btn-primary" data-id=' + arr.Id + '>' + arr.Name + '</a>'
                break;
            case 2:
                content2 += '<a class="layui-btn layui-btn-primary" data-id=' + arr.Id + '>' + arr.Name + '</a>'
                break;
            case 3:
                content3 += '<a class="layui-btn layui-btn-primary" data-id=' + arr.Id + '>' + arr.Name + '</a>'
                break;
            default:
        }
    }
    var b = "</div><ul class='MealTable-lists t-select-lists select-lists' style='margin-top:10px;'></ul></div>";
    content1 += b;
    content2 += b;
    content3 += b;

    layer.tab({
        area: ['800px', '80%'],
        skin: 'layer-form-group layui-layer-tab layer-tab moreExtend',
        btn: ['确定', '取消'],
        yes: function (index, layero) {
            //获得选中的做法/要求/配菜数组
            var clickArr = [];
            for (var i = 0; i < moreExtendData.length; i++) { //循环获取选中的数据
                var list = moreExtendData[i].ProjectExtendDTOList;
                for (var j = 0; j < list.length; j++) {
                    if (list[j].isCheck) {
                        clickArr.push(list[j]);
                    }
                }
            }
            if (clickArr.length < 1) {
                layer.msg('请选择!');
                return false;
            }
            if (type == 'All') { //全单操作
                //插入到所有已点菜品数组
                for (var i = 0; i < OrderTableProjectsdata.length; i++) {
                    if (OrderTableProjectsdata[i].Id <= 0 || OrderTableProjectsdata[i].CyddMxStatus == 0 && OrderTableProjectsdata[i].CyddMxType == 1) { //存在的非套餐的新菜品
                        OrderTableProjectsdata[i].Extend = clickArr;
                    }
                }
            } else { //当前选中菜品操作
                //插入到当前选中菜品数组
                thisOrderProjectArr.Extend = clickArr;
            }
            //更新订单/统计金额
            NewsOrder();
            layer.closeAll('page');
        },
        btn2: function (index, layero) { },
        tab: [{
            title: '做法',
            content: content1
        }, {
            title: '要求',
            content: content2
        }, {
            title: '配菜',
            content: content3
        }],
        success: function (layero, index) {
            for (var i = 0; i < 3; i++) {
                extendMoreFilterList(layero, i, i + 1)
            }
        }
    });

    //二级分类点击
    $(document).off('click', '.t-select-tab-lists a');
    $(document).on('click', '.t-select-tab-lists a', function () {
        $(this).addClass('layui-this').siblings().removeClass('layui-this')
        var layero = $(this).closest('.layui-layer-tabmain');
        var n = parseInt($(this).closest('.layui-tab-content').attr('data-num'));
        var id = $(this).attr('data-id');
        extendMoreFilterList(layero, n - 1, n, id)
    })

    //监听选中点击
    $(document).off('click', '.t-select-lists li');
    $(document).on('click', '.t-select-lists li', function () {
        var dType = $(this).attr('data-type');
        var dId = $(this).attr('data-id');
        if ($(this).hasClass('checked')) {
            $(this).removeClass('checked');
            var isCheck = false;
        } else {
            $(this).addClass('checked');
            var isCheck = true;
        }
        for (var i = 0; i < moreExtendData.length; i++) {
            if (moreExtendData[i].ExtendType == dType) {
                var list = moreExtendData[i].ProjectExtendDTOList
                for (var j = 0; j < list.length; j++) {
                    if (dId == list[j].Id) {
                        list[j].isCheck = isCheck;
                    }
                }
            }
        }
    })

    //更多选择 二级分类 根据按钮筛选内容
    function extendMoreFilterList($layero, n, filterType, filterId) {
        var list_id = filterId || "all";
        var listCon = "";
        for (var i = 0; i < moreExtendData.length; i++) {
            if (moreExtendData[i].ExtendType === filterType && (moreExtendData[i].Id == list_id || list_id === "all")) {
                var list = moreExtendData[i].ProjectExtendDTOList;
                for (var j = 0; j < list.length; j++) {
                    var checked = '';
                    if (list[j].isCheck) checked = 'class="checked"'; //判断是否选中
                    listCon += '<li data-type="' + list[j].ExtendType + '" data-type-id="' + moreExtendData[i].Id + '" data-id="' + list[j].Id + '" ' + checked + '>' +
                        '<div class="MealTable-title" style="line-height: 30px;height: 30px;font-size:16px;">' + list[j].ProjectExtendName + '</div>' +
                        '<div class="MealTable-footer"><span class="MealTable-price">' + list[j].Price + '</span> </div>' +
                        '</li>';
                }
            }
        }
        $layero.find('.MealTable-lists').eq(n).html("").append(listCon)
    }
}

//选中的菜品可操作的相关权限
function ProjectPower() {
    if (inidata.OrderAndTables.IsLock == true) {
        operation = [];
    } else {
        if (thisOrderProjectArr.Id == 0) { //新增的菜品
            var operation = [0, 1, 2]; //新增的按钮权限
            if (thisOrderProjectArr.IsChangePrice > 0) { //允许改价
                operation.push(3);
            }
            if (thisOrderProjectArr.CyddMxType == 1) { //允许做法
                operation.push(4);
            }
            if (thisOrderProjectArr.IsGive > 0) { //允许赠送
                operation.push(6);
            }
            if (thisOrderProjectArr.IsCustomer > 0) { //允许手写
                operation.push(7);
            }
        } else {
            if (thisOrderProjectArr.CyddMxStatus == 0) { //保存
                var operation = [0, 1, 2];
                if (thisOrderProjectArr.IsChangePrice > 0) { //允许改价
                    operation.push(3);
                }
                if (thisOrderProjectArr.CyddMxType == 1) { //允许做法
                    operation.push(4);
                }
                if (thisOrderProjectArr.IsGive > 0) { //允许赠送
                    operation.push(6);
                }
                if (thisOrderProjectArr.IsCustomer > 0) { //允许手写
                    operation.push(7);
                }
            } else {
                var operation = [5]; ///打厨/未打厨
                if (thisOrderProjectArr.CyddMxType == 2 && thisOrderProjectArr.IsChangePrice > 0) { //允许改价
                    operation.push(3);
                }
                if (thisOrderProjectArr.IsChangeNum > 0) {
                    operation.push(0);
                    operation.push(1);
                    operation.push(2);
                }
                if (thisOrderProjectArr.IsGive > 0) {
                    operation.push(6);
                }
            }
        }
    }

    var operation_lists = $('#operation_lists li.operation_item');
    for (var i = 0; i < operation_lists.length; i++) {
        var isoperation = $.inArray(i, operation);
        if (isoperation >= 0) {
            operation_lists.eq(i).find('a').removeClass('Disable');
        } else {
            operation_lists.eq(i).find('a').addClass('Disable');
        }
    }
}

//弹出菜品 做法/要求/配菜/单位
function ProjectLayer(thisdom) {
    layer.closeAll('page');
    var Projectdom = $('#ProjectLists_view tr.layui-this');
    if (Projectdom.length < 1) {
        layer.msg('请选择要操作的菜品');
        return false;
    }

    if ($(thisdom).hasClass('Disable')) {
        return false;
    }

    //套餐阻止弹出做法，显示弹出内容
    if (thisOrderProjectArr.CyddMxType == 2) {
        var hnum = 'auto';
        var Projecthtml = '';
        for (var i = 0; i < thisOrderProjectArr.PackageDetailList.length; i++) {
            var item = thisOrderProjectArr.PackageDetailList[i];
            Projecthtml += '<tr>' +
                '<td width="60%">' + item.Name + ' </td>' +
                '<td>' + item.Num + '</td>';
            if (thisOrderProjectArr.Id <= 0 || thisOrderProjectArr.CyddMxStatus <= 0) { //新增
                Projecthtml += '<td width="20%"><i class="layui-icon del-icon" onclick="tcItemDelete(this)" data-id="' + item.R_ProjectDetail_Id + '">&#x1006;</i></td>'
            }
            Projecthtml += '</tr>';
        }
        if (thisOrderProjectArr.Id <= 0 || thisOrderProjectArr.CyddMxStatus <= 0) { //新增
            var btnText = '<a href="javascript:void(0);" class="layui-btn" onclick="EditTc();">修改套餐</a>';
        }
        var html = '<div class="MealTable-lists box-sm" style="margin-right: 0;padding:0 10px 10px;">' +
            '<blockquote class="label-title">套餐所含菜品</blockquote>'
        html += btnText || "";
        html += '<table class="layui-table layui-table-header" style="margin:6px 0 0;" lay-skin="line">' +
            '<thead>' +
            '<th  width="60%">菜品名称</th>' +
            '<th>数量</th>';
        if (thisOrderProjectArr.Id <= 0 || thisOrderProjectArr.CyddMxStatus <= 0) { //新增
            html += '<th>操作</th>'
        }
        
        html += '</thead>' +
            '</table style="background: #fff;">' +
            '<div class="layui-table-body" style="max-height:'+(orderContentH/2 - 100) +'px; overflow-y:auto;">' +
            '<table class="layui-table" style="margin:0" lay-skin="line">' +
            '<tbody>' + Projecthtml + '</tbody>' +
            '</table>' +
            '</div>' +
            '</div>';
    } else {
        //单位
        var Detaildata = thisProjectArr.ProjectDetailList;

        var Detaillist = '';
        var Detail_box = '';
        var hnum = 0;
        if (Detaildata) {
            for (var i = 0; i < Detaildata.length; i++) {
                Detaillist += '<li data-type="4" data-no="' + i + '"><h4>' + Detaildata[i].Unit + '</h4><p>' + Detaildata[i].UnitRate + '</p></li>';
            }
        }

        //做法+要求+配菜
        var practicedata = thisProjectArr.ExtendList;
        var practicelist = '';
        var askedlist = '';
        var garnishlist = '';
        var practice_box = '';
        var asked_box = '';
        var garnish_box = '';
        if (practicedata) {
            for (var i = 0; i < practicedata.length; i++) {
                var isthis = '';
                for (var j = 0; j < thisOrderProjectArr.Extend.length; j++) {
                    if (thisOrderProjectArr.Extend[j].ExtendType == practicedata[i].ExtendType && thisOrderProjectArr.Extend[j].Id == practicedata[i].Id) {
                        isthis = 'class="layui-this"';
                    }
                }
                var price = practicedata[i].Price ? '￥' + practicedata[i].Price : '';
                if (practicedata[i].ExtendType == 1) {
                    practicelist += '<li data-type="1" data-no="' + i + '" ' + isthis + '><h4>' + practicedata[i].ProjectExtendName + '</h4><p>' + price + '</p></li>';
                } else if (practicedata[i].ExtendType == 2) {
                    askedlist += '<li  data-type="2" data-no="' + i + '" ' + isthis + '><h4>' + practicedata[i].ProjectExtendName + '</h4><p>' + price + '</p></li>';
                } else if (practicedata[i].ExtendType == 3) {
                    garnishlist += '<li  data-type="3" data-no="' + i + '" ' + isthis + '><h4>' + practicedata[i].ProjectExtendName + '</h4><p>' + price + '</p></li>';
                }
            }
        }
        if (practicelist) {
            practice_box = '<div class="practice-item clearfix">' +
                '<div class="practice-title">做法</div>' +
                '<ul class="practice-lists clearfix" style="padding-right:50px;">' + practicelist +
                '<li data-type="1" data-no="more" ><h4>更多</h4></li>' +
                '</ul>' +
                '</div>';
            hnum++;
        }
        if (askedlist) {
            asked_box = '<div class="practice-item clearfix">' +
                '<div class="practice-title">要求</div>' +
                '<ul class="practice-lists">' + askedlist +
                '<li data-type="2" data-no="more" ><h4>更多</h4></li>' +
                '</ul>' +
                '</div>';
            hnum++;
        }
        if (garnishlist) {
            garnish_box = '<div class="practice-item clearfix">' +
                '<div class="practice-title">配菜</div>' +
                '<ul class="practice-lists">' + garnishlist +
                '<li data-type="3" data-no="more" ><h4>更多</h4></li>' +
                '</ul>' +
                '</div>';
            hnum++;
        }
        var practicehtml = practice_box + asked_box + garnish_box;
        if (!practicehtml) {
            Detaillist += '<li data-type="1" data-no="more" ><h4>更多</h4></li>';
        }
        if (Detaillist) {
            hnum++;
            Detail_box = '<div class="practice-item clearfix">' +
                '<div class="practice-title">单位</div>' +
                '<ul class="practice-lists">' + Detaillist + '</ul>' +
                '</div>';
        }
        var html = practice_box + asked_box + garnish_box + Detail_box;
    }
    var src = '<div class="practice-box"><a href="javascript:void(0);" onclick=layer.closeAll("page") class="practice-close"><i class="layui-icon">&#x1006;</i></a>' + html + '</div>';
    //防止过多出现滚动条
    //	if(hnum >= 0) {
    //		var height = (hnum * 62) + 'px';
    //	} else if(hnum == 'auto') {
    //		var height = 'auto';
    //	}
    var height = 'auto';
    layer.open({
        type: 1,
        shade: 0,
        title: false,
        closeBtn: 0,
        offset: ['455px', '0px'],
        skin: 'layer-practice ProjectLayer',
        area: ['398px', height],
        content: src,
        isOutAnim: false,
        success: function (layero, index) {
            //防止套餐弹出窗口出现滚动条
            if (thisOrderProjectArr.CyddMxType == 2) {
                var $layer_con = layero.find('.layui-layer-content');
                $layer_con.height($layer_con.height() + 1);
            }

            var orderContent = Projectdom.closest('.scroll-hidden');
            //设置已选菜单margin 使其永远在下方弹窗上面
            var orderContentH = orderContent.outerHeight();
            var ProjectLayerH = layero.height();//弹窗高度
            Projectdom.closest('.order-content').css('margin-bottom', ProjectLayerH)

            var thisDomH = Projectdom.height();
            var thisDomY = Projectdom.offset().top + thisDomH;
            var ProjectLayerY = layero.offset().top;
            if (thisDomY > ProjectLayerY) { //判断当前点击的在弹窗上面还是下面
                //在下面
                orderContent.scrollTop(Projectdom.get(0).offsetTop - orderContentH + ProjectLayerH + thisDomH)
            }
        },
        end: function () {
            var orderContent = $('#ProjectLists_view tr.layui-this').closest('.order-content');
            orderContent.css('margin-bottom', 0);
            orderContent.scrollTop(orderContent.scrollTop() - 1);//防止出现空白
        }
    });
    ProjectClick();
}

//套餐弹出列表  删除按钮事件
function tcItemDelete($this) {
    var index = $($this).closest('tr').index();
    var i = $('#ProjectLists_view tr.layui-this').index();
    OrderTableProjectsdata[i].PackageDetailList.splice(index, 1)
    $($this).closest('tr').remove();
}

//编辑套餐
function EditTc() {
    //隐藏页面滚动条
//  $('body').css('overflow', 'hidden');

    var Tabhtml = $('#CategoryList_view').html();
    Tabhtml = Tabhtml.replace('id="KeyWord"', 'id="TcKeyWord"')
    //分类
    var Headstr = '<div class="Panel-form right flex-item order-form" style="padding-top:0;height:100%;"><div class="ClassTab-head">' +
        '<div id="TC_CategoryListTab">' +
        Tabhtml + '</div>' +
        '</div>';
    //可选菜品  
    var ProjectsHtml = '<div class="MealTable-lists" style="margin-right:0;overflow: hidden;overflow-y: auto;height: calc(100% - 109px);" id="Tc_ProjectsLists"><ul>';
    
    //获取默认的第一个选中项 的 全部菜品
    
    ProjectsHtml += '</ul></div></div>';
	
	//套餐内菜品数据
    var TCthisArr = [];
	$.extend(true,TCthisArr,thisOrderProjectArr.PackageDetailList);
	
	
	//已选套餐菜品
    var ProjectsListsHtml = '<div class="tcChosen">'+
    	'<div class="MealTable-lists" style="margin-right: 0;padding: 0 10px;">' +
        	'<blockquote class="label-title">套餐所含菜品</blockquote>' +
        	'<div>'+
	        	'<table class="layui-table table-head layui-table-header" lay-skin="line" style="margin: 0;">'+
	        		'<colgroup>'+
                        '<col width="12%">'+
                        '<col width="40%">'+
                        '<col width="38%">'+
                        '<col width="10%">'+
                    '</colgroup>'+
	                '<thead>'+
	                    '<tr>'+
	                    	'<th></th>'+
	                        '<th class="tl">菜名</th>'+
	                        '<th>数量</th>'+
	                        '<th></th>'+
	                    '</tr>'+
	                '</thead>'+
	            '</table>'+
	            '<div class="table-body" style="max-height:'+(winH - 180)+'px;overflow-y:auto;">'+
                '<table class="layui-table" lay-skin="line" style="margin: 0;">'+
                	'<colgroup>'+
                        '<col width="12%">'+
                        '<col width="40%">'+
                        '<col width="38%">'+
                        '<col width="10%">'+
                    '</colgroup>'+
                    '<tbody id="select_tc"></tbody>'+
                '</table>'+
                '</div>'+
	        '</div>'+
    	'</div>'+
    '</div>';
    var str = Headstr + ProjectsHtml + ProjectsListsHtml;

    layer.open({
        type: 1,
        anim: -1,
        closeBtn: 0,
        title: '编辑套餐',
        //shadeClose: true,
        skin: 'layer-header layer-form-group EditTc',
        //shade: 0,
        area: ['100%', '100%'],
        content: str,
        btn: ['确定', '取消'],
        yes: function (index, layero) {
            if (TCthisArr < 1) {
                layer.msg('请选择套餐所含菜品!');
                return false;
            }
            thisOrderProjectArr.PackageDetailList = TCthisArr;
            layer.closeAll('page');

        },
        btn2: function (index, layero) { },
        success: function (layero, index) {
        	$('#TC_CategoryListTab .tabList li').eq(0).click();
        	
            element.init();

            //检索input
            tcSearch_input('#TcKeyWord', TcKeyWord, '#Tc_ProjectsLists');

        }
    });
    //更新套餐菜品
    NewTcProjects(TCthisArr);
    
    //菜品的选中--多选
    $('#Tc_ProjectsLists').delegate('li', 'click', function (event) {
        var thisli = $(this);
        var clickid = thisli.attr('data-id');
        thisli.addClass('layui-this').siblings().removeClass('layui-this');

        for (var i = 0; i < inidata.Projects.length; i++) {
            if (clickid == inidata.Projects[i].Id) {
                var IsCustomer = (inidata.Projects[i].IsCustomer > 0);
                var clickArrz = inidata.Projects[i];
                var Arr = {
                    R_ProjectDetail_Id: clickArrz.Id,
                    Name: clickArrz.ProjectName + '(' + clickArrz.Unit + ')',
                    Num: 1,
                    R_OrderDetail_Id: thisOrderProjectArr.Id,
                    IsCustomer: IsCustomer
                }
                TCthisArr.push(Arr);
            }
        }
        NewTcProjects(TCthisArr, 1)
    })
//
    //监听套餐二级分类点击
    $('#TC_CategoryListTab .class-group').delegate('a.layui-btn', 'click', function (event) {
        var classno = $(this).parent('.class-group').parent('.layui-tab-item').index();
        $(this).addClass('layui-this').siblings('a').removeClass('layui-this');
        var newsArr = [];
        var btnno = $(this).index();
        if (btnno == 0) { //分类下的全部
            $('#TC_CategoryListTab .layui-tab-title .layui-this').click();
            return false;
        } else {
            var classdata = inidata.CategoryList[classno];
            var classid = classdata.ChildList[btnno - 1].Id;
            for (var j = 0; j < inidata.Projects.length; j++) {
                var item = inidata.Projects[j];
                if (classid == item.Category) { //成立
                    newsArr.push(item);
                }
            }
        }
        var ProjectsHtml = getProjectsHtml(newsArr);
        $('#Tc_ProjectsLists ul').html(ProjectsHtml);
    })

//  //套餐已选菜品删除
    $('#select_tc').delegate('.del-icon', 'click', function (event) {
        var $tr = $(this).closest('tr');
        var index = $tr.index();
        TCthisArr.splice(index, 1);
        if ($tr.hasClass('layui-this')) {
            if($tr.next().length > 0){
	        	$tr.next().addClass('layui-this');
	        }else{
	        	$tr.prev().addClass('layui-this');
	        }
        }
        $tr.remove();
    })

}

//可选菜品返回html
function getProjectsHtml(Arr) {
    //可选菜品     
    var ProjectsHtml = '';
    for (var i = 0; i < Arr.length; i++) {
        var item = Arr[i];
        var checked = '';
        //		for(var j = 0; j < thisOrderProjectArr.PackageDetailList.length; j++) {
        //			var TC_cid = thisOrderProjectArr.PackageDetailList[j].R_ProjectDetail_Id;
        //			if(item.Id == TC_cid) {
        //				var checked = 'class="checked"';
        //			}
        //		}
        ProjectsHtml += '<li data-id="' + item.Id + '" data-IsCustomer="' + item.IsCustomer + '" data-cyddmxtype="' + item.CyddMxType + '" ' + checked + ' style="height:72px;">' +
            '<a href="javascript:void(0);">' +
            '<div class="MealTable-head flex">' +
            '<span class="item MealTable-number flex-item">' + item.Id + '</span>' +
            '</div>' +
            '<div class="MealTable-title" style="font-size:16px;line-height: 27px;height:27px;"> ' + item.ProjectName + ' </div> ' +
            '<div class="MealTable-footer flex">' +
            '<span class="MealTable-stock flex-item">' + item.Unit + ' </span> ' +
            '<span class="MealTable-price flex-item">￥' + item.Price + ' </span> ' +
            '</div> ' +
            '</a> ' +
            '</li>';
    }
    return ProjectsHtml;
}

//更新套餐已选菜品
function NewTcProjects(TCthisArr, type) {
    var ProjectsLists = '';
    var $select_tc = $('#select_tc');
    for (var i = 0; i < TCthisArr.length; i++) {
        var item = TCthisArr[i];
        var isRevise = item.IsCustomer ? '<i class="layui-icon revise" style="font-size:40px;">&#xe642;</i>' : ""
        ProjectsLists += '<tr data-id="' + item.R_ProjectDetail_Id + '">'+
        	'<td style="padding:2px;">'+isRevise+'</td>'+
        	'<td class="tl">'+ item.Name +'</td>'+
        	'<td>'+
        		'<div class="layui-btn-group add_minus">'+
					'<a href="javascript:void(0)" class="layui-btn layui-btn-primary layui-btn-sm minus" style="width:34px;"><i class="layui-icon">-</i></a>'+ 
					'<input type="text" style="width:60px !important;height:30px;padding:0;line-height:30px;" min="0.01" max="200000"  value="' + item.Num + '" class="layui-btn layui-btn-primary layui-btn-small num" disabled="disabled">'+
					'<a href="javascript:void(0)" class="layui-btn layui-btn-primary layui-btn-sm plus" style="width:34px;"><i class="layui-icon">+</i></a>'+
				'</div>'+
        	'</td>'+
        	'<td><i class="layui-icon del-icon">&#x1006;</i></td>'+
        '</tr>'
    }
    $select_tc.html(ProjectsLists);

    //为最后选中项添加样式
    if (type && type == 1) {
        $select_tc.find('tr:last').addClass('layui-this');
        $select_tc.closest('.table-body').scrollTop($select_tc.height());
    }

	$select_tc.find('tr').on('click',function(){
		$(this).addClass('layui-this').siblings().removeClass('layui-this');
	})

    //监听数量加减的点击
    $('#select_tc .add_minus a').click(function () {
        event.stopPropagation(); //阻止冒泡
        var aindex = $(this).index();
        var threshold = 0.5;
        if (aindex == 0) { //减
            var inputdom = $(this).next('input');
            var value = inputdom.val() ? inputdom.val() : 0;
            var newval = parseFloat(value) - threshold;
        } else if (aindex == 2) { //加
            var inputdom = $(this).prev('input');
            var value = inputdom.val() ? inputdom.val() : 0;
            var newval = parseFloat(value) + threshold;
        }
        var min = parseFloat(inputdom.attr('min'));
        var max = parseFloat(inputdom.attr('max'));
        var no = inputdom.closest('tr').index();
        if (newval == 0) {
            TCthisArr.splice(no, 1);
            var $tr = $(this).closest('tr');
            if($tr.next().length > 0){
            	$tr.next().addClass('layui-this');
            }else{
            	$tr.prev().addClass('layui-this');
            }
            $tr.remove();
        }
        if (newval && newval > min && newval <= max) {
            TCthisArr[no].Num = newval;
            inputdom.val(newval);
        }
    })

    //监听数量的输入是否正确
    $('#select_tc .add_minus input').blur(function (e) {
        var value = $(this).val();
        value = value.replace(/[(\ )(\~)(\!)(\@)(\#)(\$)(\%)(\^)(\&)(\*)(\()(\))(\-)(\_)(\+)(\=)(\[)(\])(\{)(\})(\|)(\\)(\;)(\:)(\')(\")(\,)(\/)(\<)(\>)(\?)(\)]+/, '')
        var min = parseFloat($(this).attr('min'));
        var max = parseFloat($(this).attr('max'));
        value = parseFloat(value)
        $(this).val(value);
        if (!isNaN(value)) {
            if (!value || value < min || value > max) {
                layer.msg('输入错误!');
                $(this).val(max);
                return false;
            } else {
                var no = $(this).attr('data-no');
                var num = parseFloat($(this).val());
                TCthisArr[no].Num = num;
            }
        } else {
            layer.msg('请输入数字!');
            $(this).val(max);
            return false;
        }
    })

    //监听修改按钮
    $('#select_tc .revise').on('click', function () {
        var $name = $(this).parent().next();
        var $tr = $(this).closest('tr');
        var $index = $tr.index();
        //R_ProjectDetail_Id
        var id = $tr.attr('data-id');
        //修改按钮 用字符串
        
        
//      str = '<div class="layui-form" style="padding:20px 50px 0 0;"><div class="layui-form-item"><label class="layui-form-label">名称:</label>' +
//          '<div class="layui-input-inline">' +
//          '<input class="layui-input" type="text" name="AuthPwd" placeholder="请输入名称" data-type="text" style="padding-right: 32px;"/>' +
//          '</div>' +
//          '</div></div>'
		str = '<form action="" class="layui-form" id="KeyboardForm">'+
		'<div class="layui-block" style="">'+
		'<input type="text" id="KeyboardInput" placeholder="请输入菜品名称(单位)" class="layui-input" style="width:calc(100% - 84px);display:inline-block;height: 60px;font-size: 28px;" value="'+$name.html()+'" />'+
		'<a href="javascript:;" class="layui-btn subBtn" style="vertical-align: top;line-height:60px;height:60px;font-size: 24px;">确认</a></div></form>'
        layer.open({
            type: 1,
            content: str,
            title: false,
            shade: [0.4, '#000'],
	        isOutAnim: false,
	        closeBtn:0,
	        skin: 'keyboard-box',
	        area: ['420px', '60px'],
	        offset: winH/2 - 70,//设置为屏幕一半往上
            success: function (layero, index) {
            	var inputdom = layero.find('#KeyboardInput').focus();
        		var subBtn = layero.find('.subBtn');
            	
                inputdom.focus();
                
                //软键盘回车
			    layero.find('#KeyboardForm').submit(function(){
			    	subBtn.click();
			    	return false;
			    })
			    
			    //确认按钮
			    subBtn.on('click',function(){
			    	var val = layero.find('.layui-input').val();
	                if (val === "") {
	                    layer.msg('菜品名不能为空');
	                    return false;
	                }
	                $name.html(val);
	
	                TCthisArr[$index].Name = val;
			        layer.close(index);
			        layer.msg('修改成功');
			    })
            },
            maxmin: false
        });
    })
}

//修改选中菜品的数量
function EditNum(dom, num, type) {
    if($(dom).hasClass('Disable'))return false;

    var Projectdom = $('#ProjectLists_view tr.layui-this');
    var ProjectListsno = Projectdom.index();
    if (ProjectListsno < 0) { //不存在选中菜品
        layer.msg('请选择要操作的菜品');
        return false;
    }
    var editOrderProjectsdata = OrderTableProjectsdata[ProjectListsno];
    //获取当前选中菜品 赠送/退菜/转出 的数量
    var othernum = 0;
    if (editOrderProjectsdata.OrderDetailRecordCount) {
        for (var i = 0; i < editOrderProjectsdata.OrderDetailRecordCount.length; i++) {
            var otheritem = editOrderProjectsdata.OrderDetailRecordCount[i];
            if (otheritem.CyddMxCzType == 1 || otheritem.CyddMxCzType == 2 || otheritem.CyddMxCzType == 4) {
                othernum += parseFloat(editOrderProjectsdata.OrderDetailRecordCount[i].Num);
            }
        }
    }
    if (type == 'plus') { //加
        var editnum = parseFloat(thisOrderProjectArr.Num) + parseFloat(num);
        if (editnum.toString().indexOf('.') >= 0) {
            if (editnum.toString().split(".")[1].length > 2) {
                editnum = parseFloat(editnum.toFixed(2));
            }
        }
        if (OrderTableProjectsdata[ProjectListsno].Id > 0 && OrderTableProjectsdata[ProjectListsno].CyddMxStatus > 0) { //已保存/下厨/打单 修改数量
            OrderTableProjectsdata[ProjectListsno].IsUpdateNum = true;
        }
        OrderTableProjectsdata[ProjectListsno].Num = editnum;
    } else if (type == 'minus') { //减
        if (OrderTableProjectsdata[ProjectListsno].Num - othernum < 1 || (othernum == 0 && OrderTableProjectsdata[ProjectListsno].Num <= 1)) {
            if (OrderTableProjectsdata[ProjectListsno].Id == 0 || OrderTableProjectsdata[ProjectListsno].CyddMxStatus <= 0) { //取消该菜品--只有新点/保存的菜品才能取消
                //删除菜品
                OrderTableProjectsdata.splice(ProjectListsno, 1);
                if (OrderTableProjectsdata.length >= 1) {
                    //下一个菜品选中
                    if (OrderTableProjectsdata[ProjectListsno]) {
                        thisOrderProjectArr = OrderTableProjectsdata[ProjectListsno];
                    } else {
                        thisOrderProjectArr = OrderTableProjectsdata[OrderTableProjectsdata.length - 1];
                        thisProjectsIndex = ProjectListsno - 1;
                    }
                    //更新当前选中菜品数组
                    var thisdata;
                    for (var i = 0; i < inidata.ProjectAndDetails.length; i++) {
                        if (thisOrderProjectArr.R_Project_Id == inidata.ProjectAndDetails[i].Id && thisOrderProjectArr.CyddMxType == inidata.ProjectAndDetails[i].CyddMxType) {
                            thisdata = inidata.ProjectAndDetails[i];
                        }
                    }
                    thisProjectArr = thisdata;
                    ProjectPower();
                    layer.closeAll('page');
                }
            } else if (OrderTableProjectsdata[ProjectListsno].Num - othernum == 1) { //退菜
                var editnum = parseFloat(thisOrderProjectArr.Num) - parseFloat(num);
                if (editnum.toString().indexOf('.') >= 0) {
                    if (editnum.toString().split(".")[1].length > 2) {
                        editnum = parseFloat(editnum.toFixed(2));
                    }
                }
                OrderTableProjectsdata[ProjectListsno].Num = editnum;
                if (OrderTableProjectsdata[ProjectListsno].Id > 0 && OrderTableProjectsdata[ProjectListsno].CyddMxStatus > 0) { //已保存/下厨/打单 修改数量
                    OrderTableProjectsdata[ProjectListsno].IsUpdateNum = true;
                }

            }
            //return false;
        } else if (OrderTableProjectsdata[ProjectListsno].Num <= othernum) {
            layer.msg('数量不能小于' + othernum);
            OrderTableProjectsdata[ProjectListsno].Num = parseFloat(othernum);
        } else {
            var editnum = parseFloat(thisOrderProjectArr.Num) - parseFloat(num);
            if (editnum.toString().indexOf('.') >= 0) {
                if (editnum.toString().split(".")[1].length > 2) {
                    editnum = parseFloat(editnum.toFixed(2));
                }
            }
            OrderTableProjectsdata[ProjectListsno].Num = editnum;
            if (OrderTableProjectsdata[ProjectListsno].Id > 0 && OrderTableProjectsdata[ProjectListsno].CyddMxStatus > 0) { //已保存/下厨/打单 修改数量
                OrderTableProjectsdata[ProjectListsno].IsUpdateNum = true;
            }
        }
    } else { //改数量
        if (num <= 0) {
            OrderTableProjectsdata.splice(ProjectListsno, 1);
            if (OrderTableProjectsdata.length >= 1) {
                if (OrderTableProjectsdata[ProjectListsno]) {
                    thisOrderProjectArr = OrderTableProjectsdata[ProjectListsno];
                } else {
                    thisOrderProjectArr = OrderTableProjectsdata[OrderTableProjectsdata.length - 1];
                    thisProjectsIndex = ProjectListsno - 1;
                }
                //更新当前选中菜品数组
                var thisdata;
                for (var i = 0; i < inidata.ProjectAndDetails.length; i++) {
                    if (thisOrderProjectArr.R_Project_Id == inidata.ProjectAndDetails[i].Id && thisOrderProjectArr.CyddMxType == inidata.ProjectAndDetails[i].CyddMxType) {
                        thisdata = inidata.ProjectAndDetails[i];
                    }
                }
                thisProjectArr = thisdata;
                ProjectPower();
                layer.closeAll('page');
            }
        } else {
            OrderTableProjectsdata[ProjectListsno].Num = parseFloat(num);
            if (OrderTableProjectsdata[ProjectListsno].Id > 0 && OrderTableProjectsdata[ProjectListsno].CyddMxStatus > 0) { //已保存/下厨/打单 修改数量
                OrderTableProjectsdata[ProjectListsno].IsUpdateNum = true;
            }
        }
    }

    //更新订单/统计金额
    NewsOrder();
}

//修改选中菜品的单价
function EditPrice(price) {
    var Projectdom = $('#ProjectLists_view tr.layui-this');
    var ProjectListsno = Projectdom.index();
    OrderTableProjectsdata[ProjectListsno].Price = parseFloat(price);
    if (OrderTableProjectsdata[ProjectListsno].CyddMxType == 2 && OrderTableProjectsdata[ProjectListsno].Id) OrderTableProjectsdata[ProjectListsno].IsUpdatePrice = true;
    //更新订单/统计金额
    NewsOrder();
}

//删除菜品
function DelProject(thisdom) {
	if($(thisdom).hasClass('Disable'))return false;
	
    var Projectdom = $('#ProjectLists_view tr.layui-this');
    if (Projectdom.length < 1) {
        layer.msg('请选择要操作的菜品');
        return false;
    }


    var ProjectListsno = Projectdom.index();
    layer.closeAll('page');
    layer.confirm('您是确认删除该菜品吗？', {
        btn: ['确认', '取消'] //按钮
    }, function (index) {
        OrderTableProjectsdata.splice(ProjectListsno, 1);
        Projectdom.remove();
        //更新订单/统计金额
        NewsOrder();
        layer.close(index);
    }, function () { });

}

//全单即起/叫起
function DishesStatus(type) {
//	if($(thisdom).hasClass('Disable'))return false;
	
    if (!OrderTableProjectsdata) {
        layer.msg('无可操作的菜品!');
        return false;
    }

    if (type == '1') { //即起
        var title = '即起';
    } else { //叫起
        var title = '叫起';
    }

    layer.closeAll('page');
    layer.confirm('您是确认操作全单' + title + '吗？', {
        btn: ['确认', '取消'] //按钮
    }, function (index) {
        for (var i = 0; i < OrderTableProjectsdata.length; i++) {
            var item = OrderTableProjectsdata[i];
            if (item.Id <= 0 || item.CyddMxStatus == 0) {
                OrderTableProjectsdata[i].DishesStatus = parseFloat(type);
            }
        }
        allStatus = type;
        //更新订单/统计金额
        NewsOrder();
        layer.close(index);
    }, function () { });
}

//单品即起，叫起
function EditDishesStatus(type) {
    var Projectdom = $('#ProjectLists_view tr.layui-this');
    if (Projectdom.length < 1) {
        layer.closeAll('page');
        layer.msg('请选择要操作的菜品');
        return false;
    }
    if (type == '1') { //即起
        var title = '即起';
    } else { //叫起
        var title = '叫起';
    }
    thisOrderProjectArr.DishesStatus = parseFloat(type);
    //更新订单/统计金额
    NewsOrder();
	
	layer.closeAll('page');
}

//退菜,赠送
function ProjectOther(CyddMxCzType, num,other) {
    var Projectdom = $('#ProjectLists_view tr.layui-this');
    if (Projectdom.length < 1) {
        layer.closeAll('page');
        layer.msg('请选择要操作的菜品');
        return false;
    }



    var ProjectListsno = Projectdom.index();
    num = parseFloat(num);
    //增加数组退菜参数
    var arr = {
        'CyddMxCzType': CyddMxCzType,
        'Num': num
    };
    var OrderDetailRecordCount = OrderTableProjectsdata[ProjectListsno].OrderDetailRecordCount;

    if (OrderDetailRecordCount) { //存在 退/赠、转入、转出
        var is = false;
        for (var i = 0; i < OrderDetailRecordCount.length; i++) {
            if (OrderDetailRecordCount[i].CyddMxCzType == CyddMxCzType) { //已存在退菜，更新退菜数量
                OrderTableProjectsdata[ProjectListsno].OrderDetailRecordCount[i].Num = parseFloat(OrderTableProjectsdata[ProjectListsno].OrderDetailRecordCount[i].Num) + parseFloat(num);
                is = true;
            }
        }
        if (is == false) { //不存在 退菜
            OrderTableProjectsdata[ProjectListsno].OrderDetailRecordCount.push(arr);
        }
    } else { //不存在
        OrderTableProjectsdata[ProjectListsno].OrderDetailRecordCount = [];
        OrderTableProjectsdata[ProjectListsno].OrderDetailRecordCount.push(arr);
    }

    if((CyddMxCzType == 1 || CyddMxCzType == 2) && thisOrderProjectArr.Id > 0) {
        //赠送传到后台
        if(CyddMxCzType == 1 && thisOrderProjectArr.Id > 0) {
        	var thisOrderProjectNum = thisOrderProjectArr.Num;
            var req = {
                R_OrderDetail_Id: thisOrderProjectArr.Id,
                CyddMxCzType: 1,
                Num: num,
                CyddMxName: thisOrderProjectArr.CyddMxName,
                OrderId: inidata.OrderAndTables.OrderId,
                TableName: inidata.OrderAndTables.TableName,
                Remark:other.Remark,
				R_OrderDetailCause_Id:other.Id
            }
            var para = {
                req: req
            };

            $.ajax({
                url: "/Res/Home/CreateOrderDetailRecord",
                type: "post",
                data: JSON.stringify(para),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function (xhr) {
		            layindex = layer.open({type: 3});
		        },
		        complete: function (XMLHttpRequest, textStatus) {
		            layer.close(layindex);
		        },
                success: function(data) {
                    if(data.Data) {
                    	thisOrderProjectArr.Num = thisOrderProjectNum;
						thisOrderProjectArr.OrderDetailRecord.push(data.Data)
                        //更新订单/统计金额
                        NewsOrder();
                        layer.msg('赠送成功')
                    } else {
                        layer.msg(data.Message);
                    }
                },
                error: function (msg) {
                    console.log(msg.responseText);
                }
            });
        }

        //退菜传到后台
        if (CyddMxCzType == 2 && thisOrderProjectArr.Id > 0) {
            //套餐退菜  弹出窗口

            var thisOrderProjectNum = thisOrderProjectArr.Num;
            var req = {};
            $.extend(true, req, thisOrderProjectArr)
            req.Num = num; //退菜数量
            req.PackageDetailList = [];
            req.Remark = other.Remark
			req.R_OrderDetailCause_Id = other.Id
            var table = {
                Restaurant: inidata.OrderAndTables.Restaurant,
                Name: inidata.OrderAndTables.TableName
            }
            var para = {
                req: req,
                table: table
            };

            $.ajax({
                url: "/Res/Home/ReturnOrderDetail",
                type: "post",
                data: JSON.stringify(para),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function (xhr) {
		            layindex = layer.open({type: 3});
		        },
		        complete: function (XMLHttpRequest, textStatus) {
		            layer.close(layindex);
		        },
                success: function (data) {
                    if (data.Data) {
                        thisOrderProjectArr.Num = thisOrderProjectNum;
						data.Data.Num = num;
						data.Data.CyddMxCzType = 2;
						data.Data.CyddMxCzTypeName = '退菜';
						thisOrderProjectArr.OrderDetailRecord.push(data.Data)
						layer.msg('退菜成功')
						//更新订单/统计金额
						NewsOrder();
                    } else {
                        layer.msg('系统错误，请稍后尝试!');
                    }
                },
                error: function (msg) {
                    console.log(msg.responseText);
                }
            });
        }
    } else {
        //更新订单/统计金额
        NewsOrder();
    }
}


//套餐退菜
function tcReturnOrder() {
    var str = '<div class="MealTable-lists" style="margin-right:0;overflow: hidden;overflow-y: auto; height:500px"><ul style="padding:10px;">'
    var list = thisOrderProjectArr.PackageDetailList;
    for (var i = 0; i < list.length; i++) {
        str += '<li data-id="' + list[i].Id + '" style="height:72px;width:140px;margin:3px;">' +
            '<a href="javascript:void(0);">' +
            '<div class="MealTable-title" style="font-size:16px;line-height: 40px;height:40px;"> ' + list[i].Name + ' </div> ' +
            '<div class="MealTable-footer flex" style="margin-left: 10px;margin-top: 10px;">' +
            '<span class="MealTable-stock flex-item">数量： ' + list[i].Num + ' </span> ' +
            '</div> ' +
            '</a> ' +
            '</li>';
    }
    str += '</ul></div>'

    layer.open({
        type: 1,
        shade: [0.4, '#000'],
        title: '套餐退菜',
        skin: 'layer-header',
        area: ['800px', '600px'],
        content: str,
        btn: ['确认', '取消'],
        success: function (layero, index) {
            var $li = layero.find('.MealTable-lists li');
            var len = $li.length;
            //添加全选按钮
            var $checkbox = $('<div class="layui-form" lay-filter="returnOrder" style="position: absolute;bottom: 12px;left: 10px;"><input type="checkbox" name="isAllSelected" title="全选" class="isAllSelected" lay-filter="isAllSelected"></div>')
            layero.children('.layui-layer-btn').append($checkbox);
            var isAllSelected = layero.find('.isAllSelected'); //全选标签

            form.render('checkbox', 'returnOrder');

            form.on('checkbox(isAllSelected)', function (data) {
                if (data.elem.checked == true) {//全选
                    $li.addClass('checked')
                } else {
                    $li.removeClass('checked')
                }
            });

            //点击菜品效果
            layero.find('.MealTable-lists li').on('click', function () {
                $(this).hasClass('checked') ? $(this).removeClass('checked') : $(this).addClass('checked');
                if ($li.filter('.checked').length === len) {
                    isAllSelected.get(0).checked = true;
                } else {
                    isAllSelected.attr("checked", false);
                }
                form.render('checkbox', 'returnOrder');
            })
        },
        yes: function (index, layero) {
            //按钮【按钮一】的回调
            var $li = layero.find('.MealTable-lists li');
            var len = thisOrderProjectArr.PackageDetailList.length;
            var req = $.extend(true, req, thisOrderProjectArr)
            var $indexArr = [];
            req.PackageDetailList = []
            $.each($li, function (i) {
                if ($(this).hasClass('checked')) {
                    req.PackageDetailList.push(list[i])
                    $indexArr.push(i);
                }
            })

            var table = {
                Restaurant: inidata.OrderAndTables.Restaurant,
                Name: inidata.OrderAndTables.TableName
            }
            var para = {
                req: req,
                table: table
            };
            $.ajax({
                url: "/Res/Home/ReturnOrderDetail",
                type: "post",
                data: para,
                dataType: "json",
                beforeSend: function (xhr) {
		            layindex = layer.open({type: 3});
		        },
		        complete: function (XMLHttpRequest, textStatus) {
		            layer.close(layindex);
		        },
                success: function (data) {
                    if (data.Data) {
                        parent.layer.msg('退菜成功');
                        var domThis = $('#ProjectLists_view tr.layui-this');
                        var $index = domThis.index();
                        for (var i = 0; i < $indexArr.length; i++) {
                            OrderTableProjectsdata[$index].PackageDetailList.splice($indexArr[i], 1);
                        }
                        //如果全部菜品退完
                        if (req.PackageDetailList.length === len) {
                            OrderTableProjectsdata[$index].OrderDetailRecordCount.push({ 'CyddMxCzType': 2, 'Num': OrderTableProjectsdata[$index].Num });
                        }
                        domThis.trigger('click');
                        NewsOrder();
                        layer.close(index);
                    } else {
                        layer.alert(data.Message);
                    }
                },
                error: function (msg) {
                    console.log(msg.responseText);
                }
            });

        },
        btn2: function (index, layero) {
        }
    });
}

//退菜前判断
function orderReturnBtn(Event, thisdom) {
	if($(thisdom).hasClass('Disable'))return false;
	
    //如果是套餐退菜
    if (thisOrderProjectArr.CyddMxType == 2 && thisOrderProjectArr.Id > 0) {
        //获取当前选中菜品 赠送/退菜/转出 的数量
        var othernum = 0;
        if (thisOrderProjectArr.OrderDetailRecordCount) {
            for (var i = 0; i < thisOrderProjectArr.OrderDetailRecordCount.length; i++) {
                var otheritem = thisOrderProjectArr.OrderDetailRecordCount[i];
                if (otheritem.CyddMxCzType == 1 || otheritem.CyddMxCzType == 2 || otheritem.CyddMxCzType == 4) {
                    othernum += parseFloat(thisOrderProjectArr.OrderDetailRecordCount[i].Num);
                }
            }
        }
        //如果没有数量了
        if (thisOrderProjectArr.Num - othernum <= 0) {
            layer.msg('已到允许最大值！');
            return false;
        }

        var str = '<div style="padding:20px 40px;text-align: center;"><button class="layui-btn layui-btn-lg num" style="line-height:60px;height:60px;padding:0 60px;font-size:24px;">退数量</button><button class="layui-btn layui-btn-normal layui-btn-lg" style="line-height:60px;height:60px;padding:0 60px;font-size:24px;" onclick="tcReturnOrder();">退菜品</button></div>'
        layer.open({
            type: 1,
            title: '退菜方式选择',
            shadeClose: true,
            shade: [0.4, '#000'],
            area: '500px',
            content: str,
            success: function (layero, index) {
                layero.find('.num').on('click', function () {
                    NumberKeyboard(Event, thisdom);
                    layer.close(index)
                })
            }
        })
    } else {//单品退菜
        NumberKeyboard(Event, thisdom);
    }
}


//弹出可输入的数字键盘
function NumberKeyboard(Event, thisdom) {
	if($(thisdom).hasClass('Disable'))return false;
	
    var Projectdom = $('#ProjectLists_view tr.layui-this');
    if(Projectdom.length < 1 && Event != 'editSum') {
        layer.msg('请选择要操作的菜品');
        return false;
    }


    //获取当前选中菜品 赠送/退菜/转出 的数量
    var othernum = 0;
    if(thisOrderProjectArr && thisOrderProjectArr.OrderDetailRecordCount) {
        for (var i = 0; i < thisOrderProjectArr.OrderDetailRecordCount.length; i++) {
            var otheritem = thisOrderProjectArr.OrderDetailRecordCount[i];
            if (otheritem.CyddMxCzType == 1 || otheritem.CyddMxCzType == 2 || otheritem.CyddMxCzType == 4) {
                othernum += parseFloat(thisOrderProjectArr.OrderDetailRecordCount[i].Num);
            }
        }
    }
	
	
//  var mrval = '';
    var tips = "";
    if(Event == "give") { //赠送
        tips = "请输入赠送数量"
        var max = Math.round(thisOrderProjectArr.Num*100 - othernum*100)/100
    } else if(Event == "retreat") { //退菜 数量
		tips = "请输入退菜数量"
        var max = Math.round(thisOrderProjectArr.Num*100 - othernum*100)/100
    } else {
        var max = 10000000;
    }
    if(Event == 'editprice') {
        tips = '请输入菜品价格';
    }
    if(Event == "editnum") { //修改菜品数量
        tips = "请输入菜品数量"
        var mix = othernum;
    } else if(Event == 'retreat' || Event == 'give'){
		var mix = 0;
	}else{
		var mix = 0.01;
	}
    
    if(Event == 'editSum'){ //修改人数
		var mix = 0;
		tips = "请输入餐桌人数"
	}

    if(max <= 0) {
        layer.msg('已到允许最大值！');
        return false;
    }
    
    if(Event == 'retreat' || Event == "give"){
    	var str = '<form action="" class="layui-form" id="KeyboardForm" style="padding:10px">'
    	str += '<div class="layui-form-item" pane>' + 
					'<a href="javascript:;" class="layui-btn  layui-btn-normal" style="float:left;" id="keyboardRemarkBtn" data-type="'+ Event +'" style="width:100px;">选择</a>'+
					'<div class="layui-input-block">'+
						'<input type="text" id="CauseType" name="CauseType" placeholder="请输入'+ (Event == 'give'?'赠送':'退菜') +'理由" class="layui-input" value=""/>' + 
						'<input type="hidden" id="KeyboardInputId" value="0"/>' +
					'</div>' + 
				'</div>' + 
						'<input type="number" id="KeyboardInput" placeholder="' + tips + '" class="layui-input" placeholder="请输入数量"/>' 
    	str += '</form>'
    	
    	
    	layer.open({
			type:1,
			title: (Event == 'give'?'赠送':'退菜'),
			shadeClose: true,
			offset: 't',
			shade: [0.1, '#fff'],
			area: ['40%', '210px'],
			content: str,
			btn: ['确认', '取消'],
			yes: function(index, layero){
				var inputval = layero.find('#KeyboardInput').val();
				
				var userreg = /^[1-9]\d*([.]{1}[0-9]{1,2})?|0([.]{1}[0-9]{1,2})?$/;
				if(!userreg.test(inputval)){
					layer.msg('输入的数字有误')
					return false;
				}
				if (inputval <= 0 || !inputval) {
		            layer.msg('请输入大于0的数字!');
		            return false;
		        }
				if(inputval > max) {
					layer.msg('数量不能大于' + max);
					return false;
				} else if(inputval < mix) {
					layer.msg('数量不能小于' + mix);
					return false;
				}
				
				if (Event == 'give') { //赠送
		            ProjectOther(1, inputval,{Remark:$('#CauseType').val(),Id:$('#KeyboardInputId').val()});
		        } else if (Event == 'retreat') { //退菜
		            ProjectOther(2, inputval,{Remark:$('#CauseType').val(),Id:$('#KeyboardInputId').val()});
		        }
		        layer.close(index)
			},
			success: function(layero,index){
				NumberKeyboardFuc(layero)
				
				$('#keyboardRemarkBtn').on('click',function(){
					var str = '<div class="layui-form">'
										
					if(Event == 'give'){
						for(var i=0;i<inidata.GiveCauses.length;i++){
							str += '<a href="javascript:;" class="layui-btn  layui-btn-normal Remark" style="margin:10px;" data-id="'+ inidata.GiveCauses[i].Id +'">' + inidata.GiveCauses[i].Remark + '</a>'
						}
					}else if(Event == 'retreat'){
						for(var i=0;i<inidata.ReturnCauses.length;i++){
							str += '<a href="javascript:;" class="layui-btn  layui-btn-normal Remark" style="margin:10px;" data-id="'+ inidata.ReturnCauses[i].Id +'">' + inidata.ReturnCauses[i].Remark + '</a>'
						}
					}
					str += '</div>';
					
					layer.open({
						type:1,
						title: (Event == 'give'?'赠送':'退菜') + '理由选择',
						shadeClose: true,
						shade: [0.1, '#fff'],
						area: ['60%', '60%'],
						content: str,
						success: function(layero,index){
							layero.find('.Remark').on('click',function(){
								$('#KeyboardInputId').val($(this).attr('data-id'));
								$('#CauseType').val($(this).html()).focus();
								layer.close(index)
							})
						}
					});
				})
			}
		});
    }else{
    	var str  = '<form action="" class="layui-form" id="KeyboardForm">'+
			'<div class="layui-block" style="position:relative">'+
			'<input type="number" id="KeyboardInput" placeholder="' + tips + '" class="layui-input" style="width:calc(100% - 84px);display:inline-block;height: 60px;font-size: 28px;padding-right:60px;"/>'+
			'<i class="layui-icon clearBtn" style="position:absolute;right:94px;top:0;font-size: 50px;line-height: 67px;height: 60px;">&#x1007;</i>'+
			'<a href="javascript:;" class="layui-btn subBtn" style="vertical-align: top;line-height:60px;height:60px;font-size: 24px;">确认</a></div></form>'
	
	    layer.open({
	        type: 1,
	        title: false,
	        closeBtn:0,
	        offset: winH/2 - 70,//设置为屏幕一半往上
	        shadeClose: true,
	        shade: [0.4, '#000'],
	        isOutAnim: false,
	        skin: 'keyboard-box',
	        area: ['420px', '60px'],
	        content: str,
	        success: function(layero,index){
	        	var subBtn = layero.find('.subBtn')
	        	var inputdom = $('#KeyboardInput')
	        	NumberKeyboardFuc(layero)
	        	layero.css('background','none');
	        	
	        	//软键盘回车
			    layero.find('#KeyboardForm').submit(function(){
			    	subBtn.click();
			    	return false;
			    })
			    
			    //确认按钮
			    subBtn.on('click',function(){
			    	var inputval = inputdom.val();
			    	
			    	var userreg = Event != 'editprice' ? /^[1-9]\d*([.]{1}[0-9]{1,2})?|0([.]{1}[0-9]{1,2})?$/ : /^[-]?[1-9]\d*([.]{1}[0-9]{1,2})?|0([.]{1}[0-9]{1,2})?$/;
					if(!userreg.test(inputval)){
						layer.msg('输入的数字有误')
						return false;
					}else{
						//如果是修改数量 || 退菜 || 赠菜   不可小于0
						if((Event == 'retreat' || Event == 'give' || Event == 'editnum')){
							if(inputval <= 0){
								layer.msg('输入的数字有误')
								return false;
							}
							if(inputval > max) {
								layer.msg('数量不能大于' + max);
								return false;
							} else if(inputval < mix) {
								layer.msg('数量不能小于' + mix);
								return false;
							}
						}
						//如果是修改人数
						if(Event == 'editSum' && !(/^[1-9]*[0-9]?$/).test(inputval)){
							layer.msg('输入的数字有误')
							return false;
						}
					}
			    	
//			    	if (inputval < 0 || !inputval) {
//			            layer.msg('请输入大于0的数字!');
//			            return false;
//			        }
			    	
			    	if (Event == 'editnum') { //修改菜品数量
			            EditNum('', inputval, '');
			        } else if (Event == 'editprice') { //修改菜品价格
			            EditPrice(inputval);
			        } else if (Event == 'give') { //赠送
			            ProjectOther(1, inputval);
			        } else if (Event == 'retreat') { //退菜
			            ProjectOther(2, inputval);
			        } else if(Event == 'editSum') { //修改人数
						$.ajax({
				            type: "post",
				            url: "/Res/Home/OrderTablePersonUpdate",
				            dataType: "json",
				            data : {Id : OrderTableIds[0],PersonNum : inputval},
				            //contentType: "application/json; charset=utf-8",
				            async: false,
				            beforeSend: function (xhr) {
					            layindex = layer.open({type: 3});
					        },
					        complete: function (XMLHttpRequest, textStatus) {
					            layer.close(layindex);
					        },
				            success: function (data, textStatus) {
				            	if(data.Data == true){
				            		layer.closeAll('page');
									layer.msg('人数修改成功');
									inidata.OrderAndTables.PersonNum = inputval;
									var getTpl = OrderAndTables_tpml.innerHTML,
										view = document.getElementById('OrderAndTables_view');
									laytpl(getTpl).render(inidata.OrderAndTables, function(html) {
										view.innerHTML = html;
									});
				            	}else{
				            		layer.alert(data.Message)
				            	}
				            }
				        });
						return false;
					}
			        layer.close(index);
			        layer.msg('修改成功');
			    })
	        }
	    });
    }
    
    function NumberKeyboardFuc(layero){
    	if(Event == 'editSum'){
			$('#KeyboardInput').val(inidata.OrderAndTables.PersonNum);
		}
    	
    	
    	var inputdom = layero.find('#KeyboardInput').focus();
    	var subBtn = layero.find('.subBtn');
    	
    	layero.find('.clearBtn').on('click',function(){
    		inputdom.val('');
    	})
    	
    	//输入字符判断  是否是数字
    	inputdom.bind('input propertychange', function (e) {
	        var value = $(this).val();
	        if (value == null || value == '') {
	            return false;
	        }
	        
	        if(Event == 'editprice'){
				if(value == "-"){
			    	return;
			    }
			    
			    if (!isNaN(value)) {
			        var userreg = /^[-]{0,1}[0-9]+([.]{1}[0-9]{1,2})?$/;
			        
			        if (userreg.test(value) && value > 1000000) {
						layer.msg('数量不能大于' + 1000000);
			            $(this).val('');
			       	}else{
			        	var numindex = parseInt(value.indexOf("."), 10);
			            
			            if(numindex < 0)numindex = value.length;
			            
			            if (numindex == 0) {
			                $(this).val("");
			                layer.msg("输入的数字不规范");
			            }
			            
			            var head = value.substring(0, numindex);
			            var bottom = value.substring(numindex, numindex + 3);
			            var fianlNum = head + bottom;
			            $(this).val(fianlNum);
			        }
			    } else {
			        $(this).val("");
			        layer.msg("请输入数字");
			    }
			}else if (!isNaN(value)) {
	            var userreg = /^[0-9]+([.]{1}[0-9]{1,2})?$/;
	            if (userreg.test(value)) {
	                if (value < 0) {
	                    layer.msg('请输入大于0的数字!');
	                    $(this).val('');
	                } else if (value > max) {
	                    layer.msg('数量不能大于' + max);
	                    $(this).val('');
	                } else if (value < mix) {
	                    layer.msg('数量不能小于' + mix);
	                    $(this).val('');
	                }
	            } else {
	                var numindex = parseInt(value.indexOf("."), 10);
	                if (numindex == 0) {
	                    $(this).val("");
	                    layer.msg("输入的数字不规范");
	                    return false;
	                }
	                var head = value.substring(0, numindex);
	                var bottom = value.substring(numindex, numindex + 3);
	                var fianlNum = head + bottom;
	                $(this).val(fianlNum);
	            }
	        } else {
	            $(this).val("");
	            layer.msg("请输入数字");
	            return false;
	        }
	    })
	    
	    
    }
}

/**
 * 手写菜
 * @param {Object} thisdom  点击按钮的dom
 */
function EditName(thisdom) {
	if($(thisdom).hasClass('Disable'))return false;
	
//  layer.closeAll('page');
    var Projectdom = $('#ProjectLists_view tr.layui-this');
    if (Projectdom.length < 1) {
        layer.msg('请选择要操作的菜品');
        return false;
    }

    var str = '<form action="" class="layui-form" id="KeyboardForm" style="padding:10px 50px 0 0">' +
					'<div class="layui-form-item">' +
					    '<label class="layui-form-label">菜品：</label>' +
				    	'<div class="layui-input-block">' +
					      '<input type="text" name="title" lay-verify="title" autocomplete="off" placeholder="请输入菜品名称" class="layui-input editName">' +
					    '</div>' +
				  	'</div>' +
				  	'<div class="layui-form-item">' +
					    '<label class="layui-form-label">价格：</label>' +
					    '<div class="layui-input-block">' +
					    	'<input type="text" name="username" lay-verify="required" placeholder="请输入菜品价格" value="' + thisOrderProjectArr.Price + '" autocomplete="off" class="layui-input editPrice">' +
					    '</div>' +
				  	'</div>' +
				'</form>';

    layer.open({
        type: 1,
        title: '手写菜',
        closeBtn:0,
        offset: winH/2 - 70,//设置为屏幕一半往上
        shadeClose: true,
        shade: [0.4, '#000'],
        isOutAnim: false,
        skin:'editNameLayer',
        offset: 't',
        content: str,
        btn: ['确认', '取消'],
		yes: function(index, layero){
			editNameSubmit();
  		}
  		,btn2: function(index, layero){
  		},
        success: function (layero,index) {
            
        	var subBtn = layero.find('.subBtn');
        	//清除  字符   （手写）
	        if (thisOrderProjectArr.ProjectName.indexOf('(手写)') >= 0) {
	            var newName = thisOrderProjectArr.ProjectName.replace("(手写)", "");
	        } else {
	            var newName = thisOrderProjectArr.ProjectName;
	        }
	        layero.find('.layui-input').eq(0).focus().val(newName);
        	
        	//软键盘回车
		    layero.find('#KeyboardForm').submit(function(){
		    	console.log($(':focus'))
		    	return false;
		    	var nextItem = $(this).closest('.layui-form-item').next('.layui-form-item');
				if(nextItem.length > 0){
					nextItem.find('.layui-input').focus();
				}else{
					editNameSubmit();
				}
		    	return false;
		    })
		    
        }
    });
    
    //提交
	function editNameSubmit(layero){
		var layero = $('.layui-layer.editNameLayer');
		var newname = layero.find('.layui-input.editName').val();
		var newprice = layero.find('.layui-input.editPrice').val();
		if(!newname){
			layer.msg('请输入菜品名称');
			return false;
		}
		if(isNaN(newprice) || newprice == ""){
			layer.msg('金额必须是数字且不能为空');
			return false;
		}else if(newprice < 0  || newprice.indexOf('.') + 1 === newprice.length){
			layer.msg('必须大于等于0', {icon: 5,shift: 6});
			return false;
		}
		if(newprice.split(".")[1] && newprice.split(".")[1].length > 2){
			layer.msg('最多只能有两位小数', {icon: 5,shift: 6});
			return false
		}
		
		newprice = parseFloat(newprice);
		thisOrderProjectArr.ProjectName = newname + '(手写)';
		thisOrderProjectArr.CyddMxName = newname + '(手写)';
		thisOrderProjectArr.Price = newprice;
		NewsOrder();
		layer.closeAll('page');
	}
}

/**
 * 手写做法
 * @param {Object} thisdom  点击按钮的dom
 */
function EditTypeName(thisdom) {
	if($(thisdom).hasClass('Disable'))return false;
	
    layer.closeAll('page');
    var Projectdom = $('#ProjectLists_view tr.layui-this');
    if (Projectdom.length < 1) {
        layer.msg('请选择要操作的菜品');
        return false;
    }


    var str  = '<form action="" class="layui-form" id="KeyboardForm">'+
		'<div class="layui-block">'+
			'<input type="text" id="KeyboardInput" placeholder="请输入做法名称" class="layui-input" style="width:calc(100% - 84px);display:inline-block;height: 60px;font-size: 28px;"/>'+
			'<a href="javascript:;" class="layui-btn subBtn" style="vertical-align: top;line-height:60px;height:60px;font-size: 24px;">确认</a>'+
		'</div>'+
	'</form>'
    layer.open({
        type: 1,
        title: false,
        closeBtn:0,
        offset: winH/2 - 70,//设置为屏幕一半往上
        shadeClose: true,
        shade: [0.4, '#000'],
        isOutAnim: false,
        skin: 'keyboard-box',
        area: ['420px', '60px'],
        content: str,
        success: function (layero,index) {
            layero.css('background','none');
        	var inputdom = layero.find('#KeyboardInput').focus();
        	var subBtn = layero.find('.subBtn');
        	
        	//软键盘回车
		    layero.find('#KeyboardForm').submit(function(){
		    	subBtn.click();
		    	return false;
		    })
		    
		    //确认按钮
		    subBtn.on('click',function(){
		    	var inputval = inputdom.val();

				if(!inputval) {
					layer.msg('请输入做法');
					return false;
				}
				thisOrderProjectArr.Remark = inputval;
				
				NewsOrder();//更新已选菜品
		    	layer.msg('修改成功');
		        layer.close(index);
		    })
        }
    });
}

//更多按钮 => 换台
function changeTab(thisdom) {
    layer.closeAll('page');
    if ($(thisdom).hasClass('Disable')) {
        return false;
    }

    layer.open({
        type: 2,
        anim: -1,
        title: '换台',
        shadeClose: true,
        skin: 'layer-header',
        shade: 0.8,
        area: ['80%', '80%'],
        content: "/Res/MHome/AllChangeTable?orderTableId=" + OrderTableIds[0] + "&restaurantId=" + inidata.OrderAndTables.R_Restaurant_Id + "&TableId=" + inidata.OrderAndTables.R_Table_Id
    });
}

//更多按钮 => 加台
function addTab(thisdom) {
	layer.closeAll('page');
	if($(thisdom).hasClass('Disable')) {
		return false;
	}

	layer.open({
		type: 2,
		anim: -1,
		title: '请选择需要添加的桌台',
		shadeClose: true,
		skin: 'layer-header',
		shade: 0.8,
		area: ['80%', '80%'],
		content: "/Res/MHome/AddTable?orderTableId=" + OrderTableIds[0] + "&restaurantId=" + inidata.OrderAndTables.R_Restaurant_Id + "&TableId=" + inidata.OrderAndTables.R_Table_Id
	});
}

//更多按钮 => 撤台
function revokeTab(thisdom) {
	layer.confirm('是否确认撤台?', {icon: 3, title:'提示'}, function(index){
		layer.closeAll('page');
		layer.close(index);
	  	$.ajax({
			type: "post",
			url: "/Res/Home/CancelOrderTable",
			dataType: "json",
			data: {
				orderTableId: OrderTableIds[0]
			},
			async: false,
			beforeSend: function (xhr) {
	            layindex = layer.open({type: 3});
	        },
	        complete: function (XMLHttpRequest, textStatus) {
	            
	        },
			success: function(data, textStatus) {
				if (data["Data"] == true) {
					$.connection.hub.start().done(function () {
                        chat.server.notifyResServiceRefersh(true);
                        location.replace("/Res/MHome/Index");
                    });
                    //parent.Refresh();
                } else {
                	layer.close(layindex);
                    layer.alert(data["Message"]);
                }
			}
		});
	});
}

//更多按钮 => 并台
function combineTab(thisdom) {
	layer.closeAll('page');
	if($(thisdom).hasClass('Disable')) {
		return false;
	}
	layer.open({
		type: 2,
		anim: -1,
		title: '并台',
		shadeClose: true,
		closeBtn: 0,
		skin: 'layer-header',
		shade: 0.8,
		area: ['80%', '80%'],
		content: "/Res/MHome/JoinTable?orderTableId=" + OrderTableIds[0] + "&restaurantId=" + inidata.OrderAndTables.R_Restaurant_Id + "&orderId=" + inidata.OrderAndTables.OrderId + "&TableId=" + inidata.OrderAndTables.R_Table_Id
	});
}

//更多按钮 => 拼台
function spellTab(thisdom) {
	layer.closeAll('page');
	if($(thisdom).hasClass('Disable')) {
		return false;
	}
	layer.open({
		type: 2,
		anim: -1,
		title: '并台 （ ' + inidata.OrderAndTables.TableName + " ）",
		shadeClose: true,
		closeBtn: 0,
		skin: 'layer-header',
		shade: 0.8,
		area: ['80%', '80%'],
		content: "/Res/MHome/SpellTable/" + inidata.OrderAndTables.R_Table_Id
	});
}

//更多按钮 => 取消赠送  || 取消退菜
function cancelRetire(type){
	if(!thisOrderProjectArr || !thisOrderProjectArr.ProjectName){
		layer.msg('请选择操作菜品')
	}else if(!thisOrderProjectArr.OrderDetailRecord || thisOrderProjectArr.OrderDetailRecord.length == 0){
		type == 1 ? layer.msg('该菜品没有赠送记录') : layer.msg('该菜品没有退菜记录');
	}else {
		var list = thisOrderProjectArr.OrderDetailRecord;
		var data = {};
		var len = 0;
		var index = 0;
		for(let i = 0;i<list.length;i++){
			if(list[i].CyddMxCzType == type){
				data[i] = list[i];
				len++;
				index = i;
			}
		}
		switch(len){
			case 0:
				type == 1 ? layer.msg('该菜品没有赠送记录') : layer.msg('该菜品没有退菜记录');
				break;
			case 1:
				cancelGiveStart(index,type);
				break;
			default:
				var str = '<div style="padding:0 10px 0 20px;"><table class="table-header layui-table" lay-filter="cancelGive" lay-skin="line">' +
							'<thead>' +
								'<tr>' +
									'<th lay-data="{field:\'a\',align:\'center\',width:\'156\'}">时间</th>' +
									'<th lay-data="{field:\'b\',align:\'center\',width:\'156\'}">数量</th>' +
									'<th lay-data="{field:\'c\',align:\'center\',width:\'156\'}">操作</th>' +
								'</tr>' +
							'</thead>' +
							'<tbody>';
				var btnText = (type == 1 ? '取消赠送' : '取消退菜');
				for(var i in data){
					str += "<tr>" +
								"<td>" + data[i].CreateDate + "</td>" +
								"<td>" + data[i].Num + "</td>" +
								"<td><a href='javascript:;' class='layui-btn' onclick='cancelGiveStart(" + i + "," + type + ")'>"+ btnText +"</a></td>" +
							"</tr>"
				}
				str += '</tbody></table></div>';
				var maxH = winH*0.8;
				layer.open({
					type: 1,
					title: thisOrderProjectArr.CyddMxName + (type == 1 ? "（ 取消赠送 ）" : "（ 取消退菜 ）"),
					shadeClose: true,
					skin: 'layer-header',
					offset: 'auto',
					area: '500px',
					maxHeight:maxH,
					content: str,
					success: function(layero, index){
//							layero.find('.printOrderLayer').append($(str));

						table.init('cancelGive', {
							skin: 'line',
							height: layero.find('.layui-layer-content').height() - 20,
							limit: 99999999,
							unresize: true,
							done: function(){
								layero.find('.layui-btn').css({'height':'32px','line-height':'32px'})
							}
						});
					}
				})
		}
	}
	
}

//更多按钮 => 取消赠送  || 取消退菜 ajax
function cancelGiveStart(index,type){
    var data = [thisOrderProjectArr.OrderDetailRecord[index]];
    //data.push(thisOrderProjectArr.OrderDetailRecord[index]);
	$.ajax({
		type: "post",
		url: "/Res/Home/DeleteOrderDetailRecord",
		dataType: "json",
        data: {req : data},
		async: false,
		beforeSend: function (xhr) {
            layindex = layer.open({type: 3});
        },
        complete: function (XMLHttpRequest, textStatus) {
            layer.close(layindex);
        },
		success: function(data, textStatus) {
			if (data["Data"] == true) {
				layer.closeAll();
				type == 1 ? layer.msg('取消赠送成功') : layer.msg('取消退菜成功');
				var num = thisOrderProjectArr.OrderDetailRecord[index].Num;
				var $list = OrderTableProjectsdata[$('#ProjectLists_view tr.layui-this').index()].OrderDetailRecordCount;
				for(var i = 0;i < $list.length;i++){ //删除总数量中取消的部分
					if($list[i].CyddMxCzType == type){
						$list[i].Num = $list[i].Num - num;
						if($list[i].Num == 0){
							$list.splice(i,1)
						}
						break;
					}
				}
				thisOrderProjectArr.OrderDetailRecord.splice(index,1)
				NewsOrder();//更新数据

			} else {
				layer.alert(data["Message"]);
			}
		}
	});
}

//更多按钮 => 订单台号解锁
function UnLock() {
    if ($('.locking').length == 0) {
    	layer.msg('当前桌台未锁定，无需解锁')
        return false;
    }
    var req = { OrderId: inidata.OrderAndTables.OrderId, OrderTableIds: inidata.OrderAndTables.R_Table_Id, IsLocked: false };
    $.ajax({
        type: "POST",
        url: "/Res/Checkout/Unlock",
        data: JSON.stringify(req),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        beforeSend: function (xhr) {
            layindex = layer.open({type: 3});
        },
        complete: function (XMLHttpRequest, textStatus) {
            layer.close(layindex);
        },
        success: function (data, textStatus) {
            if (data.Data == true) {
                layer.alert("解锁成功");
                location.reload();
            }
            else {
                layer.alert(data.Message);
            }
        }
    })
}

//更多按钮 => 订单信息查看
function orderInfoCheck() {
    layer.open({
        type: 2,
        title: '订单信息查看',
        area: ["80%", "80%"],
        content: "/Res/Order/NewOrderEdit?id=" + inidata.OrderAndTables.OrderId,
        maxmin: false
    });
}

//更新订单/统计金额
function NewsOrder() {
    //重组数组--做法/要求/配菜
    var totalprice = 0,
        totalnum = 0;

    for (var i = 0; i < OrderTableProjectsdata.length; i++) {
        var OrderTableProject = OrderTableProjectsdata[i];
        var OrderTableProjectprice = parseFloat(OrderTableProject.Price); //菜品单价
        //++做法/要求/配菜 +金额
        var Extendprice = 0;
        for (var j = 0; j < OrderTableProject.Extend.length; j++) {
            Extendprice += parseFloat(OrderTableProject.Extend[j].Price);
            //OrderTableProjectprice+=parseFloat(OrderTableProject.Extend[j].Price);
        }
        //--退菜/赠送 价格合计
        var DetailRecordCountPrice = 0;
        if (OrderTableProject.OrderDetailRecordCount) {
            for (var j = 0; j < OrderTableProject.OrderDetailRecordCount.length; j++) {
                if (OrderTableProject.OrderDetailRecordCount[j].CyddMxCzType != 3) {
                    DetailRecordCountPrice += parseFloat(OrderTableProject.OrderDetailRecordCount[j].Num) * (parseFloat(OrderTableProject.Price) + parseFloat(Extendprice));
                }
            }
        }
        //菜品单价
        OrderTableProjectprice = parseFloat(OrderTableProject.Price) + parseFloat(Extendprice);

        totalnum += OrderTableProject.Num;

        totalprice += parseFloat(OrderTableProjectprice) * parseFloat(OrderTableProject.Num) - parseFloat(DetailRecordCountPrice);
    }



    //渲染 已点菜品
    var getTpl = ProjectLists_tpml.innerHTML,
        view = document.getElementById('ProjectLists_view');
    laytpl(getTpl).render(OrderTableProjectsdata, function (html) {
        view.innerHTML = html;
    });

    var Projectdom = $('#ProjectLists_view tr.layui-this');
    if(Projectdom.length > 0) {
        var orderContent = Projectdom.closest('.scroll-hidden');
        //设置已选菜单margin 使其永远在下方弹窗上面
        var orderContentH = orderContent.outerHeight();
        var $layero = $('.layui-layer.ProjectLayer');
        var ProjectLayerH = $layero.height();//弹窗高度
        Projectdom.closest('.order-content').css('margin-bottom',ProjectLayerH)
        var thisDomH = Projectdom.height();
        var thisDomY = Projectdom.offset().top + thisDomH;

        var ProjectLayerY = $layero.length > 0 ? $layero.offset().top : 0;
        if (thisDomY > ProjectLayerY) { //判断当前点击的在弹窗上面还是下面
            //在下面
            orderContent.scrollTop(Projectdom.get(0).offsetTop - orderContentH + thisDomH)
        }
    }

    $('#totalprice').text(totalprice.toFixed(2));
    $('#allTotalprice').text((totalprice * OrderTableIds.length).toFixed(2));

    $('#totalnum').text(OrderTableProjectsdata.length);

    $('#sumNum').text(parseFloat((Math.round(OrderTableProjectsdata.length * 100  * OrderTableIds.length * 100)/10000).toFixed(2)))
}

//菜品转台弹窗
function openChangeTable() {
    if (OrderTableIds.length > 1) {
        layer.msg('多桌点餐不支持菜品转台!');
        return false;
    }

    layer.open({
        type: 2,
        anim: -1,
        title: '菜品转台',
        shadeClose: true,
        skin: 'layer-header',
        shade: 0.8,
        area: ['80%', '80%'],
        content: "/Res/MHome/ChangeTable?orderTableId=" + OrderTableIds[0] + "&restaurantId=" + inidata.OrderAndTables.R_Restaurant_Id
    });
}

//多桌点餐
function openChoseTable() {
    //  if(OrderTableIds.length>1){
    //     layer.msg('多桌点餐不支持菜品转台!');
    //     return false;
    //  }

    layer.open({
        type: 2,
        anim: -1,
        title: '多桌点餐',
        shadeClose: true,
        skin: 'layer-header',
        shade: 0.8,
        area: ['80%', '80%'],
        content: "/Res/MHome/ChoseTable?orderTableId=" + OrderTableIds[0]
    });
}

//更多按钮显示
function MoreShow() {
    var src = '<div class="layui-row" style="padding:80px 15px 15px;"><a href="javascript:void(0);" onclick=layer.closeAll("page") class="practice-close" style="top:20px;"><i class="layui-icon">&#x1006;</i></a>' + $('#more-btn-group').html() + '</div>';
    var order_w = $('.Panel-side.left').width(),
        nav_w = $('.actions-vertical').width(),
        win_w = winW,
        width = win_w - (order_w + nav_w),
        height = winH;
    layer.open({
        type: 1,
        anim: -1,
        title: false,
        closeBtn: 0,
        shadeClose: true,
        skin: 'layer-header',
        shade: [0.01, '#fff'],
        offset: ['0', '470px'],
        area: [width + 'px', height + 'px'],
        content: src,
        success: function(layero,index){
        	layero.find('.layui-row a').css({'margin-bottom':'15px','margin-left':'0','margin-right':'10px','transition':'none'});
        }
    });
}

//打列印单
function PrintLXDALL() {
    reportorJs.showPdb(8801, inidata.OrderAndTables.OrderId, Number(OrderTableIds[0]), '', 0, 0, 0, '');
}

//催菜
function UrgeOrder(){
	if(OrderTableProjectsdata.length < 1){
		layer.msg('没有菜品')
		return false;
	}
	
	var $tr = '';
	for(var i=0;i<OrderTableProjectsdata.length;i++){
		if(OrderTableProjectsdata[i].CyddMxStatus > 0){
			var DcNum = OrderTableProjectsdata[i].Num
			var Unit = OrderTableProjectsdata[i].Unit ? OrderTableProjectsdata[i].Unit : '份';
			var RecordCountNum = 0;
			for(var j = 0; j < OrderTableProjectsdata[i].OrderDetailRecordCount.length; j++) {
				if(OrderTableProjectsdata[i].OrderDetailRecordCount[j].CyddMxCzType == 2) {
					RecordCountNum += OrderTableProjectsdata[i].OrderDetailRecordCount[j].Num;
				}
			}
			var DcNum = OrderTableProjectsdata[i].Num - RecordCountNum;
			if(DcNum > 0) {
				$tr += '<tr data-id=' + OrderTableProjectsdata[i].Id + '>' +
					'<td>' + OrderTableProjectsdata[i].ProjectName + '</td>' +
					'<td width="13%"><div class="tc">' + Unit + '</div></td>' +
					'<td width="13%"><div class="tc">' + DcNum + '</div></td>' +
					'<td width="15%"><div class="tc">' + OrderTableProjectsdata[i].Price + '</div></td>' +
					'</tr>';
			}
		}
	}
	
	var str = '<div style="padding:10px;position:relative;overflow:hidden;">'+
					'<table class="layui-table layui-table-header table-head" lay-skin="line" style="margin: 0;">'+
		                '<thead>'+
		                    '<tr>'+
		                        '<th width="">菜名</th>'+
		                        '<th width="13%"><div class="tc">单位</div></th>'+
		                        '<th width="13%"><div class="tc">数量</div></th>'+
		                        '<th width="15%"><div class="tc">单价</div></th>'+
		                    '</tr>'+
		                '</thead>'+
		            '</table>'+
		            '<div class="order-content sm-scroll-hidden" style="max-height:330px;position: initial;">'+
		                '<table class="layui-table" lay-skin="line" style="margin:0;">'+
		                    '<tbody id="InitCookOrder_lists">' + $tr + '</tbody>'+
		                '</table>'+
		            '</div>'+
		        '</div>';
		        
		        
	layer.open({
		type: 1,
		anim: -1,
		title: '催菜',
		shadeClose: true,
		skin: 'layer-header layer-form-group',
		shade: 0,
		area: ['80%', '80%'],
		content: str,
		btn: ['确定', '取消'],
		success: function(layero, index){
			$('#InitCookOrder_lists tr').on('click',function(){
				if($(this).hasClass('layui-this')){
					$(this).removeClass('layui-this')
				}else{
					$(this).addClass('layui-this')
				}
			})
		},
		yes: function(index, layero) {
			var req = [];
			var isSelectTr = $('#InitCookOrder_lists tr.layui-this');
			var index;
			for(var i = 0; i < isSelectTr.length; i++) {
				req.push(isSelectTr.eq(i).attr('data-id'));
			}
			GetUrgeOrder(req);
		},
		btn2: function(index, layero) {}
	});
}

//催菜	提交
function GetUrgeOrder(req){
	if(req.length == 0) {
		layer.msg('请选择菜品!');
		return false;
	}
	var getdata = {
		detailIds: req,
		orderTableId: OrderTableIdString
	};
	$.ajax({
		type: "post",
		url: "/Res/Order/RemindOrder",
		data: JSON.stringify(getdata),
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		async: false,
		beforeSend: function (xhr) {
            layindex = layer.open({type: 3});
        },
        complete: function (XMLHttpRequest, textStatus) {
            layer.close(layindex);
        },
		success: function(data, textStatus) {
			if(data.Data == true) {
				layer.closeAll();
				layer.msg('催菜成功!');
			}

		}
	});
}

//打厨单
function InitCookOrder() {
    //获取打厨单参数
	$.ajax({
		type: "post",
		url: "/Res/Project/InitCookOrderInfo",
		dataType: "json",
		data: {
			orderTableId: OrderTableIds
		},
		async: false,
		beforeSend: function (xhr) {
            layindex = layer.open({type: 3});
        },
        complete: function (XMLHttpRequest, textStatus) {
            layer.close(layindex);
        },
		success: function(data, textStatus) {
			console.log(data)
			InitCookOrderData = data.Data;
			if(data.Data.length < 1) {
				layer.msg('没有可打厨的菜品!');
				return false;
			}
			var $tr = '';
			for(var i = 0; i < data.Data.length; i++) {
				var Unit = data.Data[i].Unit ? data.Data[i].Unit : '份';
				var RecordCountNum = 0;
				for(var j = 0; j < data.Data[i].OrderDetailRecordCount.length; j++) {
					if(data.Data[i].OrderDetailRecordCount[j].CyddMxCzType == 2) {
						RecordCountNum += data.Data[i].OrderDetailRecordCount[j].Num;
					}
				}
				var DcNum = data.Data[i].Num - RecordCountNum;
				if(DcNum > 0) {
					$tr += '<tr>' +
						'<td>' + data.Data[i].ProjectName + '</td>' +
						'<td width="13%"><div class="tc">' + Unit + '</div></td>' +
						'<td width="13%"><div class="tc">' + DcNum + '</div></td>' +
						'<td width="15%"><div class="tc">' + data.Data[i].Price + '</div></td>' +
						'</tr>';
				}
			}
			
			
			
			var str = '<div style="padding:10px;position:relative;overflow:hidden;">'+
					'<table class="layui-table layui-table-header table-head" lay-skin="line" style="margin: 0;">'+
		                '<thead>'+
		                    '<tr>'+
		                        '<th width="">菜名</th>'+
		                        '<th width="13%"><div class="tc">单位</div></th>'+
		                        '<th width="13%"><div class="tc">数量</div></th>'+
		                        '<th width="15%"><div class="tc">单价</div></th>'+
		                    '</tr>'+
		                '</thead>'+
		            '</table>'+
		            '<div class="order-content sm-scroll-hidden" style="max-height:330px;position: initial;">'+
		                '<table class="layui-table" lay-skin="line" style="margin:0;">'+
		                    '<tbody id="InitCookOrder_lists">' + $tr + '</tbody>'+
		                '</table>'+
		            '</div>'+
		        '</div>';
				
			layer.open({
				type: 1,
				anim: -1,
				title: '选择打厨单的菜品',
				shadeClose: true,
				skin: 'layer-header layer-form-group',
				shade: 0,
				area: ['80%', '80%'],
				content: str,
				btn: ['确定', '取消'],
				success: function(layero, index){
					$('#InitCookOrder_lists tr').on('click',function(){
						if($(this).hasClass('layui-this')){
							$(this).removeClass('layui-this')
						}else{
							$(this).addClass('layui-this')
						}
					})
				},
				yes: function(index, layero) {
					var req = [];
					var isSelectTr = $('#InitCookOrder_lists tr.layui-this');
					var index;
					for(var i = 0; i < isSelectTr.length; i++) {
						index = isSelectTr.eq(i).index()
						req.push(InitCookOrderData[index]);
					}
					GetInitCookOrder(req);
				},
				btn2: function(index, layero) {}
			});
		}
	});
}

//提交打厨单
function GetInitCookOrder(req) {
    var table = {
        Restaurant: inidata.OrderAndTables.Restaurant,
        Name: inidata.OrderAndTables.TableName
    };
    if (!req) {
        layer.msg('请选择需要打厨单的菜品!');
        return false;
    }
    var getdata = {
        req: req,
        table: table
    };
    $.ajax({
        type: "post",
        url: "/Res/Home/CookingMenu",
        dataType: "json",
        data: getdata,
        async: false,
        beforeSend: function (xhr) {
            layindex = layer.open({type: 3});
        },
        complete: function (XMLHttpRequest, textStatus) {
            layer.close(layindex);
        },
        success: function (data, textStatus) {
            if (data.Data == true) {
                layer.msg('打厨单成功!');
                setTimeout(function () {
                	layer.open({type: 3,shadeClose: false});
                    location.reload();
                }, 1000)
            }

        }
    });

}

//显示转入转出记录
function OrderDetailRecordCountTable() {
    setTimeout(function () {
        var tableHtml = '';
        for (var i = 0; i < thisOrderProjectArr.OrderDetailRecord.length; i++) {
            var item = thisOrderProjectArr.OrderDetailRecord[i];
            tableHtml += '<tr>' +
                '<td>' + item.CyddMxCzTypeName + '</td>' +
                '<td>' + item.CreateUserName + '</td>' +
                '<td>' + item.CreateDate + '</td>' +
                '<td>' + item.Remark + '</td>' +
                '</tr>';
        }
        if (tableHtml) {
            var html = '<table class="layui-table" style="margin-top:0;">' +
                '<thead>' +
                '<tr>' +
                '<th>类型</th>' +
                '<th>操作员</th>' +
                '<th>操作时间</th>' +
                '<th>备注</th>' +
                '</tr>' +
                '</thead>' +
                '<tbody>' + tableHtml + '</tbody>' +
                '</table>';

        }
        layer.open({
            type: 1,
            title: '转入转出记录',
            shadeClose: true,
            skin: 'layer-header',
            shade: 0.8,
            area: ['800px', '500px'],
            content: html
        });
    }, 100);
}

//跳转结账
function OpenCheckout() {
    //获取参数
    $.ajax({
        url: "/Res/CheckOut/GetOrderTablesByOrderId",
        data: {
            orderId: $("#OrderId").val()
        },
        type: "post",
        dataType: "json",
        beforeSend: function (xhr) {
            layindex = layer.open({type: 3});
        },
        complete: function (XMLHttpRequest, textStatus) {
            layer.close(layindex);
        },
        success: function (data) {
            var urlkey;
            var ordertableid = [];
            if (data.length == 1) {
                var tableidarr = [];
                tableidarr.push(data[0].R_Table_Id);
                var ordertableidArr = {
                    Id: data[0].Id,
                    Name: data[0].Name
                };
                ordertableid.push(ordertableidArr);
                urlkey = "?orderId=" + $("#OrderId").val() + "&tableIds=" + tableidarr.join(',');
                goCheckout(urlkey, ordertableid);
            } else { //弹出选择结账的台
                var tables = '';
                for (var i = 0; i < data.length; i++) {
                    tables += ' <li id="Table_' + data[i].R_Table_Id + '" class="checked" data-no="' + i + '" data-areaid="' + data[i].R_Area_Id + '" style="width: 131px;">' +
                        '<a href="javascript:void(0);">' +
                        '<div class="MealTable-head flex">' +
                        '<span class="item MealTable-number flex-item">' + data[i].R_Table_Id + '</span>' +
                        '</div>' +
                        '<div class="MealTable-title">' + data[i].Name + '</div>' +
                        '</a>' +
                        '</li>';
                }
                var str = '<div class="MealTable-lists layer-tables" style="margin-right:0;">' +
                    '<ul id="AddTable">' + tables + '</ul>' +
                    '</div>';

                layer.open({
                    type: 1,
                    btn: ['确定', '取消'],
                    yes: function (index, layero) {
                        var tablebox = $('.layer-tables ul li');
                        var tablesarr = [];
                        var ordertableid = [];
                        for (var i = 0; i < tablebox.length; i++) {
                            if (tablebox.eq(i).hasClass('checked')) { //已选
                                var tableno = tablebox.eq(i).attr('data-no');
                                tablesarr.push(data[tableno].R_Table_Id);
                                var ordertableidArr = {
                                    Id: data[tableno].Id,
                                    Name: data[tableno].Name
                                };
                                ordertableid.push(ordertableidArr);
                            }
                        }
                        urlkey = "?orderId=" + $("#OrderId").val() + "&tableIds=" + tablesarr.join(',');
                        goCheckout(urlkey, ordertableid);

                    },
                    btn2: function (index, layero) {
                        //按钮【按钮二】的回调
                    }
                    , success: function (layero) {
                        var $checkbox = $('<div class="layui-form" lay-filter="empty_GetOrderTable" style="position: absolute;bottom: 12px;left: 10px;"><input type="checkbox" name="isAllSelected" title="全选" class="isAllSelected" checked="checked" lay-filter="empty_GetOrderTable"></div>')
                        layero.children('.layui-layer-btn').append($checkbox);
                        var isAllSelected = layero.find('.isAllSelected'); //全选标签
                        var $li = layero.find('li');

                        form.render('checkbox', 'empty_GetOrderTable');

                        form.on('checkbox(empty_GetOrderTable)', function (data) {
                            if (data.elem.checked == true) {//全选
                                $li.addClass('checked')
                            } else {
                                $li.removeClass('checked')
                            }

                        });
                        var len = $li.length;
                        $li.click(function () {
                            if ($(this).hasClass('checked')) {
                                $(this).removeClass('checked');
                            } else {
                                $(this).addClass('checked');
                            }
                            if ($li.filter('.checked').length === len) {
                                isAllSelected.get(0).checked = true;
                            } else {
                                isAllSelected.attr("checked", false);
                            }
                            form.render('checkbox', 'empty_GetOrderTable');
                        });

                    },
                    title: "选择要结账的餐台",
                    area: ["600px", "500px"],
                    content: str,
                    maxmin: false
                });

                //				$('.layer-tables ul li').click(function() {
                //					if($(this).hasClass('checked')) {
                //						$(this).removeClass('checked');
                //					} else {
                //						$(this).addClass('checked');
                //					}
                //				})

            }

        },
        error: function (msg) {
            console.log(msg.responseText);
        }
    });
}

//跳转结账
function goCheckout(urlkey, orderTableInfos) {
    //后台判断所选台是否存在保存为落单的菜品
    $.ajax({
        type: "post",
        url: "/Res/Project/JudgeOrderPay",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(orderTableInfos),
        async: false,
        beforeSend: function (xhr) {
            layindex = layer.open({type: 3});
        },
        complete: function (XMLHttpRequest, textStatus) {
            layer.close(layindex);
        },
        success: function (data, textStatus) {
            if (data.Data == 2000) {
                //全部所选台没有保存为落单的菜品
                //跳转结账
                var index = parent.layer.getFrameIndex(window.name);
                parent.layer.iframeSrc(index, '/Res/CheckOut/OpenCheckout' + urlkey);
                parent.layer.title('结账', index);
            } else if (data.Data == 3000) {
                layer.confirm('' + data.Message + '？', {
                    btn: ['继续结账', '取消'] //按钮
                }, function () {
                    var index = parent.layer.getFrameIndex(window.name);
                    parent.layer.iframeSrc(index, '/Res/CheckOut/OpenCheckout' + urlkey);
                    parent.layer.title('结账', index);
                }, function () {

                    //return false;
                });
                return false;
            } else if (data.Data == 1000) {
                layer.alert(data.Message);
                return false;
            } else {
                layer.alert('所选台有保存未落单的菜品，请处理后再结账!');
                return false;
            }

        }
    });

}



//验证是否存在新增，未保存的菜品
function isNewProject() {
    var is = false;
    for (var i = 0; i < OrderTableProjectsdata.length; i++) {
        if (OrderTableProjectsdata[i].Id <= 0) {
            is = true;
        }
    }
    return is;
}

//点餐页面 菜品自适应
function projectAndDetailsAuto() {
    $('#ProjectAndDetails_view').parent().height(winH - $('#CategoryList_view').parent().outerHeight() - $('#actionsbtn_view').outerHeight() - 30)
}

//点餐页面  取消按钮（退出桌台点餐）
function cancelOut(){
	if(isNewProject()){
		layer.confirm('有未保存的菜品，是否保存？', {
		  btn: ['保存','退出'] //按钮
		}, function(){
			$('#actionsbtn_view').find('button[name=Keep]').click();
		}, function(){
			$.ajax({
		        type: "post",
		        url: "/Res/Order/UpdateOrderTableIsControl",
		        dataType: "json",
		        beforeSend: function (xhr) {
		            layindex = layer.open({type: 3});
		        },
		        data: {ordertableIds: OrderTableIds,isControl: false},
		        async: false,
		        success: function (data, textStatus) {
		            if (data.Data == true) {
						history.go(-1);
		            } else {
		                layer.alert(data.Message);
		            }
		        }
		    });
		});
		//阻止关闭
		return false;
	}else{
		
		$.ajax({
	        type: "post",
	        url: "/Res/Order/UpdateOrderTableIsControl",
	        dataType: "json",
	        data: {ordertableIds: OrderTableIds,isControl: false},
	        async: false,
	        beforeSend: function (xhr) {
	            layindex = layer.open({type: 3});
	        },
	        success: function (data, textStatus) {
	            if (data.Data == true) {
	            	var chat = $.connection.systemHub;
					chat.hubName = 'systemHub';
					chat.connection.start();
					$.connection.hub.start().done(function() {
						chat.server.notifyResServiceRefersh(true);
	                	//初始化动作全部完成时
						history.go(-1);
					});
	            } else {
	                layer.alert(data.Message);
	            }
	        }
	    });
	}
}


//监控已选菜品横向滚动条	滚动
$('.chooseOrder .order-content').on('scroll',function(){
	$(this).prev().scrollLeft($(this).scrollLeft())
})
$('.chooseOrder > .Panel-side > .table-header').on('scroll',function(){
	$(this).next().scrollLeft($(this).scrollLeft())
})