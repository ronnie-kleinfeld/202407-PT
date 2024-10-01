<%@ Page language="c#" Codebehind="Ping.aspx.cs" AutoEventWireup="false" Inherits="DDSWeb.Ping" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <title>Ping</title>
    <meta charset="utf-8" />
    <meta content="-1" http-equiv="Expires" />
    <meta content="no-cache" http-equiv="Pragma" />
		<script>
		function ShowResults()
		{
		    window.parent.document.getElementById("PingNumber").value = document.getElementById("PingNumber").value;
		    window.parent.bInPing = false;
		}
		</script>
	</head>
	<body onload="ShowResults();">
		<input type="hidden" id="PingNumber" runat="server" name="PingNumber" value=""/>
	</body>
</html>
