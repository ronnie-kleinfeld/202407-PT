//-----------------------------------//
// Module of Messages                //
// Edit only in notepad in UTF-8     //
// Hebrew                            //
//-----------------------------------//
var gDir = "rtl";
var gYes = "אשר";
var gNo = "בטל";
var gClose = "סגור";
//vk 07.13
var g1 = "ביטול"
var g2 = "שינוי שם"
var g3 = "החלפה"

function msg(iSeq) {
    var sRet = "";

    switch (iSeq) {
        case 1:
            sRet = "נא להמתין. הדף בטעינה...";
            break;
        //	case 2 :
        //		sRet = "אותיות רישיות בלבד";
        //		break;
        case 3:
            sRet = "קומטק";
            break;
        case 4:
            sRet = "יש לבחור פוליסה/תוספת לטיפול";
            break;
        case 5:
            sRet = "נא לבחור פעולה לביצוע";
            break;
        case 6:
            sRet = "נא לאשר ביצוע פעולה:";
            break;
        case 7:
            sRet = "האפשרות אינה פעילה עבור המשתמש הנוכחי";
            break;
        case 8:
            sRet = "נותקת עקב אי פעילות. אנא סגור/י את המערכת והתחבר/י מחדש";
            break;
        case 9:
            sRet = "095 " + "נא להמתין לאתחול מדפסות...";
            break;
        case 10:
            sRet = "100 " + "נא להמתין לטעינת המדפסות..." + " *Init...";
            break;
        case 11:
            sRet = "110 " + "נא להמתין לקבלת זיהוי התחנה..." + " *Config...";
            break;
        case 12:
            sRet = "נתונים שלא שמרת יאבדו\nהאם ברצונך לצאת מהמערכת?"
            break;
        case 13:
            sRet = "בעיה בהפעלת שרת ההדפסות";
            break;
        case 14:
            sRet = "פנה לתקשוב סוכנים.";
            break;
        case 15:
            sRet = "נשלחו דפים: ";
            break;
        case 16:
            sRet = " מתוך ";
            break;
        case 17:
            sRet = "091 " + "נא להמתין לאיתור JAVA במחשב...";
            break;
        case 18:
            sRet = "092 " + "נא להמתין להתקנת JAVA במחשב...";
            break;
        case 19:
            sRet = "המלל ארוך מדי. המלל העודף הוא";
            break;
        case 20:
            sRet = "שגיאה בפנייה ל-ActiveX:";
            break;
        case 21:
            sRet = "התקבל פרמטר ריק ל-ActiveX";
            break;
        case 22:
            sRet = "נא להמתין";
            break;
        case 23:
            sRet = "פירוט מידע";
            break;
        case 24:
            sRet = "ספירה לאחור לפני ניתוק";
            break;
        case 25:
            sRet = "אירעה תקלה במערכת. יש לסגור את הדפדפן ולהיכנס מחדש!";
            break;
        case 26:
            sRet = "שגיאה";
            break;
        case 27:
            sRet = "יש לבחור פעולה לביצוע, נסה שנית";
            break;
        case 28:
            sRet = "ערים ורחובות";
            break;
        case 29: //ntg 10.07.24 vladi's change regarding city-street screen
            sRet = "יש לבחור";
            break;
    }
    return sRet;
}  // msg

function msg_Empty(iSeq) {
    var sRet = "";

    switch (iSeq) {
        case 1:
            sRet = "חובה למלא שדה זה באופן תקין";
            break;
        case 2:
            sRet = "ערך שדה זה חייב להיות שונה מאפס";
            break;
        case 3:
            sRet = "שדה זה חייב להכיל אותיות אנגליות או ספרות";
            break;
        case 4:
            sRet = "יש לבחור ערך בשדה זה";
            break;
    }
    return sRet;
}  // msg_Empty

function msg_Num(iSeq) {
    var sRet = "";

    switch (iSeq) {
        case 1:
            sRet = "שימוש שגוי בסימן או בנקודה עשרונית, או יותר מדי ספרות הוקלדו";
            break;
        case 2:
            sRet = "התקבל ערך לא נומרי בשדה";
            break;
    }
    return sRet;
}  // msg_Num

function msg_Date(iSeq) {
    var sRet = "";

    switch (iSeq) {
        case 1:
            sRet = "תאריך לא חוקי";
            break;
    }
    return sRet;
}  // msg_Date

function msg_Time(iSeq) {
    var sRet = "";

    switch (iSeq) {
        case 1:
            sRet = "זמן לא תקין";
            break;
    }
    return sRet;
}  // msg_Time

function msg_Text(iSeq) {
    var sRet = "";

    switch (iSeq) {
        case 1:
            sRet = "אין להקליד אותיות קטנות";
            break;
        case 2: //vk 01.07
            sRet = "נא להקליד אותיות גדולות וספרות בלבד";
            break;
    }
    return sRet;
}  // msg_Text

function msg_Address(iSeq) //vk 04.04
{
    var sRet = "";

    switch (iSeq) {
        case 1:
            sRet = "חובה לבחור עיר";
            break;
        case 2:
            sRet = "חובה לבחור רחוב";
            break;
    }
    return sRet;
}

function msg_Archive(sCode) //vk 07.13
{
    var sRet = "";

    switch (sCode) {
        case "OK":
            sRet = "הקובץ הועבר לארכוב";
            break;
        case "File exists":
            sRet = "קובץ עם שם זהה כבר קיים";
            break;
        case "Illegal symbol":
            sRet = "ישנם תווים אסורים בשם הקובץ";
            break;
        case "Replace Question":
            sRet = "האם ברצונך להחליף את הקובץ ?";
            break;
    }
    return sRet;
}

function msg_CG(sCode) //vk 07.13
{
    var sRet = "";

    switch (sCode) {
        case "Terminal":
            sRet = "לא התקבל ערך בשדה: מספר מסוף";
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
//		sRet = "נא לבחור מדפסת ומגירה";
//		break;
//	case "no trays":
//		sRet = "לא נמצאו מגירות";
//		break;
//	}
//	return sRet;
//}

function msg_Consist(sCode) //vk 12.14
{
    var sRet = "";

    switch (sCode) {
        case "rc":
            sRet = "קרתה שגיאה בעת הדפסת הקובץ. קוד השגיאה: ";
            break;
        case "no object":
            sRet = "קרתה שגיאה בעת הדפסת הקובץ: הקליינט אינו זמין";
            break;
    }
    return sRet;
}

function msg_Sql(sCode) //vk 11.15
{
    var sRet = "";

    switch (sCode) {
        case "wait":
            sRet = "מתבצעת העתקת נתונים...";
            break;
        case "copied":
            sRet = "מתבצעת העתקת נתונים, עד כה הועתקו רשומות";
            break;
        case "illegal":
            sRet = "תשובה בלתי חוקית מתהליך העתקת טבלאות";
            break;
    }
    return sRet;
}

function GetCopyright() //vk 03.03
{
    //var d = new Date();
    var s;
    s = "<a href='" + GetHid("Hpath_Comtec", "") + "' target='_blank' tabindex='-1' ";
    s += "style='text-decoration:none;color:#000000;' ";
    s += "onMouseOver='this.style.textDecoration=\"underline\";this.style.color=\"blue\";' ";
    s += "onMouseOut='this.style.textDecoration=\"none\";this.style.color=\"#000000\";'>";
    s += "קומטק בע&quot;מ <FONT COLOR=\"#FF4500\">&copy;</FONT>";
    //s += " " + d.getFullYear();
    s += " כל הזכויות שמורות</a>";
    return s;
}
function GetCopyrightNetto() //vk 07.05
{
    var s;
    s = "<p tabindex='-1' ";
    s += "style='text-decoration:none;color:#000000;'>";
    s += "קומטק בע&quot;מ <FONT COLOR=\"#FF4500\">&copy;</FONT> כל הזכויות שמורות</p>";
    return s;
}

function GetCaption(sTopic) //vk 10.04
{
    var sRet = "";
    switch (sTopic) {
        case "print": //vk 01.04
            sRet = "הדפס";
            break;
        case "help": //vk 07.04
            sRet = "עזרה";
            break;
    }
    return sRet;
}

//function KbdButtons() //vk 02.05
//{
//	KbdButtons_once("אישור","מחיקה");
//}
