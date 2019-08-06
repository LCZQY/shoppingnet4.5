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
                        <h2>我的购物车</h2>
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
                            <div class="table-content table-responsive">
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
                            </div>
                            <div class="row">
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
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- cart area end -->
    <div id="back-top"><i class="fa fa-angle-up"></i></div>
    <script src="Scripts/jquery-3.3.1.min.js"></script>
    <script src="AppData/layui/layui.js"></script>
    <script src="Scripts/Shopping/master.js"></script>
    <script>
        var userid = localStorage.getItem("id");
        if (userid) {

            //单个购物车源码
            var cartConentHtml = function (options) {
                var image = '<tr><td class="product-thumbnail"><a href="##"><img src="' + options.path + '" alt=""></a></td>';
                var name = '<td class="product-name"><a href="#">"' + options.name + '"</a></td>';
                var price = '<td class="product-price"><span class="amount">￥"' + options.amount + '"</span></td>';
                var quantity = '<td class="product-quantity"><input value="' + options.quantity + '" type="text" disabled></td>'; //number
                var subtotal = '<td class="product-subtotal">￥"' + options.amount * options.quantity + '"</td>';
                var pj = '';
                if (options.states == "2") { pj = '<a href="##" name="' + options.id + '">评价</a>'; }
                var product = '<td class="product-remove">' + pj + '<a href="##" name="' + options.id + '">结算</a><a href="##" name="' + options.id + '">删除</a></td></tr>';
                return image + name + price + quantity + subtotal + product;
            }
            var carthtml = function (obj) {
                    var list = JSON.parse(obj);
                    var html = '';
                    $.each(list, function (index, item) {
                        console.log(item, "!!!!!!!!!!!!!!!!!!!!!!!");                                               
                        html += cartConentHtml({
                            path: item.Path,
                            name: item.Title,
                            quantity: item.Quantity,
                            amount: item.Total,
                            states: item.States,
                            id: item.ProductId
                        });
                    });
                    $("#tbody").append(html);           
            };
            //获取购物车列表
            ajax_request({
                url: "Aspx/ManagePages/orderhandler.ashx?action=cart",
                data: { "UserId": userid },
                callback: function (e) {
                    e = JSON.parse(e);
                    console.log(e, "加入购物车成功！！！！！");
                    if (e.code === 0) {
                        carthtml(e.data);
                    } else {

                    }
                }
            });


            /*收藏*/
            $("#favoritelist").click(function () {

                ajax_request({
                    url: "Aspx/ManagePages/favoritehandler.ashx?action=list",
                    data: { "UserId": userid },
                    callback: function (e) {
                        e = JSON.parse(e);
                        console.log(e, "加入购物车成功！！！！！");
                        if (e.code === 0) {
                            carthtml(e.data);
                        } else {

                        }
                    }
                });
                 
            });


        } else {
            alert("请登陆");
        }
    </script>
</asp:Content>
