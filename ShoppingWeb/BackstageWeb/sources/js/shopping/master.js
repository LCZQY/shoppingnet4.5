/*!
 * master.js
 *  
 * 主用于母版页
 */

var layer;
layui.use(['layer', 'form'], function () {
    layer = layui.layer

});

/**
 * PSOT ajax 请求方式
 * @param {any} options
 * data: 对象请求体
 * url: 请求路径
 */
var ajax_request = function (options) {

    $.ajax({
        method: "post",
        async: false,           
        dataType: "json",        
        contentType: "application/json;charset=UTF-8",//指定消息请求类型        
        data: JSON.stringify(options.data),
        //headers: { "Authorization": "Bearer " + $("#jwt").val().trim() },
        url: options.url,
   
        success: function (data, textStatus) {
            options.callback(data);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(textStatus,"ajax请求失败");
           // layer.msg("请求失败");
        }
    });
};


/**
 * 页面刷新
 * */
var page_reload = function () {

    setTimeout(function () {
        window.parent.location.reload();
    }, 2000);
};

/**
 * 数据加载中...
 * */
var loading_start = function () {
    layer.load(2, {
        shade: [0.1, '#000']
    });

};

/**
 * 关闭数据加载弹窗
 * */
var loading_end = function () {
    layer.close(loading);
};


