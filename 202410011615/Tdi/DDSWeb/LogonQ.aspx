<%@ Page language="c#" Codebehind="LogonQ.aspx.cs" AutoEventWireup="false" Inherits="DDSWeb.LogonQ" EnableViewState="False" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
	<HEAD runat="server">
		<title>Com-Display</title>
        <link rel="SHORTCUT ICON" href="pics/<%=sIcon%>">
		<META http-equiv="Content-Type" content="text/html; charset=UTF-8">
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<%=Application["META"]%>
		<LINK href="css/dds.css" type="text/css" rel="stylesheet">
        <script src = 'jquery/jquery-1.10.2.min.js'></script>
		<SCRIPT language="JAVASCRIPT" src="js/ddsLogon.js"></SCRIPT>

		<script>
		function checkjs() {
		    try {
		        afunction();
		    } catch (e) {
		        alert("You have a problem with JavaScript");
		    }
		}
		</script>
        <!-- vk 01.16 -->
        <!--
        <% if (sAntiJack=="1") { %>
        <style id="antiClickjack">body{display:none !important;}</style>
        <script>
        if (self === top){
            var antiClickjack = document.getElementById("antiClickjack");
            antiClickjack.parentNode.removeChild(antiClickjack);
            //document.getElementById("txtUser").focus();
        }else{
            top.location = self.location;
        }
        <% } %>
        </script>
        -->
	</HEAD>
	<BODY oncontextmenu="return false;" onkeydown=return false;" onfocus="return false;" style="BACKGROUND-COLOR: #f5f5f5" text="#000000" vLink="#000000" aLink="#000000" link="#000000" bgProperties="fixed" leftMargin="0" background="" topMargin="0" scroll="no" onload="checkjs();document.getElementById('txtWait').value=msg(22);btnOk_click(true,'q');" marginheight=" " marginwidth=" ">

		<form id="Attach" method="post" runat="server">
			<input name="Hpath_Comtec" id="Hpath_Comtec" type="hidden" value='<%=sComtec%>' >
			<INPUT id="hdnCodeFromFile" type="hidden" name="hdnCodeFromFile" runat="server">
			<INPUT id="hdnFileForCode" type="hidden" name="hdnFileForCode" runat="server"> <INPUT id="hdnIp" type="hidden" name="hdnIp" runat="server">
			<INPUT id="hdnGetIpWay" type="hidden" name="hdnGetIpWay" runat="server">
			<INPUT id="hdnFromOK" type="hidden" value="0" name="hdnFromOK" runat="server">
			<INPUT id="hdnServerName" type="hidden" value='<%=Request.ServerVariables["SERVER_NAME"]%>' name="hdnServerName">
			<INPUT id="hdnDetectJavaPath" type="hidden" name="hdnDetectJavaPath" value='http://<%=Request.ServerVariables["SERVER_NAME"]%>/jadvantage/detectjava1.html' >
			<INPUT id="hdnResult" type="hidden" name="hdnResult" runat="server"> <INPUT id="hdnReady" type="hidden" name="hdnReady" runat="server" /><!-- onpropertychange="if (this.value!='' && event.propertyName=='value') btnOk_continue();" -->
			<!-- <INPUT id="hdnCopyright" type="hidden" name="hdnCopyright" runat="server"> -->
			<INPUT id="hdnSso" type="hidden" name="hdnSso" runat="server" value="Logonq">
			<DIV style='DISPLAY:none'>
				<%=sAdditionalHtml%>
			</DIV>
            <!-- vk 08.17 -->
            <input class="TDText" style="width:100%;height:100%;font-size:72;text-align:center;background-color:transparent;" id="txtWait" name="txtWait">
            <INPUT id="hdnUser" type="hidden" name="hdnUser" runat="server">
            <INPUT id="hdnPassword" type="hidden" name="hdnPassword" runat="server">
            <asp:label id="lblMsg" runat="server" ForeColor="#C00000" EnableViewState="false"></asp:label>
		</form>

        <IFRAME id='ifrRefresh' name='ifrRefresh' src='Refresh.aspx' width='0' height='0'></IFRAME>
		<IFRAME id='oIFrame' name='oIFrame' src='Empty.htm' width='0' height='0'></IFRAME>
	</BODY>
</HTML>
