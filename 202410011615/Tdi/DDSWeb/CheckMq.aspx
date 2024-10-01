<%@ Page language="c#" Codebehind="CheckMq.aspx.cs" AutoEventWireup="false" Inherits="DDSWeb.CheckMq" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>CheckMq</title>
		<META HTTP-EQUIV='Expires' CONTENT='-1'>
		<META HTTP-EQUIV='Pragma' CONTENT='no-cache'>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<SCRIPT>
		function ShowResults()
		{
		    window.parent.document.getElementById("hResult").value = document.getElementById("txtResult").value;
		    window.parent.wnet_answer();
		}
		</SCRIPT>
	</head>
	<body MS_POSITIONING="GridLayout" onload="ShowResults();">
		<input type="hidden" id="txtResult" runat="server" NAME="txtResult" value="">
	</body>
</html>
