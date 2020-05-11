

layui.define(['form', 'layer'], function (exports) {
    var $ = layui.jquery,
        layer = layui.layer,
        form = layui.form;

    var index = {
        //һ���ǵ�¼ʱ��������������ȡ��Token
        login: function (username, password) {

            $.get(AuthentictionURL + "/api/token/password?name=" + username + "&pwd=" + password + "", function (data) {
                 loading = layer.load(2, {
                    shade: [0.1, '#000'] //0.2͸���ȵİ�ɫ����
                });
                if (data.code === '0') {
                    layer.close(loading);
                    console.log(data,"token");
                    //��response�õ���Token���浽localStorage����
                    localStorage.setItem('token', "Bearer " + data.extension.access_token);
                //    var obj = { "user": data.field.UserName };
                ////                obj = JSON.stringify(obj); //ת��ΪJSON�ַ���
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
            //�����ύ
            form.on('submit(login)', function (data) {       
                index.login(data.field.name, data.field.password);             
                return false;
            });
        }
    }
    //����ӿ� 
    exports('index', index);
});


