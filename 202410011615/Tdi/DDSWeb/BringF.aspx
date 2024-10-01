<%@ Page language="c#" Codebehind="BringF.aspx.cs" AutoEventWireup="false" Inherits="DDSWeb.BringF" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>BringF</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<SCRIPT>
		function ShowResults()
		{
		    window.parent.document.getElementById("hHeight_" + document.getElementById("txtTarget").value).value = document.getElementById("txtHeight").value;
		    window.parent.document.getElementById("hWidth_" + document.getElementById("txtTarget").value).value = document.getElementById("txtWidth").value;
		    window.parent.document.getElementById("hCurrentText_" + document.getElementById("txtTarget").value).value = document.getElementById("txtCurrentText").value;
		    window.parent.document.getElementById("hResult_" + document.getElementById("txtTarget").value).value = document.getElementById("txtResult").value;
			window.parent.flexlist2(document.getElementById("txtTarget").value);
		}
		</SCRIPT>
	</head>
	<body MS_POSITIONING="GridLayout" onload="ShowResults();">
		<input type="hidden" id="txtTarget" runat="server" NAME="txtTarget" value="">
		<input type="hidden" id="txtResult" runat="server" NAME="txtResult" value="">
		<input type="hidden" id="txtHeight" runat="server" NAME="txtHeight" value="">
		<input type="hidden" id="txtWidth" runat="server" NAME="txtWidth" value="">
		<input type="hidden" id="txtCurrentText" runat="server" NAME="txtCurrentText" value="">
	</body>
</html>
