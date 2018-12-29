/**
*结账
*/

var inidata;  //初始化数据
var laytpl;
var form;
var element;
var table;
var OrderPaidRecordList = [];//当前付款明细
var AuthObjArr = [];//授权对象列表
var userDiscountRate = 1;
var userClearMoney = 0;//权限抹零金额

layui.use(['element', 'form', 'laytpl', 'layer','table'], function () {
    var layer = layui.layer;
    element = layui.element;
    table=layui.table;
    form = layui.form;
    laytpl = layui.laytpl;
    var winheight = $(window).height() - 70;
    $('.actions-vertical li a').css('line-height', winheight / 12 + 'px');

    var orderId = getUrlParam('orderId'),
        tableids = getUrlParam('tableIds'),
        tabIdArr = tableids.split(',');
    var para = { OrderId: orderId, TableIds: tabIdArr };


    //获取参数
    $.ajax({
        url: "/Res/CheckOut/GetOrderInfoForReCheckout",
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
            inidata = data;
            inidata.SourceId = 0;
            inidata.SourceName = "";
            userClearMoney = inidata.AuthClearValue;
            
            //去小数方法  文字提示
			switch(inidata.CheckoutRound){
				case 1:
					CheckoutRoundHTML = "四舍五入";
					break;
				case 2:
					CheckoutRoundHTML = "只舍不入";
					break;
				case 3:
					CheckoutRoundHTML = "只入不舍";
					break;
			}

            //渲染付款方式
            var getTpl = PayTypeList_tpml.innerHTML
                , view = document.getElementById('PayTypeList_view');
            laytpl(getTpl).render(inidata.PayTypeList, function (html) {
                view.innerHTML = html;
            });
            
            //初始化付款明细
            for (var i = 0; i < inidata.PaidRecordList.length; i++) {
                var disabled = 0;
                var item = inidata.PaidRecordList[i];
                if (item.CyddJzType != 2 || item.CyddJzStatus != 2) {//非消费记录
                    disabled = 1;
                }

                var Arr = {
                    CyddJzStatus: item.CyddJzStatus
                    , CyddJzType: item.CyddJzType
                    , CyddPayType: item.CyddPayType
                    , Id: item.Id
                    , isDisabled: disabled
                    , PayAmount: item.PayAmount
                    , Remark: item.Remark
                    , SourceId: item.SourceId
                    , SourceName: item.SourceName
                    , PayTypeName: item.PayTypeName
                    , JzTypeName: item.JzTypeName
                    , CreateUserName: item.CreateUserName
                    , CreateDate: item.CreateDate
                };
                OrderPaidRecordList.push(Arr);
            }
            newOrderPaidRecordList();

            //渲染数据
            UpdateData(0);


            //切换桌台合计当前桌价格，数量
            element.on('tab(OrderTab)', function (data) {
                TotalOrderPrice(data.index);

                $('#OrderTableName').text(inidata.OrderTableList[data.index].Name);
                //修改当前餐台名称
            });

            //菜品选中
            $('#OrderTablesAndProject_view').delegate(' .layui-tab-item .order-content .layui-table tbody tr', 'click', function (event) {
                if ($(this).hasClass('layui-this')) {
                    $(this).removeClass('layui-this');
                } else {
                    $(this).addClass('layui-this');
                }
                var thisOrderTr = $('#OrderTablesAndProject_view .layui-tab-item.layui-show .order-content .layui-table tbody');
                //当前台 菜品数量
                var thisTableDetailNum = thisOrderTr.find('tr').length;
                //选中的tr数量
                var thisTableDetailActiveNum = thisOrderTr.find('tr.layui-this').length;
                if (thisTableDetailActiveNum == thisTableDetailNum) {//全选状态
                    $('#allcheck').each(function () { this.checked = true; });
                } else {
                    $('#allcheck').each(function () { this.checked = false; });
                }
                form.render('checkbox');

            });

            //当前台全选与反选
            form.on('checkbox(allcheck)', function (data) {
                var thisOrderTr = $('#OrderTablesAndProject_view .layui-tab-item.layui-show .order-content .layui-table tbody');
                if (data.elem.checked == true) {//全选
                    thisOrderTr.find('tr').addClass('layui-this');
                } else {
                    thisOrderTr.find('tr').removeClass('layui-this');
                }

            });


            //付款方式 点击监听
            $('#PayTypeList_view li').on("click", function () {
                $(this).addClass('active').siblings('li').removeClass('active');
                var payid = $(this).attr('data-default-key');
                var list = inidata.PayTypeList;
                var arr = [];
				//判断是否有2级
				for(var i=0;i<list.length;i++){
					if(list[i].Pid == payid){
						arr.push(i);
					}
				}
				//如果没有
				if(arr.length === 0){
					if (payid != 1) {//非现金，自动填入未付款金额
	                    $('#AmountOfMoney').val($('#AmountOfMoney').attr('data-money'));
	                } else {
	                    $('#AmountOfMoney').val('').focus();
	                }
	                $('.GiveChange-text').html();
				}else{ //如果有
					//恢复默认值  和 样式
                	
					var str = '<ul style="padding:10px 20px 20px;">'; 
					for(var j=0;j<arr.length;j++){
						str += '<li data-key='+ list[arr[j]].Id +' data-num='+arr[j]+'>'+ list[arr[j]].Name +'</li>'
					}
					str += '<div style="clear:both"></div></ul>'
					layer.open({
						type: 1
	                    , title: '请选择'
	                    , area: ['372px']
	                    , content: str
	                    , skin: 'payment-method-layer'
	                    , success: function (layero, index) {
	                    	layero.find('li').on('click',function(){
	                    		var id = $(this).attr('data-key')
	                    		var num = $(this).attr('data-num')
	                    		layer.close(index);
	                    		$('#PayTypeList_view .active').attr('data-key',id).append($('<p class="hint"><span>'+list[num].Name+'</span></p>'));
	                    		
	                    	})
	                    }
	                    , isOutAnim: false 
	                    , end: function(){
	                    	$(this).attr('data-key',0);
                			$('#PayTypeList_view .active').attr('data-key',0).find('.hint').remove();
                			
                			
                			if (payid != 1) {//非现金，自动填入未付款金额
			                    $('#AmountOfMoney').val($('#AmountOfMoney').attr('data-money'));
			                } else {
			                    $('#AmountOfMoney').val('').focus();
			                }
			                $('.GiveChange-text').html();
	                    }
	                    , shade: [0.00001, '#fff']
	                    , shadeClose : true
	                    , maxmin: false
					})
				}
            });



            function layer_open(Options) {
                options = {
                    title: '',
                    str: ''
                }

                $.extend(options, Options);

                layer.open({
                    type: 1
                    , title: options.title
                    , content: options.str
                    , skin: 'layer-form-group'
                    , success: function (layero, index) {
                        options.success && options.success(layero, index);

                        $(layero).delegate('a.input-keyboard', 'click', function (event) {
                            var input = $(this).prev('.layui-input');
                            var type = input.attr('data-type');
                            var name = input.attr('name');
                            input.focus();
                            var mymode = layui.data('set');
                            if (mymode.mymode != 'touch') {//触摸
                                Keyboard(name);
                            }
                        });
                    }
                    , maxmin: false
                    , btn: ['确定', '取消']
                    , yes: function (index, layero) {
                        options.callback && options.callback(index, layero)
                    }
                    , btn2: function (index, layero) {
                        options.callback2 && options.callback2(index, layero)
                    }
                });
            }

            T_list_auto(true, false);

        }, error: function (msg) {
            console.log(msg.responseText);
        }
    });


    //键盘输入
    $('.AmountEnter').delegate('.keyboard-number li', 'click', function (event) {
        var inputdom = $('#AmountOfMoney');
        var value = $(this).attr('data-val');
        var inputVal = inputdom.val();

        
        if (value == '.') {
            if (inputVal.indexOf(".") >= 0) {
                return false;
            }
        }
        
        if(value == "-"){
			return false;
		}
        var newValue = inputVal + value;
        
        if (!isNaN(newValue)) {
	        var userreg = /^[-]{0,1}[0-9]+([.]{1}[0-9]{1,2})?$/;
	        
	        if (userreg.test(newValue)){
	        	if (newValue > 1000000) {
                    layer.msg('数量不能大于' + 1000000);
                    $(this).val('');
                    return false;
                } else {
                    inputdom.val(newValue);
                }
	        } else{
	        	var numindex = parseInt(newValue.indexOf("."), 10);
	            
	            if(numindex < 0)numindex = newValue.length;
	            
	            if (numindex == 0) {
	                inputdom.val("");
	                layer.msg("输入的数字不规范");
	                is = false;
	            }
	            
	            var head = newValue.substring(0, numindex);
	            var bottom = newValue.substring(numindex, numindex + 3);
	            var fianlNum = head + bottom;
	            inputdom.val(fianlNum);
	        }
        }else {
            $(this).val("");
            layer.msg("请输入数字");
            return false;
        }
        
        GiveChange();
    });

    //监听输入的值是否为数字
    $('#AmountOfMoney').bind('input propertychange', function (e) {

        var value = $(this).val();
        
        //验证数字
        if (ValidateNumber($(this),value)) {
       	   GiveChange();
        }


    });

    //删除数字
    $('.Keyboard-del-number').on("click", function () {
        var inputdom = $('#AmountOfMoney');
        var inputval = inputdom.val();
        var newval = inputval.substring(0, inputval.length - 1);
        inputdom.val(newval);
        GiveChange();
    })

    //付款、结账点击
    $('.Keyboard-btn a').on("click", function () {
        var value = $(this).attr('data-val');
        var inputdom = $('#AmountOfMoney');
        var inputval = inputdom.val();
        if (value == 'get') {//付款
            var activediv = $('#PayTypeList_view').find('.active');
            var payid = activediv.attr('data-key');
            var dPayid = activediv.attr('data-default-key');
            //根据payid不同进行不同的操作  T
            if (dPayid == 4) {
                //CustomerId 挂账
                var str = '<div class="layui-form" style="padding:10px 40px 0 0;">' + $("#gzDiv").html() + '</div>';
                layer_open({
                    title: '挂账',
                    str: str,
                    success: function (layero, index) {
                        form.render();
                        $(layero).children('.layui-layer-content').css('overflow', 'visible');
                    },
                    callback: function (index, layero) {
                        inidata.SourceId = $(layero).find('#CustomerId option:selected').val();
                        inidata.SourceName = $(layero).find('#CustomerId option:selected').text();
                        //挂账请求验证
                        var req = { id: inidata.SourceId, name: inidata.SourceName, money: inputval, payType: 4 };
                        GetValidate(req);
                        layer.close(index);
                    }
                })
            }
            else if (dPayid == 5) {
                //SourceName 房号
                var str = '<div id="dRoom" style="padding:10px 20px;"><div class="layui-form" style="text-align:right">' + $("#divMember").html() + '</div>';
                str +=  '<table id="dRoomTable" lay-filter="dRoomTable"></table></div>'
                str = str.replace('placeholder="客户名/电话/证件号/卡号"','placeholder="请输入房号"')
                str = str.replace('searchMember','searchRoom')
				layer.open({
			        type: 1
			        , title: '转客房'
			        , content: str
			        , skin: 'layer-form-group'
			        , area : ['650px','540px']
			        , success: function (layero, index) {
			        	var str1 = '<div class="layui-form" style="padding:20px 40px 0 0;"><div class="layui-form-item"><label class="layui-form-label">密码:</label><div class="layui-input-inline"><input class="layui-input" type="password" name="memberPwd" placeholder="请输入密码" data-lang="en" onfocus="ShowKeyboard(this.name)" /><a class="input-keyboard"><i class="iconfont">&#xe67a;</i></a></div></div></div>'
			        	var serachInput = layero.find('.layui-input');
			        	var searchBtn = layero.find('.searchRoom');
			        	var Data = [];
			        	var iNum;
						//键盘事件绑定
			            $(layero).delegate('a.input-keyboard', 'click', function (event) {
			                var input = $(this).prev('.layui-input');
			                var type = input.attr('data-type');
			                var name = input.attr('name');
			                input.focus();
			                var mymode = layui.data('set');
			                if (mymode.mymode != 'touch') {//触摸
			                    Keyboard(name);
			                }
			            });
			            
			            
			            //回车
			            serachInput.on('focus',function(){
			            	$(this).on('keydown',function(e){
			            		if(e.keyCode == 13){
			            			searchBtn.click();
			            		}
			            	})
			            }).on('blur',function(){
			            	$(this).off('keydown')
			            })
			            var a = 1;
			            //查询
			            searchBtn.on('click',function(){
			            	var val = serachInput.val();
			            	if(val == ""){
			            		layer.msg('请输入查询内容');
			            		return false;
			            	}
			            	//会员卡请求验证
						    $.ajax({
						        type: "post",
						        url: "/Res/CheckOut/SearchRoomInfoBy",
						        dataType: "json",
						        data: { text: val },
						        beforeSend: function (xhr) {
						            layindex = layer.open({type: 3});
						        },
						        complete: function (XMLHttpRequest, textStatus) {
						            layer.close(layindex);
						        },
						        success: function (data, textStatus) {
						            if (data.Successed) {//返回成功
						            	Data = data.Data;
						            	
					            		table.reload('dRoomTable', {data:''});
					            		table.reload('dRoomTable', {data:Data});
						            } else {
						                layer.msg(data.Message);
						            }
						
						        }
						    });
			            })
			            //表格渲染
			            table.render({
				            elem: '#dRoomTable'
				            , where: ''
				            , height: layero.height() - 150 //容器高度
				            , cols: [[ //标题栏
				                { field: 'CustomerId', title: '客户ID',align:'center'}
				                , { field: 'CustomerName', title: '姓名',align:'center'}
				                , { field: 'LimitAmount', title: '余额',align:'center'}
				                , { field: 'LastAmount', title: '限额',align:'center'}
                                , { field: 'PreAmount', title: '预授权金额', align: 'center' }
				            ]]
				            , even: true
				            , page: false
				            , data : Data
				            , skin : 'line'
				            , limit: 999 //每页默认显示的数量
				            , done: function (res, curr, count) {
				            	//选中
                                $('#dRoom .layui-table-body tr').on('dblclick', function () {
                                    $(this).addClass('layui-this').siblings().removeClass('layui-this');
                                    iNum = $(this).index();
                                    //如果该会员卡有密码
                                    
                                	layer.confirm('是否确认选择的是该房间？', {icon: 3, title:'提示'}, function(index){
                                		var req = { id: 0, name: serachInput.val(), money: inputval, payType: 5 };
                                        inidata.SourceId = Data[iNum].Id;
                                        inidata.SourceName = serachInput.val();
                                        GetValidate(req);
										layer.close(index);
									});
                                });
				            }
				        });
				        
			            serachInput.focus();
			            
			        }
			        , maxmin: false
			    });
            }else if (dPayid == 3) {
                var str = '<div id="dMember" style="padding:10px 20px;"><div class="layui-form" style="text-align:right">' + $("#divMember").html() + '</div>';
                str +=  '<table id="dMemberTable" lay-filter="dMemberTable"></table></div>'
				layer.open({
			        type: 1
			        , title: '会员卡搜索'
			        , content: str
			        , skin: 'layer-form-group'
			        , area : ['650px','540px']
			        , success: function (layero, index) {
			        	var str1 = '<div class="layui-form" style="padding:20px 40px 0 0;"><div class="layui-form-item"><label class="layui-form-label">密码:</label><div class="layui-input-inline"><input class="layui-input" type="password" name="memberPwd" placeholder="请输入密码" data-lang="en" onfocus="ShowKeyboard(this.name)" /><a class="input-keyboard"><i class="iconfont">&#xe67a;</i></a></div></div></div>'
			        	var serachInput = layero.find('.layui-input');
			        	var searchBtn = layero.find('.searchMember');
			        	var Data = [];
			        	var iNum;
						//键盘事件绑定
			            $(layero).delegate('a.input-keyboard', 'click', function (event) {
			                var input = $(this).prev('.layui-input');
			                var type = input.attr('data-type');
			                var name = input.attr('name');
			                input.focus();
			                var mymode = layui.data('set');
			                if (mymode.mymode != 'touch') {//触摸
			                    Keyboard(name);
			                }
			            });
			            
			            
			            //回车
			            serachInput.on('focus',function(){
			            	$(this).on('keydown',function(e){
			            		if(e.keyCode == 13){
			            			searchBtn.click();
			            		}
			            	})
			            }).on('blur',function(){
			            	$(this).off('keydown')
			            })
			            var a = 1;
			            //查询
			            searchBtn.on('click',function(){
			            	var val = serachInput.val();
			            	if(val == ""){
			            		layer.msg('请输入查询内容');
			            		return false;
			            	}
			            	//会员卡请求验证
						    $.ajax({
						        type: "post",
						        url: "/Res/CheckOut/SearchMemberInofBy",
						        dataType: "json",
						        data: { text: val },
						        beforeSend: function (xhr) {
						            layindex = layer.open({type: 3});
						        },
						        complete: function (XMLHttpRequest, textStatus) {
						            layer.close(layindex);
						        },
						        success: function (data, textStatus) {
						            if (data.Successed) {//返回成功
						            	Data = data.Data;
						            	
					            		table.reload('dMemberTable', {data:''});
					            		table.reload('dMemberTable', {data:Data});
						            } else {
						                layer.alert(data.Message);
						            }
						
						        }
						    });
			            })
			            //表格渲染
			            table.render({
				            elem: '#dMemberTable'
				            , where: ''
				            , height: layero.height() - 150 //容器高度
				            , cols: [[ //标题栏
				                { field: 'MemberName', title: '会员名',align:'center'}
				                , { field: 'MemberPhoneNo', title: '联系电话',align:'center'}
				                , { field: 'MemberCardNo', title: '会员卡号',align:'center'}
				                , { field: 'MemberIdentityNo', title: '证件号码',align:'center'}
				            ]]
				            , even: true
				            , page: false
				            , data : Data
				            , skin : 'line'
				            , limit: 999 //每页默认显示的数量
				            , done: function (res, curr, count) {
				            	//选中
                                $('#dMember .layui-table-body tr').on('dblclick', function () {
                                    $(this).addClass('layui-this').siblings().removeClass('layui-this');
                                    iNum = $(this).index();
                                    //如果该会员卡有密码
                                    if (Data[iNum].MemberPwd != null && Data[iNum].MemberPwd != "") {
                                        layer.open({
                                            type: 1
                                            , title: '请输入密码'
                                            , content: str1
                                            , skin: 'layer-form-group'
                                            , success: function (layero, index) {
                                                var pwdInput = layero.find('.layui-input');
                                                //小键盘
                                                $(layero).delegate('a.input-keyboard', 'click', function (event) {
                                                    var input = $(this).prev('.layui-input');
                                                    var type = input.attr('data-type');
                                                    var name = input.attr('name');
                                                    input.focus();
                                                    var mymode = layui.data('set');
                                                    if (mymode.mymode != 'touch') {//触摸
                                                        Keyboard(name);
                                                    }
                                                });
												
												//回车
                                                pwdInput.on('focus', function () {
                                                    $(this).on('keydown', function (e) {
                                                        if (e.keyCode == 13) {
                                                            layero.find('.layui-layer-btn0').click();
                                                        }
                                                    });
                                                }).on('blur', function () {
                                                    $(this).off('keydown')
                                                });
												
												//自动聚焦
                                                pwdInput.focus();
                                            }
                                            , btn: ['确认', '取消']
                                            , yes: function (index, layero) {
                                                var pwdInput = layero.find('.layui-input');
                                                var val = pwdInput.val();
                                                if(val == ""){
                                                	layer.msg('请输入密码');
                                                	return false;
                                                }
                                                pwdInput.blur();
                                                //提交数据
                                                var req = { id: Data[iNum].Id, name: val, money: inputval, payType: 3 };
                                                inidata.SourceId = Data[iNum].Id;
                                                inidata.SourceName = val;
                                                GetValidate(req);
                                            }
                                            , btn2: function (index, layero) {
                                            }
                                            , maxmin: false
                                        });
                                    }
                                    else {//无密码直接验证
                                    	layer.confirm('是否确认选择的是该会员卡？', {icon: 3, title:'提示'}, function(index){
                                    		var req = { id: Data[iNum].Id, name: "", money: inputval, payType: 3 };
	                                        inidata.SourceId = Data[iNum].Id;
	                                        inidata.SourceName = "";
	                                        GetValidate(req);
											layer.close(index);
										});
                                    }
                                });
				            }
				        });
				        
			            serachInput.focus();
			            
			        }
			        , maxmin: false
			    });
            }else {
                Payment();
            }

        } else if (value == 'success') {//结账
            CheckOut()
        }
    });

    //付款明细删除
    $('#tbodyForPayRecord').delegate('tr td a.del', 'click', function (event) {
        
        var no = $(this).parent().parent().attr('data-no');
        var thisArr = OrderPaidRecordList[no];
        if (thisArr.Id == 0) {
            OrderPaidRecordList.splice(no, 1);
        } else {
            var newPaidRecordList = [];
            for (var i = 0; i < OrderPaidRecordList.length; i++) {
                if (OrderPaidRecordList[i].Id != thisArr.Id) {
                    newPaidRecordList.push(OrderPaidRecordList[i]);
                }
            }
            for (var i = 0; i < newPaidRecordList.length; i++) {
                var item = newPaidRecordList[i];
                if (item.CyddJzType == 2 && item.Id > 0) {
                    newPaidRecordList[i].isDisabled = 0;
                }
            }
            OrderPaidRecordList = newPaidRecordList;
        }

        newOrderPaidRecordList();

        var tableIndex = $('#OrderTables_view .layui-this').index();
        UpdateData(tableIndex);
    });

});

//验证数字输入
function ValidateNumber(thisdom,value){
	
	var is = true;
    if (value == null || value == '') {
        is = false;
    }
    if(value == "-"){
    	return is;
    }
    
    if (!isNaN(value)) {
        var userreg = /^[-]{0,1}[0-9]+([.]{1}[0-9]{1,2})?$/;
        
        if (userreg.test(value) && value > 1000000) {

			layer.msg('数量不能大于' + 1000000);
            thisdom.val('');
            is = false;
       	}else{
        	var numindex = parseInt(value.indexOf("."), 10);
            
            if(numindex < 0)numindex = value.length;
            
            if (numindex == 0) {
                thisdom.val("");
                layer.msg("输入的数字不规范");
                is = false;
            }
            
            var head = value.substring(0, numindex);
            var bottom = value.substring(numindex, numindex + 3);
            var fianlNum = head + bottom;
            thisdom.val(fianlNum);
        }
    } else {
        thisdom.val("");
        layer.msg("请输入数字");
        is = false;
    }
    return is;
}


//更新付款明细
function newOrderPaidRecordList() {
    var getPaidTpl = PaidRecordList_tpml.innerHTML
        , view = document.getElementById('tbodyForPayRecord');
    laytpl(getPaidTpl).render(OrderPaidRecordList, function (html) {
        view.innerHTML = html;
    });
}

//获取

//转客房 and 挂账请求验证
function GetValidate(req) {

    $.ajax({
        type: "post",
        url: "/Res/CheckOut/VerifyByOutsideInfo",
        dataType: "json",
        data: req,
        beforeSend: function (xhr) {
            layindex = layer.open({type: 3});
        },
        complete: function (XMLHttpRequest, textStatus) {
            layer.close(layindex);
        },
        success: function (data, textStatus) {
            if (data.Successed) {//返回成功
                Payment();
            } else {
                layer.alert(data.Message);
            }

        }
    });
}

function layer_open(Options) {
    options = {
        title: '',
        str: ''
    }

    $.extend(options, Options);

    layer.open({
        type: 1
        , title: options.title
        , content: options.str
        , skin: 'layer-form-group'
        , success: function (layero, index) {
            options.success && options.success(layero, index);

            $(layero).delegate('a.input-keyboard', 'click', function (event) {
                var input = $(this).prev('.layui-input');
                var type = input.attr('data-type');
                var name = input.attr('name');
                input.focus();
                var mymode = layui.data('set');
                if (mymode.mymode != 'touch') {//触摸
                    Keyboard(name);
                }
            });
        }
        , maxmin: false
        , btn: ['确定', '取消']
        , yes: function (index, layero) {
            options.callback && options.callback(index, layero)
        }
        , btn2: function (index, layero) {
            options.callback2 && options.callback2(index, layero)
        }

    });
}

//清空数字
function number_empty() {
    $('#AmountOfMoney').val('');
    GiveChange();
}

//找零显示
function GiveChange() {
    //获取付款方式
    var key = $('#PayTypeList_view li.active').attr('data-key');
    var giveChangebox = $('.GiveChange-text');
    var topMoney = parseFloat($('#AmountOfMoney').attr('data-money'));
    if (key == '1') {//现金方式
        //未付款金额
        var money = parseFloat($('#AmountOfMoney').val());
        if (money > topMoney) {//显示找零
            var giveMoney = money - topMoney;
            giveChangebox.html('<label>找零:</label><span>' + giveMoney.toFixed(2) + '</span>');
        } else {
            giveChangebox.html('');
        }
    } else {
        //  	if(topMoney>0){//自动填写到输入框
        //  		$('#AmountOfMoney').val(topMoney);
        //  	}
        //      giveChangebox.html('');
    }
}



//渲染数据
function UpdateData(TabIndex) {

    //重组 订单合计数据
    var newArr = NewTotalArr();

    //渲染订单头部信息
    var getTpl = OrderAndTables_tpml.innerHTML
        , view = document.getElementById('OrderAndTables_view');
    laytpl(getTpl).render(inidata, function (html) {
        view.innerHTML = html;
    });

    //渲染订单台tab列表
    var getTpl = OrderTables_tpml.innerHTML
        , view = document.getElementById('OrderTables_view');
    laytpl(getTpl).render(inidata.OrderTableList, function (html) {
        view.innerHTML = html;
        
        //判断是否需要添加箭头
        var $OrderTables = $('#OrderTables_view');
        var $li = $OrderTables.children('li');
        var $li_s_w = 0;//所有li标签宽度
        $.each($li,function(){
        	$li_s_w += $(this).outerWidth() - 2
        })
        if($li_s_w > $OrderTables.width()){
        	$OrderTables.find('.layui-tab-bar').on('click',function(){
	        	if($OrderTables.hasClass('layui-tab-more')){
	        		$OrderTables.removeClass('layui-tab-more')
	        	}else{
	        		$OrderTables.addClass('layui-tab-more')
	        	}
	        })
        }else{
        	$OrderTables.find('.layui-tab-bar').hide();
        }
        
    });


    //渲染订单菜品信息
    var getTpl = OrderTablesAndProject_tpml.innerHTML
        , view = document.getElementById('OrderTablesAndProject_view');
    laytpl(getTpl).render(inidata.OrderTableList, function (html) {
        view.innerHTML = html;
    });

    //填入未付款金额到输入框

    $('#AmountOfMoney').attr('data-money', newArr.AmountOfMoney);
    GiveChange();

    //渲染订单合计信息
    var getTpl = OrderTotle_tpml.innerHTML
        , view = document.getElementById('OrderTotle_view');
    laytpl(getTpl).render(newArr, function (html) {
        view.innerHTML = html;
    });

    TotalOrderPrice(TabIndex);

    element.tabChange('OrderTab', 'TableTab' + TabIndex);

    //显示当前台
}

//重组 订单合计数据
function NewTotalArr() {

    var totalAmount = 0; //总消费金额

    inidata.ConAmount = 0;
    inidata.GiveAmount = 0;
    inidata.OriginalAmount = 0;//应收金额
    inidata.DiscountAmount = 0;//折扣金额
    inidata.ServiceAmount = 0;

    //循环桌台
    for (var i = 0; i < inidata.OrderTableList.length; i++) {
        var tableItem = inidata.OrderTableList[i];

        var tableAmount = 0;//当前台折后总金额

        //循环菜品
        for (var j = 0; j < tableItem.OrderDetailList.length; j++) {
            var detailItem = tableItem.OrderDetailList[j];

            totalAmount += detailItem.Amount;
            //折后金额
            var discountAmount = parseFloat((detailItem.Amount * detailItem.DiscountRate).toFixed(2));
            detailItem.DiscountedAmount = discountAmount;
            inidata.DiscountAmount += detailItem.Amount - discountAmount;
            tableAmount += discountAmount;
            inidata.OriginalAmount += discountAmount;
        }

        inidata.ServiceAmount += tableAmount * tableItem.ServerRate;//当前台服务费
    }

    inidata.OriginalAmount += inidata.ServiceAmount;

    //获取付款金
    var sumPayMoney = 0;
    for (var i = 0; i < OrderPaidRecordList.length; i++) {
        if (OrderPaidRecordList[i].CyddJzType == 2 || OrderPaidRecordList[i].CyddJzType == 3) {
            sumPayMoney += parseFloat(OrderPaidRecordList[i].PayAmount);
        }
    }

    var valueString = inidata.OriginalAmount.toFixed(2);
    inidata.OriginalAmount = parseFloat(inidata.OriginalAmount.toFixed(2));
    inidata.RealAmount = parseFloat(sumPayMoney.toFixed(2));//实收金额
    inidata.DiscountAmount = parseFloat(inidata.DiscountAmount.toFixed(2));
    inidata.ServiceAmount = parseFloat(inidata.ServiceAmount.toFixed(2));
    inidata.ClearAmount = parseFloat(inidata.ClearAmount.toFixed(2));
    
    var index = parseInt(valueString.toString().indexOf("."), 10);
    var fraction = valueString.substring(index + 1, index + 3);
    
    //只舍不入
//  inidata.Fraction = 0 - (parseInt(fraction) / 100);
    
    
    switch(inidata.CheckoutRound){
    	case 1: //四舍五入
    		if (fraction < 50) {//舍
		        inidata.Fraction = 0 - (parseInt(fraction) / 100);
		    } else {//入
		        inidata.Fraction = 1 - (parseInt(fraction) / 100);
		    }
    		break;
    	case 2: //只舍不入
    		inidata.Fraction = 0 - (parseInt(fraction) / 100);
    		break;
    	case 3: //只入不舍
    		inidata.Fraction = 1 - (parseInt(fraction) / 100);;
    		break;
    }
    //四舍五入 计算
//  if (fraction < 50) {//舍
//      inidata.Fraction = 0 - (parseInt(fraction) / 100);
//  } else {//入
//      inidata.Fraction = 1 - (parseInt(fraction) / 100);
//  }

    inidata.Fraction = parseFloat(inidata.Fraction.toFixed(2));

    inidata.OriginalAmount = parseFloat(inidata.OriginalAmount) + inidata.Fraction;
    
    var AmountOfMoney = inidata.OriginalAmount - inidata.ClearAmount - inidata.RealAmount;

    var Arr = {
        ConAmount: totalAmount.toFixed(2)//消费金额
        , OriginalAmount: inidata.OriginalAmount.toFixed(2)//应收金额
        , DiscountAmount: inidata.DiscountAmount.toFixed(2)//折扣金额
        , ServiceAmount: inidata.ServiceAmount.toFixed(2) //服务费
        , ClearAmount: inidata.ClearAmount.toFixed(2) //抹零金额
        , RealAmount: inidata.RealAmount.toFixed(2) //实收金额
        , Fraction: inidata.Fraction.toFixed(2) //四舍五入
        , AmountOfMoney: AmountOfMoney.toFixed(2)
    };

    return Arr;

}

//合计当前订单价格，数量
function TotalOrderPrice(TabIndex) {

    if (!TabIndex) {
        TabIndex = 0;
    }

    //当前订单数据
    var orderdata = inidata.OrderTableList[TabIndex];
    var ordertotalprice = 0, ordertotalnum = 0;
    //循环当前订单下的菜品
    for (var i = 0; i < orderdata.OrderDetailList.length; i++) {
        ordertotalprice += orderdata.OrderDetailList[i].Amount * orderdata.OrderDetailList[i].DiscountRate;
        ordertotalnum += orderdata.OrderDetailList[i].Num;
    }
    //当前台
    var thisTableBox = $('#OrderTablesAndProject_view .layui-tab-item').eq(TabIndex).find('tbody');
    //当前台 菜品数量
    var thisTableDetailNum = thisTableBox.find('tr').length;
    //选中的tr数量
    var thisTableDetailActiveNum = thisTableBox.find('tr.layui-this').length;
    if (thisTableDetailActiveNum == thisTableDetailNum) {//全选状态
        $('#allcheck').each(function () { this.checked = true; });
    } else {
        $('#allcheck').each(function () { this.checked = false; });
    }
    form.render('checkbox');
    $('#totalprice').text(ordertotalprice.toFixed(2));
    $('#serverRate').text(orderdata.ServerRate.toFixed(2));
}


//单品折
function SingleAgio() {
    var Checkedlist = $("#OrderTablesAndProject_view .layui-tab-item.layui-show tr[class=layui-this]");

    //return false;
    if (Checkedlist.length <= 0) {
        layer.alert("请选中至少一个菜品");
        return false;
    }
    var listOrderDetailID = new Array();
    //循环选中的每个菜品
    var messageArr = [];
    //var messageOld = message;
    $(Checkedlist).each(function () {
        var item = $(this);
        $(inidata.OrderTableList).each(function () { //每桌

            $(this.OrderDetailList).each(function () { //每菜品
                if (this.Id == item.data("id")) {
                    if (this.IsDiscount) {
                        listOrderDetailID.push(this.Id);
                    }
                    else {
                        messageArr.push(this.ProjectName);
                    }
                }

            });
        });

    });

    //筛选出可打折的菜品
    if (messageArr.length > 0) {//存在不允许打折的提示
        var message = '<tr style="text-align:center; font-weight:600;">';
        message += '<td colspan="3"><span class="color-red">以下菜品设置不可打折</span></td></tr><tr>';
        for (var i = 0; i < messageArr.length; i++) {
            message += '<td>' + messageArr[i] + '</td>';
            if ((i + 1) % 3 == 0) {
                message += '</tr><tr>';
            }
        }

        layer.open({
            type: 1,
            area: ['460px', '420px'], //宽高
            content: '<div style="padding:0 10px;"><table class="layui-table"><tbody>' + message + '</tbody></table></div>'
            , shade: 0
            , btn: ['确认']
            , yes: function (index, layero) {
                layer.close(index);

                OpenSingleAgio(listOrderDetailID);
            }
        });
    }
    else {
        OpenSingleAgio(listOrderDetailID);
    }
}

//执行授权后的单品折
function AuthSingleAgio(authObj, inputValue) {
    if (userDiscountRate > inputValue) {//权限不够啊
        layer.confirm('当前操作员折扣权限为' + userDiscountRate + ',是否继续授权？', {
            btn: ['继续授权', '取消'] //按钮
        }, function (index, layero) {
            //继续授权
            OpenAuth('SingleDiscountRate', inputValue);
            layer.close(index);
        }, function () {
            //$('#SingleAgio input.error').val('');
        });
        return false;
    } else {
        $('#SingleAgio input.Auth').val(inputValue).removeClass('Auth error');
        $('#userSingleAgio').text(userDiscountRate);
        //插入授权数据
        if (AuthObjArr) {
            var newAuthObjArr = [];
            for (var i = 0; i < AuthObjArr.length; i++) {
                if (AuthObjArr[i].OperateType != 1) {
                    newAuthObjArr.push(AuthObjArr[i]);
                }
            }
            AuthObjArr = newAuthObjArr;
        }
        AuthObjArr.push(authObj);
        inidata.DiscountRate = parseFloat(userDiscountRate);
    }
}

//打开单品折设置弹窗
function OpenSingleAgio(listOrderDetailID) {
    if (listOrderDetailID.length > 0) {

        var thisOrderDetailArr = ReturnData(listOrderDetailID);

        var tablelists = '';
        for (var i = 0; i < thisOrderDetailArr.length; i++) {
            var item = thisOrderDetailArr[i];
            tablelists += '<tr>'
                + '<td>' + item.ProjectName + '</td>'
                + '<td>盘</td>'
                + '<td>' + item.Num + '</td>'
                + '<td>' + item.Price + '</td>'
                + '<td>'
                + '<div class="layui-input-inline">'
                + '<input type="text" name="discountRate' + item.Id + '" placeholder="点击输入" title="折扣" value="' + item.DiscountRate + '" data-id="' + item.Id + '" data-type="number" onfocus="ShowKeyboard(this.name)" lay-verify="required" class="layui-input">'
                + '<a class="input-keyboard"><i class="iconfont">&#xe67a;</i></a>'
                + '</div>'
                + '</td>'
                + '</tr>';
        }
        var str = '<div class="layui-form" id="DiscountRate_form" style="padding:0 10px;" >'
            + '<table class="layui-table ">'
            + '<thead>'
            + '<tr>'
            + '<th>菜品名称</th>'
            + '<th>单位</th>'
            + '<th>数量</th>'
            + '<th>单价</th>'
            + '<th>折扣率</th>'
            + '</tr>'
            + '</thead>'
            + '<tbody id="SingleAgio">' + tablelists + '</tbody>'
            + '</table>'
            + '<p style="color:#f60">*折扣率取值必须是大于等于<span id="userSingleAgio"> ' + inidata.DiscountRate + '</span> 且小于等于1的数字</p>'
            + '</div>';
        layer.open({
            type: 1
            , title: "单品折设置"
            , area: ["600px", "600px"]
            , content: str
            , skin: 'layer-form-group'
            , maxmin: false
            , btn: ['确定', '取消']
            , yes: function (index, layero) {
                var errornum = $('#DiscountRate_form').find('.layui-input.error[data-type="number"]').length;
                if (errornum) {
                    layer.msg('折扣率取值必须是大于等于 ' + inidata.DiscountRate + ' 且小于等于1的数字');
                    return false;
                }

                //获得填写的折扣值
                var EditDiscountRateArr = $('#DiscountRate_form').find('.layui-input[data-type="number"]');
                var thisTableIndex = $("#OrderTables_view .layui-this").index();
                for (var i = 0; i < EditDiscountRateArr.length; i++) {
                    var editDiscountRateItemVal = parseFloat(EditDiscountRateArr[i].value);
                    var thisTableArr = inidata.OrderTableList[thisTableIndex];
                    //循环当前桌台的菜品
                    for (var j = 0; j < thisTableArr.OrderDetailList.length; j++) {
                        //每个菜品
                        var detailitem = thisTableArr.OrderDetailList[j];
                        if (detailitem.Id == thisOrderDetailArr[i].Id) {
                            detailitem.DiscountRate = editDiscountRateItemVal;
                        }
                    }

                }
                inidata.SchemeDiscountId = 0;
                inidata.DiscountMethod = 1;//单品折
                //渲染数据
                UpdateData(thisTableIndex);
                layer.close(index);

            }
            , btn2: function (index, layero) {
            }

        });
        //监听软键盘
        RegKeyboard();
        //监听输入的值是否正确
        $('#DiscountRate_form').find('.layui-input[data-type="number"]').blur(function () {
            var value = $(this).val();
            
            if(ValidateNumber($(this),value)){
            	if (value > 1) {
                    $(this).addClass('error');
                    layer.msg('请输入大于' + inidata.DiscountRate + '且小于1的数字');
                    $(this).val('');
                    return false;
                } else if (value < inidata.DiscountRate) {//授权
                    $(this).addClass('Auth');
                    $(this).val('');
                    OpenAuth('SingleDiscountRate', value);
                    return false;
                }else{
                	$(this).removeClass('error');
                }
            }


        })
    }
}


//全单折--弹出窗口
function OpenAllAgio(authObj, inputValue) {
    if (inputValue && inputValue > userDiscountRate) {//执行打折
        //执行打折
        allzk(inputValue);
        inidata.DiscountRate = parseFloat(userDiscountRate);
        //插入授权参数
        AuthObjArr.push(authObj);
        var thisTableIndex = $("#OrderTables_view .layui-this").index();
        UpdateData(thisTableIndex);
    } else {
        if (authObj) {
            //有授权并折扣还小于
            layer.confirm('当前操作员折扣权限为' + userDiscountRate + ',是否继续授权？', {
                btn: ['继续授权', '取消'] //按钮
            }, function (index, layero) {
                //继续授权
                OpenAuth('AllDiscountRate', inputValue);
                layer.close(index);
            }, function () {

            });
            return false;
        }

        var str = '<div class="layui-input-inline layui-form" style="margin: 15px;  width: 300px;">'
            + '<input type="text" name="AlldiscountRate" placeholder="点击输入" title="折扣" data-type="number" onfocus="ShowKeyboard(this.name)" lay-verify="required" class="layui-input">'
            + '<a  class="input-keyboard"><i class="iconfont">&#xe67a;</i></a>'
            + '</div>';

        layer.open({
            type: 1
            , title: "输入全单折扣，并确认"
            , content: str
            , skin: 'layer-form-group'
            , maxmin: false
            , btn: ['确定', '取消']
            , yes: function (index, layero) {
                var editDiscountRateItemVal = $('input[name="AlldiscountRate"]').val();
                
                if(editDiscountRateItemVal < inidata.DiscountRate){//授权
	       	    	OpenAuth('AllDiscountRate', editDiscountRateItemVal);
	       	    	return false;
	       	    }else if (editDiscountRateItemVal > 1) {
                    layer.msg('请输入大于' + inidata.DiscountRate + '且小于1的数字');
                    $(this).val('');
                    return false;
                }


                //执行打折
                allzk(editDiscountRateItemVal);
            }
            , btn2: function (index, layero) {
            }
            ,success:function(){
            	//监听软键盘
        		RegKeyboard();
        		$('input[name="AlldiscountRate"]').bind('input propertychange', function(e) {  
        			var inputval=$(this).val();
        			//验证
			        ValidateNumber($(this),inputval);
			    }).val('0.').focus();
        		
            }

        });
      
    }
}


//全单打折
function allzk(editDiscountRateItemVal) {
    var messageArr = [];
    //循环桌台
    for (var i = 0; i < inidata.OrderTableList.length; i++) {
        var orderTableItem = inidata.OrderTableList[i];
        //循环桌台的菜品
        for (var j = 0; j < orderTableItem.OrderDetailList.length; j++) {
            //每个菜品
            var detailItem = orderTableItem.OrderDetailList[j];

            detailItem.DiscountRate = 1;//先重置折扣率
            if (detailItem.IsDiscount > 0) {//允许打折
                detailItem.DiscountRate = editDiscountRateItemVal;
            } else {//没有折扣权限
                messageArr.push(detailItem.ProjectName);
            }
        }

    }
    var thisTableIndex = $("#OrderTables_view .layui-this").index();
    if (messageArr.length > 0) {//存在不允许打折的提示
        var message = '<tr style="text-align:center; font-weight:600;">';
        message += '<td colspan="3"><span class="color-red">以下菜品设置不可打折</span></td></tr><tr>';
        for (var i = 0; i < messageArr.length; i++) {
            message += '<td>' + messageArr[i] + '</td>';
            if ((i + 1) % 3 == 0) {
                message += '</tr><tr>';
            }
        }

        layer.open({
            type: 1,
            area: ['460px', '420px'], //宽高
            content: '<div style="padding:0 10px;"><table class="layui-table"><tbody>' + message + '</tbody></table></div>'
            , shade: 0
            , btn: ['确认']
            , yes: function (index, layero) {
                //渲染数据
                UpdateData(thisTableIndex);
                layer.closeAll();
            }
        });
    } else {
        UpdateData(thisTableIndex);
        layer.closeAll();
    }

    inidata.SchemeDiscountId = 0;
    inidata.DiscountMethod = 2;//全单折
    DelClear();
}


//清除折扣
function DelAgio(){
	 //循环桌台
    for (var i = 0; i < inidata.OrderTableList.length; i++) {
        var orderTableItem = inidata.OrderTableList[i];
        //循环桌台的菜品
        for (var j = 0; j < orderTableItem.OrderDetailList.length; j++) {
            //每个菜品
            var detailItem = orderTableItem.OrderDetailList[j];

            detailItem.DiscountRate = 1;//先重置折扣率
        }

    }
    var thisTableIndex = $("#OrderTables_view .layui-this").index();
    
    inidata.SchemeDiscountId = 0;
    inidata.DiscountMethod = 2;//全单折
    DelClear();
    UpdateData(thisTableIndex);
}

//强制折--弹出窗口
function ForceAgio(authObj, inputValue) {
    if (inputValue && inputValue > userDiscountRate) {//执行打折
        //执行打折
        getForceAgio(inputValue);
        inidata.DiscountRate = parseFloat(userDiscountRate);
        //插入授权参数
        AuthObjArr.push(authObj);
        var thisTableIndex = $("#OrderTables_view .layui-this").index();
        UpdateData(thisTableIndex);
    } else {
        if (authObj) {
            //有授权并折扣还小于
            layer.confirm('当前操作员折扣权限为' + userDiscountRate + ',是否继续授权？', {
                btn: ['继续授权', '取消'] //按钮
            }, function () {
                //继续授权
                OpenAuth('ForceDiscountRate', inputValue);
            }, function () {

            });
            return false;
        }
        var str = '<div class="layui-input-inline layui-form" style="margin: 15px;  width: 300px;">'
            + '<input type="text" name="AlldiscountRate" placeholder="点击输入" title="折扣" data-type="number" onfocus="ShowKeyboard(this.name)" lay-verify="required" class="layui-input">'
            + '<a  class="input-keyboard"><i class="iconfont">&#xe67a;</i></a>'
            + '</div>';

        layer.open({
            type: 1
            , title: "输入折扣，并确认"
            , content: str
            , skin: 'layer-form-group'
            , maxmin: false
            , btn: ['确定', '取消']
            , yes: function (index, layero) {
                var editDiscountRateItemVal = $('input[name="AlldiscountRate"]').val();
                
                if(editDiscountRateItemVal < inidata.DiscountRate){
                	//授权
                    OpenAuth('ForceDiscountRate', editDiscountRateItemVal);
                    return false;
                }else if (editDiscountRateItemVal > 1) {
                    layer.msg('请输入大于' + inidata.DiscountRate + '且小于1的数字');
                    $(this).val('');
                    return false;
                }

                //执行打折
                getForceAgio(editDiscountRateItemVal);


            }
            , btn2: function (index, layero) {
            }
            ,success:function(){
            	//监听软键盘
        		RegKeyboard();
        		$('input[name="AlldiscountRate"]').bind('input propertychange', function(e) {  
        			var inputval=$(this).val();
        			//验证
			        ValidateNumber($(this),inputval);
			    }).val('0.').focus();
        		
            }

        });
        //监听软键盘
        RegKeyboard();
    }
}


//强制折扣打折
function getForceAgio(editDiscountRateItemVal) {
    var messageArr = [];
    //循环桌台
    var tro = 0;
    for (var i = 0; i < inidata.OrderTableList.length; i++) {
        var orderTableItem = inidata.OrderTableList[i];
        //循环桌台的菜品
        for (var j = 0; j < orderTableItem.OrderDetailList.length; j++) {
            //每个菜品
            var detailItem = orderTableItem.OrderDetailList[j];
            detailItem.DiscountRate = 1;//先重置折扣率

            if (detailItem.IsDiscount > 0 || (detailItem.IsDiscount == 0 && detailItem.IsForceDiscount > 0)) {//允许强制打折
                detailItem.DiscountRate = editDiscountRateItemVal;
            } else {//没有折扣权限
                messageArr.push(detailItem.ProjectName);
            }
        }

    }
    var thisTableIndex = $("#OrderTables_view .layui-this").index();
    if (messageArr.length > 0) {//存在不允许打折的提示
        var message = '<tr style="text-align:center; font-weight:600;">';
        message += '<td colspan="3"><span class="color-red">以下菜品设置不可强制打折</span></td></tr><tr>';
        for (var i = 0; i < messageArr.length; i++) {
            message += '<td>' + messageArr[i] + '</td>';
            if ((i + 1) % 3 == 0) {
                message += '</tr><tr>';
            }
        }

        layer.open({
            type: 1,
            area: ['460px', '420px'], //宽高
            content: '<div style="padding:0 10px;"><table class="layui-table"><tbody>' + message + '</tbody></table></div>'
            , shade: 0
            , btn: ['确认']
            , yes: function (index, layero) {
                //渲染数据
                UpdateData(thisTableIndex);
                layer.closeAll();
            }
        });
    } else {
        UpdateData(thisTableIndex);
        layer.closeAll();
    }

    inidata.SchemeDiscountId = 0;
    inidata.DiscountMethod = 4;//强制折
    DelClear();
}


//抹零--
function OpenClear(authObj) {

    //获取未付金额
    var newArr = NewTotalArr();
    var wfmoney = newArr.AmountOfMoney;
    if (wfmoney <= 0) {
        layer.msg('未付金额为空，无需抹零!');
        return false;
    }
    if (userClearMoney >= wfmoney) {//授权金额大于抹零金额
        inidata.ClearAmount = parseFloat(wfmoney);
        var thisTableIndex = $("#OrderTables_view .layui-this").index();
        UpdateData(thisTableIndex);
        if (!authObj) {//当前用户抹零
            var authObj = Object();
            authObj.AuthUserId = inidata.OperateUser;
            authObj.OperateType = 2;
        } else {//授权用户抹零
            var newAuthObjArr = [];
            for (var i = 0; i < AuthObjArr.length; i++) {
                if (AuthObjArr[i].OperateType != 2) {
                    newAuthObjArr.push(AuthObjArr[i]);
                }
            }
            AuthObjArr = newAuthObjArr;
        }
        AuthObjArr.push(authObj);


    } else {//弹出授权
        if (authObj) {
            layer.confirm('授权人抹零权限为' + userClearMoney + ',是否继续授权？', {
                btn: ['继续授权', '直接抹零'] //按钮
            }, function (index, layero) {
                OpenAuth('userClearMoney');
                layer.close(index);
            }, function (index, layero) {
                inidata.ClearAmount = parseFloat(userClearMoney);
                var thisTableIndex = $("#OrderTables_view .layui-this").index();
                UpdateData(thisTableIndex);
                //插入授权参数
                AuthObjArr.push(authObj);
                layer.close(index);
            });
        } else {
            OpenAuth('userClearMoney');
        }
    }


}

//清除抹零
function DelClear() {
    inidata.ClearAmount = 0;
    if (AuthObjArr) {
        var newAuthObjArr = [];
        for (var i = 0; i < AuthObjArr.length; i++) {
            if (AuthObjArr[i].OperateType != 2) {
                newAuthObjArr.push(AuthObjArr[i]);
            }
        }
    }
    userClearMoney = inidata.AuthClearValue;//还原当前用户最大抹零值
    AuthObjArr = newAuthObjArr;
    var thisTableIndex = $("#OrderTables_view .layui-this").index();
    UpdateData(thisTableIndex);
}

//付款
function Payment() {
    var activediv = $('#PayTypeList_view').find('.active');
    var payType = activediv.attr('data-key');//选中方式type
    var dPayType = activediv.attr('data-default-key');//默认方式 type
    var payAmount = parseFloat($('#AmountOfMoney').val());
    var topmoney = parseFloat($('#AmountOfMoney').attr('data-money'));
    
    var payTypeName;
    if (!payAmount || payAmount <= 0) {
        layer.msg('请输入付款金额!');
        return false;
    }

    var remark = "";
    //现金付款大于未付金额的取未付金额
	if (dPayType == '1') {
        if (payAmount > topmoney) {
            payAmount = topmoney;
        }
        remark = "现金支付";
    } else if (dPayType == '2') {
        remark = "银行卡支付";
    } else if (dPayType == '6') {
        remark = "代金券支付";
    } else if (dPayType == '4') {
        remark = "挂账客户：" + inidata.SourceName;
    } else if (dPayType == '5') {
        remark = "转客房房号：" + inidata.SourceName;
    } else if (dPayType == '8') {
        remark = "微信支付";
    } else if (dPayType == '9') {
        remark = "支付宝支付";
    }
    
    //判断是否是二级分类支付
    payType != 0 ? payTypeName = activediv.find('.hint').text() : payTypeName = activediv.text();

    //插入付款记录
    var Arr = {
        CyddJzStatus: 2
        , CyddJzType: 2
        , CyddPayType: payType == 0 ? dPayType : payType
        , Id: 0
        , PayAmount: payAmount
        , Remark: remark
        , SourceId: inidata.SourceId
        , SourceName: inidata.SourceName
        , PayTypeName: payTypeName
        , JzTypeName: '消费'
        , CreateUserName: inidata.OperateUserName
        , CreateDate: getTime()
    };
    OrderPaidRecordList.push(Arr);
    newOrderPaidRecordList();

    var thisTableIndex = $("#OrderTables_view .layui-this").index();
    $('#AmountOfMoney').val('');
    inidata.SourceId = 0;
    inidata.SourceName = "";
    UpdateData(thisTableIndex);

    layer.closeAll();    
}


//转客房 and 挂账请求验证
function GetValidate(req) {

    $.ajax({
        type: "post",
        url: "/Res/CheckOut/VerifyByOutsideInfo",
        dataType: "json",
        data: req,
        beforeSend: function (xhr) {
            layindex = layer.open({type: 3});
        },
        complete: function (XMLHttpRequest, textStatus) {
            layer.close(layindex);
        },
        success: function (data, textStatus) {
            if (data.Successed) {//返回成功
                Payment();
            } else {
                layer.alert(data.Message);
            }

        }
    });
}

//获取结账参数
function GetCheckOutParams() {

    return req;
}

//生成四舍五入、抹零、服务费支付记录
function BuildOtherPayRecords(payRecords) {
    var newOtherPaidRecordList = [];
    for (var i = 0; i < payRecords.length; i++) {
        if (payRecords[i].CyddJzType != 4 && payRecords[i].CyddJzType != 5 && payRecords[i].CyddJzType != 6) {
            newOtherPaidRecordList.push(payRecords[i]);
        }
    }

    //插入结账四舍五入记录
    var residueObj = {
        CyddJzStatus: 2
        , CyddJzType: 4  //四舍五入类型
        , CyddPayType: 0
        , Id: 0
        , PayAmount: inidata.Fraction
        , Remark: "四舍五入记录"
        , SourceId: 0
        , SourceName: ""
        , PayTypeName: ""
        , JzTypeName: "四舍五入"
        , CreateUserName: inidata.OperateUserName
        , CreateDate: getTime()
    };
    newOtherPaidRecordList.push(residueObj);

    //插入结账抹零记录
    var clearObj = {
        CyddJzStatus: 2
        , CyddJzType: 5  //抹零类型
        , CyddPayType: 0
        , Id: 0
        , PayAmount: inidata.ClearAmount
        , Remark: "抹零记录"
        , SourceId: 0
        , SourceName: ""
        , PayTypeName: ""
        , JzTypeName: "抹零"
        , CreateUserName: inidata.OperateUserName
        , CreateDate: getTime()
    };
    newOtherPaidRecordList.push(clearObj);

    //插入结账服务费记录
    var servcieObj = {
        CyddJzStatus: 2
        , CyddJzType: 6  //服务费类型
        , CyddPayType: 0
        , Id: 0
        , PayAmount: inidata.ServiceAmount
        , Remark: "服务费记录"
        , SourceId: 0
        , SourceName: ""
        , PayTypeName: ""
        , JzTypeName: "服务费"
        , CreateUserName: inidata.OperateUserName
        , CreateDate: getTime()
    };
    newOtherPaidRecordList.push(servcieObj);

    //插入结账折扣记录
    var discountObj = {
        CyddJzStatus: 2
        , CyddJzType: 7  //折扣类型
        , CyddPayType: 0
        , Id: 0
        , PayAmount: inidata.DiscountAmount
        , Remark: "折扣金额记录"
        , SourceId: 0
        , SourceName: ""
        , PayTypeName: ""
        , JzTypeName: "折扣"
        , CreateUserName: inidata.OperateUserName
        , CreateDate: getTime()
    };

    payRecords = [];
    newOtherPaidRecordList.push(discountObj);
    payRecords = newOtherPaidRecordList;

    return payRecords;
}

//结账
function CheckOut() {

    //结账金额
    var money = 0;
    var hasNewRecords = false;
    //付款记录集合
    for (var i = 0; i < OrderPaidRecordList.length; i++) {
        if (OrderPaidRecordList[i].CyddJzType == 2 || OrderPaidRecordList[i].CyddJzType == 3) {
            money += OrderPaidRecordList[i].PayAmount;
        }

        if (OrderPaidRecordList[i].Id <= 0)
            hasNewRecords = true;
    };

    if (isNaN(money)) {
        layer.alert("结账金额输入不正确，请重新输入！");
        return;
    }

    if (money != inidata.RealAmount) {
        layer.alert("本次结账实收金额" + money + "不正确！");
        return;
    }
    //如果有赠送，抹零等，在此判断
    if (inidata.OriginalAmount < (inidata.RealAmount + inidata.ClearAmount)) {
        layer.alert("结账金额超出待结金额！");
        return;
    }

    if (inidata.OriginalAmount > (inidata.RealAmount + inidata.ClearAmount)) {
        layer.alert("尚未付清待结账金额，不能结账！");
        return;
    }

    if (hasNewRecords != true)
        return;

    var req = new Object();
    req.OrderId = inidata.Id; //订单id
    req.TableIds = inidata.TableIds; //当前订单下待结账台号
    req.ListOrderPayRecordDTO = OrderPaidRecordList;

    req.Money = money;//结账金额

    req.ConAmount = inidata.ConAmount;
    req.OriginalAmount = inidata.OriginalAmount;
    req.ClearAmount = inidata.ClearAmount;
    req.DiscountAmount = inidata.DiscountAmount;
    req.ServiceAmount = inidata.ServiceAmount;
    req.GiveAmount = inidata.GiveAmount;

    req.SchemeDiscountId = inidata.SchemeDiscountId;
    req.DiscountMethod = inidata.DiscountMethod;
    req.Fraction = inidata.Fraction;
    req.AuthUserList = AuthObjArr;

    var ListOrderDetailDTO = [];
    $(inidata.OrderTableList).each(function () {
        $(this.OrderDetailList).each(function () {
            ListOrderDetailDTO.push(this);
            //为了节省传输数据量，将用不到的数据去掉
            this.Extend = null;
            this.ExtendRequire = null;
            this.ExtendExtra = null;

        });
    });

    req.ListOrderDetailDTO = ListOrderDetailDTO;
    req.ListOrderPayRecordDTO = BuildOtherPayRecords(req.ListOrderPayRecordDTO);

    if (req == undefined || req == null)
        return;

    req.IsReCheckout = true;//反结操作
    //var chat = $.connection.systemHub;
    //chat.hubName = 'systemHub';
    //chat.connection.start();
	
    $.ajax({
        type: "POST",
        url: "/Res/Checkout/ReCheckout",
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
            if (data.Result == true) {
                ////通知刷新
                //$.connection.hub.start().done(function () {//通知刷新工作台界面
                //    chat.server.notifyResServiceRefersh(true);
                //});

                layer.alert("反结成功！", function () {
                    parent.RefreshOrdeInfo_index(inidata.Id);
                });
            }
            else {
                layer.alert(data.Info);
            }
        }
    });
}

//结账小票打单
function CheckOutBill(dom, isLocked) {
    if ($(dom).hasClass('Disable')) {
        return false;
    }

    var req = GetCheckOutParams();
    req.isLocked = isLocked;
    $.ajax({
        type: "POST",
        url: "/Res/Checkout/CheckOutBill",
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
            if (data.Result == true) {
                layer.alert("出票成功！", function () {
                    parent.layer.closeAll();
                });
            }
            else {
                layer.alert(data.Info);
            }
        }
    })
}


//删除全部付款记录
function DelAllPay() {
    var newPaidRecordList = [];
    for (var i = 0; i < OrderPaidRecordList.length; i++) {
        if (OrderPaidRecordList[i].Id > 0) {
            if (OrderPaidRecordList[i].Id > 0 && OrderPaidRecordList[i].CyddJzType == 2)
                OrderPaidRecordList[i].isDisabled = 0;
            newPaidRecordList.push(OrderPaidRecordList[i]);
        }
    }
    OrderPaidRecordList = newPaidRecordList;
    newOrderPaidRecordList();

    var thisTableIndex = $("#OrderTables_view .layui-this").index();
    UpdateData(thisTableIndex);
}

//反结付款记录
function ReturnPay(rId) {
    if (rId <= 0)
        return false;

    var hasRecord = false;
    //结账金额
    var money = 0;
    var djArr;
    for (var i = 0; i < OrderPaidRecordList.length; i++) {
        if (rId == OrderPaidRecordList[i].Id) {
            djArr = OrderPaidRecordList[i];
            money = djArr.PayAmount;
        }
        if (-rId == OrderPaidRecordList[i].Id)
            hasRecord = true;
    }
    if (hasRecord)
        return;
    
    var Arr = {
        CyddJzStatus: 4
        , CyddJzType: 2
        , CyddPayType: djArr.CyddPayType
        , Id: -rId
        , PayAmount: -money
        , Remark: '反结支付记录'
        , SourceId: djArr.SourceId
        , SourceName: djArr.SourceName
        , PayTypeName: djArr.PayTypeName
        , JzTypeName: djArr.JzTypeName
        , CreateUserName: inidata.OperateUserName
        , CreateDate: getTime()
    };

    OrderPaidRecordList.push(Arr);
    newOrderPaidRecordList();

    var thisTableIndex = $("#OrderTables_view .layui-this").index();
    $('#AmountOfMoney').val('');
    UpdateData(thisTableIndex);
    
}

//根据选中可操作的id数组返回 相关数据
function ReturnData(listOrderDetailID) {
    //取得修改菜品的数组
    var thisTableIndex = $("#OrderTables_view .layui-this").index();

    var returndataArr = [];
    for (var i = 0; i < listOrderDetailID.length; i++) {
        //当前桌台数组
        var thisTableArr = inidata.OrderTableList[thisTableIndex];
        //循环当前桌台的菜品
        for (var j = 0; j < thisTableArr.OrderDetailList.length; j++) {
            //每个菜品
            var Detailitem = thisTableArr.OrderDetailList[j];
            if (listOrderDetailID[i] == Detailitem.Id) {
                returndataArr.push(Detailitem);
            }
        }
    }

    return returndataArr;
}

//弹出授权窗口
function OpenAuth(EventName, inputValue) {
	
    var str = '<div class="layui-form" style="padding:10px 20px 0 0;">' + $("#AuthDiv").html() + '</div>';

    layer.open({
        type: 1
        , title: "请选择操作用户授权"
        , content: str
        , skin: 'layer-form-group'
        , maxmin: false
        , btn: ['确定', '取消']
        , success: function (layero, index) {
            form.render();
            $(layero).children('.layui-layer-content').css('overflow', 'visible');

            $(layero).delegate('a.input-keyboard', 'click', function (event) {
                var input = $(this).prev('.layui-input');
                var type = input.attr('data-type');
                var name = input.attr('name');
                input.focus();
                var mymode = layui.data('set');
                if (mymode.mymode != 'touch') {//触摸
                    Keyboard(name);
                }
            });
        }
        , yes: function (index, layero) {
            var pwd = $('.layui-layer-content #AuthPwd').val();
            var userId = $('.layui-layer-content #AuthUserId option:selected').val();

            if (userId <= 0) {
                layer.alert("请选择授权用户！");
                return;
            }

            var req = new Object();
            req.UserId = userId;
            req.UserPwd = pwd;
            $.ajax({
                type: "POST",
                url: "/Res/Checkout/GetAuthUser",
                data: JSON.stringify(req),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function (xhr) {
		            layindex = layer.open({type: 3});
		        },
		        complete: function (XMLHttpRequest, textStatus) {
		            layer.close(layindex);
		        },
                success: function (data, textStatus) {
                    var obj = data.Data;
                    if (obj != null && obj.UserId > 0) {

                        var authObj = Object();
                        authObj.AuthUserId = obj.UserId;
                        if (EventName == 'userClearMoney') {//抹零授权
                            userClearMoney = obj.MaxClearValue;//更新抹零授权金额
                            //插入授权记录
                            authObj.OperateType = 2;
                            //执行抹零
                            OpenClear(authObj);
                            layer.close(index);
                        } else if (EventName == 'AllDiscountRate') {//全单折扣授权
                            userDiscountRate = obj.MinDiscountValue;
                            authObj.OperateType = 1;
                            //执行全单折
                            OpenAllAgio(authObj, inputValue);
                            layer.close(index);

                        } else if (EventName == 'ForceDiscountRate') {//强制折扣授权
                            userDiscountRate = obj.MinDiscountValue;
                            authObj.OperateType = 1;
                            //执行强制折
                            ForceAgio(authObj, inputValue);
                            layer.close(index);

                        } else if (EventName == 'SingleDiscountRate') {//单品折扣授权
                            userDiscountRate = obj.MinDiscountValue;
                            authObj.OperateType = 1;
                            //执行强制折
                            AuthSingleAgio(authObj, inputValue);
                            layer.close(index);

                        } else {
                            authObj.OperateType = 1;
                            AuthObjArr.push(authObj);
                            inidata.DiscountRate = obj.Discount;
                            inidata.DiscountMethod = 0;
                            layer.alert("授权成功！", function () {
                                for (var i = 0; i < inidata.OrderTableList.length; i++) {
                                    var orderTableItem = inidata.OrderTableList[i];

                                    for (var j = 0; j < orderTableItem.OrderDetailList.length; j++) {
                                        //每个菜品
                                        orderTableItem.OrderDetailList[j].DiscountRate = 1;//重置折扣率                                    
                                    }
                                }

                                UpdateData(0);
                                layer.close(index);
                            });
                        }
                    }
                    else {
                        layer.alert(data.Message);
                    }
                }
            });
        }
        , btn2: function (index, layero) {
        }
    });
}

//方案折
function OpenPlanAgio() {
    var checkArr = '';
    $.ajax({
        type: "POST",
        url: "/Res/Discount/GetSchemeDiscountList",
        dataType: "json",
        async: true,
        beforeSend: function (xhr) {
            layindex = layer.open({type: 3});
        },
        complete: function (XMLHttpRequest, textStatus) {
            layer.close(layindex);
        },
        success: function (data, textStatus) {
            var str = '';
            var itemstr = '';
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var start = item.StartDate.substr(0, 10);
                var end = item.EndDate.substr(0, 10);
                var check = '';
                //默认
                if (i == 0) {
                    checkArr = item;
                    check = 'class="checked"';
                    for (var j = 0; j < item.DetailList.length; j++) {
                        var itemj = item.DetailList[j];
                        itemstr += '<tr>'
                            + '<td>' + itemj.CategoryName + '</td>'
                            + '<td>' + itemj.DiscountRate + '</td>'
                            + '</tr>';
                    }
                }
                str += '<li data-id="' + item.Id + '" ' + check + ' style="cursor: pointer;">'
                    + '<div class="MealTable-head flex">'
                    + '<span class="item MealTable-number flex-item">' + item.Id + '</span>'
                    + '</div>'
                    + '<div class="MealTable-title" style="line-height: 30px;height: 30px;font-size:16px;"> ' + item.Name + ' </div>'
                    + '<div style="font-size:12px;padding-left:10px;">开始: ' + start + '</div>'
                    + '<div style="font-size:12px;padding-left:10px;">结束: ' + end + '</div>'
                    + '</li>';


            }
            if (!str) {
                layer.msg('暂无折扣方案!');
                return false;
            }
            var strhtml = '<div class="layui-form" style="background: #e6e6e6;height:483px;">'
                + '<div class="StartDesk-form flex-item" style="padding-left: 10px;">'
                + '<blockquote class="label-title">选择打折方案</blockquote>'
                + '<div class="MealTable-lists "><ul id="PlanAgio_list">' + str + '</ul></div>'
                + '</div>'
                + '</div>'
                + '<div class="Panel-side" style="top:0;position: absolute;">'
                + '<div class="layui-tab layui-tab-brief">'
                + '<div class="ClassTab-head"><blockquote class="label-title">折扣明细</blockquote></div>'
                + '<div style="padding:0 10px;">'
                + '<table class="layui-table">'
                + '<thead>'
                + '<tr>'
                + '<th>分类名称</th>'
                + '<th>折扣率</th>'
                + '</tr>'
                + '</thead>'
                + '<tbody id="PlanAgio_data">' + itemstr + '</tbody>'
                + '</table>'
                + '</div>'
                + '</div>'
                + '</div>';

            layer.open({
                type: 1,
                anim: -1,
                title: '方案折',
                shadeClose: true,
                skin: 'layer-header layer-form-group',
                shade: 0.8,
                area: ['1000px', '600px'],
                content: strhtml
                , btn: ['确定', '取消']
                , yes: function (index, layero) {
                    //提交方案折
                    $(inidata.OrderTableList).each(function () { //每桌
                        //判断是否可打折的区域
                        if (this.R_Area_Id == checkArr.AreaId || checkArr.AreaId == 0) {

                            $(this.OrderDetailList).each(function () { //每菜品
								this.DiscountRate = 1;
                                for (var i = 0; i < checkArr.DetailList.length; i++) {//每个可打折分类
                                	
                                    if (this.CategoryId == checkArr.DetailList[i].CategoryId) {
                                        var zkl = checkArr.DetailList[i].DiscountRate;
                                        this.DiscountRate = zkl;
                                    }else{
                                    	
                                    }
                                }

                            });
                        }else{
                        	$(this.OrderDetailList).each(function () { //还原不可打折的区域的每个菜品折扣率
								this.DiscountRate = 1;
                            });
                        }

                    });

                    var thisTableIndex = $("#OrderTables_view .layui-this").index();
                    UpdateData(thisTableIndex);
                    inidata.SchemeDiscountId = checkArr.Id;
                    inidata.DiscountMethod = 3;//方案折
                    DelClear();
                    layer.close(index);
                    return false;

                }
                , btn2: function (index, layero) {

                }, success: function (index, layero) {
                    $('#PlanAgio_list li').click(function () {
                        $(this).addClass('checked').siblings('li').removeClass('checked');
                        var id = $(this).attr('data-id');
                        var itemindex = $(this).index();
                        var itemarr = data[itemindex];
                        checkArr = itemarr;
                        var itemstr = '';
                        for (var j = 0; j < itemarr.DetailList.length; j++) {
                            var itemj = itemarr.DetailList[j];
                            itemstr += '<tr>'
                                + '<td>' + itemj.CategoryName + '</td>'
                                + '<td>' + itemj.DiscountRate + '</td>'
                                + '</tr>';
                        }
                        $('#PlanAgio_data').html(itemstr);
                    })
                }
            });
        }
    })


}
