//#define AVOIDBACK
#define PDFPROJECT
using System;
using System.Collections;
using System.Web;
using Comtec.TIS;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
//using System.Threading.Tasks;

namespace DDSWeb
{
    /// <summary>
    /// Summary description for screen.
    /// </summary>
    public class screen : System.Web.UI.Page
    {
        const string ERROR_FILE_PDF = "Error.pdf";

        private AS400Page.BuildPage oAS400Page;
#if PDFPROJECT
        private SpoolPage.BuildPage oSpoolPage;
#endif
        private string sXMLFromAS;
        private string ModalScreen = "";
        private string sTail; //vk 09.06
        private string sHTMLPage;
        private string sXMLToAS; //vk 01.05

        //vk 01.05
        private string sTable = "";
        private string sCommand = ""; //vk 03.06
        private string sDb = "";
        private string sFlr = "";
        private bool bModal;

        Hashtable cPages;
        private string sXmlFil, sXmlRec;
        private string sWarning = "";
        private string sBuffer = "";
        private string sPrinter = "";
        private bool bBreakFor;
        private bool bDifferentPage;
        private bool bIsModalScreen;
        private string xmlLocalCounter = "";
        private string PageHeaderHtml = "";

        private Reader dp;
        PocketKnife.Memory mem; //vk 09.06
        PocketKnife.ShowTable sht; //vk 06.16

        private string copyPdfFilePath = "";
        //vk 11.15
        const string DONT_ANSWER = "don't need to answer to AS/400";

        private bool Build_HTMLPage(string sXMLToAS, string sXmlOK, bool bF5, bool bFirstScreen)
        {

            SaveFile("saved.xml", sXMLFromAS.Replace("Windows-1255", "utf-8"));

            SaveFile($"tmp-{Session.SessionID}-InputFile.xml", sXMLFromAS.Replace("Windows-1255", "utf-8"), "ShowXml");

            bool bStopSession = false;
            long nMaxLen = Int64.Parse(dp.getProperty("MaxLen"));
            int nLineLength = 0;

            if (dp.getProperty("DebugXml") == "true")
            {
                string Test0FilePath = (string)Application["PATH"] + "\\test0.xml";
                if (File.Exists(Test0FilePath))
                {
                    PocketKnife.Fq f = new PocketKnife.Fq((string)Application["PATH"] + "\\test0.xml", PocketKnife.Fq.FileType.Read);
                    string s = f.Receive();
                    f.Dispose();
                    f = null;
                    //vk 01.10
                    int i = s.IndexOf("</screen>");
                    sTail = s.Substring(i + "</screen>".Length);
                    if (sTail == "\r\n") sTail = "";
                    if (sTail == "") //|| sTable == "30")
                        Session["PageXML"] = s.Substring(0, i + "</screen>".Length);
                    //else
                    //    sXMLFromAS = s.Substring(0, i + "</screen>".Length);
                    //vk 12.09
                    if (sTable == "30")
                    {
                        sTable = "";
                    }
                    else
                    {
                        if (sTail != "")
                            Session["Got_Xml"] = s.Substring(0, i + "</screen>".Length);
                        else
                            Session["Got_Xml"] = s;
                        Session["PreviousXML"] = Session["Got_Xml"]; //vk 01.23
                        //Session["Got_Xml"] = sXMLFromAS;
                        Session["PreviousXML"] = Session["Got_Xml"]; //vk 01.23
                        Session["Got_Tail"] = sTail;
                        //sXMLFromAS = (string)Session["Got_Xml"];
                    }
                    if (sTail != "")
                    { //vk 09.15
                        sTable = "30";
                        sFlr = "0";
                        GetBuffersForMemory();
                    }
                }

                string TestFilePath = (string)Application["PATH"] + "\\test.htm";
                if (File.Exists(TestFilePath))
                {
                    PocketKnife.Fq f = new PocketKnife.Fq(TestFilePath, PocketKnife.Fq.FileType.Read);
                    sHTMLPage = f.Receive();
                    f.Dispose();
                    f = null;
                    return true;
                }
            }
            try
            {

                sCommand = "";
                GreatConnect gc = (GreatConnect)Session["gc"];
                string As400WarningStr = string.Join("<br>", (List<string>)Session["As400WarningList"]);
                Session["As400WarningList"] = new List<string>();
                PageHeaderHtml = Session["PageHeaderHtml"].ToString();
                bDifferentPage = (string)Session["XmlFil"] != sXmlFil || (string)Session["XmlRec"] != sXmlRec;
                bIsModalScreen = IsModalScreen(sXMLFromAS);
                bool bIsWinRScreen = IsWinRScreen(sXMLFromAS);

                if ((bIsModalScreen && Session["PreviousXML"] != null) || (bIsModalScreen && bIsWinRScreen)) // בדיקה אם חלון מודלי
                //if (bIsModalScreen && (Session["PreviousXML"] != null)) //ntg 07.06.23 now works in the environments
                {
                    ModalScreen = sXMLFromAS;
                    if (!bIsWinRScreen)
                    {
                        sXMLFromAS = Session["PreviousXML"].ToString();
                    }
                    Session["XmlFil"] = sXmlFil;
                    Session["XmlRec"] = sXmlRec;
                }

                sHTMLPage = oAS400Page.GetPage(sXMLFromAS, ScreenCaption(), (string)Session["LastEntry"], sWarning, Session["ScreenNum"].ToString(), ref bStopSession, ref nMaxLen,
                                                ref sTable, ref sCommand, ref copyPdfFilePath, "", 1, bFirstScreen, As400WarningStr, ref PageHeaderHtml, bDifferentPage, ModalScreen, (string)Session["QpxlNew"]);

                Session["PageHeaderHtml"] = PageHeaderHtml;

                oAS400Page.FilRec(ref sXmlFil, ref sXmlRec);

                if (sXmlFil != "" && !bIsModalScreen)
                {
                    Session["XmlFil"] = sXmlFil;
                    Session["XmlRec"] = sXmlRec;
                }
                SaveFile("saved1.htm", sHTMLPage);

                if (!bIsModalScreen) // לשמור את המסך הקודם רק במקרה שזה לא חלון מודלי
                    Session["PreviousXML"] = sXMLFromAS;

                if (sHTMLPage == "")
                    gc.LogXML("HTML", "sTable=" + sTable, "Log_XML", Session["ScreenNum"].ToString(), Session); //vk 06.10
                else
                    gc.LogXML("HTML", sHTMLPage, "Log_HTML", Session["ScreenNum"].ToString(), Session); //vk 03.08
                if (sTable == "printserver"
                    || sTable == "spool" || sTable == "spoolpdf" || sTable == "spoolpdfnew" || sTable == "spool+"
                    || sTable == "image" || sTable == "imagetoken" || sTable == "imagepost" || sTable == "imageurl" || sTable == "imageurltoken" || sTable == "imageurlpost"
                    || sTable == "scan" || sTable == "scanpost" || sTable == "scanurl" || sTable == "scanurlpost"
                    || sTable == "image_M")
                {
                    int nPages = 0;

                    switch (sTable)
                    {
                        case "spool+":
                        case "spoolpdfnew": //vk 03.09
                            if (copyPdfFilePath.Trim().Length == 0) //vk 11.15
                            {
                                GetBuffersForSpool(
#if PDFPROJECT
                                    oSpoolPage,
#else
                                    oAS400Page,
#endif
                                    sTable, "", "", "", "", "", true, bFirstScreen, ref nLineLength);
                                cPages = (Hashtable)Session["PageHTMLAdd"];
                                nPages = (int)Session["PageHTMLAdd_Count"]; //vk 08.07
                            }
                            else
                            { //vk 11.15
                                cPages = new Hashtable();
                                nPages = 1;
                            }
                            if (sTable == "spoolpdfnew")
                            { //vk 04.09, 07.09
                                string s, rc;
                                PocketKnife.Info o = new PocketKnife.Info();
                                PocketKnife.Fq ff = new PocketKnife.Fq(
                                    (string)Application["PATH"] + "\\tmp-" + Session.SessionID + ".htm",
                                    PocketKnife.Fq.FileType.Write);
                                if (copyPdfFilePath.Trim().Length > 0)
                                {
                                    s = ""; //vk 11.15
                                }
                                else
                                {
                                    try
                                    {
                                        s = cPages["0001"].ToString();
                                    }
                                    catch
                                    {
                                        s = "";
                                    }
                                }
                                if (s != "" || copyPdfFilePath.Trim().Length > 0) //vk 11.15
                                {
                                    ff.Send(s);
                                    ff.Dispose(); //vk 11.11
                                    o.DelFiles((string)Session["Path"], "tmp-" + Session.SessionID + "*.pdf", dp, "PDF_");
                                    //vk 06.09, 07.09
                                    string sPdf = "tmp-" + Session.SessionID +
                                        "-" + Session["ScreenNum"].ToString() +
                                        "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";

                                    Session["Pdf"] = dp.getProperty("PdfPrefix2") + sPdf; //vk 01.15
                                    string pset = gc.Param(sXMLFromAS, "pset", "");

                                    //vk 02.15
                                    string sUrl = "tmp-" + Session.SessionID + ".htm";
                                    if (dp.getProperty("PdfPrefix1") == "")
                                        sUrl = Session["Url"] + sUrl;
                                    else
                                        sUrl = dp.getProperty("PdfPrefix1") + sUrl;

                                    gc.LogXML("PDF HTML", sUrl, "Log_Conn", Session["ScreenNum"].ToString(), Session); //vk 02.10
                                    //-------------- 
                                    //Copy file pdf.
                                    //--------------
                                    if (copyPdfFilePath.Trim().Length > 0)
                                    {
                                        copyPdfFile(sPdf, copyPdfFilePath.Trim());
                                    }
                                    else
                                    {
                                        rc = o.html2pdf(sUrl,
                                            (string)Application["PATH"] + "\\" + sPdf,
                                            o.DecryptPassword(dp.getProperty("PdfKey")),
                                            PropertyByPset("PdfPaper", pset),
                                            PropertyByPset("PdfOrientation", pset),
                                            bool.Parse(dp.getProperty("PdfSelectable")),
                                            PropertyByPset("PdfWidth", pset),
                                            PropertyByPset("PdfLeft", pset),
                                            PropertyByPset("PdfRight", pset),
                                            PropertyByPset("PdfTop", pset),
                                            PropertyByPset("PdfBottom", pset),
                                            dp.getProperty("PdfProductVersion")); //DecryptPassword vk 12.23

                                        if (rc == "OK")
                                        {
                                            gc.LogXML("PDF", sPdf, "Log_XML", Session["ScreenNum"].ToString(), Session); //vk 07.09
                                        }
                                        else
                                        {
                                            //Task t = new Task(() =>
                                            //{
                                            LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                                            lw.SetLocalCounter(Session);
                                            lw.WriteLine("ERROR", (string)Session["Station"], "PDF: " + rc,
                                                Session, (string)Session["User"], gc.Synchr(), "", "", "");
                                            lw.Dispose();
                                            lw = null;
                                            //});
                                            //t.Start();
                                        }
                                    }


                                    //vk 02.13
                                    if (sPrinter != "")
                                    {
                                        o.DelFiles((string)Session["Path"], "tmp-" + Session.SessionID + "*.tif", dp, "PDF_");
                                        FlexPage.FlexReport fr = new FlexPage.FlexReport((string)Application["PATH"],
                                            gc.Param(sXMLFromAS, "pset", ""),
                                            gc.Param(sXMLFromAS, "cli", "00"),
                                            gc.Param(sXMLFromAS, "stp", ""),
                                            sPdf.Replace(".pdf", ""));
                                        fr.MakeRptOnce("", "", sBuffer, nLineLength);
                                        fr = null;
                                    }
                                }
                                else
                                {
                                    if (ff != null) ff.Dispose();
                                }
                                ff = null;
                                o = null;
                            }
                            break;
                        case "spool":
                        case "spoolpdf": //vk 08.07
                            GetBuffersForSpool(
#if PDFPROJECT
                                oSpoolPage,
#else
                                oAS400Page,
#endif
                                sTable, "", "", "", "", "", true, bFirstScreen, ref nLineLength);
                            nPages = 1;
                            break;
                        case "image":
                        case "imagetoken": //vk 05.22
                        case "imagepost": //vk 03.09
                        case "imageurl": //vk 10.09
                        case "imageurltoken": //vk 05.22
                        case "imageurlpost": //vk 12.09
                        case "scan":
                        case "scanpost":
                        case "scanurl":
                        case "scanurlpost":
                        case "image_M": //vk 06.12
                            string sAnswer =
                                "<?xml version=\"1.0\" encoding=\"Windows-1255\"?><screen>" +
                                "<s fil=\"\" rec=\"\" fld=\"0000000000\" find=\"0000\" fcmd=\"00\"/>" +
                                "<fields></fields></screen>";
                            //========================================
                            string e;
                            e = gc.SendUsual(sAnswer, "", Session, false);
                            if (e != "")
                            {
                                print_err(new Exception(e), "", "", false);
                                return false;
                            }
                            //========================================
                            if (!Receive_XMLFromAs(true))
                                return false;
                            else
                                oAS400Page.UpdateLocalCounter(xmlLocalCounter);
                            break;
                    }

                    string sImageMethod =
                        sTable == "image" ? "II" :
                        (sTable == "imagetoken" ? "IJ" :
                        (sTable == "imagepost" ? "IF" :
                        (sTable == "imageurl" ? "IU" :
                        (sTable == "imageurltoken" ? "IV" :
                        (sTable == "imageurlpost" ? "IP" :
                        (sTable == "scan" ? "SI" :
                        (sTable == "image_M" ? "M" :
                        "")))))));
                    PageHeaderHtml = Session["PageHeaderHtml"].ToString();
                    bDifferentPage = (string)Session["XmlFil"] != sXmlFil || (string)Session["XmlRec"] != sXmlRec;

                    sHTMLPage = oAS400Page.GetPage(sXMLFromAS, ScreenCaption(), (string)Session["LastEntry"], sWarning, Session["ScreenNum"].ToString(), ref bStopSession,
                        ref nMaxLen, ref sTable, ref sCommand, ref copyPdfFilePath, (string)Session["Pdf"], 2, bFirstScreen, As400WarningStr, ref PageHeaderHtml, bDifferentPage, "", (string)Session["QpxlNew"]);

                    Session["PageHeaderHtml"] = PageHeaderHtml;

                    oAS400Page.FilRec(ref sXmlFil, ref sXmlRec);
                    if (sXmlFil != "")
                    {
                        Session["XmlFil"] = sXmlFil;
                        Session["XmlRec"] = sXmlRec;
                    }
                    SaveFile("saved1.htm", sHTMLPage);
                    gc.LogXML("HTML", sHTMLPage, "Log_HTML", Session["ScreenNum"].ToString(), Session); //vk 03.08

                    Session["Pdf"] = "";
                }
                if (dp.getProperty("DebugXml") == "true")
                {
                    string TestFilePath = (string)Application["PATH"] + "\\test.htm";
                    if (File.Exists(TestFilePath))
                    {
                        PocketKnife.Fq f = new PocketKnife.Fq((string)Application["PATH"] + "\\test.htm", PocketKnife.Fq.FileType.Read);
                        sHTMLPage = f.Receive();
                        f.Dispose();
                        f = null;
                    }
                }
                if (bStopSession) Session["StopSession"] = true;
                bModal = false;
                Session["Modal"] = false;
                if (sTable == ""
                    || sTable == "spool" || sTable == "spoolpdf" || sTable == "spoolpdfnew" || sTable == "spool+"
                    || sTable == "image" || sTable == "imagetoken" || sTable == "imagepost" || sTable == "imageurl" || sTable == "imageurltoken" || sTable == "imageurlpost"
                    || sTable == "scan" || sTable == "scanpost" || sTable == "scanurl" || sTable == "scanurlpost"
                    || sTable == "image_M")
                {
                    Session["PageXML"] = sXMLFromAS;
                    Session["PageHTML"] = sHTMLPage;
                }
            }
            catch (Exception e1)
            {
                print_err(e1, sXMLFromAS, (string)Session["PageXML"], false);
                return false;
            }
            return true;
        }

        private bool IsModalScreen(string sXMLFromAS)
        {
            XDocument InputXMl = XDocument.Parse(sXMLFromAS);
            XElement sElement = InputXMl.Descendants("s").First();
            if (sElement.Attribute("win") != null)
            {
                //return (sElement.Attribute("win").Value.Trim() == "S");// || (sElement.Attribute("win").Value.Trim() == "R");
                //return (sElement.Attribute("win").Value.Trim() == "S" && sElement.Attribute("fil").Value.Trim() != "HFFLNAVFM" && sElement.Attribute("rec").Value.Trim() != "ROPTION") || (sElement.Attribute("win").Value.Trim() == "R");

                bool isRoptionRec = sElement.Attribute("fil").Value.Trim() == "HFFLNAVFM" && sElement.Attribute("rec").Value.Trim() == "ROPTION";
                return (sElement.Attribute("win").Value.Trim() == "S" || sElement.Attribute("win").Value.Trim() == "R") && !isRoptionRec;
            }
            else
                return false;
        }

        private bool IsWinRScreen(string sXMLFromAS)
        {
            XDocument InputXMl = XDocument.Parse(sXMLFromAS);
            XElement sElement = InputXMl.Descendants("s").First();
            bool isRoptionRec = sElement.Attribute("fil").Value.Trim() == "HFFLNAVFM" && sElement.Attribute("rec").Value.Trim() == "ROPTION";

            if (sElement.Attribute("win") != null)
                //return sElement.Attribute("win").Value.Trim() == "R" && sElement.Attribute("fil").Value.Trim() != "HFFLNAVFM" && sElement.Attribute("rec").Value.Trim() != "ROPTION";
                return sElement.Attribute("win").Value.Trim() == "R" && !isRoptionRec;
            else
                return false;
        }

        private bool IsGridModal(XElement sElement)
        {
            return false;
            //return (sElement.Attribute("win") != null && sElement.Attribute("fgr") != null &&  sElement.Attribute("win").Value.Trim() == "S" && sElement.Attribute("fgr").Value.Trim() == "G");
        }

        private bool IsAS400WarningMessage(string sXMLFromAS)
        {
            XDocument InputXMl = XDocument.Parse(sXMLFromAS);

            XElement sElement = InputXMl.Descendants("s").First();
            //vk 08.22
            if (sElement.Attribute("fil") == null)
                sXmlFil = "";
            else
                sXmlFil = sElement.Attribute("fil").Value;
            if (sElement.Attribute("rec") == null)
                sXmlRec = "";
            else
                sXmlRec = sElement.Attribute("rec").Value;

            if (sXmlFil.Trim() == "MSGSCRFM" &&
                sXmlRec.Trim() == "MSGSCR01" &&
                sElement.Attribute("win").Value.Trim() == "S")
            {
                foreach (XElement FieldElem in InputXMl.Descendants("f"))
                {
                    //if (FieldElem.Attribute("num").Value.Trim() == "HODAA")
                    //{
                    //    var list = (List<string>)Session["As400WarningList"];
                    //    list.Add(FieldElem.Attribute("val").Value.Trim());
                    //    Session["As400WarningList"] = list;
                    //}
                    if (FieldElem.Attribute("num").Value.Trim() == "HODAA") //ntg 08.06.23 err msgs were backwards
                    {
                        var list = (List<string>)Session["As400WarningList"];
                        Comtec.Tis.ConvCom oConv = new Comtec.Tis.ConvCom();
                        string sLang = "";
                        list.Add(oConv.RevHeb(FieldElem.Attribute("val").Value.Trim(), ref sLang));
                        Session["As400WarningList"] = list;
                        oConv = null;
                    }

                }
                return true;

            }
            return false;
        }

        private void copyPdfFile(string sFileLocal, string sFileRemote)
        {
            PocketKnife.Info o = new PocketKnife.Info();
            AS400.WinNet a;
            GreatConnect gc = (GreatConnect)Session["gc"];
            string sImpersResult = "";
            string sMappingResult = "";
            string sFolder = "";
            bool bMappingError = false;
            bool bLocal = sFileRemote.Substring(0, 2).ToLower() == "c:"; //vk 05.22
            bool bNeedImpers = dp.getProperty("GetPDF_Source") != "NoImpers" && !bLocal; //vk 02.17, 05.22
            bool bPC = dp.getProperty("GetPDF_Source") == "PC"; //vk 02.17
            try
            {
                sFileLocal = (string)Application["PATH"] + "\\" + sFileLocal;
                if (sFileRemote.Substring(1, 1) == ":" && !bLocal) //vk 05.22
                {
                    a = new AS400.WinNet();
                    sMappingResult = a.MappingToPath(sFileRemote.Substring(0, 1).ToString());
                    if (sMappingResult.Substring(0, 7) == "ERROR: ")
                    {
                        bMappingError = true;
                        File.Copy((string)Application["PATH"] + "\\" + ERROR_FILE_PDF, sFileLocal);
                        gc.LogXML("PDF ERROR", sMappingResult.Substring(7), "Log_Conn", Session["ScreenNum"].ToString(), Session);
                    }
                    else
                    {
                        sFileRemote = sMappingResult + sFileRemote.Substring(2);
                    }
                }
                if (!bMappingError)
                {
                    if (bNeedImpers)
                    {
                        if (bPC)
                        {
                            //vk 02.17
                            if (o.impersonateValidUser(dp, "GetPDF_"))
                                sImpersResult = "OK";
                            else
                                sImpersResult = "Impersonation failed";
                        }
                        else
                        {
                            int i = sFileRemote.LastIndexOf("\\");
                            sFolder = sFileRemote.Substring(0, i);
                            sImpersResult = o.impersonateValidUser_AS400(dp, "GetPDF_", sFolder);
                        }
                        gc.LogXML("PDF IMPERS", sFileRemote, "Log_Conn", Session["ScreenNum"].ToString(), Session); //vk 09.20
                    }
                    else
                    {
                        sImpersResult = "OK"; //vk 02.17
                    }
                    if (sImpersResult == "OK")
                    {
                        File.Copy(sFileRemote, sFileLocal);
                        gc.LogXML("PDF", sFileRemote, "Log_Conn", Session["ScreenNum"].ToString(), Session);
                    }
                    else
                    {
                        File.Copy((string)Application["PATH"] + "\\" + ERROR_FILE_PDF, sFileLocal);
                        gc.LogXML("PDF ERROR", sImpersResult, "Log_Conn", Session["ScreenNum"].ToString(), Session);
                    }
                }
            }
            catch (Exception e1)
            {
                File.Copy((string)Application["PATH"] + "\\" + ERROR_FILE_PDF, sFileLocal);
                gc.LogError("Could not copy the file: " + sFileRemote, Session, e1);
            }
            finally
            {
                if (sImpersResult == "OK" && bNeedImpers)
                {
                    if (bPC)
                        o.undoImpersonation(); //vk 02.17
                    else
                        o.undoImpersonation_AS400(sFolder);
                }
                a = null;
            }
        }

        private Int32 PropertyByPset(string s, string pset) //vk 07.09
        {
            if (pset != "")
            {
                string r = dp.getProperty(pset + "_" + s);
                if (r != "")
                    return Int32.Parse(r);
            }
            return Int32.Parse(dp.getProperty(s));
        }
        private bool Receive_XMLFromAs(bool bSecond)
        {
            if (sXMLToAS == DONT_ANSWER)
            {
                sXMLFromAS = (string)Session["Got_Xml"];
                sTail = (string)Session["Got_Tail"];
                return true; //vk 11.15
            }
            string s = ""; //vk 12.04
            int i;
            if (dp.getProperty("DebugXml") == "true")
            {
                string InputTestPath = (string)Application["PATH"] + "\\" + (bSecond ? "test1.xml" : "test.xml");
                if (File.Exists(InputTestPath))
                {
                    PocketKnife.Fq f = new PocketKnife.Fq(InputTestPath, PocketKnife.Fq.FileType.Read);
                    sXMLFromAS = f.Receive();
                    f.Dispose();
                    f = null;
                    GreatConnect gc = (GreatConnect)Session["gc"];
                    gc.LogXML("XML READ", s, "", Session["ScreenNum"].ToString(), Session); //vk 12.13
                    xmlLocalCounter = Session["LocalCounter"].ToString();
                    return true;
                }
            }
            try
            {
                sXMLFromAS = "";
                string sVer = ""; //vk 12.06
                for (; ; )
                {
                    //vk 03.05
                    //========================================
                    GreatConnect gc = (GreatConnect)Session["gc"];
                    string e;
                    e = gc.ReceiveUsual(ref s, Session);
                    xmlLocalCounter = Session["LocalCounter"].ToString();

                    //Session["SynchrGot"]=gc.Synchr(); //vk 03.08
                    if (gc.TimeOut())
                    {
                        Session["StopSession"] = true; //vk 05.05
                        print_err(new Exception(e), "", "", false);
                        return false;
                    }
                    else if (e != "")
                    {
                        Session["StopSession"] = true; //vk 05.05
                        print_err(new Exception(e), "", "", false);
                        return false;
                    }
                    //========================================
                    //vk 12.04, 01.05
                    if (s.Substring(0, 1) == "+")
                    {
                        sXMLFromAS += s.Substring(1).Trim();
                        //vk 12.06
                        if (sVer == "")
                            sVer = gc.Param(s, "ver", "000");
                        try { i = Int32.Parse(sVer); }
                        catch { i = 0; }
                        if (i >= 2)
                        {
                            e = gc.SendUsual("+", "", Session, false);
                            if (e != "")
                            {
                                print_err(new Exception(e), "", "", false);
                                return false;
                            }
                        }
                    }
                    else
                    {
                        sXMLFromAS += s.Trim();
                        break;
                    }
                }
            }
            catch (Exception e1)
            {
                print_err(e1, "", "", false);
                return false;
            }
            if (sXMLFromAS == "") //null) //"" vk 12.04
            {
                print_err(new Exception("NULL was received from AS400"), "", "", false);
                return false;
            }
            i = sXMLFromAS.IndexOf("</screen>");
            sTail = "";
            if (i >= 0)
            {
                sTail = sXMLFromAS.Substring(i + "</screen>".Length);
                sXMLFromAS = sXMLFromAS.Substring(0, i + "</screen>".Length);
            }
            //vk 03.08
            Session["Got_Xml"] = sXMLFromAS;
            Session["Got_Tail"] = sTail;
            return true;
        }

        private void print_err(Exception e, string sXml, string sXmlBkg, bool bNoSession) //vk 03.04, 10.11
        {
            string sOut = "";
            sOut += "<html><head><title>DSP - ERROR - DSP</title></head><SCRIPT LANGUAGE='JAVASCRIPT' SRC='js/dds.js'></SCRIPT>";
            sOut += "<body>"; //onbeforeunload='CloseJob();'>"; //vk 08.04, 10.04, 03.06
            sOut += "<form method='post' action='screen.aspx' >";
            sOut += "<input name='Hfcmd' id='Hfcmd' type='hidden' value='' >";
            sOut += "<input name='Hx' id='Hx' type='hidden' value='x' >";

            sOut += "<input type=button value=Close onClick='top.opener=top;opener=window;top.close();'>"; //vk 12.05, 06.10
            if (bNoSession || dp.getProperty("FineErrors") == "true") //vk 03.22
            {
                //vk 03.05
                sOut += "<H3>ERROR</H3>";
            }
            else
            {
                sOut += "<H3>";
                for (Exception ee = e; ee != null; ee = ee.InnerException)
                {
                    if (ee.InnerException == null)
                        sOut += ee.Source + "<BR>";
                }
                sOut += e.Message + "</H3>";
                if (sXml != "")
                {
                    sOut += "<BR><textarea style='overflow-y:auto;' cols='80' rows='10'>";
                    sOut += sXml;
                    sOut += "</textarea>";
                }
                //vk 08.06
                if (sXmlBkg != "")
                {
                    sOut += "<BR><textarea style='overflow-y:auto;' cols='80' rows='10'>";
                    sOut += sXmlBkg;
                    sOut += "</textarea>";
                }
                for (Exception ee = e; ee != null; ee = ee.InnerException)
                {
                    sOut += "<BR><textarea style='overflow-y:auto;' cols='80' rows='10'>";
                    sOut += ee.StackTrace;
                    sOut += "</textarea>";
                }
            }
            //vk 02.05 till here
            sOut += "</form></body></html>";
            SaveFile("saved1.htm", sOut); //vk 01.07
            GreatConnect gc = (GreatConnect)Session["gc"];
            gc.LogXML("HTML ERROR", sOut, "Log_HTML", "", Session); //vk 03.08
            //HttpContext.Current.Response.Write(sOut);
            SendHtml(sOut, false); //vk 12.11
                                   //Task t = new Task(() =>
                                   //{
            LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
            lw.SetLocalCounter(Session);
            lw.WriteLine(bNoSession ? "NO SESSION" : "ERROR", (string)Session["Station"], sXml,
                Session, (string)Session["User"], gc.Synchr(), "", "", e);
            lw.Dispose();
            lw = null;
            //});
            //t.Start();
            //System.Web.Security.FormsAuthentication.SignOut(); //vk 11.09
            gc.CloseSession(Session); //vk 12.11
        }

        private void SendOnce() //vk 01.05
        {
            try
            {
                //vk 03.05
                //========================================
                if (sXMLToAS != DONT_ANSWER) //vk 11.15
                {
                    GreatConnect gc = (GreatConnect)Session["gc"];
                    string e = gc.SendUsual(sXMLToAS, oAS400Page.ScreenNumFromHtml(), Session, false);
                    if (e != "")
                        print_err(new Exception(e), "", "", false);
                }
                //========================================
            }
            catch (Exception e1)
            {
                print_err(e1, "", "", false);
            }
            SaveFile("sent.xml", sXMLToAS);
            Session["LastEntry"] = ""; //vk 11.07
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            //vk 07.23
            GreatConnect gc = (GreatConnect)Session["gc"];
            if (HttpContext.Current.Response.ClientDisconnectedToken.IsCancellationRequested) return;
            if ((string)Session["User"] == "")
            {
                gc.LogXML("SECURITY", "No user", "Log_Error", Session["ScreenNum"].ToString(), Session);
                string x = System.Web.Security.FormsAuthentication.LoginUrl;
                Response.Redirect(x.Substring(x.LastIndexOf("/") + 1));
            }
            if ((string)Session["Station"] != Request.UserHostAddress)
            {
                gc.LogXML("SECURITY", "Wrong IP", "Log_Error", Session["ScreenNum"].ToString(), Session);
                System.Web.Security.FormsAuthentication.SignOut();
                string x = System.Web.Security.FormsAuthentication.LoginUrl;
                Response.Redirect(x.Substring(x.LastIndexOf("/") + 1));
            }
            dp = (Reader)Application["DEFPROP"];
            if (dp.getProperty("CheckCookies") != "false")
            {
                if (Request.Cookies["SessionKey_" + Session.SessionID] == null)
                {
                    gc.LogXML("SECURITY", "No cookies", "Log_Error", Session["ScreenNum"].ToString(), Session);
                    System.Web.Security.FormsAuthentication.SignOut();
                    string x = System.Web.Security.FormsAuthentication.LoginUrl;
                    Response.Redirect(x.Substring(x.LastIndexOf("/") + 1));
                }
                else if ((string)Session["SessionKey"] != Request.Cookies["SessionKey_" + Session.SessionID].Value)
                {
                    gc.LogXML("SECURITY", "Wrong cookies", "Log_Error", Session["ScreenNum"].ToString(), Session);
                    System.Web.Security.FormsAuthentication.SignOut();
                    string x = System.Web.Security.FormsAuthentication.LoginUrl;
                    Response.Redirect(x.Substring(x.LastIndexOf("/") + 1));
                }
            }
            bool bBroswerRefresh = false;
            //GreatConnect gc = (GreatConnect)Session["gc"];
            string sAction = Request.QueryString["action"];
            if (sAction == null)
                sAction = "";


            if ((sAction == "" && gc.RandomNumber == "NoRandom") || (sAction != "" && gc.RandomNumber == Request.QueryString["random"])
              || (sAction == "" && (int)Session["ScreenNum"] > 0) || (sAction != "" && gc.RandomNumberPrev == Request.QueryString["random"])) //ntg 17.06.24 vladi change regarding "back" button in browser 
            {
                bBroswerRefresh = true;
                SendHtml((string)Session["PageHTML"], true);
                Session["Refresh"] = "x"; //ntg 01.07.24 vladi change regarding "back" button in browser
                //gc.LogXML("REFRESH", Request.QueryString["random"], "Log_Error", Session["ScreenNum"].ToString(), Session); //ntg 01.07.24 vladi change regarding "back" button in browser
                gc.LogXML("REFRESH", Request.QueryString["random"] != null ? Request.QueryString["random"] : "", "Log_Error", Session["ScreenNum"].ToString(), Session); //ntg 10.07.24 vladi fix regarding "back" button in browser
                return;

                //string sFromSession = gc.Synchr();
                //bF5 = true;
                //LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                //lw.SetLocalCounter(Session);
                //lw.WriteLine("F5", (string)Session["Station"], "Synchr got from browser: " + sFromBrowser, Session, (string)Session["User"], sFromSession, "", "", "Log_Error");
                //lw.Dispose();
                //lw = null;
            }

            Session["Refresh"] = ""; //ntg 01.07.24 vladi change regarding "back" button in browser
            gc.RandomNumberPrev = gc.RandomNumber; //ntg 17.06.24 vladi change regarding "back" button in browser 
            gc.RandomNumber = Request.QueryString["random"] != null ? Request.QueryString["random"].ToString() : "NoRandom";

            //dp = (Reader)Application["DEFPROP"]; commented by vk 06.23
            if (dp.getProperty("ReloadDefprop") == "true")
            { //vk 05.07
                dp = new Reader((string)Application["PATH"] + "\\" + "Defprop.config");
                Application["DEFPROP"] = dp;
                //vk 08.07
                PocketKnife.Fq f = new PocketKnife.Fq((string)Application["PATH"] + "\\Color\\ColorDef.xml", PocketKnife.Fq.FileType.Read);
                Application["DDS_COLOR"] = "<record>" + f.Receive() + "</record>";
                f.Dispose();
                f = null;
            }
            mem = (PocketKnife.Memory)Session["Memory"];
            if ((string)Session["User"] == "")
            {
                print_err(new Exception("no session"), "", "", true);
                return;
            }
            Session["PingNum"] = 0; //vk 04.05
            gc.LogXML("PAGE LOAD", Request.RawUrl, "Log_XML", Session["ScreenNum"].ToString(), Session);
#if !AVOIDBACK
            Response.Expires = -1;
#endif
            sXMLToAS = "";
            string sXmlOK = "";
            oAS400Page = new AS400Page.BuildPage(dp, mem, sht, (string)Application["DDS_COLOR"],
                (string)Application["PATH"],
                (string)Session["User"], (string)Session["Guid"], (string)Session["Job"],
                (string)Session["Url"], (string)Session["Station"], (string)Session["UserAgent"], Session.SessionID);
#if PDFPROJECT
            oSpoolPage = new SpoolPage.BuildPage(dp, mem, sht, (string)Application["DDS_COLOR"],
                (string)Application["PATH"],
                (string)Session["User"], (string)Session["Guid"], (string)Session["Job"],
                (string)Session["Url"], (string)Session["Station"], (string)Session["UserAgent"]);
#endif
            switch (sAction)
            {
                case "":
                    //gc.LogXML("RESOLUTION", (string)Session["ResolutionW"] + " x " + (string)Session["ResolutionH"],
                    //    "Log_Conn", "", Session); //vk 06.10
                    //oAS400Page.SetResol((string)Session["ResolutionW"], (string)Session["ResolutionH"],
                    //    true, (string)Session["Token"] != "", ""); //vk 04.10
                    //gc.LogXML("RESOLUTION", "(first screen) " + oAS400Page.ResolForLog(), "Log_Conn", "", Session); //vk 01.12
                    break;
                case "next":
                    if ((bool)Session["Independent"])
                    { //vk 10.07
                        Session["Independent"] = false;
                        break;
                    }
                    if ((string)Session["FileToDelete"] != "")
                    {
                        try
                        {
                            File.Delete((string)Session["FileToDelete"]);
                        }
                        catch
                        {
                        }
                        Session["FileToDelete"] = "";
                    }
                    if (bBroswerRefresh)
                    {
                        //oAS400Page.SetResol((string)Session["ResolutionW"], (string)Session["ResolutionH"],
                        //    false, (string)Session["Token"] != "", ""); //vk 08.10
                        //gc.LogXML("RESOLUTION", "(F5) " + oAS400Page.ResolForLog(), "Log_XML", Session["ScreenNum"].ToString(), Session); //vk 01.12
                    }
                    else
                    {
                        string sFind = "", sFil = "", sGridPos = ""; //vk 01.09, 05.09, 05.10
                        //string sAcroError = ""; //vk 02.11
                        string sClient = ""; //vk 10.11
                        string ErrorInXML = "";
                        string sQpxlNew = "";
                        sXMLToAS = oAS400Page.GetXML2AS400(Context, ref sXmlOK, ref sFind, ref sFil, ref sGridPos, (string)Session["XmlFil"],
                                                            (string)Session["XmlRec"], ref sClient, ref ErrorInXML, ref sQpxlNew);
                        Session["QpxlNew"] = ((string)Session["XmlFil"]).Trim() + ";" + ((string)Session["XmlRec"]).Trim() + ";" + sQpxlNew; //vk 09.24

                        Session["UserAgent"] = sClient;

                        if (ErrorInXML != "")
                            gc.LogXML("TDI ERROR", ErrorInXML, "Log_Error", "", Session);

                        if (oAS400Page.ErrorInBuffer())
                            sWarning = dp.getProperty("BufferWarning"); //vk 05.12
                        else
                            sWarning = "";

                        if (sClient != "")
                        {
                            gc.LogXML("CLIENT", sClient, "Log_Conn", "", Session); //vk 10.11
                        }

                        if (oAS400Page.DelPdf())
                        { //vk 04.09, 07.09
                            PocketKnife.Info oDelFiles = new PocketKnife.Info();
                            oDelFiles.DelFiles((string)Session["Path"], "tmp-" + Session.SessionID + "*.*", dp, "PDF_");
                        }
                        if ((string)Session["PageXML"] != "" && sXmlOK != "" && !(bool)Session["Modal"])
                        { //vk 06.06
                            string s;
                            s = (string)Session["PageXML"];
                            s = oAS400Page.GetUpdatePageXML(s, sXmlOK);
                            Session["PageXML"] = s;
                        }
                        switch (sGridPos) //vk 01.09
                        {
                            case "remember":
                                Session["Find"] = sFind;
                                Session["Fil"] = sFil;
                                break;
                            case "delete":
                                Session["Find"] = "";
                                Session["Fil"] = "";
                                break;
                        }
                        SendOnce();
                    }
                    //vk 01.08
                    break;
                case "modcancel":
                    if (!bBroswerRefresh)
                    {
                        sXMLToAS = oAS400Page.GetXML2AS400ModCancel((string)Session["PageXMLMOD"]);
                        sXmlOK = sXMLToAS;
                        SendOnce();
                    }
                    sXMLToAS = "";
                    sXmlOK = "";
                    break;
            }
            bBreakFor = false;
            for (; ; ) //vk 01.05
            {
                if (bBroswerRefresh)
                {
                    sXMLFromAS = (string)Session["Got_Xml"];
                    sTail = (string)Session["Got_Tail"];
                }
                else
                {
                    if (!Receive_XMLFromAs(false))
                        return;
                    else
                        oAS400Page.UpdateLocalCounter(xmlLocalCounter);
                }

                if (IsAS400WarningMessage(sXMLFromAS))
                {
                    // אם מדובר על הודעת שגיאה אפליקטיבית אז שומרים אותה ומחזירים תשובה קבועה למערכת הירוקה
                    string sAnswer =
                                "<?xml version=\"1.0\" encoding=\"Windows-1255\"?>" +
                                "<screen>" +
                                "<s fil=\"MSGSCRFM  \" rec=\"MSGSCR01  \" fld=\"0000000000\" find=\"0000\" fcmd=\"00\"/>" +
                                "<fields></fields></screen>";

                    string ErrorFromSql = gc.SendUsual(sAnswer, "", Session, false);
                    if (ErrorFromSql != "")
                        print_err(new Exception(ErrorFromSql), "", "", false);

                    var list = (List<string>)Session["As400WarningList"];
                    var MaximumAS400Warnings = oAS400Page.getProperty("MaximumAS400Warnings");
                    if (MaximumAS400Warnings != "")
                    {
                        if (list.Count <= int.Parse(MaximumAS400Warnings))
                        {
                            SaveFile("sent.xml", sAnswer);
                            continue;
                        }
                        else
                        {
                            var NewList = new List<string>() { "ExceedsMax" };
                            NewList.AddRange((List<string>)Session["As400WarningList"]);
                            Session["As400WarningList"] = NewList;
                        }
                    }
                }

                if (!Build_HTMLPage(sXMLToAS, sXmlOK, bBroswerRefresh, sAction == "")) return;
                if (bBroswerRefresh)
                {
                    SendHtml(sHTMLPage, true);
                    bBreakFor = true;
                    break;
                }
                switch (sTable)
                {
                    case "":
                    case "spool":
                    case "spoolpdf":
                    case "spoolpdfnew":
                    case "spool+":
                    case "image":
                    case "imagetoken":
                    case "imagepost":
                    case "imageurl":
                    case "imageurltoken":
                    case "imageurlpost":
                    case "scan":
                    case "scanpost":
                    case "scanurl":
                    case "scanurlpost":
                    case "image_M":
                        //case "wnet":
                        //case "showlog_L1":
                        //case "showlog_L2":
                        //case "showlog_L3":
                        //case "showlog_L4":
                        Session["LastHTML"] = sHTMLPage; //vk 12.06
                        //HttpContext.Current.Response.Write(sHTMLPage);
                        //gc.LogXML("LOADED", "", "Log_XML", Session["ScreenNum"].ToString(), Session);
                        SendHtml(sHTMLPage, true);
                        bBreakFor = true;
                        break;
                    case "30":
                    case "memory_delete":
                    case "memory_delete_by_key":
                        GetBuffersForMemory();
                        break;
                    default:
                        GetBuffersForSqlServer();
                        break;
                }
                if (bBreakFor) break;
            }
            Session["ScreenNum"] = (int)Session["ScreenNum"] + 1; //vk 04.05
            //string sc = (string)Request.QueryString["screen"];
            //if (sc == null || sc == "") sc = "0";
            //Session["ScreenNum"] = int.Parse(sc); //+ 1;
            if (oAS400Page != null) oAS400Page.Dispose();
            oAS400Page = null; //vk 02.05
#if PDFPROJECT
            if (oSpoolPage != null) oSpoolPage.Dispose();
            oSpoolPage = null;
#endif
            //maybe we have to delete these lines
            if ((bool)Session["StopSession"])
                if (!bModal)
                {
                    //System.Web.Security.FormsAuthentication.SignOut(); //vk 11.09
                    gc.CloseSession(Session); //vk 12.11
                    if (dp.getProperty("Relogon").ToLower() == "true") //vk 04.24 - ntg 08.04.24 changes to not have blank screen after exiting the program
                        Session.Abandon();

                }
        }

        private void SendHtml(string sText, bool bLoaded) //vk 12.11
        {
            HttpContext.Current.Response.Charset = "utf-8";
            // HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");


            //if (HttpContext.Current.Response.IsClientConnected)
            //{
            HttpContext.Current.Response.Write(sText);
            try //vk 10.20
            {
                HttpContext.Current.Response.Flush();
            }
            catch (Exception e)
            {
                LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                lw.SetLocalCounter(Session);
                lw.WriteLine("NO CLIENT", (string)Session["Station"], sText,
                    Session, (string)Session["User"], "", "", "", e);
                lw.Dispose();
                lw = null;
            }
            if (bLoaded)
            {
                GreatConnect gc = (GreatConnect)Session["gc"];
                gc.LogXML("LOADED", "", "Log_XML", Session["ScreenNum"].ToString(), Session);
            }
            //}
            //else
            //{
            //    //Task t = new Task(() =>
            //    //{
            //        LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
            //        lw.SetLocalCounter(Session);
            //        lw.WriteLine("NO CLIENT", (string)Session["Station"], sText,
            //            Session, (string)Session["User"], "", "", "", new Exception("IsClientConnected == false"));
            //        lw.Dispose();
            //        lw = null;
            //    //});
            //    //t.Start();
            //}
        }

        private void GetBuffersForSqlServer() //vk 01.05
        {
            try
            {
                LogWriter brz = new LogWriter(sDb, (string)Application["PATH"], dp.getProperty("ConnStrAddition"), true);
                if (brz.ErrorText() != "") //vk 01.09
                {
                    print_err(new Exception("1021 " + brz.ErrorText()), "", "", false);
                    return;
                }
                bool bNew = false; //vk 07.05
                switch (sTable)
                {
                    case "01":
                        sTable = "tblFlex";
                        break;
                    case "02":
                        sTable = "tblHelp";
                        break;
                    case "03":
                        sTable = "tblCities";
                        break;
                    case "04":
                        sTable = "tblStreets";
                        break;
                    case "05": //vk 12.05
                        sTable = "tb156";
                        break;
                    case "06": //vk 07.05
                        sTable = "tblFlex"; bNew = true;
                        break;
                    case "07": //vk 02.06
                        sTable = "tblAccUsr";
                        break;
                    case "08": //ik 01.10
                        sTable = "tblConverse";
                        break;
                    case "09": //ik 12.11
                        sTable = "tblVendor";
                        break;
                    case "10": //ik 12.11
                        sTable = "tblCategory";
                        break;
                    case "11": //ik 12.11
                        sTable = "tblModel";
                        break;
                    case "12": //vk 04.15
                        sTable = "tblFlexBuffer";
                        break;
                    case "13": //vk 09.24
                        sTable = "tb398";
                        break;
                }
                brz.MakeTable(Session, Request, sTable, dp.getProperty("SqlUserTables"), dp.getProperty("SqlUser"), dp.getProperty("SqlAddUsers")); //, bNew);
                if (brz.ErrorText() != "") //vk 07.06
                {
                    print_err(new Exception("1021 " + brz.ErrorText()), "", "", false);
                    bBreakFor = true;
                    return;
                }
                //vk 06.13 from here
                long nRecords = 0;
                long nRecordsToReopen = 1000; //vk 10.13
                string s = dp.getProperty("RecordsToReopen");
                if (s != "")
                    nRecordsToReopen = Int64.Parse(s);
                //vk 06.13 till here
                s = "<";
                System.DateTime dStart = System.DateTime.Now; //vk 03.08
                int nTimeout = Session.Timeout; //vk 03.08
                for (; ; )
                {
                    if (System.DateTime.Now > dStart.AddMinutes(Session.Timeout - 5))
                        Session.Timeout += 1; //vk 03.08
                    string sAnswer = "";
                    if (s == "<" || brz.ErrorText() != "")
                        sAnswer = "0000";
                    else
                        sAnswer = s.Substring(1, 4);
                    sAnswer =
                        "<?xml version=\"1.0\" encoding=\"Windows-1255\"?>" +
                        "<screen><s fil=\"          \" rec=\"          \"" +
                        " fld=\"          \" find=\"" + sAnswer + "\" fcmd=\"00\"/>" +
                        "<fields></fields></screen>";
                    //vk 03.05
                    //========================================
                    GreatConnect gc = (GreatConnect)Session["gc"];
                    string e;
                    e = gc.SendUsual(sAnswer, "", Session, false);
                    if (e != "")
                    {
                        print_err(new Exception(e), "", "", false);
                        return;
                    }
                    //========================================
                    if (s.Substring(0, 1) == " ") break;
                    //vk 03.05
                    //========================================
                    e = gc.ReceiveUsual(ref s, Session);
                    xmlLocalCounter = Session["LocalCounter"].ToString();

                    //Session["SynchrGot"]=gc.Synchr(); //vk 03.08
                    if (gc.TimeOut())
                    {
                        Session["StopSession"] = true; //vk 05.05
                        print_err(new Exception(e), "", "", false); //"Communication broken"
                        return;
                    }
                    if (e != "")
                    {
                        Session["StopSession"] = true; //vk 05.05
                        print_err(new Exception(e), "", "", false);
                        return;
                    }
                    //========================================
                    int nPartsCount = Int32.Parse(s.Substring(1, 4));
                    int nPartLength = Int32.Parse(s.Substring(5, 5));
                    s = s.PadRight(10 + nPartsCount * nPartLength); //vk 07.05
                    for (int i = 0; i < nPartsCount; i++)
                    {
                        brz.WriteLine_Tap(Session, Request, sTable, s.Substring(10 + i * nPartLength, nPartLength), sFlr, bNew);
                        if (brz.ErrorText() != "") //vk 07.06
                        {
                            print_err(new Exception("1021 " + brz.ErrorText()), "", "", false);
                            return;
                        }
                        //vk 06.13
                        if (nRecordsToReopen > 0)
                        {
                            nRecords += 1;
                            if (nRecords >= nRecordsToReopen)
                            {
                                nRecords = 0;
                                brz.Dispose();
                                brz = new LogWriter(sDb, (string)Application["PATH"], dp.getProperty("ConnStrAddition"), true);
                            }
                        }
                    }
                    if (!HttpContext.Current.Response.IsClientConnected)
                    { //vk 11.15
                      //Task t = new Task(() =>
                      //{
                        LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                        lw.SetLocalCounter(Session);
                        lw.WriteLine("NO CLIENT", (string)Session["Station"], "client disconnected during filling tables",
                            Session, (string)Session["User"], "", "", "", new Exception("IsClientConnected == false"));
                        lw.Dispose();
                        lw = null;
                        //});
                        //t.Start();
                    }
                }
                brz.MakeKeys(Session, Request, sTable, bNew); //vk 02.14
                if (brz != null) brz.Dispose();
                brz = null;
                string sPool_List = dp.getProperty("WwaPool"); //vk 11.13
                //if ((sTable == "tblConverse" || sTable == "tblAccUsr") && sPool_List != "") //vk 06.13
                if ((sTable == "tblConverse" || sTable == "tblFlexBuffer" || sTable == "tblAccUsr") && sPool_List != "") //vk 06.13
                { //vk 04.10
                    GreatConnect gc = (GreatConnect)Session["gc"];
                    bool bImpersonated = false;
                    PocketKnife.Info o = new PocketKnife.Info();
                    try
                    {
                        //if (dp.getProperty("WWA_NetworkUser") != "")
                        bImpersonated = o.impersonateValidUser(dp, "WWA_");
                        foreach (string sPool_One in sPool_List.Replace(',', ';').Split(';'))
                        {
                            string sReturn = "";
                            Exception eReturn1 = null, eReturn2 = null;
                            string sGuid = dp.getProperty("GuidWWA"); //vk 09.18
                            if (sGuid == "")
                                o.ReloadWWA(sPool_One, ref sReturn, ref eReturn1, ref eReturn2);
                            else
                                o.ReloadWWA(sPool_One, ref sReturn, ref eReturn1, ref eReturn2, o.DecryptPassword(sGuid));
                            if (eReturn1 != null)
                                gc.LogError("RESTART " + sPool_One, Session, eReturn1);
                            if (eReturn2 != null)
                                gc.LogError("RESTART " + sPool_One, Session, eReturn2);
                            if (sReturn != "OK")
                                gc.LogError("RESTART " + sPool_One, Session, new Exception(sReturn));
                            else
                                gc.LogXML("WWA RESTART", sPool_One, "Log_StartEnd", Session["ScreenNum"].ToString(), Session);
                        }
                    }
                    catch (Exception e)
                    {
                        gc.LogError("RESTART " + sPool_List, Session, e);
                    }
                    finally
                    {
                        if (bImpersonated)
                            o.undoImpersonation();
                    }
                }
                Session.Timeout = nTimeout; //vk 03.08
            }
            catch (Exception e1)
            {
                print_err(e1, "", "", false);
                return;
            }
        }

        private void GetBuffersForMemory() //vk 09.06
        {
            try
            {
                //-sql-
                string sAnswer = "";
                mem = (PocketKnife.Memory)Session["Memory"];
                switch (sTable)
                {
                    case "30":
                        ClearMem();
                        int nPartsCount = Int32.Parse(sTail.Substring(1, 4));
                        int nPartLength = Int32.Parse(sTail.Substring(5, 5));
                        sTail = sTail.PadRight(10 + nPartsCount * nPartLength);
                        for (int i = 0; i < nPartsCount; i++)
                            mem.AddItem(sTail.Substring(10 + i * nPartLength, nPartLength), sFlr);
                        sAnswer = sTail.Substring(1, 4);
                        break;
                    case "memory_delete_by_key":
                        ClearMem();
                        sAnswer = "0001";
                        break;
                    case "memory_delete":
                        mem.Clear();
                        sAnswer = "0001";
                        break;
                }
                sAnswer =
                    "<?xml version=\"1.0\" encoding=\"Windows-1255\"?>" +
                    "<screen><s fil=\"          \" rec=\"          \"" +
                    " fld=\"          \" find=\"" + sAnswer + "\" fcmd=\"00\"/>" +
                    "<fields></fields></screen>";
                //vk 03.05
                //========================================
                GreatConnect gc = (GreatConnect)Session["gc"];
                string e;
                e = gc.SendUsual(sAnswer, "", Session, false);
                if (e != "")
                {
                    print_err(new Exception(e), "", "", false);
                    return;
                }
                //========================================
            }
            catch (Exception e1)
            {
                print_err(e1, "", "", false);
                return;
            }
        }
        private void ClearMem() //vk 11.07
        {
            try
            {
                int nFirstKey = Int32.Parse(sTail.Substring(11, 2));
                if (nFirstKey == 27)
                    mem.ClearByKey(nFirstKey);
                else
                { //vk 11.07
                    int nSecondKey = Int32.Parse(sTail.Substring(13, 3));
                    if (nSecondKey != 0)
                        mem.ClearByTwoKeys(nFirstKey, nSecondKey);
                }
            }
            catch
            {
            }
        }

        private void GetBuffersForSpool(
#if PDFPROJECT
            SpoolPage.BuildPage oPage,
#else
            AS400Page.BuildPage oPage,
#endif
            string sMode,
            string winI, string wlinI, string wcolI, string wlstI, string wcstI, bool bBrowser, bool bFirstScreen,
            ref int nLineLength) //vk 09.05, 02.13
        {
            sBuffer = "";
            sht = (PocketKnife.ShowTable)Session["ShowTable"];
            sht.Clear();
            try
            {
                int nPartsCount = 0, nPartLength = 0;
                //if (sMode != "showlog_L2" && sMode != "showlog_L4")
                //{
                string s = "<";
                if (dp.getProperty("DebugXml") == "true") //vk 01.06
                {
                    try
                    {
                        PocketKnife.Fq f = new PocketKnife.Fq((string)Application["PATH"] + "\\test.txt", PocketKnife.Fq.FileType.Read);
                        sBuffer = f.Receive();
                        f.Dispose();
                        f = null;
                        nPartLength = Int32.Parse(sBuffer.Substring(0, 5));
                        sBuffer = sBuffer.Substring(5);
                        if (sMode != "spool" && sMode != "spoolpdf" && sMode != "spoolpdfnew" && sMode != "spool+") //&& sMode != "wnet" &&
                                                                                                                    //sMode != "showlog_L1" && sMode != "showlog_L2" && sMode != "showlog_L3" && sMode != "showlog_L4")
                        {
                            string sAnswer =
                                "<?xml version=\"1.0\" encoding=\"Windows-1255\"?>" +
                                "<screen><s fil=\"          \" rec=\"          \"" +
                                " fld=\"          \" find=\"0000\" fcmd=\"00\"/>" +
                                "<fields></fields></screen>";
                            GreatConnect gc = (GreatConnect)Session["gc"];
                            string e = gc.SendUsual(sAnswer, "", Session, false);
                            if (e != "") //vk 07.06
                            {
                                print_err(new Exception(e), "", "", false);
                                return;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        var x = e;
                    }
                }
                if (sBuffer == "")
                {
                    System.DateTime dStart = System.DateTime.Now; //vk 03.08
                    int nTimeout = Session.Timeout; //vk 03.08
                    for (; ; )
                    {
                        if (System.DateTime.Now > dStart.AddMinutes(Session.Timeout - 5))
                            Session.Timeout += 1; //vk 03.08
                        if (sMode == "spool" || sMode == "spoolpdf" || sMode == "spoolpdfnew" || sMode == "spool+") //|| sMode == "wnet" ||
                                                                                                                    //sMode == "showlog_L1" || sMode == "showlog_L2" || sMode == "showlog_L3" || sMode == "showlog_L4")
                            if (s.Substring(0, 1) == " ") break; //vk 01.06
                        string sAnswer = "";
                        if (s == "<")
                            sAnswer = "0000";
                        else
                            sAnswer = s.Substring(1, 4);
                        sAnswer =
                            "<?xml version=\"1.0\" encoding=\"Windows-1255\"?>" +
                            "<screen><s fil=\"          \" rec=\"          \"" +
                            " fld=\"          \" find=\"" + sAnswer + "\" fcmd=\"00\"/>" +
                            "<fields></fields></screen>";
                        //========================================
                        GreatConnect gc = (GreatConnect)Session["gc"];
                        string e;
                        e = gc.SendUsual(sAnswer, "", Session, false);
                        if (e != "")
                        {
                            print_err(new Exception(e), "", "", false);
                            return;
                        }
                        //========================================
                        if (s.Substring(0, 1) == " ") break;
                        //========================================
                        e = gc.ReceiveUsual(ref s, Session);
                        xmlLocalCounter = Session["LocalCounter"].ToString();
                        //Session["SynchrGot"]=gc.Synchr(); //vk 03.08
                        if (gc.TimeOut())
                        {
                            Session["StopSession"] = true;
                            print_err(new Exception(e), "", "", false); //"Communication broken"
                            return;
                        }
                        if (e != "")
                        {
                            Session["StopSession"] = true;
                            print_err(new Exception(e), "", "", false);
                            return;
                        }
                        //========================================
                        nPartsCount = Int32.Parse(s.Substring(1, 4));
                        nPartLength = Int32.Parse(s.Substring(5, 5));
                        s = s.PadRight(10 + nPartsCount * nPartLength);
                        sBuffer += s.Substring(10, nPartsCount * nPartLength);
                        //if (sMode == "showlog_L1" || sMode == "showlog_L3")
                        //{
                        //    sht.AddItemFromBuffer("..."); //vk 06.16
                        //}
                    }
                    Session.Timeout = nTimeout; //vk 03.08
                    //}
                    SaveFile("spool.txt", sBuffer); //vk 01.06
                }
                //switch (sMode)
                //{ //vk 06.16
                //    case "showlog_L1":
                //    case "showlog_L4":
                //        sht.FillFromSql("...", "select tblLog where MsgId=...");
                //        return;
                //    case "showlog_L2":
                //    case "showlog_L3":
                //        sht.FillFromSql("...", "select tblLog where RDate=... and RTime between ... and ...");
                //        return;
                //}
#if PDFPROJECT
                oPage.Init(sXMLFromAS);
#endif
                int nPages = 0, i;
                oPage.FillPrintServer(sBuffer, nPartLength, ref nPages); //oPage vc 06.21
                nLineLength = nPartLength; //vk 02.13
                if (sTable == "spool") return; //vk 01.06
                Session["PageHTMLAdd_Count"] = nPages; //vk 08.07
                cPages = new Hashtable(nPages);
                if (sTable == "spoolpdfnew")
                { //vk 03.09
                    string sHtml;
                    sHtml = oPage.GetPage_PrintOnly(Session["ScreenNum"].ToString(),
                        winI, wlinI, wcolI, wlstI, wcstI, 0, nPages, bBrowser, true, bFirstScreen); //oPage vc 06.21
                    cPages.Add("0001", sHtml);
                    SaveFile("spool0001.htm", sHtml);
                    GreatConnect gc = (GreatConnect)Session["gc"];
                    gc.LogXML("HTML PRINT", sHtml, "Log_HTML", Session["ScreenNum"].ToString(), Session);
                }
                else
                {
                    for (i = 1; i <= nPages; i++)
                    {
                        string sHtml;

                        sHtml = oPage.GetPage_PrintOnly(Session["ScreenNum"].ToString(),
                            winI, wlinI, wcolI, wlstI, wcstI, i, nPages, bBrowser, false, bFirstScreen); //oPage vc 06.21
                        cPages.Add(i.ToString("0000"), sHtml);
                        SaveFile("spool" + i.ToString("0000") + ".htm", sHtml);
                        GreatConnect gc = (GreatConnect)Session["gc"];
                        gc.LogXML("HTML PRINT", sHtml, "Log_HTML", Session["ScreenNum"].ToString(), Session); //vk 03.08
                    }
                }
                Session["PageHTMLAdd"] = cPages;
            }
            catch (Exception e1)
            {
                print_err(e1, "", "", false);
                return;
            }
        }

        private string ScreenCaption()
        {
            string s = "";
            try
            {
                s = dp.getProperty("WindowTitle");
                if (s != "")
                    return s.Trim();
            }
            catch
            {
                return "";
            }
            try { s += (string)Application["VERSION"]; }
            catch { s += "(ver)"; }
            s += " - ";
            //try {s+=Request.ApplicationPath.Substring(1);}
            //catch {s+="(site)";} s+=" ";
            //vk 11.13
            try { s += ((string)Session["Job"]).Substring(1, 6); }
            catch { s += "(job)"; }
            s += " - ";
            try { s += (string)Session["Url"]; }
            catch { s += "(url)"; }
            s += " - ";
            s += DateTime.Now.ToString("dd/MM/yy hh:mm:ss");

            if (dp.getProperty("DebugTitle") == "true")
            {
                //try {s+=Request.ServerVariables["SERVER_NAME"];}
                //catch {s+="(server)";} s+=" ";
                // try {s+=System.Net.Dns.Resolve("").AddressList[0].ToString();} catch {s+="(s.r.v.r)";} s+=" ";
                try { s += $" Session: {Session.SessionID}"; }
                catch { s += "(session)"; }
                s += " ";
                try { s += $" Station: {(string)Session["Station"]}; "; }
                catch { s += "(c.l.n.t)"; }
                s += " ";
                try { s += (string)Session["User"]; }
                catch { s += "(user)"; }
                s += " ";
                //try {s+=System.DateTime.Now.ToString("dd/MM/yy HH:mm:ss:fff");}
                //catch {s+="(time)";} s+=" ";
            }
            return s.Trim();
        }

        private void SaveFile(string sFile, string sString, string ParamToCheck = "") //vk 01.06
        {

            if (ParamToCheck == "" && (dp.getProperty("DebugXml") == "true") ||
               (ParamToCheck != "" && (dp.getProperty(ParamToCheck) == "true")))
            {
                try
                {
                    PocketKnife.Fq f = new PocketKnife.Fq((string)Application["PATH"] + "\\" + sFile, PocketKnife.Fq.FileType.Write);
                    f.Send(sString);
                    f.Dispose();
                    f = null;
                }
                catch (Exception e) //vk 03.22+
                {
                    GreatConnect gc = (GreatConnect)Session["gc"];
                    gc.LogError("Could not save the file: " + sFile, Session, e);
                }
            }
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }
        #endregion
    }
}
