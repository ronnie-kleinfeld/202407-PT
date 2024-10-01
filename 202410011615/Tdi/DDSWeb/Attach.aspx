<%@ Page language="c#" Codebehind="Attach.aspx.cs" AutoEventWireup="false" Inherits="DDSWeb.Attach" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>Attach</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<SCRIPT>
		function ShowResults()
		{
		    window.parent.document.getElementById("hdnResult").value = document.getElementById("txtResult").value;
			//window.parent.document.all.hdnReady.value = "!";
			window.parent.btnOk_continue();
		}
		</SCRIPT>
	</head>
	<body MS_POSITIONING="GridLayout" onload="ShowResults();">
		<input type="hidden" id="txtResult" runat="server" NAME="txtResult" value="">
		<input type="hidden" id="txtReady" runat="server" NAME="txtReady" value="">
	</body>
</html>
