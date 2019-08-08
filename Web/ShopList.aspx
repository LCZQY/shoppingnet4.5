<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShopList.aspx.cs" Inherits="System.Web.Aspx.ShopList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="shop-area last-list">
        <div class="container">
            <div class="row">
                <div class="col-md-12 col-sm-10">
                    <div class="shop-right-area">
                        <%-- <div class="section-title">
                            <h2>women</h2>
                        </div>--%>
                        <div class="shop-tab-pill">
                            <ul>
                                <li><a href="#grid" data-toggle="tab"><i class="fa fa-th-large"></i></a></li>
                                <li class="active"><a href="#list" data-toggle="tab"><i class="fa fa-th-list"></i></a></li>
                                <li>
                                    <div class="sort-position">
                                        <label>排序方式 : </label>
                                        <%--<select>
                                            <option value="quantity:desc">In stock</option>
                                            <option value="price:desc">Price: Highest first</option>
                                            <option value="name:asc">Product Name: A to Z</option>
                                            <option value="name:desc">Product Name: Z to A</option>
                                            <option value="reference:asc">Reference: Lowest first</option>
                                            <option value="reference:desc">Reference: Highest first</option>
                                        </select>--%>
                                    </div>
                                </li>
                                <li>
                                    <div class="show-label">
                                        <label>展示 : </label>
                                        <select>
                                            <option selected="selected" value="10">10</option>
                                            <option value="09">09</option>
                                            <option value="08">08</option>
                                            <option value="07">07</option>
                                            <option value="06">06</option>
                                        </select>
                                        <span>个</span>
                                    </div>
                                </li>
                            </ul>
                        </div>
                        <div class="tab-content mrgn-40">
                            <div id="list" class="row tab-pane  active">
                            </div>
                        </div>
                        <%--分页--%>
                        <%--  <div class="bottom-pagination-content clearfix mrgn-40">                      
                            <div class="pagination-button">
                                <ul class="pagination">
                                    <li id="pagination-previous-bottom" class="disabled"><a href="#"><i class="fa fa-angle-left"></i></a></li>
                                    <li class="active"><a href="#">1 <span class="sr-only">(current)</span></a></li>
                                    <li><a href="#">2 <span class="sr-only">(current)</span></a></li>
                                    <li id="pagination-next-bottom"><a href="#"><i class="fa fa-angle-right"></i></a></li>
                                </ul>
                            </div>
                            <div class="product-count">页码</div>
                        </div>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="Scripts/jquery-3.3.1.js"></script>
    <script src="AppData/layui/layui.js"></script>
    <script src="Scripts/Shopping/master.js"></script>
    <script src="Scripts/Shopping/pageindex.js"></script>
    <script>
        //获取Url参数   
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return decodeURI(r[2]); return null;
        }
        var Serch = function () {
            var title = getQueryString('value');
            console.log(title,"------------------------------------");
            ajax_request({
                url: "Aspx/ManagePages/groundinghandler.ashx?action=seach",
                data: { "Title": title },
                callback: function (e) {
                    if (e) {

                        var html = productList(e);
                        $("#list").append(html);
                        //console.log(html, "*------------");

                    }
                }
            });
        }
        Serch();

        $("#listshop").click(function () {
            Serch();
        });
    </script>
</asp:Content>
