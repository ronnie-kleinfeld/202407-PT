// ---------------------------------------------
//functions that taking care numbers
// written by Michael Shmushkin 12/2002
// example of use
//	<input dir='ltr' name='i1' id='i1' tabIndex=1 maxLength=11 size = 11 
//	onblur="fnBlurNum(this,10,0,' ');" /* fnBlurNum(cnt,len,dec,edt) */
//	onfocus='fnFocusNum(this);'
//	onkeypress="fnKeyPressNum(this,'N',0);" /* fnKeyPressNum(cnt,typ,dec) */
//      onkeydown="fnKeyDownNum(this,10,0);"   /* fnKeyDownNum(cnt,len,dec) */
//      >

// Description of parameters: 
// typ - type of numbers : 'N' - numeric without sign, 'S' - numeric with sign
// len - length of number
// dec - number of symbols after decimal point
// edt - edit of display : ' ' - without edition, 'J' - with comma ( 2,345,6.23 )
// ---------------------------------------------

var gIsNumber=true;

function BoundNumber(len,dec)
{
	// Create max number with width = len and dec decimal points
	ilen = parseFloat(len);
	idec = parseFloat(dec);
	ilen1 = ilen - idec;
	fnum = Math.pow(10,ilen1);
	return fnum;
}

function InsertComma(sval)
{
	i = sval.indexOf(".");

	if (i <= 0)
	{
		fval = sval;
		dval = "";
	}
	else
	{
		fval = sval.substring(0,i);
		dval = sval.substr(i+1);
	}

	l = fval.length;
	fval1 = "";

	// insert "," in the string
	for (j=l-1,k=1; j >=0; j--,k++)
	{
		if (k % 3 == 0)
		{
			fval1 = fval.substr(j,1) + fval1;
			if (j != 0)
				fval1 = "," + fval1;
		}
		else
			fval1 = fval.substr(j,1) + fval1;
	} // for j

	if (dval != "")
		fval1 += "." + dval;

	return fval1;
}

function TailLen(cnt)
{
	var s=cnt.value;
	var i=s.indexOf(".");
	if (i<0)
		return 0;
	else
		return s.length-i-1;
}

//' -------------------------------
//'function onBlur for text fields
//' -------------------------------

function fnBlurNum(cnt,len,dec,edt) //+vk 03.06
{
	if (!IsEnable()) return;
	if (bInBlur) return true;
	bInBlur=true;
	bStopped=false;
	if (cnt.value == "")
	{
		gIsNumber = true;
		bInBlur=false;
		return;
	}	

	//vk 02.04
	var nval = cnt.value;
	//var nval = new Number(cnt.value);

	if (isNaN(cnt.value))
	{
		bStopped=true;
		//cnt.focus();
		MyMsgBox(msg_Num(1),"false",cnt);
		gIsNumber = false;
		if (event) { //vk 01.14
		    event.returnValue = false;
		    event.cancelBubble = true;
		}
		bInBlur=false;
		return;
	}

	sign = "";
	if (nval < 0)
	{
		sign="-";
		nval *= -1;
	}

	fBound = BoundNumber(len,dec);

	//vk 02.04
	var bGreater;
	if (nval.toString().indexOf(".")<0) {
		if (nval.toString().length > fBound.toString().length)
			bGreater = true;
		else if (nval.toString().length < fBound.toString().length)
			bGreater = false;
		else
			bGreater = (nval.toString() > fBound.toString());
	} else {
		bGreater = (nval >= fBound);
	}

	if (bGreater || TailLen(cnt) > dec)
	{
		bStopped=true;
		//cnt.focus();
		MyMsgBox(msg_Num(1),"false",cnt);
		gIsNumber = false;
		if (event) { //vk 01.14
		    event.returnValue = false;
		    event.cancelBubble = true;
		}
		bInBlur=false;
		return;
	}

	//vk 02.04
	if (dec==0) {
		sval = nval.toString();
	} else {
		nval = new Number(nval);
		sval = nval.toFixed(parseInt(dec));
	}

	if (edt == 'J') sval = InsertComma(sval);
	sval = sign + sval;
	cnt.value = sval;		
	gIsNumber = true;
	bInBlur=false;
}

function fnFocusNum(cnt)
{
	var val = cnt.value;
	cnt.value = val.replace(/,/g,"");
	mySelect(cnt);
}

// -----------------------------
//function KeyDown on input of type "N" and "F"
// ------------------------------

function DecKeyNum(key)
{
	if (key < 48 || key > 57)
		return false;
	else
		return true;

}  // DecKeyNum

function fnKeyPressNum(cnt,typ,dec)
{
	var name = cnt.name;
	var key = event.keyCode;

	if (typ == "S")
	{
		if (key == 43 || key == 45)
		{
			event.returnValue = true;
			return;
		}
	}
	if (typ == "N" || typ == "S")
	{
		if ( dec == "00")
			event.returnValue = DecKeyNum(key);
		else
		{
			if (key == 46)
				event.returnValue = true;
			else
				event.returnValue = DecKeyNum(key);
		}
	}
	else
		event.returnValue = true;
} // fnKeyPress

//' -------------------------------
//'function fnIsNumber
// return true if value is Number
//' -------------------------------

function fnIsNumber(val,len,dec)
{
	if (val == "") return true;
	var nval = new Number(val);
	if (isNaN(nval))
		return false;
	else
	{
		if (nval < 0)
		{
			sign="-";
			nval *= -1;
		}
		else
		{
			sign = "";
		}

		fBound = BoundNumber(len,dec);

		if (nval >= fBound || TailLen(cnt) > dec) //vk 08.03
			return false;
		else
			return true;

	} //else (isNaN(nval))
}

//' -------------------------------
//'function fnKeyDownNum
//' -------------------------------

function fnKeyDownNum(cnt,len,dec)
{
	var val = cnt.value;

	if (fnIsAction())
	{
		gIsNumber = fnIsNumber(val,len,dec);
		if (!gIsNumber)
			cnt.blur();
	}
} // fnKeyDownNum
