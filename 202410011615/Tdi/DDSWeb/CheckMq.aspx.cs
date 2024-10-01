using System;
using System.Web.UI.HtmlControls;
using Comtec.TIS;
//using System.Threading.Tasks;

namespace DDSWeb
{
    /// <summary>
    /// Summary description for CheckMq.
    /// </summary>
    public class CheckMq : System.Web.UI.Page
    {
        protected HtmlInputHidden txtResult;
        private void Page_Load(object sender, System.EventArgs e)
        {
            if ((string)Session["User"] == "")
            {
                string x = System.Web.Security.FormsAuthentication.LoginUrl;
                Response.Redirect(x.Substring(x.LastIndexOf("/") + 1));
            }
            //vk 10.17 till here
            Reader dp = (Reader)Application["DEFPROP"];
            if (Request["hBefore"] != "")
                LogOk("BEFORE CHECK MQ", "CheckMq. " + Request["hMqComment"], dp.getProperty("ConnStrAddition"));
            PocketKnife.Mq oMq = null;
            try
            {
                oMq = new PocketKnife.Mq(Request["hMq"], "", false, Convert.ToInt32(Request["hTimeOut"]));
                string s = oMq.ReceiveSimple(Request["hKey"]);
                if (oMq.ErrorText() != "")
                    LogError(oMq.OriginalException(), oMq.ErrorText(), dp.getProperty("ConnStrAddition"));
                else if (oMq.TimeOut())
                    txtResult.Value = "timeout";
                else
                {
                    LogOk("AFTER CHECK MQ", "CheckMq. Received=" + s, dp.getProperty("ConnStrAddition"));
                    txtResult.Value = s;
                }
            }
            catch (Exception ee)
            {
                LogError(ee, ee.Message, dp.getProperty("ConnStrAddition"));
            }
            finally
            {
                if (oMq != null) oMq.Dispose();
                oMq = null;
            }
        }
        private void LogError(Exception e, string sValueOut, string sConnStrAddition)
        {
            //Task t = new Task(() =>
            //{
                LogWriter lw = new LogWriter("", (string)Application["PATH"], sConnStrAddition);
                lw.SetLocalCounter(Session);
                lw.WriteLine("AFTER CHECK MQ", (string)Session["Station"], sValueOut,
                    Session, (string)Session["User"], "", "", "", e);
                txtResult.Value = sValueOut;
                lw.Dispose();
                lw = null;
            //});
            //t.Start();
        }
        private void LogOk(string sEvent, string sValueForLog, string sConnStrAddition)
        {
            //Task t = new Task(() =>
            //{
                LogWriter lw = new LogWriter("", (string)Application["PATH"], sConnStrAddition);
                lw.SetLocalCounter(Session);
                lw.WriteLine(sEvent, (string)Session["Station"], sValueForLog,
                    Session, (string)Session["User"], "", "", "", "");
                //txtResult.Value = sValueOut;
                lw.Dispose();
                lw = null;
            //});
            //t.Start();
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
