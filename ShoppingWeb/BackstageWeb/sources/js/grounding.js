﻿


var fileArry = [];//图片路径数组
var iconIndex = -1; //是否封面
var cateId = ""; //商品类型id

/**
 *多文件上传
 */
var uploadfile = function () {
    layui.use(['upload', 'jquery'], function () {
        var upload = layui.upload;
        var $ = layui.$;

        ////执行实例
        //var uploadInst = upload.render({
        //    elem: '#files'                //绑定元素
        //    , url: 'groundinghandler.ashx?action=upload'      //上传接口
        //    //*********************传输限制
        //    , size: 100                   //传输大小100k
        //    , exts: 'jpg|png|gif|'        //可传输文件的后缀
        //    , accept: 'file'              //video audio images
        //    //****************传输操作相关设置
        //    , data: { Parm1: "hello", Parm2: "world" }    //额外传输的参数
        //    , headers: { token: 'sasasasa' }                   //额外添加的请求头
        //    , auto: true                                 //自动上传,默认是打开的
        //    , bindAction: '#btnUpload'                    //auto为false时，点击触发上传
        //    , multiple: false                             //多文件上传
        //    //, number: 100                               //multiple:true时有效
        //    , done: function (res) {                      //传输完成的回调
        //        $('#myPic').attr("src", "../.." + res.src);
        //        //复值到后端
        //        $("#Photo").val("../.." + res.src);
        //    }
        //    , error: function () {                         //传输失败的回调
        //        //请求异常回调
        //    }
        //});


        //多文件列表示例 https://www.layui.com/doc/modules/upload.html
        var demoListView = $('#demoList')
            , uploadListIns = upload.render({
                elem: '#testList'               
                , url: WEBURL + '/api/file/upload'
                , accept: 'file'
                , multiple: true
                , auto: false
                , bindAction: '#testListAction'
                , choose: function (obj) {
                    var files = this.files = obj.pushFile(); //将每次选择的文件追加到文件队列
                    //读取本地文件
                    obj.preview(function (index, file, result) {                        
                        var tr = $([
                            '<tr id="upload-' + index + '">'                         
                            , '<td><img src=' + result+'  /></td>'
                            , '<td>' + file.name + '</td>'
                            , '<td>' + (file.size / 1024).toFixed(1) + 'kb</td>'
                            , '<td>等待上传</td>'
                            , '<td>'
                            , '<button class="layui-btn layui-btn-xs demo-reload layui-hide">重传</button>'
                            , '<input type="button" value="设为封面" class="layui-btn layui-btn-xs layui-hide demo-icon" />'
                            , '<button class="layui-btn layui-btn-xs layui-btn-danger demo-delete">删除</button>'
                            , '</td>'
                            , '</tr>'].join(''));
                        //设为封面
                        tr.find('.demo-icon').on('click', function () {
                            var tr = demoListView.find('tr#upload-' + index)
                                , tds = tr.children();
                            $(this).val("封面图");                                                        
                            $(".demo-icon").not($(this)).hide();
                            iconIndex = index.split('-')[1];
                            tds.eq(3).html('<ol style="color: #FF5722;"><li>1. 上传成功</li ><li>2. 封面设置成功</li></ol>');                 
                            return;
                        });
                        //单个重传
                        tr.find('.demo-reload').on('click', function () {
                            obj.upload(index, file);
                        });
                        //删除
                        tr.find('.demo-delete').on('click', function () {                            
                            delete files[index]; //删除对应的文件
                            tr.remove();
                            uploadListIns.config.elem.next()[0].value = ''; //清空 input file 值，以免删除后出现同名文件不可选
                        });
                        demoListView.append(tr);
                    });
                }
                , done: function (res, index, upload) {                                  
                    console.log(res.extension, "上传成功");
                    if (res.code === '0') { //上传成功
                        fileArry.push(res.extension);
                        var tr = demoListView.find('tr#upload-' + index)
                            , tds = tr.children();
                        tds.eq(3).html('<span style="color: #5FB878;">上传成功</span>');
                        tds.eq(4).find('.demo-icon').removeClass('layui-hide'); //显示重传
                        tds.eq(4).find('.demo-delete').attr("class", 'layui-hide'); //显示重传
                        return delete this.files[index]; //删除文件队列已经上传成功的文件                    
                    }
                    this.error(index, upload);
                }
                , error: function (index, upload) {
                    var tr = demoListView.find('tr#upload-' + index)
                        , tds = tr.children();
                    tds.eq(3).html('<span style="color: #FF5722;">上传失败</span>');
                    tds.eq(4).find('.demo-reload').removeClass('layui-hide'); //显示重传
                }
            });

    });
}


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
        if (iconIndex === -1) {
            layer.msg("必须设置一张封面图");
            return false;
        }
        console.log(fileArry, "文件列表是");
        console.log(fileArry[iconIndex], "封面是", iconIndex);        
        var fileReqesut = [];
        $.each(fileArry, function (i, val) {
            var object = new Object();
            fileArry[iconIndex] === val ? object.isIcon = true : object.isIcon = false;
            object.path = val;
            fileReqesut.push(object);
        });
        var json_file = JSON.stringify(fileReqesut);       
        data.field.cateId = cateId;
        data.field.icon = fileArry[iconIndex];
        data.field.files = [];
        data.field.files.push(json_file);
        console.log(data.field, "add");

        ajax_request({
            url: WEBURL + "/api/product/edit",
            data: data.field,
            callback: function (e) {
               // e = JSON.parse(e);
                if (e.code === '0') {
                    layer.msg("上传成功啦");
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
 * 选择树
 */
var select_tree = function () {
    //学习网址： https://fly.layui.com/extend/eleTree/#doc
    layui.config({
        base: "../../sources/layui/lay/mymodules/"
    }).use(['jquery', 'table', 'eleTree', 'code', 'form', 'slider'], function () {
        var $ = layui.jquery;
        var eleTree = layui.eleTree;
        var el5;
        $("[name='type']").on("click", function (e) {
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
                            emptText: "暂无数据",
                            highlightCurrent: true,
                            // contentType: 'json',
                            data: data.extension,
                            //url: WEBURL + "/api/type/tree",
                            //headers: {},
                            defaultExpandAll: false,
                            expandOnClickNode: false,

                        });
                    }
                });
            }
            $(".ele5").toggle();
        })
        eleTree.on("nodeClick(data5)", function (d) {
            $("[name='type']").val(d.data.currentData.label)
            cateId = d.data.currentData.id;
            //console.log(d.data.currentData.label, d.data.currentData.id);
            $(".ele5").hide();
        })
        $(document).on("click", function () {
            $(".ele5").hide();
        })
    });
}
//选择树
select_tree();
//上传图片
uploadfile();