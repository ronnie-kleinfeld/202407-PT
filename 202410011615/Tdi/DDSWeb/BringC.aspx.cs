using System;
using System.Web.UI.HtmlControls;
//using System.Data.SqlClient;
//using System.Data.OleDb;
using Comtec.TIS;
//using System.Threading.Tasks;

namespace DDSWeb
{
    /// <summary>
    /// Summary description for BringC.
    /// </summary>
    public class BringC : System.Web.UI.Page
    {
        protected HtmlInputHidden txtResult;
        protected HtmlInputHidden txtResult1;
        protected HtmlInputHidden txtResult2;
        protected HtmlInputHidden txtResult3;
        protected HtmlInputHidden txtWhat;
        private void Page_Load(object sender, System.EventArgs e)
        {
            if ((string)Session["User"] == "")
            {
                string x = System.Web.Security.FormsAuthentication.LoginUrl;
                Response.Redirect(x.Substring(x.LastIndexOf("/") + 1));
            }
            //vk 10.17 till here
            try
            {
                if ((string)Request["txtQuery"] == "list")
                {
                    ListFiller lf = new ListFiller((Reader)Application["DEFPROP"], Session, Request,
                        Request["txtDb"], Request["txtTable"], Request["txtWhere"],
                        Request["txtOrderBy"], "", Request["txtCurrentCode"], Request["txtTextField"], Request["txtCodeField"], false, false, (string)Application["PATH"]);
                    txtResult.Value = lf.Html();
                    lf = null;
                }
                if ((string)Request["txtQuery"] == "value")
                {
                    ValueReceiver vr = new ValueReceiver((Reader)Application["DEFPROP"], Session, Request,
                        Request["txtDb"], Request["txtTable"], Request["txtWhere"],
                        Request["txtField0"], Request["txtField1"], Request["txtField2"], Request["txtField3"], (string)Application["PATH"]);
                    txtResult.Value = vr.Answer();
                    txtResult1.Value = vr.Answer1();
                    txtResult2.Value = vr.Answer2();
                    txtResult3.Value = vr.Answer3();
                    vr = null;
                }
                txtWhat.Value = (string)Request["txtDest"];
            }
            catch (Exception ee)
            {
                //Task t = new Task(() =>
                //{
                    Reader dp = (Reader)Application["DEFPROP"];
                    LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                    lw.SetLocalCounter(Session);
                    lw.WriteLine("ERROR", (string)Session["Station"], "BringC",
                        Session, (string)Session["User"], "", "", "", ee);
                    lw.Dispose();
                    lw = null;
                //});
                //t.Start();
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
