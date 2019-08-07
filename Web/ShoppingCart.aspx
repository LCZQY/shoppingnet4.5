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
                                        <tr>
                                            <td class="product-thumbnail"><a href="#">
                                                <img src="img/product/10.jpg" alt=""></a></td>
                                            <td class="product-name"><a href="#">北极熊</a></td>
                                            <td class="product-price"><span class="amount">￥165.00</span></td>
                                            <td class="product-quantity">
                                                <input value="1" type="number"></td>
                                            <td class="product-subtotal">￥165.00</td>
                                            <td class="product-remove">
                                                <a href="#">删除</a>
                                                <a href="#">结算</a>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class="row" id="row">
                                    <div class="col-md-8 col-sm-7 col-xs-12">
                                        <div class="buttons-cart">
                                            <input value="更新购物车" type="button" id="flsh">
                                            <a href="#">继续购物</a>
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
                                                            <strong><span class="amount">￥215.00</span></strong>
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
                                        <tr>
                                            <td class="product-thumbnail"><a href="#">
                                                <img src="img/product/10.jpg" alt=""></a></td>
                                            <td class="favio-name"><a href="#">北极熊</a></td>
                                            <td class="favio-price"><span class="amount">￥165.00</span></td>
                                            <td class="favio-quantity"></td>
                                            <td class="favio-remove"><a href="#">删除</a></td>
                                        </tr>
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
                                        <tr>
                                            <td class="address-quantity">的说法是手动阀手动阀撒旦发射点</td>
                                            <td class="address-quantity">的说法是手动阀手动阀撒旦发射点</td>
                                            <td class="address-quantity">的说法是手动阀手动阀撒旦发射点</td>
                                            <td class="address-remove"><a href="#">删除</a></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <%--评价--%>
                            <div id="appraise_table" hidden>
                                <table>
                                    <thead>
                                        <tr>
                                           <th class="appraise-thumbnail">图片</th>
                                            <th class="appraise-name">商品</th>
                                            <th class="appraise-price">评价等级</th>
                                            <th class="appraise-subtotal">评价时间</th>
                                            <th class="appraise-remove">操作</th>
                                        </tr>
                                    </thead>
                                    <tbody id="appraise_tbody">
                                        <tr>
                                            <td class="product-quantity"><a href="#">
                                                <img src="img/product/10.jpg" alt=""></a></td>
                                            <td class="appraise-quantity">的说法是手动阀手动阀撒旦发射点</td>
                                            <td class="appraise-quantity">的说法是手动阀手动阀撒旦发射点</td>
                                            <td class="appraise-quantity">的说法是手动阀手动阀撒旦发射点</td>
                                            <td class="appraise-remove"><a href="#">删除</a></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="table-content table-responsive" id="div">
                            </div>

                        </form>
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
    <script src="Scripts/Shopping/cart.js" charset="utf-8" ></script>
    <script>
        var userid = localStorage.getItem("id");
        console.log(userid,"--------userid-----------------------------");
        if (userid) {         
            ////获取购物车列表
            //ajax_request({
            //    url: "Aspx/ManagePages/orderhandler.ashx?action=cart",
            //    data: { "UserId": userid },
            //    callback: function (e) {
            //        e = JSON.parse(e);
            //        console.log(e, "加入购物车成功！！！！！");
            //        if (e.code === 0) {
            //            carthtml(e.data);
            //        } else {
            //        }
            //    }
            //});


            $("#div").html($("#cart_table").html());

           

        } else {
            alert("请登陆");
        }
    </script>
</asp:Content>
