using System;
using System.Web.UI;
//using System.Data.SqlClient;
//using System.Data.OleDb;
using Comtec.TIS;
//using System.Threading.Tasks;

namespace DDSWeb
{
    /// <summary>
    /// Summary description for LogonQ.
    /// </summary>
    public class LogonQ : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.Label lblMsg;
        protected System.Web.UI.WebControls.Label Label2;
        protected System.Web.UI.WebControls.Label Label1;
        protected System.Web.UI.HtmlControls.HtmlInputText txtUser;
        protected System.Web.UI.HtmlControls.HtmlInputButton btnOk;
        protected System.Web.UI.HtmlControls.HtmlInputButton btnHelp; //vk 01.07
        protected System.Web.UI.HtmlControls.HtmlInputText txtPassword;
        protected System.Web.UI.HtmlControls.HtmlTable tblMainTable;
        protected System.Web.UI.HtmlControls.HtmlImage imgLogoLeft; //vk 03.06
        protected System.Web.UI.HtmlControls.HtmlImage imgLogoRight; //vk 03.06
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
        protected string sFocusAction;
        protected string sAntiJack = "0"; //vk 01.16
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdnResult;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdnReady; //vk 03.06
        protected string sAdditionalHtml; //vk 03.06
        protected string sOnLoadLogon; //vk 04.06
        protected string sComtec; //vk 12.07
        //protected System.Web.UI.HtmlControls.HtmlInputHidden hdnCopyright; //vk 02.08
        protected string sIcon; //vk 08.10
        //vk 11.17
        protected string sErrorRedirect;
        protected string sConnStrAddition;

        protected string EdgeText = "";
        private void SetLogo(Reader dp) //vk 03.06
        {
            string scriptString;
            Page.Title = dp.getProperty("Caption");
            if (dp.getProperty("Language").ToUpper() == "ENG")
            {
                scriptString = "<SCRIPT Language='JavaScript' SRC='js/ddsMsgEng.js'></SCRIPT>";
            }
            else
            {
                scriptString = "<SCRIPT Language='JavaScript' SRC='js/ddsMsgHeb.js'></SCRIPT>";
            }
            if (!ClientScript.IsClientScriptBlockRegistered("scrMsg"))
            {
                RegisterClientScriptBlock("scrMsg", scriptString);
            }
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
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

            //vk 08.17, 11.17
            sErrorRedirect = dp.getProperty("SsoLogon");
            sConnStrAddition = dp.getProperty("ConnStrAddition");
            GreatConnect gc = (GreatConnect)Session["gc"];
            if (hdnFromOK.Value == "1")
            {
                LogWriter lw = new LogWriter("", (string)Application["PATH"], sConnStrAddition);
                string U;
                switch (dp.getProperty("SSO"))
                {
                    case "Basic": //vk 10.19
                        U = User.Identity.Name;
                        if (U.IndexOf("\\") >= 0)
                        {
                            U = U.Split('\\')[1];
                        }
                        string P = Request.Headers["pass"];

                        lw.SetLocalCounter(Session);
                        lw.WriteLine("SSO", (string)Session["Station"], "UserName=" + U + "; Password=*****",
                            Session, "", "", "", "", "");
                        if (P == "" || P == null)
                        {
                            Bad(dp.getProperty("SsoError_NoPassword"));
                        }
                        else if (U == "" || U == null)
                        {
                            Bad(dp.getProperty("SsoError_NoUser"));
                        }
                        else
                        {
                            try
                            {
                                string sRes = gc.CheckByConnect(Session, U, P);
                                if (sRes == "")
                                {
                                    sRes = gc.CheckUserBySql(Session, U);
                                }
                                if (sRes == "")
                                {
                                    Session["User"] = U;
                                    lblMsg.Text = "";
                                    bool rc = gc.FullLogon(Session, Server, Request, Response,
                                        hdnIp.Value, lblMsg, null, null, null, U, "",
                                        "logonq", "", "", "", false, false, false, false);
                                    if (rc && lblMsg.Text != "")
                                    {
                                        Bad("Logon: " + lblMsg.Text);
                                    }
                                }
                                else
                                {
                                    Bad("WS: " + sRes);
                                }
                            }
                            catch (Exception ex)
                            {
                                Bad(ex);
                            }
                        }
                        break;
                    default:
                        U = (string)Request.QueryString["UserName"];
                        string sToken = (string)Request.QueryString["Token"];
                        lw.SetLocalCounter(Session);
                        lw.WriteLine("SSO", (string)Session["Station"], "Token=" + sToken + "; UserName=" + U,
                            Session, "", "", "", "", "");
                        if (sToken == "")
                        {
                            Bad(dp.getProperty("SsoError_NoToken"));
                        }
                        else if (U == "")
                        {
                            Bad(dp.getProperty("SsoError_NoUser"));
                        }
                        else
                        {
                            try
                            {
                                //vk 04.19
                                string x = dp.getProperty("SsoWs") + "?username=" + U + "&token=" + sToken;
                                lw.SetLocalCounter(Session);
                                lw.WriteLine("SSO RQ", Request.UserHostAddress, x, Session, "", "", "", "", "");
                                System.Net.WebRequest req = System.Net.WebRequest.Create(x);
                                req.Proxy = new System.Net.WebProxy();
                                System.Net.WebResponse resp = req.GetResponse();
                                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                                string y = sr.ReadToEnd().Trim();
                                lw.SetLocalCounter(Session);
                                lw.WriteLine("SSO RP", Request.UserHostAddress, y, Session, "", "", "", "", "");
                                MyResponse rp = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<MyResponse>(y);
                                sr.Dispose();
                                sr.Close();
                                sr = null;
                                resp.Close();
                                resp = null;
                                req = null;

                                if (rp.TokenIsValid)
                                {
                                    Session["User"] = U;
                                    lblMsg.Text = "";
                                    bool rc = gc.FullLogon(Session, Server, Request, Response,
                                        hdnIp.Value, lblMsg, null, null, null, U, "",
                                        "logonq", "", "", "", false, false, false, false);
                                    if (rc && lblMsg.Text != "")
                                    {
                                        Bad("Logon: " + lblMsg.Text);
                                    }
                                }
                                else
                                {
                                    Bad("WS: " + rp.ErrorMessage);
                                }
                            }
                            catch (Exception ex)
                            {
                                Bad(ex);
                            }
                        }
                        break;
                }
                lw.Dispose();
                lw = null;
            }
            else
            {
                string res = "";

                res = gc.CheckByIp(Session, Request);
                if (res != "")
                {
                    Bad("Check by IP: " + res);
                }
                else
                {
                    res = gc.CheckSiteBySql(Session);
                    if (res != "")
                    {
                        Bad("Check by SQL: " + res);
                    }
                    else
                    {
                        SetLogo(dp);
                        hdnFileForCode.Value = dp.getProperty("FileForCode"); //vk 05.05
                        hdnGetIpWay.Value = dp.getProperty("GetIpWay"); //vk 03.06
                        //hdnResolutionMethod.Value = dp.getProperty("SameWindow") == "false" || dp.getProperty("Flexibility") == "hard" ? "hard" : "flexible"; //vk 01.12
                        Page.DataBind();
                    }
                }
            }
        }

        void Bad(string sError)
        {
            //Task t = new Task(() =>
            //{
                LogWriter lw = new LogWriter("", (string)Application["PATH"], sConnStrAddition);
                lw.SetLocalCounter(Session);
                lw.WriteLine("SSO ERROR", (string)Session["Station"], sError,
                    Session, "", "", "", "", "");
                lw.Dispose();
                lw = null;
            //});
            //t.Start();
            Session["SsoError"] = sError;
            //Server.Transfer(sErrorRedirect);
            Response.Redirect(sErrorRedirect, false); //vk 02.21
        }
        void Bad(Exception e)
        {
            //Task t = new Task(() =>
            //{
                LogWriter lw = new LogWriter("", (string)Application["PATH"], sConnStrAddition);
                lw.SetLocalCounter(Session);
                lw.WriteLine("SSO ERROR", e, Session);
                lw.Dispose();
                lw = null;
            //});
            //t.Start();
            Session["SsoError"] = e.Message;
            //Server.Transfer(sErrorRedirect);
            Response.Redirect(sErrorRedirect, false); //vk 02.21
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
    public class MyResponse
    {
        public bool TokenIsValid { get; set; }
        public string ErrorMessage { get; set; }
    }
}
