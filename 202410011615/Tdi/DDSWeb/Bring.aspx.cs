using System;
using System.Web.UI;
using Comtec.TIS;
//using System.Threading.Tasks;

namespace DDSWeb
{
    public partial class Bring : Page
    {
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
                if (Request["txtCurrent"] == "")
                {
                    txtResult.Value = "";
                }
                else
                {
                    ListFiller lf = new ListFiller((Reader)Application["DEFPROP"], Session, Request,
                        Request["txtDb"], Request["txtTable"], Request["txtWhere"], "sText",
                        Request["txtCurrent"], "", "sText", "sCode", false, true, (string)Application["PATH"]);
                    txtResult.Value = lf.Html();
                    lf = null;
                }
                if ((string)Request["txtTable"] == "tblCities")
                {
                    txtWhat.Value = "City";
                }
                else
                {
                    txtWhat.Value = "Street";
                }
            }
            catch (Exception ee)
            {
                //Task t = new Task(() =>
                //{
                    Reader dp = (Reader)Application["DEFPROP"];
                    LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                    lw.SetLocalCounter(Session);
                    lw.WriteLine("ERROR", (string)Session["Station"], "Bring",
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