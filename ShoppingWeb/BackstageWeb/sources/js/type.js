/**
        * 一般用于删除数据行使用
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
 * 绑定数据表格
 * */
var loadtable = function (typeid, typename) {
    layui.use(['table', 'layer', 'form'], function () {
        var table = layui.table;
        console.log(table, "tables")
        table.render({
            url: WEBURL + "/api/product/layui/table/list"
            , method: "POST"
            , startByZero: 0
            , where: { cateId: typeid }
            //,headers: {""} 携带token
            , contentType: 'application/json'
            , elem: '#test'
            , toolbar: '#toolbarDemo'
            , title: '商品列表'
            , location: true
            , cols: [
                [
                    { type: 'checkbox', fixed: 'left' }
                    , { field: 'title', title: '商品名称', align: "center", width: 120, edit: 'text' }
                    , { field: 'icon', title: '封面', width: 100, align: "center", templet: "#imgtmp" }
                    , { field: 'cateId', title: '商品类型', width: 120, align: "center", sort: true, }
                    , { field: 'marketPrice', title: '市场价格(元)', width: 120, align: "center", edit: 'text' }
                    , { field: 'price', title: '本站价格(元)', width: 120, sort: true, align: "center", edit: 'text' }
                    , {
                        field: 'postTime', title: '上架时间', width: 120, sort: true, align: "center", templet: function (d) {
                            return layui.util.toDateString(d.DeliveryDate);
                        }, edit: ''
                    }
                    , { field: 'stock', title: '库存数量(件)', width: 120, align: "center", edit: 'text' }
                    , { field: 'content', title: '商品描述', width: 120, align: "center", edit: 'text' }
                    //, { field: 'DetailStates', title: '订单跟踪状态', width: 120, templet: '#table-DetailStates' }
                    , { fixed: 'right', title: '操作', align: "center", toolbar: '#takeaction', width: 140 }
                ]
            ]
            , page: { limit: 10}
        });
    });
};

//表格操作
var table_show = function () {
    layui.use(['table', 'layer', 'form'], function () {
        var table = layui.table;
        //头工具栏事件
        table.on('toolbar(test)', function (obj) {
            switch (obj.event) {
                case 'add':
                    layer.open({
                        title: '',
                        /*如果是外部的html，type2，内部，type1*/
                        type: 1,
                        btnAlign: 'c',
                        area: '50%',
                        content: $("#add-main").html()
                        //未做的是去监听表单提交，给后台发送ajax请求
                    });
                    break;
                case 'batchDel':
                    layer.msg("开发中...");
                    /*发送ajax请求到后台执行批量删除*/
                    break;
                case 'flush':
                    console.log(0);
                    table.reload('test', {
                        url: WEBURL + "/api/product/layui/table/list"
                        , method: "POST"
                        , where: { name: null }
                    });
                    break;
                case 'search':                    
                    var name = $('input[name="search"]').val();
                    if (name === '') {
                        layer.msg("请输入商品名称");
                    } else {
                        table.reload('test', {
                            url: WEBURL + "/api/product/layui/table/list"
                            //, method: "POST"
                            , where: { name: name }
                            , page: { curr: 1 }
                        });
                    }
                    break;
            };
        });

        //监听单元格编辑，修改
        table.on('edit(test)', function (obj) {
            var value = obj.value //得到修改后的值
                , data = obj.data //得到所在行所有键值
                , field = obj.field; //得到字段
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
        //监听行工具事件
        table.on('tool(test)', function (obj) {
            var data = obj.data;
            switch (obj.event) {
                case 'del'://删除
                    table_confirm({
                        obj: obj,
                        url: "groundinghandler.ashx?action=delete",
                        tips: "是否确认删除",
                        data: { id: data.ProductId }
                    });
                    //page_reload();
                    break;
            }
        });


    });
}


//树形结构
function loadtree() {
    $.ajax({
        type: "get",
        url: WEBURL +"/api/type/tree",
        dataType: 'json',
        success: function (res) {
            console.log(res, "数据源");
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
                    showCheckbox: true,//显示多选框
                    showLine: true,//显示线
                    accordion: true,//手风琴模式
                    drag: false,//拖拽
                    skin: "laySimple", // laySimple主题风格
                    showSearch: true,//显示搜索框
                    //edit: false,
                    edit: ['update'], //操作节点的图标,
                    click: function (obj) {
                        //  $(".radio").eq(0).removeAttr("checked");
                        $("#parentId").val(obj.data.title);
                        $("#parentId").attr("guid", obj.data.id);
                        $("#type").val("子级");
                        console.log(obj.data, "点击查看商品");
                        //表格展示区
                        reload(obj.data.id, obj.data.title);
                        //表单操作
                        table_show();
                    },
                    //树形菜单的增删改查，操作
                    operate: function (obj) {
                        var type = obj.type; //得到操作类型：add、edit、del
                        var data = obj.data; //得到当前节点的数据
                        var elem = obj.elem; //得到当前节点元素
                        console.log(type, "当前选择中的是..");
                        if (type === 'del') { //删除的组件layui 里面有个Bug ！！ 直接点击删除就不见了
                            console.log(0);
                            var arr = obj.data;
                            if (arr.id > 0 && (!typeof arr.id != 'undefined')) {
                                if (arr.children.length > 0) {
                                    layer.msg("该商品类型下有子类型不允许删除");
                                    return false;
                                } else {
                                    table_confirm({
                                        obj: obj,
                                        url: "typehandler.ashx?action=update",
                                        tips: "是否确定删除？",
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
                                tips: "是否确定修改？",
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


//菜单搜索！！
//$("input[placeholder='请输入关键字进行过滤']").change(function () {
//    console.log(1);
//});

var inserttype = function () {

    layui.use(['form', 'tree', 'util', 'layer'], function () {
        layer = layui.layer
            , form = layui.form

        /**添加商品类型 */
        var active = {
            offset: function (othis) {

                var type = othis.data('type')
                layer.open({
                    type: 1
                    , title: "添加商品类型"
                    , area: '50%'
                    , offset: type
                    , id: 'layerDemo' + type //防止重复弹出
                    , content: $('#from')
                    // , btn: '关闭全部'
                    , btnAlign: 'l' //按钮居中
                    , shade: .1 //不显示遮罩
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

        //表单的提交
        form.on('submit(demo1)', function (data) {
            console.log(data.field, "请求参数");
            data.field.type = data.field.type == "顶级" ? "1" : "2";
            data.field.parentId = $("#parentId").attr("guid");

            //加载父级菜单
            ajax_request({
                url: "typehandler.ashx?action=add",
                data: data.field,
                callback: function (data) {
                    data = JSON.parse(data);
                    if (data.code == 0) {
                        layer.msg("添加成功");
                        layer.closeAll("page"); //关闭（信息框，默认dialog）1（page页面层）2（iframe层）3（loading加载层）4（tips层）
                        $("input[name='cateName']").val(" ");
                        loadtree();

                    } else {
                        console.log(data);
                        layer.msg("服务器出错了，请重试");
                    }
                }
            });
            return false;
        });
    });
}



loadtree(); //加载树
loadtable(); //加载表格
table_show();//加载表格操作
inserttype(); //添加商品