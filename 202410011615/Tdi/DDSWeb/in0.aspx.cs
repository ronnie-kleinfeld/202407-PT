using System;
using Comtec.TIS; //vk 06.10

namespace DDSWeb
{
    /// <summary>
    /// Summary description for _in0.
    /// </summary>
    public class _in0 : System.Web.UI.Page
	{
        //protected System.Web.UI.HtmlControls.HtmlInputHidden ResolutionMethod;
        private void Page_Load(object sender, System.EventArgs e)
		{
            if ((string)Session["User"] == "")
            {
                string x = System.Web.Security.FormsAuthentication.LoginUrl;
                Response.Redirect(x.Substring(x.LastIndexOf("/") + 1));
            }
            //vk 10.17 till here
            //vk 06.10
            Reader dp = (Reader)Application["DEFPROP"];
            //if (dp.getProperty("ForceWinSize") == "true")
            //{
            //    if ((string)Session["Token"] == "") Session["Token"] = "ForceWinSize";
            //    //Session["ResolutionW"] = dp.getProperty("MinWidth");
            //    //Session["ResolutionH"] = dp.getProperty("MinHeight");
            //}
            //ResolutionMethod.Value = dp.getProperty("SameWindow") == "false" || dp.getProperty("Flexibility") == "hard" ? "hard" : "flexible";
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
