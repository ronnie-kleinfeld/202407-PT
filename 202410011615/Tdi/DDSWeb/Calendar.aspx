<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Calendar.aspx.cs" Inherits="DDSWeb.Calendar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>
        ... &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    </title>

    <meta charset="utf-8" />
    <meta content="-1" http-equiv="Expires" />
    <meta content="no-cache" http-equiv="Pragma" />

    <link href="assets/bootstrap5/css/bootstrap.css" rel="stylesheet" />


    <script src="jquery/jquery-1.10.2.min.js"></script>
    <script src="assets/bootstrap5/js/bootstrap.bundle.min.js "></script>


    <style>
        .outButton {
            width: 100%;
            height: 23px;
            font-size: 12px;
            font-family: 'Arial, Verdana, Tahoma';
            letter-spacing: 1;
            text-align: center;
            color: #000000;
            font-weight: bold;
            cursor: default;
            border-top: 1px solid #FFFFFF;
            border-left: 1px solid silver;
            border-bottom: 1px solid gray;
            border-right: 1px solid gray;
            background: linear-gradient(#FFFFFF,#D4D0C8);
        }

        .onButton {
            width: 100%;
            height: 23px;
            font-size: 12px;
            font-family: 'Arial, Verdana, Tahoma';
            letter-spacing: 1px;
            text-align: center;
            color: #000000;
            font-weight: bold;
            cursor: default;
            border-top: 1px solid #FFFFFF;
            border-left: 1px solid silver;
            border-bottom: 1px solid gray;
            border-right: 1px solid gray;
            background: linear-gradient(#D4D0C8,#FFFFFF);
        }

        .top-buffer { margin-top:100px; }

    </style>
    <!-- <script  src='js/ddsModalPing.js'></script> -->
<script>

function BodyLoad()
{
	var dArguments,top;
	//popup=true;
	if (location.search!="") {
		var srch=location.search.substring(1).split("&");
		for (var param in srch) {
		    var pair = srch[param].split("=");
		    switch (pair[0]) {
		        case "date": dArguments = pair[1]; break;
		        case "top": top = pair[1]; break;
		    }
		}
        ///SetMiddle(250, 235, top);
	} else {
		window.returnValue = "Value=Cancel";
		StartPing(); //vk 12.06
	}
}

function OK_Click()
{
    var dateValue = document.getElementById("TextBox1").value;
    WinClose("Value=" + dateValue);
}


$(document).ready(function () {
    BodyLoad();
});


function Cancel_Click()
{
	WinClose("Value=Cancel");
}
function toDay() {
    var dateValue = document.getElementById("ToDay").value;
    WinClose("Value=" + dateValue);
}
</script>

</head>
<body  dir="rtl" style="margin:0 auto; text-align:center; overflow:hidden;">
        <form id="form1" runat="server" >
             <div style="margin:0 auto; text-align:center">
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <asp:DropDownList id="drpCalMonth" Runat="Server" OnSelectedIndexChanged="Set_Calendar" AutoPostBack="true"></asp:DropDownList>
                                    <asp:DropDownList id="drpCalYear" Runat="Server" OnSelectedIndexChanged="Set_Calendar" AutoPostBack="true"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                     <asp:Calendar id="Calendar1" runat="server"  onselectionchanged="Calendar1_SelectionChanged"></asp:Calendar>
                                     <asp:HiddenField id="TextBox1"  runat="server"></asp:HiddenField>
                                     <asp:HiddenField id="ToDay"  runat="server"></asp:HiddenField>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>         
                </asp:UpdatePanel>
            </div>
     
            <table border="0" CELLSPACING="0" CELLPADDING="0" style="width:100%;" dir="ltr">
                <tr>
                    <td style="width:33%;">
                        <input id="btnCancel" type="button" value="ביטול" onclick="Cancel_Click()" class="outButton" onmouseover="this.className='onButton';" onmouseout="this.className='outButton';"/>
                    </td>
                    <td style="width:33%;">
                       <input id="btnToday" type="button" value="היום" onclick="toDay()" class="outButton" onmouseover="this.className='onButton';" onmouseout="this.className='outButton';"/>
                    </td>
                    <td style="width:33%;">
                        <input id="btnOK" type="button" value="אישור" onclick="OK_Click()" class="outButton" onmouseover="this.className='onButton';" onmouseout="this.className='outButton';"/>
                    </td>
                </tr>
            </table>
        </form>

        <div style="display:none;">
            <input name="PingNumber" id="PingNumber" type="hidden"/>
            <iframe id='ifrPing' name='ifrPing' src='../Empty.htm' width='0' height='0'></iframe>

            <form id='frmPing' action='../Ping.aspx' method='post' target='ifrPing'></form>
        </div> 
</body>
</html>
