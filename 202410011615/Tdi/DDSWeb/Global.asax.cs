using System;
using Comtec.TIS;
using System.Collections.Generic;
//Basic Authentication
//using System.Net.Http.Headers;
//using System.Security.Principal;

namespace DDSWeb
{
    /// <summary>
    /// Summary description for Global.
    /// </summary>
    public class Global : System.Web.HttpApplication
    {

        //vk 04.09
        string gsPath, gsComment, gsUser; //gsSession,
        bool gbCounter;
        //vk 09.09
        DspWsServer.Service1 ws;
        //Basic Authentication
        //IHttpModule m;

        public Global()
        {
            InitializeComponent();
        }

        protected void Application_Start(Object sender, EventArgs e)
        {
            string stPropPath = Server.MapPath("Defprop.config"); //vk 11.05
            string stPropVerPath = Server.MapPath("Version.txt");
            Reader rd = new Reader(stPropPath);
            Reader rdVer = new Reader(stPropVerPath);
            Application["VERSION"] = rdVer.getProperty("AppVersion");
            Application["DEFPROP"] = rd;
            Application["PRINTINGGROUP"] = rd.getProperty("PrintingGroup"); //vk 10.05
            Application["LANGUAGE"] = rd.getProperty("Language"); //vk 10.05
            //Application["WIDESCREENSUPPORT"] = rd.getProperty("WideScreenSupport"); //vk 03.08
            Application["PATH"] = Server.MapPath(""); //vk 09.09
            Application["FRAME"] = rd.getProperty("Frame"); //vk 05.10
            Application["META"] = rd.getProperty("AdditionalHtml_Header"); //vk 01.14

            PocketKnife.Fq f = new PocketKnife.Fq(Server.MapPath("Color//ColorDef.xml"), PocketKnife.Fq.FileType.Read);
            Application["DDS_COLOR"] = "<record>" + f.Receive() + "</record>";
            f.Dispose();

            //vk 10.15
            try
            {
                if (!System.IO.Directory.Exists((string)Application["PATH"] + "\\TextLog\\"))
                    System.IO.Directory.CreateDirectory((string)Application["PATH"] + "\\TextLog\\");
            }
            catch (Exception ee) { var x = ee; }

            Reader dp;
            try
            {
                dp = (Reader)Application["DEFPROP"];
                LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                lw.WriteLine("APPLICATION STARTED", "");
                lw.Dispose();
                lw = null;
            }
            catch (Exception ee) { var x = ee; }

            //DelTmp((string)Application["PATH"], "", false, "Application_Start", "");
        }

        private void ws_ClearMqCompleted(object sender, DDSWeb.DspWsServer.ClearMqCompletedEventArgs e) //vk 09.09
        {
            if (ws.Result() != "")
            {
                Reader dp = (Reader)Application["DEFPROP"];
                LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                lw.SetLocalCounter(Session);
                lw.WriteLine("CLEAR MQ ERROR", ws.Result(), Session);
                lw.Dispose();
                lw = null;
            }
            ws.Dispose();
            ws = null;
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            Reader dp = (Reader)Application["DEFPROP"];
            Session["ScreenNum"] = 0;
            Session["PingNum"] = 1;
            if (dp.getProperty("GarbageCollect") == "true") GC.Collect();
            Session["gc"] = new GreatConnect(dp, Request, Server, Session, (string)Application["PATH"]);
            Session["PageHTML"] = "<html></html>";
            Session["PageXML"] = "<screen></screen>";
            Session["PageHTMLMod"] = "<html></html>";
            Session["PageHTMLAdd"] = null; //vk 09.05
            Session["PageHTMLAdd_Count"] = 0; //vk 08.07
            Session["PageXMLMOD"] = "<screen></screen>";
            Session["StopSession"] = false;
            //Session["ResolutionW"] = "";
            //Session["ResolutionH"] = "";
            Session["User"] = "";
            Session["Password"] = ""; //vk 04.12
            //Session["Kiosk"] = false;
            Session["Path"] = Application["PATH"]; //vk 05.07
            Session["LocalCounter"] = 0;
            Session["Modal"] = false;
            Session["Guid"] = ""; //vk 09.06
            Session["LastHTML"] = ""; //vk 12.06
            Session["Memory"] = new PocketKnife.Memory();
            Session["ShowTable"] = new PocketKnife.ShowTable(); //vk 06.16
            Session["Job"] = ""; //vk 05.07
            Session["Independent"] = false; //vk 10.07
            Session["LastEntry"] = ""; //vk 11.07
            Session["FileToDelete"] = ""; //vk 01.08
            Session["Got_Xml"] = ""; //vk 03.08
            Session["Got_Tail"] = ""; //vk 03.08
            Session["Find"] = ""; //vk 01.09
            Session["Fil"] = ""; //vk 01.09
            Session["XmlFil"] = ""; //vk 05.09
            Session["XmlRec"] = ""; //vk 05.09
            Session["XmlRec"] = "";
            Session["Pdf"] = ""; //vk 07.09
            Session["Token"] = ""; //vk 05.10
            //Session["AcroMethod"] = ""; //vk 02.11
            Session["CG_Transaction"] = ""; //vk 07.13
            Session["MailSent"] = false; //vk 10.13
            Session["UserAgent"] = ""; //vk 04.15
            string sUrl = Request.Url.AbsoluteUri;
            int n = sUrl.LastIndexOf("/");
            Session["Url"] = sUrl.Substring(0, n + 1); //vk 02.13
            Session["SqlError"] = false;
            Session["SqlNew"] = false;
            Session["SqlTable"] = "";
            Session["SqlDb"] = "";
            Session["SqlFlr"] = "";
            Session["SsoError"] = ""; //vk 08.17
            Session["LogonOpen"] = ""; //vk 10.17
            Session["Station"] = Request.UserHostAddress;
            Session["As400WarningList"] = new List<string>();
            Session["PageHeaderHtml"] = "";
            Session["SessionKey"] = ""; //vk 06.23
            Session["Refresh"] = "";//ntg 01.07.24 vladi change regarding "back" button in browser
            Session["QpxlNew"] = ""; //vk 09.24
            LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
            lw.SetLocalCounter(Session);
            lw.WriteLine("SESSION STARTED", (string)Session["Station"], Request.Url.AbsoluteUri,
                Session, (string)Session["User"], "", "", "", "", "Log_StartEnd");
            lw.Dispose();
            lw = null;
            //Basic Authentication
            //m = new BasicAuthHttpModule();
            //m.Init(Context.ApplicationInstance);
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            //Basic Authentication
            //m.OnApplicationAuthenticateRequest();
            //var request = HttpContext.Current.Request;
            //var authHeader = request.Headers["Authorization"];
            //if (authHeader != null)
            //{
            //    var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);

            //    // RFC 2617 sec 1.2, "scheme" name is case-insensitive
            //    if (authHeaderVal.Scheme.Equals("basic",
            //            StringComparison.OrdinalIgnoreCase) &&
            //        authHeaderVal.Parameter != null)
            //    {
            //        AuthenticateUser(authHeaderVal.Parameter);
            //    }
            //}
        }
        //private static void AuthenticateUser(string credentials)
        //{
        //    try
        //    {
        //        var encoding = Encoding.GetEncoding("iso-8859-1");
        //        credentials = encoding.GetString(Convert.FromBase64String(credentials));

        //        int separator = credentials.IndexOf(':');
        //        string name = credentials.Substring(0, separator);
        //        string password = credentials.Substring(separator + 1);

        //        if (CheckPassword(name, password))
        //        {
        //            var identity = new GenericIdentity(name);
        //            SetPrincipal(new GenericPrincipal(identity, null));
        //        }
        //        else
        //        {
        //            // Invalid username or password.
        //            HttpContext.Current.Response.StatusCode = 401;
        //        }
        //    }
        //    catch (FormatException)
        //    {
        //        // Credentials were not formatted correctly.
        //        HttpContext.Current.Response.StatusCode = 401;
        //    }
        //}
        //private static bool CheckPassword(string username, string password)
        //{
        //    return true; // username == "user" && password == "password";
        //}
        //private static void SetPrincipal(IPrincipal principal)
        //{
        //    System.Threading.Thread.CurrentPrincipal = principal;
        //    if (HttpContext.Current != null)
        //    {
        //        HttpContext.Current.User = principal;
        //    }
        //}
        protected void Application_EndRequest(Object sender, EventArgs e)
        {
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            try
            {
                Reader dp = (Reader)Application["DEFPROP"];
                LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                lw.SetLocalCounter(Session);
                try
                {
                    lw.WriteLine("ERROR", "", "",
                        Session, (string)Session["User"], "", "", "", Server.GetLastError()); //vk 08.11
                }
                catch
                {
                    lw.WriteLine("ERROR", Server.GetLastError(), Session); //vk 12.11
                }
                lw.Dispose();
                lw = null;
            }
            catch { }
        }

        protected void Application_End(Object sender, EventArgs e)
        {
            Reader dp = (Reader)Application["DEFPROP"];
            try
            {
                LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                lw.SetLocalCounter(Session);
                lw.WriteLine("APPLICATION ENDED", "");
                lw.Dispose();
                lw = null;
            }
            catch { }

            DelTmp((string)Application["PATH"], "", false, "Application_End", "");

            try
            {
                dp = (Reader)Application["DEFPROP"];
                if (dp.getProperty("DqMqAutoClear") == "true")
                {
                    ws = new DspWsServer.Service1();
                    ws.Url = dp.getProperty("WsPath");
                    ws.ClearMqCompleted += new DDSWeb.DspWsServer.ClearMqCompletedEventHandler(ws_ClearMqCompleted);
                    ws.ClearMqAsync("389E9A03-62BE-4fdf-A93A-3F8E5D56B494",
                        dp.getProperty("DqMqComputerForClear"), dp.getProperty("DqMqMask"),
                        int.Parse(dp.getProperty("DqMqDaysToSave")));
                    //ws = null;
                }
            }
            catch { }
        }

        protected void Session_End(Object sender, EventArgs e)
        {
            Reader dp = (Reader)Application["DEFPROP"];
            try
            {
                GreatConnect gc = (GreatConnect)Session["gc"];

                try
                {
                    PocketKnife.Memory m;
                    m = (PocketKnife.Memory)Session["Memory"];
                    m.Clear();
                    m = null;
                }
                catch { }
                try
                {
                    PocketKnife.ShowTable m;
                    m = (PocketKnife.ShowTable)Session["ShowTable"];
                    m.Clear();
                    m = null;
                }
                catch { }


                DelTmp((string)Application["PATH"], Session.SessionID, true, "Session_End", (string)Session["User"]);

                if ((string)Session["User"] != "")
                    if (!(bool)Session["StopSession"])
                        gc.CloseSession(Session);
                LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                lw.SetLocalCounter(Session);
                lw.WriteLine("SESSION ENDED", "", "",
                    Session, (string)Session["User"], "", "", "", "", "Log_StartEnd");
                lw.Dispose();
                lw = null;

                try
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                }
                catch (Exception)
                {
                    //throw;
                    int x = 0;
                }

                gc.Dispose(Session);
                gc = null;
            }
            catch (Exception ee)
            {
                try
                {
                    LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                    lw.SetLocalCounter(Session);
                    lw.WriteLine("ERROR", "", "", Session, "", "", "", "", ee);
                    lw.Dispose();
                    lw = null;
                }
                catch { }
            }
        }

        void DelTmp(string sPath, string sMask, bool bCounter, string sComment, string sUser)
        {
            Reader dp = (Reader)Application["DEFPROP"];
            try
            {
                gsPath = sPath;
                gbCounter = bCounter;
                gsComment = sComment;
                gsUser = sUser;
                PocketKnife.Info oDelFiles = new PocketKnife.Info();
                oDelFiles.DelFiles_Error += new PocketKnife.Info.DelFiles_ErrorEventHandler(DelTmp_Error);
                oDelFiles.DelFiles(sPath, "tmp-" + sMask + "*.*", dp, "PDF_");
                oDelFiles = null;
            }
            catch (Exception ee)
            {
                try
                {
                    LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                    lw.SetLocalCounter(Session);
                    lw.WriteLine("ERROR", "", "", Session, "", "", "", "", ee);
                    lw.Dispose();
                    lw = null;
                }
                catch { }
            }
        }
        void DelTmp_Error(Exception e)
        {
            Reader dp = (Reader)Application["DEFPROP"];
            LogWriter lw = new LogWriter("", gsPath, dp.getProperty("ConnStrAddition"));
            if (gbCounter) lw.SetLocalCounter(Session);
            try
            {
                lw.WriteLine("ERROR", "", gsComment + " - delete files", Session, gsUser, "", "", "", e);
            }
            catch (Exception ex)
            {
                lw.WriteLine("ERROR", ex, Session); //vk 12.11
            }
            lw.Dispose();
            lw = null;
        }

        #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }
        #endregion
    }
}
