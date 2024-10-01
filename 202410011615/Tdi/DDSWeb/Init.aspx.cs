using System;
using System.Web.UI.HtmlControls;
using Comtec.TIS;
//using System.Threading.Tasks;

namespace DDSWeb
{
    /// <summary>
    /// Summary description for Init.
    /// </summary>
    public class Init : System.Web.UI.Page
    {
        private void Page_Load(object sender, System.EventArgs e)
        {
            if ((string)Session["User"] == "")
            {
                string x = System.Web.Security.FormsAuthentication.LoginUrl;
                Response.Redirect(x.Substring(x.LastIndexOf("/") + 1));
            }
            if (Request.Cookies["SessionKey_" + Session.SessionID] != null)
                Session["SessionKey"] = Request.Cookies["SessionKey_" + Session.SessionID].Value; //vk 06.23
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
