function fnFocusSL(cnt,lin) //lin vk 04.03
{ 
    if (!IsEnable()) {
        return;
    }
    if (bMayMarkCurrent) {
	    document.getElementById("Hfld").value = cnt.value.substr(numS,numL);
	    document.getElementById("Hfind").value = cnt.value.substr(indS,indL);
    }
    //var oR = document.getElementsByName("R");
    //if (oR != null)
    //{
    //    for (i=0;i<oR.length;i++)
    //    {
	//        //oR[i].className = 'InpComSL';
	//        var l = $(oR[i]).attr("linenum");
	//        if (l == lin)
	//        {
	//	        oR[i].checked=true;
	//	        if (event.type=="focus" && event.srcElement==oR[i]); //vk 10.07
	//	        else
	//		        oR[i].focus(); //vk 09.06
	//	        var pch = $(oR[i]).attr("pch");
	//	        if (pch) {
	//		        document.getElementById("Hfpch").value = pch;
	//	        }
	//        }
    //    }
    //}
    ////vk 04.03, 06.03
    //var coll = document.getElementsByTagName('*');
    //var iFrom=1;
    //var iTo=coll.Gcolumns.value-0;
    //if (iTo>0)
    //{
    //    var jFrom=coll.Gline_first.value-0;
    //    var jTo=coll.Gline_last.value-0;
    //    for (i=iFrom;i<=iTo;i++)
    //    {
    //       // j = coll.Gchecked.value;
    //        j = document.getElementById("Gchecked").value;
	//        if (j>=jFrom && j<=jTo)
	//        {
    //            var o=document.getElementById("G"+MyRight("000"+j,3)+MyRight("00"+i,2));
	//	        if (o!=null)
	//	        {
    //                s="grid"+$(o).attr("origclass");
    //                if ($(o).attr("className") != s) {
	//	                $(o).attr("class",s);
	//	            }
	//	        }
	//        }
	//        j=lin-0;
	//        {
	//            var o = document.getElementById("G" + MyRight("000" + j, 3) + MyRight("00" + i, 2));
	//	        if (o!=null)
	//	        {
	//		        s="gridLF";
	//		        if ($(o).attr("className") != s) {
	//		            $(o).attr("class", s);
	//		        }
	//	        }
	//        }
    //    }
    //}
    //coll.Gchecked.value=lin-0;
    //cnt.className = 'InpFocSL';
}

function fnClearOptions() //vk 03.03
{
if (!IsEnable()) return;
var oR = document.getElementsByName("R");

if (oR != null)
{
//	if (oR.length==undefined) //vk 05.03
//	{
////if one line only
//		oR.className = 'InpComSL';
//		oR.checked = false;
////
//	} else {
		for (i=0;i<oR.length;i++)
		{
//if one line of many
			//oR[i].className = 'InpComSL';
			oR[i].checked = false;
//
		}
//	}
}
//vk 04.03, 06.03
var coll = document.getElementsByTagName('*');
var iFrom=1;
var iTo=coll.Gcolumns.value-0;
if (iTo>0)
{
var jFrom=coll.Gline_first.value-0;
var jTo=coll.Gline_last.value-0;
for (i=iFrom;i<=iTo;i++)
{
	j=coll.Gchecked.value;
	if (j>=jFrom && j<=jTo)
	{
		var o=coll["G"+MyRight("000"+j,3)+MyRight("00"+i,2)];
		if (o!=null)
		{
		    s = "grid" + $(o).attr("origclass");
			if (o.className!=s) o.className=s;
		}
	}
}}
coll.Gchecked.value=0;
}

//function fnClickGrid(o) //vk 04.03, 08.04
//{
//    var linenum = $(o).attr("linenum");
//    fnClickGridByLine(linenum)
//}

//function fnClickGridByLine(lin) //vk 08.04
//{
//    //debugger;
//    var oR = document.getElementsByName("R");
//	if (oR != null)
//	{
////		if (oR.length==undefined)
////		{
//////if one line only
////			if (oR.linenum==lin)
////			{
////				oR.checked=true;
////				fnFocusSL(oR,lin);
////			}
//////
////		} else {
//			for (var j=0;j<oR.length;j++)
//			{
////if one line of many
//				if ($(oR[j]).attr("linenum")==lin)
//				{
//					oR[j].checked=true;
//					fnFocusSL(oR[j],lin);
//				}
//			}
////		}
//	}
//}

//function fnpsl_dblclick_f_line(oo) //vk 02.04
//{
//	if (!IsEnable()) return;
//	var lin = $(oo).attr("linenum"); //vk 08.04
//	var oR = document.getElementsByName("R");
//	if (oR == null) return;
//	var o;
////	if (oR.length==undefined)
////	{
//////if one line only
////		if (oR.linenum==lin) o=oR;
//////
////	} else {
//		for (var j=0;j<oR.length;j++)
//		{
////if one line of many
//		    if (oR[j].getAttribute("linenum") == lin) o = oR[j];
////
//		}
////	}
//	if (o == null) return;
//	fnpsl_dblclick_folders(o);
//}

function CheckBoxClick(o) //vk 06.03
{
    var sName = o.name.substring(1);

    var oo = document.getElementById(sName); //o.name.substring(1));
    oo.value = ' ';
	if (o.checked) {
		oo.value = $(o).attr("mark"); //o.mark;   
	}
}

function CheckBoxDblClick(o) //vk 06.03
{
	if (o.tagName == "SPAN") o = o.parentNode.childNodes[1];
	o.checked=true;
	o.focus(); //vk 09.06
	CheckBoxClick(o); //,true);
	gFocus=o; //vk 05.09
	CmdPchClick_Common("", "00", false, false, 0);
}

function CheckBoxClick_Grid(o) //vk 08.04
{
    var linenum = $(o).attr("linenum");
    var oInput = document.getElementsByTagName("input");
   // debugger;
    for (var j = 0; j < oInput.length; j++)
    {
        var oo = oInput[j];
        if (oo.type == "checkbox") {
            if ($(oo).attr("linenum") == linenum) {
                oo.checked = !oo.checked;
                CheckBoxClick(oo, true);
            }
        }
        if (oo.type == "radio") {
            if ($(oo).attr("linenum") == linenum) {
                //vk 12.04
                oo.checked = true;
                oo.focus(); 
                fnClickGridByLine(linenum);
            }
        }
    }
}

function CheckBoxDblClick_Grid(o) //vk 08.04
{
    var linenum = $(o).attr("linenum");
    var oInput = document.getElementsByTagName("input");
    for (var j = 0; j < oInput.length; j++)
    {
        var oo = oInput[j];
        if (oo.type == "checkbox") {
            if ($(oo).attr("linenum") == linenum) {
                CheckBoxDblClick(oo);
            }
        }     
        if (oo.type == "radio") {
            if ($(oo).attr("linenum") == linenum) {
                fnpsl_dblclick(); //(oo);
            }
        }
    }
}

function MyRight(s,n)
{
	return s.substr(s.length-n,s.length);
}
