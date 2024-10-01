// ---------------------------------------------
//functions that taking care time AND TEXT
// written by Michael Shmushkin 12/2002
// example of use

//	<input dir='ltr' name='i1' id='i1' tabIndex=1 maxLength=6 size = 8
//	onblur="fnBlurTime(this,'T');"
//	onfocus="fnFocusTime(this);"
//	onkeypress="fnKeyPressTime()"
//      onkeydown="fnKeyDownTime(this,'T')"
//      >
// -----------------------------------------------

var gIsTime=true;
var gIsText=true; //vk 07.06

//function fnAddLeftZeroT(sVal,iLen)
//{
//	j = sVal.length;
//	sRes = ""
//	for (i=0; i<iLen; i++)
//	{
//		j--;
//		if (j >= 0)
//			s = sVal.substr(j,1);
//		else
//			s = "0";
//		sRes = s + sRes;
//	}
//	return sRes;
//}

//' -------------------------------
//'function onBlur for text fields
//' -------------------------------

function fnBlurTime(cnt,sType) //+vk 03.06
{
	if (!IsEnable()) return;
	if (bInBlur) return true;
	bInBlur=true;
	bStopped=false;
	sVal = cnt.value;

	if (sVal == "")
	{
		gIsTime = true;
		bInBlur=false;
		return;
	}			

	if (!bReady) {
	    sVal = fnReadyBlurTime(sVal,sType);
	    bReady = true; //vk 09.14
	}

	if (sVal == "")
	{
		bStopped=true;
		//cnt.focus();
		MyMsgBox(msg_Time(1),"false",cnt);
		gIsTime = false;
		if (event) { //vk 01.14
		    event.cancelBubble = true;
		    event.returnValue = false;
		}
	} else {
		cnt.value = sVal;
		gIsTime = true;
	}
	bInBlur=false;
}

function fnBlurText_UpperOnly(cnt,bAlphaNumOnly) //,bMakeUpper) //vk 07.06, 01.07, 02.20
{
	if (!IsEnable()) return;
	if (bInBlur) return true;
	bInBlur=true;
	bStopped=false;
	//sVal = cnt.value + "";
	sVal = $(cnt).val() + "";
	var i,c;

	//if (bMakeUpper) {
	//    //vk 02.20
	//    sVal = sVal.toUpperCase();
	//    $(cnt).val(sVal);
	//}
	gIsText=true;
	if (sFieldValue !== sVal) { //vk 07.09
	    if (bAlphaNumOnly)
	    {
		    for (i=0;i<sVal.length;i++)
		    {
			    c=sVal.substr(i,1);
			    if (!(
				    (c>="A" && c<="Z") ||
				    (c>="0" && c<="9")
			    ))
				    gIsText=false;
		    }
	    } else {
		    for (i=0;i<sVal.length;i++)
		    {
			    c=sVal.substr(i,1);
			    if (c>="a" && c<="z")
				    gIsText=false;
		    }
	    }
	    if (!gIsText)
	    {
		    bStopped=true;
		    //cnt.focus();
		    if (bAlphaNumOnly)
			    MyMsgBox(msg_Text(2),"false",cnt);
		    else
			    MyMsgBox(msg_Text(1),"false",cnt);
		    if (event) { //vk 01.14
		        event.cancelBubble = true;
		        event.returnValue = false;
		    }
	    }
	}
	bInBlur=false;
}

function fnReadyBlurTime(sVal,sType)
{
	switch (sType)	
	{
	case "T":
		sVal = fnAddLeftZero(sVal,6);
		sHour = sVal.substr(0,2)
		sMin = sVal.substr(2,2)
		sSec = sVal.substr(4,2)
		iHour = Number(sHour)
		iMin = Number(sMin)
		iSec = Number(sSec)
		if (sVal == "999999")
			return "99:99:99"; //vk 05.07
		if (iHour >= 24 || iMin >= 60 || iSec >= 60)
			return "";
		else
			return sHour + ":" + sMin + ":" + sSec;
		break;
	case "U":
		sVal = fnAddLeftZero(sVal,4);
		sHour = sVal.substr(0,2)
		sMin = sVal.substr(2,2)
		iHour = Number(sHour)
		iMin = Number(sMin)
		if (sVal == "9999")
			return "99:99"; //vk 05.07
		if (iHour >= 24 || iMin >= 60)
			return "";
		else
			return sHour + ":" + sMin;
		break;
	}	
}

function fnFocusTime(cnt)
{
	//gIsTime = true;
	var val = cnt.value;
	cnt.value = val.replace(/:/g,"");
	mySelect(cnt);
}

// -----------------------------
//function KeyDown on input of type "T" 
// ------------------------------

function DecKeyTime(key)
{
	if (key < 48 || key > 57)
		return false;
	else
		return true;
}  // DecKeyNum

function fnKeyPressTime()
{
	var key = event.keyCode;
	event.returnValue = DecKeyTime(key);
} // fnKeyPress

function MakeUpper_Paste(o) { //vk+ib 04.21
	var clipboardData, pastedData;
	event.stopPropagation();
	event.preventDefault();
	var clipboardData = event.clipboardData || window.clipboardData;
	var pastedData = clipboardData.getData('Text');
	//vk 05.21
	var s1, s2, s3;
	try { s1 = o.value.substring(0, o.selectionStart); } catch (e) { s1 = ""; }
	try { s2 = pastedData.toUpperCase(); } catch (e) { s2 = pastedData; }
	try { s3 = o.value.substring(o.selectionEnd); } catch (e) { s3 = ""; }
	try {
		o.value = s1 + s2 + s3; //pastedData.toUpperCase();
		o.selectionStart = (s1 + s2).length;
		o.selectionEnd = o.selectionStart;
	} catch (e) { }
}

function MakeUpper(o) //vk 07.06, 05.21
{
	//if (event.keyCode>=97 && event.keyCode<=122)
	//	event.keyCode-=32;
	//vk 02.20
	var n = o.selectionStart;
	o.value = o.value.toUpperCase();
	o.selectionStart = n;
	o.selectionEnd = n;
	//var sVal = $(cnt).val() + "";
	//sVal = sVal.toUpperCase();
	//$(cnt).val(sVal);
}
function MakeLower(o) //vk 07.06, 05.21
{
	//if (event.keyCode>=65 && event.keyCode<=90)
	//	event.keyCode+=32;
	//vk 02.20
	var n = o.selectionStart;
	o.value = o.value.toLowerCase();
	o.selectionStart = n;
	o.selectionEnd = n;
	//var sVal = $(cnt).val() + "";
	//sVal = sVal.toLowerCase();
	//$(cnt).val(sVal);
}

function EnglishOrDigit() {
	return (event.keyCode >= 97 && event.keyCode <= 122) ||
		(event.keyCode >= 65 && event.keyCode <= 90) ||
		(event.keyCode >= 48 && event.keyCode <= 57)
}

function BlockNonAlphaNum() 
{
	if (!EnglishOrDigit) event.keyCode=0;
}

//' -------------------------------
//'function fnIsTime
//  return true if sVal is Time
//' -------------------------------

function fnIsTime(sVal,sType)
{
	if (sVal == "") return true;
	if (!bReady) {
	    sVal = fnReadyBlurTime(sVal,sType);
	    bReady = true; //vk 09.14
	}
	if (sVal == "")
		return false;
	else
		return true;
} // fnIsTime

//' -------------------------------
//'function fnKeyDownTime
//' -------------------------------

function fnKeyDownTime(cnt,sType)
{
	var sVal = cnt.value;

	if (fnIsAction())
	{
		gIsTime = fnIsTime(sVal,sType);		
		if (!gIsTime)
			cnt.blur();
	}
} // fnKeyDownTime
