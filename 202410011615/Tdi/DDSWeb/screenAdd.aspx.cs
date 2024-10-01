using System;
using System.Collections;
using System.Web;

namespace DDSWeb
{
    /// <summary>
    /// Summary description for screenAdd.
    /// </summary>
    public class screenAdd : System.Web.UI.Page //vk 08.05
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
            if ((string)Session["User"] == "")
            {
                string x = System.Web.Security.FormsAuthentication.LoginUrl;
                Response.Redirect(x.Substring(x.LastIndexOf("/") + 1));
            }
            //vk 10.17 till here
            Session["PingNum"]=0;
			Response.Expires = -1;
			Hashtable cPages=(Hashtable)Session["PageHTMLAdd"];
			HttpContext.Current.Response.Write(cPages[Request.QueryString["page"]]);
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
