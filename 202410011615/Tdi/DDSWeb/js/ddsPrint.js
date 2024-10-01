//vk 09.05
var m_left;
var m_right;
var m_top;
var m_bottom;
var REGKEY="HKCU\\\Software\\Microsoft\\Internet Explorer\\PageSetup\\margin_";
var bChangeMargins;
var bPrintServerFromButton; //vk 05.06
var oWindow; //vk 02.07
var bMayClose; //vk 10.10
//var sFileToDspClient; //vk 12.13

//vk 09.05
function get_m()
{
	try {
		var sh = new ActiveXObject("WScript.Shell");
		m_left   = sh.RegRead(REGKEY+"left");
		m_right  = sh.RegRead(REGKEY+"right");
		m_top    = sh.RegRead(REGKEY+"top");
		m_bottom = sh.RegRead(REGKEY+"bottom");
		sh = null;
	} catch (e) {}
}
function set_m(l,r,t,b)
{
	try { //RegRead vk 10.07
		var sh = new ActiveXObject("WScript.Shell");
		if (sh.RegRead(REGKEY+"left")!=l)   sh.RegWrite(REGKEY+"left", l);
		if (sh.RegRead(REGKEY+"right")!=r)  sh.RegWrite(REGKEY+"right", r);
		if (sh.RegRead(REGKEY+"top")!=t)    sh.RegWrite(REGKEY+"top", t);
		if (sh.RegRead(REGKEY+"bottom")!=b) sh.RegWrite(REGKEY+"bottom", b);
		sh = null;
	} catch (e) {}
}

function PrintServer(nPage,nPages,nSec,sEsc) //vk 09.05
{
	ShowPages(nPage-1,nPages);
	if (document.getElementById("Hwait").value=="open")
	{ //vk 02.07
		try
		{
			var tmp=oWindow.document.title;
		}
		catch(e)
		{
		    document.getElementById("Hwait").value="";
			ExitFromPrintServer(nPages,nSec,sEsc);
			return;
		}
	} else {
	    document.getElementById("Hwait").value="open";
		var s="0000"+nPage;
		var h=100;
		var w=300;
		var t;
		var l;
		if (bPrintServerFromButton)
		{
		    t = (document.documentElement.clientHeight - h) / 2;
		    l = (document.documentElement.clientWidth - w) / 2;
		} else {
			t=0;
			l=0;
		}
		oWindow=window.open("screenAdd.aspx?page="+s.substring(s.length-4),"",
			"height="+h+",width="+w+",top="+t+",left="+l+",status=0,toolbar=0,menubar=0,location=0,"
			+"directories=0,resizable=0,scrollbars=0,titlebar=1");
		if (!bPrintServerFromButton)
			window.focus();
		if (nPage>=nPages)
		{
			ExitFromPrintServer(nPages,nSec,sEsc);
			return;
		}
		nPage++;
	}
	setTimeout("PrintServer("+nPage+","+nPages+","+nSec+",'"+sEsc+"')",500)
}
function ExitFromPrintServer(nPages,nSec,sEsc) //vk 09.07
{
	if (bPrintServerFromButton)
		DisableButtonsOnly(false);
	else
		setTimeout("TryToSubmit("+nSec+",'"+sEsc+"',"+nPages+")",500);
}

function TryToSubmit(nSec,sEsc,nPages) //vk 09.05
{
	ShowPages(nPages,nPages);
	if (document.getElementById("Hwait").value!="open")
	    setTimeout("document.getElementById('Hfcmd').value='"+sEsc+"';doSubmit();",nSec*1000);
	else
		setTimeout("TryToSubmit("+nSec+",'"+sEsc+"',"+nPages+")",500);
}

function ShowPages(nSent,nTotal) //vk 09.05
{
    if (document.getElementById("lblMsg"))
	{
        document.getElementById("lblMsg").innerText = msg(15)+nSent+msg(16)+nTotal;
        document.getElementById("lblMsg").className = 'Msg';
	}	
}

//function SendPdf(sFileName,bMail) //vk 12.06, 03.09
//{
//	var arrArgs = new Array();
//	arrArgs[0] = sFileName; //vk 08.07
//	arrArgs[1] = bMail; //vk 03.09
//	arrArgs[2] = msg(22); //vk 04.09
//	FillForModalPing(arrArgs);
//	clearInterval(PingInterval);
//	var winStats = "dialogHeight:"+XP(200)+"px; dialogWidth:300px; edge:sunken; scroll:No; " 
//		+ "dialogTop:"+(screen.height/2-100)+"px; dialogLeft:"
//		+ (screen.width/2-150)+"px; help:No; resizable:No; status:No; unadorned:Yes;";
//	bInCalendar=true;
//	rValue = window.showModalDialog('email.htm', arrArgs, winStats);
//	bInCalendar=false;
//	StartPing();
//	//vk 04.09
//	if (rValue && !bMail)
//		window.open("tmp-"+sFileName);
//}

//vk 10.10
function PrintAndClose()
{
    try{
        document.getElementById("WebBrowser1").ExecWB(6,2);
    }
    catch(e){
        MyMsgBox(e.message);
    }
    document.getElementById("Hx").value='';
    WaitAndClose();
}
function WaitAndClose()
{
    if (bMayClose)
        window.close();
    else
        setTimeout('WaitAndClose();',500);
}
//function BeforePrint(l,r,t,b)
//{
//    bMayClose=false;
//    if (document.getElementById("leftbuttons"))
//        document.getElementById("leftbuttons").style.visibility = "hidden"; //vk 10.06
//    if (document.getElementById("leftbuttonsbkg"))
//        document.getElementById("leftbuttonsbkg").style.visibility = "hidden"; //vk 05.07
//	bChangeMargins=(l!="");
//	if (!bChangeMargins) return;
//	get_m();
//	set_m(l,r,t,b);
//}
//function AfterPrint()
//{
//    if (document.getElementById("leftbuttons"))
//        document.getElementById("leftbuttons").style.visibility = "visible"; //vk 10.06
//    if (document.getElementById("leftbuttonsbkg"))
//        document.getElementById("leftbuttonsbkg").style.visibility = "visible"; //vk 05.07
//	if (!bChangeMargins) {
//	    bMayClose=true;
//	    return;
//	}
//	setTimeout("RestoreMargins();",500);
//}
//function RestoreMargins()
//{
//    set_m(m_left,m_right,m_top,m_bottom);
//    bMayClose=true;
//}
//function PrintTif(bTryPdf) //vk 02.13
//{
//    CallDspClient("Print","Action=Print"+CRLF+
//        "Printer=" + document.getElementById("Hprinter").value + CRLF +
//        "Url=" + document.getElementById("Hurl").value + CRLF +
//        "Tif=" + document.getElementById("Htif").value + CRLF +
//        "Top=" + document.getElementById("HtifTop").value * 37.8 + CRLF +
//        "Left=" + document.getElementById("HtifLeft").value * 37.8 + CRLF +
//        "Tray=" + document.getElementById("Htray").value,
//        bTryPdf)
//}
function PrintPdf() //vk 02.14
{
    var o = document.getElementById("acrobat");
    if (o.tagName.toLowerCase() == "iframe")
        o.contentWindow.print();
    else
        o.print();
}
function PrintPdf_Consist(sPdf,bHova) //vk 12.14
{
try {
    var myAx = new ActiveXObject("PBPDFWrapper.PBPDFWrapper");
    var result = myAx.DownloadAndPrintPDFToTray(sPdf,"",-1,-1,(bHova?"Tray=4":"Tray=1"));
    if (result!=0)
        MyMsgBox(msg_Consist("rc") + result);
    else {
        document.getElementById("Hfcmd").value="00";
        doSubmit();
    }
} catch(e)
{
    MyMsgBox(e.message);
}
}

//vk 12.13
//function TraysList(sAns) {
//var even;
//    try {
//        //var o = document.all.DspPrint;
//        //var p = o.PrinterList();
//        var a = sAns.split(CRLF);
//        var t = "<table class='trays' width=100%>";
//        var pr;
//        var even;
//        for (v=0;v<a.length;v++) {
//	        var w = a[v];
//	        if (w!="") {
//	            if (w.substr(0,1)!="*") {
//		            //printer
//		            t += "<tr><td colspan=4 class='traystitle'><b>" + w + "</b></td></tr>" + CRLF;
//		            pr = w;
//		            even = 1;
//	            } else {
//		            //tray
//		            w = w.substr(2);
//		            var i = w.indexOf("-");
//		            t += "<tr><td style='display:none;'>" + pr + "</td><td style='display:none;'>";
//		            if (i>-1) t += w.substr(0,i-1); else t += "0";
//			        t += "</td><td class='trays"+even+"'>&nbsp;&nbsp;&nbsp;&nbsp;";
//		            if (i>-1) t += w.substr(i+2); else t += w;
//		            t += "</td><th class='trays"+even+"'><input type=radio name=hTrayRadio></th></tr>" + CRLF;
//		            even = 3 - even;
//	            }
//	        }
//        }
//        t += "</table>";
//    } catch(e) {
//        t = e.message;
//    }
//    return t;
//}
//function DrawTrays() {
//    //document.all("Htrays").innerHTML=TraysList();
//    CallDspClient("List","Action=List",false);
//}
//function ChooseTray(sAction) {
//    var sPrinter="";
//    var sTray="0";
//    if (document.getElementById("hTrayRadio")) {
//        if (document.getElementsByName("hTrayRadio").length) {
//	        //more than one tray
//            for (j = 0; j < document.getElementsByName("hTrayRadio").length; j++) {
//                var r = document.getElementById("hTrayRadio")[j];
//		        if (r.checked) {
//			        var tr = r.parentElement.parentElement;
//			        sPrinter = tr.children[0].innerText;
//			        sTray = tr.children[1].innerText;
//		        }
//	        }
//        } else {
//	        //one tray only
//            var r = document.getElementById("hTrayRadio");
//	        if (r.checked) {
//		        var tr = r.parentElement.parentElement;
//		        sPrinter = tr.children[0].innerText;
//		        sTray = tr.children[1].innerText;
//	        }
//        }
//    } else {
//	    //no trays at all
//        MyMsgBox(msg_Tray("no trays"));
//        return;
//    }
//    if (sPrinter=="") {
//        MyMsgBox(msg_Tray("choose"));
//        return;
//    }
//    switch (sAction) {
//        case "testpage":
////            try {
////                //var o = document.all.DspPrint;
////                //var s = document.all("Hurl").value+"Pics/testpage.tif";
////                var s = document.all("Htestpage").src;
////                //rc=o.sendToPrintTifFile(sPrinter,s,0,0,sTray-0);
////            } catch(e) {
////                rc=e.message;
////            }
////            if (rc!="OK") MyMsgBox(rc);
//            //CallDspClient("Print",
//            //    "Action=Print"+CRLF+
//            //    "Printer="+sPrinter+CRLF+
//            //    "Tif=" + document.getElementById("Htestpage").src + CRLF +
//            //    "Top=0"+CRLF+"Left=0"+CRLF+"Tray="+sTray,
//            //    false)
//            break;
//        case "sendto400":
//            for (i=0;i<150;i++) sPrinter+=" ";
//            document.getElementById(document.getElementById("Hv1_name").value).value = sPrinter.substr(0, 50);
//            document.getElementById(document.getElementById("Hv2_name").value).value = sPrinter.substr(50, 50);
//            document.getElementById(document.getElementById("Hv3_name").value).value = sPrinter.substr(100, 50);
//            document.getElementById(document.getElementById("Hv4_name").value).value = sTray;
//            document.getElementById(document.getElementById("Hv5_name").value).value = document.getElementById("HIP").value;
//            fnBtnClick(event.srcElement);
//            break;
//    }
//}

//vk 12.13
//function CallDspClient(sAction,sText,bTryPdf) {

//try {
//	var d=new Date();
//	var p;
//	sFileToDspClient = document.getElementById("Hbuffer").value;
//	p=d.getFullYear();                 sFileToDspClient=sFileToDspClient+p;
//	p=d.getMonth()+1; if(p<10)p="0"+p; sFileToDspClient=sFileToDspClient+p;
//	p=d.getDate();    if(p<10)p="0"+p; sFileToDspClient=sFileToDspClient+p;
//	p=d.getHours();   if(p<10)p="0"+p; sFileToDspClient=sFileToDspClient+p;
//	p=d.getMinutes(); if(p<10)p="0"+p; sFileToDspClient=sFileToDspClient+p;
//	p=d.getSeconds(); if(p<10)p="0"+p; sFileToDspClient=sFileToDspClient+p;
//	p=d.getMilliseconds();
//	                  if(p<10)p="00"+p;else if(p<100)p="0"+p;
//	                                   sFileToDspClient=sFileToDspClient+p;

//	var fso = new ActiveXObject("Scripting.FileSystemObject");
//	var a=fso.CreateTextFile(sFileToDspClient + ".txt",true,true);
//	a.WriteLine(sText);
//	a.close();
//	fso.MoveFile(sFileToDspClient + ".txt", sFileToDspClient + ".msg")
//	setTimeout("CallDspClient_cont('"+sAction+"',0,"+(bTryPdf?"true":"false")+");",100);
//} catch(e) {
//    MyMsgBox(e.Message);
//}

//}
//function CallDspClient_cont(sAction,nNumber,bTryPdf) {

//try {
//	var fso = new ActiveXObject("Scripting.FileSystemObject");
//	if (fso.FileExists(sFileToDspClient + ".don")) {
//	    //var a=fso.OpenTextFile(sFileToDspClient + ".ans",1,false,1);
//	    var f = fso.GetFile(sFileToDspClient + ".ans");
//	    var a = f.OpenAsTextStream(1,-1);
//	    var sText=a.ReadAll();
//	    a.close();
//        if (sAction=="List") {
//            document.getElementById("Htrays").innerHTML = TraysList(sText);
//        } else if (sText!="OK") {
//            if (bTryPdf)
//                document.getElementById("acrobat").print();
//            else
//                MyMsgBox(sText);
//        }
//	    fso.DeleteFile(sFileToDspClient + ".ans");
//	    fso.DeleteFile(sFileToDspClient + ".don");
//	} else {
//	    var nMax;
//	    if (sAction=="List") nMax=20; else nMax=400;
//	    if (nNumber>nMax) {
//	        MyMsgBox("Run DspClient, please")
//	        fso.DeleteFile(sFileToDspClient + ".msg");
//	    } else {
//	        setTimeout("CallDspClient_cont('"+sAction+"',"+(nNumber+1)+","+(bTryPdf?"true":"false")+");",100);
//	    }
//	}
//} catch(e) {
//    MyMsgBox(e.Message);
//}

//}
