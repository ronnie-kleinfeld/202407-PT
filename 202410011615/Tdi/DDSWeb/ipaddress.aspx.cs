using System;

namespace TestSite
{
    /// <summary>
    /// Summary description for ipaddress.
    /// </summary>
    public class ipaddress : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
            if ((string)Session["User"] == "")
            {
                string x = System.Web.Security.FormsAuthentication.LoginUrl;
                Response.Redirect(x.Substring(x.LastIndexOf("/") + 1));
            }
            //vk 10.17 till here
            //Comtec.TIS.Reader rd=new Comtec.TIS.Reader(Server.MapPath("Defprop.txt"));
            Comtec.TIS.Reader rd=new Comtec.TIS.Reader((string)Application["PATH"] + "\\Defprop.config"); //vk 11.05
			PocketKnife.Info o=new PocketKnife.Info();
			string secAddress=rd.getProperty("secAddress");					//ConfigurationSettings.AppSettings["secAddress"];
			string secValue=o.DecryptPassword(rd.getProperty("secValue"));	//ConfigurationSettings.AppSettings["secValue"];
			if (Request.Form["secret"]==secValue) 
			{
				Response.AddHeader("content-disposition", "attachment; filename=Address.txt");
				Response.ContentType = "text/plain; name=Address.txt";
				Response.Write(secAddress);
			}
			else
				Response.StatusCode=501;
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
