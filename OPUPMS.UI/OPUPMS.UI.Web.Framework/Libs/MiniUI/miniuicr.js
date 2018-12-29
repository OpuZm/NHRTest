var etrolcommon = etrolcommon || function () { }
etrolcommon.alertFun = window.alert;
window.alert = function (str) {
    try {
        if (str.indexOf("www.miniui.com") == -1) {
            etrolcommon.alertFun(str);
        }
    } catch (Exception) {
        etrolcommon.alertFun(str);
    }
};