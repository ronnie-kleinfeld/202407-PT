using System;
using System.Web.UI.HtmlControls;
using Comtec.TIS;
//using System.Threading.Tasks;

namespace DDSWeb
{
    /// <summary>
    /// Summary description for BringF.
    /// </summary>
    public class BringF : System.Web.UI.Page
    {
        protected HtmlInputHidden txtResult;
        protected HtmlInputHidden txtHeight;
        protected HtmlInputHidden txtWidth;
        protected HtmlInputHidden txtTarget;
        protected HtmlInputHidden txtCurrentText;
        private void Page_Load(object sender, System.EventArgs e)
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
                    Request["hDb"], "tblFlex",
                    "(flkod=" + Request["hKod"] + ") AND (fltbno=" + Request["hTbno"] + ")",
                    Request["hOrderBy"], "", Request["hCurValue"], "flltxt", "flnoin", false, true, (string)Application["PATH"]);
                txtTarget.Value = Request["hTarget"];
                txtCurrentText.Value = lf.CurrentCode();
                txtResult.Value = lf.Html();
                lf = null;
            }
            catch (Exception ee)
            {
                Reader dp = (Reader)Application["DEFPROP"];
                LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                lw.SetLocalCounter(Session);
                lw.WriteLine("ERROR", (string)Session["Station"], "BringF",
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
