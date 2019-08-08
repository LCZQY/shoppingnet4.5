
//单个购物车源码
var cartConentHtml = function (options) {
    var image = '<tr id="' + options.id +'" name="' +options.proid+'"><td class="product-thumbnail"><a href="##"><img src="' + options.path + '" alt=""></a></td>';
    var name = '<td class="product-name"><a href="#">' + options.name + '</a></td>';
    var price = '<td class="product-price"><span class="amount">￥' + options.price + '</span></td>';
    var quantity = '<td class="product-quantity"><input value="' + options.quantity + '" type="text" disabled></td>'; //number
    var subtotal = '<td class="product-subtotal">￥' + options.amount + '</td>';
    var html = '';
    if (options.states === 2) { html = '<a href="##" class="Receiving" name="' + options.id + '">确认收货</a>'; }  
    if (options.states === 0) { html += '<a href="##" class="paymoeny" name="' + options.id + '">结算</a>'; }
    if (options.states === 1) { html += '<span style="color:Red;">待发货</span>'; }
    if (options.states === 3) { html += '<a href="##" class="pingjia" name="' + options.id + '">立即评价</a>'; }
    if (options.states === 4) { html += '<span style="color:Red;">已评价</span>';}
    var product = '<td class="product-remove">' + html + ' <a href="##" name="' + options.id + '">删除</a></td></tr>';
    return image + name + price + quantity + subtotal + product;
};


//单个收藏源码
var favoriteConentHtml = function (options) {

    var img = '<tr><td class="product-thumbnail"><a href="#"><img src="' + options.path + '" alt=""></a></td>';
    var name = '<td class="favio-name"><a href="#">' + options.title + '</a></td>';
    var price = '<td class="favio-price"><span class="amount">￥' + options.price + '</span></td>';
    var quantity = '<td class="favio-quantity">' + options.date + '</td>';
    var action = '<td class="favio-remove"><a href="##"  name="' + options.id + '">删除</a></td></tr>';
    return img + name + price + quantity + action;
};


//我的地址
var addreesConentHtml = function (options) {
    var consingnee = '<tr><td class="address-quantity">' + options.consingnee + '</td>';
    var complete = '<td class="address-quantity">' + options.complete + '</td>';
    var phone = '<td class="address-quantity">' + options.phone + '</td><td class="address-remove"><a href="#">删除</a></td></tr>';
    return consingnee + complete + phone;
};




//我的评价
var pingjiaContentHmtl = function (options) {
    var img = '<tr><td class="product-thumbnail"><a href="#"><img src="' + options.path + '" alt=""></a></td>';
    var name = '<td class="favio-name"><a href="#">' + options.title + '</a></td>';
    var dengji = '<td class="favio-price"><span class="amount">' + options.dengji + '</span></td>';
    var conent = '<td class="favio-quantity">' + options.conent + '</td>';
    var quantity = '<td class="favio-quantity">' + options.date + '</td>';
    var action = '<td class="favio-remove"><a href="##"  name="' + options.id + '">删除</a></td></tr>';
    return img + name + dengji + conent + quantity + action;
};


//我的购物车
var MyCartList = function (obj) {
    var list = obj;
    var html = '';
    var totals = 0;
    $.each(list, function (index, item) {
        totals += item.Total;
        html += cartConentHtml({
            path: item.Path,
            name: item.Title,
            quantity: item.Quantity,
            amount: item.Total,
            states: item.States,
            id: item.OrdersId,
            price: item.Price,
            proid: item.ProductId,
        });
    });
    $("#tbody").append(html);
    return totals;
};


//我的购藏
var MyFacoiriteList = function (obj) {
    var list = obj;
    var html = '';

    $.each(list, function (index, item) {
        html += favoriteConentHtml({
            path: item.Path,
            title: item.Title,
            date: item.FavoriDate,
            id: item.ProductId,
            price: item.Price
        });
    });
    $("#favio_tbody").append(html);
};



//我的购藏
var MyAdreesList = function (obj) {
    var list = obj;
    var html = '';

    $.each(list, function (index, item) {

        html += favoriteConentHtml({
            path: item.Path,
            title: item.Title,
            date: item.FavoriDate,
            id: item.ProductId,
            price: item.Price
        });
    });
    $("#address_tbody").append(html);
};



//我的评价
var MyPingjiaList = function (obj) {
    var list = obj;
    var html = '';

    $.each(list, function (index, item) {

        html += pingjiaContentHmtl({
            path: item.Path,
            title: item.Title,
            date: item.FavoriDate,
            id: item.ProductId,
            price: item.Price,
            dengji: item.Grade === 0 ? "很好" : item.Grade === 1 ? "好" : "不好",
            conent : item.Content
        });
    });
    $("#appraise_tbody").append(html);
};



/*点击收藏*/
$("#favoritelist").click(function () {
    $("#title").text("我的收藏");
    $("#div").html($("#favio_table").html());
   
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