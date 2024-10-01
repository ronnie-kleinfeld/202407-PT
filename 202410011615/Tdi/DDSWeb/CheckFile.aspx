<%@ Page language="c#" Codebehind="CheckFile.aspx.cs" AutoEventWireup="false" Inherits="DDSWeb.CheckFile" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>CheckFile</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<SCRIPT>
		function ShowResults()
		{
		    window.parent.document.getElementById("HarchiveCheckResult").value = document.getElementById("txtResult").value;
		    window.parent.ArchiveAfterCheck();
		}
		</SCRIPT>
	</head>
	<body MS_POSITIONING="GridLayout" onload="ShowResults();">
        <!-- <input type="hidden" id="hArchiveWhat" runat="server" NAME="hArchiveWhat" value=""> vk 03.20 -->
		<input type="hidden" id="txtResult" runat="server" NAME="txtResult" value="">
	</body>
</html>
