function ChangeDateFormat(d) {

    if (d != null) {
        var date = new Date(parseInt(d.replace("/Date(", "").replace(")/", ""), 10));
        var month = padLeft(date.getMonth() + 1, 10);
        var currentDate = padLeft(date.getDate(), 10);
        var hour = padLeft(date.getHours(), 10);
        var minute = padLeft(date.getMinutes(), 10);
        return date.getFullYear() + "-" + month + "-" + currentDate + " " + hour + ":" + minute;
    }
}

function ChangeDateFormatYM(d) {
    if (d != null) {
        var date = new Date(parseInt(d.replace("/Date(", "").replace(")/", ""), 10));
        var month = padLeft(date.getMonth() + 1, 10);
        var currentDate = padLeft(date.getDate(), 10);
        var hour = padLeft(date.getHours(), 10);
        var minute = padLeft(date.getMinutes(), 10);
        return date.getFullYear() + "-" + month;
    }
}

function ChangeDateFormatYMD(d) {
    if (d != null) {
        var date = new Date(parseInt(d.replace("/Date(", "").replace(")/", ""), 10));
        var month = padLeft(date.getMonth() + 1, 10);
        var currentDate = padLeft(date.getDate(), 10);
        var hour = padLeft(date.getHours(), 10);
        var minute = padLeft(date.getMinutes(), 10);
        return date.getFullYear() + "-" + month + "-" + currentDate;
    }
}

function padLeft(str, min) {
    if (str >= min)
        return str;
    else
        return "0" + str;
}

function subString(d) {
    if (d != null) {
        if (d.length > 50) {
            return d.substr(0, 50) + "....";
        } else {
            return d;
        }
    }
}

function sumFormatter(data) {
    field = this.field;
    return data.reduce(function (sum, row) {
        return sum + (+row[field]);
    }, 0);
}

Date.prototype.format = function (format) {
    var date = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "h+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3),
        "S+": this.getMilliseconds()
    };
    if (/(y+)/i.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + '').substr(4 - RegExp.$1.length));
    }
    for (var k in date) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1
                   ? date[k] : ("00" + date[k]).substr(("" + date[k]).length));
        }
    }
    return format;
}