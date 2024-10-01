using System;
using Comtec.TIS;
//using System.Threading.Tasks;

namespace DDSWeb
{
    /// <summary>
    /// Summary description for Close.
    /// </summary>
    public class Close : System.Web.UI.Page
    {
        private void Page_Load(object sender, System.EventArgs e)
        {
            if ((string)Session["User"] == "")
            {
                string x = System.Web.Security.FormsAuthentication.LoginUrl;
                Response.Redirect(x.Substring(x.LastIndexOf("/") + 1));
            }
            if ((string)Session["Refresh"] != "") //ntg 01.07.24 vladi change regarding "back" button in browser
                return;

            //vk 10.17 till here
            //Session["StopSession"]=true;
            //Task t = new Task(() =>
            //{
            Reader dp = (Reader)Application["DEFPROP"];
            LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
            lw.SetLocalCounter(Session);
            lw.WriteLine("CLOSE", (string)Session["Station"], "",
                Session, (string)Session["User"], "", "", "", "");
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
