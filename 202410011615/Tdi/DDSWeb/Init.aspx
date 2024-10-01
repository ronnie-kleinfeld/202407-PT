<%@ Page language="c#" Codebehind="Init.aspx.cs" AutoEventWireup="false" Inherits="DDSWeb.Init" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <title>Init</title>
    <meta charset="utf-8" />
    <meta content="-1" http-equiv="Expires" />
    <meta content="no-cache" http-equiv="Pragma" />
		<script>
		function ShowResults()
		{
		    window.parent.goon();
		}
		</script>
	</head>
	<body onload="ShowResults();">
	</body>
</html>
