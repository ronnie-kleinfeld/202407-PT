<%@ Page language="c#" Codebehind="Sql.aspx.cs" AutoEventWireup="false" Inherits="DDSWeb.Sql" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>Sql</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<SCRIPT>
		function ShowResults()
		{
		    window.parent.document.getElementById("txtWhatSql").value = document.getElementById("txtWhat").value;
		    window.parent.document.getElementById("txtResultSql").value = document.getElementById("txtResult").value;
		    window.parent.document.getElementById("Hsynchr").value = document.getElementById("txtSynchr").value;
		    window.parent.Sql2();
		}
		</SCRIPT>
	</head>
	<body MS_POSITIONING="GridLayout" onload="ShowResults();">
		<input type="hidden" id="txtWhat" runat="server" NAME="txtWhat" value="">
		<input type="hidden" id="txtResult" runat="server" NAME="txtResult" value="">
        <input type="hidden" id="txtSynchr" runat="server" NAME="txtSynchr" value="">
	</body>
</html>
