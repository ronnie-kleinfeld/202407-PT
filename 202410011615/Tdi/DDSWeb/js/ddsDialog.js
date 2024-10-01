var dLastSecond; //vk 01.08
var MqInterval, MqTimeout; //vk 05.08
var gAdr; //vk 11.09
var sError; //vk 07.13
var bInCG = false; //vk 02.20

var sPingAlertText;
var nPingAlertCount;
var nPingSeconds;
var nPingMaxCount;
var modalHeight; //ntg 30.04.24 vladi change regarding CG functionallity

//vk 05.04
function fnpf4_dblclick_adr(s, sN) {
    //debugger;
    if (!IsEnable()) {
        return;
    }

    var objC, objN, objS, objT;
    var b = false;
    //vk 11.09
    gAdr = s;
    gFocus = document.getElementById(sN);

    if (document.getElementById(s + "C")) objC = document.getElementById(document.getElementById(s + "C").value);
    if (objC) {
        if (document.getElementById(s + "N")) objN = document.getElementById(document.getElementById(s + "N").value);
        if (document.getElementById(s + "S")) objS = document.getElementById(document.getElementById(s + "S").value);
        if (document.getElementById(s + "T")) objT = document.getElementById(document.getElementById(s + "T").value);
    }
    else return;

    if (objC) document.getElementById("Modal_sCityCode").value = objC.value; else document.getElementById("Modal_sCityCode").value = "";
    if (objN) document.getElementById("Modal_sCityName").value = objN.value; else document.getElementById("Modal_sCityName").value = "";
    if (objS) document.getElementById("Modal_sStreetCode").value = objS.value; else document.getElementById("Modal_sStreetCode").value = "";
    if (objT) document.getElementById("Modal_sStreetName").value = objT.value; else document.getElementById("Modal_sStreetName").value = "";
    if (objS) b = true;
    if (objT) b = true;
    if (b) document.getElementById("Modal_sHalves").value = "2"; else document.getElementById("Modal_sHalves").value = "1";

    document.getElementById("Hdialog").value = "y";
    document.getElementById("Hresult100").value = "";
    //setTimeout("Adr_cont();",100);
    //till here vk 11.09
    document.forms.frmModal_Adr.action = "CityStreet.aspx?" + s;
    document.forms.frmModal_Adr.submit();

    $('#Modal').modal('show');

    $('#Modal').on('hidden.bs.modal', Adr_cont)
}
function Adr_cont() { //vk 11.09

    switch (document.getElementById("Hresult100").value) {
        //case "":
        //    setTimeout("Adr_cont();", 100);
        //    return;
        case "y":
            AfterAdr(true);
            return;
        case "n":
            AfterAdr(false);
            return;
    }
}
function AfterAdr(bYes) { //vk 11.09
    var s, p, n;
    try {
        gFocus.focus();
    } catch (e) { }
    if (!bYes)
        return;

    if ($(gFocus).attr("q") == "true") {
        n = 1;
    } else {
        n = 0;
    }
    document.getElementById("Hfld").value = gFocus.name.substr(numS + n, numL);
    document.getElementById("Hfind").value = gFocus.name.substr(indS + n, indL);
    if (document.getElementById("Hadr" + gAdr)) {
        s = document.getElementById("Hadr" + gAdr).value;
        p = s.split(";");
        if (p[0] == "1")
            fnBlur_Check(gFocus, p[1]);
    }
}
//function fnpf4_click_adr(s) //,per) ?
//{
//	//if (!gIsEnable) return;
//	//..?
//	fnpf4_dblclick_adr(s);
//}
function CopyToProtect(obj, r) //vk 05.09
{
    if (objExists(obj)) {
        obj.value = r;
        if (document.getElementById("P" + obj.name))
            document.getElementById("P" + obj.name).value = r;
        if (document.getElementById("Y" + obj.name))
            document.getElementById("Y" + obj.name).value = r; //vk 06.09
    }
}

//vk 08.04
//function fnpf4_dblclick_calc(s,anf)
//{
//    if (!IsEnable()) {
//        return;
//    }
//	var j;
//	var oTags = document.getElementsByTagName("*");
//	for (j = 0; j < oTags.length; j++) {
//		var oo=oTags[j];
//		if (oo.calccol) if (oo.linenum==s) document.getElementsById("Modal_"+oo.calccol).value=oo.value;
//	}
//	document.getElementById("Modal_sClass").value=anf;
//	//vk 09.04
//	document.getElementById("Modal_sLine").value="usual";
//	if ((s-0)==(document.getElementById("Gline_first").value-0)) document.getElementById("Modal_sLine").value="first";
//	if ((s-0)==(document.getElementById("Gline_last").value-0)) document.getElementById("Modal_sLine").value="last";

//	var arrArgs = new Array();
//	arrArgs[0] = document.forms.frmModal_Calc;
//	arrArgs[1] = "Calcul.aspx";
//	//FillForModalPing(arrArgs);

//	var winStats = "dialogHeight:"+XP(375)+"px; dialogWidth:820px; edge:sunken; scroll:No; center:Yes; help:No; resizable:No; status:No; unadorned:Yes;";
//	bInCalendar=true;
//	var rValue = window.showModalDialog("WinModalSubmit.htm", arrArgs, winStats);     
//	bInCalendar=false;

//	if (rValue) {
//	    document.getElementById("Modal_IFA1").value=rValue[0];
//	    document.getElementById("Modal_IFA2").value=rValue[1];
//        document.getElementById("Modal_IFA3").value=rValue[2];
//        document.getElementById("Modal_IF").value=rValue[3];
//        document.getElementById("Modal_IFB1").value=rValue[4];
//        document.getElementById("Modal_IFB2").value=rValue[5];
//        document.getElementById("Modal_IFB3").value=rValue[6];
//        document.getElementById("Modal_OR").value=rValue[7];
//        document.getElementById("Modal_ACTA1").value=rValue[8];
//        document.getElementById("Modal_ACTA2").value=rValue[9];
//        document.getElementById("Modal_ACTA3").value=rValue[10];
//        document.getElementById("Modal_ACT").value=rValue[11];
//        document.getElementById("Modal_ACTB1").value=rValue[12];
//	    document.getElementById("Modal_ACTB2").value=rValue[13];
//        document.getElementById("Modal_ACTB3").value=rValue[14];
//        document.getElementById("Modal_RND").value=rValue[15];
//        document.getElementById("Modal_INTO").value=rValue[16];
//        document.getElementById("Modal_RSL1").value=rValue[17];
//        document.getElementById("Modal_RSL2").value=rValue[18];
//        document.getElementById("Modal_RSL3").value=rValue[19];
//        document.getElementById("Modal_GO").value=rValue[20];
//        document.getElementById("Modal_T40").value=rValue[21];
//        document.getElementById("Modal_REM").value = rValue[22];


//        var oTags = document.getElementsByTagName("*");
//        //for (j=0;j<document.all.length;j++) {
//        for (j = 0; j < oTags.length; j++) {
//            //var oo = document.all[j];
//            var oo = oTags[j];
//			if (oo.calccol)
//				if (oo.linenum==s)
//				    oo.value = document.getElementById("Modal_" + oo.calccol).value;
//		}
//		//vk 09.04
//		var t;
//		if (rValue[23]==1 || rValue[23]==-1) {
//			t="000"+((s-0)+rValue[23]);
//			t=t.substr(t.length-3);
//			fnpf4_dblclick_calc(t,anf);
//		}
//	}
//}

//vk 05.05
function modellist1(s, bClear) {
    if (!IsEnable()) return;
    var objM, objY, objG, objP;
    s = s + "";

    if (document.getElementById(s + "M")) objM = document.getElementById(document.getElementById(s + "M").value);
    if (document.getElementById(s + "Y")) objY = document.getElementById(document.getElementById(s + "Y").value);
    if (document.getElementById(s + "P")) objP = document.getElementById(document.getElementById(s + "P").value);
    objG = document.getElementById("Hgear"); //vk 02.13
    if (objG.value == "")
        if (document.getElementById(s + "G"))
            objG = document.getElementById(document.getElementById(s + "G").value);
    if (objM) { } else return;
    if (objY) { } else return;
    if (objG) { } else return;
    if (objP) { } else return;
    var sN = objP.name;
    var o = document.getElementById("S" + sN);
    if (o) { } else return;
    var sPer = o.sPer;
    var sPfk = o.sPfk;
    var sWidth1 = o.sWidth1;
    var sWidth2 = o.sWidth2;
    var sHeight1 = o.sHeight1;

    if (bClear) {
        o.value = "...";
        document.getElementById(sN).value = "0";
        if (document.getElementById("C" + sN))
            document.getElementById("C" + sN).value = "";
    }

    if (objM.value + objY.value + "0" - 0 == 0) {
        if (document.getElementById("T" + sN)) //vk 06.20
            document.getElementById("T" + sN).outerHTML =
                "<table id='T" + sN + "' name='T" + sN + "' "
                + "style='display:none;cursor:default;width:" + sWidth1 + "' "
                + "class='TblSel' cellspacing=0 cellpadding=0>\n"
                + "<col /><col /><col />\n"
                + "<tr id='0' name='0' "
                + "onmouseenter='trS_mouseenter(this,\"" + sN + "\");' "
                + "onmouseleave='trS_mouseleave(this,\"" + sN + "\");' "
                + "onclick     ='trS_click     (this," + sPer + ",\"" + sPfk + "\");' "
                + "helptext='.'>\n"
                + "<td>0</td><td>...</td><td>...</td></tr></table>\n";
        if (document.getElementById("D" + sN)) { //vk 06.20
            document.getElementById("D" + sN).style.height = (sHeight1 - 0) + 2 + "px";
            document.getElementById("D" + sN).style.width = sWidth1 + "px";
        }
    } else {
        //o.setAttribute("mayopen", "false");
        var x = document.forms["frmModelsCombo"];
        x.hMnf.value = objM.value;
        x.hIzr.value = objY.value;
        x.hTrn.value = objG.value;
        x.hDgm.value = objP.value;
        x.hSN.value = sN;
        x.hPer.value = sPer;
        x.hPfk.value = sPfk;
        x.hWidth1.value = sWidth1;
        x.hWidth2.value = sWidth2;
        x.hHeight1.value = sHeight1;
        x.sMaxRows.value = o.getAttribute("sMaxRows"); //vk 03.14
        x.submit();
    }
}
function modellist2() {
    //if (InGet()) return; //vk 07.10
    //if (event.propertyName!="value") return; //vk 08.13
    var sN = document.getElementById("hWhat").value;
    if (sN == "") return; //vk 05.13
    var s = document.getElementById("S" + sN);
    var t = document.getElementById("T" + sN);
    var d = document.getElementById("D" + sN);
    if (t) {
        t.outerHTML = document.getElementById("hResult").value;
        d.style.height = document.getElementById("hHeight").value + "px";
        d.style.width = document.getElementById("hWidth").value + "px";
    } else {
        s.innerHTML = document.getElementById("hResult").value;
    }
    s.value = document.getElementById("hCurrentText").value;
    //s.setAttribute("mayopen", "true");
}

//vk 01.08
//function wnet_ask(nTimeOut,sUrl)
//{
//try {
//	var dNow=new Date();
//	dLastSecond=new Date(dNow.getYear(),dNow.getMonth(),dNow.getDate(),dNow.getHours(),dNow.getMinutes(),dNow.getSeconds()+nTimeOut);
//	document.getElementById("hMqComment").value =
//		"MQ=" + document.getElementById("hMq").value +
//		"; Key=" + document.getElementById("hKey").value +
//		"; Url="+sUrl+
//		"; Begin="+dNow.toString()+
//		"; End="+dLastSecond.toString()+
//		" (if there is a timeout, it is the only record in the log for this request)";
//	//vk 05.08
//	var nLocalTimeOut = document.getElementById("hTimeOut").value;
//	document.getElementById("hTimeOut").value = '1';
//	document.getElementById("Hfcmd").value = '99';
//	MqInterval=setInterval("wnet_submitonly();",nLocalTimeOut*1000);
//	MqTimeout =setTimeout ("wnet_return();",    nTimeOut*1000);
//}
//catch(e){MyMsgBox(e.message);}
//}
//function wnet_submitonly()
//{
//try {
//	var dNow=new Date();
//	document.forms.frmWnet.action="CheckMq.aspx?param="+dNow.toString();
//	document.forms.frmWnet.submit();
//}
//catch(e){MyMsgBox(e.message);}
//}
//function wnet_answer()
//{
////if (InGet()) return; //vk 07.10
////if (event.propertyName!="value") return; //vk 08.13
//try {
//    document.getElementById("hBefore").value = "";
//    switch (document.getElementById("hResult").value) {
//	    case "":        return; //vk 05.13
//		case "timeout": return;
//        case "1": document.getElementById("Hfcmd").value = '00'; break;
//        case "-1": document.getElementById("Hfcmd").value = '01'; break;
//        case "0": document.getElementById("Hfcmd").value = '98'; break;
//	}
//	wnet_return();
//}
//catch(e){MyMsgBox(e.message);}
//}
//function wnet_return()
//{
//	clearInterval(MqInterval);
//	clearTimeout(MqTimeout);
//	doSubmit();
//}

function streetlist1(cnt, txt, txt2) { //ntg 10.07.24 new function- vladi's change regarding city-street screen
    if (!IsEnable()) return;
    var sOut = document.getElementById(txt).value;
    var oOut = document.getElementById(sOut);
    if (oOut != null) oOut.value = ""; //street code, hidden
    sOut = "S" + sOut;
    oOut = document.getElementById(sOut);
    if (oOut != null) oOut.value = ""; //street name, combo
    sOut = document.getElementById(txt2).value;
    oOut = document.getElementById(sOut);
    if (oOut != null) oOut.value = ""; //street name, hidden
    var x = document.forms["frmStreetsCombo"];
    x.hCity.value = cnt.value;
    x.hTarget.value = txt;
    x.submit();
}
function streetlist2(txt) { //ntg 10.07.24 new function- vladi's change regarding city-street screen
    var sOut = "S" + document.getElementById(txt).value;
    var oOut = document.getElementById(sOut);
    if (oOut != null)
        oOut.innerHTML = "<option class='firstOption' selected value=''>" + msg(29) + "</option>" + document.getElementById("hResult").value;
}

//vk 07.07
function flexlist1(sSource, sTarget, bClear) {
    if (!IsEnable()) return;
    var o = document.getElementById("S" + sTarget);
    if (o) { } else return;

    if (bClear) {
        o.value = "...";
        document.getElementById(sTarget).value = "0";
        if (document.getElementById("C" + sTarget))
            document.getElementById("C" + sTarget).value = "";
    }
    //o.setAttribute("mayopen", "false");
    var x = document.forms["frmFlexCombo_" + sTarget];
    x.hTbno.value = document.getElementById(sSource).value;
    x.hOrderBy.value = (o.getAttribute("sOrderByCode") == "true" ? "flnoin" : "flltxt");
    x.hPer.value = o.getAttribute("sPer");
    x.hPfk.value = o.getAttribute("sPfk");
    x.hCurValue.value = document.getElementById(sTarget).value;
    x.hTarget.value = sTarget;
    x.sMaxRows.value = o.getAttribute("sMaxRows"); //vk 03.14
    x.submit();
}
function flexlist2(sTarget) {
    //if (InGet()) return; //vk 07.10
    //if (event.propertyName!="value") return; //vk 08.13
    if (document.getElementById("hResult_" + sTarget).value == "") return; //vk 08.13
    var s = document.getElementById("S" + sTarget);
    var t = document.getElementById("T" + sTarget);
    var d = document.getElementById("D" + sTarget);
    if (t) { //vk 02.09
        t.outerHTML = document.getElementById("hResult_" + sTarget).value;
        if (document.getElementById("hHeight_" + sTarget).value > "") //vk 09.11
            d.style.height = document.getElementById("hHeight_" + sTarget).value + "px";
        if (document.getElementById("hWidth_" + sTarget).value > "") //vk 09.11
            d.style.width = document.getElementById("hWidth_" + sTarget).value + "px";
    } else {
        s.innerHTML = document.getElementById("hResult_" + sTarget).value;
    }
    s.value = document.getElementById("hCurrentText_" + sTarget).value;
}

function MyMsgBox(sText, sQue, oFocus, HeaderText) {

    var AlertType = "";

    if (sQue == undefined)
        sQue = "false";

    if (oFocus) {
        oFocus.blur();
    }
    document.getElementById("Htext").value = ((HeaderText) ? HeaderText : sText);
    document.getElementById("Hresult100").value = "";

    if (sQue != "true") {
        if (oFocus) {
            gFocus = oFocus;
            setTimeout("MyMsgBox_cont()", 100);
        }
        else if (sQue == "submit") {
            setTimeout("MyMsgBox_submit()", 100);
        }

        if (sQue == "ArchiveFileExists") {
            AlertType = sQue
            sQue = "true";
        }
    }

    document.getElementById("ifr100").src = "alert.htm?que=" + sQue + "&dir=" + gDir + "&yes=" + gYes + "&no=" + gNo + "&ok=" + gYes + "&1=" + g1 + "&2=" + g2 + "&3=" + g3;
    $("#ModalHeaderText").text(sText);
    $('#Modal').modal('show');

    if (AlertType == "ArchiveFileExists") {

        //$('#Modal').on('hidden.bs.modal', function () {
        //    ArchiveSubmit("true")
        //});

        $("#OkButton").click(function () {
            setTimeout(
                function () {
                    ArchiveSubmit("true")
                }, 500);
        });
        $("#CancelButton").click(ResetArchive);
    }

    if (sQue == "true")
        $("#OkButton").click(AfterConfirm)

    return false;
}
function MyMsgBox_cont() {
    switch (document.getElementById("Hresult100").value) {
        case "":
            setTimeout("MyMsgBox_cont()", 100);
            return;
        case "ok":
            if (gFocus) {
                try { //03.15
                    gFocus.focus();
                } catch (e) { }
            }
            return;
        default:
            return;
    }
}
function MyMsgBox_submit() { //vk 11.15
    switch (document.getElementById("Hresult100").value) {
        case "":
            setTimeout("MyMsgBox_submit()", 100);
            return;
        case "ok":
            fnBtnClick(document.getElementById("C02"));
            return;
        default:
            return;
    }
}
function ModalTop(o, d) { //vk 06.09
    var iTop = o.offsetTop + o.clientHeight + 5;
    with (o.parentElement)
    if (id == "scroll")
        iTop += offsetTop - scrollTop;
    if (iTop > document.documentElement.clientHeight - d)
        iTop -= (d + o.clientHeight);
    //else if (!NoPopups())
    //    iTop += 30;
    if (iTop < 0) iTop = 0; //vk 04.10
    return iTop;
}
function ShowHelp(o) {

    var t = o.getAttribute("helptext");
    //for (; t.indexOf('\\n') >= 0;)
    //    t = t.replace('\\n', '\n');  /* ntg 18.05.23 */
    t = t.replace(/([^\.])\\n/g, '$1 ').replace(/\.\\n/g, '.\n'); /* ntg 18.05.23 some lines had a newline instead of space, so now its fixed*/

    var IFrameObj = $("#ifr100")

    $("#Htext").val(t);
    IFrameObj.attr("src", "help.htm?dir=rtl");

    $("#CancelButton").hide();
    $("#ModalHeaderText").text(msg(23));
    $('#Modal').modal('show');
}

//function fixHelpText() {
//    helpTxt = document.getElementsByClassName("specialHelpText");
//    for (var i = 0; i < helpTxt.length; i++) {
//        helpTxt[i].innerHTML = helpTxt[i].innerHTML.replace(/([^\.])\\n/g, '$1 ').replace(/\.\\n/g, '.\n'); /* ntg 18.05.23 some lines had a newline instead of space, so now its fixed*/
//    }
//}

function ShowError(text) {
    var IFrameObj = $("#ifr100")

    $("#Htext").val(text);
    IFrameObj.attr("src", "help.htm?dir=rtl&type=error");

    $("#CancelButton").hide();
    $("#ModalHeaderText").text(msg(26));
    $('#Modal').modal('show');
}

function MyCount() {
    sPingAlertText = document.getElementById("PingAlertText").value.replaceAll("\\n", " <br> ");
    nPingAlertCount = parseInt(document.getElementById("PingAlertCount").value);
    nPingSeconds = parseInt(document.getElementById("PingSeconds").value);
    nPingMaxCount = parseInt(document.getElementById("PingMaxCount").value);

    document.getElementById("ifr100").src = "count.htm?txt=" + sPingAlertText + "&sec=" + (nPingAlertCount * nPingSeconds) + "&dir=" + gDir + "&ok=" + gYes;
    $("#ModalHeaderText").text(msg(24));
    $('#Modal').modal('show');
    $("#OkButton").click(function () { SetPingCounter("0") })
}

function SetPingCounter(PingSet) {
    switch (PingSet) {
        case "max":
            nPings = nPingMaxCount;
            timerModalOpen = false;
            SignOut();
            return;
        case "0":
            nPings = 0;
            return;
    }
}

//vk 07.13
function ArchiveBrowse() {
    var x = document.getElementById("ifrArchive");
    if (x.contentWindow)
        x = x.contentWindow;
    var o = x.document.getElementById("hArchiveFile"); //vk 03.20
    if (o)
        o.click();
}
function ArchiveSubmit(sReplace) {
    var sFile = document.getElementById(document.getElementById("Harchive").value).value;
    var sIllegal = "\\/:*?\"<>|";
    var i;
    for (i = 0; i < sIllegal.length; i++) {
        if (sFile.indexOf(sIllegal.substr(i, 1)) > -1) {
            MyMsgBox(msg_Archive("Illegal symbol"));
            return;
        }
    }

    if (sReplace == "true")
        ArchiveSubmitOnce("true");
    else {
        document.getElementById("HarchiveCheckResult").value = "";
        document.getElementById("HarchiveKeyAndName").value = document.getElementById("HarchiveKey").value + "\\" + sFile;

        document.forms.frmArchiveCheck.submit();
    }
}
function fnTrimString(sValue) //thanx to sk, vk 09.18
{
    var stResult = sValue.toString();
    if (stResult.length <= 0) return "";
    while (stResult.substr(0, 1) == " ") {
        stResult = stResult.substr(1);
    }
    var len = stResult.length;
    while (stResult.substr(--len) == " ") {
        stResult = stResult.substr(0, len);
        len = stResult.length;
    }
    return stResult;
}
function ArchiveSubmitOnce(sReplace) {
    var x = document.getElementById("ifrArchive");
    if (x.contentWindow)
        x = x.contentWindow;
    x.document.getElementById("hArchiveKey").value = document.getElementById("HarchiveKey").value;
    x.document.getElementById("hArchiveName").value = document.getElementById(document.getElementById("Harchive").value).value;
    x.document.getElementById("hArchiveDescr").value = document.getElementById("HarchiveDescr").value;
    x.document.getElementById("hArchiveDB").value = document.getElementById("HarchiveDB").value; //vk 07.17
    x.document.getElementById("hArchiveMail").value = document.getElementById(document.getElementById("HarchiveMail").value).value; //vk 10.17
    x.document.getElementById("hArchiveWhat").value = document.getElementById("HarchiveWhat").value; //vk 03.20
    x.document.getElementById("hArchiveSystem").value = document.getElementById("HarchiveSystem").value; //vk 12.18
    //vk 09.18, 12.19
    var s = "";
    for (var i = 1; i <= 5; i++) {
        var o = document.getElementById("HarchiveText" + i);
        if (o) {
            var oo = document.getElementById(o.value);
            if (oo) {
                var t = fnTrimString(oo.value + "");
                if (s != "" && t != "")
                    s = s + " " + t;
                else
                    s = s + t;
            }
        }
    }
    for (; s.indexOf("&") >= 0;) s = s.replace("&", "&amp;");
    for (; s.indexOf("\"") >= 0;) s = s.replace("\"", "&quot;");
    for (; s.indexOf("'") >= 0;) s = s.replace("'", "&#39;");
    for (; s.indexOf("<") >= 0;) s = s.replace("<", "&lt;");
    for (; s.indexOf(">") >= 0;) s = s.replace(">", "&gt;");
    x.document.getElementById("hArchiveText").value = s;
    //vk 09.18 till here
    x.document.getElementById("hArchiveDocType").value = document.getElementById(document.getElementById("HarchiveDT").value).value; //vk 07.17
    x.document.getElementById("hArchiveReplace").value = sReplace;
    document.getElementById("HarchiveResult").value = "";
    document.getElementById("C_ArchiveSubmit").disabled = true;
    //document.all.C_ArchiveSubmitR.disabled=true;

    x.document.forms[0].submit();
}
function ArchiveAfterSubmit() {
    //if (event.propertyName!="value") return;
    var sResult = document.getElementById("HarchiveResult").value;
    switch (sResult) {
        case "":
            return;
        case "OK":
            MyMsgBox(msg_Archive(sResult), "ok");
            break;
        case "File exists":
            MyMsgBox(msg_Archive(sResult));
            break;
        default:
            MyMsgBox(sResult);
            break;
    }
    //    var sFile=document.all.item(document.all.Harchive.value).value;
    document.getElementById(document.getElementById("Harchive").value).value = "";
    document.getElementById(document.getElementById("Harchive").value).title = "";
    document.getElementById(document.getElementById("Harchive").value).disabled = true;
    //document.all.item(document.all.Harchive.value).className="InpCom"; //rem vc 07.15
    //    if (sResult=="OK" && document.all.HarchiveList.value.indexOf(";"+sFile+";")==-1)
    //        document.all.HarchiveList.value+=sFile+";";
}
function ArchiveAfterCheck() {
    //if (event.propertyName!="value") return;
    switch (document.getElementById("HarchiveCheckResult").value) {
        case "":
            return;
        case "yes":
            MyMsgBox(msg_Archive("File exists"), "ArchiveFileExists", null, msg_Archive("Replace Question"));
            break;
        case "no":
            ArchiveSubmitOnce("false");
            break;
        default:
            MyMsgBox(document.getElementById("HarchiveCheckResult").value);
            break;
    }
}

function ResetArchive() {
    var x = document.getElementById("ifrArchive");
    if (x.contentWindow)
        x = x.contentWindow;
    x.document.getElementById("hArchiveFile").outerHTML =
        "<INPUT type=file id=hArchiveFile name=hArchiveFile accept='" + document.getElementById("HarchiveMask").value + "' oninput=ArchiveAfterBrowse();>"; //vk 10.17
    document.getElementById(document.getElementById("Harchive").value).value = "";
    document.getElementById(document.getElementById("Harchive").value).title = "";
    document.getElementById("C_ArchiveSubmit").disabled = true;
    document.getElementById(document.getElementById("Harchive").value).disabled = true;
    return;
}

function ArchiveAfter3() {
    switch (document.getElementById("Hresult100").value) {
        case "":
            setTimeout("ArchiveAfter3()", 100);
            return;
        case "1":
            //document.all.item(document.all.Harchive.value).focus(); //rem vk 07.15
            var x = document.getElementById("ifrArchive");
            if (x.contentWindow)
                x = x.contentWindow;
            x.document.getElementById("hArchiveFile").outerHTML =
                "<INPUT type=file id=hArchiveFile name=hArchiveFile accept='" + document.getElementById("HarchiveMask").value + "' oninput=ArchiveAfterBrowse();>"; //vk 10.17
            document.getElementById(document.getElementById("Harchive").value).value = "";
            document.getElementById(document.getElementById("Harchive").value).title = "";
            document.getElementById("C_ArchiveSubmit").disabled = true;
            document.getElementById(document.getElementById("Harchive").value).disabled = true;
            return;
        case "2":
            //document.all.item(document.all.Harchive.value).focus(); //rem vk 07.15
            return;
        case "3":
            //document.all.item(document.all.Harchive.value).focus(); //rem vk 07.15
            ArchiveSubmit("true");
            return;
        default:
            return;
    }
}

function CG_ESB() { //vk 04.21
    document.getElementById("txtResultCG").value = "";
    if (document.getElementById("Hi1_name")) {
        var v = document.getElementById(document.getElementById("Hi1_name").value).value;
        if (v - 0 == 0 || isNaN(v)) {
            MyMsgBox(msg_CG("Terminal"));
            return;
        }
        document.getElementById("Hi1_value").value = v;
    } else {
        MyMsgBox(msg_CG("Terminal"));
        return;
    }
    document.getElementById("frmCG").submit();
}

function CG1() {
    if (bInCG) return;
    document.getElementById("txtResultCG").value = ""; //08.13
    document.getElementById("txtWhatCG").value = "Url";
    if (document.getElementById("Hi1_name")) {
        var v = document.getElementById(document.getElementById("Hi1_name").value).value;
        if (v - 0 == 0 || isNaN(v)) {
            MyMsgBox(msg_CG("Terminal"));
            return;
        }
        document.getElementById("Hi1_value").value = v;
    } else {
        MyMsgBox(msg_CG("Terminal"));
        return;
    }
    DisableButtons();
    bInCG = true;
    document.getElementById("frmCG").submit();
    //document.all.ifr100.src="empty.htm";
}
function CG2() {
    //if (event.propertyName!="value") return;
    var v = document.getElementById("txtResultCG").value;
    if (v == "") return;
    document.getElementById("txtWhatCG").disabled = false;
    var o = document.getElementById(document.getElementById("Hcg").value);
    sFieldValue = ""; //vk 09.21
    switch (document.getElementById("txtWhatCG").value) {
        case "Url":
            if (v.substr(0, 5) == "ERROR") {
                // Close100b();
                bInCG = false;
                //ReturnFields(true);
                ReturnFields(true, v); //ntg 30.04.24 vladi change regarding CG functionallity
                //fnFocus_tab(o,false,"",true); //08.13
                break;
            }
            document.getElementById("txtResultCG").value = "";
            document.getElementById("txtWhatCG").value = "Screen";
            document.getElementById("Htext").value = "";
            document.getElementById("ifr100").src = v;
            //"cg_ok.htm?uniqueID=17c0aa5e-42b5-468d-8dc2-71421e09fc19&lang=HE&authNumber=&cgUid=719774&responseMac=716f3334a6bcda4e4bf3ee900071591504b73b8d2e5652d3a5a8aed7a6555194&cardToken=1081067095973698&cardExp=0118&personalId=&cardMask=532610******3698&txId=0c7c4037-16de-4cb6-991c-8a6d7746bbd3&numberOfPayments=0&firstPayment=&periodicalPayment=";
            //document.getElementById("ifr100").style.height = $("#HcgHeight").val() + "px";
            //document.getElementById("ifr100").style.cssText = 'height:' + $("HcgHeight").val() + "px !important";
            //document.getElementById("ifr100").style.cssText = 'height:' + document.getElementById("HcgHeight").value + "px !important"; //ntg 30.04.24 vladi change regarding CG functionallity
            var IFrameObj = document.getElementById('ifr100'); //ntg 30.04.24 vladi change regarding CG functionallity
            modalHeight = document.getElementById("HcgHeight").value + "px"; //ntg 30.04.24 vladi change regarding CG functionallity

            IFrameObj.onload = function () { //ntg 30.04.24 vladi change regarding CG functionallity
                if (modalHeight == "") {
                    $("#ifr100").css("height", "");
                    $("#ifr100").css("max-height", "400px");
                } else {
                    $("#ifr100").css("height", modalHeight);
                    $("#ifr100").css("max-height", modalHeight);
                }
            };

            $(".modal-footer").hide();
            $("#ModalHeaderText").text("CreditGuard");
            $('#Modal').modal('show');
            break;
        case "Screen":
            Close100a("");
            $('#Modal').modal('hide');
            modalHeight = ""; //ntg 30.04.24 vladi change regarding CG functionallity
            $(".modal-footer").show(); //ntg 30.04.24 vladi change regarding CG functionallity


            //if (v.substr(0, 5) == "ERROR") { 
            if (v.substr(0, 6) == "CANCEL") {//ntg 30.04.24 vladi change regarding CG functionallity - added: || v.substr(0, 6) == "CANCEL"
                //  Close100b();
                bInCG = false;
                //ReturnFields(true);
                ReturnFields(true, v); //ntg 30.04.24 vladi change regarding CG functionallity
                //fnFocus_tab(o,false,"",true); //08.13
                break;
            }
            document.getElementById("txtResultCG").value = ""; //08.13
            document.getElementById("txtWhatCG").value = "Token";
            document.getElementById("frmCG").submit();
            break;
        case "Token":
            bInCG = false;
            if (v.substr(0, 5) == "ERROR") {
                //ReturnFields(true);
                ReturnFields(true, v); //ntg 30.04.24 vladi change regarding CG functionallity
                //fnFocus_tab(o,false,"",true); //08.13
                break;
            }
            ReturnFields(false);
            if (sError != "") {
                //fnFocus_tab(o,false,"",true); //08.13
                break;
            }
            //var o=document.all.item(document.all.Hcg.value);
            o.value = v.trim();
            // o.className = "InpProt";
            o.onfocus = "";
            o.ondeactivate = "";
            o.onblur = "";
            //o.unselectable="on";
            unsel(o);
            o.tabIndex = "-1";
            //vk 06.22
            if (o.getAttribute("cod"))
                Fire(o.getAttribute("cod") - 0 + 111);
            //fnFocus_tab(o, true, o.getAttribute("cod"), true);
            else
                fnFocus_tab(o, false, "", true);
            break;
    }
}
function ReturnField(sPrefix, bCheck) {
    if (document.getElementById(sPrefix + "_name")) {
        var sTarget = document.getElementById(sPrefix + "_name").value;
        var sValue = document.getElementById(sPrefix + "_value").value;
        var sLength = sTarget.substr(lenS, lenL) - 0;
        if (sTarget.substr(typS, typL) == "N") {
            if (isNaN(sValue)) {
                sError = msg_Num(2)
                    + CRLF + sTarget.substr(numS, numL)
                    + CRLF + sValue;
                sValue = "0";
            } else if (sValue.length > sLength) {
                sValue = sValue.substr(sValue.length - sLength, sLength);
            }
        } else {
            nExtraLen = sValue.replace(/\//g, "").length - sLength; //vk 07.20
            if (nExtraLen > 0) {
                sValue = sValue.substr(0, sLength - nExtraLen);
            }
        }
        if (!bCheck)
            document.getElementById(sTarget).value = sValue;
    }
}
//function ReturnFields(bErrorFromCG) {
function ReturnFields(bErrorFromCG, v) { //ntg 30.04.24 vladi change regarding CG functionallity
    var n;
    if (bErrorFromCG)
        n = 2;
    else
        n = 9; //vk 08.13
    sError = "";
    for (var i = n; i >= 1; i--)
        ReturnField("Hv" + i, true);
    if (sError == "") {
        for (var i = n; i >= 1; i--)
            ReturnField("Hv" + i, false);
        if (bErrorFromCG)
            MyMsgBox(v); //ntg 30.04.24 vladi change regarding CG functionallity
            //MyMsgBox("ERROR:" //--rem ntg 30.04.24 vladi change regarding CG functionallity
            //    + CRLF + document.getElementById("Hv2_value").value
            //    + CRLF + document.getElementById("Hv1_value").value);
    } else {
        document.getElementById(document.getElementById("Hv1_name").value).value = "999";
        document.getElementById(document.getElementById("Hv2_name").value).value = "Comtec: " + sError;
        if (bErrorFromCG)
            MyMsgBox("ERROR:"
                + CRLF + document.getElementById("Hv2_value").value
                + CRLF + document.getElementById("Hv1_value").value
                + CRLF + CRLF + sError);
        else
            MyMsgBox("ERROR:"
                + CRLF + sError);
    }
}
//function SetMiddle_Main(h,w,t) {
//	popup=false;
//	var b=document.body;
//	with (document) {
//	    getElementById("ifr100_dc").style.width = w + "px";
//	    getElementById("ifr100").style.height = h + "px";
//	    getElementById("ifr100").style.width = w + "px";
//	    getElementById("ifr100_height").style.height = h + "px";
//	    getElementById("ifr100_width").style.width = w + "px";
//	    if (t)
//	        getElementById("ifr100_above").style.height = t + "px";
//	    else if (b.clientHeight < (h-0) + 27)
//	        getElementById("ifr100_above").style.height = "100%";
//	    else
//	        getElementById("ifr100_above").style.height = (b.clientHeight - h - 27) / 2 + "px";
//	    //vk 01.11
//	    var bb = parent.document.getElementById("bottom");
//	    if (bb) {
//	        if (bb.clientWidth < (w-0) + 6)
//	            getElementById("ifr100_before").style.width = "0px";
//	        else
//	            getElementById("ifr100_before").style.width = (bb.clientWidth - w - 6) / 2 + "px";
//	    }
//	    getElementById("div100").style.visibility = 'visible';
//	}
//    //document.body.unselectable="on";
//	unsel(document.body);
//    //LockOnce("input");
//	//LockOnce("img");
//	//LockOnce("a");
//	//LockOnce("textarea");
//}
//function NoPopupsTitleClick() { //vk 08.13
//    var ifr100Exit = document.getElementById("ifr100_exit");
//    if (ifr100Exit.style.visibility == "visible")
//        if (event.x >= ifr100Exit.offsetLeft) {
////            if (document.all.item("txtResultCG")) {
////                document.all.Hv1_value.value="";
////                document.all.Hv2_value.value="";
////                document.all.txtResultCG.disabled=false;
////                document.all.txtResultCG.value="ERROR CANCEL";
////            }
////            else
//            Close100('');
//            var resultCG = document.getElementById("txtResultCG");
//            //if (resultCG) {
//            //    var o=document.getElementById($("#Hcg").val());

//            //    fnFocus_tab(o, false, "", true);
//            //    // var o=document.all.item(document.all.Hcg.value);
//            //}
//        }
//}

//vk 11.15
function Sql1() {
    DisableButtons();
    ShowWait();
    document.getElementById("lblSqlMsg").innerText = msg_Sql("wait");
    document.getElementById("lblSqlMsg").style.display = ""; //ntg 19.06.23 it wasnt shown
    document.forms.frmSql.submit();
}
function Sql2() {
    //if (event.propertyName != "value") return;
    switch (document.getElementById("txtWhatSql").value) {
        case "part":
            document.getElementById("lblSqlMsg").innerText = msg_Sql("copied") + ": " + document.getElementById("txtResultSql").value;
            document.getElementById("txtRecordsTotal").value = document.getElementById("txtResultSql").value;
            document.forms.frmSql.submit(); //submit iframe again
            return;
        case "finish":
            document.getElementById("lblSqlMsg").innerText = msg_Sql("copied") + ": " + document.getElementById("txtResultSql").value;
            document.getElementById("HdontAnswer").value = "dont";
            fnBtnClick(document.getElementById("C02"));
            //MyMsgBox(msg_Sql("copied") + ": " + document.all.txtResultSql.value, "submit"); //submit main window after dialog is closed
            return;
        case "error":
            document.getElementById("lblSqlMsg").style.border = "1px solid #FF0000";
            document.getElementById("lblSqlMsg").innerText = document.getElementById("txtResultSql").value; //show error and stop
            return;
        case "":
            return;
        default:
            document.getElementById("lblSqlMsg").style.border = "1px solid #FF0000";
            document.getElementById("lblSqlMsg").innerText = msg_Sql("illegal") + ": " + document.getElementById("txtWhatSql").value; //show error and stop
            return;
    }
}

function unsel(db) {
    var st = db.getAttribute("style");
    st = st + "-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none;";
    db.setAttribute("style", st);
}

//ntg 09.07.23 the header of the pop up of cities and streets window is now displayed inside #ModalHeaderText + 'i' image removed
function addHeaderText() {
    $("#ModalHeaderText").text(msg(28));
    $("#helpPic").css("display", "none");
}