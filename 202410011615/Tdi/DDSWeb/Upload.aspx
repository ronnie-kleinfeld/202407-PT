<%@ Page language="c#" Codebehind="Upload.aspx.cs" AutoEventWireup="false" Inherits="DDSWeb.Upload" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>Upload</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<SCRIPT>
		function ShowResults()
		{
		    try { //vk 10.17
		        window.parent.document.getElementById("HarchiveResult").value = document.getElementById("txtResult").value;
		        window.parent.ArchiveAfterSubmit();
		    } catch (e) { }
		}
        function ArchiveAfterBrowse() {
            //if (event.propertyName!="value") return;
            var s=document.getElementById("hArchiveFile").value;
            window.parent.document.getElementById(window.parent.document.getElementById("Harchive").value).title = s;
            var i;
		    for (i=s.length-1;i>=0;i--) {
			    if (s.substr(i,1)=="\\") {
			        s=s.substr(i+1);
			        break;
			    }
			}
		    window.parent.document.getElementById(window.parent.document.getElementById("Harchive").value).value = s;
		    window.parent.document.getElementById("C_ArchiveSubmit").disabled = (document.getElementById("hArchiveFile").value == "");
            //window.parent.document.getElementById("C_ArchiveSubmitR").disabled=(document.getElementById("hArchiveFile").value=="");
		    window.parent.document.getElementById(window.parent.document.getElementById("Harchive").value).disabled = false;
        }
		</SCRIPT>
	</head>
	<body MS_POSITIONING="GridLayout" onload="ShowResults();">
		<input type="hidden" id="txtResult" runat="server" NAME="txtResult" value="">
		<form runat=server>
		    <input type="hidden" id="hArchiveKey" runat="server" NAME="hArchiveKey" value="">
		    <input type="hidden" id="hArchiveName" runat="server" NAME="hArchiveName" value="">
		    <input type="hidden" id="hArchiveDescr" runat="server" NAME="hArchiveDescr" value="">
            <input type="hidden" id="hArchiveDB" runat="server" NAME="hArchiveDB" value=""><!-- vk 07.17 -->
            <input type="hidden" id="hArchiveMail" runat="server" NAME="hArchiveMail" value=""><!-- vk 10.17 -->
            <input type="hidden" id="hArchiveWhat" runat="server" NAME="hArchiveWhat" value=""><!-- vk 03.20 -->
            <input type="hidden" id="hArchiveText" runat="server" NAME="hArchiveText" value=""><!-- vk 09.18 -->
            <input type="hidden" id="hArchiveSystem" runat="server" NAME="hArchiveSystem" value=""><!-- vk 12.18 -->
            <input type="hidden" id="hArchiveDocType" runat="server" NAME="hArchiveDocType" value=""><!-- vk 07.17 -->
		    <input type="hidden" id="hArchiveReplace" runat="server" NAME="hArchiveReplace" value="">
		    <input type="file" id="hArchiveFile" runat="server" NAME="hArchiveFile" oninput="ArchiveAfterBrowse();">
		</form>
	</body>
</html>
