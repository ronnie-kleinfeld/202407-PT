<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bring.aspx.cs" Inherits="DDSWeb.Bring" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
		<title>Bring</title>
		
		<script>
		function ShowResults()
		{
		    window.parent.document.getElementById("txtWhat").value = document.getElementById("txtWhat").value;
		    window.parent.document.getElementById("txtResult").value = document.getElementById("txtResult").value;
		    window.parent.CopyList();
		}
		</script>
	</head>
	<body  onload="ShowResults();">
		<input type="hidden" id="txtWhat" runat="server"  value=""/>
		<input type="hidden" id="txtResult" runat="server"  value=""/>
	</body>
</html>
