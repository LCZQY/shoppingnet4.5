<%@ Page Title="Change官网-后台-后台主页" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="ManageIndex.aspx.cs" Inherits="System.Web.Aspx.ManageIndex" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Content/Shopping/font.css" rel="stylesheet" />
    <link href="../AppData/layui/css/layui.css" rel="stylesheet" />
    <link href="../Content/Shopping/weadmin.css" rel="stylesheet" />
    <link href="../Content/Shopping/weadmin.css" rel="stylesheet" />

    <!-- 顶部开始 -->
    <div class="container">
        <div class="logo">
            <a href="./index.html">Change 商城后台管理</a>
        </div>
        <div class="left_open">
            <i title="展开左侧栏" class="iconfont">&#xe699;</i>
        </div>
        <ul class="layui-nav right" lay-filter="">
            <li class="layui-nav-item">
                <a id="userName" href="javascript:;">欢迎：Admin</a>
                <dl class="layui-nav-child">
                    <!-- 二级菜单 -->
                    <dd>
                        <a onclick="WeAdminShow('个人信息','http://www.baidu.com')">个人信息</a>
                    </dd>
                    <dd>
                        <a onclick="WeAdminShow('切换帐号','./login.html')">切换帐号</a>
                    </dd>
                    <dd>
                        <a class="loginout" href="./login.html">退出</a>
                    </dd>
                </dl>
            </li>
            <li class="layui-nav-item to-index">
                <a runat="server" href="~" target="_blank">前台首页</a>
            </li>
        </ul>
    </div>
    <!-- 顶部结束 -->
    <!-- 中部开始 -->
    <!-- 左侧菜单开始 -->
    <div class="left-nav">
        <div id="side-nav">
            <ul class="nav" id="nav">
                <li id="menu1" class=""><a _href=""><i class="iconfont"></i><cite>用户管理</cite><i class="iconfont nav_right"></i></a><ul class="sub-menu" style="display: none;">
                    <li id="menu2"><a _href="ManagePages/user.html"><i class="iconfont"></i><cite>用户列表</cite></a></li>
                </ul>
                </li>
                <li id="menu5" class=""><a _href=""><i class="iconfont"></i><cite>商品管理</cite><i class="iconfont nav_right"></i></a><ul class="sub-menu" style="display: none;">
                    <li id="menu4"><a _href="ManagePages/grounding.html"><i class="iconfont"></i><cite>商品上架</cite></a></li>
                    <li id="menu3"><a _href="ManagePages/type.html"><i class="iconfont"></i><cite>商品类型</cite></a></li>
                </ul>
                </li>
                <li id="menu8" class="open"><a _href=""><i class="iconfont"></i><cite>促销管理</cite><i class="iconfont nav_right"></i></a><ul class="sub-menu" style="display: block;">
                    <li id="menu9"><a _href="ManagePages/news.html"><i class="iconfont"></i><cite>促销咨询列表</cite></a></li>
                </ul>
                </li>
                <li id="menu11" class=""><a _href=""><i class="iconfont"></i><cite>订单管理</cite><i class="iconfont nav_right"></i></a><ul class="sub-menu" style="display: none; height: 45px; padding-top: 0px; margin-top: 0px; padding-bottom: 0px; margin-bottom: 0px;">
                    <li id="menu12"><a _href="ManagePages/order.html"><i class="iconfont"></i><cite>订单列表</cite></a></li>
                </ul>
                </li>
                <li id="menu13" class=""><a _href=""><i class="iconfont"></i><cite>管理员权限</cite><i class="iconfont nav_right"></i></a><ul class="sub-menu" style="display: none; height: 45px; padding-top: 0px; margin-top: 0px; padding-bottom: 0px; margin-bottom: 0px;">
                    <li id="menu14"><a _href="ManagePages/adminuser.html"><i class="iconfont"></i><cite>管理员列表</cite></a></li>
                </ul>
                </li>
            </ul>
        </div>

    </div>
    <!-- <div class="x-slide_left"></div> -->
    <!-- 左侧菜单结束 -->
    <!-- 右侧主体开始 -->
    <div class="page-content">
        <div class="layui-tab tab" lay-filter="wenav_tab" id="WeTabTip" lay-allowclose="true">
            <ul class="layui-tab-title" id="tabName">
                <li>我的桌面</li>
            </ul>
            <div class="layui-tab-content">
                <div class="layui-tab-item layui-show">
                    <iframe src="ManagePages/order.html" frameborder="0" scrolling="yes" class="weIframe"></iframe>
                </div>
            </div>
        </div>
    </div>
    <div class="page-content-bg"></div>
    <!-- 右侧主体结束 -->
    <!-- 中部结束 -->
    <!-- 底部开始 -->
    <div class="footer">
        <div class="copyright">Copyright ©2019 Change 商城后台管理V1.0 </div>
    </div>
    <!-- 底部结束 -->
    <script src="../Scripts/jquery-3.3.1.js"></script>
    <script src="../AppData/layui/layui.js"></script>


    <script type="text/javascript">
        //			layui扩展模块的两种加载方式-示例
        //		    layui.extend({
        //			  admin: '{/}../../static/js/admin' // {/}的意思即代表采用自有路径，即不跟随 base 路径
        //			});
        //			//使用拓展模块
        //			layui.use('admin', function(){
        //			  var admin = layui.admin;
        //			});
        layui.config({ //加载js资源
            base: '../Scripts/Shopping/'
            , version: '101100'
        }).extend({ //设定模块别名
            admin: 'admin'
            , menu: 'menu'
        });
        layui.use(['jquery', 'admin', 'menu'], function () {
            var $ = layui.jquery,
                admin = layui.admin,
                menu = layui.menu;
            $(function () {

                var login = JSON.parse(localStorage.getItem("login"));
                console.log(login, "！！！！！");
                if (login) {
                    if (login === 0) {
                        window.location.href = './login.html';
                        return false;
                    } else {
                        $("#userName").text(login.user);
                        /**简单的处理下权限管理*/
                        if (login.r === "0") {

                            //menu.getMenu('../json/menu2.json');
                        } else if (login.r === "1") {
                            $("#menu13").hide();
                           // menu.getMenu('../json/menu.json');
                        }
                        else {
                            $(".weIframe").hide();
                        }
                        return false;
                    }
                } else {
                    window.location.href = './login.html';
                    return false;
                }
            });
        });

        //退出
        $(".loginout").on("click", function () {
            localStorage.clear();
        });


    </script>

    <!--Tab菜单右键弹出菜单-->
    <ul class="rightMenu" id="rightMenu">
        <li data-type="fresh">刷新</li>
        <li data-type="current">关闭当前</li>
        <li data-type="other">关闭其它</li>
        <li data-type="all">关闭所有</li>
    </ul>
</asp:Content>
