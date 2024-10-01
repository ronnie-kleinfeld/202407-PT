<%@ Page language="c#" Codebehind="Call.aspx.cs" AutoEventWireup="false" Inherits="DDSWeb.Call" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>Call</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<SCRIPT>
		function ShowResults()
		{
			//window.parent.document.all.txtWhatCG.value = document.all.txtWhat.value;
            for (var i=9;i>=1;i--) {
                window.parent.document.getElementById("Hu" + i + "_value").value = document.getElementById("txtResultU" + i).value;
                window.parent.document.getElementById("Hv" + i + "_value").value = document.getElementById("txtResultV" + i).value;
                window.parent.document.getElementById("Hw" + i + "_value").value = document.getElementById("txtResultW" + i).value;
		    }
            window.parent.document.getElementById("txtResultCG").value = document.getElementById("txtResult").value;
            window.parent.CG2();
		}
		</SCRIPT>
	</head>
	<body MS_POSITIONING="GridLayout" onload="ShowResults();">
		<!-- <input type="hidden" id="txtWhat" runat="server" NAME="txtWhat" value=""> -->
		<input type="hidden" id="txtResult" runat="server" NAME="txtResult" value="">
		<input type="hidden" id="txtResultU1" runat="server" NAME="txtResultU1" value="">
		<input type="hidden" id="txtResultU2" runat="server" NAME="txtResultU2" value="">
		<input type="hidden" id="txtResultU3" runat="server" NAME="txtResultU3" value="">
		<input type="hidden" id="txtResultU4" runat="server" NAME="txtResultU4" value="">
		<input type="hidden" id="txtResultU5" runat="server" NAME="txtResultU5" value="">
		<input type="hidden" id="txtResultU6" runat="server" NAME="txtResultU6" value="">
		<input type="hidden" id="txtResultU7" runat="server" NAME="txtResultU7" value="">
		<input type="hidden" id="txtResultU8" runat="server" NAME="txtResultU8" value="">
		<input type="hidden" id="txtResultU9" runat="server" NAME="txtResultU9" value="">
		<input type="hidden" id="txtResultV1" runat="server" NAME="txtResultV1" value="">
		<input type="hidden" id="txtResultV2" runat="server" NAME="txtResultV2" value="">
		<input type="hidden" id="txtResultV3" runat="server" NAME="txtResultV3" value="">
		<input type="hidden" id="txtResultV4" runat="server" NAME="txtResultV4" value="">
		<input type="hidden" id="txtResultV5" runat="server" NAME="txtResultV5" value="">
		<input type="hidden" id="txtResultV6" runat="server" NAME="txtResultV6" value="">
		<input type="hidden" id="txtResultV7" runat="server" NAME="txtResultV7" value="">
		<input type="hidden" id="txtResultV8" runat="server" NAME="txtResultV8" value="">
		<input type="hidden" id="txtResultV9" runat="server" NAME="txtResultV9" value="">
		<input type="hidden" id="txtResultW1" runat="server" NAME="txtResultW1" value="">
		<input type="hidden" id="txtResultW2" runat="server" NAME="txtResultW2" value="">
		<input type="hidden" id="txtResultW3" runat="server" NAME="txtResultW3" value="">
		<input type="hidden" id="txtResultW4" runat="server" NAME="txtResultW4" value="">
		<input type="hidden" id="txtResultW5" runat="server" NAME="txtResultW5" value="">
		<input type="hidden" id="txtResultW6" runat="server" NAME="txtResultW6" value="">
		<input type="hidden" id="txtResultW7" runat="server" NAME="txtResultW7" value="">
		<input type="hidden" id="txtResultW8" runat="server" NAME="txtResultW8" value="">
		<input type="hidden" id="txtResultW9" runat="server" NAME="txtResultW9" value="">
	</body>
</html>
