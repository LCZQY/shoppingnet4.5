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

/**
 * 选择树的加载
 * */
var select_tree = function () {
    //学习网址： https://fly.layui.com/extend/eleTree/#doc
    layui.config({
        base: "../../sources/layui/lay/mymodules/"
    }).use(['jquery', 'table', 'eleTree', 'code', 'form', 'slider'], function () {
        var $ = layui.jquery;
        var eleTree = layui.eleTree;
        var el5;
        $("[name='title']").on("click", function (e) {
            e.stopPropagation();
            if (!el5) {
                $.ajax({
                    type: "get",
                    url: WEBURL + "/api/type/tree",
                    dataType: 'json',
                    success: function (data) {
                        el5 = eleTree.render({
                            elem: '.ele5',
                            method: "GET",
                            // contentType: 'json',
                            data: data.extension,
                            //url: WEBURL + "/api/type/tree",
                            //headers: {},

                            defaultExpandAll: true,
                            expandOnClickNode: false,
                            highlightCurrent: true
                        });
                    }
                });
            }
            $(".ele5").toggle();
        })
        eleTree.on("nodeClick(data5)", function (d) {
            $("[name='title']").val(d.data.currentData.label)
            console.log(d.data.currentData.label, d.data.currentData.id);
            $(".ele5").hide();
        })
        $(document).on("click", function () {
            $(".ele5").hide();
        })
    });
}
//选择树
select_tree();

