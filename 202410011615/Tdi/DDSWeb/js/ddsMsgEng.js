//-----------------------------------//
// Module of Messages                //
// English                           //
//-----------------------------------//
var gDir = "ltr";
var gYes = "OK";
var gNo = "Cancel";
var gClose = "Close";
//vk 07.13
var g1 = "Cancel"
var g2 = "Rename"
var g3 = "Replace"

function msg(iSeq) {
    var sRet = "";

    switch (iSeq) {
        case 1:
            sRet = "Loading, please wait...";
            break;
        //	case 2 :
        //		sRet = "Uppercase only";
        //		break;					
        case 3:
            sRet = "COMTEC";
            break;
        case 4:
            sRet = "Please select a line to process";
            break;
        case 5:
            sRet = "Please select an action to execute";
            break;
        case 6:
            sRet = "Please confirm:";
            break;
        case 7:
            sRet = "The option is not active for the current user";
            break;
        case 8:
            sRet = "You have been disconnected due to timeout. Close the application and re-connect";
            break;
        case 9:
            sRet = "095 " + "Please wait while initializing printers...";
            break;
        case 10:
            sRet = "100 " + "Please wait while loading printers..." + "*Init...";
            break;
        case 11:
            sRet = "110 " + "Please wait while getting station name..." + "*Config...";
            break;
        case 12:
            sRet = "Data that was not saved will be lost.\nDo you want to exit the system?";
            break;
        case 13:
            sRet = "Problem in print server activation";
            break;
        case 14:
            sRet = "Speak to support agents.";
            break;
        case 15:
            sRet = "Pages sent: ";
            break;
        case 16:
            sRet = " of ";
            break;
        case 17:
            sRet = "091 " + "Please wait while Java is detecting...";
            break;
        case 18:
            sRet = "092 " + "Please wait while Java is installing ...";
            break;
        case 19:
            sRet = "The text is too long. The extra text is:";
            break;
        case 20:
            sRet = "Error calling ActiveX:";
            break;
        case 21:
            sRet = "Empty parameter for ActiveX received";
            break;
        case 22:
            sRet = "Please wait...";
            break;
        case 23:
            sRet = "Info details";
            break;
        case 24:
            sRet = "Countdown before disconnect";
            break;
        case 25:
            sRet = "There was a problem with the system. Please close your browser and log in again!";
            break;
        case 26:
            sRet = "Error";
        case 27:
            sRet = "Please select an activity and try again";
            break;
        case 28:
            sRet = "Cities and Streets";
            break;
        case 29: //ntg 10.07.24 vladi's change regarding city-street screen
            sRet = "Select";
            break;
    }
    return sRet;
}  // msg

function msg_Empty(iSeq) {
    var sRet = "";

    switch (iSeq) {
        case 1:
            sRet = "Mandatory field";
            break;
        case 2:
            sRet = "The value of this field must differ from zero";
            break;
        case 3:
            sRet = "The value of this field must contain english letters or digits";
            break;
        case 4:
            sRet = "The value of this field must not be empty";
            break;

    }
    return sRet;
}  // msg_Empty

function msg_Num(iSeq) {
    var sRet = "";

    switch (iSeq) {
        case 1:
            sRet = "Use of decimals or sign not correct or too many digits entered";
            break;
        case 2:
            sRet = "Not numeric value has arrived in field";
            break;
    }
    return sRet;
}  // msg_Num

function msg_Date(iSeq) {
    var sRet = "";

    switch (iSeq) {
        case 1:
            sRet = "Invalid Date";
            break;
    }
    return sRet;
}  // msg_Date

function msg_Time(iSeq) {
    var sRet = "";

    switch (iSeq) {
        case 1:
            sRet = "Invalid Time";
            break;
    }
    return sRet;
}  // msg_Time

function msg_Text(iSeq) {
    var sRet = "";

    switch (iSeq) {
        case 1:
            sRet = "Enter Upper Case Only";
            break;
        case 2: //vk 01.07
            sRet = "Enter Upper Case Letters And Digits Only";
            break;
    }
    return sRet;
}  // msg_Text

function msg_Address(iSeq) //vk 04.04
{
    var sRet = "";

    switch (iSeq) {
        case 1:
            sRet = "You must choose a city";
            break;
        case 2:
            sRet = "You must choose a street";
            break;
    }
    return sRet;
}

function msg_Archive(sCode) //vk 07.13
{
    var sRet = "";

    switch (sCode) {
        case "OK":
            sRet = "File successfully uploaded";
            break;
        case "File exists":
            sRet = "File already exists";
            break;
        case "Illegal symbol":
            sRet = "This file name contains illegal symbols";
            break;
        case "Replace Question":
            sRet = "Would you like to replace the file ?";
            break;
    }
    return sRet;
}

function msg_CG(sCode) //vk 07.13
{
    var sRet = "";

    switch (sCode) {
        case "Terminal":
            sRet = "Value not received for field: Terminal number";
            break;
    }
    return sRet;
}

//function msg_Tray(sCode) //vk 12.13
//{
//	var sRet = "";

//	switch (sCode)
//	{
//	case "choose":
//		sRet = "Please choose a printer and a tray";
//		break;
//	case "no trays":
//		sRet = "Trays not found";
//		break;
//	}
//	return sRet;
//}

function msg_Consist(sCode) //vk 12.14
{
    var sRet = "";

    switch (sCode) {
        case "rc":
            sRet = "Printing error. The error code is ";
            break;
        case "no object":
            sRet = "Printing error. The client is not available";
            break;
    }
    return sRet;
}

function msg_Sql(sCode) //vk 11.15
{
    var sRet = "";

    switch (sCode) {
        case "wait":
            sRet = "Copying data...";
            break;
        case "copied":
            sRet = "Copying data, till now records copied";
            break;
        case "illegal":
            sRet = "Illegal return from SQL";
            break;
    }
    return sRet;
}

function GetCopyright() //vk 03.03
{

    var s;
    s = "<a href='" + GetHid("Hpath_Comtec", "") + "' target='_blank' tabindex='-1' ";
    s += "style='text-decoration:none;color:#000000;' ";
    s += "onMouseOver='this.style.textDecoration=\"underline\";this.style.color=\"blue\";' ";
    s += "onMouseOut='this.style.textDecoration=\"none\";this.style.color=\"#000000\";'>";
    s += "All Rights Reserved <FONT COLOR=\"#FF4500\">&copy;</FONT>";
    //s += " " + d.getFullYear();
    s += " COMTEC Ltd.</a>";
    return s;
}
function GetCopyrightNetto() //vk 07.05
{
    var s;
    s = "<p tabindex='-1' ";
    s += "style='text-decoration:none;color:#000000;'>";
    s += "All Rights Reserved <FONT COLOR=\"#FF4500\">&copy;</FONT> COMTEC Ltd.</p>";
    return s;
}

function GetCaption(sTopic) //vk 10.04
{
    var sRet = "";
    switch (sTopic) {
        case "print": //vk 01.04
            sRet = "Print";
            break;
        case "help": //vk 07.04
            sRet = "Help";
            break;
    }
    return sRet;
}

//function KbdButtons() //vk 02.05
//{
//	KbdButtons_once("OK","Del");
//}
