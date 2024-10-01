var nPingMaxCount;
var nPingSeconds;
var nPings;
var PingInterval;
//var popup; //vk 05.09
function StartPing() {
    nPingMaxCount = window.dialogArguments[7];
    nPingSeconds = window.dialogArguments[8];
    nPings = 0;
    if (nPingMaxCount < 0) return;
    if (nPingSeconds < 0) return;
    //Ping(nPingMaxCount);
    PingInterval = setInterval("Ping(" + nPingMaxCount + ");", nPingSeconds);
}
function Ping(nPingMaxCount) {
    if (nPings < nPingMaxCount || nPingMaxCount == 0) {
        if (nPingMaxCount != 0)
            nPings++;
        if (document.getElementById("oFrameSet")) {
            var x = document.getElementById("oFrameSet").document.getElementById("oFrame");
            if (x.contentWindow)
                x = x.contentWindow;
            x.document.forms.frmPing.submit();
        }
        else
            document.forms.frmPing.submit();
    } else if (nPings == nPingMaxCount) {
        clearInterval(PingInterval); //vk 10.07
        //if (!popup)
        document.getElementById("ifrPing").src = "Close.aspx"; //vk 05.09
        //else
        //    window.open('Close.aspx', '',
        //	    "top="+(screen.height+1)+",left="+(screen.width+1)+",height=150,width=250,status=0,toolbar=0,menubar=0,location=0,channelmode=0,directories=0,resizable=1,scrollbars=1,titlebar=0"); //vk 10.07
        document.body.innerHTML =
            "<table width=100% height=100%><tr><th><font color=blue size=4><input name='Hfcmd' id='Hfcmd' type='hidden'>"
            + window.dialogArguments[6] + "</font></th></tr><tr><th>"
            + "<input type=button value=Close onClick='top.returnValue=\"timeout\";top.opener=top;opener=window;top.close();'>" //timeout vk 02.06
            + "</th></tr></table>";
        document.body.onkeydown = "";
        document.body.onbeforeunload = "";
    }
}
//vk 05.09, 06.09
//function SetMiddle(h,w,t) {
//	popup=false;
//	var b=parent.document.documentElement;
//    with (parent.document) {
//	    getElementById("ifr100_dc").style.width=w + "px";
//		getElementById("ifr100").style.height=h+ "px";
//		getElementById("ifr100").style.width=w+ "px";
//		getElementById("ifr100_height").style.height=h+ "px";
//		getElementById("ifr100_width").style.width=w+ "px";
//		if (t)
//		    getElementById("ifr100_above").style.height = t + "px";
//		else if (b.clientHeight<(h-0)+27)
//		    getElementById("ifr100_above").style.height="100%";
//		else
//		    getElementById("ifr100_above").style.height = (b.clientHeight - h - 27) / 2 + "px";
//		//vk 01.11
//		var bb=parent.document.getElementById("bottom");
//	    if (bb) {
//		    if (bb.clientWidth<(w-0)+6)
//		        getElementById("ifr100_before").style.width = "0px";
//		    else
//		        getElementById("ifr100_before").style.width = (bb.clientWidth - w - 6) / 2 + "px";
//		}
//	    getElementById("div100").style.visibility='visible';
//	}
//	//vk 06.09
//	//document.body.focus();
//    //document.body.unselectable="on";
//    var db = document.body;
//    var st = db.getAttribute("style");
//    st = st + "-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none;";
//    db.setAttribute("style", st);
//	RememberAndLockOnce("input");
//	RememberAndLockOnce("img");
//	RememberAndLockOnce("a");
//	RememberAndLockOnce("textarea");
//	RememberAndLockOnce("span"); //vk 04.20
//	RememberAndLockOnce("label"); //vk 10.20
//	RememberAndLockOnce("div"); //vk 04.20
//	RememberAndLockOnce("option"); //vk 04.20
//	RememberAndLockOnce("select"); //vk 04.20
//}
function WinClose(retVal) {
    //if (!popup) {
    parent.document.getElementById("Hresult100").value = retVal;
    //HideAndUnlock();
    //} else {
    //	window.returnValue=retVal;
    //	window.close();
    //}
}
//function HideAndUnlock() {
//    parent.document.getElementById("div100").style.visibility='hidden';
//    parent.document.getElementById("ifr100_exit").style.visibility='hidden';
//    parent.document.getElementById("ifr100").src='Empty.htm';
//    parent.document.getElementById("ifrRefresh").src='Empty.htm'; //vk 07.13
//    UnlockOnce("input");
//    UnlockOnce("img");
//    UnlockOnce("a");
//    UnlockOnce("textarea");
//    UnlockOnce("span"); //vk 07.13
//    UnlockOnce("label"); //vk 10.20
//    UnlockOnce("div"); //vk 04.20
//    UnlockOnce("option"); //vk 04.20
//    UnlockOnce("select"); //vk 04.20
//}
/*
function RememberAndLockOnce(s) //vk 06.09
{
    //setTimeout("debugger;",5000);
    var coll = parent.document.getElementsByTagName(s);
    if (coll) {
        var count = coll.length;
        if (count)
            for (var i = 0; i < count; i++) {
                origdisSet(coll[i]); //vk 06.15
                if (coll[i].disabled) {
                    //coll[i].origdis = "true"; //vk 07.13
                } else {
                    coll[i].disabled = true;
                    //coll[i].origdis = "false";
                }
                //vk 07.13
                //if (coll[i].className=="BtnComFoc") coll[i].className="BtnCom";
                //oEvent = parent.document.createEventObject();
                //coll[i].fireEvent("onblur",oEvent);
            }
    }
}
function UnlockOnce(s) //vk 06.09
{
    var coll = parent.document.getElementsByTagName(s);
    if (coll) {
        var count = coll.length;
        if (count)
            for (var i = 0; i < count; i++) {
                //if (coll[i].type!="hidden") { //vk 07.13
                if (coll[i].getAttribute("origdis") == "false")
                        coll[i].disabled = false;
                    //coll[i].style.cursor=coll[i].origcur; //vk 07.13
                    //ResetCursor(coll[i]); //vk 04.20
                //}
            }
    }
}
function ResetCursor(o) { //vk 04.20
    var s = o.getAttribute("origcur");
    if (s == null)
        s = ""; //"default";
    //else if (s == "")
    //    s = "default";
    o.style.cursor = s;
}

function origdisSet(c) //vk 06.15, 04.20
{
    if (c.getAttribute("origdis") > "") return;
    if (c.disabled)
        c.setAttribute("origdis", "true");
    else
        c.setAttribute("origdis", "false");
}
*/
