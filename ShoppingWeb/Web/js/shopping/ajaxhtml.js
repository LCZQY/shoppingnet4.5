
/**
 * 组合商品列表标签
 * @param {any} json 
 */
var prodotct_list = function (json) {
   
    $.each(json, function (i, val) {

        var str = $(['<li>',
            '<dl>',
            '<dt><a href=""><img src="' + val.icon + '" alt="" /></a></dt>',
            '<dd><a href="">' + val.title + '</a></dd>',
            '<dd><strong>￥' + val.price + '</strong></dd>',
            '<dd><a href=""><em>已有' + val.discuss + '人评价</em></a></dd>',
            '</dl>',
           '</li>'].join(""));
        $("#goodslist").append(str);
    });   
}




/**
 * 组合商品列表标签
 * @param {any} json 
 */
var prodotct_list = function (json) {

    $.each(json, function (i, val) {

        var str = $(['<li>',
            '<dl>',
            '<dt><a href=""><img src="' + val.icon + '" alt="" /></a></dt>',
            '<dd><a href="">' + val.title + '</a></dd>',
            '<dd><strong>￥' + val.price + '</strong></dd>',
            '<dd><a href=""><em>已有' + val.discuss + '人评价</em></a></dd>',
            '</dl>',
            '</li>'].join(""));
        $("#goodslist").append(str);
    });
}