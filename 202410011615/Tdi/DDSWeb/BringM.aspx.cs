using System;
using System.Web.UI.HtmlControls;
using Comtec.TIS;
//using System.Threading.Tasks;

namespace DDSWeb
{
    /// <summary>
    /// Summary description for BringM.
    /// </summary>
    public class BringM : System.Web.UI.Page
    {
        protected HtmlInputHidden txtResult;
        protected HtmlInputHidden txtHeight;
        protected HtmlInputHidden txtWidth;
        protected HtmlInputHidden txtWhat;
        protected HtmlInputHidden txtCurrentText;
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
                Reader dp = (Reader)Application["DEFPROP"];
                //if (dp.getProperty("ComboStyle") == "ms")
                //{
                    ListFiller lf = new ListFiller(dp, Session, Request,
                        Request["hDb"], "TB156", //"U56DGM, U56NAME, U56SMK",
                            "(U56MNFN=" + Request["hMnf"] + ") AND (U56IZR=" +
                            Request["hIzr"] + ") AND (U56TRN=" + Request["hTrn"] + ")",
                            "U56NAME", "", Request["hDgm"], "U56NAME", "U56DGM", false, true, (string)Application["PATH"]);
                    txtWhat.Value = Request["hSN"];
                    txtCurrentText.Value = lf.CurrentCode();
                    txtResult.Value = lf.Html();
                    lf = null;
                //}
                //else
                //{
                //    ComboBuilder cb = new ComboBuilder(dp, Session, Request,
                //        Request["hDb"], "TB156", "U56DGM, U56NAME, U56SMK",
                //        "(U56MNFN=" + Request["hMnf"] + ") AND (U56IZR=" +
                //            Request["hIzr"] + ") AND (U56TRN=" + Request["hTrn"] + ")",
                //        "U56NAME", Request["hDgm"], "U56DGM", "U56NAME", (string)Application["PATH"],
                //        Request["hSN"], Request["hPer"], Request["hPfk"],
                //        Request["hWidth1"], Request["hWidth2"], Request["hHeight1"], true,
                //        Int32.Parse(Request["sMaxRows"]));
                //    txtResult.Value = cb.Html();
                //    txtWhat.Value = Request["hSN"];
                //    txtHeight.Value = cb.Height();
                //    txtWidth.Value = cb.Width();
                //    txtCurrentText.Value = cb.CurrentText();
                //    cb = null;
                //}
            }
            catch (Exception ee)
            {
                //Task t = new Task(() =>
                //{
                    Reader dp = (Reader)Application["DEFPROP"];
                    LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                    lw.SetLocalCounter(Session);
                    lw.WriteLine("ERROR", (string)Session["Station"], "BringM",
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
