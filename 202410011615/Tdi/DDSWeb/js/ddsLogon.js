function FromFile()
{
	try
	{
	    if (document.getElementById("hdnFileForCode").value=="")
			return "no param";
		var fso = new ActiveXObject("Scripting.FileSystemObject");
		var a = fso.OpenTextFile(document.getElementById("hdnFileForCode").value);
		var b=a.ReadLine();
		a.close();
		a=null;
		fso=null;

		c=b;
		var s = 0;
		while (c.charAt(s)==" " || c.charAt(s)=="\n") s++;
		if (s==c.length)
		{
			c="";
		} else {
			var e = 0;
			while (c.charAt(c.length-e-1)==" " || c.charAt(c.length-e-1)=="\n") e++;
			c = c.substring(s, c.length-e);
		}

		if (c=="")
			return "empty";
		return b;
	}
	catch(e)
	{
		switch (e.message)
		{
			case "Path not found": return "no path";
			case "File not found": return "not found";
			case "Automation server can't create object": return "disabled";
			default: return "other err";
		}
	}
}

function btnHelp_click() //vk 01.07
{
}
function btnOk_click1() //vk 11.05, 12.05
{
    //var W = document.getElementById("hdnResolutionW");
    //var H = document.getElementById("hdnResolutionH");
    //GetResol(document.getElementById("hdnResolutionMethod"), W, H);
}
function btnOk_click(ok1, sLogonScreen) //vk 11.05, 12.05
{
    if (document.getElementById("txtUser"))
        document.getElementById("hdnUser").value = document.getElementById("txtUser").value; //vk 08.06
    else
        document.getElementById("hdnUser").value = ""; //vk 04.17
    if (document.getElementById("txtPassword"))
        document.getElementById("hdnPassword").value = document.getElementById("txtPassword").value; //vk 08.06
	else
        document.getElementById("hdnPassword").value = ""; //vk 04.17
    if (document.getElementById("hdnUserAgent"))
        document.getElementById("hdnUserAgent").value = navigator.userAgent; //vk 12.14
    if (document.getElementById("hdnTz"))
        document.getElementById("hdnTz").value = document.getElementById("txtTz").value; //vk 11.07
	if (ok1)
	    document.getElementById("hdnFromOK").value = '1'; //vk 08.17
	else if (document.getElementById("hdnFromOK").value == '1')
	    document.getElementById("hdnFromOK").value = '0'; //this does not work with external agent!!!
	if (document.getElementById("lblMsg"))
	    document.getElementById("lblMsg").innerHTML = "";
	LockButtons(true);
	if (document.getElementById("hdnUser_"))
	    document.getElementById("hdnUser_").value = document.getElementById("hdnUser").value;
    if (document.getElementById("hdnPassword_"))
	    document.getElementById("hdnPassword_").value=document.getElementById("hdnPassword").value;
    if (document.getElementById("hdnTz")) document.getElementById("hdnTz_").value=document.getElementById("hdnTz").value; //vk 11.07

	if (sLogonScreen == 'q')  
    	btnOk_continue();  
    else 
    	document.getElementById("hdnReady").value="";
	//vk 01.15
	if (document.getElementById("hdnSso")) {
	    if (document.getElementById("hdnSso").value == "Continue") {
	        btnOk_continue();
	        return;
	    }
	    //if (document.getElementById("hdnSso").value == "Logonq") {
	    //    GetSize();
	    //    document.getElementById("hdnResolutionW").value = MaxWidth;
	    //    document.getElementById("hdnResolutionH").value = MaxHeight;
	    //}
	}
	document.forms.Attach.submit();
}

function btnOk_continue() //vk 11.05
{
    if (document.getElementById("oIFrame"))
        document.getElementById("oIFrame").src = "Empty.htm";
    if (document.getElementById("hdnResult").value != "")
	{
        document.getElementById("lblMsg").innerHTML = document.getElementById("hdnResult").value; //vk 07.06
		LockButtons(false);
		return;
	}
		Ok_onClient();
}

function Ok_onClient()
{
    if (document.getElementById("hdnFromOK").value == '1')
        return; //vk 12.05

	LockButtons(true);

	document.getElementById("hdnFromOK").value='1';
	document.getElementById("lblWait").innerText = msg(1);
	document.getElementById("lblMsg").innerHTML = "";
	//GetResol(document.getElementById("hdnResolutionMethod"),document.getElementById("hdnResolutionW"),document.getElementById("hdnResolutionH"));
	document.getElementById("hdnCodeFromFile").value = FromFile();

	var coll = document.getElementsByTagName("*");
	var count = coll.length;
	for (var i = 0; i < count; i++) coll[i].style.cursor = "wait";

	WebForm2.submit(); //ik 06/2005
}

function LockButtons(bLock) //vk 12.05, 04.17
{
    if (document.getElementById("txtUser")) document.getElementById("txtUser").disabled = bLock;
    if (document.getElementById("txtPassword")) document.getElementById("txtPassword").disabled = bLock;
    if (document.getElementById("txtTz")) document.getElementById("txtTz").disabled = bLock; //vk 11.07
    if (document.getElementById("btnOk")) document.getElementById("btnOk").disabled = bLock;
	if (bLock)
		document.body.style.cursor = "wait";
	else
		document.body.style.cursor = "default";
}

function OnEnterKeyDown()
{
	if (event.keyCode != 13) return;
	try {
		event.returnValue = false;
		document.getElementById("btnOk").click();
	} catch(e){} //vk 04.06
}

function body_load()
{
    try {
        window.moveTo(0, 0);
        window.resizeTo(screen.availWidth, screen.availHeight);
     
        //if($("#lblCopyright")){
        //    switch ($("#hdnCopyright").val()) 
        //    {
        //        case "link":
        //            $("#lblCopyright").wrapInner(GetCopyright());
        //            break;
        //        case "text":
        //            $("#lblCopyright").wrapInner(GetCopyrightNetto());
        //            break;
        //    }
        //}
		if (document.getElementById("imgLogoLeft")) {
			document.getElementById("imgLogoLeft").prop( "disabled", false );
        }
		if (document.getElementById("imgLogoRight")){
			document.getElementById("imgLogoRight").prop("disabled", false);
        }

        $(document).ready(function () {
			//document.getElementById("MainContent_UserName").focus();
			document.getElementById("txtUser").focus();
		});
		try {
			document.cookie = "SessionKey_" + document.getElementById("hdnSession").value + "=" + Math.random().toString(); //vk 06.23
		} catch (ee) { }

    } catch (e) {
        setTimeout("body_load();",100); //vk 07.10
    }
}

function GetHid(sWhat,sDefault) //vk 02.08
{
    if (document.getElementById(sWhat))
        return document.getElementById(sWhat).value;
	else
		return sDefault;
}

function afunction() //vk 03.11
{
    //don't delete this function
}