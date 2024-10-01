
function NotInBlur() //vk 03.06
{
    bInBlur = false;
}

function trS_click_radio(cnt, per, cod, bKiosk) {
    if (!IsEnable()) return;
    fnFocus_Common(cnt, 1, 0, false); //vk 12.19
    var oSelCode = document.getElementById(cnt.name.substr(1));
    sFieldValue = oSelCode.value + ""; //vk 02.07
    oSelCode.value = cnt.value;
    fnClearLowerGroups(cnt, bKiosk); //vk 02.05
    if (per == 1) fnBlur_Check(oSelCode, cod);
}

function trS_click_check(cnt, opYes, opNo, per, cod) {

    if (!IsEnable()) return;
    fnFocus_Common(cnt, 1, 0, false); //vk 12.19
    var oSelCode = document.getElementById(cnt.name.substr(1));
    sFieldValue = oSelCode.value;

    if (cnt.checked) {
        oSelCode.value = opYes;
    } else {
        oSelCode.value = opNo;
    }
    if (per == 1) fnBlur_Check(oSelCode, cod);
}

function help_radio(o) {
    if (!IsEnable()) return;
    show_prompt_text_bycoord(o, o, true, o.q == "true");
}

function help_check(o, show) {
    if (!IsEnable()) return;
    if (o.checked)
        o.setAttribute("helptext", o.getAttribute("helptexttrue"));
    else
        o.setAttribute("helptext", o.getAttribute("helptextfalse"));
    show_prompt_text_bycoord(o, o, show, o.q == "true");
}

function StepByKey(key) //vk 07.06
{
    switch (key) {
        case 38: return -1;   // ArrowUp
        case 40: return 1;    // ArrowDown
        case 33: return -10;  // PgUp
        case 34: return 10;   // PgDn
        case 36: return -100; // Home
        case 35: return 100;  // End
        case 13: return 0;    // Enter
        default: return null;
    }
}
