new Vue({
  	el: '#app',
 	data: function() {
    	return { 
    		fullscreenLoading:false,
    		messageIndex:-1,			//提示	下标
    		RestaurantsData:[			//餐厅数据
    			{
    				IsDefaultValue:0	//默认选中的餐厅分市
    			}
    		]			
    	}
 	},
 	computed : {
 		
	},
  	mounted(){
  		this.Initialization();
	},
	methods:{
		//初始化
        Initialization(){
        	var _this = this;
        	$.ajax({
	            type: "POST",
	            url: "/Res/Flat/GetRestaurants",
	            dataType: "json",
	            beforeSend: function (XMLHttpRequest) {
	                _this.fullscreenLoading = true;
	            },
	            complete: function (XMLHttpRequest, textStatus) {
	                _this.fullscreenLoading = false;
	            },
	            success: function(data){
	            	if(data.Successed){
	            		var Data = data.Data;
	            		for(let i=0;i<Data.length;i++){
	            			var list = Data[i].MarketList;
	            			Data[i].IsDefaultValue = '';
            				Data[i].IsDefaultNum = -1;
            				
            				for(let j=0;j<list.length;j++){
	            				if(list[j].IsDefault){
	            					Data[i].IsDefaultNum=j;
	            					Data[i].IsDefaultValue=list[j].Name;
	            					break;
	            				}
	            			}
	            		}
	            		_this.RestaurantsData = data.Data;
	            		console.log(_this.RestaurantsData)
	            	}else{
	            		if(data.Message)_this.$alert(data.Message, '提示', {confirmButtonText: '确定'});
	            	}
	            	$('#initLoading').remove();
                }
	        });
        },
        //餐厅分市切换
        RestaurantsItemListChange(value,index){
        	var Num = 0;
			this.RestaurantsData[index].MarketList.find((item,i)=>{
				if(item.Name === value)return item;
				Num = i;
      		});
      		this.RestaurantsData[index].IsDefaultNum = Num+1;
        },
        //跳转页面
        chosenRestaurants(index){
        	var data = this.RestaurantsData[index];
        	
        	if(data.IsDefaultNum < 0){
        		this.messageIndex.close && this.messageIndex.close()
				this.messageIndex = this.$message({type: 'warning',message: '请先选择分市!'});
				return false;
        	}
        	this.fullscreenLoading = true;
        	sessionStorage.setItem('RestaurantsData',JSON.stringify({Id:this.RestaurantsData[index].Id,MarketList:data.MarketList[data.IsDefaultNum]}));
        	window.location.href = "/Res/Flat/ChoseProject";
        }
    }
})