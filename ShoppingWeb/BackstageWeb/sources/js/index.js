

layui.define(['form', 'layer'], function (exports) {
    var $ = layui.jquery,
        layer = layui.layer,
        form = layui.form;

    var index = {
        //一般是登录时向服务器发请求获取到Token
        login: function (username, password) {

            $.get(AuthentictionURL + "/api/token/password?name=" + username + "&pwd=" + password + "", function (data) {
                 loading = layer.load(2, {
                    shade: [0.1, '#000'] //0.2透明度的白色背景
                });
                if (data.code === '0') {
                    layer.close(loading);
                    console.log(data,"token");
                    //将response得到的Token缓存到localStorage里面
                    localStorage.setItem('token', "Bearer " + data.extension.access_token);
                //    var obj = { "user": data.field.UserName };
                ////                obj = JSON.stringify(obj); //转化为JSON字符串
                ////                localStorage.setItem("login", obj);
                    window.location.href = 'page/home.html';
                } else {
                    layer.close(loading);
                    layer.msg(data.message);
                }
            });
            return false;
        },     
        submitLogin: function () {
            //监听提交
            form.on('submit(login)', function (data) {       
                index.login(data.field.name, data.field.password);             
                return false;
            });
        }
    }
    //输出接口 
    exports('index', index);
});


