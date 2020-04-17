/**
        * һ������ɾ��������ʹ��
        */
var table_confirm = function (options) {
    layer.confirm(options.tips, function (index) {

        var loading = layer.load(2, {
            shade: [0.1, '#000']
        });
        ajax_request({
            url: options.url,
            data: options.data,
            callback: function (e) {
                layer.close(loading);
                e = JSON.parse(e);
                if (e.code === 0) {
                    layer.msg(e.msg);
                } else {
                    layer.msg(e.msg);
                }
            }
        });
    });
};
/**
 * �����ݱ��
 * */
var reload = function (typeid, typename) {
    layui.use(['table', 'layer', 'form'], function () {
        var table = layui.table;
        table.render({
            url: "typehandler.ashx?action=type"
            , method: "POST"
            , where: { CateId: typeid, Name: typename }
            , elem: '#test'
            , toolbar: '#toolbarDemo'
            , title: '�û����ݱ�'
            , location: true
            , cols: [
                [
                    { type: 'checkbox', fixed: 'left' }
                    , { field: 'Title', title: '��Ʒ����', width: 120, edit: 'text' }
                    , { field: 'CateId', title: '��Ʒ����', width: 120, sort: true, }
                    , { field: 'MarketPrice', title: '�г��۸�', width: 120, edit: 'text' }
                    , { field: 'Price', title: '��վ�۸�', width: 120, sort: true, edit: 'text' }
                    , {
                        field: 'PostTime', title: '�ϼ�ʱ��', width: 120, sort: true, templet: function (d) {
                            return layui.util.toDateString(d.DeliveryDate);
                        }, edit: ''
                    }
                    , { field: 'Stock', title: '�������', width: 120, edit: 'text', edit: 'text' }
                    , { field: 'Content', title: '��Ʒ����', width: 120, edit: 'text' }
                    //, { field: 'DetailStates', title: '��������״̬', width: 120, templet: '#table-DetailStates' }
                    , { fixed: 'right', title: '����', toolbar: '#takeaction', width: 140 }
                ]
            ]
            , page: true
        });
    });
};

//���νṹ
function loadtree() {
    $.ajax({
        type: "get",
        url: WEBURL +"/api/type/tree",
        dataType: 'json',
        success: function (res) {
            console.log(res, "����Դ");
            var data = [{}];
            layui.use(['tree', 'util'], function () {
                var tree = layui.tree,
                    layer = layui.layer,
                    util = layui.util,
                    data1 = res.extension
                tree.render({
                    elem: '#test9',
                    id: 'treedept',
                    data: data1,
                    showCheckbox: true,//��ʾ��ѡ��
                    showLine: true,//��ʾ��
                    accordion: true,//�ַ���ģʽ
                    drag: false,//��ק
                    skin: "laySimple", // laySimple������
                    showSearch: true,//��ʾ������
                    //edit: false,
                    edit: ['update'], //�����ڵ��ͼ��,
                    click: function (obj) {
                        //  $(".radio").eq(0).removeAttr("checked");
                        $("#parentId").val(obj.data.title);
                        $("#parentId").attr("guid", obj.data.id);
                        $("#type").val("�Ӽ�");
                        console.log(obj.data.title, "����鿴��Ʒ");
                        //���չʾ��
                        reload(obj.data.id, obj.data.title);
                        table_show();
                    },
                    //���β˵�����ɾ�Ĳ飬����
                    operate: function (obj) {
                        var type = obj.type; //�õ��������ͣ�add��edit��del
                        var data = obj.data; //�õ���ǰ�ڵ������
                        var elem = obj.elem; //�õ���ǰ�ڵ�Ԫ��
                        console.log(type, "��ǰѡ���е���..");
                        if (type === 'del') { //ɾ�������layui �����и�Bug ���� ֱ�ӵ��ɾ���Ͳ�����
                            console.log(0);
                            var arr = obj.data;
                            if (arr.id > 0 && (!typeof arr.id != 'undefined')) {
                                if (arr.children.length > 0) {
                                    layer.msg("����Ʒ�������������Ͳ�����ɾ��");
                                    return false;
                                } else {
                                    table_confirm({
                                        obj: obj,
                                        url: "typehandler.ashx?action=update",
                                        tips: "�Ƿ�ȷ��ɾ����",
                                        data: { id: arr.id }
                                    });
                                }
                            } else {
                                elem.remove();
                            }
                        } else if (type === 'update') {
                            console.log(data, "---------------")
                            table_confirm({
                                obj: obj,
                                url: "typehandler.ashx?action=update",
                                tips: "�Ƿ�ȷ���޸ģ�",
                                data: { CateName: data.title, id: data.id }
                            });
                            page_reload();
                        }
                    }
                });
            });
        }
    });
}
loadtree();

//������
var table_show = function () {

    layui.use(['table', 'layer', 'form'], function () {
        var table = layui.table;

        //ͷ�������¼�
        table.on('toolbar(test)', function (obj) {

            switch (obj.event) {

                case 'add':
                    layer.open({
                        title: '',
                        /*������ⲿ��html��type2���ڲ���type1*/
                        type: 1,
                        btnAlign: 'c',
                        area: '50%',
                        content: $("#add-main").html()
                        //δ������ȥ�������ύ������̨����ajax����
                    }
                    );
                    break;
                case
                    'batchDel'
                    :
                    layer.msg("������...");
                    /*����ajax���󵽺�ִ̨������ɾ��*/
                    break;
                case
                    'flush'
                    :
                    table.reload('test', {
                        url: "groundinghandler.ashx?action=list"
                        , method: "GET"
                    });
                    break;
                case
                    'search'
                    :
                    //   layer.msg("�����û�������");
                    var name = $('input[name="search"]').val();
                    table.reload('test', {
                        url: 'groundinghandler.ashx?action=search',
                        where: {
                            Title: name,
                        },
                        page: {
                            curr: 1
                        }
                    });
                    break;
            };
        });

        //������Ԫ��༭���޸�
        table.on('edit(test)', function (obj) {
            var value = obj.value //�õ��޸ĺ��ֵ
                , data = obj.data //�õ����������м�ֵ
                , field = obj.field; //�õ��ֶ�
            var loading = layer.load(2, {
                shade: [0.1, '#000']
            });

            console.log(data, "0000000000");
            ajax_request({
                url: "groundinghandler.ashx?action=update",
                data: data,
                callback: function (e) {
                    layer.close(loading);
                    e = JSON.parse(e);
                    if (e.code === 0) {
                        layer.msg(e.msg);
                    } else {
                        layer.msg(e.msg);
                    }
                }
            });
        });
        //�����й����¼�
        table.on('tool(test)', function (obj) {
            var data = obj.data;
            switch (obj.event) {
                case 'del'://ɾ��
                    table_confirm({
                        obj: obj,
                        url: "groundinghandler.ashx?action=delete",
                        tips: "�װ���,��Ҫɾ������",
                        data: { id: data.ProductId }
                    });
                    //      page_reload();
                    break;
            }
        });


    });
}

//�˵���������
//$("input[placeholder='������ؼ��ֽ��й���']").change(function () {
//    console.log(1);
//});

layui.use(['form', 'tree', 'util', 'layer'], function () {
    layer = layui.layer
        , form = layui.form

    /**�����Ʒ���� */
    var active = {
        offset: function (othis) {

            var type = othis.data('type')
            layer.open({
                type: 1
                , title: "�����Ʒ����"
                , area: '50%'
                , offset: type
                , id: 'layerDemo' + type //��ֹ�ظ�����
                , content: $('#from')
                // , btn: '�ر�ȫ��'
                , btnAlign: 'l' //��ť����
                , shade: .1 //����ʾ����
                , yes: function () {
                    layer.closeAll();
                }
            });
        }
    };
    $('#layerDemo').on('click', function () {
        var othis = $(this), method = othis.data('method');
        active[method] ? active[method].call(this, othis) : '';
    });

    //�����ύ
    form.on('submit(demo1)', function (data) {
        console.log(data.field, "�������");
        data.field.type = data.field.type == "����" ? "1" : "2";
        data.field.parentId = $("#parentId").attr("guid");

        //���ظ����˵�
        ajax_request({
            url: "typehandler.ashx?action=add",
            data: data.field,
            callback: function (data) {
                data = JSON.parse(data);
                if (data.code == 0) {
                    layer.msg("��ӳɹ�");
                    layer.closeAll("page"); //�رգ���Ϣ��Ĭ��dialog��1��pageҳ��㣩2��iframe�㣩3��loading���ز㣩4��tips�㣩
                    $("input[name='cateName']").val(" ");
                    loadtree();

                } else {
                    console.log(data);
                    layer.msg("�����������ˣ�������");
                }
            }
        });
        return false;
    });
});