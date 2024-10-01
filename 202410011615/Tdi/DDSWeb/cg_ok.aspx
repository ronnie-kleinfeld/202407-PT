<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cg_ok.aspx.cs" Inherits="DDSWeb.cg_ok" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script>
        function thefunction() {
            //alert("1=" + window.parent.document.location.href + '\r\n2=' + document.location.href);
            window.parent.document.getElementById("txtResultCG").disabled = false;
            window.parent.document.getElementById("txtResultCG").value = 'OK';
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
