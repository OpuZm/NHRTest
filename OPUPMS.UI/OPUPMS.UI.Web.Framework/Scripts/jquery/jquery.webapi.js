(function ($) {
    var webApiMethods = {
        get: 'GET',
        post: 'POST',
        put: 'PUT',
        delete: 'DELETE'
    };

    var tokens = {
        default: 'OPUPMS.UI.Web.Framework-WebApi-Ver1-2013-08-20'
    };

    var defaultOptions = {
        contentType: "application/json; charset=utf-8",
        headers: { 'Token': tokens.default }
    };

    var webApiAjax = function (options, method) {
        $.extend(options, defaultOptions, { type: method });
        $.ajax(options);
    };

    $.extend({
        webApi: {
            get: function (options) {
                webApiAjax(options, webApiMethods.get);
            },
            post: function (options) {
                webApiAjax(options, webApiMethods.post);
            },
            put: function (options) {
                webApiAjax(options, webApiMethods.put);
            },
            delete: function (options) {
                webApiAjax(options, webApiMethods.delete);
            }
        }
    });
})(jQuery)