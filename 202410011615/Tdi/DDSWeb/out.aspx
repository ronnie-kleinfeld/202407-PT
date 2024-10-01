<%@ Page language="c#" Codebehind="out.aspx.cs" AutoEventWireup="false" Inherits="DDSWeb._out" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>Please don't close this window</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<script lang="javascript">
	function body_load()
	{
	    opener = window.open(document.getElementById("hdnOpen").value);
		opener = window.open('empty.htm', '_self');
		this.close();
	}
		</script>
	</head>
	<body onload="body_load();">
	<input name="hdnOpen" id="hdnOpen" type="hidden" value='<%=sOpen%>'>
	</body>
</html>
