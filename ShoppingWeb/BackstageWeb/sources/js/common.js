const WEBURL = "https://localhost:3001";

/*!
 * master.js
 * ������ĸ��ҳ
 */

var layer;
layui.use(['layer', 'form'], function () {
    layer = layui.layer

});

/**
 * �첽����ȷ�Ͽ�
 * url �� url
 * data: json
 * active : �ɹ���Ĵ���
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
 * ɾ��ȷ�Ͽ�
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
        },"DELETE");
    });
};



/**
 * Ĭ��POST ajax ����ʽ
 * @param {any} options
 * data: ����������
 * url: ����·��
 * type: PSOT,GET,DELETE,PUT
 */
var ajax_request = function (options,type="post") {

    $.ajax({
        method: type,
        async: false,
        dataType: "json",
        contentType: "application/json;charset=UTF-8",//ָ����Ϣ��������        
        data: JSON.stringify(options.data),
        //headers: { "Authorization": "Bearer " + $("#jwt").val().trim() },
        url: options.url,

        success: function (data, textStatus) {
            options.callback(data);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(textStatus, "ajax����ʧ��");
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


