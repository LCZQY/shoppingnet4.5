/**业务中心域名 */
const WEBURL = "https://localhost:3001";
/**权限中心域名 */
const AuthentictionURL = "https://localhost:5000";

/*!
 * master.js
 * 主用于母版页
 */

//var layer;
//layui.use(['layer', 'form'], function () {
//    layer = layui.layer

//});

/**
 * 异步交互确认框
 * url ： url
 * data: json
 * active : 成功后的处理
 * @param {any} options
 */
var table_confirm = function (options) {
    layer.confirm(options.tips, function (index) {
        var loading = layer.load(2, {
            shade: false
        });
        ajax_request({
            url: options.url,
            data: options.data,
            callback: function (e) {
                options.active(e);
            }
        });
    });
};

/**
 * 删除确认框
 * @param {any} options
 */
var delte_confirm = function (options) {
    layer.confirm(options.tips, function (index) {
        var loading = layer.load(2, {
            shade: [0.1, '#000']
        });
        ajax_request({
            url: options.url,
            data: options.data,
            callback: function (e) {
                options.active(e);
            }
        }, "DELETE");
    });
};


/**
 * 获取Token
 */
var getToken = function () {

    if (!!localStorage.getItem('token')) {

        return localStorage.getItem('token');
    } else {
        //layer.msg("您还没有登录本系统,请先登录");
        window.location.href = 'index.html';
    }
}

//token 过期处理 
function handleTokenFailed(code) {
    if (code == 401) {
        localStorage.clear();
        new Toast().showMsg('登录信息已过期，请重新登录', 500)
        setTimeout(function () {
            location.href = 'index.html';
        }, 1000)

    }
}

/**
 * 默认POST ajax 请求方式
 * @param {any} options
 * data: 对象请求体
 * url: 请求路径
 * type: PSOT,GET,DELETE,PUT
 */
var ajax_request = function (options, type = "post") {

    $.ajax({
        method: type,
        async: false,
        dataType: "json",
        contentType: "application/json;charset=UTF-8",//指定消息请求类型        
        data: JSON.stringify(options.data),
        headers: { "Authorization": getToken() },
        url: options.url,

        success: function (data, textStatus) {
            options.callback(data);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(textStatus, "ajax请求失败");
            // layer.msg("请求失败");
        }
    });
};

