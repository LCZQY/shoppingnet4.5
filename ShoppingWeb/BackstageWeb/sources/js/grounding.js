
var uploadfile = function () {
    layui.use(['upload', 'jquery'], function () {
        var upload = layui.upload;
        var $ = layui.$;
        //ִ��ʵ��
        var uploadInst = upload.render({
            elem: '#files'                //��Ԫ��
            , url: 'groundinghandler.ashx?action=upload'      //�ϴ��ӿ�

            //*********************��������
            , size: 100                   //�����С100k
            , exts: 'jpg|png|gif|'        //�ɴ����ļ��ĺ�׺
            , accept: 'file'              //video audio images

            //****************��������������
            , data: { Parm1: "hello", Parm2: "world" }    //���⴫��Ĳ���
            , headers: { token: 'sasasasa' }                   //������ӵ�����ͷ
            , auto: true                                 //�Զ��ϴ�,Ĭ���Ǵ򿪵�
            , bindAction: '#btnUpload'                    //autoΪfalseʱ����������ϴ�
            , multiple: false                             //���ļ��ϴ�
            //, number: 100                               //multiple:trueʱ��Ч
            , done: function (res) {                      //������ɵĻص�
                $('#myPic').attr("src", "../.." + res.src);
                //��ֵ�����
                $("#Photo").val("../.." + res.src);
            }
            , error: function () {                         //����ʧ�ܵĻص�
                //�����쳣�ص�
            }
        });
    });
}

//�ϴ�ͼƬ
uploadfile();

layui.use(['form', 'layedit', 'laydate'], function () {

    var form = layui.form
        , layer = layui.layer
        , layedit = layui.layedit
        , laydate = layui.laydate;
    //���ڿؼ�
    laydate.render({
        elem: '#test11' //ָ��Ԫ��
    });
    //����һ���༭��
    var editIndex = layedit.build('LAY_demo_editor');

    //�����ύ
    form.on('submit(demo1)', function (data) {
        console.log(data.field, "11111111111111111����ϴ�����");
        //layer.alert(JSON.stringify(data.field), {
        //    title: '���յ��ύ��Ϣ'
        //})
        ajax_request({
            url: "groundinghandler.ashx?action=add",
            data: data.field,
            callback: function (e) {
                e = JSON.parse(e);
                if (e.code == 0) {
                    layer.msg(e.msg);
                    setTimeout(function () {
                        window.parent.location.reload();
                    }, 2000);

                } else {
                    layer.msg(e.msg);
                }
            }
        });
        return false;
    });
});


//��Ʒ����
ajax_request({
    url: "typehandler.ashx?action=tree",
    callback: function (data) {

        json = JSON.parse(data);
        console.log(json, "0000");

        var html = ' <select name="CateId">';
        $.each(json, function (index, item) {
            if (index == 0) html += ' <option value="">��ѡ����Ʒ����</option>';
            console.log(item, "--------------");
            html += '<optgroup label="' + item.title + '">';
            if (item.children != null)
                $.each(item.children, function (i, childname) {
                    html += '<option value="' + childname.id + '">' + childname.title + '</option>';
                });
            html += ' </optgroup>';
        });
        html += '</select>';
        console.log(html, "����");
        $("#appendsSelect").html(html);

    }
});

