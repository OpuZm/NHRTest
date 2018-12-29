/**
 * 点菜
 */

var inidata; //初始化得到的数据
var OrderTableIds;
var OrderTableProjectsdata = []; //已选订单的数组 
var thisOrderProjectArr = [] //已选订单的数组  当前选中数组
var thisProjectArr; //
var laytpl;
var thisProjectsIndex;
var element;
var form;
var userInfo; //登陆用户信息
var enter_type = 1;
var allStatus = 1;

var isAllSelected = false;//是否全选

var winW = $(window).width();
var winH = $(window).height();

layui.use(['element', 'form', 'laytpl'], function() {
	element = layui.element;
	form = layui.form;
	laytpl = layui.laytpl;

	//获取参数
	$.ajax({
		url: "/Res/Project/ProjectClearInit",
		data: {},
		type: "post",
		dataType: "json",
		beforeSend: function (xhr) {
            layindex = layer.open({type: 3});
        },
        complete: function (XMLHttpRequest, textStatus) {
            layer.close(layindex);
        },
		success: function(data) {
			inidata = data;

			//渲染 菜品分类
			var getTpl = CategoryList_tpml.innerHTML,
				view = document.getElementById('CategoryList_view');
			laytpl(getTpl).render(data.CategoryList, function(html) {
				view.innerHTML = html;
			});

			//渲染 菜品
			var getTpl = ProjectAndDetails_tpml.innerHTML,
				view = document.getElementById('ProjectAndDetails_view');
			laytpl(getTpl).render(data.ProjectAndDetails, function(html) {
				view.innerHTML = html;
            });
			
			

			//监听搜索
			$('#KeyWord').bind('input propertychange', function(e) {
				var value = $(this).val().toUpperCase();
				KeyWord(value);
			})

			$('.CategoryListTab').width($(window).width() - 470);

			element.init();
			projectAndDetailsContainerAuto();
//			projectAndDetailsAuto();//渲染菜品主框  设置当前可支持的最大高度
			AddProject();

			//监听菜品分类一 点击
			element.on('tab(ClassTab)', function(data) {
				//判断是否需要显示  滚动按钮
				var $content = data.elem.find('.layui-tab-content');
				var maxWidth = $content.width();
				var a_width = 0;
				$.each($content.find('.layui-tab-item').eq(data.index).find('.class-group a'),function(i){
					a_width += $(this).outerWidth();
					if(i > 0)a_width += 10;
				})
				$content.hasClass('expand') ? maxWidth += 90 : maxWidth ;
				maxWidth >= a_width ? $content.removeClass('expand expand-sm') : $content.addClass('expand');
				if(winW <= 1024)$content.addClass('expand-sm')
				
				//点击的分类
				var newsArr = [];
				if(data.index == 0) { //全部
					newsArr = inidata.ProjectAndDetails;
				} else {
					var CategoryList = inidata.CategoryList[data.index - 1];
					if(CategoryList.ChildList.length > 0) { //有子分类
						for(var i = 0; i < CategoryList.ChildList.length; i++) {
							classid = CategoryList.ChildList[i].Id;
							for(var j = 0; j < inidata.ProjectAndDetails.length; j++) {
								var item = inidata.ProjectAndDetails[j];
								if(classid == item.Category) { //成立
									newsArr.push(item);
								}
							}
						}
					} else { //没有子分类的
						for(var j = 0; j < inidata.ProjectAndDetails.length; j++) {
							var item = inidata.ProjectAndDetails[j];
							if(CategoryList.Id == item.Category) { //成立
								newsArr.push(item);
							}
						}
					}
				}
				var getTpl = ProjectAndDetails_tpml.innerHTML,
					view = document.getElementById('ProjectAndDetails_view');
				laytpl(getTpl).render(newsArr, function(html) {
					view.innerHTML = html;
				});
				
				//保证点击后   重置二级分类按钮到第一个
				$(data.elem).closest('.layui-tab').find('.layui-tab-content .layui-tab-item').eq(data.index).find('.class-group a').eq(0).addClass('layui-this').siblings().removeClass('layui-this');
//				projectAndDetailsAuto();//菜品父元素自适应
				T_list_auto(false, true);//菜品宽度自适应
			});
			
			
			
			//监听二级分类点击
			$('#CategoryList_view .class-group').delegate('a.layui-btn', 'click', function(event) {
				//var Projectlists=$('#ProjectAndDetails_view li');
				var classno = $(this).parent('.class-group').parent('.layui-tab-item').index();
				$(this).addClass('layui-this').siblings('a').removeClass('layui-this');
				var newsArr = [];
				if(classno == 0) { //全部--全部
					newsArr = inidata.ProjectAndDetails;
				} else {
					var btnno = $(this).index();
					if(btnno == 0) { //分类下的全部
						$('#CategoryList_view .layui-tab-title .layui-this').click();
						return false;
					} else {
						var classdata = inidata.CategoryList[classno - 1];
						var classid = classdata.ChildList[btnno - 1].Id;
						for(var j = 0; j < inidata.ProjectAndDetails.length; j++) {
							var item = inidata.ProjectAndDetails[j];
							if(classid == item.Category) { //成立
								newsArr.push(item);
							}
						}
						var getTpl = ProjectAndDetails_tpml.innerHTML,
							view = document.getElementById('ProjectAndDetails_view');
						laytpl(getTpl).render(newsArr, function(html) {
							view.innerHTML = html;
						});
						T_list_auto(false, true);
					}
				}
			})

//			//监听套餐菜品分类一 点击
//			element.on('tab(fClassTab)', function(data) {
//				//点击的分类
//				var newsArr = [];
//				if(data.index == 0) { //全部
//					newsArr = inidata.Projects;
//				} else {
//					var CategoryList = inidata.CategoryList[data.index - 1];
//					if(CategoryList.ChildList.length > 0) { //有子分类
//						for(var i = 0; i < CategoryList.ChildList.length; i++) {
//							classid = CategoryList.ChildList[i].Id;
//							for(var j = 0; j < inidata.Projects.length; j++) {
//								var item = inidata.Projects[j];
//								if(classid == item.Category) { //成立
//									newsArr.push(item);
//								}
//							}
//						}
//						$('#CategoryList_view .layui-tab-content .layui-tab-item.layui-show .class-group a.layui-btn').eq(0).addClass('layui-this').siblings('a').removeClass('layui-this');
//					} else { //没有子分类的
//						for(var j = 0; j < inidata.Projects.length; j++) {
//							var item = inidata.Projects[j];
//							if(CategoryList.Id == item.Category) { //成立
//								newsArr.push(item);
//							}
//						}
//					}
//				}
//				var ProjectsHtml = getProjectsHtml(newsArr);
//				$('#Tc_ProjectsLists ul').html(ProjectsHtml);
//				//根据剩余高度设置菜品高度   62是按钮组高度   194 是已选菜品宽度
//				$('#Tc_ProjectsLists').css('height',$(window).height() - $('#TC_CategoryListTab').outerHeight() - 64 - 204 - 43);
//				
//				//自适应
//				tcReviseAuto();
//			});
			
//          projectAndDetailsAuto()
			T_list_auto(true, true);
			
			
		},
		error: function(msg) {
			console.log(msg.responseText);
		}
	});
	
	
	//全选按钮
	form.on('switch(allSelect)', function(data) {
		if(data.elem.checked){//若全选
			isAllSelected = true;
			var tr = $(ProjectListsView).children('tr');
			tr.addClass('layui-this');
			$.each(tr,function(){
				var index = $(this).attr('data-index');
				OrderTableProjectsdata[index].T_Orderselected = true;//是否选中
			})
		}else{
			isAllSelected = false;
			var tr = $(ProjectListsView).children('tr');
			tr.removeClass('layui-this');
			$.each(tr,function(){
				var index = $(this).attr('data-index');
				OrderTableProjectsdata[index].T_Orderselected = false;//是否选中
			})
		}
	});
	
	//是否启用库存 单选按钮
	form.on('switch(orderSelect)', function(data) {
		var index = $(data.elem).closest('tr').attr('data-index');
		OrderTableProjectsdata[index].IsStock = data.elem.checked;
	});
});

//搜索检索
function KeyWord(value) {
	var newsArr = [];
	if(!value) {
		newsArr = inidata.ProjectAndDetails;
	} else {
		for(var i = 0; i < inidata.ProjectAndDetails.length; i++) {
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
	laytpl(getTpl).render(newsArr, function(html) {
		view.innerHTML = html;
	});
	T_list_auto(false, true);
}

//点菜  选择菜品
function AddProject() {
	$('#ProjectAndDetails_view').delegate('li', 'click', function(event) {
		
		var CyddMxType = $(this).attr('data-CyddMxType');
		var id = $(this).attr('data-id');
		var itemdata = '';

		//判断是否已经选中
		for(var i=0;i<OrderTableProjectsdata.length;i++){
			var item = OrderTableProjectsdata[i];
			if (item.CyddMxType==CyddMxType && item.Id==id) {
				$(this).removeClass('layui-this');
				delSelect([i]);//删除选中项
				
				return false;
			}
		}

		//循环搜索   获取菜品信息
		for(var i = 0; i < inidata.ProjectAndDetails.length; i++) {
			var item = inidata.ProjectAndDetails[i];
			if (item.CyddMxType==CyddMxType && item.Id==id) {
				item.T_isSelected = true; //是否选中
				item.T_isSelectedNum = i; //选中的是第几个
				itemdata = item;
				
			}
		}
		
		$(this).addClass('layui-this');
		//添加数组到已选菜单
		AddSelect(itemdata);
		
	})
}


//已选菜单 选中
$(document).on('click','#ProjectLists_view tr',function(e){
//	if(isAllSelected)return;
	var index = $(this).attr('data-index')
	if($(this).hasClass('layui-this')){
		$(this).removeClass('layui-this');
		OrderTableProjectsdata[index].T_Orderselected = false;//是否选中
		if(isAllSelected){
			$('#allSelected').prop('checked',false);
			isAllSelected = false;
			form.render('checkbox');
		}
		return false;
	}
	$(this).addClass('layui-this');
	OrderTableProjectsdata[index].T_Orderselected = true;
	var $tr = $(this).parent().children('tr');
	if($tr.filter('.layui-this').length == $tr.length){
		$('#allSelected').prop('checked',true)
		isAllSelected = true;
		form.render('checkbox');
	}
});

//已经菜单	删除按钮
function orderDelete(_this){
	var index = $(_this).closest('tr').attr('data-index');
	var id = $(_this).closest('tr').attr('data-id');
	var cyddmxtype = $(_this).closest('tr').attr('data-cyddmxtype');
	
	$('#ProjectAndDetails_view li[data-cyddmxtype='+cyddmxtype+'][data-id='+id+']').removeClass('layui-this');
	
	delSelect([index]);
	
}

//已选订单   添加
function AddSelect(pro) {
	var data = {}
	$.extend(true, data,pro);
	OrderTableProjectsdata.push(data);
	OrderTableProjectsdata[OrderTableProjectsdata.length - 1].T_Orderselected = true;//是否选中
	//更新订单
	NewsOrder()
}


//已选订单 删除
function delSelect(pro){
	var index = 0;
	for(var i = 0;i<pro.length;i++){
		inidata.ProjectAndDetails[OrderTableProjectsdata[pro[i] - index].T_isSelectedNum].T_isSelected = false;
		OrderTableProjectsdata.splice(pro[i] - index,1);
		index++;
	}
	//更新订单
	NewsOrder()
}


var ProjectLists = ProjectLists_tpml.innerHTML
, ProjectListsView = document.getElementById('ProjectLists_view');
//更新已选订单
function NewsOrder(type){
	//渲染 已点菜品
    laytpl(ProjectLists).render(OrderTableProjectsdata, function (html) {
        ProjectListsView.innerHTML = html;
        form.render('checkbox');
        
        var $tr = $('#ProjectLists_view tr');
		if($tr.filter('.layui-this').length == $tr.length){
			$('#allSelected').prop('checked',true)
			isAllSelected = true;
			form.render('checkbox');
		}
    });
}


//已选菜品  加
function orederNumPlus(_this){
	var index = $(_this).closest('tr').attr('data-index');
	OrderTableProjectsdata[index].Stock++;
	$(_this).siblings('.num').val(OrderTableProjectsdata[index].Stock);
	return false;
}

//已选菜品  减
function orederNumMinus(_this){
	var index = $(_this).closest('tr').attr('data-index');
	if(--OrderTableProjectsdata[index].Stock < 0)OrderTableProjectsdata[index].Stock = 0;
	$(_this).siblings('.num').val(Math.floor(OrderTableProjectsdata[index].Stock * 100) / 100  );
	return false;
}



//中间操作按钮组 全部加1
function allPlus(){
	var tr = $('tr.layui-this',ProjectListsView);
	$.each(tr,function(){
		var index = $(this).attr('data-index');
		OrderTableProjectsdata[index].Stock++;
		$(this).find('.num').val(OrderTableProjectsdata[index].Stock);
	})
}


//中间操作按钮组 全部减1
function allMinus(){
	var tr = $('tr.layui-this',ProjectListsView);
	$.each(tr,function(){
		var index = $(this).attr('data-index');
		if(--OrderTableProjectsdata[index].Stock < 0){
			OrderTableProjectsdata[index].Stock = 0;
		}//若小于0  直接进入下一轮
		$(this).find('.num').val(Math.floor(OrderTableProjectsdata[index].Stock * 100) / 100  );
	})
}

//中间操作按钮组	估清
function selectClear(){
	layer.confirm('是否确认进行估清?',{icon: 3, title:'友情提示'}, function(index) {
		var tr = $('tr.layui-this',ProjectListsView);
		$.each(tr,function(){
			var index = $(this).attr('data-index');
			OrderTableProjectsdata[index].Stock = 0;
			$(this).find('.num').val(0);
		})
		layer.close(index);
	});
}


//中间操作按钮组	批量删除
function selectDelete(){
	layer.confirm('是否确认删除选中菜品?',{icon: 3, title:'友情提示'}, function(index) {
		var tr = $('tr.layui-this',ProjectListsView);
		var indexArry = []
		$.each(tr,function(){
			var tr = $(this).closest('tr');
			var index = tr.attr('data-index');
			var id = tr.attr('data-id');
			var cyddmxtype = tr.attr('data-cyddmxtype');
			$('#ProjectAndDetails_view li[data-cyddmxtype='+cyddmxtype+'][data-id='+id+']').removeClass('layui-this');
			indexArry.push(index);
		})
		delSelect(indexArry);
		layer.close(index);
	});
}

//已估清 启用的
function isSelecrClear(){
	var classno = $('#CategoryList_view .layui-tab-title li.layui-this').index();
	var btnno = $('#CategoryList_view .layui-tab-content .layui-tab-item').eq(classno).find('.class-group a.layui-this').index();
	var newArr = [];//已经启用数组
	var orderNewArr = [];//当前右侧菜品  （根据  1 ，2级分类筛选）
	OrderTableProjectsdata = [];//清空已选菜品
	//全部重置为  未选中
	for(var i=0;i<inidata.ProjectAndDetails.length;i++){
		inidata.ProjectAndDetails[i].T_isSelected = false;
	}
//	$('#ProjectAndDetails_view li').removeClass('layui-this');
	
	if(classno == 0){ //如果  当前   1级分类是   全部
		for(var j=0;j<inidata.ProjectAndDetails.length;j++){
			var item = inidata.ProjectAndDetails[j];
			orderNewArr.push(item);
			if(item.IsStock){
				item.T_isSelected = true; //是否选中
				item.T_Orderselected = true; //已选菜品中 是否选中
				item.T_isSelectedNum = j; //选中的是第几个
				newArr.push(item);
			}
		}
	}else if(btnno == 0){ //如果 当前2级分类是全部
		var CategoryList = inidata.CategoryList[classno - 1];
		if(CategoryList.ChildList.length > 0) { //有子分类
			for(var i = 0; i < CategoryList.ChildList.length; i++) {
				var classid = CategoryList.ChildList[i].Id;
				for(var j = 0; j < inidata.ProjectAndDetails.length; j++) {
					var item = inidata.ProjectAndDetails[j];
					if(classid == item.Category) { //成立
						orderNewArr.push(item);
						if(item.IsStock){
							item.T_isSelected = true; //是否选中
							item.T_Orderselected = true; //已选菜品中 是否选中
							item.T_isSelectedNum = j; //选中的是第几个
							newArr.push(item);
						}
					}
				}
			}
		} else { //没有子分类的
			for(var j = 0; j < inidata.ProjectAndDetails.length; j++) {
				var item = inidata.ProjectAndDetails[j];
				if(CategoryList.Id == item.Category) { //成立
					orderNewArr.push(item);
					if(item.IsStock){
						item.T_isSelected = true; //是否选中
						item.T_Orderselected = true; //已选菜品中 是否选中
						item.T_isSelectedNum = j; //选中的是第几个
						newArr.push(item);
					}
				}
			}
		}
	}else{ //选择了二级分类
		var CategoryList = inidata.CategoryList[classno - 1];
		var classid = CategoryList.ChildList[btnno - 1].Id;
		for(var j = 0; j < inidata.ProjectAndDetails.length; j++) {
			var item = inidata.ProjectAndDetails[j];
			if(classid == item.Category ) { //成立
				orderNewArr.push(item);
				if(item.IsStock){
					item.T_isSelected = true; //是否选中
					item.T_Orderselected = true; //已选菜品中 是否选中
					item.T_isSelectedNum = j; //选中的是第几个
					newArr.push(item);
				}
			}
		}
	}
	$.extend(true, OrderTableProjectsdata,newArr);
	
	//渲染菜品列表  （ 根据 1，2级分类）
	var getTpl = ProjectAndDetails_tpml.innerHTML,
		view = document.getElementById('ProjectAndDetails_view');
	laytpl(getTpl).render(orderNewArr, function(html) {
		view.innerHTML = html;
	});
	T_list_auto(false, true);//菜品宽度自适应
	
	//全选按钮   选中
	$('#allSelected').prop('checked',true)
	isAllSelected = true;
	form.render('checkbox');
	NewsOrder();//渲染已点菜品列表
	
//	var CategoryList = inidata.CategoryList[data.index - 1];
//	if(CategoryList.ChildList.length > 0) { //有子分类
//		for(var i = 0; i < CategoryList.ChildList.length; i++) {
//			classid = CategoryList.ChildList[i].Id;
//			for(var j = 0; j < inidata.ProjectAndDetails.length; j++) {
//				var item = inidata.ProjectAndDetails[j];
//				if(classid == item.Category) { //成立
//					newsArr.push(item);
//				}
//			}
//		}
//	} else { //没有子分类的
//		for(var j = 0; j < inidata.ProjectAndDetails.length; j++) {
//			var item = inidata.ProjectAndDetails[j];
//			if(CategoryList.Id == item.Category) { //成立
//				newsArr.push(item);
//			}
//		}
//	}
	
	
	
//	if(classno == 0) { //全部--全部
//		newsArr = inidata.ProjectAndDetails;
//	} else {
//		var btnno = $(this).index();
//		if(btnno == 0) { //分类下的全部
//			$('#CategoryList_view .layui-tab-title .layui-this').click();
//			return false;
//		} else {
//			var classdata = inidata.CategoryList[classno - 1];
//			var classid = classdata.ChildList[btnno - 1].Id;
//			for(var j = 0; j < inidata.ProjectAndDetails.length; j++) {
//				var item = inidata.ProjectAndDetails[j];
//				if(classid == item.Category) { //成立
//					newsArr.push(item);
//				}
//			}
//			var getTpl = ProjectAndDetails_tpml.innerHTML,
//				view = document.getElementById('ProjectAndDetails_view');
//			laytpl(getTpl).render(newsArr, function(html) {
//				view.innerHTML = html;
//			});
//			T_list_auto(false, true);
//		}
//	}
}



//弹出可输入的数字键盘
function NumberKeyboard(Event, thisdom) {

	var Projectdom = $('#ProjectLists_view tr.layui-this');
	if(Projectdom.length < 1) {
		layer.msg('请选择要操作的菜品');
		return false;
	}

	var max = 10000000;
	
	
	layer.closeAll('page');
	var str = '<div class="keyboard-number keyboard number-input">' +
		'<div class="Keyboard-input-group">' +
		'<input type="text" id="KeyboardInput"  class="layui-input" value=""/>' +
		'</div>' +
		'<ul class="row">' +
		'<li data-val="1">1</li>' +
		'<li data-val="2">2</li>' +
		'<li data-val="3">3</li>' +
		'<li data-val="4">4</li>' +
		'<li data-val="5">5</li>' +
		'<li data-val="6">6</li>' +
		'<li data-val="7">7</li>' +
		'<li data-val="8">8</li>' +
		'<li data-val="9">9</li>' +
		'<li data-val="0" style="width: 62%;">0</li>' +
		'<li data-val=".">.</li>' +
		'</ul>' +
		'<div class="Keyboard-btn">' +
		'<a href="javascript:void(0);" data-val="del" ><i class="layui-icon" >&#xe65c;</i></a>' +
		'<a href="javascript:void(0);" data-val="success">确定</a>' +
		'</div>' +
		'</div>';
	layer.open({
		type: 1,
		title: false,
		shadeClose: true,
		shade: [0.1, '#fff'],
		isOutAnim: false,
		skin: 'keyboard-box',
		area: ['380px', '420px'],
		content: str
	});
	var inputdom = $('#KeyboardInput');
	inputdom.focus();
	
	//数字点击
	$('.keyboard-number li').on("click", function() {
	
		var value = $(this).attr('data-val');
		var inputval = inputdom.val();
		var newval = inputval + value;
		//禁止多个 .
		if(value == '.') {
			if(inputval.indexOf(".") >= 0) {
				return false;
			}
		}
	
	
		if(newval > max ) {
			layer.msg('数量不能小于0或大于' + max);
			return false;
		}
	
		if(newval.indexOf(".") >= 0) {
			var numindex = parseInt(newval.indexOf("."), 10);
			if(numindex == 0) {
				inputdom.val("");
				layer.msg("输入的数字不规范");
				return false;
			}
			var head = newval.substring(0, numindex);
			var bottom = newval.substring(numindex, numindex + 3);
			var fianlNum = head + bottom;
			newval = fianlNum;
		}
	
		inputdom.val(newval);
		return false;
	});

	//监听输入是否正确
	//监听输入的值是否为数字
	inputdom.bind('input propertychange', function(e) {
		var value = $(this).val();
		if(value == null || value == '') {
			return false;
		}
		if(!isNaN(value)) {
			var userreg = /^[0-9]+([.]{1}[0-9]{1,2})?$/;
			if(userreg.test(value)) {
				if(value < 0) {
					layer.msg('请输入数字!');
					$(this).val('');
				} else if(value > max) {
					layer.msg('数量不能大于' + max);
					$(this).val('');
				}
			} else {
				var numindex = parseInt(value.indexOf("."), 10);
				if(numindex == 0) {
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

	$('.Keyboard-btn a').on("click", function() {
		var value = $(this).attr('data-val');
		var inputval = inputdom.val();
		if(value == 'del') { //删除
			var newval = inputval.substring(0, inputval.length - 1);
			inputdom.val(newval);
		} else if(value == 'success') { //确定
			if(inputval < 0 || !inputval) {
				layer.msg('请输入大于0的数字!');
				return false;
			}
	
			$.each(Projectdom,function(){
				var index = $(this).attr('data-index');
				OrderTableProjectsdata[index].Stock = parseFloat(inputval);
				$(this).find('.num').val(inputval);
			})
			layer.closeAll('page');
		}
	
		return false;
	});
}
//提交已选表单信息
function submitOrderTable(){
	if(OrderTableProjectsdata.length === 0){
		layer.msg('请选择需要提交的菜品')
		return false;
	}
	layer.confirm('是否确认提交?',{icon: 3, title:'友情提示'}, function(index) {
		layer.close(index);
		$.ajax({
            type: "post",
            url: "/Res/Project/ProjectStockSubmit",
            dataType: "json",
            data: JSON.stringify(OrderTableProjectsdata),
            contentType: "application/json; charset=utf-8",
            beforeSend: function (xhr) {
	            layindex = layer.open({type: 3});
	        },
	        complete: function (XMLHttpRequest, textStatus) {
	            layer.close(layindex);
	        },
            success: function (data, textStatus) {
            	if(data.Data == true){
            		parent.layer.msg('提交成功');
            		parent.layer.close(parent.layer.getFrameIndex(window.name));
            	}
            },
            error: function (XMLHttpRequest, textStatus) {
                layer.close(index);
                layer.msg('加载失败，请刷新后重试')
            }
        });
		
	});
}

//阻止启用库存按钮冒泡事件
$(document).on('click','#ProjectLists_view .layui-form-switch,#ProjectLists_view .minus,#ProjectLists_view .plus',function(e){
	e.stopPropagation();
})

//点餐页面 菜品宽度自适应
//function projectAndDetailsAuto(){
//	var $mealTable = $('#ProjectAndDetails_view');
//	var $mealTable_w = $mealTable.width();
//	var maxW = 120;
//	var outW = 12;
//	if(winW <= 1024){
//		maxW = 100;
//		outW = 2;
//	}
//	line_sum = Math.floor($mealTable_w / maxW);
//	$mealTable_li_w = $mealTable_w / line_sum - outW;//120为最小宽度(包含padding margin border)
//	$mealTable.children('li').css('width',$mealTable_li_w);
//}


//点餐页面 菜品自适应
function projectAndDetailsContainerAuto(){
	var h = $(window).height() - $('#CategoryList_view').parent().outerHeight() - $('#actionsbtn_view').outerHeight();
	var minH = 82;
	if(winW <= 1024)minH = 44; 
	h = Math.floor(h/minH)*minH
	$('#ProjectAndDetails_view').parent().height(h)
}