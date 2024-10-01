<%@ Page language="c#" Codebehind="BringS.aspx.cs" AutoEventWireup="false" Inherits="DDSWeb.BringS" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>BringS</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<SCRIPT>
		function ShowResults()
		{ //vk 06.24+
		    window.parent.document.getElementById("hResult").value = document.getElementById("txtResult").value;
            window.parent.streetlist2(document.getElementById("txtTarget").value);
		}
        </SCRIPT>
	</head>
	<body MS_POSITIONING="GridLayout" onload="ShowResults();">
		<input type="hidden" id="txtResult" runat="server" NAME="txtResult" value="">
		<input type="hidden" id="txtTarget" runat="server" NAME="txtTarget" value="">
	</body>
</html>
