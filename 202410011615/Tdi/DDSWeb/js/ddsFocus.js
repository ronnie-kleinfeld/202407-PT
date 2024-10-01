var sButtonVisited = "";
var sButtonTab;
var sFieldValue;
var bInPer = false;
var bInBlur = false; //vk 03.06
var bBlurCancel = false; //vk 07.06
var sPermittedButtonsList = "";
var bMayMarkCurrent = true; //vk 05.07
var oTimeout; //vk 03.08
var bClickLocked = false; //vk 06.10
var bInNext = false; //vk 06.10
var bDontSelect = false; //vk 09.11
var bReady; //vk 09.14

function fnFocusBoo4(cntid, n) {
    if (!IsEnable()) return;
    fnFocusBoo_Common("Y" + cntid, n, 1);
}

function fnFocusCombo(cnt, bCheckBox) {
    if (!gIsEnable) return;
    if (bCheckBox) {
        if (cnt.tagName == "SPAN") cnt = cnt.parentNode.childNodes[1];
    }
    fnFocus_Common(cnt, 1, 0, false);
    //if (f) {
    //    //cnt.className = 'InpFoc';
    //    //$(cnt).removeClass().addClass("InpFoc");
    //    bInCombo = true;
    //}
}

function fnFocusBoo_Common(cntid, n, m) {
    $("#Hfld").val(cntid.substr(numS + n, numL));
    $("#Hfind").val(cntid.substr(indS + n, indL));
    $("#Hcur").val(cntid.substr(m));
}

function fnFocus_Common(cnt, n, m, bTextBox) {

    if (numS == undefined)
        InitConst(10, 4, 1, 5, 2, 1)

    if (!bMayMarkCurrent) return;
    if (cnt.length == undefined); else if (cnt.id); else cnt = cnt[0]; //vk 03.09
    $("#Hfld").val(cnt.id.substr(numS + n, numL));
    $("#Hfind").val(cnt.id.substr(indS + n, indL));
    //document.all.Hfld.value = cnt.name.substr(numS + n, numL);
    //document.all.Hfind.value = cnt.name.substr(indS + n, indL);

    $("#Hcur").val("");
    if (bTextBox) {
        // document.all.Hcur.value = cnt.name.substr(m);
        $("#Hcur").val(cnt.id.substr(m));
    }
}
function fnFocus(cnt, n) {
    if (!IsEnable()) return;
    var m = 0;
    if (cnt.name.substr(0, 1) == "Y") m = 1; //vk 07.09
    if (cnt.name.substr(0, 1) == "X") m = 1;
    fnFocus_Common(cnt, n, m, true);
    //cnt.className = 'InpFoc';
}

function fnFocusSelect(cnt) //vk 01.04
{
    if (!IsEnable()) return;
    fnFocus_Common(cnt, 1, 1, true);
}

function fnBlur_Check(oo, cod, ooText, bPreserveValue, bChangeByCode, bChanged) {
    if (!IsEnable() || !gIsDate || !gIsTime || !gIsNumber || !gIsText) return;
    if (bInCalendar) return;
    if (sButtonVisited == undefined) sButtonVisited = null;
    if (bBlurCancel) return; //vk 07.06
    if (bStopped) return; //vk 12.06
    if (bInNext) return; //vk 06.10
    // if (!sFieldValue) return;

    if (bInPer && event.type == "blur") { //vk 10.23; ntg 15.10.23 added && event.type == "blur"
        var Targetelem = event.relatedTarget;
        if (Targetelem == null) Targetelem = document.activeElement;
        if (Targetelem)
            if (Targetelem.type == 'button') {
                Targetelem.click();
                bInPer = false;
                return;
            }
    }

    if (oo) {
        var bCont;
        if (sFieldValue == undefined)
            bCont = true; //vk 02.22
        else if (bChangeByCode)
            bCont = bChanged; //vk 08.13
        else if (ooText)
            bCont = sFieldValue !== ooText.value + "";
        else
            bCont = sFieldValue !== oo.value + "";
        if (bCont) {
            //sComboOpen = null;
            if (cod.length == 2) {

                //document.getElementById("Hfld_per").value = oo.name.substr(numS + 1, numL); //ntg 17.06.24 changes regarding focus in field- by vladi
                //document.getElementById("Hfind_per").value = oo.name.substr(indS + 1, indL);//ntg 17.06.24 changes regarding focus in field- by vladi

                if (oo.name.substr(0, 1) == "F") { //ntg 30.06.24 changes regarding focus in field- by vladi
                    document.getElementById("Hfld_per").value = oo.name.substr(numS, numL);
                    document.getElementById("Hfind_per").value = oo.name.substr(indS, indL);
                } else {
                    document.getElementById("Hfld_per").value = oo.name.substr(numS + 1, numL);
                    document.getElementById("Hfind_per").value = oo.name.substr(indS + 1, indL);
                }

                document.getElementById("Hfcmd").value = cod;
                bMayMarkCurrent = false;
                setTimeout("doSubmit();", 100);
            } else if (cod == "$copy") {
                var s = oo.name.substr(1);
                var oHid = document.getElementById(s);
                oHid.value = oo.value;
                var oTab = document.getElementById("T" + s);
                var oCur = document.getElementById("S" + s);
                var jCur = -1;
                for (var j = 0; j < oTab.rows.length; j++) {
                    if (oTab.rows[j].cells[0].innerText - 0 == oHid.value - 0)
                        jCur = j;
                }
                if (jCur >= 0)
                    oCur.value = oTab.rows[jCur].cells[1].innerText;
            } else
                setTimeout(cod, 1);
        } else {
            if (cod.length == 2) {
                UnlockClick();
                if (!bPreserveValue)
                    sFieldValue = "";
                if (oo.tagName == "DUET-DATE-PICKER") { /*vladi change- ntg 02.04.24 this adds a validation message to date-time-picker that its value is illegal (compared do when its empty)*/
                    $(oo).addClass("is-invalid");
                }
            }
        }
    } else {
        if (cod.length == 2) {
            document.getElementById("Hfcmd").value = cod;
            bMayMarkCurrent = false;
            setTimeout("doSubmit();", 100);
        }
    }
    bInPer = false;
    return bCont; //vk 09.11
}

function fnKey_Check(oo) {
    if (!IsEnable()) return;
    switch (event.keyCode) {
        case 9:
        case 13:
            bInPer = false;
            break;
        case 16:
        case 17:
        case 18:
        case 27:
            break; //vk 07.10
        default:
            LockClick(); //vk 05.10
            break;
    }
}

function LockClick() //vk 06.10
{
    var obj = document.getElementById("HlockClick");
    if (objExists(obj)) {
        if (obj.value != "lock")
            return;
        if (bClickLocked)
            return;
        bClickLocked = true;
        var coll = document.getElementsByTagName("input");
        if (coll) {
            var cursorWait = getCursorTypeWait();
            var count = coll.length;
            if (count)
                for (var i = 0; i < count; i++)
                    if (coll[i].type == "checkbox" || coll[i].type == "radio")
                        if (!coll[i].disabled) {
                            //coll[i].origdis = "false";
                            //origdisSet(coll[i]); //vk 06.15
                            //coll[i].origcur = coll[i].style.cursor; //vk 07.13
                            coll[i].disabled = true;
                            coll[i].style.cursor = cursorWait; //"wait";
                        }
        }
    }
}

function UnlockClick() //vk 06.10
{
    var obj = document.getElementById("HlockClick");
    if (objExists(obj)) {
        if (obj.value != "lock")
            return;
        if (!bClickLocked)
            return;
        bClickLocked = false;
        /*
                var coll = document.getElementsByTagName("input");
                if (coll) {
                    var count = coll.length;
                    if (count)
                        for (var i = 0; i < count; i++)
                            if (coll[i].type == "checkbox" || coll[i].type == "radio")
                                //if (coll[i].getAttribute("origdis"))
                                    if (coll[i].getAttribute("origdis") == "false") {
                                        coll[i].disabled = false;
                                        //coll[i].style.cursor = coll[i].origcur; //vk 07.13
                                        //ResetCursor(coll[i]); //vk 04.20
                                    }
                }
        */
        //setTimeout("SetNext();", 100);
    }
}

//function SetNext() //vk 06.10
//{
//    if (oNext) {
//        bInNext = true;
//        oNext.focus();
//        oNext = null;
//        bInNext = false;
//    }
//}

function fnFocus_Check(oo) {
    if (!IsEnable()) return;
    sFieldValue = oo.value + "";
    bInPer = true;
    gIsNumber = true;
    gIsDate = true;
    gIsTime = true;
    gIsText = true;
    bReady = false; //vk 09.14
}

function fnFocus_UpperOnly(oo) //vk 07.09
{
    if (!IsEnable()) return;
    sFieldValue = oo.value + "";
    gIsNumber = true;
    gIsDate = true;
    gIsTime = true;
    gIsText = true;
    bReady = false; //vk 09.14
}

function fnBtnEnter(cnt, withprompt) {
    if (!IsEnable()) return;
    sButtonVisited = cnt.name;
}

function fnBtnEnter_Folder(cnt, withprompt) {
    if (!IsEnable()) return;
    if (cnt.spec == undefined) cnt.spec = ""; //vk 05.07
    sButtonVisited = cnt.name;
}

function fnBtnEnter_Field(cnt, withprompt) //vk 05.05
{
    if (!IsEnable()) return;
    sButtonVisited = cnt.name;
}

function fnBtnFocus(cnt) //vk 01.04
{
    if (!IsEnable()) return;
    sButtonTab = cnt.name;
}

function fnBtnBlur(cnt) //vk 01.04
{
    if (!IsEnable()) return;
    sButtonTab = null;
    sButtonVisited = null; //vk 07.06
}

function fnBlur_DeleteOnly(oo) {
    if (bBlurCancel) return; //vk 07.06
    if (oo.value + "" !== oo.getAttribute("oldvalue") && oo.value + "" !== "") {
        oo.value = oo.getAttribute("oldvalue");
        oo.focus();
    }
}

function fnBlurEmpty(cnt, bNumeric, bCombo) //vk 03.06
{
    if (!IsEnable()) return true;
    if (bInBlur) return true;
    var sname = cnt.name.substr(1);
    bBlurCancel = false; //vk 07.06
    if (sButtonVisited == undefined) sButtonVisited = null;
    if (sButtonVisited) //vk 07.06, 06.09
    {
        if (sButtonVisited.charAt(0) == "C") return true;
        if (sButtonVisited == "A" + sname) return true;
    }
    bInBlur = true;
    bStopped = false;
    var oValue;
    var oFocus;
    if (bCombo) {
        //oValue = document.all(sname);
        //oFocus = document.all("D" + sname);
        oValue = document.getElementById(sname);
        oFocus = document.getElementById("D" + sname);
        if (oFocus.style.display != "block")
            oFocus = cnt;
    } else {
        oValue = cnt;
        oFocus = cnt;
    }
    var s = oValue.value;
    if (bNumeric) {
        if (isNaN(s)) {
            bInBlur = false;
            return true;
        }
        if (s - 0 != 0) {
            bInBlur = false;
            return true;
        }
        if (bCombo)
            AlertEmpty(oFocus, 1);
        else
            AlertEmpty(oFocus, 2);
    } else if (s == "") {
        AlertEmpty(oFocus, 1);
    } else {
        for (var i = 0; i < s.length; i++)
            if (s.charAt(i) != " ") {
                bInBlur = false;
                return true;
            }
        AlertEmpty(oFocus, 1);
    }
    event.returnValue = false;
    event.cancelBubble = true;
    bInBlur = false;
    bBlurCancel = true; //vk 07.06
    return false;
}

function AlertEmpty(oFocus, nAlert) //vk 07.06
{
    bStopped = true;
    MyMsgBox(msg_Empty(1), "false", oFocus);
    try { //vk 12.06
        oFocus.focus();
    } catch (e) { ; }
}

function show_prompt_text_bycoord(o, oo, show, q) {
    with (oo.style) {
        var l = parseInt(left) - 10;
        var t = parseInt(top) - 10;
        if (document.body.dir != "rtl") l += parseInt(width) + 20;
    }
    if (q) {
        var obj = document.getElementById("scroll");
        with (obj) {
            l += parseInt(style.left);
            t += parseInt(style.top) - scrollTop;
        }
    }
}

function mySelect(o) //vk 09.11
{
    if (!bDontSelect)
        $(o).select();
}

function fnCur(s) {
    cnt = document.getElementsByName(s);
    if (cnt) {
        //if (cnt.length == undefined) {
        //    if (fnCurOnce(cnt)) return;
        //} else {
        for (i = 0; i < cnt.length; i++) {
            if (cnt[i].checked) {
                if (fnCurOnce(cnt[i])) return;
            }
        }
        //vk 06.04
        for (i = 0; i < cnt.length; i++) {
            if (fnCurOnce(cnt[i])) return;
        }
        //}
    }
    try { //06.10
        document.body.focus();
    } catch (e) { ; }
}
function fnCurOnce(cnt) //vk 06.04
{

    if (cnt.type != 'hidden' && !cnt.disabled) //disabled vk 09.05
    {
        if (!cnt.parentElement.disabled) //vk 09.06
        {
            if (cnt.id == "Gfocus") {

                var glast = document.getElementById("Glast");
                if (glast) {
                    glast.focus();
                }
                var gfirst = document.getElementById("Gfirst");
                if (gfirst) {
                    gfirst.focus();
                }
            }
            try { //vk 05.10
                if (cnt.tagName == "DUET-DATE-PICKER") //vk 15.07.24 changes to make focus appear in "date" type field
                    cnt.setFocus();
                else
                    cnt.focus();
            } catch (e) {
                var x = e;
            }
            if (cnt.type == 'text') mySelect(cnt);
        }
        return true;
    }
    return false;
}
function fnFocusQfk(s) {
    sPermittedButtonsList = s;
    PermitButtons();
}

function PermitButtons() {
    var cnt;
    var coll = $(":input[type=button]");
    if (coll == null) return;
    if (coll.length == null) return;
    for (var i = 0; i < coll.length; i++) {
        cnt = coll[i];
        if (cnt.name.length == 3) {
            cnt.disabled = !ButtonPermitted($(cnt).attr("pch"));
        }
        //$(cnt).css('cursor', 'default');
        //if (!cnt.disabled) {
        //    $(cnt).css('cursor', 'pointer');
        //} 
    }
}
function ButtonPermitted(n) {
    if (n == undefined) return false; //vk 05.07
    if (n < 1 || n > 30 || sPermittedButtonsList == '') return true;
    return (sPermittedButtonsList.indexOf('abcdefghijklmnopqrstuvwx123456'.charAt(n - 1)) >= 0);
}