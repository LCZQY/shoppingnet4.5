
layui.use(['form', 'layer'], function () {
    var form = layui.form
        , layer = layui.layer;

    //监听提交
    form.on('submit(login)', function (data) {
        loading = layer.load(2, {
            shade: [0.1, '#000'] //0.2透明度的白色背景
        });
        ajax_request({
            url: WEBURL + "/api/customers/login",
            data: data.field,
            callback(e) {
                layer.close(loading);
                console.log(e, "-----------------------");
                if (e.code == 0) {
                    if (e.extension == true) {
                        ////JSON对象转JSON字符串
                        var obj = { "user": data.field.UserName };
                        obj = JSON.stringify(obj); //转化为JSON字符串
                        localStorage.setItem("login", obj);
                        window.location.href = 'page/home.html';
                    } else {
                        alert("fail");
                        //layer.msg("密码或者账号错误");
                    }
                } else {
                    alert("error");
                    //layer.msg(e.message);
                }
            }
        });
        return false;
    });
});



//获取Token
ajax_request({
    url: "",
    data: {
        grant_type: "client_credentials",
        client_Id: "clientId",
        client_secret: "clientsecret"
    },
    callback: function (e) {
        console.log(e, "是否成功");
    }
}, "POST");


