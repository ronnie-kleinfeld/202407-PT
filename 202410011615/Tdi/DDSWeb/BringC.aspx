<%@ Page language="c#" Codebehind="BringC.aspx.cs" AutoEventWireup="false" Inherits="DDSWeb.BringC" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>BringC</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<SCRIPT>
		function ShowResults()
		{
		    window.parent.document.getElementById("txtWhat").value = document.getElementById("txtWhat").value;
		    window.parent.document.getElementById("txtResult1").value = document.getElementById("txtResult1").value;
		    window.parent.document.getElementById("txtResult2").value = document.getElementById("txtResult2").value;
		    window.parent.document.getElementById("txtResult3").value = document.getElementById("txtResult3").value;
		    window.parent.document.getElementById("txtResult").value = document.getElementById("txtResult").value;
		}
		</SCRIPT>
	</head>
	<body MS_POSITIONING="GridLayout" onload="ShowResults();">
		<input type="hidden" id="txtWhat" runat="server" NAME="txtWhat" value="">
		<input type="hidden" id="txtResult" runat="server" NAME="txtResult" value="">
		<input type="hidden" id="txtResult1" runat="server" NAME="txtResult1" value="">
		<input type="hidden" id="txtResult2" runat="server" NAME="txtResult2" value="">
		<input type="hidden" id="txtResult3" runat="server" NAME="txtResult3" value="">
	</body>
</html>
