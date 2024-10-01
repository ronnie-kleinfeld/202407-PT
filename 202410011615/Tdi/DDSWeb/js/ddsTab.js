// ---------------------------------------------
// functions that taking care of full advance
// written by Michael Shmushkin 12/2002
// example of use
//<input id='I2' name='I2' size=3 maxLength=3 tabIndex=5
//onfocus="fnFocus_tab(this)" onblur="fnBlur_tab(this)" >
// ---------------------------------------------

var giTimeID;
var gsValuePrev;
var gaCnt = new Array();
var bStopped = false; //vk 08.03
var oNext = null; //vk 06.10

function compare(ar1,ar2)
{
    //------------------
    // function for comparision two elements
    // for sorting array
    // -----------------
    arg = ar1.split(",");
    arg1 = Number(arg[1]);

    arg = ar2.split(",");
    arg2 = Number(arg[1]);

    if (arg1 < arg2)  return -1;
    if (arg1 == arg2) return 0;
    if (arg1 > arg2)  return 1;
}

function fnFocus_tab(cnt, per, cod, force, typ, p1, p2, p3) //force vk 07.13, typ etc. vk 01.14
{
    //----------------------------
    // setInterval and value on focus
    //----------------------------

    clearInterval(giTimeID);
    giTimeID = 0;
    if (bStopped) return;

    //copyedit vk 03.16
    if (typ) { } else typ = '';
    if (p1) { } else p1 = 0;
    if (p2) { } else p2 = 0;
    if (p3) { } else p3 = '';

    cName = cnt.name;
    giTimeID = setInterval("IsNeedTab('" + cName + "'," + per + ",'" + cod + "',"
	    + (force ? "true" : "false") + ",'" + typ + "'," + p1 + "," + p2 + ",'" + p3 + "')", 500);
    gsValuePrev = cnt.value;
    bStopped = false; //vk 08.03
}

function fnBlur_tab(cnt)
{
    //----------------------------
    // clear Interval on Blur
    //----------------------------

    if (bBlurCancel) return; //vk 07.06
    if (giTimeID != 0)
    {
        clearInterval(giTimeID);
        giTimeID = 0;
    }
}

function IsNeedTab(cName,per,cod,force,typ,p1,p2,p3) //force vk 07.13, typ etc. vk 01.14
{
    //----------------------------
    // Test if control is filled to its max length
    // if yes then put focus on next control
    //----------------------------

    oNext=null;
    cnt = document.getElementById(cName);
    if (cnt) //vk 04.05
    {
        maxLength = cnt.maxLength;
        sVal = cnt.value
        if ((sVal != gsValuePrev) || force)
        {
            gsValuePrev = sVal;
            if ((sVal.length == maxLength) || force)
            {
                sNextName = NextTab(cName);
                if (sNextName != "")
                {
                    cntNext = document.getElementById(sNextName);
                    //if (cntNext.length==undefined)
                        oNext=cntNext;
                    //else
                    //    oNext=cntNext[0]; //vk 09.06
                    if (!bClickLocked) {
                        if (per) {
                            //vk 01.14 from here
                            switch (typ) {
                                case "D":
                                case "Y":
                                case "H":
                                case "Q":
                                    fnBlurDate(cnt,typ,p1);
                                    break;
                                case "T":
                                case "U":
                                    fnBlurTime(cnt,typ);
                                    break;
                                case "N":
                                case "S":
                                    fnBlurNum(cnt,p1,p2,p3);
                                    break;
                                case "A":
                                    fnBlurText_UpperOnly(cnt,p1); //,false);
                                    break;
                            }
                            if (bStopped) {
                                bStopped=false;
                                return;
                            }
                            //vk 01.14 till here
                            if (fnBlur_Check(cnt,cod,null,true))
                                return; //vk 09.11
                        }
                        try{ //vk 06.15
                            oNext.focus();
                        } catch(e) {}
                        oNext=null;
                    } else cnt.blur(); //vk 07.10
                    //vk 05.03
                    if (giTimeID != 0)
                    {
                        clearInterval(giTimeID);
                        giTimeID = 0;
                    }
                }
            }
        }
    } else
        if (giTimeID != 0)
        {
            clearInterval(giTimeID);
            giTimeID = 0;
        }
}

function NextTab(cName)
{
    //----------------------------
    // Choose name of next control from array of controls
    //----------------------------

    var ret="";
    l = gaCnt.length;
    for (i=0; i<l; i++)
    {
        sName = gaCnt[i];
        if (gaCnt[i] == cName) {
            if (i == l - 1)
                i = 0;
            else
                i += 1;
            ret = gaCnt[i];
            break;
        }
    }
    //vk 08.13
    if (ret!="")
        if (document.getElementById(ret).tabIndex == "-1") {
            i += 1;
            if (i > l - 1)
                i = 0;
            ret = gaCnt[i];
        }
    return ret;
}

function body_load_tab()
{
	//----------------------------------
	// Create and sort array of controls
	// that have name //and max length
	//----------------------------------
   
    // var cnts = document.all.tags("INPUT");
   // var cnts = document.getElementsByTagName("INPUT");
    var cnts = $(":input");
	l = cnts.length;
	j = 0;

    //vk 07.13
	var cg = "";
	if ($("#Hcg").get(0)) {
	    cg = $("#Hcg").val();
	}

	for (i=0;i<l;i++)
	{
		cnt = cnts[i];
		if ((cnt.type == "text" || cnt.type == "checkbox" || cnt.type == "radio" || cnt.type == "password" || cnt.type == "button")
			&& cnt.name != "" && cnt.name != "R" && cnt.tabIndex != "" && (cnt.tabIndex != -1 || (cg!="" && cnt.name==cg))) //vk 08.06, 09.06, 07.13
				if (cnt.name.substr(0,1) != "C") //vk 01.07
				{
					gaCnt[j] = cnt.name + "," + cnt.tabIndex;
					j++;
				}
	}

	arr = gaCnt.sort(compare);
	l = gaCnt.length;
	for (i=0; i<l; i++)
	{
		arg = arr[i].split(",");
		gaCnt[i] = arg[0];
	}
}
