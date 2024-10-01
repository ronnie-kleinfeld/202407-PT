// ---------------------------------------------
//functions that taking care of date
// written by Michael Shmushkin 12/2002
// example of use
// <input id='d1' name='d1' maxLength='6' size='8' value="121100"
//	ondblclick="fnCallCalendar(this,'D',1,1);" /* fnCallCalendar(cnt,sType,iLang,iUS) */
//	onblur = "fnBlurDate(this,'D',1)"  /* fnBlurDate(cnt,sType,iUS) */
//	onfocus="fnFocusDate(this);" 
//	onkeypress="fnKeyPressDate();"
//	onkeydown="fnKeyDownDate(this,'D',1)"  /* fnKeyDownDate(cnt,sType,iUS) */
// >
// Description of parameters: fnCallCalendar(this,'D',1,1)
// 'D' - type of date : 'D' - DDMMYY, 'Y' - DDMMYYYY, 'H' - MMYYYY
// 1 - Language : 0 - English, 1 - Hebrew
// 1 - date in format US : 0 - usual, 1 - format USA;
// ---------------------------------------------

var gIsDate=true;
var bInCalendar; //vk 04.04
/*
function fnCheckDate(iDay, iMonth, iYear, bMayMonth0) //bMayMonth0 vk 07.05
{
  // writing by Saulius
  var stError = false;

  if(isNaN(iDay) || iDay < 1 || iDay > 31)
    stError = true;

  if(isNaN(iMonth) || iMonth < (bMayMonth0 ? 0 : 1) || iMonth > 12)
	stError = true;

  if(isNaN(iYear) || iYear < 1000)
	stError = true;

   switch(iMonth)
   {
      case  4  :
      case  6  :
      case  9  :
      case  11 :  if(iDay > 30)
					  stError = true;
                  break;
      case  2  :  if(isLeapYear(iYear) )
                  {
                    if(iDay > 29)
					  stError = true;
                  }
                  else
                  {
                    if(iDay > 28)
					  stError = true;
                  }
   }

   if(stError)
   {
     // if this is wrong date
     return 0;
   }
   else
   {
   	 // if this is right date
     return 1;
   }
}

function isLeapYear(iYear)
{
   // writing by Saulius	
   return((((!(iYear % 4)) && (iYear % 100) ) || !(iYear % 400)) ? 1 : 0);
}

function fnCallCalendar(cnt,sType,iLang,iUS,per,cod,n)
{

	// --------------------------
	// Call window of Calendar
	// iUS = 0 - usual date; iUS = 1 - American date
	// sType = D : DD/MM/YY; sType = Y : DD/MM/YYYY; sType = H : MM/YYYY;
	// iLang = 0 - English; iLang = 1 - Hebrew;
	// --------------------------
    //debugger;
    document.getElementById("Hfld").value = cnt.name.substr(numS + n, numL); //ilia+vk 10.15, vk 12.16
    document.getElementById("Hfind").value = cnt.name.substr(indS + n, indL); //ilia+vk 10.15, vk 12.16
    if (per) fnFocus_Check(cnt); //vk 04.04
	stValue = cnt.value;
	stValue = fnReadyCalendar(stValue,sType,iUS);
	//sPathCalendar = "Calendar/Calendar" + ((iLang == 0) ? "ENG" : "HEB") + ".htm";
	sPathCalendar = "Calendar.aspx"
	//vk 05.09
	gFocus=cnt;
	var iTop = ModalTop(cnt, 290);
	//if (NoPopups()) {
//		iTop = cnt.offsetTop + 30; //vk 06.09
//		if (iTop > document.body.clientHeight - 300) iTop -= 250; //vk 06.09
		//document.all.ifr100.src="help.htm?dir="+gDir+"&top="+iTop.toString();
	    gDefault100="Value=Cancel";
	    document.getElementById("Hresult100").value="";
	    document.getElementById("ifr100").src = sPathCalendar + "?date=" + stValue + "&top=" + iTop.toString() + "&language=" + iLang.toString();
		setTimeout("fnCallCalendar_cont('"+sType+"',"+iUS+","+per+",'"+cod+"')",100);
	//} else {
	//	var arrArgs = new Array();
	//	arrArgs[0] = stValue;
	//	FillForModalPing(arrArgs);
	//	var winStats = "dialogHeight:"+XP(230)+"px; dialogWidth:240px; edge:sunken; scroll:No; dialogTop:"+iTop.toString()+"px; dialogLeft:; center:Yes; help:No; resizable:No; status:No; unadorned:Yes;";
	//	bInCalendar=true;
	//	rValue = window.showModalDialog(sPathCalendar , arrArgs , winStats);
	//	bInCalendar=false;
	//	document.getElementById("Hresult100").value = rValue;
	//	fnCallCalendar_cont(sType,iUS,per,cod);
	//}
}
function fnCallCalendar_cont(sType,iUS,per,cod) { //vk 05.09
    with (document.getElementById("Hresult100")) {
		switch (value) {
			case "":
				setTimeout("fnCallCalendar_cont('"+sType+"',"+iUS+","+per+",'"+cod+"')",100);
				return;
			case "Value=Cancel":
				return;
			default:
				break;
		}
		value = value.substring(value.indexOf("Value=") + 6);
		if (value == "") return; //vk 12.19
		sDate = value.split('/');
		switch (sType)
		{
			case "D" :
				if (iUS == 0)
					value = sDate[0] + "/" + sDate[1] + "/" + sDate[2].substr(2,2);
				else
					value = sDate[1] + "/" + sDate[0] + "/" + sDate[2].substr(2,2);
				break;
			case "Y" :
				if (iUS == 0)
					value = sDate[0] + "/" + sDate[1] + "/" + sDate[2];
				else
					value = sDate[1] + "/" + sDate[0] + "/" + sDate[2];
				break;
			case "H" :
				value = sDate[1] + "/" + sDate[2];
				break;
			case "Q" : //vk 07.05
				value = sDate[1] + "/" + sDate[2];
				break;
		}  // switch
		gFocus.value = value;
	}
	CopyOut(gFocus); //vk 11.03
	if (per)
		fnBlur_Check(gFocus,cod); //vk 04.04
	else
		gFocus.focus(); //vk 05.09
}
*/
function fnFocusDate(cnt)
{
    //gIsDate = true;
/*
    var val = cnt.value;
	cnt.value = val.replace(/\//g,"");
*/
	mySelect(cnt);
}
/*
function fnReadyCalendar(sVal,sType,iUS)
{
	// --------------------------
	// test if date in the field matches their type
	// and getting ready to calling Calendar window
	// iUS = 0 - usual date; iUS = 1 - American date
	// sType = D : DD/MM/YY; sType = Y : DD/MM/YYYY; sType = H : MM/YYYY;
	// --------------------------

	sVal = sVal.replace(/\//g,""); //vk 09.03
	switch (sType)
	{
		case "D" :
			sVal = fnAddLeftZero(sVal,6);
			if (iUS == 0)
			{
				sDay = sVal.substr(0,2);
				sMonth = sVal.substr(2,2);
			}
			else
			{
				sDay = sVal.substr(2,2);
				sMonth = sVal.substr(0,2);
			}
			sYear = sVal.substr(4,2);
			iDay = Number(sDay);
			iMonth = Number(sMonth);
			iYear = Number(sYear);
			iYear1 = iYear + 2000;
			sYear1 = iYear1.toString();
			if (fnCheckDate(iDay, iMonth, iYear1, false) == 1)
				stValue = sDay + "/" + sMonth + "/" + sYear1;
			else
				stValue = "" ;
			break;
		case "Y" :
		    //vk 12.19 from here
		    if (sVal.length <= 6) {
		        sVal = fnAddLeftZero(sVal, 6);
		        sYear = "20" + sVal.substr(4, 2);
		    } else {
		        sVal = fnAddLeftZero(sVal, 8);
		        sYear = sVal.substr(4, 4);
		    }
            //vk 12.19 till here
			if (iUS == 0)
			{
				sDay = sVal.substr(0,2);
				sMonth = sVal.substr(2,2);
			}
			else
			{
				sDay = sVal.substr(2,2);
				sMonth = sVal.substr(0,2);
			}
			//sYear = sVal.substr(4,4); //rem vk 12.19
			iDay = Number(sDay);
			iMonth = Number(sMonth);
			iYear = Number(sYear);
			iYear1 = iYear + 0;
			sYear1 = iYear1.toString();
			if (fnCheckDate(iDay, iMonth, iYear1, false) == 1)
				stValue = sDay + "/" + sMonth + "/" + sYear1;
			else
				stValue = "" ;
			break;
		case "H" :
			sVal = fnAddLeftZero(sVal,6);
			sDay = "01";
			sMonth = sVal.substr(0,2);
			sYear = sVal.substr(2,4);
			iDay = Number(sDay);
			iMonth = Number(sMonth);
			iYear = Number(sYear);
			iYear1 = iYear + 0;
			sYear1 = iYear1.toString();
			if (fnCheckDate(iDay, iMonth, iYear1, false) == 1)
				stValue = sDay + "/" + sMonth + "/" + sYear1;
			else
				stValue = "" ;
			break;
		case "Q" : //vk 07.05
			sVal = fnAddLeftZero(sVal,6);
			sDay = "01";
			sMonth = sVal.substr(0,2);
			sYear = sVal.substr(2,4);
			iDay = Number(sDay);
			iMonth = Number(sMonth);
			iYear = Number(sYear);
			iYear1 = iYear + 0;
			sYear1 = iYear1.toString();
			if (fnCheckDate(iDay, iMonth, iYear1, true) == 1)
				stValue = sDay + "/" + sMonth + "/" + sYear1;
			else
				stValue = "" ;
			break;
	}   //switch

	return stValue;

}  //fnReadyCalendar

function fnReadyBlurDate(sVal,sType,iUS)
{
//sVal=sVal.replace("\/","").replace("\/","");
	// --------------------------
	// test if date in the field matches their type
	// and getting ready to blur
	// iUS = 0 - usual date; iUS = 1 - American date
	// sType = D : DD/MM/YY; sType = Y : DD/MM/YYYY; sType = H : MM/YYYY;
	// --------------------------
	switch (sType)
	{
		case "D" :
			sVal = fnAddLeftZero(sVal,6);
			if (iUS == 0)
			{
				sDay = sVal.substr(0,2);
				sMonth = sVal.substr(2,2);
			}
			else
			{
				sDay = sVal.substr(2,2);
				sMonth = sVal.substr(0,2);
			}
			sYear = sVal.substr(4,2);
			iDay = Number(sDay);
			iMonth = Number(sMonth);
			iYear = Number(sYear);
			iYear1 = iYear + 2000;
			sYear1 = iYear1.toString();
			if ((fnCheckDate(iDay, iMonth, iYear1, false) == 1) || (sVal=='999999'))
			{
				if (iUS == 0)
					stValue = sDay + "/" + sMonth + "/" + sYear;
				else
					stValue = sMonth + "/" + sDay + "/" + sYear;
			}
			else
				stValue = "" ;
			break;
		case "Y" :
		    //vk 12.19 from here
		    if (sVal.length <= 6) {
		        sVal = fnAddLeftZero(sVal, 6);
		        sYear = "20" + sVal.substr(4, 2);
		    } else {
		        sVal = fnAddLeftZero(sVal, 8);
		        sYear = sVal.substr(4, 4);
		    }
		    //vk 12.19 till here
		    if (iUS == 0)
			{
				sDay = sVal.substr(0,2);
				sMonth = sVal.substr(2,2);
			}
			else
			{
				sDay = sVal.substr(2,2);
				sMonth = sVal.substr(0,2);
			}
			//sYear = sVal.substr(4,4); //rem vk 12.19
			iDay = Number(sDay);
			iMonth = Number(sMonth);
			iYear = Number(sYear);
			iYear1 = iYear + 0;
			sYear1 = iYear1.toString();
			if ((fnCheckDate(iDay, iMonth, iYear1, false) == 1) || (sVal=='99999999'))
			{
				if (iUS == 0)
					stValue = sDay + "/" + sMonth + "/" + sYear;
				else
					stValue = sMonth + "/" + sDay + "/" + sYear;
			}
			else
				stValue = "" ;
			break;
		case "H" :
			sVal = fnAddLeftZero(sVal,6);
			sDay = "01";
			sMonth = sVal.substr(0,2);
			sYear = sVal.substr(2,4);
			iDay = Number(sDay);
			iMonth = Number(sMonth);
			iYear = Number(sYear);
			iYear1 = iYear + 0;
			sYear1 = iYear1.toString();
			if ((fnCheckDate(iDay, iMonth, iYear1, false) == 1) || (sVal=='999999'))
				stValue = sMonth + "/" + sYear;
			else
				stValue = "" ;
			break;
		case "Q" : //vk 07.05
			sVal = fnAddLeftZero(sVal,6);
			sDay = "01";
			sMonth = sVal.substr(0,2);
			sYear = sVal.substr(2,4);
			iDay = Number(sDay);
			iMonth = Number(sMonth);
			iYear = Number(sYear);
			iYear1 = iYear + 0;
			sYear1 = iYear1.toString();
			if ((fnCheckDate(iDay, iMonth, iYear1, true) == 1) || (sVal=='999999'))
				stValue = sMonth + "/" + sYear;
			else
				stValue = "" ;
			break;
	}

	return stValue;

}
*/

/*
function fnBlurDate(cnt,sType,iUS) //+vk 03.06
{
 if (!IsEnable()) return;
	if (bInBlur) return true;
	bInBlur=true;
	bStopped=false;
	sVal = cnt.value;
	gIsDate = true;

	if (sVal == "")
	{
		//gIsDate = true;
		bInBlur=false;
		return;
	}

	//if (!bReady) {
	    //sVal = fnReadyBlurDate(sVal,sType,iUS);
	bReady = true; //vk 09.14
	//}
	if (sVal == "")
	{
		bStopped=true;
		//cnt.focus();
		MyMsgBox(msg_Date(1),"false",cnt);			
		gIsDate = false;
		if (event) { //vk 01.14
		    event.cancelBubble = true;
		    event.returnValue = false;
		}
	} else {
		cnt.value = sVal;
		gIsDate = true;
	}
	bInBlur=false;
}
*/

function fnAddLeftZero(sVal,iLen)
{
	j = sVal.length;
	sRes = ""
	for (i=0; i<iLen; i++)
	{
		j--;
		if (j >= 0)
			s = sVal.substr(j,1);
		else
			s = "0";
		sRes = s + sRes;
	}
	return sRes;
}

/*
function DecKeyDate(key)
{
	if (key < 48 || key > 57)
		return false;
	else
		return true;
}  // DecKeyDate

function fnKeyPressDate()
{
	var key = event.keyCode;
	event.returnValue = DecKeyDate(key);
}  //fnKeyPressDate 

//' -------------------------------
//'function fnIsDate
//  return true if sVal is Date
//' -------------------------------

function fnIsDate(sVal,sType,iUS)
{
	if (sVal == "") return true;
	if (!bReady) {
	    //sVal = fnReadyBlurDate(sVal,sType,iUS);
	    bReady = true; //vk 09.14
	}
	if (sVal == "")
		return false;
	else
		return true;
} // fnIsDate
*/

//' -------------------------------
//'function fnKeyDownDate
//' -------------------------------

/*
function fnKeyDownDate(cnt,sType,iUS)
{
	var sVal = cnt.value;

	if (fnIsAction())
	{
	    bReady = true;
	    gIsDate = true; //fnIsDate(sVal,sType,iUS);
		//if (!gIsDate)
		//	cnt.blur();
	}
} // fnKeyDownDate
*/