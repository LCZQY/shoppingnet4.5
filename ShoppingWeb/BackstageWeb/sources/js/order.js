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
        loadtable: function (typeid, typename) {
            table.render({
                url: "orderhandler.ashx?action=list"
                , method: "POST"
                //, where: { page: 0, limit: 10 }
                , elem: '#test'
                , toolbar: '#toolbarDemo'
                , cellMinWidth: 80 //全局定义常规单元格的最小宽度，layui 2.2.1 新增
                , title: '用户数据表'
                , location: true
                , cols: [
                    [
                        { type: 'radio', fixed: 'left' }
                        , { field: 'OrdersId', title: '订单编号' }
                        , { field: 'Title', title: '商品名称', }
                        , { field: 'Total', title: '总价',sort: true, }
                        , { field: 'Nick', title: '卖家昵称', }
                        , { field: 'Orderdate', title: '下单时间', sort: true }
                        , {
                            field: 'DeliveryDate', title: '收货时间', sort: true, templet: function (d) {
                                return layui.util.toDateString(d.DeliveryDate);
                            }, edit: ''
                        }
                        , { field: 'Remark', title: '订单备注', edit: '' }
                        , { field: 'Consignee', title: '收货人姓名', }
                        , { field: 'Phone', title: '电话号码' }
                        , { field: 'Complete', title: '收货地址', edit: '' }
                        , { field: 'States', title: '订单状态',  templet: '#table-States' }
                        , { field: 'DetailStates', title: '订单跟踪状态', templet: '#table-DetailStates' }
                        , { fixed: 'right', title: '操作', toolbar: '#barDemo' }
                    ]
                ]
                , page: true
            });
        },


        //表格操作
        toolbar: function () {
            reload();
            //头工具栏事件
            table.on('toolbar(test)', function (obj) {
                var checkStatus = table.checkStatus(obj.config.id);
                switch (obj.event) {
                    case 'getCheckData':
                        var data = checkStatus.data;
                        layer.alert(JSON.stringify(data));
                        break;
                    case 'getCheckLength':
                        var data = checkStatus.data;
                        layer.msg('选中了：' + data.length + ' 个');
                        break;
                    case 'isAll':
                        layer.msg(checkStatus.isAll ? '全选' : '未全选');
                        break;
                    case 'add':
                        layer.msg("开发中...");
                    
                        break;
                    case
                        'batchDel'
                        :
                        layer.msg("开发中...");
                        /*发送ajax请求到后台执行批量删除*/
                        break;
                    case
                        'flush'
                        :
                        table.reload('test', {
                            url: "orderhandler.ashx?action=list"
                            , method: "GET"
                        });
                        break;
                    case
                        'search'
                        :
                        //  layer.msg("根据订单编号查找");
                        var OrdersId = $('input[name="search"]').val();
                        console.log(name, "0");
                        table.reload('test', {
                            url: 'orderhandler.ashx?action=search',
                            where: {
                                id: OrdersId,
                            },
                            page: {
                                curr: 1
                            }
                        });
                        break;
                };
            });
            //监听行工具事件
            table.on('tool(test)', function (obj) {
                var data = obj.data;
                if (obj.event === 'send') { //发货
                    layer.confirm('是否确定发货', function (index) {
                        //obj.del();
                        //layer.close(index);
                        $.ajax({
                            url: "orderhandler.ashx?action=send",
                            type: "POST",
                            dataType: "json",
                            data: {
                                id: data.OrdersId
                            },
                            success: function (e) {
                                if (e.code == 0) {
                                    layer.msg(e.msg);
                                    window.parent.location.reload();
                                } else {
                                    layer.msg(e.msg);
                                }
                            },
                            error: function (e) {
                                layer.msg(e);
                            }
                        })
                    });
                }
                if (obj.event === 'detail') { //详情
                    layer.msg("开发中");
                }
                if (obj.event === 'evaluate') { //查看评价??
                    layer.msg("开发中");
                }

            });
        }
    }
    //输出接口
    exports('myprodoct', myprodoct);
});



