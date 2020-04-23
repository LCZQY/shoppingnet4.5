
/*
 * @Author: zqy
 * @Author: zqy
 * @Date:   2020-04-23
 * @lastModify 2020-04-23
 * +----------------------------------------------------------------------
 * | Zqy [ 商城后台管理模板 ]
 * | 基于Layui http://www.layui.com/
 * +----------------------------------------------------------------------
 */

layui.define(['jquery', 'form', 'layer', 'table'], function (exports) {
    var $ = layui.jquery,
        layer = layui.layer,
        form = layui.form,
        table = layui.table;
    var parentId = "";
    var type = {

        //加载表格数据
        loadtable: function (typeid, typename) {

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
                        , { field: 'title', title: '商品名称', align: "center", width: 120 }
                        , { field: 'icon', title: '封面', width: 100, align: "center", templet: "#imgtmp" }
                        , { field: 'cateName', title: '商品类型', width: 120, align: "center" }
                        , { field: 'marketPrice', title: '市场价格(元)', width: 120, align: "center", sort: true }
                        , { field: 'price', title: '本站价格(元)', width: 120, sort: true, align: "center" }
                        , {
                            field: 'postTime', title: '上架时间', width: 120, sort: true, align: "center", templet: function (d) {
                                return layui.util.toDateString(d.DeliveryDate);
                            }, edit: ''
                        }
                        , { field: 'stock', title: '库存数量(件)', width: 120, sort: true, align: "center" }
                        , { field: 'content', title: '商品描述', width: 120, align: "center" }
                        , { fixed: 'right', title: '操作', align: "center", toolbar: '#takeaction', width: 140 }
                    ]
                ]
                , page: { limit: 10 }
            });

        }
        ,
        //表格操作
        toolbar: function () {

            //头工具栏事件
            table.on('toolbar(test)', function (obj) {
                switch (obj.event) {
                    //case 'add':
                    //    layer.open({
                    //        title: '',
                    //        /*如果是外部的html，type2，内部，type1*/
                    //        type: 1,
                    //        btnAlign: 'c',
                    //        area: '50%',
                    //        content: $("#add-main").html()
                    //        //未做的是去监听表单提交，给后台发送ajax请求
                    //    });
                    //    break;
                    //case 'batchDel':
                    //    layer.msg("开发中...");
                    //    /*发送ajax请求到后台执行批量删除*/
                    //    break;
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
        }
        ,
        //树形结构
        loadtree: function () {
            $.ajax({
                type: "get",
                url: WEBURL + "/api/type/tree",
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
                            onlyIconControl: true,
                            showLine: true,//显示线
                            accordion: true,//手风琴模式
                            drag: false,//拖拽
                            skin: "laySimple", // laySimple主题风格
                            showSearch: true,//显示搜索框
                            //edit: false,
                            edit: ['update', "del", "add"], //操作节点的图标,
                            click: function (obj) {
                                //  $(".radio").eq(0).removeAttr("checked");
                                $("#parentId").val(obj.data.title);
                                $("#parentId").attr("guid", obj.data.id);
                                $("#type").val("子级");
                                console.log(obj.data, "点击查看商品");
                                //新增加的类型不展示商品列表
                                if (!(obj.data.id === undefined)) {
                                    type.loadtable(obj.data.id, obj.data.title);
                                    //表单操作
                                    table_show();
                                } else {
                                    layer.msg("请编辑商品类型名称");
                                    return false;
                                }
                            },
                            //树形菜单的增删改查，操作
                            operate: function (obj) {
                                var type = obj.type; //得到操作类型：add、edit、del
                                var data = obj.data; //得到当前节点的数据
                                var elem = obj.elem; //得到当前节点元素
                                if (type === 'del') {
                                    //删除的组件layui 里面有个Bug ！！ 直接点击删除就不见了,现已解决，在源码中添加确认框
                                    var arr = obj.data;
                                    ajax_request({
                                        url: WEBURL + "/api/type/delete?id=" + arr.id,
                                        callback: function (e) {
                                            if (e.code === '0') {
                                                layer.closeAll();
                                                loadtree();
                                                layer.msg("删除成功");
                                            } else {
                                                console.log(e.message);
                                                layer.msg("服务器出错了，请重试");
                                            }
                                        }
                                    }, "DELETE");
                                } else if (type === 'update') {
                                    //修改   
                                    if (data.id !== undefined) {
                                        table_confirm({
                                            obj: obj,
                                            url: WEBURL + "/api/type/edit",
                                            tips: "是否修改商品类型名称",
                                            data: { cateName: data.title, id: data.id },
                                            active: function (e) {
                                                if (e.code === '0') {
                                                    layer.closeAll();
                                                    loadtree();
                                                    layer.msg("修改成功");
                                                } else {
                                                    console.log(e.message);
                                                    layer.msg("服务器出错了，请重试");
                                                }
                                            }
                                        });
                                    }
                                    //新增
                                    else {
                                        ajax_request({
                                            url: WEBURL + "/api/type/edit",
                                            data: { cateName: data.title, parentId: parentId },
                                            callback: function (e) {
                                                if (e.code === '0') {
                                                    layer.closeAll();
                                                    loadtree();
                                                    layer.msg("编辑成功");
                                                } else {
                                                    console.log(e.message);
                                                    layer.msg("服务器出错了，请重试");
                                                }
                                            }
                                        });
                                    }
                                } else if (type === 'add') {
                                    parentId = data.id;
                                    elem.find('.layui-tree-pack:first').attr("style", "display: block;");
                                    elem.find('.layui-icon:first').attr("style", "layui-icon-subtraction;");
                                    elem.addClass("layui-tree-spread");
                                }
                            }
                        });
                    });
                }
            });
        }
        ,
        //添加商品类型
        inserttype: function () {

            //添加商品类型模态框 
            var active = {
                offset: function (othis) {
                    var type = othis.data('type')
                    layer.open({
                        type: 1
                        , title: "添加顶级类型"
                        , area: '50%'
                        , offset: type
                        , align: 'center'
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
                data.field.type = data.field.type === "顶级" ? "1" : "2";
                data.field.parentId = $("#parentId").attr("guid") === '' ? "0" : $("#parentId").attr("guid");
                ajax_request({
                    url: WEBURL + "/api/type/edit",
                    data: data.field,
                    callback: function (data) {
                        if (data.code === '0') {

                            layer.msg("添加成功");
                            layer.closeAll("page"); //关闭（信息框，默认dialog）1（page页面层）2（iframe层）3（loading加载层）4（tips层）
                            $("input[name='cateName']").val(" ");
                            loadtree();

                        } else {
                            console.log(data.message);
                            layer.msg("服务器出错了，请重试");
                        }
                    }
                });
                return false;
            });
        }
    }
    //输出接口
    exports('type', type);
});

