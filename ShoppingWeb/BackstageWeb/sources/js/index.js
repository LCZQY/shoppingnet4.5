
layui.use(['form', 'layer'], function () {
    var form = layui.form
        , layer = layui.layer;

    //�����ύ
    form.on('submit(login)', function (data) {
        loading = layer.load(2, {
            shade: [0.1, '#000'] //0.2͸���ȵİ�ɫ����
        });
        ajax_request({
            url: WEBURL + "/api/customers/login",
            data: data.field,
            callback(e) {
                layer.close(loading);
                console.log(e, "-----------------------");
                if (e.code == 0) {
                    if (e.extension == true) {
                        ////JSON����תJSON�ַ���
                        var obj = { "user": data.field.UserName };
                        obj = JSON.stringify(obj); //ת��ΪJSON�ַ���
                        localStorage.setItem("login", obj);
                        window.location.href = 'page/home.html';
                    } else {
                        alert("fail");
                        //layer.msg("��������˺Ŵ���");
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



//��ȡToken
ajax_request({
    url: "",
    data: {
        grant_type: "client_credentials",
        client_Id: "clientId",
        client_secret: "clientsecret"
    },
    callback: function (e) {
        console.log(e, "�Ƿ�ɹ�");
    }
}, "POST");


