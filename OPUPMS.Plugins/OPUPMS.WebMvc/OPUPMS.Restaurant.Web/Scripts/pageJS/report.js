var reportorJs = {
    userInfo: {},
    getUserInfo: function () {
        if ($.isEmptyObject(this.userInfo)) {
            var user = {};
            $.ajax({
                url: "/Res/Account/GetUserInfo",
                type: "get",
                dataType: "json",
                async: false,
                success: function (data) {
                    console.log(data);
                    user = data.Data;
                }, error: function (msg) {
                    console.log(msg.responseText);
                }
            })
            this.userInfo = user;
        }
        return this.userInfo;
    },
    showPdb: function (id, dyqx,dcqx) {
        if (this.IsEnable()) {
            var user = this.getUserInfo();
            reportor.showPOBB(id, user.UserCode, user.UserName, '','','','',dyqx,dcqx);
        }
    },
    printPdb: function (id, zh, fzh, xhs, dlqx, dcqx, dlyx, htstar, xhs2, xhs3) {
        if (this.IsEnable()) {
            var user = this.getUserInfo();
            reportor.printPOBB(id, user.UserCode, user.UserName, user.LoginMarketId.toString(), user.DepartmentId.toString(), htstar, user.BusinessDate, dlqx, dcqx, zh, fzh, xhs, xhs2, xhs3, dlyx);
            //reportor.printPOBB(8801, user.UserCode, user.UserName, user.LoginMarketId.toString(), user.DepartmentId.toString(), '5', user.BusinessDate, 0, 0, 0, inidata.OrderAndTables.OrderId, '', 0);
            //reportor.printPOBB(8801, 'hmh', '黄懋航', 'A', 'OPU酒店', '5', '2013-01-01', 0, 0, false);
        }
    },
    printMenu: function (orderTableId) {
        if (this.IsEnable()) {
            reportor.printMenu(orderTableId);
        }
    },
    cardRead: function (str) {
        var res = "";
        if (this.IsCard()) {
            res = cardinfo.request();
        } 
        return res;
    },
    IsEnable: function () {
        if (typeof (reportor) != "undefined") {
            return true;
        } else {
            return false;
        }
    },
    IsCard: function () {
        if (typeof (cardinfo) != "undefined") {
            return true;
        } else {
            return false;
        }
    }
};