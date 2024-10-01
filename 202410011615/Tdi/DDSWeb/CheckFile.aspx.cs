using System;
using System.Web.UI.HtmlControls;
//using System.Data.SqlClient;
//using System.Data.OleDb;
using Comtec.TIS;
using System.IO;
//using System.Threading.Tasks;

namespace DDSWeb
{
    /// <summary>
    /// Summary description for CheckFile.
    /// </summary>
    public class CheckFile : System.Web.UI.Page //vk 07.13
    {
        //protected HtmlInputHidden hArchiveWhat; //vk 03.20
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
            bool bImpersonated = false;
            PocketKnife.Info o = new PocketKnife.Info();
            try
            {
                bImpersonated = o.impersonateValidUser(dp, "Archive_");
                string s = dp.getProperty("ArchiveRoot_" + Request["HarchiveWhat"]); //vk 03.20
                if (s.Trim() == "")
                    s = dp.getProperty("ArchiveRoot"); //vk 03.20
                if (s.Substring(s.Length - 1) != "\\") s += "\\";
                s += Request["HarchiveKeyAndName"];
                if (File.Exists(s))
                    txtResult.Value = "yes";
                else
                    txtResult.Value = "no";
            }
            catch (Exception ee)
            {
                //Task t = new Task(() =>
                //{
                    LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                    lw.SetLocalCounter(Session);
                    lw.WriteLine("ERROR", (string)Session["Station"], "CheckFile",
                        Session, (string)Session["User"], "", "", "", ee);
                    lw.Dispose();
                    lw = null;
                //});
                //t.Start();
                txtResult.Value = ee.Message;
            }
            finally
            {
                if (bImpersonated)
                    o.undoImpersonation();
                o = null;
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
