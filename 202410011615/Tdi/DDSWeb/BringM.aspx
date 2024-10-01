<%@ Page language="c#" Codebehind="BringM.aspx.cs" AutoEventWireup="false" Inherits="DDSWeb.BringM" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>BringM</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<SCRIPT>
		function ShowResults()
		{
		    window.parent.document.getElementById("hWhat").value = document.getElementById("txtWhat").value;
		    window.parent.document.getElementById("hHeight").value = document.getElementById("txtHeight").value;
		    window.parent.document.getElementById("hWidth").value = document.getElementById("txtWidth").value;
		    window.parent.document.getElementById("hCurrentText").value = document.getElementById("txtCurrentText").value;
		    window.parent.document.getElementById("hResult").value = document.getElementById("txtResult").value;
		    window.parent.modellist2();
		}
		</SCRIPT>
	</head>
	<body MS_POSITIONING="GridLayout" onload="ShowResults();">
		<input type="hidden" id="txtWhat" runat="server" NAME="txtWhat" value="">
		<input type="hidden" id="txtResult" runat="server" NAME="txtResult" value="">
		<input type="hidden" id="txtHeight" runat="server" NAME="txtHeight" value="">
		<input type="hidden" id="txtWidth" runat="server" NAME="txtWidth" value="">
		<input type="hidden" id="txtCurrentText" runat="server" NAME="txtCurrentText" value="">
	</body>
</html>
