using System;
using System.Web.UI.HtmlControls;
using Comtec.TIS;
//using System.Threading.Tasks;

namespace DDSWeb
{
    /// <summary>
    /// Summary description for BringS.
    /// </summary>
    public class BringS : System.Web.UI.Page
    {
        protected HtmlInputHidden txtResult;
        protected HtmlInputHidden txtTarget;
        private void Page_Load(object sender, System.EventArgs e) //vk 06.24+
        {
            if ((string)Session["User"] == "")
            {
                string x = System.Web.Security.FormsAuthentication.LoginUrl;
                Response.Redirect(x.Substring(x.LastIndexOf("/") + 1));
            }
            try
            {
                Reader dp = (Reader)Application["DEFPROP"];
                ListFiller lf = new ListFiller(dp, Session, Request,
                    Request["hDb"], "tblStreets", "sCity='" + Request["hCity"] + "'",
                        "sText", "", "", "sText", "sCode", false, true, (string)Application["PATH"]);
                txtResult.Value = lf.Html();
                txtTarget.Value = Request["hTarget"];
                lf = null;
            }
            catch (Exception ee)
            {
                Reader dp = (Reader)Application["DEFPROP"];
                LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                lw.SetLocalCounter(Session);
                lw.WriteLine("ERROR", (string)Session["Station"], "BringS",
                    Session, (string)Session["User"], "", "", "", ee);
                lw.Dispose();
                lw = null;
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
