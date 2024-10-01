using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
//using System.Data.SqlClient;
//using System.Data.OleDb;
using Comtec.TIS;
using System.IO;
using Opswat.Metadefender.Core.Client;
using Opswat.Metadefender.Core.Client.Responses;
using Opswat.Metadefender.Core.Client.Exceptions;
//using System.Threading.Tasks;

namespace DDSWeb
{
    /// <summary>
    /// Summary description for Upload.
    /// </summary>
    public class Upload : System.Web.UI.Page //vk 07.13
    {
        protected HtmlInputHidden txtResult;
        protected HtmlInputHidden hArchiveKey;
        protected HtmlInputHidden hArchiveName;
        protected HtmlInputHidden hArchiveDescr;
        protected HtmlInputHidden hArchiveDB; //vk 07.17
        protected HtmlInputHidden hArchiveMail; //vk 10.17
        protected HtmlInputHidden hArchiveWhat; //vk 03.20
        protected HtmlInputHidden hArchiveText; //vk 09.18
        protected HtmlInputHidden hArchiveSystem; //vk 12.18
        protected HtmlInputHidden hArchiveDocType; //vk 07.17
        protected HtmlInputHidden hArchiveReplace;
        protected HtmlInputFile hArchiveFile;
        //vk 03.20
        //string sTmpDir = "";
        string sTmpFile = "";
        Stream inputStream = null;
        private void ClearTmp(string sError, string sShortError, bool bLog) //vk 03.20
        {
            if (inputStream != null)
            {
                inputStream.Close();
                inputStream = null;
            }
            if (sShortError == "")
                txtResult.Value = sError;
            else
                txtResult.Value = sShortError;
            if (File.Exists(sTmpFile))
            {
                try {
                    File.Delete(sTmpFile);
                }
                catch (Exception e)
                {
                    if (sShortError == "")
                        txtResult.Value = sError + "; " + e.Message;
                    else
                        txtResult.Value = sShortError + "; " + e.Message;
                }
            }
            //if (Directory.Exists(sTmpDir))
            //    Directory.Delete(sTmpDir);
            if (bLog)
            {
                Reader dp = (Reader)Application["DEFPROP"];
                LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                lw.SetLocalCounter(Session);
                if (sShortError == "")
                    lw.WriteLine("WHITENING RESULT", (string)Session["Station"], sError,
                        Session, (string)Session["User"], "", "", "", "");
                else
                    lw.WriteLine("WHITENING ERROR", (string)Session["Station"], sError,
                        Session, (string)Session["User"], "", "", "", "");
                lw.Dispose();
                lw = null;
            }
        }
        private void Page_Load(object sender, System.EventArgs e)
        {
            if ((string)Session["User"] == "")
            {
                string x = System.Web.Security.FormsAuthentication.LoginUrl;
                Response.Redirect(x.Substring(x.LastIndexOf("/") + 1));
            }
            //vk 10.17 till here
            Reader dp = (Reader)Application["DEFPROP"];
            hArchiveFile.Accept = dp.getProperty("ArchiveMask"); //vk 10.17
            if (!Page.IsPostBack)
            {
                txtResult.Value = "";
                return;
            }

            //vk 03.20
            if (dp.getProperty("ArchiveWhitening").Trim() != "")
            {
                //sTmpDir = (string)Application["PATH"] + "\\tmp\\" + Session.SessionID;
                //if (!Directory.Exists((string)Application["PATH"] + "\\tmp"))
                //    Directory.CreateDirectory((string)Application["PATH"] + "\\tmp");
                //if (!Directory.Exists(sTmpDir))
                //    Directory.CreateDirectory(sTmpDir);
                //sTmpFile = sTmpDir + "\\" + hArchiveName.Value;
                sTmpFile = hArchiveFile.PostedFile.FileName;
                int nPos = sTmpFile.LastIndexOf("\\");
                if (nPos > -1)
                    sTmpFile = sTmpFile.Substring(nPos + 1);
                sTmpFile = (string)Application["PATH"] + "\\" + "tmp-" + Session.SessionID +
                    "-" + Session["ScreenNum"].ToString() +
                    "-" + DateTime.Now.ToString("yyyyMMddHHmmss") +
                    "-" + sTmpFile;
                hArchiveFile.PostedFile.SaveAs(sTmpFile);
                try
                {
                    MetadefenderCoreClient metadefenderCoreClient = new MetadefenderCoreClient(dp.getProperty("ArchiveWhitening"));
                    inputStream = File.Open(sTmpFile, FileMode.Open);
                    FileScanResult result = metadefenderCoreClient.ScanFileSync(inputStream,
                        new FileScanOptions().SetFileName(hArchiveName.Value), 200, 1000000, 1000000);
                    if (inputStream != null)
                    {
                        inputStream.Close();
                        inputStream = null;
                    }
                    LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                    lw.SetLocalCounter(Session);
                    lw.WriteLine("WHITENING", (string)Session["Station"], result.process_info.result,
                        Session, (string)Session["User"], "", "", "", "");
                    lw.Dispose();
                    lw = null;
                    switch (result.process_info.result)
                    {
                        case "Allowed":
                            break;
                        case "Blocked":
                            ClearTmp(result.scan_results.scan_all_result_a, "", false);
                            return;
                        default:
                            ClearTmp(result.process_info.result + " (" + result.scan_results.scan_all_result_a + ")", "", false);
                            return;
                    }
                }
                catch (MetadefenderClientException ee)
                {
                    string sMsg = ee.GetDetailedMessage();
                    if (sMsg.IndexOf("The underlying connection was closed: An unexpected error occurred on a receive") > -1
                            || sMsg.IndexOf("The operation has timed out") > -1
                            || sMsg.IndexOf("Unable to connect to the remote server") > -1)
                    {
                        string sError = dp.getProperty("WhiteningError"); //vk 07.20
                        if (sError == "")
                            sError = "Whitening error";
                        ClearTmp(sMsg, sError, true);
                    }
                    else
                        ClearTmp(sMsg, "", true);
                    return;
                }
                catch (Exception ee)
                {
                    ClearTmp(ee.Message, "", true);
                    return;
                }
            }

            bool bImpersonated = false;
            PocketKnife.Info o = new PocketKnife.Info();
            try
            {
                bImpersonated = o.impersonateValidUser(dp, "Archive_");

                //vk 08.17
                //Task t = new Task(() =>
                //{
                    LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                    lw.SetLocalCounter(Session);
                    lw.WriteLine("ARCHIVE", (string)Session["Station"], (bImpersonated ? "Impersonation succeeded" : "Impersonation failed"),
                        Session, (string)Session["User"], "", "", "", "");
                    lw.Dispose();
                    lw = null;
                //});
                //t.Start();

                string sRoot = dp.getProperty("ArchiveRoot_" + hArchiveWhat.Value); //vk 03.20
                if (sRoot.Trim() == "")
                    sRoot = dp.getProperty("ArchiveRoot"); //vk 03.20
                if (sRoot.Substring(sRoot.Length - 1) != "\\") sRoot += "\\";
                string sDir, sFile, sFile0;
                if (dp.getProperty("ArchiveLogic") == "1")
                { //vk 07.17
                    string sFileOrig = hArchiveName.Value;
                    int i = sFileOrig.LastIndexOf(".");
                    string sFileNew = hArchiveKey.Value + "-" + Session.SessionID + "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff"); //ToNumeric vc 11.17 //o.ToNumeric(Session.SessionID, -48)
                    string sExt = sFileOrig.Substring(i); //vk 10.17
                    bool bFound = false;
                    foreach (string se in dp.getProperty("ArchiveMask").Split(','))
                    {
                        if (se.Trim() == sExt) bFound = true;
                    }
                    if (!bFound)
                    {
                        string sMessage = "Permitted file types: " + dp.getProperty("ArchiveMask");
                        //Task t0 = new Task(() =>
                        //    {
                            LogWriter lw0 = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                            lw0.SetLocalCounter(Session);
                            lw0.WriteLine("ERROR", (string)Session["Station"], "Upload",
                                Session, (string)Session["User"], "", "", "", new Exception(sMessage));
                            lw0.Dispose();
                            lw0 = null;
                        //    });
                        //t0.Start();
                        txtResult.Value = sMessage;
                        return;
                    }
                    sFile = sRoot + sFileNew + sExt;
                    sFile0 = sRoot + sFileNew + ".xml";
                }
                else
                {
                    sDir = sRoot + hArchiveKey.Value;
                    if (!Directory.Exists(sDir))
                        Directory.CreateDirectory(sDir);
                    sFile = sDir + "\\" + hArchiveName.Value;
                    //string sFile0 = sRoot + dp.getProperty("ArchiveFilePrefix")
                    //        + hArchiveKey.Value + hArchiveName.Value + ".txt";
                    sFile0 = sRoot + dp.getProperty("ArchiveFilePrefix") + hArchiveKey.Value
                        + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt"; //vk 11.13
                }

                if (File.Exists(sFile))
                {
                    if (hArchiveReplace.Value == "true")
                    {
                        File.Delete(sFile);
                        //vk 08.13
                        if (File.Exists(sFile0))
                            File.Delete(sFile0);
                    }
                    else
                    {
                        txtResult.Value = "File exists";
                        return;
                    }
                }
                //moved here vk 03.20
                object oFromSql = null;
                if (dp.getProperty("ArchiveLogic") == "1")
                {
                    PocketKnife.Sq s = new PocketKnife.Sq(dp.getProperty("SqlServer"), hArchiveDB.Value,
                        dp.getProperty("ArchiveSelectFrom") + hArchiveDocType.Value,
                        dp.getProperty("SqlUser"), o.DecryptPassword(dp.getProperty("SqlPass")), dp.getProperty("ConnStrAddition"));
                    oFromSql = s.Receive("FLSTXT");
                    s.Dispose();
                    s = null;
                    if (oFromSql == null)
                    {
                        ClearTmp("No record in DB", "", true);
                        return;
                    }
                }
                //till here vk 03.20
                if (dp.getProperty("ArchiveWhitening").Trim() != "")
                { //vk 03.20
                    File.Move(sTmpFile, sFile);
                    ClearTmp("OK", "", false);
                }
                else
                {
                    hArchiveFile.PostedFile.SaveAs(sFile);
                    txtResult.Value = "OK";
                }                
                //vk 08.13, 11.13
                PocketKnife.Fq f;
                string sEncoding = dp.getProperty("ArchiveEncoding");
                if (sEncoding == "")
                    f = new PocketKnife.Fq(sFile0, PocketKnife.Fq.FileType.Write);
                else
                    f = new PocketKnife.Fq(sFile0, PocketKnife.Fq.FileType.Write, sEncoding);
                if (dp.getProperty("ArchiveLogic") == "1")
                { //vk 07.17
                    //vk 12.18 from here
                    string sTemplate = "archive1.xml";
                    if (hArchiveSystem.Value == "T")
                        sTemplate = "archive1t.xml";
                    PocketKnife.Fq g = new PocketKnife.Fq(Server.MapPath("Color//" + sTemplate), PocketKnife.Fq.FileType.Read);
                    //vk 12.18 till here
                    string sXml = g.Receive();
                    g.Dispose();
                    g = null;
                    string sShort = oFromSql.ToString(); //vk 08.17
                    string[] a = hArchiveKey.Value.Split('-');
                    sXml = sXml.Replace("{@file@}", sFile);
                    sXml = sXml.Replace("{@key.1@}", Elem(a, 0));
                    sXml = sXml.Replace("{@key.2@}", Elem(a, 1));
                    sXml = sXml.Replace("{@key.3@}", Elem(a, 2));
                    sXml = sXml.Replace("{@key.4@}", Elem(a, 3));
                    sXml = sXml.Replace("{@key.5@}", Elem(a, 4));
                    sXml = sXml.Replace("{@key.6@}", Elem(a, 5));
                    sXml = sXml.Replace("{@key.7@}", Elem(a, 6));
                    sXml = sXml.Replace("{@key.8@}", Elem(a, 7));
                    sXml = sXml.Replace("{@key.9@}", Elem(a, 8));
                    sXml = sXml.Replace("{@key.10@}", Elem(a, 9));
                    //sXml = sXml.Replace("{@long@}", Right5(s.Receive("FLLTXT").ToString()));
                    //sXml = sXml.Replace("{@short@}", Right5(s.Receive("FLSTXT").ToString()));
                    sXml = sXml.Replace("{@left@}", Left5(sShort));
                    sXml = sXml.Replace("{@right@}", Right5(sShort));
                    sXml = sXml.Replace("{@date@}", DateTime.Now.ToString("yyyyMMdd"));
                    sXml = sXml.Replace("{@email@}", hArchiveMail.Value); //vk 10.17
                    sXml = sXml.Replace("{@text@}", hArchiveText.Value); //vk 09.18
                    f.SendExact(sXml);
                    //s.Dispose();
                    //s = null;
                }
                else
                {
                    f.Send(hArchiveKey.Value + hArchiveDescr.Value + sFile);
                }
                f.Dispose();
                //if (hArchiveReplace.Value != "true") //vk 07.13
                //{
                //PocketKnife.Fq f = new PocketKnife.Fq(sRoot + dp.getProperty("ArchiveFilePrefix")
                //    + DateTime.Now.ToString("yyMMddHHmmssfffffff") + ".txt",
                //    PocketKnife.Fq.FileType.Write);
                //f.Send(hArchiveDescr.Value);
                //f.Send(sFile);
                //f.Dispose();
                //}
                //txtResult.Value = "OK";
            }
            catch (Exception ee)
            {
                //Task t = new Task(() =>
                //{
                    LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                    lw.SetLocalCounter(Session);
                    lw.WriteLine("ERROR", (string)Session["Station"], "Upload",
                        Session, (string)Session["User"], "", "", "", ee);
                    lw.Dispose();
                    lw = null;
                //});
                //t.Start();
                txtResult.Value = ee.Message;
            }
            finally
            {
                if (bImpersonated)
                    o.undoImpersonation();
                o = null;
            }
        }
        private string Right5(string txt)
        {
            try
            {
                Reader dp = (Reader)Application["DEFPROP"];
                //if (dp.getProperty("ComboStyle") == "ms")
                    return txt.Substring(5, 5); //vk 05.20
                //else
                //    return txt.Substring(9, 1) + txt.Substring(7, 1) + txt.Substring(5, 1) + txt.Substring(3, 1) + txt.Substring(1, 1);
            }
            catch
            {
                return "";
            }
        }
        private string Left5(string txt)
        {
            try
            {
                Reader dp = (Reader)Application["DEFPROP"];
                //if (dp.getProperty("ComboStyle") == "ms")
                    return txt.Substring(0, 5); //vk 05.20
                //else
                //    return txt.Substring(19, 1) + txt.Substring(17, 1) + txt.Substring(15, 1) + txt.Substring(13, 1) + txt.Substring(11, 1);
            }
            catch
            {
                return "";
            }
        }
        private string Elem(string[] a, int i)
        {
            try
            {
                if (a.GetUpperBound(0) < i)
                    return "";
                else
                    return a[i];
            }
            catch
            {
                return "";
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
