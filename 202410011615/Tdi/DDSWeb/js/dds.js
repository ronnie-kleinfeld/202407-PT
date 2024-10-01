var bInPing = false;
var gIsEnable = false;
var PingInterval;
//var nPingMaxCount = 0;
var nPingSeconds = 0;
var nPingAlertCount = 0;
var sPingAlertText = "";
var validNavigation = false;
//vk 01.20
//var bMouse = false;
var bChange = false;

// ---------------------------------
// Global Constants for this module
// ----------------------------------

// Const for Length of attribute
var numL, indL, typL, lenL, decL;

// Const for Start of attribute
var numS, indS, typS, lenS, decS;

//function DebugEvent(sEvent) {
//    var qq=0;
//}

function NotReady(sEvent) {
    if (sEvent != "" && window.event == undefined) {
        event = jQuery.Event(sEvent);
    }
    return (document.readyState != 'complete' && document.readyState != 'interactive') || !IsEnable();
}

function NotReady_Key(sEvent) {
    var nr = NotReady(sEvent);
    if (nr) {
        event.keyCode = 0;
        event.charCode = 0;
        event.returnValue = false;
    }
    return nr;
}
function IgnoreKey() {
    //if (event.keyCode == 16 || event.keyCode == 17) {
    //    return;
    //}
    if (event.keyCode >= 112 && event.keyCode <= 123) {
        event.keyCode = 0;
        event.charCode = 0;
        event.returnValue = false;
        event.preventDefault();
        return false;
    }
}
function StartPing() {
    nPings = 0;
    nPingSeconds = parseInt(document.getElementById("PingSeconds").value);
    nPingMaxCount = parseInt(document.getElementById("PingMaxCount").value);

    Ping(nPingMaxCount);
    PingInterval = setInterval(function () { Ping(nPingMaxCount); }, nPingSeconds * 1000);
}

function changeClass(obj, name) {
    if (!NotReady("")) {
        $(obj).removeClass().addClass(name);
    }
    return;
}

function XP(n) //vk 08.05
{
    try {
        if (navigator.userAgent.indexOf("Windows NT 5.0") > -1)
            return n + ""; //vk 06.07
        else if (navigator.userAgent.indexOf("Windows NT 5.") > -1) //&& window.clientInformation.appMinorVersion.indexOf("SP2")>-1)
            return (n + 30) + "";
        else
            return n + "";
    }
    catch (e) {
        return n + "";
    }
}

function InitConst(pnumL, pindL, ptypL, plenL, pdecL, pnumS) {

    // Const for Length of attribute
    numL = pnumL;
    indL = pindL;
    typL = ptypL;
    lenL = plenL;
    decL = pdecL;

    // Const for Start of attribute
    numS = pnumS;
    indS = numS + numL;
    typS = indS + indL;
    lenS = typS + typL;
    decS = lenS + lenL;

    bMayMarkCurrent = true; //vk 05.07
    validNavigation = false;

    //var coll = document.all;
    //var coll = $("*")
    //for (var i = 0; i < coll.length; i++) {
    //    coll[i].setAttribute("origcur", coll[i].style.cursor);
    //}

    if ($("#Harchive").get(0)) {
        $("input[id='" + $("#Harchive").val() + "']").attr("disabled", true);
    }
    //if (document.all.item("Harchive"))
    //    document.all.item(document.all.Harchive.value).disabled=true;

    //if (sessionStorage.getItem('send')) //ntg 03.12.23 runtime check function call- by vladi
    //    document.getElementById("HeaderUserName").innerHTML = sessionStorage.getItem('send') + "<br>" + ccc();

}
function IsEnable() {
    var rt = false;
    var o = document.getElementById("gIsEnable"); //vk 03.20
    if (o) {
        if (o.value == "1") {
            rt = true;
        }
    }
    return rt;
}
//function CopyOut(cnt) { //ntg 10.07.24 --rem vladi's change regarding city-street screen
function CopyOut(cnt, txt) { //ntg 10.07.24 vladi's change regarding city-street screen

    //if (!gIsEnable) return;
    if (!IsEnable()) {
        return;
    }
    var sOut = cnt.name.substr(1);
    var oOut = document.getElementById(sOut);
    if (oOut != null) oOut.value = cnt.value;

    if (txt) { //ntg 10.07.24 vladi's change regarding city-street screen
        sOut = document.getElementById(txt).value;
        oOut = document.getElementById(sOut);
        if (oOut != null) oOut.value = cnt.selectedOptions[0].innerText;
    }
}

function SetColorForCombo(cnt) {
    if (cnt.value == "0") {
        $(cnt).addClass("firstOption")
    }
    else {
        $(cnt).removeClass("firstOption")
    }

}

function body_load(sCopyright) {
    //gIsNumber = true;
    //gIsDate = true;
    //gIsTime = true;
    //gIsText = true;
    bReady = false; //vk 09.14

    var msg = "";
    var obj = $("#lblMsg");

    if ($("#lblMsg").get(0)) {
        msg = GetHid("Hlastentry", "");
        if (msg != "") {
            obj.wrapInner(msg);
            obj.attr('class', 'LastEntryStyle');
        } else {
            switch (sCopyright) {
                case "link":
                    obj.wrapInner(GetCopyright());
                    break;
                case "text":
                    obj.wrapInner(GetCopyrightNetto());
                    break;
            }
        }
    }

    if ($("#C_Print").get(0)) {
        // document.all("C_Print").value = GetCaption("print"); //vk 01.04
        $("#C_Print").val(GetCaption("print"));
    }

    if ($("#C_PrintPages").get(0)) {
        // document.all("C_PrintPages").value = GetCaption("print"); //vk 05.06
        $("#C_PrintPages").val(GetCaption("print"));
    }

    if ($("#C_PrintPdf").get(0)) {
        //document.all("C_PrintPdf").value = GetCaption("print"); //vk 03.09
        $("#C_PrintPdf").val(GetCaption("print"));
    }

    if ($("#C_Help").get(0)) {
        //document.all("C_Help").value=GetCaption("help");
        //document.all("C_Help").disabled=true;
        $("#C_Help").val(GetCaption("help"));
        //$("#C_Help").attr("disabled", true);
        HelpOff(true);
    }
}

function HelpOff(dis) {
    document.getElementById("C_Help").disabled = dis;
}

function CloseJob() //vk 10.04
{
    if (document.getElementById("Hx").value != 'x') return;
    document.getElementById("Hfcmd").value = '##';
    doSubmit();
}

function Relogon(nSec) { //vk 04.24 - ntg 08.04.24 changes for Logon screen
    document.getElementById("Hx").value = '';
    if (nSec == 0)
        RelogonOnce();
    else
        setTimeout("RelogonOnce();", nSec * 1000);
}
function RelogonOnce() {
    window.open("Logon.aspx", "_self");
}

function CloseWindow(nSec) //vk 10.04
{
    document.getElementById("Hx").value = '';
    if (nSec == 0)
        CloseOnce();
    else
        setTimeout("CloseOnce();", nSec * 1000);
}
function CloseOnce() //vk 06.09, 06.10
{
    top.opener = top;
    opener = window;
    top.open("Empty.htm", "_self");
    top.close();
}

function ReplyToModal(swStat) //vk 02.06
{
    var rv = window.showModalDialog('screenMod.htm', '', swStat);
    if (rv == 'ok') {
        document.forms[0].action = 'screen.aspx';
    } else if (rv == 'timeout') {
        if (document.getElementById("Hx")) document.getElementById("Hx").value = '';
        //opener=window;
        CloseOnce(); //vk 06.09
    } else {
        document.forms[0].action = 'screen.aspx?action=modcancel';
    }
    ShowWait();
    document.getElementById("gIsEnable").value = 0;
    //gIsEnable = false;
    SubmitWithFrame();//document.forms(0).submit();
}

function ShowWait() //vk 02.06
{
    var oMsg = document.getElementById("lblMsg");
    document.body.style.cursor = 'wait';
    if (oMsg) {
        oMsg.innerText = msg(1);
        oMsg.className = 'Msg';
        setMsgDir();
    }
    //vk 09.24
    setTimeout("ShowWaitDelayed();", 1000);
}
function ShowWaitDelayed() //vk 09.24
{
    //vk 05.07
    var oPleaseWait = document.getElementById("PleaseWait");
    if (oPleaseWait)
        oPleaseWait.style.display = "block";
    //vk 05.10
    //GetResol(document.getElementById("HresMethod"), document.getElementById("HresW"), document.getElementById("HresH"));
}

function ShowScroll(w, h, bHebrew) //vk 05.10
{
    //if (InGet()) {
    //    return;
    //}
    with (document.body) {
        if (clientHeight < h - 2) style.overflowY = "scroll";
        if (clientWidth < w - 2) {
            style.overflowX = "scroll";
            if (bHebrew) scrollLeft = scrollWidth - clientWidth;
        }
    }
}

function CloseMsg() //vk 03.06
{
    //if (InGet()) return;
    try {
        if ($("#Hdialog").val() == 'y') {
            $("#Hdialog").val('');
            event.returnValue = "stop";
            return "";
        }
        if ($("#Hx").val() == 'x') {
            event.returnValue = CloseMsgText();
            return CloseMsgText();
        }
        else {
            $("#Hwait", opener.document).val('');
        }
    } catch (e) { }
}
function CloseMsgText() {
    var x;
    x = msg(12);
    //vk 10.11
    var m = $("#HexitMsg").get(0);
    if (m)
        if (m.value != "")
            x = m.value;
    return x;
}

function OnClose() //vk 03.07
{
    //if (InGet()) return;
    //if (InGetP()) return;
    //if (document.readyState=="loading")
    if (validNavigation)
        return;
    //if (NoPopups()) //document.all.ifrPing.src="Close.aspx"; //vk 05.09
    //{ //vk 08.09
    try {
        var xmlhttp;
        if (window.XMLHttpRequest) {
            xmlhttp = new XMLHttpRequest();
        } else if (window.ActiveXObject) {
            xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
        } else {
            return;
        }
        xmlhttp.open("GET", "Close.aspx", true);
        xmlhttp.send(null);
    }
    catch (e) {
        MyMsgBox(e.message);
    }
    //}
    //else
    //    window.open('Close.aspx', '',
    //	    "top="+(screen.height+1)+",left="+(screen.width+1)+",height=150,width=250,status=0,toolbar=0,menubar=0,location=0,channelmode=0,directories=0,resizable=1,scrollbars=1,titlebar=0");
}

function DdsDebug(s) //vk 03.06
{
    try {
        debugger;
        //var fso = new ActiveXObject("Scripting.FileSystemObject");
        //var a = fso.OpenTextFile("C:\\inetpub\\wwwroot\\DspNew\\DDSWeb\\ddsdebug.txt", 8, true);
        //var d=new Date();
        //var dh=d.getHours();
        //if (dh<10) dh="0"+dh;
        //var dm=d.getMinutes();
        //if (dm<10) dm="0"+dm;
        //var ds=d.getSeconds();
        //if (ds<10) ds="0"+ds;
        //var dms=d.getMilliseconds();
        //if (dms<10) dms="00"+dms;
        //else if (dms<100) dms="0"+dms;
        //a.WriteLine(dh+":"+dm+":"+ds+"."+dms+" "+s);
        //a.close();
    }
    catch (e) {
        MyMsgBox(e.message);
    }
}

function MoveSmth(sWhat, sTo) //vk 07.07
{
    //if (InGet()) return;
    var x = document.getElementById(sWhat);
    var y = document.getElementById(sTo);
    if (x) {
        if (y) {
            var ooo;
            var l = 0;
            var t = 0;
            for (ooo = y; ooo.tagName.toLowerCase() == "div"; ooo = ooo.parentElement) {
                l += parseInt("0" + ooo.style.left);
                t += parseInt("0" + ooo.style.top);
            }
            x.style.left = l + "px";
            x.style.top = t + "px";
        }
    }
}

function objExists(obj) {
    if (obj)
        return obj.length > 0;
    else
        return false;
}

function GetHid(sWhat, sDefault) {
    var obj = $("input[id='" + sWhat + "']");

    if (objExists(obj))
        return obj.val();
    else
        return sDefault;
}

function setMsgDir() //vk 04.03
{
    var obj = document.getElementById("lblMsg");
    if (objExists(obj)) {
        obj.dir = gDir;
    }
}

function AdjustW(bAdjustW, bMinusDelta) //vk 06.10
{
    //if (InGet()) {
    //    return;
    //}
    var w = document.getElementById("W");
    if (w) {
        with (w) {
            spellcheck = false; //vk 03.16
            if (bAdjustW) {
                var deltaW = parseInt(style.width) - clientWidth + 1;
                style.width = (parseInt(style.width) + deltaW) + "px";
                if (bMinusDelta)
                    style.left = (parseInt(style.left) - deltaW) + "px";
            }
            style.visibility = 'visible';
        }
    }
}

//function InGet() //vk 06.10
//{
//    var obj = document.getElementById("ifrGet");
//    if (objExists(obj)) {
//        return obj.contentWindow == window;
//    } else {
//        return false;
//    } 
//}
//function InGetP()
//{
//    var obj = parent.document.getElementById("ifrGet");
//    if (objExists(obj)) {
//        return obj.contentWindow == window;
//    } else {
//        return false;
//    }
//}

//function artis(sFile, sExe) //vk 12.12
//{
//    var sStep;
//    try {
//        sStep="User Control";
//        var o = document.getElementById("DspControls");
//        var rc;
//        sStep="Creating File";
//        if (document.getElementById("Hartis"))
//            rc = o.CreateFile(sFile, document.getElementById("Hartis").value); //vk 01.13
//        else
//            rc=o.CreateFile(sFile);
//        if (rc!="OK") {
//            MyMsgBox(sStep+": "+rc);
//            return;
//        }
////        sStep="Deleting File";
////        rc=o.DeleteFile(sFile);
////        if (rc!="OK") {
////            MyMsgBox(sStep+": "+rc);
////            return;
////        }
//        sStep="Running Exe";
//        rc=o.Run(sExe,'');
//        if (rc!="OK") {
//            MyMsgBox(sStep+": "+rc);
//            return;
//        }
//    }
//    catch(e) {
//        MyMsgBox(sStep+": "+e.message);
//    }
//}
function ccc() { //ntg 03.12.23 runtime check function by vladi
    var d = new Date();
    var dh = d.getHours();
    if (dh < 10) dh = "0" + dh;
    var dm = d.getMinutes();
    if (dm < 10) dm = "0" + dm;
    var ds = d.getSeconds();
    if (ds < 10) ds = "0" + ds;
    var dms = d.getMilliseconds();
    if (dms < 10) dms = "00" + dms;
    else if (dms < 100) dms = "0" + dms;
    return dh + ":" + dm + ":" + ds + "." + dms;
}
