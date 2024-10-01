<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cg_notok.aspx.cs" Inherits="DDSWeb.cg_notok" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script>
    function thefunction() {
        var s = window.location.search.substr(1).split("&");
        for (var j = 0; j < s.length; j++) {
            var v = s[j].split("=");
            switch (v[0]) {
                case "ErrorCode":
                    window.parent.document.getElementById("Hv1_value").value = v[1];
                    break;
                case "ErrorText":
                    window.parent.document.getElementById("Hv2_value").value = decodeURIComponent(v[1]);
                    break;
            }
        }
        window.parent.document.getElementById("txtResultCG").disabled = false;
        window.parent.document.getElementById("txtResultCG").value = 'ERROR';
        window.parent.CG2();
    }
    </script>
</head>
<body onload="thefunction();">
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
