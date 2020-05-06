/*
 * @Author: zqy
 * @Author: zqy
 * @Date:   2020-05-04
 * @lastModify 2020-05-04
 * ----------------------------------------------------------------------
 * | Zqy [ 商城后台管理模板 ]
 * | 基于Layui http://www.layui.com/
 * +----------------------------------------------------------------------
 */

layui.define(['jquery', 'form', 'layer', 'table'], function (exports) {
    var $ = layui.jquery,
        layer = layui.layer,
        table = layui.table,
        form = layui.form;
    var _userid = "";
    var user = {

        /**
         * 绑定数据表格
         */
        loadtable: function (typeid, typename) {
            console.log(table, "tables")
            table.render({
                url: AuthentictionURL + "/api/user/layui/table/list"
                , method: "POST"
                , startByZero: 0
                //, where: { cateId: typeid }
                //,headers: {""} 携带token
                , contentType: 'application/json'
                , cellMinWidth: 80 //全局定义常规单元格的最小宽度，layui 2.2.1 新增
                , elem: '#test'
                , toolbar: '#toolbarDemo'
                , title: '用户列表'
                , location: true
                , cols: [
                    [
                        { type: 'checkbox', fixed: 'left' }
                        , { field: 'userName', title: '用户名', align: "center"}
                        , { field: 'trueName', title: '姓名',  align: "center" }
                        , { field: 'phoneNumber', title: '手机号码', align: "center", sort: true, }
                        , {
                            field: 'roleName', title: '所属角色', align: "center", templet: function (d) {

                                if (d.roleName === null || d.roleName === '') { return "----"; }
                                return d.roleName;
                            }
                        }
                        , {
                            field: 'createTime', title: '创建时间', sort: true, align: "center", templet: function (d) {
                                return layui.util.toDateString(d.createTime);
                            }
                        }
                        , { fixed: 'right', title: '操作', align: "center", toolbar: '#takeaction' }
                    ]
                ]
                , page: { limit: 10 }

            });
        },


        /**
         *表格操作 
         */
        table_show: function () {
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
                        console.log("刷新中");
                        table.reload('test', {
                            url: AuthentictionURL + "/api/user/layui/table/list"
                            , method: "POST"
                            , contentType: 'application/json'
                            //  , where: { name: null }
                        });
                        break;
                    case 'search':
                        var name = $('input[name="search"]').val();
                        if (name === '') {
                            layer.msg("请输入商品名称");
                        } else {
                            table.reload('test', {
                                url: AuthentictionURL + "/api/user/layui/table/list"
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
                    case 'addrole':

                        var active = {
                            offset: function (othis) {
                                var type = othis.data('type')
                                layer.open({
                                    type: 1
                                    , title: "编辑用户角色"
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
                        }
                        var othis = $(this), method = othis.data('method');
                        active[method] ? active[method].call(this, othis) : '';

                        //获取角色列表
                        user.roleList(data.id);
                        //注：在动态加载多选框的时候，一定要重新渲染下，才可以显示
                        form.render('checkbox');

                        break;
                }
            });
        },
        /**
         * 用户所属角色
         * @param {any} userid 用户id
         */
        roleList: function (userid) {
            _userid = userid;
            ajax_request({
                url: AuthentictionURL + "/api/user/get/role?userid=" + userid,
                callback: function (e) {

                    if (e.code === "0") {
                        var str = "";
                        $.each(e.extension, function (i, value) {
                            var checked = value.isAuthorize ? 'checked = ""' : '';
                            str += '<input roleid = "' + value.id + '" type = "checkbox" lay-skin="primary" name = "role" title = "' + value.name + '" ' + checked + '>';
                        });
                        $("#checkboxs").html(str);

                    } else {
                        layer.msg(e.message);
                    }
                }
            }, "GET");
        },

        /**
         * 保存用户角色信息        
         */
        submit_add_role: function () {
            //表单的提交
            form.on('submit(demo1)', function (data) {

                //获取多选框的值
                var arr = [];
                $("input:checkbox[name='role']:checked").each(function (i) {
                    arr[i] = $(this).attr("roleid");
                });
                console.log(arr, "arr");
                ajax_request({
                    url: AuthentictionURL + "/api/user/role/add",
                    data: { userId: _userid, roleId: arr },
                    callback: function (data) {
                        if (data.code === '0') {
                            layer.msg("角色编辑成功");
                            layer.closeAll("page");
                            user.loadtable();
                        } else {
                            layer.msg(data.message, {
                                time: 800,
                            });
                        }
                    }
                });
                return false;
            });

        }
    }
    //输出接口 
    exports('user', user);
});

