<%@ Page Language="c#" CodeBehind="in0.aspx.cs" AutoEventWireup="false" Inherits="DDSWeb._in0" %>

<!-- vk 07.23 -->

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Please don't close this window</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio 7.0">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <%=Application["META"]%>
    <script lang="javascript">
        var Son;
        function CheckScreenAlive() {
            try {
                var d = Son.document;
            }
            catch (e) {
                opener = Son;
                document.getElementById("Hx").value = "";
                this.close();
            }
            setTimeout("CheckScreenAlive();", 1000);
        }
        function msg() {
            if (document.getElementById("Language").value.toLowerCase() == "heb") //toLowerCase vk 12.05
                return "חלון זה חייב להיות פתוח";
            else
                return "This window must be open";
        }
        function body_load() {
            //if (document.getElementById("Token").value == "")
            //    GetResol(document.getElementById("ResolutionMethod"), document.getElementById("ResolutionW"), document.getElementById("ResolutionH"))
            //MaxWidth = document.getElementById("ResolutionW").value;
            //MaxHeight = document.getElementById("ResolutionH").value;
            try {
                document.cookie = "SessionKey_" + document.getElementById("hdnSession").value + "=" + Math.random().toString();
            } catch (ee) { }
            document.forms.frmInit.submit();
        }
        function goon() {
            if (document.location.search == "?submit") {
                document.forms[0].submit();
            } else {
                var winStats;
                winStats = "top=0,left=0" //,height=" + MaxHeight + ",width=" + MaxWidth;
                winStats += ",status=0,toolbar=0,menubar=0,location=0"; //,fullscreen=0"; //msh 02.07
                winStats += ",channelmode=0,directories=0,resizable=1,scrollbars=1,titlebar=0";
                if (document.getElementById("PrintingGroup").value == "") {
                    if (document.getElementById("Frame").value == "true")
                        opener = window.open('frame.aspx', '', winStats); //vk 05.10
                    else
                        opener = window.open('screen.aspx', '', winStats);
                    document.getElementById("Hx").value = "";
                    opener = window.open('empty.htm', '_self'); //msh 02.07
                    this.close(); //rem by vk 11.04
                } else {
                    //vk 10.05
                    document.getElementById("lblMsg0").style.display = 'block'; //vk 12.05
                    Son = window.open('screen.aspx', '', winStats);
                    CheckScreenAlive();
                }
            }
        }
    </script>
</head>
<body onload="body_load();" onbeforeunload="if(document.getElementById('Hx').value=='x')event.returnValue=msg();">
    <form method='post' action='screen.aspx'>
        <center>
			<p><span name='lblMsg0' id='lblMsg0'
				style="FONT-SIZE:12pt;COLOR:red;FONT-FAMILY:Arial;font-weight:bold;display:none;-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none;">
				נא לא לסגור חלון זה. עם סגירת החלון ייסגרו גם המדפסות</span></p><!-- vk 12.05 -->
			<p><span name='lblMsg' id='lblMsg'
				style='FONT-SIZE:12pt;COLOR:red;FONT-FAMILY:Arial;font-weight:bold;-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none;'></span></p>
		</center>
    </form>
    <div style='display: none;'>
        <form id="in0" method="post" runat="server">
            <input id="hdnSession" type="hidden" value='<%=Session.SessionID%>' name="hdnSession">
            <input id="PrintingGroup" type="hidden" name="PrintingGroup" value="<%=Application["PRINTINGGROUP"]%>">
            <input id="Language" type="hidden" name="Language" value="<%=Application["LANGUAGE"]%>">
            <input id="Frame" type="hidden" name="Frame" value="<%=Application["FRAME"]%>">
            <input id="Token" type="hidden" name="Token" value="<%=Session["Token"]%>">
            <input id="Hx" type="hidden" name="Hx" runat="server" value="x">
        </form>
        <iframe id='ifrInit' name='ifrInit' src='Empty.htm' width='0' height='0'></iframe>
        <form id='frmInit' action='Init.aspx' method='post' target='ifrInit'></form>
    </div>
</body>
</html>
