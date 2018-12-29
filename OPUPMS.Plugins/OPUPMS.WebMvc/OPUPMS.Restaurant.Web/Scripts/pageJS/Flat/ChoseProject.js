var vm = new Vue({
	el: '#app',
	data: function data() {
		var isNumberValidator = function isNumberValidator(rule, value, callback) {
			if (!value) {
				return callback(new Error('就餐人数不能为空'));
			}
			if (isNaN(value) || !/^[0-9]+$/.test(value)) {
				callback(new Error('请输入数字'));
			} else {
				callback();
			}
		};
		return {
			fullscreenLoading: false,
			restaurantId: 0, //餐厅ID
			MarketList: {}, //餐厅分市选择的信息
			messageIndex: -1, //提示	下标
			inidata: {
				CategoryList: [], //左侧栏	导航数据
				ProjectExtendSplitList: [],	//更多做法要求配菜	数据
			},
			loginDialogShow: false, //用户登录弹窗是否开启
			chosenRestaurantOrderShow: false, //选择桌台弹窗是否开启
			chosenRestaurantOrderStep: 1, //当前桌台选择步骤	1：选择桌台	2：开台信息填写
			openTableForm: { //开台信息
				PersonNum: '', //就餐人数
				R_Market_Id: 0, //分市ID
				CyddOrderSource: 0, //客源类型
				ContactPerson: '', //联系人
				CustomerId: 0, //客户
				ContactTel: '', //联系电话
				OrderType: 0, //订单类型
				Remark: '' //留言
			},
			currentOrderListShow: false,//一台多单选择界面	是否开启
			currentOrderData: {},//一台多单	数据
			currentOrderIndex: -1,//一台多单  选择的台号下标
			rules2: { //验证规则
				PersonNum: [//验证数字
				{ validator: isNumberValidator, trigger: 'blur' }]
			},
			user: { //登录用户信息
				Account: '', //账号
				PassWord: '' //密码
			},
			userInfo: {}, //后端返回的用户信息
			searchInfoEmpty: true, //搜索信息清空（判断用）
			searchInfo:'',
			searchValue: '', //搜索条件
			isReviseLayerShow: false, //是否显示  点选 || 修改  菜品信息弹窗
			reviselayerDataIndex: -1, //当前打开的点选 || 修改数据的下标
			reviselayerData: { //点选 || 修改  菜品信息弹窗数据
				"index": -1, //购物车  当前修改的菜品	下标
				"CyddMxName": '', //名称
				"Unit": '', //单位
				"Price": 0, //金额
				"Num": 0, //数量
				"typeIndex": -1, //当前规格	下标		-1 表示未修改
				"type": "", //规格
				"ProjectDetailList": [], //可选规格数组
				"practice": [], //做法
				"practiceList": [], //可选做法数组
				"requirement": [], //要求
				"requirementList": [], //可选要求数组
				"garnish": [], //配菜
				"garnishList": [], //可选配菜数组
				"extendTotal": 0, //做法要求配菜	总价
				"total": '' //总价
			},
			isReviseExtendShow:false, //是否显示  修改 || 大图点菜  弹窗  =>  更多做法  更多要求   弹窗
			reviseExtendChosenType:0,//更多做法  更多要求   弹窗  =>  当前选择    1  做法   2  要求
			ReviseExtendclassifyIndex:0,	//更多做法  更多要求   弹窗  =>  分类索引
			ReviseExtendData:[	//更多做法  更多要求   弹窗  => 	数据
				{
					'Name':'',
					ProjectExtendDTOList:[],
					checked:0
				}
			],	
//			this.ReviseExtendData[this.ReviseExtendclassifyIndex].ProjectExtendDTOList[index]
			practiceExtendList:[],//更多做法
			requirementExtendList:[],//更多要求
			garnishExtendList:[],//更多配菜
			isShopCartShow: false, //是否显示菜单
			isBigPhotoShow: false, //是否打开大图模式
			BigPhotoData:[], //大图模式数据
			BigPhotoIndex: -1, //大图当前下标
			isBigPhotoDescribeBtnShow: true, //当前大图介绍按钮是否显示
			isBigPhotoDescribeShow: false, //当前大图介绍是否打开
			menuActiveFristExpand:true,//是否伸缩二级分类
			menuActiveFrist: -3, //当前选择的左侧导航栏  一级分类     -1 为 热推   -2 特价   -3 空
			menuActiveSecond: -1, //当前选择的左侧导航栏  二级分类
			ProjectAndDetailsData: [		//当前分类	可选菜品
//				{ 
//				smallImage: '', //菜品（小图）
//				CoverUrl: '', //菜品（大图）
//				practice: [], //菜品	做法
//				Requirement: [], //菜品	要求
//				Garnish: [] //菜品	配菜
//				}
			],
			ProjectAndDetailsDataClassifyIndex:0,	//当前分类下 二级分类索引	 若为 -1 表示没有二级分类索引
			ProjectAndDetailsDataIndex:0,	//当前分类最后一条数据在 源数据中的索引
			ProjectAndDetailsDataOnesNum:0,	//当前分类每次加载的数据数
			ProjectAndDetailsType: 1, //可选菜品	显示模式  （ 1：图片模式     2：文字模式   ）
			OrderTableProjectsdata: [], //已选菜品数组
			chosenTableData: { //桌台选择  原始数据
				Areas: [], //区域
				Tables: [] //桌台
			},
			chosenTableDataNow: [], //当前区域	桌台数据
			SelectedProjectTable: $(window).height() - 88,
			lineOrder:0,//当前一行可以显示的菜品数量
			orderNumFigure:1,//菜品数量可输入小数位数
		};
	},
	computed: {
		//购物车		数量		价格		总计
		SelectedProjectTotal: function SelectedProjectTotal() {
			if (this.OrderTableProjectsdata.length == 0) {
				return { orderSum: 0, sum: 0, total: '0.00' };
			} else {
				var obj = { sum: 0, total: 0 };
				obj.orderSum = this.OrderTableProjectsdata.length;
				for (var i = 0; i < this.OrderTableProjectsdata.length; i++) {
					obj.sum = this.addNumFloat([obj.sum,this.OrderTableProjectsdata[i].Num]);
					obj.total = this.addNumFloat([obj.total,this.OrderTableProjectsdata[i].total])
				}
				obj.total = obj.total.toFixed(2);
				return obj;
			}
		}
	},
	beforeMount: function beforeMount() {
		//餐厅ID  分市获取
		var RestaurantsData = JSON.parse(sessionStorage.getItem("RestaurantsData"));
		//防止报错
		if (!RestaurantsData) {
			location.replace("/Res/Flat");return false;
		}
		this.restaurantId = RestaurantsData.Id;
//		this.restaurantId = 1;
		this.MarketList = RestaurantsData.MarketList;
	},
	mounted: function mounted() {
		var winW = $(window).width();
		winW < 768 ? this.lineOrder = 2 : winW >= 768 && winW < 992 ? this.lineOrder = 3 : winW >= 992 && winW < 1200 ? this.lineOrder = 4 : winW >= 1200 && winW < 1920 ? this.lineOrder = 6 : this.lineOrder = 8;
		this.ProjectAndDetailsDataOnesNum = this.lineOrder * 4;	//设置每次加载4行的数据
		this.Initialization();
	},
	methods: {
		//公共方法	浮点数计算	=> 加
		addNumFloat: function addNumFloat(arr){
			var num = 0;
			for(let i=0;i<arr.length;i++){
				num = (Math.round(parseFloat(num) * 100 + parseFloat(arr[i]) * 100)/100)
			}
			return num
		},
		//公共方法	浮点数计算	=> 减
		minusNumFloat: function minusNumFloat(arr){
			var num = arr.length > 0 ? arr[0] : 0;
			for(let i=1;i<arr.length;i++){
				num = (Math.round(parseFloat(num) * 100 - parseFloat(arr[i]) * 100)/100)
			}
			return num
		},
		//公共方法	浮点数计算	=> 乘
		multiplyNumFloat: function multiplyNumFloat(arr){
			var num = arr.length > 0 ? arr[0] : 0;
			for(let i=1;i<arr.length;i++){
				num = (Math.round(parseFloat(num) * 100 * parseFloat(arr[i]) * 100)/10000)
			}
			return num
		},
		//公共方法	保留小数位数
		numFloatFigure: function floorNumFloat(val){
			var Multiple = 1;
			for(let i=0;i<this.orderNumFigure;i++){
				Multiple +='0'
			}
			Multiple = parseInt(Multiple);
			val = (Math.floor(parseFloat(val) * Multiple))/Multiple;
			return val
		},
		//初始化
		Initialization: function Initialization() {
			//餐厅以及分市信息选择
			var _this = this;
			$.ajax({
				type: "POST",
				url: "/Res/Flat/GetAllCategoryProject",
				dataType: "json",
				data: { restaurantId: _this.restaurantId },
				success: function success(data) {
					if (data.Successed) {
						var Data = data.Data;
						
						_this.inidata = Data;
						//更多做法要求配菜  初始化
						for (var i = 0; i < Data.ProjectExtendSplitList.length; i++) {
							for(var j = 0; j < Data.ProjectExtendSplitList[i].ProjectExtendDTOList.length;j++){
								Data.ProjectExtendSplitList[i].ProjectExtendDTOList[j].checked = false;
							}
							switch(Data.ProjectExtendSplitList[i].ExtendType){
								case 1:
									_this.practiceExtendList.push(Data.ProjectExtendSplitList[i])
									break;
								case 2:
									_this.requirementExtendList.push(Data.ProjectExtendSplitList[i])
									break;
								case 3:
									_this.garnishExtendList.push(Data.ProjectExtendSplitList[i])
									break;
							}
						}		

						//默认加载第一个分类
						_this.menuActiveFrist = 0;
						_this.fristdataLoad(true)
						
						_this.$nextTick(function () {
							
							$('#loading').hide();
							
							_this.mescroll = new MeScroll("orders", {
								down: {use:false},
								up: {
									hardwareClass:"mescroll-hardware",
									callback: _this.upCallback, //上拉回调
									isBounce: false, //此处禁止ios回弹
									empty:{ //配置列表无任何数据的提示
										warpId:"dataList",
									  	tip : "当前分类没有菜品"
									},
									noMoreSize:1,
									page:{
										num:0,
										size:_this.ProjectAndDetailsDataOnesNum
									},
									htmlNodata:'<p class="upwarp-nodata">已经是当前分类全部菜品</p>'
								}
							});
						});
					} else {
						if (data.Message) _this.$alert(data.Message, '提示', { confirmButtonText: '确定' });
						$('#loading').hide();
					}
				}
			});
		},
		//上拉回调
		upCallback: function upCallback(page) {
			$('#orders .mescroll-upwarp').css({'visibility': 'visible'});
			$('#orders .mescroll-upwarp').html('<svg viewBox="25 25 50 50" class="circularLoading"><circle cx="50" cy="50" r="20" fill="none" class="path"></circle></svg><p class="upwarp-tip">加载中..</p>')
			
			var _this = this;
			
			if(this.menuActiveFrist == - 1){	//热推
				this.specialDataLoad(-1)
			}else if(this.menuActiveFrist == - 2){	//特价
				this.specialDataLoad(-2)
			}else if(this.menuActiveSecond < 0 && this.menuActiveFrist < -2){
				//如果是搜索
				this.searchDataLoad();
			}else if(this.menuActiveSecond >= 0){
				//如果是二级分类
				this.seconddataLoad();
			}else{
				//如果是一级分类
				this.fristdataLoad();
			}
		},
		//特殊要求  分类   部分数据加载  -1 热推  -2特价
		specialDataLoad:function specialDataLoad(index,type){
			var _this = this;
			var newsArr = [];
			var inidataCopy = [];
			$.extend(true, inidataCopy, _this.inidata.ProjectAndDetails);
			for(var  i = type ? 0 : _this.ProjectAndDetailsDataIndex + 1 ,num = 0;i<inidataCopy.length && num < _this.ProjectAndDetailsDataOnesNum ;i++){
				if(index == -1 ? inidataCopy[i].IsRecommend > 0 : inidataCopy[i].IsSpecialOffer > 0){
					newsArr.push(inidataCopy[i]);
					num++;
				}
				_this.ProjectAndDetailsDataIndex = i;
			}
			
			this.DataLoad(newsArr,num,type);
		},
		//特殊要求  分类   -1 热推  -2特价
		specialChange: function specialChange(index){
			if(this.menuActiveFrist == index){
				return
			}
			
			this.menuActiveFristExpand = false;
			
			this.menuActiveFrist = index;
			
			this.searchInfoEmpty = true;

			this.menuActiveSecond = -1;
			

			this.specialDataLoad(index,true)

			this.searchInfo = '';
			
			$('.el-main').scrollTop(0);
		},
		// 一级分类  部分数据加载
		fristdataLoad:function fristdataLoad(type){
			var _this = this;
			var newsArr = [];
			var inidataCopy = [];
			$.extend(true, inidataCopy, _this.inidata.ProjectAndDetails);
			var defaultCategoryList = _this.inidata.CategoryList[_this.menuActiveFrist];
			if (defaultCategoryList.ChildList.length > 0) {
				//有子分类
				for (var _i2 = _this.ProjectAndDetailsDataClassifyIndex,num = 0; _i2 < defaultCategoryList.ChildList.length && num < _this.ProjectAndDetailsDataOnesNum; _i2++) {
					var classid = defaultCategoryList.ChildList[_i2].Id;
					if(_this.ProjectAndDetailsDataClassifyIndex != _i2){
						_this.ProjectAndDetailsDataIndex = 0;
					}
					_this.ProjectAndDetailsDataClassifyIndex = _i2;
					
					for (var j = type ? 0 : _this.ProjectAndDetailsDataIndex + 1 ; j < inidataCopy.length && num < _this.ProjectAndDetailsDataOnesNum; j++) {
						var item = inidataCopy[j];
						if (classid == item.Category) {
							newsArr.push(item);
							num++;
						}
						_this.ProjectAndDetailsDataIndex = j;
					}
				}
			} else {
				//没有子分类的
				for (var _j = type ? 0 : _this.ProjectAndDetailsDataIndex + 1 ,num = 0; _j < inidataCopy.length && _this.ProjectAndDetailsDataIndex < _this.ProjectAndDetailsDataOnesNum; _j++) {
					var item = inidataCopy[_j];
					if (defaultCategoryList.Id == item.Category) {
						newsArr.push(item);
						num++;
					}
					_this.ProjectAndDetailsDataIndex = j;
				}
			}
			
			this.DataLoad(newsArr,num,type);
		},
		//左侧栏切换   一级分类
		menuChange: function menuChange(index) {
//			this.menuActiveFrist == index && this.menuActiveSecond == -1 
//			if (this.menuActiveFrist == index && this.menuActiveSecond == -1 && this.searchInfoEmpty) return;
			
			this.menuActiveFristExpand = this.menuActiveFrist !== index ? true : this.menuActiveFristExpand ? false : true;
			
			if(this.menuActiveFrist == index && this.menuActiveSecond == -1 && this.searchInfoEmpty){
				return
			}
			
			this.searchInfoEmpty = true;

			this.menuActiveFrist = index;
			this.menuActiveSecond = -1;
			
			//重置  二级分类索引  和  数据索引
			this.ProjectAndDetailsDataIndex = 0;
			this.ProjectAndDetailsDataClassifyIndex = 0;
			
			this.fristdataLoad(true)

			this.searchInfo = '';
			
			$('.el-main').scrollTop(0);
		},
		// 二级分类  部分数据加载
		seconddataLoad:function seconddataLoad(type){
			var _this = this;
			var newsArr = [];
			var inidataCopy = [];
			$.extend(true, inidataCopy, _this.inidata.ProjectAndDetails);
			var newsArr = [];
			var classid = _this.inidata.CategoryList[_this.menuActiveFrist].ChildList[_this.menuActiveSecond].Id;
			for (var j = type ? 0 : _this.ProjectAndDetailsDataIndex + 1,num = 0; j < _this.inidata.ProjectAndDetails.length && num < _this.ProjectAndDetailsDataOnesNum; j++) {
				var item = inidataCopy[j];
				if (classid == item.Category) {
					newsArr.push(item);
					num++;
				}
				_this.ProjectAndDetailsDataIndex = j;
			}
			
			this.DataLoad(newsArr,num,type);
		},
		//左侧栏切换   二级分类
		menuChangeSecond: function menuChangeSecond(index) {
			if (this.menuActiveSecond == index) return;
			
			this.menuActiveSecond = index;
			
			this.ProjectAndDetailsDataIndex = 0;
			
			this.seconddataLoad(true);
			
			$('.el-main').scrollTop(0);
		},
		//搜索数据	部分数据加载
		searchDataLoad: function searchDataLoad(type){
			var _this = this;
			var val = _this.searchValue.toLocaleUpperCase();
			var newsArr = [];
			var inidataCopy = [];
			$.extend(true, inidataCopy, _this.inidata.ProjectAndDetails);
			if (val == '') {
				for(var i = type ? 0 : _this.ProjectAndDetailsDataIndex + 1,num = 0;i < inidataCopy.length && num < _this.ProjectAndDetailsDataOnesNum; i++){
					newsArr.push(inidataCopy[i]);
					num++;
					_this.ProjectAndDetailsDataIndex = i;
				}
			} else {
				for (var i = type ? 0 : _this.ProjectAndDetailsDataIndex + 1,num = 0; i < inidataCopy.length && num < _this.ProjectAndDetailsDataOnesNum; i++) {
					var item = inidataCopy[i];
					if (item.Name.indexOf(val) >= 0) {
						newsArr.push(item);
						num++;
					} else if (item.CharsetCodeList) { //存在 code
						//拼接 所有code
						var code = '';
						for (var j = 0; j < item.CharsetCodeList.length; j++) {
							code += item.CharsetCodeList[j].Code.toUpperCase();
						}
						if (code.indexOf(val) >= 0) {
							newsArr.push(item);
							num++;
						}
					}
					_this.ProjectAndDetailsDataIndex = i;
				}
			}
			
			this.DataLoad(newsArr,num,type);
		},
		//检索	菜品搜索
		searchInput: function searchInput() {
			if (this.searchInfoEmpty) this.searchInfoEmpty = false;
				
			$('#orders .mescroll-upwarp').html('<svg viewBox="25 25 50 50" class="circularLoading"><circle cx="50" cy="50" r="20" fill="none" class="path"></circle></svg><p class="upwarp-tip">加载中..</p>')
			//关闭左侧栏  1-2级分类
			this.menuActiveFrist = -3;
			this.menuActiveSecond = -1;
			
			this.searchValue = this.searchInfo;
			
			this.searchDataLoad(true);
			
			$('.el-main').scrollTop(0);
		},
		//菜品数据渲染	&&   图片懒加载
		DataLoad: function DataLoad(newsArr,num,type){
			var _this = this;
			if(!type){
				var imgIndex = $("#dataList img.lazy").length - 1;
			}
			
			type ? _this.ProjectAndDetailsData = newsArr: _this.ProjectAndDetailsData = _this.ProjectAndDetailsData.concat(newsArr);
			
			_this.mescroll && _this.mescroll.endSuccess(_this.ProjectAndDetailsData.length,num == _this.ProjectAndDetailsDataOnesNum);
			_this.$nextTick(function () {
				if(type){
					$("#dataList img.lazy").lazyload({ threshold :20,effect: "fadeIn", skip_invisible: false, container: $(".el-main") })
				}else{
					$("#dataList .el-col").eq(imgIndex).nextAll().find('img.lazy').lazyload({ threshold :20,effect: "fadeIn", skip_invisible: false, container: $(".el-main") });
					}
			})
		},
		//已选菜品	显示模式
		cardTypeChange: function cardTypeChange(type) {
			if (this.ProjectAndDetailsType == type) return;
			
			this.ProjectAndDetailsType = type;
			
			this.ProjectAndDetailsDataOnesNum = type == 1 ? this.ProjectAndDetailsDataOnesNum /2 : this.ProjectAndDetailsDataOnesNum * 2;
			
			var size = this.ProjectAndDetailsDataOnesNum;
			
			this.mescroll.setPageSize(size);
			this.$nextTick(function () {
				this.upCallback();
				if(type == 1)this.imgLazy();
			})
		},
		//大图模式开启
		bigPhotoShow: function bigPhotoShow(index) {
			var _this = this;
			var $index = index;
			this.BigPhotoIndex = index;
			var newsArr = [];
			var inidataCopy = [];
			$.extend(true, inidataCopy, _this.inidata.ProjectAndDetails);
			if(this.menuActiveFrist == - 1){	//热推
				for(var  i = 0;i<inidataCopy.length;i++){
					if(inidataCopy[i].IsRecommend > 0){
						newsArr.push(inidataCopy[i]);
					}
				}
			}else if(this.menuActiveFrist == - 2){	//特价
				for(var  i = 0;i<inidataCopy.length;i++){
					if(inidataCopy[i].IsSpecialOffer > 0){
						newsArr.push(inidataCopy[i]);
					}
				}
			}else if(_this.menuActiveSecond < 0 && _this.menuActiveFrist < -2){
				//如果是搜索
				var val = _this.searchValue.toLocaleUpperCase();
				if (val == '') {
					newsArr = inidataCopy;
				} else {
					
					for (var i = 0;i < inidataCopy.length; i++) {
						var item = inidataCopy[i];
						if (item.Name.indexOf(val) >= 0) {
							newsArr.push(item);
						} else if (item.CharsetCodeList) { //存在 code
							//拼接 所有code
							var code = '';
							for (var j = 0; j < item.CharsetCodeList.length; j++) {
								code += item.CharsetCodeList[j].Code.toUpperCase();
							}
							if (code.indexOf(val) >= 0) {
								newsArr.push(item);
							}
						}
					}
				}
			}else if(_this.menuActiveSecond >= 0){
				//如果是二级分类
				var classid = _this.inidata.CategoryList[_this.menuActiveFrist].ChildList[_this.menuActiveSecond].Id;
				for (var j = 0; j < _this.inidata.ProjectAndDetails.length; j++) {
					var item = inidataCopy[j];
					if (classid == item.Category) {
						newsArr.push(item);
					}
				}
			}else{
				//如果是一级分类
				var defaultCategoryList = _this.inidata.CategoryList[_this.menuActiveFrist];
				if (defaultCategoryList.ChildList.length > 0) {
					//有子分类
					for (var _i2 = 0; _i2 < defaultCategoryList.ChildList.length; _i2++) {
						var classid = defaultCategoryList.ChildList[_i2].Id;
						
						for (var j = 0 ; j < inidataCopy.length; j++) {
							var item = inidataCopy[j];
							if (classid == item.Category) {
								newsArr.push(item);
							}
						}
					}
				} else {
					//没有子分类的
					for (var _j = 0; _j < inidataCopy.length; _j++) {
						var item = inidataCopy[_j];
						if (defaultCategoryList.Id == item.Category) {
							newsArr.push(item);
						}
					}
				}
			}
			
			this.BigPhotoData = newsArr;
			
			//过滤不可选桌台
			for (var i = index; i >= 0; i--) {
				if (_this.BigPhotoData[i].IsStock && (!_this.BigPhotoData[i].Stock || _this.BigPhotoData[i].Stock <= 0)) {
					index--;
				}
			}
			this.isBigPhotoShow = true;
			var options = {
				zoom: true,
				initialSlide: index,
//				loop: true,
				autoplayDisableOnInteraction: false,
				observer: true, //修改swiper自己或子元素时，自动初始化swiper
				observeParents: true, //修改swiper的父元素时，自动初始化swiper
				on: {
					slideChangeTransitionEnd: function slideChangeTransitionEnd() {

						var index = $(this.slides[this.snapIndex]).attr('data-index');
						
						_this.BigPhotoIndex = index;
						//判断  描述是否显示
						if (_this.BigPhotoData[index].Describe && _this.BigPhotoData[index].Describe != '') {
							_this.isBigPhotoDescribeBtnShow = true;
						} else {
							_this.isBigPhotoDescribeBtnShow = false;
						}
					},
					touchEnd: function touchEnd(event){
						if(this.swipeDirection == "prev" && this.snapIndex == 0){
							_this.messageIndex.close && _this.messageIndex.close();
							_this.messageIndex = _this.$message({ type: 'warning', message: '这已经是第一道菜品!' });
						}else if(this.swipeDirection == "next" && this.slides.length == this.snapIndex + 1){
							_this.messageIndex.close && _this.messageIndex.close();
							_this.messageIndex = _this.$message({ type: 'warning', message: '这已经是最后一道菜品!' });
						}
					}
				},
				lazy: {
					loadPrevNext: true
				}
			};
			
			this.$nextTick(function () {
				var swiper = new Swiper('#BigPhoto .swiper-container', options);
			});
		},
		//大图模式关闭
		bigPhotoHide: function bigPhotoHide() {
			this.isBigPhotoShow=false;
			this.isBigPhotoDescribeShow=false;
			opu.showStatusBar();
			opu.showNavigAtionBar();
		},
		//大图模式	点菜
		bigPhotoAddSelect: function bigPhotoAddSelect(index) {
			var pro = this.BigPhotoData[index];
			if(pro.IsStock && pro.chosenNum == pro.Stock)return false;
//			var maxNum = !pro.IsStock ? 9999 : pro.Stock - pro.chosenNum;
			var maxNum = !pro.IsStock ? 9999 : this.minusNumFloat([pro.Stock,pro.chosenNum]);
			if (pro.CyddMxType == 1) {
				// 1 是菜品  2是套餐
				var data = this.createNewDish(pro);
				data.SpecificationsList = pro.ProjectDetailList; //规格数组
				data.maxNum = maxNum; //最大可输入数量
				data.index = -1; //下标   -1表示为大图点菜
				data.bigPhotoIndex = index;//大图索引
			} else {
				var data = this.createNewSetMeal(pro);
				data.index = -1;
			}
			this.reviselayerData = data;

			this.isReviseLayerShow = true;
		},
		//大图	减少数量
		bigPhotoMinusSelect: function bigPhotoMinusSelect(index){
			var pro = this.BigPhotoData[index];
			var id = pro.Id;
			var CyddMxType = pro.CyddMxType;
			pro.chosenNum = pro.chosenNum > 1 ? this.minusNumFloat([pro.chosenNum,1]) : 0;
			this.BigPhotoData.Num = this.BigPhotoData.Num > 1 ? this.minusNumFloat([this.BigPhotoData.Num,1]) : 0;

			//购物车  减
			for (var i = this.OrderTableProjectsdata.length - 1; i >= 0; i--) {
				if (this.OrderTableProjectsdata[i].R_Project_Id == id && this.OrderTableProjectsdata[i].CyddMxType == CyddMxType) {
					if (this.OrderTableProjectsdata[i].Num > 1) {
						//如果有多个
						this.OrderTableProjectsdata[i].Num = this.minusNumFloat([this.OrderTableProjectsdata[i].Num,1]);
//						this.OrderTableProjectsdata[i].Num--;
						this.OrderTableProjectsdata[i].total = this.multiplyNumFloat([this.OrderTableProjectsdata[i].Price,this.OrderTableProjectsdata[i].Num]).toFixed(2);
					} else {
						this.OrderTableProjectsdata.splice(i, 1);
					}
					break;
				}
			}
			//通用所有数据	指定菜品  总数量计算
			this.currencyDataCount(id, CyddMxType, -1);
		},
		//添加菜品
		plusSelect: function addSelect(index) {
			var pro = this.ProjectAndDetailsData[index];
			if(pro.IsStock && pro.chosenNum == pro.Stock)return false;
//			pro.chosenNum ? pro.chosenNum++ : pro.chosenNum = 1;
			pro.chosenNum ? pro.chosenNum = this.addNumFloat([pro.chosenNum,1]) : pro.chosenNum = 1;
			//计算原始数据列表中指定菜品  总数量
			this.inidataCount(pro.Id, pro.CyddMxType, 1);

			//统计出当前菜品   在已选菜品中的总数
			this.OrderTableProjectsdataCount(pro.Id, pro.CyddMxType, 1);
			//          	this.$set(pro,'chosenNum',pro.chosenNum);
			//          	this.$forceUpdate();//强制更新
			// 1 是菜品  2是套餐
			var data;
			pro.CyddMxType == 1 ? data = this.createNewDish(pro) : data = this.createNewSetMeal(pro);
			this.OrderTableProjectsdata.push(data);
		},
		//减少菜品
		minusSelect: function removeSelect(index) {
			var pro = this.ProjectAndDetailsData[index];
//			pro.chosenNum--;
//			pro.chosenNum = pro.chosenNum > 1 ? this.minusNumFloat([pro.chosenNum,1]) : 0;
			var id = pro.Id;
			var CyddMxType = pro.CyddMxType;
			var dValue = 0;
			//购物车  减
			for (var i = this.OrderTableProjectsdata.length - 1; i >= 0; i--) {
				if (this.OrderTableProjectsdata[i].R_Project_Id == id && this.OrderTableProjectsdata[i].CyddMxType == CyddMxType) {
					if (this.OrderTableProjectsdata[i].Num > 1) {
						//如果有多个
						this.OrderTableProjectsdata[i].Num = this.minusNumFloat([this.OrderTableProjectsdata[i].Num,1]);
						this.OrderTableProjectsdata[i].total = this.multiplyNumFloat([this.OrderTableProjectsdata[i].Price,this.OrderTableProjectsdata[i].Num]).toFixed(2);
						dValue = 1;
					} else {
						dValue = this.OrderTableProjectsdata[i].Num;
						this.OrderTableProjectsdata.splice(i, 1);
					}
					break;
				}
			}
			pro.chosenNum = pro.chosenNum > 1 ? this.minusNumFloat([pro.chosenNum,dValue]) : 0;
			
			//计算原始数据列表中指定菜品  总数量
			this.inidataCount(id, CyddMxType, - dValue);

			//统计出当前菜品   在已选菜品中的总数
			this.OrderTableProjectsdataCount(id, CyddMxType, - dValue);
		},
		//购物车		显示
		ShopCartShow: function ShopCartShow() {
			this.isShopCartShow = true;
		},
		//购物车		隐藏
		ShopCartHide: function ShopCartHide() {
			this.isShopCartShow = false;
		},
		//购物车		删除已选菜品
		deleteOrder: function deleteOrder(scope, rows) {
			rows.splice(scope.$index, 1);
		},
//		//购物车		菜品快捷加减数量 =>   加
//		plusNum: function plusNum(scope) {
//			//通用所有数据	指定菜品  总数量计算
//			this.currencyDataCount(scope.row.R_Project_Id, scope.row.CyddMxType, 1);
//
//			scope.row.Num++;
//			scope.row.total = (scope.row.Num * scope.row.totalPrice).toFixed(2);
//		},
//		//购物车		菜品快捷加减数量 =>   减
//		minusNum: function minusNum(scope) {
//			if (scope.row.Num > 1) {
//				//通用所有数据	指定菜品  总数量计算
//				this.currencyDataCount(scope.row.R_Project_Id, scope.row.CyddMxType, -1);
//
//				scope.row.Num--;
//				scope.row.total = (scope.row.Num * 100  * scope.row.totalPrice).toFixed(2);
//			}
//		},
		ShopCartOrderNumChange: function ShopCartOrderNumChange(scope){
			this.$nextTick(function () {
				console.log(scope.row.Num)
               	scope.row.Num = scope.row.Num ? this.numFloatFigure(scope.row.Num) : 1;
               	var dValue = this.minusNumFloat([scope.row.Num,scope.row.oldNum]);
               	scope.row.oldNum = scope.row.Num;
               	scope.row.total = this.multiplyNumFloat([scope.row.Num,scope.row.totalPrice]).toFixed(2);
               	this.currencyDataCount(scope.row.R_Project_Id, scope.row.CyddMxType, dValue);
            })
		},
		//购物车		菜品		删除
		deleteOrderTable: function deleteOrderTable(scope) {
			var _this = this;

			this.$confirm('是否确认删除菜品(' + scope.row.CyddMxName + ')', '提示', {
				confirmButtonText: '确定',
				cancelButtonText: '取消',
				type: 'warning',
				callback:function(action){
					if(action == 'confirm'){
						//通用所有数据	指定菜品  总数量计算
						_this.currencyDataCount(scope.row.R_Project_Id, scope.row.CyddMxType, -scope.row.Num);
		
						_this.OrderTableProjectsdata.splice(scope.$index, 1);
						_this.$message({ type: 'success', message: '删除成功!' });
					}
				}
			})
		},
		//购物车		全部删除
		allSelectDelete: function allSelectDelete() {
			var _this = this;
			
			if (this.OrderTableProjectsdata.length == 0) {
				this.messageIndex.close && this.messageIndex.close();
				this.messageIndex = this.$message({ type: 'warning', message: '请先选择菜品!' });
				return false;
			}

			this.$confirm('是否确认删除全部菜品', '提示', {
				confirmButtonText: '确定',
				cancelButtonText: '取消',
				type: 'warning',
				callback:function(action){
					if(action == 'confirm'){
						//原始数据
						var data = _this.inidata.ProjectAndDetails;
						for (var i = 0; i < data.length; i++) {
							data[i].chosenNum = 0;
						}

						//当前可选菜品
						var currentData = _this.ProjectAndDetailsData;
						for (var _i3 = 0; _i3 < currentData.length; _i3++) {
							currentData[_i3].chosenNum = 0;
						}

						_this.OrderTableProjectsdata = [];

						_this.$message({ type: 'success', message: '删除成功!' });
					}
				}
				
			})
		},
		//购物车		即起   叫起切换
		DishesStatusChange: function DishesStatusChange(scope, type) {
			scope.row.DishesStatus = type;
		},
		//购物车
		/*
	  	* 1  全单即起
	   	* 2  全单叫起
	   	*/
		DishesStatus: function DishesStatus(type) {
			var _this = this;

			if (this.OrderTableProjectsdata.length == 0) {
				this.messageIndex.close && this.messageIndex.close();
				this.messageIndex = this.$message({ type: 'warning', message: '请先选择菜品!' });
				return false;
			}
			var title = type == 1 ? '即起' : '叫起';
			this.$confirm('是否确认操作全单' + title, '提示', {
				confirmButtonText: '确定',
				cancelButtonText: '取消',
				type: 'warning',
				callback:function(action){
					if(action == 'confirm'){
								var data = _this.OrderTableProjectsdata;
						for (var i = 0; i < data.length; i++) {
							data[i].DishesStatus = parseFloat(type);
						}

						_this.$message({ type: 'success', message: '全单' + title + '完成!' });
					}
				}
			})
		},
		//购物车		菜品		修改  按钮点击
		reviseOrder: function reviseOrder(scope) {
			var data = this.inidata.ProjectAndDetails;
			var rowData = scope.row;
			var id = rowData.R_Project_Id;
			//获取菜品下标
			for (var i = 0; i < data.length; i++) {
				if (data[i].Id == id) {
					this.reviselayerDataIndex = i;
					break;
				}
			}
			//做法要求配菜  获取已选中索引
			var maxNum = !rowData.IsStock ? 9999 : this.addNumFloat([this.minusNumFloat([rowData.Stock,rowData.totalNum]),rowData.Num]);
			var copyData = {}; //复制原始菜品数据
			$.extend(true, copyData, rowData);
			copyData.index = scope.$index; //我的菜单中  菜品下标
			copyData.maxNum = maxNum; //最大可输入数量
			if (rowData.CyddMxType == 1) copyData.extendTotal = 0; //做法要求配菜	总价  归0
			this.reviselayerData = copyData;

			this.isReviseLayerShow = true;
		},
//		//修改弹窗	数量		减
//		reviselayerMinusNumber: function reviselayerMinusNumber() {
//			if(this.reviselayerData.Num == 1)return false;
//			this.reviselayerData.Num -= 1;
//			this.reviseChangeNum(this.reviselayerData.Num);
//		},
//		//修改弹窗	数量		加
//		reviselayerPlusNumber: function reviselayerPlusNumber() {
//			if(this.reviselayerData.Num == this.reviselayerData.maxNum)return false;
//			this.reviselayerData.Num += 1;
//			this.reviseChangeNum(this.reviselayerData.Num);
//		},
		//修改弹窗	数量		改变时
		reviselayerNumChange: function reviselayerNumChange(){
			console.log(this.reviselayerData.Num)
			var data = this.reviselayerData;
			this.$nextTick(function () {
				data.Num = data.Num ? this.numFloatFigure(data.Num) : 1;
               	this.reviseChangeNum(data.Num);
            })
		},
		//修改弹窗	数量		修改时
		reviseChangeNum: function reviseChangeNum(val) {
			var data = this.reviselayerData;
			var Unit = this.reviselayerData;
			data.total = this.multiplyNumFloat([data.totalPrice,val]).toFixed(2);
//			data.total = (Math.round(data.totalPrice * 100 * val * 100) / 10000).toFixed(2);
		},
		//修改弹窗	规格		切换
		reviseChangeUnit: function reviseChangeUnit(val) {
			var data = this.reviselayerData;
			var index = -1;

			for (var i = 0; i < data.specificationsList.length; i++) {
				if (data.specificationsList[i].Unit == val) {
					index = i;
					break;
				}
			}
			data.typeIndex = index;
			data.Price = (data.specificationsList[index].Price).toFixed(2);
			data.totalPrice = this.multiplyNumFloat([data.specificationsList[index].Price,data.extendTotal]).toFixed(2)
			data.total = this.multiplyNumFloat([data.totalPrice,data.Num]).toFixed(2);
		},
		//更多	做法  || 要求    显示弹窗
		reviseExtendChosenShow: function reviseExtendChosen(type){
			var ExtendData = [];
			var copyList = [];
			var ExtendList = [];
			this.reviseExtendChosenType = type;
			//重置分类按钮索引
			this.ReviseExtendclassifyIndex = 0;
			
			if(type == 1){	//做法
				//如果有默认做法
				if(this.reviselayerData.practiceList.length > 0){
					$.extend(true, copyList,this.reviselayerData.practiceList);
					ExtendData = [{Name:'推荐',Id:-1,ProjectExtendDTOList:[]}];//Id:-1 用于区分出推荐分类
					ExtendData[0].ProjectExtendDTOList = copyList;
				}
				$.extend(true, ExtendList,this.practiceExtendList);
				ExtendData = ExtendData.concat(ExtendList);
				//获取已选中
				for (var i = 0; i < this.reviselayerData.practice.length; i++) {
					var list = this.reviselayerData.practice[i];
					for (var j = 0; j < ExtendData.length; j++) {
						for (var k = 0; k < ExtendData[j].ProjectExtendDTOList.length; k++) {
							if(list.Id == ExtendData[j].ProjectExtendDTOList[k].Id && list.ExtendType == ExtendData[j].ProjectExtendDTOList[k].ExtendType){
								ExtendData[j].ProjectExtendDTOList[k].checked = true;
							}
						}
					}
				}
			}else if(type == 2){	//要求
				//如果有默认要求
				if(this.reviselayerData.requirementList.length > 0){
					$.extend(true, copyList,this.reviselayerData.requirementList);
					ExtendData = [{Name:'推荐',Id:-1,ProjectExtendDTOList:[]}];//Id:-1 用于区分出推荐分类
					ExtendData[0].ProjectExtendDTOList = copyList;
				}
				$.extend(true, ExtendList,this.requirementExtendList);
				ExtendData = ExtendData.concat(ExtendList);
				//获取已选中
				for (var i = 0; i < this.reviselayerData.requirement.length; i++) {
					var list = this.reviselayerData.requirement[i];
					for (var j = 0; j < ExtendData.length; j++) {
						for (var k = 0; k < ExtendData[j].ProjectExtendDTOList.length; k++) {
							if(list.Id == ExtendData[j].ProjectExtendDTOList[k].Id && list.ExtendType == ExtendData[j].ProjectExtendDTOList[k].ExtendType){
								ExtendData[j].ProjectExtendDTOList[k].checked = true;
							}
						}
					}
				}
			}
			
			this.ReviseExtendData = ExtendData;
			
			this.isReviseExtendShow = true;
		},
		//更多	做法  || 要求    分类选择
		reviseExtendChosenClassify: function reviseExtendChosenClassify(index){
			this.ReviseExtendclassifyIndex = index;
		},
		//更多	做法  || 要求    选择		
		reviseExtendChosen: function reviseExtendChosen(index){
			var item = this.ReviseExtendData[this.ReviseExtendclassifyIndex].ProjectExtendDTOList[index]
			var checked = item.checked ? false : true;
			
			this.$set(this.ReviseExtendData[this.ReviseExtendclassifyIndex].ProjectExtendDTOList[index],'checked',checked);
			//更多分类下   做法 || 要求     选中
			for(var i = 0;i < this.ReviseExtendData.length;i++){
				if( i == this.ReviseExtendclassifyIndex)continue
				
				for(var j = 0; j < this.ReviseExtendData[i].ProjectExtendDTOList.length; j++){
					var list = this.ReviseExtendData[i].ProjectExtendDTOList[j];
					if(list.Id == item.Id && list.ExtendType == item.ExtendType ){
						list.checked = checked;
						break;
					}
				}
			}
		},
		//更多	做法  || 要求    提交
		reviseExtendSubmit: function reviseExtendSubmit(){
			var newsArr = [];
			for(var i = 0;i < this.ReviseExtendData.length;i++){
				if(this.ReviseExtendData[i].Id == -1)continue;
				for(var j = 0; j < this.ReviseExtendData[i].ProjectExtendDTOList.length; j++){
					var list = this.ReviseExtendData[i].ProjectExtendDTOList[j];
					if(list.checked){
						newsArr.push(list)
					}
				}
			}
			//做法
			if(this.reviseExtendChosenType == 1){
				this.reviselayerData.practice = newsArr;
			//要求
			}else if(this.reviseExtendChosenType == 2){
				this.reviselayerData.requirement = newsArr;
			}
			this.reviseChangeExtend();
			
			this.isReviseExtendShow = false;
		},
		//修改弹窗	做法	 要求  配菜	切换时计算
		reviseChangeExtend: function reviseChangeExtend() {
			var data = this.reviselayerData;
			var extendTotal = 0; //做法要求配菜	总价
			var Extend = []; //做法要求配菜	数组
			//做法
			for (var i = 0; i < data.practice.length; i++) {
				extendTotal += data.practice[i].Price;
				Extend.push(data.practice[i]);
			}
			//要求
			for (var i = 0; i < data.requirement.length; i++) {
				extendTotal += data.requirement[i].Price;
				Extend.push(data.requirement[i]);
			}
			
			//配菜
			for (var i = 0; i < data.garnish.length; i++) {
				extendTotal += data.garnishList[data.garnish[i]].Price;
				Extend.push(data.garnishList[data.garnish[i]]);
			}
			data.extendTotal = extendTotal;
			data.Extend = Extend;
			data.totalPrice = this.addNumFloat([data.specificationsList[data.typeIndex].Price,data.extendTotal]).toFixed(2);
			data.total = this.multiplyNumFloat([data.totalPrice,data.Num]).toFixed(2);
		},
		//菜品  修改   以及    大图点选   弹窗    => 提交
		reviselayerSubmit: function reviselayerSubmit() {
			var data = this.reviselayerData;
			if (data.index >= 0) {
				//修改
				var row = this.OrderTableProjectsdata[data.index];
				var id = data.R_Project_Id;
				var CyddMxType = data.CyddMxType;
				var Dvalue = data.Num - this.OrderTableProjectsdata[data.index].Num; //数量差值
				if (CyddMxType == 1) {
					this.$set(this.OrderTableProjectsdata, data.index, data);
					row.ProjectDetailList = [data.specificationsList[data.typeIndex]]; //规格修改
				} else {
					row.Num = data.Num;
					row.total = data.total;
				}
			} else {
				//大图点菜
				var Dvalue = data.Num;
				
				this.BigPhotoData[data.bigPhotoIndex].chosenNum += Dvalue;
				
				this.OrderTableProjectsdata.push(data);
			}
			//通用所有数据	指定菜品  总数量计算
			this.currencyDataCount(data.R_Project_Id, data.CyddMxType, Dvalue);
			this.isReviseLayerShow = false;
			this.$message({ type: 'success', message: data.index >= 0 ? '修改完成!' : '菜品点选成功'});
		},
		//下单
		buying: function buying() {
			if (this.OrderTableProjectsdata.length == 0) {
				this.messageIndex = this.$message({ type: 'warning', message: '请选择菜品！' });
				return false;
			}
			this.loginDialogShow = true;
		},
		//通用所有数据	指定菜品  总数量计算
		currencyDataCount: function currencyDataCount(id, CyddMxType, val) {
			//计算当前菜品列表中指定菜品  总数量
			this.currentDataCount(id, CyddMxType, val);

			//计算原始数据列表中指定菜品  总数量
			this.inidataCount(id, CyddMxType, val);

			//统计出当前菜品   在已选菜品中的总数
			this.OrderTableProjectsdataCount(id, CyddMxType, val);
		},
		//计算当前菜品列表中指定菜品  总数量
		currentDataCount: function currentDataCount(id, CyddMxType, val) {
			var data = this.ProjectAndDetailsData;
			for (var i = 0; i < data.length; i++) {
				if (data[i].Id == id && data[i].CyddMxType == CyddMxType) {
					data[i].chosenNum ? data[i].chosenNum = this.addNumFloat([data[i].chosenNum,val])  : data[i].chosenNum = val;
//					data[i].chosenNum ? data[i].chosenNum = (data[i].chosenNum * 100 + val * 100) / 100  : data[i].chosenNum = val;
					break;
				}
			}
		},
		//计算原始数据列表中指定菜品  总数量
		inidataCount: function inidataCount(id, CyddMxType, val) {
			var data = this.inidata.ProjectAndDetails;
			for (var i = 0; i < data.length; i++) {
				if (data[i].Id == id && data[i].CyddMxType == CyddMxType) {
					data[i].chosenNum ? data[i].chosenNum = this.addNumFloat([data[i].chosenNum,val])  : data[i].chosenNum = val;
//					data[i].chosenNum ? data[i].chosenNum += (data[i].chosenNum * 100 + val * 100) / 100 : data[i].chosenNum = val;
					break;
				}
			}
			return i;
		},
		//计算已选菜品   指定菜品总计数量
		OrderTableProjectsdataCount: function OrderTableProjectsdataCount(id, CyddMxType, val) {
			var data = this.OrderTableProjectsdata;
			//统计出当前菜品   在已选菜品中的总数
			for (var i = 0; i < data.length; i++) {
				if (data[i].R_Project_Id == id && data[i].CyddMxType == CyddMxType) {
					data[i].totalNum = this.addNumFloat([data[i].totalNum,val])
//					data[i].totalNum += (data[i].totalNum * 100 + val * 100) / 100
					
				}
			}
		},
		//创建新的菜品对象
		createNewDish: function createNewDish(pro) {
			var detail = pro.ProjectDetailList[0];
			var data = {
				CyddTh: null,
				typeIndex: 0, //规格下标
				IsStock: pro.IsStock, //是否启用库存
				Stock: pro.Stock, //库存数量
				//						CyddTh: OrderTableIds,
				CyddMxType: pro.CyddMxType,
				CyddMxId: detail.Id,
				Price: detail.Price.toFixed(2),
				totalPrice: detail.Price.toFixed(2), //单价（做法要求配菜）
				Num: 1,
				oldNum:1,
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
				total: detail.Price.toFixed(2),
				totalNum: pro.chosenNum,
				type: detail.Unit, //规格	
				specificationsList: pro.ProjectDetailList, //规格数组
				practice: [], //已选做法
				practiceList: pro.practiceList, //做法数组
				requirement: [], //已选要求
				requirementList: pro.requirementList, //要求数组
				garnish: [], //已选配菜
				garnishList: pro.garnishList, //配菜数组
				extendTotal: 0, //做法要求配菜	总价
				Extend: [] //做法要求配菜归总
			};

			return data;
		},
		//创建新的套餐对象
		createNewSetMeal: function createNewSetMeal(pro) {
			var data = {
				IsStock: pro.IsStock, //是否启用库存
				Stock: pro.Stock, //库存数量
				CyddTh: null,
				CyddMxId: pro.Id,
				CyddMxType: pro.CyddMxType,
				Price: pro.Price.toFixed(2),
				totalPrice: pro.Price.toFixed(2),
				Num: 1,
				oldNum:1,
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
				total: pro.Price.toFixed(2),
				totalNum: pro.chosenNum
			};
			return data;
		},
		//登录
		loginSubmit: function loginSubmit() {
			var _this = this;
			this.messageIndex.close && this.messageIndex.close();
			if (this.user.Account == '') {
				this.messageIndex = this.$message({ type: 'warning', message: '请输入用户名！' });
				return false;
			}

			var req = {};
			$.ajax({
				type: "POST",
				url: "/Res/Account/FlatLoginIn",
				dataType: "json",
				data: _this.user,
				beforeSend:function(){
					$('#loading').show();
				},
				success: function success(data) {
					if (data.Successed) {
						_this.loginDialogShow = false;
						_this.$nextTick(function () {
							_this.user = { Account: '', PassWord: '' }; //清空用户登录信息
							_this.userInfo = data.Data;
							_this.changeTableInit();
						});
						
					} else {
						$('#loading').hide();
						_this.$nextTick(function () {
							_this.messageIndex = _this.$message({ type: 'warning', message: data.Message });
						});
						
					}
				}
			});
		},
		//用户登录信息清空
		userInfoEmpty: function userInfoEmpty(done) {
			this.user = { Account: '', PassWord: '' }; //清空用户登录信息
			this.chosenRestaurantOrderAgain();
			if (done) done();
		},
		//取消登录
		loginCancel: function loginCancel() {
			this.userInfoEmpty();
			this.loginDialogShow = false;
		},
		//桌台加载
		changeTableInit: function changeTableInit() {
			var _this = this;
			$.ajax({
				type: "POST",
				url: "/Res/Flat/InitAllChangeTableInfo",
				dataType: "json",
				data: { restaurantId: _this.restaurantId, user: _this.userInfo },
				complete: function complete() {
					$('#loading').hide();
				},
				success: function success(data) {
					data.AreasIndex = 0; //当前区域下标
					for (var i = 0; i < data.Tables.length; i++) {
						data.Tables[i].index = i;
					}
					_this.chosenTableData = data;
					_this.chosenRestaurantOrderShow = true;

					_this.chosenChangeArea(0);
				}
			});
		},
		//桌台弹窗	区域切换
		chosenChangeArea: function chosenChangeArea(index) {
			this.chosenTableData.AreasIndex = index;
			var AreaId = this.chosenTableData.Areas[index].Id;
			var newArr = [];
			for (var i = 0; i < this.chosenTableData.Tables.length; i++) {
				if (this.chosenTableData.Tables[i].AreaId == AreaId) {
					newArr.push(this.chosenTableData.Tables[i]);
				}
			}
			this.chosenTableDataNow = newArr;
		},
		//桌台选择
		chosenOrder: function chosenOrder(index) {
			var _this = this;
			var tableData = this.chosenTableData.Tables[index];
			if (tableData.CurrentOrderList.length == 1 && tableData.CurrentOrderList[0].IsLock) {
				this.messageIndex.close && this.messageIndex.close();
				this.messageIndex = this.$message({ type: 'warning', message: '该桌台已被锁定！' });
				return false;
			}

			if (tableData.CythStatus == 3) {
				this.messageIndex.close && this.messageIndex.close();
				this.messageIndex = this.$message({ type: 'warning', message: '不能选择脏台' });
				return false;
			}
			//多订单
			if(_this.chosenTableData.Tables[index].CurrentOrderList.length > 1){
				_this.currentOrderIndex = index;
				_this.currentOrderData = _this.chosenTableData.Tables[index].CurrentOrderList;
				_this.currentOrderListShow = true;
			}else {
				//新订单				
				if (_this.chosenTableData.Tables[index].CurrentOrderList.length == 0) {
					_this.chosenRestaurantOrderStep = 2;
					var data = _this.chosenTableData; //桌台选择源数据
					//默认值加载
					_this.openTableForm.R_Market_Id = _this.MarketList.Id; //分市
					_this.openTableForm.CyddOrderSource = data.CustomerSources[0].Id; //客源类型ID
					_this.openTableForm.CustomerId = data.CustomerList[0].Id; //客户ID
					_this.openTableForm.OrderType = data.OrderTypes[0].Id; //订单类型ID
					_this.openTableForm.orderTableIds = [_this.chosenTableData.Tables[index].Id]; //餐桌ID
				}else {
					var confirmInfo = _this.chosenTableData.Tables[index].CurrentOrderList[0].IsControl ? '该桌台（ ' + tableData.Name + ' ）正在被操作,是否确认选择该订单?' : '是否确认选择桌台(' + tableData.Name + ')?';
					_this.$confirm(confirmInfo, '提示', {
						confirmButtonText: '确定',
						cancelButtonText: '取消',
						callback:function(action){
							if(action == 'confirm'){
								_this.alreadyOpenTableSubmit(index,0)
							}
						}
					})
				}
			}
		},
		//一台多单  确认
		alreadyOpenTableListChosen: function alreadyOpenTableListChosen(i){
			var _this = this;
			//是否锁定
			if(this.chosenTableData.Tables[this.currentOrderIndex].CurrentOrderList[i].IsLock){
				this.messageIndex.close && this.messageIndex.close();
				this.messageIndex = this.$message({ type: 'warning', message: '该桌台已被锁定！' });
				return false;
			}
			
			
			var confirmInfo = this.chosenTableData.Tables[this.currentOrderIndex].CurrentOrderList[i].IsControl ? 
			'该桌台(' + this.chosenTableData.Tables[this.currentOrderIndex].Name + ')中的订单(' + this.chosenTableData.Tables[this.currentOrderIndex].CurrentOrderList[i].OrderNo + ')正在被操作,是否确认选择该订单?' : 
			'是否选择桌台(' + this.chosenTableData.Tables[this.currentOrderIndex].Name + ')中的订单(' + this.chosenTableData.Tables[this.currentOrderIndex].CurrentOrderList[i].OrderNo + ') ?';
			this.$confirm(confirmInfo, '提示', {
				confirmButtonText: '确定',
				cancelButtonText: '取消',
				callback:function(action){
					if(action == 'confirm'){
						_this.alreadyOpenTableSubmit(_this.currentOrderIndex,i)
					}
				}
			})
		},
		//已存在
		alreadyOpenTableSubmit: function alreadyOpenTableSubmit(index,i){
			var _this = this;
			var para = {};
			para.user = _this.userInfo; //用户信息
			para.req = _this.OrderTableProjectsdata;
			para.orderTableIds = [_this.chosenTableData.Tables[index].CurrentOrderList[i].Id];
			para.status = 2;
			$.ajax({
				type: "POST",
				url: "/Res/Flat/SubmitOrderAdd",
				dataType: "json",
				data: para,
				beforeSend: function beforeSend() {
					$('#loading').show();
				},
				success: function success(data) {
					if (data.Successed) {
						_this.messageIndex = _this.$message({ customClass:'isOverLoading',type: 'success', message: '点餐成功,正在刷新页面...' });
						setTimeout(function () {
							location.reload();
						}, 2000);
					} else {
						_this.messageIndex = _this.$message({ type: 'warning', message: data.Message });
						$('#loading').hide();
					}
				}
			});
		},
		//新订单提交
		newOpenTableSubmit: function newOpenTableSubmit(formName) {
			var _this = this;
			this.$refs[formName].validate(function (valid) {
				if (valid) {
					$('#loading').show();
					var para = {};
					para.req = _this.openTableForm; //开台信息
					para.req.R_Restaurant_Id = _this.restaurantId; //所属餐厅ID
					para.user = _this.userInfo; //用户信息
					para.list = _this.OrderTableProjectsdata;
					para.tableIds = _this.openTableForm.orderTableIds;
					para.status = 2;
					$.ajax({
						type: "POST",
						url: "/Res/Flat/SubmitOrder",
						dataType: "json",
						data: para,
						success: function success(data) {
							if (data.Successed) {
								_this.messageIndex = _this.$message({ type: 'success', message: '点餐成功,正在刷新页面...' });
								setTimeout(function () {
									location.reload();
								}, 2000);
							} else {
								_this.messageIndex = _this.$message({ type: 'warning', message: data.Message });
								$('#loading').hide();
							}
						}
					});
				} else {
					return false;
				}
			});
		},
		//重新选台
		chosenRestaurantOrderAgain: function chosenRestaurantOrderAgain() {
			this.chosenRestaurantOrderStep = 1;
			//重置开台信息
			this.openTableForm = { PersonNum: '', R_Market_Id: 0, CyddOrderSource: 0, ContactPerson: '', CustomerId: 0, ContactTel: '', OrderType: 0, Remark: '' };
		},
		//取消选台
		chosenRestaurantOrderCancel: function chosenRestaurantOrderCancel() {
			this.user = { Account: '', PassWord: '' }; //清空用户登录信息
			this.chosenRestaurantOrderAgain();
			this.chosenRestaurantOrderShow = false;
		},
		//图片懒加载
		imgLazy: function imgLazy() {
			if (this.ProjectAndDetailsType == 1) {
				// DOM 更新了
				this.$nextTick(function () {
					$("img.lazy").lazyload({ effect: "fadeIn", skip_invisible: false, container: $(".el-main") });
				});
			}
		},
		//员工模式切换
		employeeModel: function employeeModel(){
			window.location.href = '/'
		},
		//刷新  重置页面
		reload: function reload() {
			location.reload();
		}
	}
});

var backButtonPress = 0;
var backButtonIndex = 0;
//opu.show();
//监控安卓物理返回键
function goBack(){
	if(vm.messageIndex)vm.messageIndex.close && vm.messageIndex.close();
	//选台界面
	if(vm.chosenRestaurantOrderShow){
		vm.chosenRestaurantOrderCancel();
		return true;
	}
	//用户登录弹窗是否开启
	if(vm.loginDialogShow){	
		vm.loginCancel();
		return true;
	}
	//选择桌台弹窗是否开启
	if(vm.chosenRestaurantOrderShow){
		vm.chosenRestaurantOrderShow = false;
		return true;
	}
	//点选 || 修改  菜品信息弹窗
	if(vm.isReviseLayerShow){
		vm.isReviseLayerShow = false;
		return true;
	}
	//显示菜单
	if(vm.isShopCartShow){
		vm.isShopCartShow = false;
		return true;
	}
	//大图模式
	if(vm.isBigPhotoShow){
		vm.bigPhotoHide();
		return true;
	}
	backButtonPress++;
	clearTimeout(backButtonIndex);
	backButtonIndex = setTimeout(function(){
		backButtonPress = 0;
	},1000)
	if(backButtonPress > 1){
		$('#loading').show();
		return false;
	}else{
		vm.messageIndex = vm.$message({ type: 'warning', message: '再点一次退出点餐界面' });
		return true;
	}
}

