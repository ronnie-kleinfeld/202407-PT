<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <%=Application["META"]%>

    <link href="assets/bootstrap5/css/bootstrap.css" rel="stylesheet" />
    <script src="jquery/jquery-1.10.2.min.js"></script>
    <script src="assets/bootstrap5/js/bootstrap.bundle.min.js"></script>
    
<script>
    function WaitAndShow() {
        var dd = null;
        try {
            d = document.getElementById("ifrGet").contentWindow.document;
            if (d.readyState != 'complete') {
                setTimeout("WaitAndShow();", 100);
                return;
            }
            dd = d.documentElement;
        } catch (e) {
            alert(e.message);
        }

    //document.all.ifrShow.style.pixelHeight=document.body.clientHeight;
    //document.all.ifrShow.style.pixelWidth = document.body.clientWidth;

    $("#ifrShow").height($(document).height());
    $("#ifrShow").width($(document).width());

    s = document.getElementById("ifrShow").contentWindow.document;
    //s.documentElement.outerHTML = dd.outerHTML;
	s.open();
	s.clear();
	s.write(dd.outerHTML);
	s.close();

if (dd) {
    var oInput = dd.getElementsByTagName("INPUT");
    for (var i = 0; i < oInput.length; i++) {
    //for (var o in obj) { //vk 06.11
    //    if (o != "length") {
    //        var oo = dd.getElementById(o);
        var oo = oInput[i];
        if (oo) //vk 09.11
            if (oo.type == "password")
                s.getElementById(oo.id).value = oo.value;
    //    }
    }
    document.title = dd.title;
}
}
</script><title></title></head>
<body onload="WaitAndShow();" style="padding:0px;margin:0px;overflow:hidden;">
<iframe id=ifrShow style="padding:0px;margin:0px;width:100%;height:100%;" src="Empty.htm" frameborder=0></iframe>
<iframe id=ifrGet style="visibility:hidden;" src="screen.aspx"></iframe>
<!-- input type=hidden id=trigger value="0" -->
<object ID='WebBrowser1' CLASSID='CLSID:8856F961-340A-11D0-A96B-00C04FD705A2' style='display:none;'></object>
</body></html>
<SCRIPT>
 $(window).on('unload', function () {
     var xmlhttp;
     if (window.XMLHttpRequest) {
         xmlhttp = new XMLHttpRequest();
     } else if (window.ActiveXObject) {
         xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
     } else {
         return;
     }
     xmlhttp.open("GET", "Close.aspx", true);
     xmlhttp.send(null);
 });</SCRIPT>
