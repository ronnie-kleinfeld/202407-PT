using System;
using Comtec.TIS;
//using System.Threading.Tasks;

namespace DDSWeb
{
    /// <summary>
    /// Summary description for Refresh.
    /// </summary>
    public class Refresh : System.Web.UI.Page
    {
        private void Page_Load(object sender, System.EventArgs e)
        {
            if ((string)Session["LogonOpen"] == "") //in Attach.aspx and Refresh.aspx the if is different
            {
                string x = System.Web.Security.FormsAuthentication.LoginUrl;
                Response.Redirect(x.Substring(x.LastIndexOf("/") + 1));
            }
            //vk 10.17 till here
            //vk 08.05
            try
            {
                int tmp;
                tmp = (int)Session["PingNum"];
                tmp++;
                Session["PingNum"] = tmp;
                //PingNumber.Value=tmp.ToString();
                //Task t = new Task(() =>
                //{
                    Reader dp = (Reader)Application["DEFPROP"];
                    LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                    string sPingID = Session["ScreenNum"].ToString() + " " + Session["PingNum"].ToString();
                    lw.SetLocalCounter(Session);
                    lw.WriteLine("PING+", (string)Session["Station"], sPingID,
                        Session, (string)Session["User"], "", "", "", "", "Log_Ping");
                    lw.Dispose();
                    lw = null;
                //});
                //t.Start();
            }
            catch (Exception ee)
            {
                try
                {
                    //Task t = new Task(() =>
                    //{
                        Reader dp = (Reader)Application["DEFPROP"];
                        LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                        lw.SetLocalCounter(Session);
                        lw.WriteLine("ERROR", (string)Session["Station"], "Ping+",
                            Session, (string)Session["User"], "", "", "", ee);
                        lw.Dispose();
                        lw = null;
                    //});
                    //t.Start();
                }
                catch
                {
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
