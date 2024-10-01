<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cg_cancel.aspx.cs" Inherits="DDSWeb.cg_cancel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script>
    function thefunction() {
        window.parent.document.getElementById("Hv1_value").value = "999";
        window.parent.document.getElementById("Hv2_value").value = "הפעולה נעצרה מיוזמתך, אנא חזור למסך ראשי!";
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
