using System;
using System.Web.UI;
//using System.Data.SqlClient;
//using System.Data.OleDb;
using Comtec.TIS;

namespace DDSWeb
{
    /// <summary>
    /// Summary description for Logon.
    /// </summary>
    public class Logon : System.Web.UI.Page
    {
        //changed for Bootstrap only
        protected System.Web.UI.WebControls.Label lblMsg;
        protected System.Web.UI.WebControls.Label Label2;
        protected System.Web.UI.WebControls.Label Label1;
        protected System.Web.UI.HtmlControls.HtmlInputText txtUser;
        protected System.Web.UI.HtmlControls.HtmlInputButton btnOk;
        protected System.Web.UI.HtmlControls.HtmlInputButton btnHelp; //vk 01.07
        protected System.Web.UI.HtmlControls.HtmlInputText txtPassword;
        protected System.Web.UI.HtmlControls.HtmlTable tblMainTable;
        protected System.Web.UI.HtmlControls.HtmlImage imgLogo1; //vk 03.06
        protected System.Web.UI.HtmlControls.HtmlImage imgLogo2; //vk 03.06
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdnPassword;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdnUser;
        protected System.Web.UI.WebControls.Label LabelVer; //vk 07.05
        protected System.Web.UI.WebControls.Label Label4; //vk 03.04
        protected System.Web.UI.WebControls.Label Label5; //vk 03.04
        protected System.Web.UI.WebControls.Label Label6; //vk 03.04
        protected System.Web.UI.HtmlControls.HtmlGenericControl windowtitle;
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdnResolutionMethod; //vk 06.09
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdnResolutionW;
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdnResolutionH;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdnCodeFromFile; //vk 05.05
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdnFileForCode; //vk 05.05
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdnGetIpWay; //vk 03.06
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdnIp; //vk 03.06
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdnPrintingGroup; //ik 06.05
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdnFromOK;
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdnAppletResponseTimeOut;
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdnAppletValue;
        protected System.Web.UI.WebControls.Label lblCaption;
        //		protected System.Web.UI.HtmlControls.HtmlInputHidden hdnCaption;
        //		protected System.Web.UI.WebControls.Label AppletMsg;
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdnResponseMilliSeconds;
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdnAppletPath;
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdnAppletLog;
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdnAppletErrorAsOK;
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdnAsteriskToBlank;
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdnContinueWithoutStation;
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdnAppletLoadTimeOut;
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdnDetectJavaTimeOut;
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdnInstallJavaTimeOut;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdnSso; //vk 04.17
        protected string sDir = "ltl";
        protected string bDir = "ltl";
        protected string sFocusAction;
        protected string sAntiJack = "0"; //vk 01.16
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdnResult;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdnReady; //vk 03.06
        protected string sAdditionalHtml; //vk 03.06
        protected string sOnLoadLogon; //vk 04.06
        protected string sComtec; //vk 12.07
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdnCopyright; //vk 02.08
        protected string sIcon; //vk 08.10

        private void SetCaption(string sNumber, System.Web.UI.WebControls.Label oLabel) //vk 03.05
        {
            Reader dp = (Reader)Application["DEFPROP"];
            string s = dp.getProperty("CommentStyle" + sNumber);
            if (s != "")
            {
                foreach (string a in s.Split(';'))
                {
                    if (a.IndexOf(':') > 0)
                    {
                        string[] b = a.Split(':');
                        if (b.Length == 2) oLabel.Style[b[0]] = b[1];
                    }
                }
            }
            if (sNumber == "1" && dp.getProperty("Comment1") + dp.getProperty("Comment2") + dp.getProperty("Comment3") == "")
                oLabel.Text = ""; //"<br><br><br><br><br>"; //vk 05.06
            else
                oLabel.Text = dp.getProperty("Comment" + sNumber);
        }

        private void SetLogo(Reader dp) //vk 03.06
        { //simplified for Bootstrap
            string scriptString;
            SetCaption("1", Label4);
            SetCaption("2", Label5);
            SetCaption("3", Label6);
            LabelVer.Text = (string)Application["VERSION"]; //vk 07.05
            lblCaption.Text = dp.getProperty("Caption");
            Page.Title = dp.getProperty("Caption");
            string sLogo = dp.getProperty("LogoLogon");
            if (sLogo == "") sLogo = dp.getProperty("Logo");
            imgLogo1.Src = "pics/" + sLogo;
            imgLogo2.Src = "pics/" + sLogo;
            var ln = dp.getProperty("Language");
            if (dp.getProperty("Language").ToUpper() == "ENG")
            {
                scriptString = "<SCRIPT Language='JavaScript' SRC='js/ddsMsgEng.js'></SCRIPT>";
                sDir = "lang='en' dir='ltr'";
                bDir = "direction:ltr";
                Label1.Text = "User Name:";
                Label2.Text = "Password:";
                btnOk.Value = "OK";
                //btnHelp.Value = "Help";
            }
            else
            {
                scriptString = "<SCRIPT Language='JavaScript' SRC='js/ddsMsgHeb.js'></SCRIPT>";
                sDir = "lang='he' dir='rtl'";
                bDir = "direction:rtl";
                //tblMainTable.Attributes.Add("dir", "rtl");
                Label1.Text = "שם משתמש:";
                Label2.Text = "סיסמה:";
                btnOk.Value = "כניסה";
                //btnHelp.Value = "עזרה";
            }
            if (!ClientScript.IsClientScriptBlockRegistered("scrMsg"))
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "scrMsg", scriptString);
            }
            //till here moved here vk 03.06

            //switch (dp.getProperty("LogoClick"))
            //{
            //    case "click":
            //    case "dblclick":
            //        img.Attributes.Add("on" + dp.getProperty("LogoClick"), dp.getProperty("LogoAction"));
            //        img.Style.Add("cursor", "hand");
            //        break;
            //}
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            //System.Net.WebRequest req = System.Net.WebRequest.Create();
            if (Request.Cookies["SessionKey_" + Session.SessionID] != null)
                Session["SessionKey"] = Request.Cookies["SessionKey_" + Session.SessionID].Value; //vk 06.23

            Session["LogonOpen"] = "yes"; //vk 10.17
            //if (IsPostBack)
            //{
            //    string s = "ds";
            //}

            //vk 11.05
            Reader dp = (Reader)Application["DEFPROP"];
            sFocusAction = dp.getProperty("FocusAction");
            sAdditionalHtml = dp.getProperty("AdditionalHtml");
            sOnLoadLogon = dp.getProperty("OnLoadLogon");
            sComtec = dp.getProperty("ComtecPath");
            //hdnCopyright.Value = dp.getProperty("Copyright"); //vk 02.08
            sIcon = dp.getProperty("Icon"); //vk 08.10

            if (hdnFromOK.Value == "1") //04.17
            {
                SetLogo(dp); //vk 04.06
                btnOkClick();
                return; //vk 12.05
            }
            GreatConnect gc = (GreatConnect)Session["gc"];
            string res = "";

            res = gc.CheckByIp(Session, Request);
            if (res != "")
            {
                lblMsg.Text = res;
                btnOk.Visible = false;
                txtUser.Visible = false;
                txtPassword.Visible = false;
                SetLogo(dp); //vk 12.05
                Page.DataBind();
                return;
            }
            res = gc.CheckSiteBySql(Session);
            if (res != "")
            {
                lblMsg.Text = res;
                btnOk.Visible = false;
                txtUser.Visible = false;
                txtPassword.Visible = false;
                SetLogo(dp); //vk 12.05
                Page.DataBind();
                return;
            }
            res = gc.CheckBrowser(Request); //vk 12.20
            if (res != "")
            {
                lblMsg.Text = res;
                btnOk.Visible = false;
                txtUser.Visible = false;
                txtPassword.Visible = false;
                SetLogo(dp);
                Page.DataBind();
                return;
            }

            //vk 08.17, 11.17
            string sMsgDefprop = dp.getProperty("SsoOnly");
            string sMsgSession = (string)Session["SsoError"];
            if (sMsgDefprop != "" || sMsgSession != "")
            {
                if (sMsgSession == "")
                    lblMsg.Text = sMsgDefprop;
                else
                    lblMsg.Text = sMsgSession;
                btnOk.Visible = false;
                txtUser.Visible = false;
                txtPassword.Visible = false;
                SetLogo(dp); //vk 12.05
                Page.DataBind();
                return;
            }

            //vk 04.17
            string sUser = "";
            bool bSSO = dp.getProperty("SSO") == "AD" || dp.getProperty("SSO") == "Header";
            if (bSSO && hdnSso.Value != "Screen")
            {
                switch (dp.getProperty("SSO"))
                {
                    case "AD":
                        sUser = Request.ServerVariables["AUTH_USER"];
                        break;
                    case "Header":
                        sUser = Request.Headers[dp.getProperty("SSO_Header").Trim()];
                        break;
                }
                if (sUser == null) sUser = "";
                gc.LogXML("SSO", sUser, "Log_Conn", "", Session);
                if (sUser.Trim() == "")
                {
                    hdnSso.Value = "Screen";
                    SetLogo(dp);
                    hdnFileForCode.Value = dp.getProperty("FileForCode");
                    hdnGetIpWay.Value = dp.getProperty("GetIpWay");
                    //hdnResolutionMethod.Value = dp.getProperty("SameWindow") == "false" || dp.getProperty("Flexibility") == "hard" ? "hard" : "flexible";
                    Page.DataBind();
                    //SetPrintingGroup(dp);
                    return;
                }
                int i;
                i = sUser.IndexOf("\\");
                if (i > 0)
                    sUser = sUser.Substring(i + 1);
                Session["User"] = sUser;
                Session["Password"] = "";
                res = gc.CheckUserBySql(Session, sUser);
                if (res == "")
                {
                    hdnSso.Value = "Continue";
                    if (gc.FullLogon(Session, Server, Request, Response,
                            hdnIp.Value, lblMsg, btnOk, txtUser, txtPassword,
                            sUser, "", "", "", "", "", true, false, false, false))
                        return;
                    else
                    {
                        hdnSso.Value = "Screen";
                        SetLogo(dp);
                        return;
                    }
                }
                else
                {
                    hdnSso.Value = "Screen";
                    lblMsg.Text = res;
                    SetLogo(dp);
                    hdnFileForCode.Value = dp.getProperty("FileForCode");
                    hdnGetIpWay.Value = dp.getProperty("GetIpWay");
                    //hdnResolutionMethod.Value = dp.getProperty("SameWindow") == "false" || dp.getProperty("Flexibility") == "hard" ? "hard" : "flexible";
                    Page.DataBind();
                    //SetPrintingGroup(dp);
                }
                return;
            }

            //vk 07.21
            //string sReturnUrl = Request.QueryString["ReturnUrl"]; //vk 12.05
            if (dp.getProperty("SSO") == "GUID2" || dp.getProperty("SSO") == "GUID3") //vk 11.20
            {
                string sError = "";
                if (Request.QueryString["guid"] == null)
                {
                    sError = "לא הועבר GUID";
                }
                else if (Request.QueryString["user"] == null)
                {
                    sError = "לא הועבר שם משתמש";
                }
                else
                {
                    PocketKnife.Sq oSq = null;
                    try
                    {
                        PocketKnife.Info pp = new PocketKnife.Info();
                        oSq = new PocketKnife.Sq(
                            dp.getProperty("SqlServer"), dp.getProperty("DbForCheckUser"), "select * from tblSso where sGuid='" + Request.QueryString["guid"] + "';",
                            dp.getProperty("SqlUserTables"), pp.DecryptPassword(dp.getProperty("SqlPass")), dp.getProperty("ConnStrAddition"));
                        pp = null;
                        if (oSq.ErrorText() != "")
                        {
                            sError = oSq.ErrorText();
                        }
                        else if (oSq.RowCount() == 0)
                        {
                            sError = "אין אישור לכניסה למערכת";
                        }
                        else if (oSq.Receive("sUser").ToString() != Request.QueryString["user"])
                        {
                            sError = "האישור ניתן ליוזר אחר";
                        }
                        else if (!Convert.IsDBNull(oSq.Receive("dEntered")))
                        {
                            sError = "האישור כבר נוצל";
                        }
                        else
                        {
                            DateTime dChecked = (DateTime)oSq.Receive("dChecked");
                            DateTime dNow = DateTime.Now;
                            TimeSpan ts = dNow - dChecked;
                            if (ts.TotalSeconds > Int64.Parse(dp.getProperty("SecondsToEnter")))
                            {
                                sError = "פג תוקף האישור";
                            }
                            else
                            {
                                Session["User"] = Request.QueryString["user"];
                                string sSpecialCare = "";
                                string sAdditionalData = "";
                                if (dp.getProperty("SSO") == "GUID3")
                                {
                                    sSpecialCare = "zehut";
                                    sAdditionalData = (string)oSq.Receive("sId");
                                }
                                if (gc.FullLogon(Session, Server, Request, Response,
                                        hdnIp.Value, lblMsg, btnOk, txtUser, txtPassword,
                                        Request.QueryString["user"], "",
                                        sSpecialCare, sAdditionalData, "", "", true, false, false, false))
                                {
                                    oSq.Send("dEntered", dNow);
                                    return;
                                }
                            }
                        }
                    }
                    catch (Exception ee) //vk 05.06
                    {
                        sError = ee.Message;
                    }
                    finally
                    {
                        try { oSq.Dispose(); }
                        catch { }
                        oSq = null;
                    }
                }
                if (sError != "")
                {
                    lblMsg.Text = sError;
                    btnOk.Visible = false;
                    txtUser.Visible = false;
                    txtPassword.Visible = false;
                    SetLogo(dp);
                    Page.DataBind();
                }
                return;
            }
            //else if (sReturnUrl != null)
            //{
            //    sReturnUrl = sReturnUrl.ToLower(); //vk 09.06
            //    //if (sReturnUrl.IndexOf("kiosk.aspx") > 0) //vk 11.05
            //    //{
            //    //    //Reader dp = (Reader)Application["DEFPROP"];
            //    //    PocketKnife.Info pp = new PocketKnife.Info();
            //    //    string U, P;
            //    //    if (Request.QueryString["user"] == null)
            //    //    {
            //    //        U = Page.User.Identity.Name;
            //    //        int i = U.IndexOf("\\");
            //    //        if (i >= 0) U = U.Substring(i + 1);
            //    //    }
            //    //    else
            //    //        U = Request.QueryString["user"];
            //    //    P = pp.DecryptPassword(dp.getProperty("KioskPass"));
            //    //    if (gc.FullLogon(Session, Server, Request, Response,
            //    //            hdnIp.Value, lblMsg, btnOk, txtUser, txtPassword, U, P,
            //    //            (dp.getProperty("Design") == "kiosk" ? "kiosk" : ""),
            //    //            "", "", "", true, true, true, true))
            //    //        return;
            //    //}
            //}
            else if (Request.QueryString["token"] != null)
            {
                //vk 11.06
                //U=Page.User.Identity.Name;
                //U=Request.ServerVariables["AUTH_USER"];
                //U=Request.ServerVariables["LOGON_USER"];
                //U=Request.ServerVariables["REMOTE_USER"];
                string P = "";
                //uncomment this if you want to debug something for UNIX using AS/400
                //if (dp.getProperty("CheckByConnectWay") != "yalla_sa")
                //{
                //    PocketKnife.Info pp = new PocketKnife.Info();
                //    P = pp.DecryptPassword(dp.getProperty("KioskPass"));
                //}
                //string W = Request.QueryString["width"];
                //string H = Convert.ToString(Int64.Parse(W)
                //    * Int64.Parse(dp.getProperty("MinHeight"))
                //    / Int64.Parse(dp.getProperty("MinWidth")));

                //string userAgent = Request.UserAgent;
                //if (userAgent.IndexOf("Edge") <= -1) {

                if (gc.FullLogon(Session, Server, Request, Response,
                    hdnIp.Value, lblMsg, btnOk, txtUser, txtPassword,
                    Request.QueryString["user"], P, "token",
                    Request.QueryString["token"], Request.QueryString["clerk"], "", true, true, true, true)) //vk 09.08
                {
                    //Session["SynchrGot"]=gc.Synchr(); //vk 03.08
                    return;
                }
                //}
            }
            else
            {
                gc.ParamTo400(Session, Request); //vk 03.22
                if (gc.sParamTo400_user != "")
                { //vk 03.22
                    Session["User"] = gc.sParamTo400_user;
                    if (gc.FullLogon(Session, Server, Request, Response,
                        hdnIp.Value, lblMsg, btnOk, txtUser, txtPassword,
                        gc.sParamTo400_user, "", "",
                        "", "", "", true, false, true, true)) //vk 09.08
                    {
                        return;
                    }
                }
                else if (Request.QueryString["user"] != null)
                { //vk 07.21
                    if (dp.getProperty("ParamTo400") == "URL" || dp.getProperty("ParamTo400") == "API+URL")
                    { //vk 08.22
                        Session["User"] = Request.QueryString["user"].ToString();
                        if (gc.FullLogon(Session, Server, Request, Response,
                            hdnIp.Value, lblMsg, btnOk, txtUser, txtPassword,
                            Request.QueryString["user"].ToString(), "", "",
                            "", "", "", true, false, true, true)) //vk 09.08
                        {
                            return;
                        }
                    }
                    else
                    {
                        PocketKnife.Info pp = new PocketKnife.Info();
                        string P = pp.DecryptPassword(dp.getProperty("KioskPass"));
                        pp = null;
                        if (gc.FullLogon(Session, Server, Request, Response,
                            hdnIp.Value, lblMsg, btnOk, txtUser, txtPassword,
                            Request.QueryString["user"], P, "",
                            "", "", "", true, true, true, true)) //vk 09.08
                        {
                            return;
                        }
                    }
                }
            }

            SetLogo(dp);
            hdnFileForCode.Value = dp.getProperty("FileForCode"); //vk 05.05
            hdnGetIpWay.Value = dp.getProperty("GetIpWay"); //vk 03.06
            //hdnResolutionMethod.Value = dp.getProperty("SameWindow") == "false" || dp.getProperty("Flexibility") == "hard" ? "hard" : "flexible"; //vk 01.12
            Page.DataBind();
            //SetPrintingGroup(dp);
        }

        //private void SetPrintingGroup(Reader dp)
        //{
        //    string sPrintingGroup = dp.getProperty("PrintingGroup");
        //    if (sPrintingGroup != "")
        //    {
        //        hdnPrintingGroup.Value = sPrintingGroup;
        //        hdnAppletLoadTimeOut.Value = dp.getProperty("AppletLoadTimeOut");
        //        hdnAppletResponseTimeOut.Value = dp.getProperty("AppletResponseTimeOut");
        //        hdnResponseMilliSeconds.Value = dp.getProperty("ResponseMilliSeconds");
        //        hdnAppletPath.Value = dp.getProperty("AppletPath");
        //        hdnAppletLog.Value = dp.getProperty("AppletLog");
        //        hdnAppletErrorAsOK.Value = dp.getProperty("AppletErrorAsOK");
        //        hdnAsteriskToBlank.Value = dp.getProperty("AsteriskToBlank");
        //        hdnContinueWithoutStation.Value = dp.getProperty("ContinueWithoutStation");
        //        hdnDetectJavaTimeOut.Value = dp.getProperty("DetectJavaTimeOut");
        //        hdnInstallJavaTimeOut.Value = dp.getProperty("InstallJavaTimeOut");
        //    }
        //}

        private void btnOkClick()
        {
            GreatConnect gc = (GreatConnect)Session["gc"];
            string U = hdnUser.Value.ToUpper();
            string P = hdnPassword.Value.ToUpper();
            if (U == "" || P == "")
            {
                lblMsg.Text = gc.ErrDesc(100, "");
                return;
            }

            string sParam = "";
            if (hdnCodeFromFile.Value.Trim() != "") sParam = hdnCodeFromFile.Value;
            //if (hdnAppletValue.Value.Trim() != "") sParam = hdnAppletValue.Value;

            //vk 11.05, 12.05
            Reader dp = (Reader)Application["DEFPROP"];
            gc.FullLogon(Session, Server, Request, Response,
                hdnIp.Value, lblMsg, btnOk, txtUser, txtPassword, U, P,
                (gc.ExternalAgent(U) ? "exter" : (dp.getProperty("Design") == "kiosk" ? "kiosk" : "")),
                "", "", sParam, false, false, false, false);
            //Session["SynchrGot"]=gc.Synchr(); //vk 03.08
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
