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
        table = layui.table;
    var myprodoct = {

        /**
         * 绑定数据表格
         * */
        loadtable : function(typeid, typename) {
            console.log(table, "tables")
            table.render({
                url: WEBURL + "/api/product/layui/table/list"
                , method: "POST"
                , startByZero: 0
                //, where: { cateId: typeid }
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
                        , { field: 'cateName', title: '商品类型', width: 120, align: "center", sort: true, }
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
                , page: { limit: 10 }

            });
        },

      
        //表格操作
        table_show : function () {
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
        }
    }
    //输出接口
    exports('myprodoct', myprodoct);
});



