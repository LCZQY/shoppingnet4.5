/*
@功能：列表页js
@作者：zhengqiangyong
@时间：2013年11月13日
*/

$(function(){
	$(".child h3").click(function(){
		$(this).toggleClass("on").parent().find("ul").toggle();
	
	});
});

$(function () {  
    ajax_request({
        url: WEBURL + "/api/product/list",
        data: { pageIndex: 0, pageSize: 10 },
        callback: function (e) {
            if (e.code === '0') {               
                prodotct_list(e.extension);                           
            } else {
                alert("商品列表查询失败");
            }
        }
    },"POST");
});