var nSecondsLeft,sSecondsCaption;

function SetButtonCaptions(bKiosk)
{
    var oInput = $(":input[type=button]");
    // var oInput1 = document.getElementsByTagName("input");
    //for (var i=0;i<document.all.tags("input").length;i++)
    

    $('.date').datetimepicker({
        language: 'he',
        format: 'dd/mm/yyyy',
        minView: 2,
        pickerPosition: 'top-right',
        autoclose: 1
    }).on("changeDate", function (e) {
        // alert(e.date)
    });


    for (var i = 0; i < oInput.length; i++)
	{
      //  var o = document.all.tags("input")[i];
        var o = oInput[i];
		if (o.type=="button")
		    if (o.getAttribute("cap"))
			{
				var s;
				for (s=o.getAttribute("cap")+"";
					s.indexOf("\\n")>=0;
					s=s.replace("\\n",CRLF)
				){};
				//vk 05.06
				if (o.getAttribute("chck"))
					fnKeyboard_Checked(o,s,bKiosk);
				else
					o.value=s;
			}
	}
    //vk 11.10
    var oTable = $("table[class=KioskButton]");
    //for (var i=0;i<document.all.tags("table").length;i++)
    for (var i = 0; i < oTable.length; i++)
	{
       // var o = document.all.tags("table")[i];
        var o = oTable[i];
		if (o.className=="KioskButton")
		    if (o.getAttribute("cap"))
			{
				var s;
				for (s=o.getAttribute("cap")+"";
					s.indexOf("\\n")>=0;
					s=s.replace("\\n",CRLF)
				){};
				if (o.getAttribute("chck"))
				{
		            var nbsp=String.fromCharCode(160);
		            var chm=String.fromCharCode(10003);
            		o.style.backgroundColor="#6B9EB5";
            		o.style.color="#DDDDDD";
		            o.rows[0].cells[0].innerText=chm+nbsp+s+nbsp+nbsp+nbsp+nbsp;
		            o.style.tableLayout="fixed";
				}
			}
	}
}
/*
function fnKeyboard_Number(c,nMaxLen)
{
    with (document.getElementById("Hkbd")) {
		if (value.length<nMaxLen) value+=c;
	}
}
function fnKeyboard_BackSpace()
{
    with (document.getElementById("Hkbd")) {
		if (value!="") value=value.substring(0,value.length-1);
	}
}
*/
function fnKeyboard_Radio(oo,bKiosk)
{
if (!IsEnable()) return;
var oSelCode = document.getElementById(oo.name.substr(1));
if (oSelCode.value==oo.getAttribute("optval")) return;
	for (var i=0;i<document.getElementsByName(oo.name).length;i++)
	{
	    var o = document.getElementsByName(oo.name)[i];
		fnKeyboard_Unchecked(o,bKiosk);
	}
	sFieldValue = oSelCode.value+""; //vk 12.06
	oSelCode.value = oo.getAttribute("optval");
	fnKeyboard_Checked(oo,oo.getAttribute("cap")+"",bKiosk);
	fnClearLowerGroups(oo,bKiosk);
	if (oo.getAttribute("per") == '1' && bMayMarkCurrent) {
	    document.getElementById("Hfld").value = oSelCode.name.substr(numS,numL); //vk 10.06
	    document.getElementById("Hfind").value = oSelCode.name.substr(indS,indL); //vk 10.06
	    fnBlur_Check(oSelCode,oo.getAttribute("cod"));
	}
}

function fnClearLowerGroups(oo,bKiosk)
{
	if (oo.grouplevel)
	{
		for (var i=0;i<document.getElementsByTagName("input").length;i++)
		{
		    var ooo = document.getElementsByTagName("input")[i];
			if (ooo.grouplevel)
			{
				if (ooo.grouplevel!=oo.grouplevel) //not only lower!
				{
					if (ooo.type=="button")
					    if (ooo.getAttribute("cap"))
							fnKeyboard_Unchecked(ooo,bKiosk);
					if (ooo.type=="radio")
						ooo.checked=false;
					document.getElementById(ooo.name.substr(1)).value="";
				}
			}
		}
	}
}

function fnKeyboard_Checked(o,s,bKiosk)
{
	if (bKiosk) {
		o.style.backgroundColor="#6B9EB5";
		var nbsp=String.fromCharCode(160);
		var chm=String.fromCharCode(10003);
		o.value=chm+nbsp+s+nbsp+nbsp+nbsp+nbsp;
	} else {
		o.style.border="2px solid red";
	}
}

function fnKeyboard_Unchecked(o,bKiosk)
{
	if (bKiosk) {
		o.style.backgroundColor="#BDD3DE";
		o.value=o.getAttribute("cap");
	} else {
	o.style.border=$(".BtnField").css("border"); //document.styleSheets[1].rules("BtnField").style.border;
	}
}
/*
function KbdButtons_once(sConfirm,sBackspace)
{
    document.getElementById("KbdConfirm").value=sConfirm;
    document.getElementById("KbdBackspace").value=sBackspace;
}
*/
//vk 03.05
function CountSeconds(sCaption,nSeconds)
{
	sSecondsCaption=sCaption;
	nSecondsLeft=nSeconds;
	SetButtonCaptions(true);
	SetSeconds();
	setInterval('EverySecond()',1000);
}
function EverySecond()
{
	nSecondsLeft-=1;
	SetSeconds();
	if (nSecondsLeft<=0)
	{
		window.returnValue=true;
		window.close();
	}
}
function SetSeconds()
{
    document.getElementById("sec").innerText=sSecondsCaption+nSecondsLeft;
}
