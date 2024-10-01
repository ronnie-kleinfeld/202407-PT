using System;
using System.Web;
//using Comtec.Tis;
using Comtec.TIS;
//using System.Threading.Tasks;

namespace DDSWeb
{
    /// <summary>
    /// Summary description for screenMod.
    /// </summary>
    public class screenMod : System.Web.UI.Page
    {
        private void Page_Load(object sender, System.EventArgs e)
        {
            if ((string)Session["User"] == "")
            {
                string x = System.Web.Security.FormsAuthentication.LoginUrl;
                Response.Redirect(x.Substring(x.LastIndexOf("/") + 1));
            }
            //vk 10.17 till here
            if ((string)Session["User"] == "")
            {
                print_err(new Exception("no session"), "", true);
                return;
            }
            //Session["ScreenNum"]=(int)Session["ScreenNum"]+1; //vk 04.05, rem vk 08.05
            Session["PingNum"] = 0; //vk 04.05
            Response.Expires = -1; //vk 09.04
            AS400Page.BuildPage oAS400Page;
            string sOut = "";
            string sAction = Request.QueryString["action"];
            GreatConnect gc = (GreatConnect)Session["gc"];
            //vk 02.03
            oAS400Page = new AS400Page.BuildPage(
                (Reader)Application["DEFPROP"], (PocketKnife.Memory)Session["Memory"], (PocketKnife.ShowTable)Session["ShowTable"],
                (string)Application["DDS_COLOR"], (string)Application["PATH"],
                (string)Session["User"], (string)Session["Guid"], (string)Session["Job"],
                (string)Session["Url"], (string)Session["Station"], (string)Session["UserAgent"]);
            if (sAction != "next")
            {
                sOut = (string)Session["PageHTMLMod"];
            }
            else
            {
                string sXMLToAS;
                string sXmlOK = ""; //vk 07.04
                string sFind = "", sFil = "", sGridPos = ""; //vk 01.09, 05.09, 05.10
                string ErrorInXML = "";
                //string sAcroError = ""; //vk 02.11
                string sClient = ""; //vk 10.11
                sXMLToAS = oAS400Page.GetXML2AS400(Context, ref sXmlOK, ref sFind, ref sFil, ref sGridPos, (string)Session["XmlFil"], (string)Session["XmlRec"], ref sClient, ref ErrorInXML);
                //oAS400Page.SetResol(sResW, sResH, false, false, ""); //vk 04.10
                //gc.LogXML("RESOLUTION", "(screenMod) " + oAS400Page.ResolForLog(), "Log_XML", Session["ScreenNum"].ToString(), Session); //vk 01.12
                try
                {
                    //vk 03.05
                    //========================================
                    gc.SendUsual(sXMLToAS, oAS400Page.ScreenNumFromHtml(), Session, false);
                    //========================================
                }
                catch (Exception e1)
                {
                    sOut = "<html><body><H3>";
                    sOut += e1.Source + " " + e1.Message;
                    sOut += "</H3></body></html>";
                    HttpContext.Current.Response.Write(sOut);
                    //System.Web.Security.FormsAuthentication.SignOut(); //vk 11.09
                    gc.CloseSession(Session); //vk 12.11
                    return;
                }
                sOut = "<html><head>";
                sOut += "<SCRIPT FOR = 'window' EVENT = 'onload'>";
                sOut += "window.returnValue = 'ok';";
                sOut += "window.close();</SCRIPT>";
                sOut += "<body></body></html>";
            }
            if (oAS400Page != null) oAS400Page.Dispose();
            oAS400Page = null; //vk 02.05
            HttpContext.Current.Response.Write(sOut);
            if ((bool)Session["StopSession"])
            {
                //System.Web.Security.FormsAuthentication.SignOut(); //vk 11.09
                gc.CloseSession(Session); //vk 12.11
            }
        }
        private void print_err(Exception e, string sXml, bool bNoSession) //vk 03.04, 10.11
        {
            string sOut = "";
            sOut += "<html><SCRIPT LANGUAGE='JAVASCRIPT' SRC='js/dds.js'></SCRIPT>";
#if AVOIDBACK
			sOut += "<body>";
#else
            sOut += "<body onbeforeunload='CloseJob();'>"; //vk 08.04, 10.04
#endif
            sOut += "<form method='post' action='screen.aspx' >";
            sOut += "<input name='Hfcmd' id='Hfcmd' type='hidden' value='' >";
            sOut += "<input name='Hx' id='Hx' type='hidden' value='x' >";
            //bool bKiosk; //vk 04.05
            //if (Session["Kiosk"] == null)
            //    bKiosk = false;
            //else
            //    bKiosk = (bool)Session["Kiosk"];
            if (bNoSession)
            {
                //vk 03.05
                sOut += "<H3>ERROR</H3>";
            }
            else
            {
                sOut += "<H3>" + e.Source + " " + e.Message + "</H3>";
                if (sXml != "")
                {
                    sOut += "<BR><textarea style='overflow-y:auto;' cols='80' rows='10'>";
                    sOut += sXml;
                    sOut += "</textarea>";
                }
                //vk 02.05
                sOut += "<BR><textarea style='overflow-y:auto;' cols='80' rows='10'>";
                sOut += e.StackTrace;
                sOut += "</textarea>";
            }
            //vk 02.05 till here
            sOut += "<BR><input type=button value=Close onClick=\"window.returnValue='timeout';opener=window;window.close();\">"; //vk 12.05 //timeout vk 02.06
            sOut += "</form></body></html>";
            HttpContext.Current.Response.Write(sOut);
            //if (Application["LOG_ERROR"].ToString()!="true") return;
            Reader dp = (Reader)Application["DEFPROP"];
            //Task t = new Task(() =>
            //{
            LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
            lw.SetLocalCounter(Session);
            lw.WriteLine(bNoSession ? "NO SESSION" : "ERROR", (string)Session["Station"], sXml,
                Session, (string)Session["User"], "", "", "", e);
            lw.Dispose();
            lw = null;
            //});
            //t.Start();
            //System.Web.Security.FormsAuthentication.SignOut(); //vk 11.09
            GreatConnect gc = (GreatConnect)Session["gc"];
            gc.CloseSession(Session); //vk 12.11
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
