<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cg_cancel2.aspx.cs" Inherits="DDSWeb.cg_cancel2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script>
    function thefunction() {
        window.parent.document.getElementById("txtResultCG").disabled = false;
        window.parent.document.getElementById("txtResultCG").value = 'CANCEL';
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
