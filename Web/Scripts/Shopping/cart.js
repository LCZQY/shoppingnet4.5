
//单个购物车源码
var cartConentHtml = function (options) {
    var image = '<tr><td class="product-thumbnail"><a href="##"><img src="' + options.path + '" alt=""></a></td>';
    var name = '<td class="product-name"><a href="#">"' + options.name + '"</a></td>';
    var price = '<td class="product-price"><span class="amount">￥"' + options.amount + '"</span></td>';
    var quantity = '<td class="product-quantity"><input value="' + options.quantity + '" type="text" disabled></td>'; //number
    var subtotal = '<td class="product-subtotal">￥"' + options.amount * options.quantity + '"</td>';
    var pj = '';
    if (options.states === "2") { pj = '<a href="##" name="' + options.id + '">评价</a>'; }
    var product = '<td class="product-remove">' + pj + '<a href="##" name="' + options.id + '">结算</a><a href="##" name="' + options.id + '">删除</a></td></tr>';
    return image + name + price + quantity + subtotal + product;
};


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


/*点击收藏*/
$("#favoritelist").click(function () {
    $("#title").text("我的收藏");
    $("#div").html($("#favio_table").html());

    //ajax_request({
    //    url: "Aspx/ManagePages/favoritehandler.ashx?action=list",
    //    data: { "UserId": userid },
    //    callback: function (e) {
    //        e = JSON.parse(e);
    //        console.log(e, "加入收藏成功！！！！！");
    //        if (e.code === 0) {
    //            carthtml(e.data);
    //        } else {

    //        }
    //    }
    //});
});


/*点击购物车*/
$("#cartlsit").click(function () {
    $("#title").text("我的购物车");
    $("#div").html($("#cart_table").html());

});


/*点击地址*/
$("#addreeslist").click(function () {
    $("#title").text("我的收货地址");
    $("#div").html($("#address_table").html());

});


/*点击评价*/
$("#appraiselist").click(function () {
    $("#title").text("我的评价");
    $("#div").html($("#appraise_table").html());

});