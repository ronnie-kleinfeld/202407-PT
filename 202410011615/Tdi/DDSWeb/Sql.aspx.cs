using System;
//using System.Collections;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Web;
//using System.Web.SessionState;
//using System.Web.UI;
//using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
//using System.Data.SqlClient;
//using System.Data.OleDb;
using Comtec.TIS;

namespace DDSWeb
{
    /// <summary>
    /// Summary description for Sql.
    /// </summary>
    public class Sql : System.Web.UI.Page
    {
        protected HtmlInputHidden txtResult;
        protected HtmlInputHidden txtWhat;
        protected HtmlInputHidden txtSynchr;
        Reader dp;
        GreatConnect gc;
        LogWriter brz;
        string sTable, sDb, sFlr, sGot;
        string sVer = "";
        bool bNew, bError, bError_Init;
        long nRecords, nRecordsToReopen, nRecordsTotal, nBuffers, nBuffersToRerun;
        private void Trouble(string s, Exception ee, bool bToScreen)
        {
            if (bToScreen)
            {
                txtSynchr.Value = gc.Synchr();
                txtResult.Value = s + " " + ee.Message;
                txtWhat.Value = "error";
            }
            else
                bError = true;
            gc.LogError(s, Session, ee);
        }
        private void ChooseTable()
        {
            bNew = false;
            bError = false;
            bError_Init = false;
            sTable = gc.Param(sGot, "brz", "");
            sDb = gc.Param(sGot, "flib", "").Trim();
            sFlr = gc.Param(sGot, "flr", "");
            switch (sTable)
            {
                case "01": sTable = "tblFlex"; break;
                case "02": sTable = "tblHelp"; break;
                case "03": sTable = "tblCities"; break;
                case "04": sTable = "tblStreets"; break;
                case "05": sTable = "tb156"; break;
                case "06": sTable = "tblFlex"; bNew = true; break;
                case "07": sTable = "tblAccUsr"; break;
                case "08": sTable = "tblConverse"; break;
                case "09": sTable = "tblVendor"; break; //09+10+11 are for Hakhshara only, for their use, no use in DDS
                case "10": sTable = "tblCategory"; break;
                case "11": sTable = "tblModel"; break;
                case "12": sTable = "tblFlexBuffer"; break;
                case "13": sTable = "tb398"; break; //vk 09.24
            }
        }
        private void InitCounters()
        {
            nRecordsTotal = Int64.Parse(Request["txtRecordsTotal"].ToString());
            string s;
            nRecords = 0;
            s = dp.getProperty("RecordsToReopen");
            if (s != "")
                nRecordsToReopen = Int64.Parse(s);
            else
                nRecordsToReopen = 1000;
            nBuffers = 0;
            s = dp.getProperty("BuffersToRerun");
            if (s != "")
                nBuffersToRerun = Int64.Parse(s);
            else
                nBuffersToRerun = 0;
        }
        private bool SendGet(bool bFirst, bool bLast)
        {
            if (bFirst || bLast)
                bError_Init = false;
            string sFcmd, sFind, sAnswer;
            if (bFirst)
            {
                sAnswer = "<?xml version=\"1.0\" encoding=\"Windows-1255\"?>" +
                    "<screen><s fil=\"UNSCRBRZFM\" rec=\"WINISUR   \" fld=\"0000000000\" find=\"0000\" fcmd=\"02\"/><fields></fields></screen>";
            }
            else
            {
                if (sGot.Substring(0, 1) == "<" || bError)
                    sFind = "0000";
                else
                    sFind = sGot.Substring(1, 4);
                if (bError_Init && Int32.Parse(sVer) >= 3)
                    sFcmd = "99";
                else
                    sFcmd = "00";
                sAnswer = "<?xml version=\"1.0\" encoding=\"Windows-1255\"?>" +
                    "<screen><s fil=\"          \" rec=\"          \" fld=\"          \" find=\"" + sFind + "\" fcmd=\"" + sFcmd + "\"/><fields></fields></screen>";
            }
            //========================================
            string e;
            e = gc.SendUsual(sAnswer, "", Session, false);
            if (e != "")
            {
                Trouble(e, new Exception("Could not send data to AS/400"), true);
                return false;
            }
            //========================================
            //if (bLast) return true;
            //========================================
            e = gc.ReceiveUsual(ref sGot, Session);
            if (gc.TimeOut())
            {
                Session["StopSession"] = true;
                Trouble(e, new Exception("Timeout while getting data from AS/400"), true);
                return false;
            }
            if (e != "")
            {
                Session["StopSession"] = true;
                Trouble(e, new Exception("Could not get data from AS/400"), true);
                return false;
            }
            //========================================
            if (bLast || bError_Init)
                sGot = sGot.TrimStart();
            Session["Got_Xml"] = sGot;
            Session["Got_Tail"] = "";
            if (bFirst)
                sVer = gc.Param(sGot, "ver", "000");
            return true;
        }
        private void OneBufferToTable()
        {
            int nPartsCount = Int32.Parse(sGot.Substring(1, 4));
            int nPartLength = Int32.Parse(sGot.Substring(5, 5));
            sGot = sGot.PadRight(10 + nPartsCount * nPartLength);
            nBuffers += 1;

            for (int i = 0; i < nPartsCount; i++)
            {
                brz.WriteLine_Tap(Session, Request, sTable, sGot.Substring(10 + i * nPartLength, nPartLength), sFlr, bNew);
                if (brz.ErrorText() != "")
                    Trouble("Writing record to SQL", brz.ErrorItself(), false);
                nRecordsTotal += 1;
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
            return;
        }
        private void RestartWwa()
        {
            if (sTable != "tblConverse" && sTable != "tblFlexBuffer" && sTable != "tblAccUsr")
                return;
            string sPool_List = dp.getProperty("WwaPool");
            if (sPool_List == "")
                return;
            bool bImpersonated = false;
            PocketKnife.Info o = new PocketKnife.Info();
            try
            {
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
                o = null;
            }
        }
        private void Page_Load(object sender, System.EventArgs ea)
        {
            if ((string)Session["User"] == "")
            {
                string x = System.Web.Security.FormsAuthentication.LoginUrl;
                Response.Redirect(x.Substring(x.LastIndexOf("/") + 1));
            }
            //vk 10.17 till here
            try
            {
                dp = (Reader)Application["DEFPROP"];
                gc = (GreatConnect)Session["gc"];
                InitCounters();
                bool bFirst = (nRecordsTotal == 0);
                if (bFirst)
                {
                    //sGot = "<";
                    if (!SendGet(true, false))
                        return;
                    ChooseTable();
                }
                for (; ; ) //loop for several tables
                {
                    if (bFirst)
                    {
                        Session["SqlError"] = bError;
                        Session["SqlNew"] = bNew;
                        Session["SqlTable"] = sTable;
                        Session["SqlDb"] = sDb;
                        Session["SqlFlr"] = sFlr;
                    }
                    else
                    {
                        bError = (bool)Session["SqlError"];
                        bNew = (bool)Session["SqlNew"];
                        sTable = (string)Session["SqlTable"];
                        sDb = (string)Session["SqlDb"];
                        sFlr = (string)Session["SqlFlr"];
                        sGot = (string)Session["Got_Xml"];
                    }
                    brz = new LogWriter(sDb, (string)Application["PATH"], dp.getProperty("ConnStrAddition"), true);
                    if (brz.ErrorText() != "")
                    {
                        bError_Init = true;
                        Trouble("New object for SQL", brz.ErrorItself(), false);
                    }
                    if (bFirst && !bError_Init)
                    {
                        brz.MakeTable(Session, Request, sTable, dp.getProperty("SqlUserTables"), dp.getProperty("SqlUser"), dp.getProperty("SqlAddUsers"));
                        if (brz.ErrorText() != "")
                        {
                            bError_Init = true;
                            Trouble("Creating table in SQL", brz.ErrorItself(), false);
                        }
                    }
                    for (; ; ) //loop for buffers
                    {
                        if (!SendGet(false, false))
                            return;
                        if (!bError_Init)
                            OneBufferToTable();
                        if (sGot.Substring(0, 1) == " " || bError_Init)
                            break;
                        if (nBuffers >= nBuffersToRerun && nBuffersToRerun > 0 && !bError_Init)
                        {
                            txtSynchr.Value = gc.Synchr();
                            txtResult.Value = nRecordsTotal.ToString();
                            txtWhat.Value = "part";
                            return;
                        }
                    }
                    if (bError_Init && sTable == "")
                        break;
                    if (!bError_Init)
                    {
                        brz.MakeKeys(Session, Request, sTable, bNew);
                        if (brz.ErrorText() != "")
                            Trouble("Creating primary key in SQL", brz.ErrorItself(), false);
                        RestartWwa();
                        if (!SendGet(false, true))
                            return;
                    }
                    ChooseTable();
                    if (sTable == "")
                        break;
                    else
                        bFirst = true;
                }
                txtSynchr.Value = gc.Synchr();
                txtResult.Value = nRecordsTotal.ToString();
                txtWhat.Value = "finish";
                return;
            }
            catch (Exception ee)
            {
                Trouble("Error while copying data to SQL server:", ee, true);
            }
            finally
            {
                if (brz != null)
                    brz.Dispose();
                brz = null;
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
