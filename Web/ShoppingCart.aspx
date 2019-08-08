<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShoppingCart.aspx.cs" Inherits="Web.ShoppingCart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .nav-pills > li.active > a, .nav-pills > li.active > a:focus, .nav-pills > li.active > a:hover {
            background: #3c3c3c;
        }
    </style>
    <!-- cart area start -->
    <div class="cart-area mrgn-40">
        <div class="container">
            <div class="row">
                <div class="col-xs-12">
                    <div class="entry-title">
                        <h2 id="title">我的购物车</h2>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-2">
                    <ul class="nav nav-pills nav-stacked">
                        <li role="presentation" class="active"><a href="##" id="cartlsit">我的购物车</a></li>
                        <li role="presentation"><a href="##" id="favoritelist">我的收藏</a></li>
                        <li role="presentation"><a href="##" id="appraiselist">我的评价</a></li>
                        <li role="presentation"><a href="##" id="addreeslist">我的地址</a></li>
                    </ul>
                </div>
                <div class="col-xs-10">
                    <div class="cart-content">
                        <form action="#">
                            <%--购物车--%>
                            <div id="cart_table" hidden>
                                <table>
                                    <thead>
                                        <tr>
                                            <th class="product-thumbnail">图片</th>
                                            <th class="product-name">商品</th>
                                            <th class="product-price">价格</th>
                                            <th class="product-quantity">数量</th>
                                            <th class="product-subtotal">总价</th>
                                            <th class="product-remove">操作</th>
                                        </tr>
                                    </thead>
                                    <tbody id="tbody">
                                    </tbody>
                                </table>
                                <div class="row" id="row">
                                    <div class="col-md-8 col-sm-7 col-xs-12">
                                        <div class="buttons-cart">
                                            <input value="更新购物车" type="button" id="flsh">
                                            <a href="~" runat="server" >继续购物</a>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-5 col-xs-12">
                                        <div class="cart_totals">
                                            <h2>购物车总计</h2>
                                            <table>
                                                <tbody>
                                                    <tr class="order-total">
                                                        <th>总共：</th>
                                                        <td>
                                                            <strong><span id="amount" class="amount"></span></strong>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                            <div class="wc-proceed-to-checkout">
                                                <a href="#">立即支付</a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%--收藏--%>
                            <div id="favio_table" hidden>
                                <table>
                                    <thead>
                                        <tr>
                                            <th class="product-thumbnail">图片</th>
                                            <th class="favio-name">商品</th>
                                            <th class="favio-price">价格</th>
                                            <th class="favio-subtotal">收藏时间</th>
                                            <th class="favio-remove">操作</th>
                                        </tr>
                                    </thead>
                                    <tbody id="favio_tbody">
                                        <%--  <tr>
                                            <td class="product-thumbnail"><a href="#">
                                                <img src="img/product/10.jpg" alt=""></a></td>
                                            <td class="favio-name"><a href="#">北极熊</a></td>
                                            <td class="favio-price"><span class="amount">￥165.00</span></td>
                                            <td class="favio-quantity">1</td>
                                            <td class="favio-remove"><a href="#">删除</a></td>
                                        </tr>--%>
                                    </tbody>
                                </table>
                            </div>
                            <%--地址--%>
                            <div id="address_table" hidden>
                                <table>
                                    <thead>
                                        <tr>
                                            <th class="address-thumbnail">收货人</th>
                                            <th class="address-thumbnail">详细地址</th>
                                            <th class="address-thumbnail">手机</th>
                                            <th class="address-remove">操作</th>
                                        </tr>
                                    </thead>
                                    <tbody id="address_tbody">
                                    </tbody>
                                </table>
                                <div class="row">
                                    <div class="col-md-8 col-sm-7 col-xs-12">
                                        <div class="buttons-cart">
                                            <a href="##" data-toggle="modal" data-target="#AddreesModal" id="adrees" data-whatever="@mdo">添加地址</a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%--评价--%>
                            <div id="appraise_table" hidden>
                                <table>
                                    <thead>
                                        <tr>
                                            <th class="appraise-thumbnail">图片</th>
                                            <th class="appraise-name">商品</th>
                                            <th class="appraise-price">评价等级</th>
                                            <th class="appraise-price">评价内容</th>
                                            <th class="appraise-subtotal">评价时间</th>
                                            <th class="appraise-remove">操作</th>
                                        </tr>
                                    </thead>
                                    <tbody id="appraise_tbody">                                        
                                    </tbody>
                                </table>
                            </div>
                            <div class="table-content table-responsive" id="div">
                            </div>
                        </form>
                        <%--添加地址模态框开始--%>
                        <div class="modal fade" id="AddreesModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel">
                            <div class="modal-dialog model-sm" role="document">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                        <h4 class="modal-title" id="exampleModalLabel">添加收货地址</h4>
                                    </div>
                                    <div class="modal-body">
                                        <form method="post">
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">收件人姓名:</label>
                                                <input type="text" class="form-control" id="Consignee">
                                            </div>
                                            <div class="form-group">
                                                <label for="message-text" class="control-label">详细地址:</label>
                                                <input type="text" class="form-control" id="Complete">
                                            </div>
                                            <div class="form-group registration">
                                                <label for="message-text" class="control-label">手机号:</label>
                                                <input type="text" class="form-control " id="Phone">
                                            </div>
                                        </form>
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                                        <button type="button" id="submitadrees" class="btn btn-primary">确认添加</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--用户登陆模态框结束--%>

                        <%--商品支付界面开始--%>
                        <div class="modal fade" id="PayModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel">
                            <div class="modal-dialog model-sm" role="document">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                        <h4 class="modal-title">请核对订单信息</h4>
                                    </div>
                                    <div class="modal-body">
                                        <form method="post">
                                           <%-- <div class="form-group">
                                                <label for="recipient-name" class="control-label">商品名称:<label class="spmc">1</label></label>
                                            </div>
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">应付价格:<label class="yfjg"></label></label>
                                            </div>
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">联系人号码:<label class="lxrhm"></label></label>
                                            </div>--%>
                                            <div class="form-group">
                                                <label for="message-text" class="control-label">请选择收货地址:</label>
                                                <select class="form-control" id="selects">
                                                    <option>请选择</option>
                                                </select>
                                            </div>
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">备注信息:</label>
                                                <textarea class="form-control" id="bzxx" rows="3"></textarea>
                                            </div>
                                        </form>
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                                        <button type="button" id="sumbitpay" class="btn btn-primary">立即支付</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--商品支付界面结束--%>
                        <%--商品评论界面开始--%>
                        <div class="modal fade" id="evaluateModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel">
                            <div class="modal-dialog model-sm" role="document">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                        <h4 class="modal-title">商品评价</h4>
                                    </div>
                                    <div class="modal-body">
                                        <form method="post">
                                           <%-- <div class="form-group">
                                                <label for="recipient-name" class="control-label">商品名称:<label class="spmc">1</label></label>
                                            </div>--%>
                                            <div class="form-group">
                                                <label for="message-text" class="control-label">等级:</label>
                                                <select class="form-control" id="evaluate_selects">
                                                    <option>请选择</option>
                                                    <option value="0">很好</option>
                                                    <option value="1">好</option>
                                                    <option value="2">不好</option>
                                                </select>
                                            </div>
                                            <div class="form-group">
                                                <label for="recipient-name" class="control-label">内容:</label>
                                                <textarea class="form-control" id="evaluate_conent" rows="3"></textarea>
                                            </div>
                                        </form>
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                                        <button type="button" id="sumbit_evaluate" class="btn btn-primary">立即评价</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--商品评论界面结束--%>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- cart area end -->
    <div id="back-top"><i class="fa fa-angle-up"></i></div>
    <script src="AppData/layui/jquery.js"></script>
    <script src="AppData/layui/layui.js"></script>
    <script src="Scripts/Shopping/master.js"></script>
    <script src="Scripts/Shopping/cart.js" charset="utf-8"></script>
    <script>
        layui.use(['layer'], function () {
            layer = layui.layer;
            var userid = localStorage.getItem("id");
            console.log(userid, "--------userid-----------------------------");
            if (userid) {

                ////获取购物车列表
                ajax_request({
                    url: "Aspx/ManagePages/orderhandler.ashx?action=cart",
                    data: { "UserId": userid },
                    callback: function (e) {
                        if (e) {
                            e = JSON.parse(e);
                            console.log(e, "加入购物车成功！！！！！");
                            if (e.code === 0) {
                                if (e.data != null) {
                                    var total = MyCartList(e.data);
                                    $("#amount").text(total);
                                }
                            } else {

                            }
                        }
                    }
                });
                //默认显示购物车
                $("#div").html($("#cart_table").html());

                //收藏
                ajax_request({
                    url: "Aspx/ManagePages/favoritehandler.ashx?action=list",
                    data: { "UserId": userid },
                    callback: function (e) {
                        if (e) {
                            e = JSON.parse(e);
                            console.log(e, "加入收藏成功！！！！！");
                            if (e.code === 0) {
                                MyFacoiriteList(e.data);
                            }
                        }
                    }
                });
                var arrlist = [];
                //我的地址列表
                var addreeslist = function () {
                    ajax_request({
                        url: "Aspx/ManagePages/deliveryhandler.ashx?action=list",
                        data: { "UserId": userid },
                        callback: function (e) {
                            console.log(e, "我的地址列表！！！！！");
                            if (e) {
                                e = JSON.parse(e);
                                
                                if (e.code === 0) {
                                    arrlist = e.data;
                                    MyAdreesList(e.data);
                                }
                            }
                        }
                    });
                }
                addreeslist();

                //我的评价列表
                ajax_request({
                    url: "Aspx/ManagePages/appraisehandler.ashx?action=list",
                    data: { "UserId": userid },
                    callback: function (e) {
                        console.log(e, "我的地址列表！！！！！");
                        if (e) {
                            e = JSON.parse(e);
                            
                            if (e.code === 0) {
                               // arrlist = e.data;
                                MyPingjiaList(e.data);
                            }
                        }
                    }
                });

                //模态框添加地址
                $("#submitadrees").click(function () {
                    ajax_request({
                        url: "Aspx/ManagePages/deliveryhandler.ashx?action=add",
                        data: { "UserId": userid, "Complete": $("#Complete").val(), "Consignee": $("#Consignee").val(), "Phone": $("#Phone").val() },
                        callback: function (e) {
                            if (e) {
                                e = JSON.parse(e);
                                console.log(e, "添加地址！！！！！");
                                if (e.code === 0) {
                                    $('#AddreesModal').modal('hide');
                                    addreeslist();
                                    page_reload();
                                }
                            }
                        }
                    });
                });

                /*监听模态框关闭事件*/
                $(function () {
                    $('#AddreesModal').on('hidden.bs.modal', function () {
                        $(this).removeData('bs.modal');
                    });
                });

                //获取地址id
                var deliveryId = "";
                var orderid = "";
                $("#selects").change(function () {
                    deliveryId = $("#selects").find("option:selected").attr("name");
                    console.log(deliveryId,"地址ID>>>>>>>>>>>>>>>>>>>>>>");
                });

                //展示模态框
                $(".paymoeny").click(function () {
                    orderid = $(this).attr("name");
                    $('#PayModal').modal('show');
                    if (arrlist) {
                        $.each(arrlist, function (index, item) {
                            $("#selects").append('<option name="' + item.DeliveryId + '">' + item.Complete + '<option>');
                        });
                    }
                });


                //修改改订单状态
                $("#sumbitpay").click(function () {
                    if (deliveryId == "") {
                        layer.msg("请选择收货地址");
                        return false;
                    } else {
                        var request = { "UserId": userid, "action": "1", "DeliveryId": deliveryId, "OrdersId": orderid, "Remark": $("#bzxx").val() };
                        console.log(request, "修改订单参数~~~");
                        ajax_request({
                            url: "Aspx/ManagePages/orderhandler.ashx?action=update",
                            data: request,
                            callback: function (e) {
                                if (e) {
                                    e = JSON.parse(e);
                                    if (e.code === 0) {
                                        layer.msg("正在支付中.......");
                                        $('#PayModal').modal('hide');
                                        window.parent.location.reload();
                                    } else {
                                        layer.msg("支付失败请重试.......");
                                    }
                                }
                            }
                        });
                    }
                });

                //确定收货
                $(".Receiving").click(function () {
                    orderid = $(this).attr("name");
                    layer.confirm('是否确定收货', function (index) {

                        var request = { "UserId": userid, "action": "3", "OrdersId": orderid, };
                        ajax_request({
                            url: "Aspx/ManagePages/orderhandler.ashx?action=update",
                            data: request,
                            callback: function (e) {
                                if (e) {
                                    e = JSON.parse(e);
                                    if (e.code === 0) {
                                        layer.msg("收货成功");
                                        page_reload();
                                    } else {
                                        layer.msg("失败,请重试");
                                    }
                                }
                            }
                        });
                    });
                });

                //评价   
                var grade = "";
                var ProId = "";
                $("#evaluate_selects").change(function () {
                    grade = $(this).val();
                });
                $(".pingjia").click(function () {
                    $('#evaluateModal').modal('show');                  
                        orderid = $(this).attr("name");
                        ProId = $("#" + orderid).attr("name");                                        
                });               
                $("#sumbit_evaluate").click(function () {
                    if (grade == "") {
                        layer.msg("请选择评价等级");
                        return false;
                    } else {
                        ajax_request({
                            url: "Aspx/ManagePages/appraisehandler.ashx?action=add",
                            data: { "UserId": userid, "ProductId": ProId, "Grade": grade, "Content": $("#evaluate_conent").val() },
                            callback: function (e) {
                                if (e) {
                                    e = JSON.parse(e);
                                    if (e.code === 0) {

                                        //修改订单状态
                                        var request = { "UserId": userid, "action": "4", "OrdersId": orderid};
                                        ajax_request({
                                            url: "Aspx/ManagePages/orderhandler.ashx?action=update",
                                            data: request,
                                            callback: function (e) {
                                                if (e) {
                                                    e = JSON.parse(e);
                                                    if (e.code === 0) {
                                                        layer.msg("评论成功");
                                                        window.parent.location.reload();
                                                    } else {
                                                        layer.msg("失败,请重试");
                                                    }
                                                }
                                            }
                                        });
                                    } else {
                                        layer.msg("失败,请重试");
                                    }
                                }
                            }
                        });
                    }
                });              
            } else {
                layer.msg("请先登陆");
            }
        });
    </script>
</asp:Content>
