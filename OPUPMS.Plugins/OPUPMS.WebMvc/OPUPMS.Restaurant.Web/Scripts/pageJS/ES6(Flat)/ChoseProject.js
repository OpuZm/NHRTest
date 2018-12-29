new Vue({
  	el: '#app',
 	data: function() {
 		var isNumberValidator = (rule, value, callback) => {
	        if(!value) {
				return callback(new Error('就餐人数不能为空'));
			}
	        console.log(value)
			if(isNaN(value) || !Number.isInteger(parseFloat(value))) {
				callback(new Error('请输入数字'));
			} else {
				callback();
			}
		};
    	return { 
    		fullscreenLoading:false,
    		restaurantId:0,										//餐厅ID
    		MarketList:{},										//餐厅分市选择的信息
    		messageIndex:-1,									//提示	下标
    		inidata:{
    			CategoryList:[],  								//左侧栏	导航数据
    		},
    		loginDialogShow:false,								//用户登录弹窗是否开启
    		chosenRestaurantOrderShow:false,					//选择桌台弹窗是否开启
    		chosenRestaurantOrderStep:1,						//当前桌台选择步骤	1：选择桌台	2：开台信息填写
    		openTableForm:{										//开台信息
    			PersonNum:'',									//就餐人数
    			R_Market_Id:0,									//分市ID
    			CyddOrderSource:0,								//客源类型
    			ContactPerson:'',								//联系人
    			CustomerId:0,									//客户
    			ContactTel:'',									//联系电话
    			OrderType:0,									//订单类型
    			Remark:''										//留言
    		},
    		rules2: {											//验证规则
	          	PersonNum: [									//验证数字
	        		{ validator: isNumberValidator, trigger: 'blur' }
	          	]
	        },
    		user:{												//登录用户信息
    			Account:'',										//账号
    			PassWord:''										//密码
    		},
    		userInfo:{},										//后端返回的用户信息
    		searchInfoEmpty:false,								//搜索信息清空（判断用）
    		searchInfo:'',										//搜索条件
			isReviseLayerShow:false,							//是否显示  点选 || 修改  菜品信息弹窗
    		reviselayerDataIndex:-1,							//当前打开的点选 || 修改数据的下标
    		reviselayerData:{									//点选 || 修改  菜品信息弹窗数据
    			"index":-1,										//购物车  当前修改的菜品	下标
    			"CyddMxName":'',								//名称
    			"Unit":'',										//单位
    			"Price":0,										//金额
    			"Num":0,										//数量
    			"typeIndex":-1,									//当前规格	下标		-1 表示未修改
    			"type":"",										//规格
    			"ProjectDetailList":[],							//可选规格数组
    			"practice":[],									//做法
    			"practiceList":[],								//可选做法数组
    			"requirement":[],								//要求
    			"requirementList":[],							//可选要求数组
    			"garnish":[],									//配菜
    			"garnishList":[],								//可选配菜数组
    			"extendTotal":0,								//做法要求配菜	总价
				"total":''										//总价
    		},
    		isShopCartShow:false,								//是否显示菜单
    		isBigPhotoShow:false,								//是否打开大图模式
    		BigPhotoIndex:-1,									//大图当前下标
    		isBigPhotoDescribeBtnShow:true,						//当前大图介绍按钮是否显示
    		isBigPhotoDescribeShow:false,						//当前大图介绍是否打开
    		menuActive_frist:0,									//当前选择的左侧导航栏  一级分类
    		menuActive_second:-1,								//当前选择的左侧导航栏  二级分类
    		ProjectAndDetailsData:{								//当前分类	可选菜品
    			smallImage:'',									//菜品（小图）
    			CoverUrl:'',									//菜品（大图）
				practice:[],									//菜品	做法
    			Requirement:[],									//菜品	要求
    			Garnish:[]										//菜品	配菜
    		},	
    		ProjectAndDetailsType:1,							//可选菜品	显示模式  （ 1：图片模式     2：文字模式   ）
    		OrderTableProjectsdata:[],							//已选菜品数组
    		chosenTableData:{									//桌台选择  原始数据
    			Areas:[],										//区域
    			Tables:[]										//桌台
    		},
    		chosenTableDataNow:[],							//当前区域	桌台数据
    		SelectedProjectTable:$(window).height() - 88
    	}
 	},
 	computed : {
 		//购物车		数量		价格		总计
		SelectedProjectTotal: function () {
			if(this.OrderTableProjectsdata.length == 0){
				return {orderSum:0,sum:0,total:'0.00'};
			}else{
				var obj = {sum:0,total:0};
				obj.orderSum = this.OrderTableProjectsdata.length;
				for(let i = 0;i < this.OrderTableProjectsdata.length;i++){
					obj.sum += this.OrderTableProjectsdata[i].Num;
					obj.total = Math.round(parseFloat(obj.total) * 100 + parseFloat(this.OrderTableProjectsdata[i].total) * 100)/100;
				}
				obj.total = obj.total.toFixed(2);
				return obj;
			}
		},
	},
	watch: {
		//检索	菜品搜索
	    searchInfo :{
	    	handler:function(val,oldval){
	    		if(this.searchInfoEmpty)this.searchInfoEmpty=false;
	    		//关闭左侧栏  1-2级分类
	    		this.menuActive_frist = -1;
	    		this.menuActive_second = -1;
	    		
	    		val = val.toLocaleUpperCase();
	    		var newsArr = [];
	    		var inidataCopy = [];
				$.extend(true, inidataCopy, this.inidata.ProjectAndDetails);
		     	if(val == '') {
					newsArr = inidataCopy;
				} else {
					for(var i = 0; i < inidataCopy.length; i++) {
						var item = inidataCopy[i];
						if(item.Name.indexOf(val) >= 0){
							newsArr.push(item);
						}else if(item.CharsetCodeList) { //存在 code   
							//拼接 所有code
							var code = '';
							for(var j = 0; j < item.CharsetCodeList.length; j++) {
								code += item.CharsetCodeList[j].Code.toUpperCase();
							}
							if(code.indexOf(val) >= 0) { //成立
								newsArr.push(item);
							}
						}
					}
				}
				this.ProjectAndDetailsData = newsArr;
				this.imgLazy();//图片开启懒加载
		    }
	    }
	},
	beforeMount(){
		//餐厅ID  分市获取
		var RestaurantsData = JSON.parse(sessionStorage.getItem("RestaurantsData"));
		//防止报错
		if(!RestaurantsData){location.replace("/Res/Flat");return false}
    	this.restaurantId = RestaurantsData.Id;
    	this.MarketList = RestaurantsData.MarketList;
	},
  	mounted(){
		this.Initialization();
	},
	methods:{
		//初始化
        Initialization(){
        	//餐厅以及分市信息选择
        	var _this = this;
        	$.ajax({
	            type: "POST",
	            url: "/Res/Flat/GetAllCategoryProject",
	            dataType: "json",
	            data: {restaurantId:_this.restaurantId},
	            success: function(data){
	            	if(data.Successed){
	            		var Data = data.Data;
//	            		$('#initLoading').remove();
	            		//图片地址初始化
	            		for (let i = 0; i < Data.ProjectAndDetails.length; i++) {
	            			var $arr = Data.ProjectAndDetails[i].CoverUrl ? Data.ProjectAndDetails[i].CoverUrl.split('.') : [''];
	            			Data.ProjectAndDetails[i].smallImage = $arr.length > 1 ? Data.ProjectAndDetails[i].CoverUrl + '320x.' + $arr[1] : '';
	            			Data.ProjectAndDetails[i].chosenNum = 0;
	            		}
						//做法	要求		配菜		初始化
						for (let i = 0; i < Data.ProjectAndDetails.length; i++) {
							if(Data.ProjectAndDetails[i].CyddMxType == 1){
								Data.ProjectAndDetails[i].practiceList = [];			//菜品	做法
								Data.ProjectAndDetails[i].requirementList = [];			//菜品	要求
								Data.ProjectAndDetails[i].garnishList = [];				//菜品	配菜
								if(Data.ProjectAndDetails[i].ExtendList){
									for (var j = 0; j < Data.ProjectAndDetails[i].ExtendList.length; j++) {
										switch (Data.ProjectAndDetails[i].ExtendList[j].ExtendType){
											case 1:
												Data.ProjectAndDetails[i].practiceList.push(Data.ProjectAndDetails[i].ExtendList[j])
												break;
											case 2:
												Data.ProjectAndDetails[i].requirementList.push(Data.ProjectAndDetails[i].ExtendList[j])
												break;
											case 3:
												Data.ProjectAndDetails[i].garnishList.push(Data.ProjectAndDetails[i].ExtendList[j])
												break;
											default:
												break;
										}
									}
								}
							}
						}
						
		            	_this.inidata = Data;
		            	
		            	//默认加载第一个分类
		            	var inidataCopy = [];
						$.extend(true, inidataCopy, Data.ProjectAndDetails);
						var defaultDetailsArr = [];
						var defaultCategoryList = _this.inidata.CategoryList[0];
						if (_this.inidata.CategoryList[0].ChildList.length > 0) { //有子分类
			                for (let i = 0; i < defaultCategoryList.ChildList.length; i++) {
			                    var classid = defaultCategoryList.ChildList[i].Id;
			                    for (var j = 0; j < _this.inidata.ProjectAndDetails.length; j++) {
			                        var item = inidataCopy[j];
			                        if (classid == item.Category) { //成立
			                            defaultDetailsArr.push(item);
			                        }
			                    }
			                }
			            } else { //没有子分类的
			                for (let j = 0; j < _this.inidata.ProjectAndDetails.length; j++) {
			                    var item = inidataCopy[j];
			                    if (defaultCategoryList.Id == item.Category) { //成立
			                        defaultDetailsArr.push(item);
			                    }
			                }
			            }
						
		            	_this.ProjectAndDetailsData = defaultDetailsArr;
		            	_this.$nextTick(function () {	// DOM 更新了
					        $("img.lazy").lazyload({effect: "fadeIn",skip_invisible : false,container: $(".el-main")});
				      	})
	            	}else{
	            		if(data.Message)_this.$alert(data.Message, '提示', {confirmButtonText: '确定'});
	            	}
	            	$('#initLoading').remove();
                }
	        });
        },
        //左侧栏切换   一级分类
        menuChange(index){
        	if(this.menuActive_frist == index && this.menuActive_second == -1 && this.searchInfoEmpty)return;
        	
        	this.searchInfoEmpty = true;
        	
        	this.menuActive_frist = index;
        	this.menuActive_second = -1;
        	
        	var inidataCopy = [];
			$.extend(true, inidataCopy, this.inidata.ProjectAndDetails);
        	var newsArr = [];
        	var CategoryList = this.inidata.CategoryList[index];
            if (CategoryList.ChildList.length > 0) { //有子分类
                for (let i = 0; i < CategoryList.ChildList.length; i++) {
                    classid = CategoryList.ChildList[i].Id;
                    for (let j = 0; j < this.inidata.ProjectAndDetails.length; j++) {
                        var item = inidataCopy[j];
                        if (classid == item.Category) { //成立
                            newsArr.push(item);
                        }
                    }
                }
            } else { //没有子分类的
                for (let j = 0; j < this.inidata.ProjectAndDetails.length; j++) {
                    var item = inidataCopy[j];
                    if (CategoryList.Id == item.Category) { //成立
                        newsArr.push(item);
                    }
                }
            }
            
            this.ProjectAndDetailsData = newsArr;
        	
			this.imgLazy();
			
			this.searchInfo = '';
        },
        //左侧栏切换   二级分类
        menuChangeSecond(index){
        	if(this.menuActive_second == index)return;
        	this.menuActive_second = index;
        	var classno = this.menuActive_frist;
        	
        	var inidataCopy = [];
			$.extend(true, inidataCopy, this.inidata.ProjectAndDetails);
        	
        	var newsArr = [];
        	var classdata = this.inidata.CategoryList[classno];
            var classid = classdata.ChildList[index].Id;
            for (let j = 0; j < this.inidata.ProjectAndDetails.length; j++) {
                var item = inidataCopy[j];
                if (classid == item.Category) { //成立
                    newsArr.push(item);
                }
            }
            this.ProjectAndDetailsData = newsArr;
        	
        	this.imgLazy()
        },
        //大图模式开启
        bigPhotoShow(index){
        	var _this = this;
        	var $index = index;
        	//过滤不可选桌台
        	for(var i=index;i>=0;i--){
        		if( _this.ProjectAndDetailsData[i].IsStock && (!_this.ProjectAndDetailsData[i].Stock || _this.ProjectAndDetailsData[i].Stock <= 0) ){
        			index--;
        		}
        	}
        	
        	this.isBigPhotoShow = true;
        	var options = {
    			zoom : true,
        		initialSlide :index,
				loop:true,
				autoplayDisableOnInteraction: false,
		      	observer:true,//修改swiper自己或子元素时，自动初始化swiper
		      	observeParents:true,//修改swiper的父元素时，自动初始化swiper
		      	on: {
		      		slideChangeTransitionEnd: function(){
  						
  						var index = $(this.slides[this.snapIndex]).attr('data-index');
  						
  						_this.BigPhotoIndex = index;
  						
//	      						//判断  描述是否显示
  						if(_this.ProjectAndDetailsData[index].Describe && _this.ProjectAndDetailsData[index].Describe != ''){
  							_this.isBigPhotoDescribeBtnShow = true;
  						}else{
  							_this.isBigPhotoDescribeBtnShow = false;
  						}
					}
		      	},
		      	lazy: {
					loadPrevNext: true,
				}
		    }
        	if(_this.ProjectAndDetailsData.length < 2)options.loop = false;
        	setTimeout(function(){
        		var swiper = new Swiper('#BigPhoto .swiper-container', options);
        	},500)
        },
        //大图模式	点菜
        bigPhotoAddSelect(index){
        	var pro = this.ProjectAndDetailsData[index];
        	var maxNum = !pro.IsStock ? 9999 : pro.Stock - pro.chosenNum;
        	if(pro.CyddMxType == 1) {  // 1 是菜品  2是套餐
        		var data = this.createNewDish(pro);
        		data.SpecificationsList = pro.ProjectDetailList;//规格数组
        		data.maxNum = maxNum;//最大可输入数量
        		data.index = -1;//下标   -1表示为大图点菜
			} else {
				var data = this.createNewSetMeal(pro)
				data.index = -1;
			}
			this.reviselayerData = data;
			
			this.isReviseLayerShow = true;
        },
        //已选菜品	显示模式
        cardTypeChange(type){
        	if(this.ProjectAndDetailsType == type)return;
        	
        	this.ProjectAndDetailsType = type;
        	
        	this.imgLazy();
        },
        //添加菜品
        addSelect(index){
        	var pro = this.ProjectAndDetailsData[index];
			
        	pro.chosenNum ? pro.chosenNum++ : pro.chosenNum = 1;
        	
        	//计算原始数据列表中指定菜品  总数量
			this.inidataCount(pro.Id,pro.CyddMxType,1)
        	
        	//统计出当前菜品   在已选菜品中的总数
        	this.OrderTableProjectsdataCount(pro.Id,pro.CyddMxType,1);
//          	this.$set(pro,'chosenNum',pro.chosenNum);
//          	this.$forceUpdate();//强制更新
			// 1 是菜品  2是套餐
			var data;
			pro.CyddMxType == 1 ?  data = this.createNewDish(pro) : data = this.createNewSetMeal(pro);
			this.OrderTableProjectsdata.push(data);
		},
		//减少菜品
		removeSelect(index){
			var data = this.inidata.ProjectAndDetails;
			var pro = this.ProjectAndDetailsData[index];
        	pro.chosenNum--;
        	var id = pro.Id;
        	var CyddMxType = pro.CyddMxType;
        	
        	
        	//购物车  减
        	for(let i=this.OrderTableProjectsdata.length-1;i>=0;i--){
        		if(this.OrderTableProjectsdata[i].R_Project_Id == id && this.OrderTableProjectsdata[i].CyddMxType == CyddMxType){
        			if(this.OrderTableProjectsdata[i].Num > 1){  //如果有多个
        				this.OrderTableProjectsdata[i].Num--;
        				this.OrderTableProjectsdata[i].total = (Math.round( this.OrderTableProjectsdata[i].Price * 100 * this.OrderTableProjectsdata[i].Num * 100 ) / 10000).toFixed(2);
        			}else{
        				this.OrderTableProjectsdata.splice(i,1);
        			}
        			break;
        		}
        	}
        	
        	//计算原始数据列表中指定菜品  总数量
			this.inidataCount(id,CyddMxType,-1)
        	
        	//统计出当前菜品   在已选菜品中的总数
        	this.OrderTableProjectsdataCount(id,CyddMxType,-1);
		},
		//购物车		删除已选菜品
		deleteOrder(scope,rows){
			rows.splice(scope.$index,1);
		},
		//购物车		菜品快捷加减数量 =>   加
		plusNum(scope){
			//通用所有数据	指定菜品  总数量计算
        	this.currencyDataCount(scope.row.R_Project_Id,scope.row.CyddMxType,1)
        	
			scope.row.Num++;
			scope.row.total = (scope.row.Num * scope.row.Price).toFixed(2);
		},
		//购物车		菜品快捷加减数量 =>   减
		minusNum(scope){
			if(scope.row.Num > 1){
            	//通用所有数据	指定菜品  总数量计算
        		this.currencyDataCount(scope.row.R_Project_Id,scope.row.CyddMxType,-1)
				
				scope.row.Num--;
				scope.row.total = (scope.row.Num * scope.row.Price).toFixed(2);
			}
		},
		//购物车		菜品		删除
		deleteOrderTable(scope){
			this.$confirm('是否确认删除菜品(' + scope.row.CyddMxName + ')', '提示', {
				confirmButtonText: '确定',
				cancelButtonText: '取消',
				type: 'warning',
			}).then(() => {
            	//通用所有数据	指定菜品  总数量计算
        		this.currencyDataCount(scope.row.R_Project_Id,scope.row.CyddMxType,-scope.row.Num)
				
				
				this.OrderTableProjectsdata.splice(scope.$index, 1)
				this.$message({type: 'success',message: '删除成功!'});
			});
		},
		//购物车		全部删除
		allSelectDelete(){
			this.$confirm('是否确认删除全部菜品', '提示', {
				confirmButtonText: '确定',
				cancelButtonText: '取消',
				type: 'warning',
			}).then(() => {
				//原始数据
				var data = this.inidata.ProjectAndDetails;
            	for(let i=0;i<data.length;i++){
        			data[i].chosenNum = 0;
            	}
            	
            	//当前可选菜品
            	var currentData = this.ProjectAndDetailsData;
            	for(let i=0;i<currentData.length;i++){
        			currentData[i].chosenNum = 0 ;
            	}
            	
				
				this.OrderTableProjectsdata = [];
				
				this.$message({type: 'success',message: '删除成功!'});
			});
		},
		//购物车		即起   叫起切换
		DishesStatusChange(scope,type){
			scope.row.DishesStatus = type;
		},
		//购物车
		/*
		 * 1  全单即起
		 * 2  全单叫起
		 */
		DishesStatus(type){
			if(this.OrderTableProjectsdata.length == 0){
				this.messageIndex.close && this.messageIndex.close()
				this.messageIndex = this.$message({type: 'warning',message: '请先选择菜品!'});
				return false;
			}
			var title = type == 1 ? '即起': '叫起';
			this.$confirm('是否确认操作全单' + title, '提示', {
				confirmButtonText: '确定',
				cancelButtonText: '取消',
				type: 'warning',
			}).then(() => {
				var data = this.OrderTableProjectsdata;
            	for(let i=0;i<data.length;i++){
        			data[i].DishesStatus = parseFloat(type);
            	}
				
				this.$message({type: 'success',message: '全单' + title +  '完成!'});
			});
		},
		//购物车		菜品		修改  按钮点击
		reviseOrder(scope){
			var data = this.inidata.ProjectAndDetails;
			var rowData = scope.row;
			var id = rowData.R_Project_Id;
			//获取菜品下标
        	for(let i=0;i<data.length;i++){
        		if(data[i].Id == id){
        			this.reviselayerDataIndex = i;
        			break;
        		}
        	}
        	//做法要求配菜  获取已选中索引
			var maxNum = !rowData.IsStock ? 9999 : rowData.Stock - rowData.totalNum + rowData.Num;
			var copyData = {};//复制原始菜品数据
			$.extend(true, copyData, rowData);
			copyData.index = scope.$index;//我的菜单中  菜品下标
			copyData.maxNum = maxNum;//最大可输入数量
			if(rowData.CyddMxType == 1)copyData.extendTotal = 0;//做法要求配菜	总价  归0
			this.reviselayerData = copyData;

        	this.isReviseLayerShow = true;
		},
		//修改弹窗	数量		修改
		reviseChangeNum(val){
			var data = this.reviselayerData;
			var Unit = this.reviselayerData;
			data.total = (Math.round(data.totalPrice * 100 * val * 100)/10000).toFixed(2);
		},
		//修改弹窗	规格		切换
		reviseChangeUnit(val){
			var data = this.reviselayerData;
			var index = -1;
			
			for(let i = 0;i < data.specificationsList.length;i++){
				if(data.specificationsList[i].Unit == val){
					index = i;
					break;
				}
			}
			data.typeIndex = index;
			data.totalPrice = (data.specificationsList[index].Price + data.extendTotal ).toFixed(2);
			data.total = (Math.round(data.totalPrice * 100 * data.Num * 100)/10000).toFixed(2);
		},
		//修改弹窗	做法	 要求  配菜	切换时计算
		reviseChangeExtend(){
			var data = this.reviselayerData;
			var extendTotal = 0;//做法要求配菜	总价
			var Extend = [];		//做法要求配菜	数组
			//做法
			for(let i = 0;i < data.practice.length;i++){
				extendTotal += data.practiceList[data.practice[i]].Price;
				Extend.push(data.practiceList[data.practice[i]]);
			}
			//要求
			for(let i = 0;i < data.requirement.length;i++){
				extendTotal += data.requirementList[data.requirement[i]].Price;
				Extend.push(data.requirementList[data.requirement[i]]);
			}
			//配菜
			for(let i = 0;i < data.garnish.length;i++){
				extendTotal += data.garnishList[data.garnish[i]].Price;
				Extend.push(data.garnishList[data.garnish[i]]);
			}
			data.extendTotal = extendTotal;
			data.Extend = Extend;
			data.totalPrice = (data.specificationsList[data.typeIndex].Price + data.extendTotal).toFixed(2);
			data.total = (Math.round(data.totalPrice * 100 * data.Num * 100)/10000).toFixed(2);
		},
		//菜品  修改   以及    大图点选   弹窗    => 提交
		reviselayerSubmit(){
			var data = this.reviselayerData;
			if(data.index >= 0){	//修改
				var row = this.OrderTableProjectsdata[data.index];
				var id = data.R_Project_Id;
				var CyddMxType = data.CyddMxType;
				var Dvalue = data.Num - this.OrderTableProjectsdata[data.index].Num;//数量差值
				if(CyddMxType == 1){
					this.$set(this.OrderTableProjectsdata, data.index, data)
					row.ProjectDetailList = [data.specificationsList[data.typeIndex]];//规格修改
				}else{
					row.Num = data.Num;
					row.total = data.total;
				}
				
			}else{	//大图点菜
				var Dvalue = data.Num;
				this.OrderTableProjectsdata.push(data)
			}
			//通用所有数据	指定菜品  总数量计算
        	this.currencyDataCount(data.R_Project_Id,data.CyddMxType,Dvalue)
			this.isReviseLayerShow = false;
			this.$forceUpdate();//强制更新
			this.$message({type: 'success',message: '修改完成!'});
		},
		//下单
		buying(){
			if(this.OrderTableProjectsdata.length == 0){
				this.messageIndex = this.$message({type: 'warning',message: '请选择菜品！'});
				return false;
			}
			this.loginDialogShow = true;
		},
		//通用所有数据	指定菜品  总数量计算
		currencyDataCount(id,CyddMxType,val){
			//计算当前菜品列表中指定菜品  总数量
        	this.currentDataCount(id,CyddMxType,val)
        	
        	//计算原始数据列表中指定菜品  总数量
			this.inidataCount(id,CyddMxType,val)
        	
        	//统计出当前菜品   在已选菜品中的总数
        	this.OrderTableProjectsdataCount(id,CyddMxType,val);
		},
		//计算当前菜品列表中指定菜品  总数量
		currentDataCount(id,CyddMxType,val){
			var currentData = this.ProjectAndDetailsData;
        	for(let i=0;i<currentData.length;i++){
        		if(currentData[i].Id == id && currentData[i].CyddMxType == CyddMxType) {
        			currentData[i].chosenNum ? currentData[i].chosenNum += val : currentData[i].chosenNum = val;
        			break;
        		}
        	}
		},
		//计算原始数据列表中指定菜品  总数量
		inidataCount(id,CyddMxType,val){
			var data = this.inidata.ProjectAndDetails;
        	for(var i=0;i<data.length;i++){
        		if(data[i].Id == id && data[i].CyddMxType == CyddMxType){
        			data[i].chosenNum ? data[i].chosenNum += val : data[i].chosenNum = val;
        			break;
        		}
        	}
        	return i;
		},
		//计算已选菜品   指定菜品总计数量
		OrderTableProjectsdataCount(id,CyddMxType,val){
			var data = this.inidata.ProjectAndDetails;
			//统计出当前菜品   在已选菜品中的总数
			for (let j = 0; j < this.OrderTableProjectsdata.length; j++) {
				if(this.OrderTableProjectsdata[j].R_Project_Id == id && this.OrderTableProjectsdata[j].CyddMxType == CyddMxType){
					 this.OrderTableProjectsdata[j].totalNum += val;
				}
			}
		},
		//创建新的菜品对象
		createNewDish(pro){
			var detail = pro.ProjectDetailList[0]
			var data = {
				CyddTh: null,
				typeIndex:0,	//规格下标
				IsStock:pro.IsStock,	//是否启用库存
				Stock:pro.Stock,		//库存数量
//						CyddTh: OrderTableIds,
				CyddMxType: pro.CyddMxType,
				CyddMxId: detail.Id,
				Price: detail.Price.toFixed(2),
				totalPrice:detail.Price.toFixed(2),//单价（做法要求配菜）
				Num: 1,
				CyddMxStatus: 0,
				ProjectName: pro.Name,
				Unit: pro.ProjectDetailList[0].Unit,
				Id: 0,
				R_Project_Id: pro.Id,
				CyddMxName: pro.Name,
				ProjectDetailList: [detail],
				CostPrice: pro.CostPrice,
				IsChangePrice: pro.IsChangePrice,
				IsChangeNum: pro.IsChangeNum,
				IsCustomer: pro.IsCustomer,
				IsUpdataPrice: false,
				IsGive: pro.IsGive,
				OrderDetailRecordCount: [],
				DishesStatus: 1,
				OrderDetailRecord: pro.OrderDetailRecord,
				total:detail.Price.toFixed(2),
				totalNum:pro.chosenNum,
				type:detail.Unit,	//规格	
        		specificationsList:pro.ProjectDetailList,	//规格数组
        		practice:[],	//已选做法
        		practiceList:pro.practiceList,	//做法数组
        		requirement:[],	//已选要求
        		requirementList:pro.requirementList,	//要求数组
        		garnish:[],	//已选配菜
        		garnishList:pro.garnishList,	//配菜数组
        		extendTotal:0,	//做法要求配菜	总价
        		Extend:[]	//做法要求配菜归总
			}
			
			return data;
		},
		//创建新的套餐对象
		createNewSetMeal(pro){
			var data = {
				IsStock:pro.IsStock,	//是否启用库存
				Stock:pro.Stock,		//库存数量
				CyddTh: null,
				CyddMxId: pro.Id,
				CyddMxType: pro.CyddMxType,
				Price: pro.Price.toFixed(2),
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
				DishesStatus: 1,
				Extend: [],
				PackageDetailList: pro.PackageDetailList,
				IsGive: 0,
				OrderDetailRecord: pro.OrderDetailRecord,
				total:pro.Price.toFixed(2),
				totalNum:pro.chosenNum
			}
			return data;
		},
		//登录
		loginSubmit(){
			var _this = this;
			this.messageIndex.close && this.messageIndex.close();
			if(this.user.Account == ''){
				this.messageIndex = this.$message({type: 'warning',message: '请输入用户名！'});
				return false;
			}
			this.fullscreenLoading = true;
			
			var req = {};
			$.ajax({
	            type: "POST",
	            url: "/Res/Account/FlatLoginIn",
	            dataType: "json",
	            data: _this.user,
	            beforeSend: function(){
	            	_this.fullscreenLoading = true;
			    },
			    complete: function(){
			    	_this.fullscreenLoading = false;
			    },
	            success: function(data){
	            	if(data.Successed){
	            		this.user = {Account:'',PassWord:''};//清空用户登录信息
	            		_this.userInfo = data.Data;
	            		_this.loginDialogShow = false;
	            		_this.changeTableInit();
	            	}else{
	            		_this.messageIndex = _this.$message({type: 'warning',message: data.Message});
	            	}
                }
	        });
		},
		//用户登录信息清空
		userInfoEmpty(done){
			this.user = {Account:'',PassWord:''};//清空用户登录信息
			this.chosenRestaurantOrderAgain();
			if(done)done();
		},
		//取消登录
		loginCancel(){
			this.userInfoEmpty();
			this.loginDialogShow = false;
		},
		//桌台加载
		changeTableInit(){
			var _this = this;
			$.ajax({
	            type: "POST",
	            url: "/Res/Flat/InitAllChangeTableInfo",
	            dataType: "json",
	            data: {restaurantId:_this.restaurantId,user:_this.userInfo},
	            beforeSend: function(){
	            	_this.fullscreenLoading = true;
			    },
			    complete: function(){
			    	_this.fullscreenLoading = false;
			    },
	            success: function(data){
	            	_this.fullscreenLoading = false;
	            	data.AreasIndex = 0;//当前区域下标
	            	for(let i=0;i < data.Tables.length;i++){
	            		data.Tables[i].index = i;
	            	}
	            	_this.chosenTableData = data;
	            	_this.chosenRestaurantOrderShow = true;
	            	
	            	_this.chosenChangeArea(0)
                }
	        });
		},
		//桌台弹窗	区域切换
		chosenChangeArea(index){
			this.chosenTableData.AreasIndex = index;
			var AreaId = this.chosenTableData.Areas[index].Id;
			var newArr = [];
			for(let i=0;i<this.chosenTableData.Tables.length;i++){
				if(this.chosenTableData.Tables[i].AreaId == AreaId){
					newArr.push(this.chosenTableData.Tables[i])
				}
			}
			this.chosenTableDataNow = newArr;
		},
		//桌台选择
		chosenOrder(index){
			var _this = this;
			var tableData = this.chosenTableData.Tables[index];
			if(tableData.CurrentOrderList.length > 0 && tableData.CurrentOrderList[0].IsLock){
				this.messageIndex.close && this.messageIndex.close();
				this.messageIndex = this.$message({type: 'warning',message: '该桌台已被锁定！'});
				return false;
			}
			
			if(tableData.CythStatus == 3){
				this.messageIndex.close && this.messageIndex.close();
				this.messageIndex = this.$message({type: 'warning',message: '不能选择脏台'});
				return false;
			}
			this.$confirm('是否确认选择桌台('+ tableData.Name +')?', '提示', {
      		confirmButtonText: '确定',
      		cancelButtonText: '取消',
	        }).then(() => {
		        //已存在订单
				if(this.chosenTableData.Tables[index].CurrentOrderList.length > 0){
					var para = {};
					para.user = this.userInfo;//用户信息
					para.req = this.OrderTableProjectsdata;
					para.orderTableIds = [this.chosenTableData.Tables[index].CurrentOrderList[0].Id];
					para.status = 2;
					$.ajax({
			            type: "POST",
			            url: "/Res/Flat/SubmitOrderAdd",
			            dataType: "json",
			            data: para,
			            beforeSend: function(){
			            	_this.fullscreenLoading = true;
					    },
			            success: function(data){
			            	if(data.Successed){
			            		_this.messageIndex = _this.$message({type: 'success',message: '点餐成功,正在刷新页面...'});
			            		setTimeout(()=>{location.reload();},2000)
			            	}else{
			            		_this.messageIndex = _this.$message({type: 'warning',message: data.Message});
			            		_this.fullscreenLoading = false;
			            	}
		                }
			        });
				}else{	
					//新订单
					this.chosenRestaurantOrderStep = 2;
					var data = this.chosenTableData;//桌台选择源数据
					//默认值加载
					this.openTableForm.R_Market_Id = this.MarketList.Id;//分市
					this.openTableForm.CyddOrderSource = data.CustomerSources[0].Id;//客源类型ID
					this.openTableForm.CustomerId = data.CustomerList[0].Id;//客户ID
					this.openTableForm.OrderType = data.OrderTypes[0].Id;//订单类型ID
					this.openTableForm.orderTableIds = [this.chosenTableData.Tables[index].Id];//餐桌ID
				}
	       }).catch(() => {});
			
		},
		//新订单提交
		newOpenTableSubmit(formName){
			var _this = this;
			this.$refs[formName].validate((valid) => {
          		if (valid) {
          			var para = {};
					para.req = this.openTableForm;//开台信息
					para.req.R_Restaurant_Id = this.restaurantId;//所属餐厅ID
					para.user = this.userInfo;//用户信息
					para.list = this.OrderTableProjectsdata;
					para.tableIds = this.openTableForm.orderTableIds;
					para.status = 2
					$.ajax({
			            type: "POST",
			            url: "/Res/Flat/SubmitOrder",
			            dataType: "json",
			            data: para,
			            beforeSend: function(){
			            	_this.fullscreenLoading = true;
					    },
			            success: function(data){
			            	if(data.Successed){
			            		_this.messageIndex = _this.$message({type: 'success',message: '点餐成功,正在刷新页面...'});
			            		setTimeout(()=>{location.reload();},2000)
			            	}else{
			            		_this.messageIndex = _this.$message({type: 'warning',message: data.Message});
			            		_this.fullscreenLoading = false;
			            	}
		                }
			        });
	          	} else {
		            console.log('error submit!!');
		            return false;
	          	}
        	});
		},
		//重新选台
		chosenRestaurantOrderAgain(){
			this.chosenRestaurantOrderStep = 1;
			//重置开台信息
			this.openTableForm = {PersonNum:'',R_Market_Id:0,CyddOrderSource:0,ContactPerson:'',CustomerId:0,ContactTel:'',OrderType:0,Remark:''}
		},
		//取消选台
		chosenRestaurantOrderCancel(){
			this.user = {Account:'',PassWord:''};//清空用户登录信息
			this.chosenRestaurantOrderAgain();
			this.chosenRestaurantOrderShow = false;
		},
		//图片懒加载
		imgLazy(){
			if(this.ProjectAndDetailsType == 1){
        		// DOM 更新了
	        	this.$nextTick(function () {
			        $("img.lazy").lazyload({effect: "fadeIn",skip_invisible : false,container: $(".el-main")});
		      	})
        	}
		},
		//刷新  重置页面
		reload(){
			location.reload();
		}
        
    }
})