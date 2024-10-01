var qflx;
var CRLF = String.fromCharCode(13) + String.fromCharCode(10); //vk 12.06
//vk 05.09
var gFocus;
var gDefault100;
var gPassForWord;
var gSubmit;
var gKey;
var gWhat;
var bInPing;
var bInSubmit = false; //vk 02.20
var timerModalOpen = false

function SetQflx(b) {
    qflx = b;
}

function PressManually() //vk 05.05
{
    var o = document.getElementById(sButtonVisited);
    if (o)
        if (o.onclick) { //vk 06.09
            var s = o.onclick + "";
            if (s.indexOf("fnBtnClick(this)") >= 0) fnBtnClick(o);
            if (s.indexOf("fnPencilClick(this)") >= 0) fnPencilClick(o); //vk 05.05
            if (s.indexOf("CmdPchClick(this)") >= 0) CmdPchClick(o);
            if (s.indexOf("CmdPchlClick(this)") >= 0) CmdPchlClick(o);
            if (s.indexOf("fnKeyboard_Radio(this,true)") >= 0) fnKeyboard_Radio(o, true); //vk 02.05
            if (s.indexOf("fnKeyboard_Radio(this,false)") >= 0) fnKeyboard_Radio(o, false); //vk 08.06
        }
}

function fnpsl_dblclick_folders(o) //vk 02.04
{
    var oR = document.getElementsByName("R");
    if (!IsEnable() || oR == null) {
        return;
    }

    pch = $(o).attr("pch");
    if (pch == null) {
        var hrdf = document.getElementById("Hrdf").value;
        if (hrdf != " ") {
            $(o).attr("pch", hrdf);
        }
    }
    if (pch != null) {
        gFocus = o; //vk 05.09
        CmdPchClick_Common("Hfpch", "00", false, false, 0);
    } else
        MyMsgBox(msg(5));
}
function Fire(code) {
    document.getElementById("Hx").value = ''; //vk 10.04
    fnWindowKeyDown('', 0, 0, code, false);
}

function fnpsl_dblclick() {
    if (!IsEnable()) {
        return;
    }
    var oR = document.getElementsByName("R");
    if (oR != null) {
        Fire(13);
    }
}

function ListClick(sName, nValue) //vk 09.06
{
    if (!IsEnable()) {
        return;
    }
    document.getElementById(sName).value = nValue;
    document.getElementById("Hx").value = '';
    Fire(13);
}

function fnpf4_dblclick() {

    if (!IsEnable()) {
        return;
    }
    document.getElementById("Hx").value = ''; //vk 10.04
    Fire(115);
}

function fnpEnter_dblclick() { //rk 24.07.24 changes for menu route field

    if (!IsEnable()) {
        return;
    }
    document.getElementById("Hx").value = ''; //vk 10.04
    Fire(13);
}

function fnpf4_click(s) {
    if (!IsEnable()) return;
    if (document.getElementById("Y" + s))
        document.getElementById("Y" + s).focus(); //vk 07.09
    else if (document.getElementById("P" + s))
        document.getElementById("P" + s).focus(); //vk 03.10
    else
        document.getElementById(s).focus();
    fnFocus_Common(document.getElementById(s), 0, 0, true);
    fnpf4_dblclick(); //vk 05.04
}

function Close100(retVal) //vk 06.09
{
    Close100a(retVal);
    //Close100b();
}
//vk 07.13
function Close100a(retVal) {
    document.getElementById("Hresult100").value = retVal;
    //document.getElementById("div100").style.visibility = 'hidden';
    //document.getElementById("ifr100_exit").style.visibility = 'hidden';
    document.getElementById("ifr100").src = 'Empty.htm';
    //document.getElementById("ifrRefresh").src = 'Empty.htm';
}
//function Close100b() {
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

function fnWindowKeyDown(sField, per, cod, key, KeyboardPress) {
    //event.preventDefault();
    //debugger;
    if (!IsEnable()) return;

    if (key == 0 || key == undefined)
        //if (key == 0 )
        gKey = event.keyCode;
    else
        gKey = key;

    if (gKey == 113 && document.getElementById("lblSqlMsg") != null) { //vk 08.22
        Sql1();
        return;
    }
    if (gKey == 16 || gKey == 17 || gKey == 18)
        return; //vk 05.09
    var bCardReader = sField != "" && gKey != 33 && gKey != 34 && (gKey < 112 || gKey > 123); //vk 05.09

    gSubmit = false;
    //if (document.getElementById("div100"))
    //    if (document.getElementById("div100").style.visibility == "visible") { //vk 05.09
    //        if (gKey == 27)
    //            Close100(gDefault100);
    //        event.keyCode = 0;
    //        event.returnValue = false;
    //        return;
    //    }
    if (document.getElementById("W"))
        if (event.shiftKey || (gKey != 113 && gKey != 123)) {
            if (gKey >= 112 && gKey <= 123) {
                event.keyCode = 0;
                event.returnValue = false;
            }
            return;
        }
    //if (sComboOpen != null) {
    //    var oDiv = document.getElementById(sComboOpen);
    //    var oTab = document.getElementById("T" + oDiv.id.substring(1, oDiv.id.length));
    //    if (!event.ctrlKey && !event.altKey && !event.shiftKey) {
    //        var step = StepByKey(gKey);
    //        //if (gKey == 27)
    //        //    selectS_click(oDiv.id.substring(1, oDiv.id.length)); // Esc
    //        //if (!(step == null)) //vk 07.10
    //        //    OnArrow(oDiv, oTab, step);
    //    }
    //    return;
    //}
    if (gKey == 145) {
        TestSubmit();
        return;
    }
    //if (gKey == 13 && document.activeElement.name == sButtonTab && document.activeElement.name != null) {
    //    PressManually();
    //    return;
    //}
    if (gKey == 8 && (event.srcElement.tagName.toLowerCase() != 'input' || event.srcElement.readOnly)) {
        event.keyCode = 0;
        event.returnValue = false;
        return;
    }
    if (bCardReader) {
        if (gKey == 13) {
            event.keyCode = 0;
            event.returnValue = false;
            document.getElementById(sField).value = sCard;
            if (per) {
                document.getElementById.Hfcmd.value = cod;
                gSubmit = true;
            } else {
                bNewCard = true;
            }
        } else {
            if (bNewCard)
                sCard = "";
            bNewCard = false;
            if (gKey >= 48 && gKey <= 57) sCard += (gKey - 48 + "");
            else if (gKey >= 96 && gKey <= 105) sCard += (gKey - 96 + "");
            else if (gKey == 187) sCard += "=";
            else sCard += "?";
        }
    } else {
        if (gKey == 13) { // Enter
            event.keyCode = 0; //vk 09.10
            event.returnValue = false; //vk 09.10
            //if (GetHid("Henter", "true") != "true")
            //    return;
            document.getElementById("Hfcmd").value = "00";
            gSubmit = true;
        }
        if (gKey == 33) { // PageUp
            if (NoButton(25)) {
                event.keyCode = 0;
                event.returnValue = false;
                return;
            }
            document.getElementById("Hfcmd").value = "91";
            gSubmit = document.getElementById("Hspool").value == "false" && !qflx;
        }
        if (gKey == 34) { // PageDown
            if (NoButton(26)) {
                event.keyCode = 0;
                event.returnValue = false;
                return;
            }
            document.getElementById("Hfcmd").value = "90";
            gSubmit = document.getElementById("Hspool").value == "false" && !qflx;
        }
    }
    if (gKey >= 112 && gKey <= 123) {
        event.keyCode = 0; //vk 09.10
        event.returnValue = false; //vk 09.10
        var n = gKey - 111;
        if (event.ctrlKey || event.altKey)
            return;
        if (event.shiftKey)
            n += 12;
        if (KeyboardPress && NoButton(n)) // Check againts the fnn Paramter only if the F button (keyboard) was clicked
            return;
        var sControl;
        if (n < 10)
            sControl = "C0" + n;
        else
            sControl = "C" + n;
        gFocus = document.getElementById(sControl); //vk 05.09
        document.getElementById("Hfcmd").value = sControl.substr(sControl.length - 2, 2);
        if (gFocus) {
            if (gFocus.disabled && key == 0)
                return;
            if (gFocus.getAttribute("gridpos"))
                document.getElementById("Hgridpos").value = gFocus.getAttribute("gridpos");
        }
        gWhat = "Kbd";
        //if (NoPopups()) {
        // setTimeout("AfterConfirm()", 100);
        //fConfirm();
        //} else {
        //    fConfirm();
        AfterConfirm(); //vk 05.12 ?
        //}
    } else {
        gWhat = "Kbd";
        if ((gKey >= 112 && gKey <= 123) || gSubmit) {
            $("#Hresult100").val("y");
            document.getElementById("Hresult100").value = "y";
        } else {
            document.getElementById("Hresult100").value = "";
        }
        AfterConfirm();
    }
}

function NoButton(n) //vk 12.07
{
    var s;
    s = GetHid("Hfnn", "111111111111111111111111111111").charAt(n - 1);
    return s != '1' && s != '2';
}

function DisableButtonsOnly(dis) //vk 05.06
{
    var coll;
    coll = document.getElementsByTagName("INPUT");
    if (coll) {
        var count = coll.length;
        if (count)
            for (var i = 0; i < count; i++)
                with (coll[i])
                if (type == 'button')
                    disabled = dis;
    }
}

function DisableButtons() {
    var count;
    var cursorWait = getCursorTypeWait();
    var oIfrRefresh = document.getElementById("ifrRefresh");
    if (oIfrRefresh) {
        if (oIfrRefresh.src == "Refresh.aspx") {
            return; //vk 03.06
        }
        oIfrRefresh.src = "Refresh.aspx"; //vk 01.06
    }

    var scroll = document.getElementById("scroll");
    if (scroll) {
        scroll.disabled = true;
    }
    var coll;
    coll = document.getElementsByTagName("INPUT");
    if (coll) {
        var count = coll.length;
        if (count)
            for (var i = 0; i < count; i++)
                with (coll[i]) {
                    //if (type != 'hidden') {
                    style.cursor = cursorWait; //"wait";
                    //coll[i].origdis = (disabled?"true":"false"); //vk 07.13
                    //origdisSet(coll[i]); //vk 06.15
                    //}
                    if (type == 'button' || coll[i].getAttribute("ro") == "true")
                        if (id != 'msbug')
                            disabled = true; //vk 07.04, 02.20
                }
    }
    LockOnce("img");
    LockOnce("a");
    LockOnce("textarea");
    //LockOnce("span"); //vk 04.20
    //LockOnce("label"); //vk 10.20
    //LockOnce("div"); //vk 04.20
    //LockOnce("option"); //vk 04.20
    //LockOnce("select"); //vk 04.20

    // coll = document.all;
    coll = document.getElementsByTagName('*');
    if (coll) {
        var count = coll.length;
        if (count) {
            for (var i = 0; i < count; i++) {
                //with (coll[i])
                if (coll[i].tagName == "SELECT" || coll[i].tagName == "SPAN" || coll[i].tagName == "LABEL") {
                    coll[i].style.cursor = cursorWait; //"wait";
                }
            }
        }
    }
} // DisableButtons()

function getCursorTypeWait() {
    //---------------------------
    //Ilia Bulaevskiy 21/08/2018.
    //---------------------------
    var cursor = "wait";
    jQuery.browser = {};
    jQuery.browser.chrome = /chrome/.test(navigator.userAgent.toLowerCase());
    if (jQuery.browser.chrome) {
        cursor = "default";
    }
    return cursor;
}
/*
function ResetCursor(o) { //vk 04.20
    var s = o.getAttribute("origcur");
    if (s == null)
        s = ""; //"default";
    //else if (s == "")
    //    s = "default";
    o.style.cursor = s;
}
*/
function LockOnce(s) {
    // var coll = document.all.tags(s);
    var cursorWait = getCursorTypeWait();
    var coll = document.getElementsByTagName(s);
    if (coll) {
        var count = coll.length;
        if (count)
            for (var i = 0; i < count; i++) {
                //coll[i].origdis = (coll[i].disabled?"true":"false"); //vk 07.13
                //origdisSet(coll[i]); //vk 06.15
                //coll[i].origcur = coll[i].style.cursor; //vk 07.13
                coll[i].disabled = true;
                coll[i].style.cursor = cursorWait; //"wait";
                //vk 07.13
                //if (coll[i].className == "BtnComFoc") coll[i].className = "BtnCom";
            }
    }
}
/*
function UnlockOnce(s) //vk 06.09
{
    // var coll = document.all.tags(s);
    var coll = document.getElementsByTagName(s);
    //var coll = $(s);
    if (coll) {
        var count = coll.length;
        if (count)
            for (var i = 0; i < count; i++) {
                //vk 07.13, 04.20
                if (coll[i].getAttribute("origdis") == "false") //{
                    coll[i].disabled = false;
                //} else {
                //coll[i].style.cursor = coll[i].getAttribute("origcur");
                //ResetCursor(coll[i]); //vk 04.20
                //}
            }
    }
}
*/
function fnIsAction() {
    var key = event.keyCode;
    if (key == 13) return true;  // Enter
    if (key == 33) return true;  // PageUp
    if (key == 34) return true;  // PageDown
    if (key >= 112 && key <= 123 && !event.ctrlKey && !event.altKey) return true; // F1,F2,...,F12
    return false;
} // fnIsAction

function fnCanSubmit() {
    //debugger;
    if (!IsEnable() || !gIsNumber || !gIsDate || !gIsTime || !gIsText)
        return false;
    //vk 06.04
    if (document.getElementById("Hforbidden").value.indexOf(';' + document.getElementById("Hfcmd").value + ';') < 0)
        return true;
    document.getElementById("Hfcmd").value = '';
    MyMsgBox(msg(7));
    return false;
}

function CheckTheRow(o) { //vk 08.24+++ fixed
    var TrElement = o.parentElement.parentElement; //$(o).parent().parent()
    var radioElement = $(TrElement).find("#R").first(); //TrElement.find("#R").first();
    if (radioElement && $(radioElement).attr('type') == "radio") { //radioElement.attr('type') == "radio")
        $(radioElement).prop("checked", true); //radioElement.prop("checked", true);
        $(TrElement).addClass("trSelected");
    }
}

function CmdPchClick(o) {
    CmdPchClick_CommonWithConfirm(o, "Pch");
}
function CmdPchlClick(o) {
    //var pch = $("#ActionChooseBtn").attr("pch");
    var pch = $(o).attr("pch"); //ntg 23.02.23 adding double click to list

    if (pch == "") {
        MyMsgBox(msg(27));
        return;
    }

    $(".list-group-item[pch=" + pch + "]").focus();

    CmdPchClick_CommonWithConfirm(o, "Pchl");
}

//function CmdPchlClickEnglish(o) {
//    var pch = $("#ActionChooseBtn").attr("pch");

//    if (pch == "") {
//        alert('Please select an action and try again');
//        return;
//    }

//    $(".list-group-item[pch=" + pch + "]").focus();

//    CmdPchClick_CommonWithConfirm(o, "Pchl");
//}

function fnBtnClick(o) {
    CmdPchClick_CommonWithConfirm(o, "Btn");
}
function fnPencilClick(o) {
    CmdPchClick_CommonWithConfirm(o, "Pencil");
}

function CmdPchClick_CommonWithConfirm(o, sWhat) //vk 05.09
{
    var h;
    if (sWhat == "Pchl") {
        h = document.getElementById("Honly");
        if (h) { } else return;
    }

    gFocus = o;
    gWhat = sWhat;
    if (fConfirm())
        AfterConfirm();
}

function AfterConfirm() {

    //switch (document.getElementById("Hresult100").value) {
    //    case "":
    //        setTimeout("AfterConfirm()", 100);
    //        return;
    //    case "n":
    //        return;
    //    default:
    //        break;
    //}
    if (gWhat == "Kbd") {
        if (gKey >= 112 && gKey <= 123) {
            if (!CopyWordControl())
                return;
            gSubmit = true;
        }
        if (gSubmit) {
            if (fnCanSubmit()) {
                //if (giTimeID != 0) {
                clearInterval(giTimeID);
                //    giTimeID = 0;
                //}
                doSubmit();
            }
        }
    } else {
        if (!CopyWordControl())
            return;
        switch (gWhat) {
            case "Pch": CmdPchClick_Common("Hfpch", "00", true, false, 0); break;
            case "Pchl": CmdPchClick_Common(document.getElementById("Honly").value,
                "00", false, false, document.getElementById("Honly").getAttribute("len")); break;
            case "Btn": CmdPchClick_Common("Hfcmd", "", false, false, 0); break;
            case "Pencil": CmdPchClick_Common("Hfcmd", "", false, true, 0); break;
        }
    }
}

function fConfirm() //vk 09.05, 11.06, 05.09
{
    var n, ans;
    gPassForWord = false;

    document.getElementById("Hresult100").value = "y";
    if (gFocus) {
        if (gFocus.name) {
            //n = gFocus.name;
            n = $(gFocus).attr("name");
        } else {
            // n = "C" + gFocus.pch;
            n = "C" + $(gFocus).attr("pch");
        }
        gPassForWord = n == "C02";
        if (document.getElementById('Hcnf_' + n)) {
            var cnf = document.getElementById('Hcnf_' + n).value;
            if (cnf != "1" && cnf != "Y") {
                var arrArgs = new Array();
                // arrArgs[0] = (gFocus.cnfcap ? gFocus.cnfcap : gFocus.value);
                arrArgs[0] = ($(gFocus).attr("cnfcap") ? $(gFocus).attr("cnfcap") : $(gFocus).attr("value"));
                //FillForModalPing(arrArgs);
                bInCalendar = true;
                ans = window.showModalDialog('confirm' + cnf + '.htm', arrArgs,
                    'scroll:no;center:yes;status:no;help:no;dialogHeight:' + XP(400) + 'px;dialogWidth:700px;'); //vk 11.05
                bInCalendar = false;
                document.getElementById("Hresult100").value = ans;
                return ans;
            }
            return MyMsgBox(msg(6) + CRLF + gFocus.value, "true");
        }
    }
    return true;
}

function CopyWordControl() {
    if (document.getElementById("W")) {
        if (!gPassForWord) {
            document.getElementById('Htextused').value = '0';
            return true; //vk 03.09
        }
        var t = document.getElementById("W").value;
        //vk 03.08
        for (; t.indexOf(String.fromCharCode(8208)) >= 0;) t = t.replace(String.fromCharCode(8208), "-");
        for (; t.indexOf(String.fromCharCode(8209)) >= 0;) t = t.replace(String.fromCharCode(8209), "-");
        for (; t.indexOf(String.fromCharCode(8210)) >= 0;) t = t.replace(String.fromCharCode(8210), "-");
        for (; t.indexOf(String.fromCharCode(8211)) >= 0;) t = t.replace(String.fromCharCode(8211), "-");
        for (; t.indexOf(String.fromCharCode(8212)) >= 0;) t = t.replace(String.fromCharCode(8212), "-");
        for (; t.indexOf(String.fromCharCode(8213)) >= 0;) t = t.replace(String.fromCharCode(8213), "-");
        for (; t.indexOf(String.fromCharCode(8214)) >= 0;) t = t.replace(String.fromCharCode(8214), "||");
        for (; t.indexOf(String.fromCharCode(8215)) >= 0;) t = t.replace(String.fromCharCode(8215), "_");
        for (; t.indexOf(String.fromCharCode(8216)) >= 0;) t = t.replace(String.fromCharCode(8216), "'");
        for (; t.indexOf(String.fromCharCode(8217)) >= 0;) t = t.replace(String.fromCharCode(8217), "'");
        for (; t.indexOf(String.fromCharCode(8218)) >= 0;) t = t.replace(String.fromCharCode(8218), "'");
        for (; t.indexOf(String.fromCharCode(8219)) >= 0;) t = t.replace(String.fromCharCode(8219), "'");
        for (; t.indexOf(String.fromCharCode(8220)) >= 0;) t = t.replace(String.fromCharCode(8220), "\"");
        for (; t.indexOf(String.fromCharCode(8221)) >= 0;) t = t.replace(String.fromCharCode(8221), "\"");
        for (; t.indexOf(String.fromCharCode(8222)) >= 0;) t = t.replace(String.fromCharCode(8222), "\"");
        for (; t.indexOf(String.fromCharCode(8223)) >= 0;) t = t.replace(String.fromCharCode(8223), "\"");
        for (; t.indexOf(String.fromCharCode(8226)) >= 0;) t = t.replace(String.fromCharCode(8226), "*");
        for (; t.indexOf(String.fromCharCode(8227)) >= 0;) t = t.replace(String.fromCharCode(8227), "*");
        for (; t.indexOf(String.fromCharCode(8228)) >= 0;) t = t.replace(String.fromCharCode(8228), ".");
        for (; t.indexOf(String.fromCharCode(8229)) >= 0;) t = t.replace(String.fromCharCode(8229), "..");
        for (; t.indexOf(String.fromCharCode(8230)) >= 0;) t = t.replace(String.fromCharCode(8230), "...");
        for (; t.indexOf(String.fromCharCode(8231)) >= 0;) t = t.replace(String.fromCharCode(8231), "-");
        for (; t.indexOf(String.fromCharCode(183)) >= 0;) t = t.replace(String.fromCharCode(183), "*");
        //for (i=0;i<t.length;i++) if (t.charCodeAt(i)>255) alert(t.charCodeAt(i))
        var cou = document.getElementById('Htextcoumax').value;
        var len = document.getElementById('Htextlen').value - 0;
        var nPos, nPlus, i;
        for (i = 1; i <= cou; i++) {
            var s = "0000" + i;
            nPos = t.indexOf("\n"); //(CRLF);
            if (nPos >= 0 && nPos <= len + 1) {
                nPlus = 1; //2;
            } else if (t.length <= len) {
                nPos = t.length;
                nPlus = 0;
            } else {
                nPos = t.substring(0, len + 1).lastIndexOf(' ');
                if (nPos >= 0) {
                    nPlus = 1;
                } else {
                    nPos = len;
                    nPlus = 0;
                }
            }
            document.getElementById('W_' + s.substring(s.length - 4)).value = t.substring(0, nPos);
            t = t.substring(nPos + nPlus);
            if (t.length == 0) {
                document.getElementById('Htextused').value = i;
                break;
            }
        }
        if (t.length > 0) {
            MyMsgBox(msg(19) + CRLF + CRLF + t, "false", document.getElementById("W"));
            //document.all("W").focus();
            return false;
        }
        else
            return true; //vk 03.09
    }
    else
        return true; //vk 03.09
}

function TestSubmit() //vk 06.04
{
    CmdPchClick_Common("", "@@", false, false, 0);
}

function doSubmit() {
    if (!IsEnable()) return;

    //if (!($("#Hfcmd").val() == "24" && $("#HFlexible").val() == "1")) {
    var valuesToCheck = ["01", "03", "17", "80"]; /*ntg 10.07.23 only in certain cases we want the SDOT HOVA to be activated*/
    var hfcmdValue = $("#Hfcmd").val();

    if ($("#HFlexible").val() == "1" && (valuesToCheck.includes(hfcmdValue) || (hfcmdValue >= 50 && hfcmdValue <= 61))) {
        var MainForm = document.forms[0]
        var CheckValidForm = MainForm.checkValidity()
        if (CheckValidForm === false) {

            // Color only the invalid form element
            var invalidGroup = document.querySelectorAll(":invalid");
            for (var j = 0; j < invalidGroup.length; j++) {
                invalidGroup[j].classList.add('is-invalid');
                invalidGroup[j].style.backgroundImage = 'none'; // ntg 05.07.23 removing the exclamation point in the SADE HOVA

                //ntg 18.12.23 this code section creates the "is-invalid" msg under the field to appear only under the first radio btn in case of an invalid radio btns
                if (invalidGroup[j].type === 'radio' && $(invalidGroup[j]).hasClass('form-check-input')) {
                    var isFirstInvalidRadioButton = $(invalidGroup[j]).closest('.form-control').find('.form-check-input.is-invalid').index(invalidGroup[j]) === 0;

                    if (isFirstInvalidRadioButton) {
                        $(invalidGroup[j]).siblings('.invalid-feedback.InvalidMan').show();
                    } else {
                        $(invalidGroup[j]).siblings('.invalid-feedback.InvalidMan').hide();
                    }
                }
            }
            invalidGroup[1].focus();
            //       MainForm.classList.add('was-validated');
            return;
        }
    }
    Accords_Gather(); //vk 09.24

    if (document.getElementById("Hx")) {
        document.getElementById("Hx").value = '';

        var s = location.pathname;
        var i = s.lastIndexOf("/");
        var s0 = s.toLowerCase().substring(i);

        if (s0.substr(0, 6) == "/logon" && s0.substr(s0.length - 5) == ".aspx")
            s = s.substring(0, i) + "/screen.aspx";

        var RandomNumber = Math.random();

        document.forms[0].action = s + "?action=next&random=" + RandomNumber.toString();

        var hcur = document.getElementById("Hcur");
        var o = document.getElementById(hcur.value);
        if (o != null)
            if (o.type == "hidden")
                o = document.getElementById("Y" + hcur.value);
            else
                try {
                    o.blur();
                } catch (e) { }
        if (o != null) CopyOut(o);
        document.getElementById("gIsEnable").value = 0;

        DisableButtons();
        ShowWait();
        WaitAndSubmit();
    }
}
function WaitAndSubmit() //vk 04.05, 03.10
{
    if (bInPing)
        ;
    else
        bInPing = false;

    //oRefresh = document.getElementById("ifrRefresh");
    //if (oRefresh) {
    //    var x = oRefresh;
    //    if (x.contentWindow)
    //        x = x.contentWindow;
    //    bWait = bInPing || x.document.readyState != 'complete' || oRefresh.src != "Refresh.aspx";
    //}
    //else
    bWait = bInPing;
    //gSubmit = true; //vk 02.20
    if (bWait)
        setTimeout("SubmitWithFrame();", 200); //setTimeout("WaitAndSubmit();",200); //vk 08.11
    else
        SubmitWithFrame();//document.forms(0).submit();
}

function SubmitWithFrame() //05.10, 06.10
{
    //vk 10.13
    //if (isNaN(document.getElementById("HresW").value) || isNaN(document.getElementById("HresH").value)) {
    //    var er;
    //    er = GetResol(document.getElementById("HresMethod"), document.getElementById("HresW"), document.getElementById("HresH"));
    //    if (param) {
    //        if (param > 10)
    //            MyMsgBox("Could not get resolution! " + er);
    //        else
    //            setTimeout("SubmitWithFrame(" + (param + 1) + ");", 200);
    //    }
    //    else
    //        setTimeout("SubmitWithFrame(1);", 200);
    //    return;
    //}


    //sessionStorage.setItem('send', ccc()); //ntg 03.12.23 - calling a temp function to check runtime


    NoDanger_Document(document);
    //if (top == window) {
    validNavigation = true;
    //gSubmit = true; //vk 02.20
    document.forms[0].submit();
    //return;
    //}
    /*
        var x = top.document.getElementById("ifrGet");
        if (x.contentWindow)
            x = x.contentWindow;
        var copy = x.document;
        var cnt, d;
        var oInput = copy.getElementsByTagName("input");
        //for (var s in oInput)
        //    if (s != "length" && s != "item" && s != "namedItem" && s.substr(0, 2) != "__" && isNaN(s)) {
        //if (copy.getElementsByName(s).length == undefined) {
        //    cnt = copy.getElementById(s);
        //    d = document.getElementById(s);
        //    if (cnt.type != "checkbox" && cnt.type != "radio")
        //        cnt.value = d.value;
        //    else
        //        cnt.checked = d.checked;
        //}
        //else
        //        for (var i = 0; i < copy.getElementsByName(s).length; i++) {
        //            cnt = copy.getElementsByName(s)[i];
        //            d = document.getElementsByName(s)[i];
        //            if (cnt.tagName.toLowerCase() == "input") { //vk 02.12
        for (var i = 0; i < oInput.length; i++) {
            id = oInput[i].id;
            cnt = copy.getElementById(id);
            d = document.getElementById(id);
            //try {
            if (cnt.type != "checkbox" && cnt.type != "radio")
                cnt.value = d.value;
            else
                cnt.checked = d.checked;
            //} catch (e) {
            //    debugger;
            //}
            //                }
            //            }
        }
        //gSubmit = true; //vk 02.20
        var f = copy.forms[0];
        f.action = f.action + "?action=next";
        validNavigation = true;
        f.submit();
        bInSubmit = true; //vk 02.20
        top.WaitAndShow();
        //var t = top.document.getElementById("trigger");
        //if (t.value == "0")
        //    t.value = "1";
        //else
        //    t.value = "0";
    */
}

function NoDanger_Document(d) //vk 12.10
{
    if (document.getElementById("hWhat"))
        document.getElementById("hWhat").value = "";
    if (document.getElementById("hResult"))
        document.getElementById("hResult").value = "";
    //vk 05.13 till here
    var oInput = $(d).find('input[type=hidden],[type=text]')
    for (var i = 0; i <= oInput.length - 1; i++) {
        NoDanger_One(oInput[i]);
    }
    bInSubmit = true; //vk 02.20
}

function NoDanger_One(cnt) //vk 10.13
{
    if (cnt.oninput)
        cnt.oninput = ""; //vk 12.14
    //    cnt.value=""; //vk 07.13
    //else
    switch (cnt.id.substr(0, 1)) {
        case "S":
        case "F":
        case "W":
        case "Z":
        case "H":
        case "R":
        case "M":
            cnt.value = NoDanger_Field(cnt.value);
            break;
        default:
            cnt.disabled = true;
            break;
    }
}

function NoDanger_Field(s) //vk 12.10
{
    if (s == undefined)
        return "";
    if (s.indexOf("<") < 0 && s.indexOf(">") < 0 && s.indexOf("₪") < 0)
        return s;
    var t = "";
    for (var i = 0; i < s.length; i++) {
        var c = s.substr(i, 1);
        switch (c) {
            case "<": t += "&lt;"; break;
            case ">": t += "&gt;"; break;
            case "₪": t += "ש\"ח"; break; //vk 09.16
            default: t += c; break;
        }
    }
    return t;
}


function Ping(nPingMaxCount) {
    if (!IsEnable()) return;

    nPingAlertCount = parseInt(document.getElementById("PingAlertCount").value);

    if (nPings < nPingMaxCount || nPingMaxCount == 0) {
        if (nPingMaxCount != 0)
            nPings++;
        bInPing = true;
        document.forms.frmPing.submit();

        if ((nPingAlertCount > 0) && (nPings == nPingMaxCount - nPingAlertCount + 1)) {
            timerModalOpen = true;
            MyCount();
        }

    } else if ((nPings == nPingMaxCount) && !timerModalOpen) {
        SignOut()
    }
}

function SignOut() {
    clearInterval(PingInterval);
    var Message = msg(8)
    var HtmlText = "<div class='container'> \
                      <h3> " + Message + " </h3> \
                      <IFRAME id='ifrPing' name='ifrPing' src='Empty.htm' width='0' height='0' style='visibility: hidden;'></IFRAME> \
                      <form id='frmPing' action='Close.aspx' method='post' target='ifrPing'></form> \
                     </div>";

    document.body.innerHTML = HtmlText

    document.forms.frmPing.submit();

    document.body.onkeydown = "";
    document.body.onbeforeunload = "";
}

function CheckPing() //vk 04.05
{
    if (bInPing)
        MyMsgBox("Ping did not come back in five seconds!");
}

function MenuClick(s) {
    //debugger;
    if (bMayMarkCurrent) {
        document.getElementById("Hfld").value = s.substr(numS, numL);
        document.getElementById("Hfind").value = s.substr(indS, indL);
        document.getElementById("R").value = s;
    }
    fnpsl_dblclick();
}

function ValidateNonZero(input) {
    if (Number(input.value) == 0) {
        input.setCustomValidity('the number must not be zero.');
    } else {
        // input is fine -- reset the error message
        input.setCustomValidity('');
    }
}

function CmdPchClick_Common(sFieldForValue, sCodeForCmd, bMustCheckR, bPassField, nLen) //ilia
{
    var bCheckOk = false;
    var oR = document.getElementsByName("R");
    var trSelectedCount = document.querySelectorAll('.trSelected').length; //ntg 26.06.24 for fgr=B screens with an option to select multiple rows in a table

    if (!fnCanSubmit()) return;
    var opch = $(gFocus).attr("pch") + "";

    if (bMustCheckR) {
        if (oR != null) {
            for (i = 0; i < oR.length; i++) {
                if (oR[i].checked) bCheckOk = true;
            }
            if (oR.length == 0) bCheckOk = true; //vk 05.21
            if (trSelectedCount > 1) bCheckOk = true;
            if (trSelectedCount == 0) bCheckOk = false;

        }
    } else {
        bCheckOk = true;
    }

    if (bCheckOk) {
        //document.body.style.cursor = 'wait'; /*ntg 07.01.24 the cursor was on "wait" all the time*/
        //ShowWait(); /*ntg 07.01.24 when there is a must-fill field, and the user tries to go to the next screen without filling it, now it will show the waiting gif*/
        if (sCodeForCmd != '')
            document.getElementById("Hfcmd").value = sCodeForCmd; //vk 06.04
        if (bPassField && bMayMarkCurrent) { //vk 05.05
            $("#Hfld").val($(gFocus).attr("fld"));
            $("#Hfind").val($(gFocus).attr("ind"));
        }
        if (sFieldForValue != '') {
            var opch = $(gFocus).attr("pch") + "";
            if (nLen - 0 > 0)
                if (opch.length > nLen - 0)
                    opch = MyRight(opch, nLen - 0);
            document.getElementById(sFieldForValue).value = opch;

            if (trSelectedCount > 1) {
                for (var i = 0; i < oR.length; i++) {
                    if (oR[i].closest('.trSelected')) { //ntg 26.06.24 for fgr=B screens with an option to select multiple rows in a table
                        oR[i].type = 'checkbox';
                        oR[i].checked = true;
                        bCheckOk = true;
                        document.getElementById("Z" + oR[i].value).value = opch; //adding the selected action to the hiddens of each selected row in the table
                    }
                }
            }
        }
        doSubmit();
        document.body.style.cursor = 'auto'; /*ntg 07.01.24 the cursor was on "wait" all the time*/

    } else {
        MyMsgBox(msg(4), "ok");
    }
}

