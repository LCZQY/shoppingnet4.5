layui.extend({
    admin: 'sources/js/shopping/admin'
});
layui.use(['form', 'admin'], function () {
    var form = layui.form
        , admin = layui.admin
        , layer = layui.layer
        , $ = layui.jquery;
    //�����ύ
    form.on('submit(login)', function (data) {
        loading = layer.load(2, {
            shade: [0.1, '#000'] //0.2͸���ȵİ�ɫ����
        });
        ajax_request({
            url: "adminhandler.ashx?action=isexist",
            data: data.field,
            callback(e) {
                layer.close(loading);
                e = JSON.parse(e);
                console.log(e, "-----------------------");
                if (e.code == 0) {
                    //JSON����תJSON�ַ���
                    var obj = { "user": data.field.UserName, "r": e.model };
                    obj = JSON.stringify(obj); //ת��ΪJSON�ַ���
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