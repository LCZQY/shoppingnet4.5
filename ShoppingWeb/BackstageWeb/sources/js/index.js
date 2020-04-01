layui.extend({
    admin: 'sources/js/shopping/admin'
});
layui.use(['form', 'admin'], function () {
    var form = layui.form
        , admin = layui.admin
        , layer = layui.layer
        , $ = layui.jquery;
    //监听提交
    form.on('submit(login)', function (data) {
        loading = layer.load(2, {
            shade: [0.1, '#000'] //0.2透明度的白色背景
        });
        ajax_request({
            url: "adminhandler.ashx?action=isexist",
            data: data.field,
            callback(e) {
                layer.close(loading);
                e = JSON.parse(e);
                console.log(e, "-----------------------");
                if (e.code == 0) {
                    //JSON对象转JSON字符串
                    var obj = { "user": data.field.UserName, "r": e.model };
                    obj = JSON.stringify(obj); //转化为JSON字符串
                    localStorage.setItem("login", obj);
                    window.location.href = 'ManageIndex.aspx';
                } else {
                    layer.msg(e.msg);
                }
            }
        });
        return false;
    });
});