/*!
 * master.js
 *  
 * ������ĸ��ҳ
 */

var layer;
layui.use(['layer', 'form'], function () {
    layer = layui.layer

});

/**
 * PSOT ajax ����ʽ
 * @param {any} options
 * data: ����������
 * url: ����·��
 */
var ajax_request = function (options) {

    $.ajax({
        type: "post",
        async: false,           
        dataType: "json",        
        contentType: "application/json;charset=UTF-8",//ָ����Ϣ��������        
        data: options.data,
        //headers: { "Authorization": "Bearer " + $("#jwt").val().trim() },
        url: options.url,
       //crossDomain: true,     
        success: function (data, textStatus) {
            options.callback(data);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(textStatus,"ajax����ʧ��");
           // layer.msg("����ʧ��");
        }
    });
};


/**
 * ҳ��ˢ��
 * */
var page_reload = function () {

    setTimeout(function () {
        window.parent.location.reload();
    }, 2000);
};

/**
 * ���ݼ�����...
 * */
var loading_start = function () {
    layer.load(2, {
        shade: [0.1, '#000']
    });

};

/**
 * �ر����ݼ��ص���
 * */
var loading_end = function () {
    layer.close(loading);
};

