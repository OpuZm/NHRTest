"use strict";
var vm = new Vue({
	el: '#app',
	data: function data() {
		return {
			fullscreenLoading: false,
			messageIndex: -1, //提示	下标
			RestaurantsData: [ //餐厅数据
				{
					IsDefaultValue: 0 //默认选中的餐厅分市
				}
			]
		};
	},
	computed: {},
	mounted: function mounted() {
		this.Initialization();
	},
	methods: {
		//初始化
		Initialization: function Initialization() {
			var _this = this;
			$.ajax({
				type: "POST",
				url: "/Res/Flat/GetRestaurants",
				dataType: "json",
				success: function success(data) {
					if(data.Successed) {
						var Data = data.Data;
						for(var i = 0; i < Data.length; i++) {
							var list = Data[i].MarketList;
							Data[i].IsDefaultValue = '';
							Data[i].IsDefaultNum = -1;

							for(var j = 0; j < list.length; j++) {
								if(list[j].IsDefault) {
									Data[i].IsDefaultNum = j;
									Data[i].IsDefaultValue = list[j].Name;
									break;
								}
							}
						}
						_this.RestaurantsData = data.Data;
						console.log(_this.RestaurantsData);
					} else {
						if(data.Message) _this.$alert(data.Message, '提示', {
							confirmButtonText: '确定'
						});
					}
					_this.$nextTick(function () {
						$('#loading').hide();
					});
				}
			});
		},

		//餐厅分市切换
		RestaurantsItemListChange: function RestaurantsItemListChange(value, index) {
			var Num = 0;
			for (var i = 0; i < this.RestaurantsData.length; i++) {
				if(this.RestaurantsData[i].Name == value)break;
				Num = i;
			}
			this.RestaurantsData[index].IsDefaultNum = Num + 1;
		},

		//跳转页面
		chosenRestaurants: function chosenRestaurants(index) {
			var data = this.RestaurantsData[index];

			if(data.IsDefaultNum < 0) {
				this.messageIndex.close && this.messageIndex.close();
				this.messageIndex = this.$message({
					type: 'warning',
					message: '请先选择分市!'
				});
				return false;
			}
			$('#loading').show();
			
			sessionStorage.setItem('RestaurantsData', JSON.stringify({
				Id: this.RestaurantsData[index].Id,
				MarketList: data.MarketList[data.IsDefaultNum]
			}));
			this.$nextTick(function () {
				window.location.href = "/Res/Flat/ChoseProject";
			});
		}
	}
});

var backButtonPress = 0;
var backButtonIndex = 0;
//opu.show();
//监控安卓物理返回键
function goBack() {
	backButtonPress++;
	clearTimeout(backButtonIndex);
	backButtonIndex = setTimeout(function(){
		backButtonPress = 0;
	},1000)
	if(backButtonPress > 1){
		opu.finish();
		return false;
	}else{
		vm.messageIndex = vm.$message({ type: 'warning', message: '再点一次退出系统' });
		return true;
	}
	return false;
}

opu.setStatusBarColor('#282b33')
opu.setNavigationBarColor('#282b33')

//goBack  监控物理返回键

//setStatusBarColor(final String color) 	设置顶部导航栏颜色 
//setNavigationBarColor(final String color) 设置底部导航栏颜色 

//hideStatusBar() 隐藏状态栏
//showStatusBar() 展示状态栏

//hideNavigationsBar  	隐藏底部按钮
//showNavigAtionBar		展示底部按钮