/**上传图片 */
var uploadfile = function () {
    layui.use(['upload', 'jquery'], function () {
        var upload = layui.upload;
        var $ = layui.$;
        //执行实例
        var uploadInst = upload.render({
            elem: '#files'                //绑定元素
            , url: 'groundinghandler.ashx?action=upload'      //上传接口
            //*********************传输限制
            , size: 100                   //传输大小100k
            , exts: 'jpg|png|gif|'        //可传输文件的后缀
            , accept: 'file'              //video audio images
            //****************传输操作相关设置
            , data: { Parm1: "hello", Parm2: "world" }    //额外传输的参数
            , headers: { token: 'sasasasa' }                   //额外添加的请求头
            , auto: true                                 //自动上传,默认是打开的
            , bindAction: '#btnUpload'                    //auto为false时，点击触发上传
            , multiple: false                             //多文件上传
            //, number: 100                               //multiple:true时有效
            , done: function (res) {                      //传输完成的回调
                $('#myPic').attr("src", "../.." + res.src);
                //复值到后端
                $("#Photo").val("../.." + res.src);
            }
            , error: function () {                         //传输失败的回调
                //请求异常回调
            }
        });
    });
}

//上传图片
uploadfile();

layui.use(['form', 'layedit', 'laydate'], function () {

    var form = layui.form
        , layer = layui.layer
        , layedit = layui.layedit
        , laydate = layui.laydate;
    //日期控件
    laydate.render({
        elem: '#test11' //指定元素
    });
    //创建一个编辑器
    var editIndex = layedit.build('LAY_demo_editor');

    //监听提交
    form.on('submit(demo1)', function (data) {
        console.log(data.field, "11111111111111111最后上传的是");
        //layer.alert(JSON.stringify(data.field), {
        //    title: '最终的提交信息'
        //})
        ajax_request({
            url: "groundinghandler.ashx?action=add",
            data: data.field,
            callback: function (e) {
                e = JSON.parse(e);
                if (e.code == 0) {
                    layer.msg(e.msg);
                    setTimeout(function () {
                        window.parent.location.reload();
                    }, 2000);

                } else {
                    layer.msg(e.msg);
                }
            }
        });
        return false;
    });
});

//加载父级商品类型
ajax_request({
    url: WEBURL + "/api/type/list",
    data: { pageIndex: 0, pageSize: 1000 },
    callback: function (data) {
        if (data.code === "0") {
            var json = data.extension;
            var html = ' <select name="id">';
            $.each(json, function (index, item) {
                if (index === 0) html += ' <option value="">商品类型</option>';            
                html += '<optgroup label="' + item.cateName + '">';
                if (item.children !== null)
                    $.each(item.children, function (i, childname) {
                        html += '<option value="' + childname.id + '">' + childname.cateName + '</option>';
                    });
                html += ' </optgroup>';
            });
            html += '</select>';
            $("#appendsSelect").html(html);
        }
    }
});

