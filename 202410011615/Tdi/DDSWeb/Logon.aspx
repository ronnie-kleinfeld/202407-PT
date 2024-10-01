<%@ Page language="c#" Codebehind="Logon.aspx.cs" AutoEventWireup="false" Inherits="DDSWeb.Logon" EnableViewState="False" %>
<!DOCTYPE html>
<html <%=sDir%>><!-- lang="he" dir="rtl" -->

<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <SCRIPT>
        history.forward();
    </SCRIPT>

    <link rel="stylesheet" href="./assets/styles/login.css" />
    <link href="assets/bootstrap5/css/bootstrap.css" rel="stylesheet" />
		<title>MyTitle</title>
		<link rel="SHORTCUT ICON" href="pics/<%=sIcon%>">
		<%=Application["META"]%>
		<SCRIPT language="JAVASCRIPT" src="./assets/scripts/jquery-3.3.1.slim.min.js"></SCRIPT>
		<SCRIPT language="JAVASCRIPT" src="js/ddsLogon.js"></SCRIPT>
		<script>
            //history.forward();
            function checkjs() {
                try {
                    afunction();
                } catch (e) {
                    alert("You have a problem with JavaScript");
                }
            }
        </script>
</head>

<body style="<%=bDir%>";
	      oncontextmenu ="return false;" 
          onkeydown="OnEnterKeyDown();"
          onfocus="<%=sFocusAction%>"
          onload="checkjs();body_load();<%=sOnLoadLogon%>if(document.getElementById('hdnSso').value=='Continue')btnOk_click(false);" >
    <div class="wrapper d-flex flex-wrap">
        <div class="imgSide d-none flex-column d-lg-flex justify-content-start align-items-start flex-wrap">
            <img id="imgLogo1" runat="server" class="pl-3 mt-3 align-self-end" src="./assets/images/logo.png" alt="Logo" height="50" style=" margin-left: 1rem;" /><!-- width="auto"  -->
            <h3 class="d-block mt-auto align-self-stretch text-center">
                <asp:label id="LabelVer" runat="server" EnableViewState="false"></asp:label>
            </h3>
        </div>
        <div class="menuSide d-flex flex-wrap flex-column align-items-center">
            <h1 class="h1  display-1 mt-6 text-center font-weight-normal">
                <asp:label id="lblCaption" runat="server" EnableViewState="false"></asp:label>
            </h1>

            <form class="form-signin d-flex flex-wrap flex-column align-items-center m-auto" id="WebForm2" method="post" runat="server">
			<input name="Hpath_Comtec" id="Hpath_Comtec" type="hidden" value='<%=sComtec%>' >
			<INPUT id="hdnCodeFromFile" type="hidden" name="hdnCodeFromFile" runat="server">
			<INPUT id="hdnFileForCode" type="hidden" name="hdnFileForCode" runat="server">
			<INPUT id="hdnIp" type="hidden" name="hdnIp" runat="server">
			<INPUT id="hdnGetIpWay" type="hidden" name="hdnGetIpWay" runat="server">
			<INPUT id="hdnFromOK" type="hidden" value="0" name="hdnFromOK" runat="server">
			<INPUT id="hdnServerName" type="hidden" value='<%=Request.ServerVariables["SERVER_NAME"]%>' name="hdnServerName">
			<INPUT id="hdnDetectJavaPath" type="hidden" name="hdnDetectJavaPath" value='http://<%=Request.ServerVariables["SERVER_NAME"]%>/jadvantage/detectjava1.html' >
			<INPUT id="hdnResult" type="hidden" name="hdnResult" runat="server">
			<INPUT id="hdnReady" type="hidden" name="hdnReady" runat="server" /><!-- onpropertychange="if (this.value!='' && event.propertyName=='value') btnOk_continue();" -->
			<!-- <INPUT id="hdnCopyright" type="hidden" name="hdnCopyright" runat="server"> -->
			<INPUT id="hdnSso" type="hidden" name="hdnSso" runat="server">
			<INPUT id="hdnSession" type="hidden" value='<%=Session.SessionID%>' name="hdnSession"><!-- vk 06.23 -->
			<DIV style='DISPLAY:none'>
				<%=sAdditionalHtml%>
			</DIV>
                <img id="imgLogo2" runat="server" class="mb-4 d-lg-none" src="./assets/images/logo.png" alt="Logo" height="50" /><!-- width="auto"  -->
				<b><asp:label class="d-block text-center pb-3" id="Label4" runat="server" EnableViewState="false"></asp:label></b>
                <!-- <h4 class="h4 font-weight-normal mb-4 text-center">נא להזדהות</h4> -->

                <asp:label for="txtUser" class="align-self-start label label-default" id="Label1" runat="server"><strong>שם משתמש:</strong></asp:label>
		<INPUT class="form-control mb-2" required autofocus id="txtUser" name="txtUser" dir="ltr" tabIndex="1" runat="server" maxlength="10"
			onkeypress="if(window.event.keyCode == 13){document.getElementById('btnOk').click();}"
			onkeyup="document.getElementById('hdnUser').value=document.getElementById('txtUser').value;">
		<INPUT id="hdnUser" type="hidden" name="hdnUser" runat="server">
                <asp:label for="txtPassword" class="align-self-start label label-default" id="Label2" runat="server"><strong>סיסמה:</strong></asp:label>
		<INPUT class="form-control mb-2" required type="password" id="txtPassword" name="txtPassword" dir="ltr" tabIndex="2" runat="server" maxLength="10"
			onkeypress="if(window.event.keyCode == 13){document.getElementById('btnOk').click();}"
			onkeyup="document.getElementById('hdnPassword').value=document.getElementById('txtPassword').value;">
		<INPUT id="hdnPassword" type="hidden" name="hdnPassword" runat="server">

                <INPUT class="btn btn-lg mt-2 mb-3 pl-5 pr-5 btn-lg align-self-center align-self-lg-end btn-primary"
                    value="כניסה" id="btnOk" onclick="btnOk_click(false);" type="button" name="btnOk" runat="server">

                <div class="mt-2 text-center">
<b><asp:label id="Label5" runat="server" EnableViewState="false"></asp:label></b>
					<br />
<b><asp:label id="Label6" runat="server" EnableViewState="false"></asp:label></b>
										<br />
<b><asp:label id="lblMsg" runat="server" EnableViewState="false"></asp:label></b>
<span id="lblWait"></span>
                </div>
            </form>
            <footer class="footer bg-transparent border-top-0 p-0 pb-0 m-0">
                <div class="container">
                    <div class="row">
                        <div class="col-12 pb-2 text-center">
                            קומטק בע"מ © כל הזכויות שמורות
                        </div>
                    </div>
                </div>
            </footer>
		<DIV style='DISPLAY:none'>
			<IFRAME id='oIFrame' name='oIFrame' src='Empty.htm' width='0' height='0'></IFRAME>
			<form id="Attach" action="Attach.aspx" method="post" target="oIFrame">
				<INPUT type="hidden" id="hdnUser_" name="hdnUser_"> <INPUT type="hidden" id="hdnPassword_" name="hdnPassword_">
			</form>
		</DIV>
        <IFRAME id='ifrRefresh' name='ifrRefresh' src='Refresh.aspx' width='0' height='0'></IFRAME>
        </div>
    </div>

</body>

</html>
