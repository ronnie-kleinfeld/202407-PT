using System;
using System.Web.UI.HtmlControls;
using Comtec.TIS;

namespace DDSWeb
{
    /// <summary>
    /// Summary description for Attach.
    /// </summary>
    public class Attach : System.Web.UI.Page
	{
		protected HtmlInputHidden txtResult;
		protected HtmlInputHidden txtReady;
		private void Page_Load(object sender, System.EventArgs e)
		{
            if ((string)Session["LogonOpen"] == "") //in Attach.aspx and Refresh.aspx the if is different
            {
                string x = System.Web.Security.FormsAuthentication.LoginUrl;
                Response.Redirect(x.Substring(x.LastIndexOf("/") + 1));
            }
            //vk 10.17 till here
            GreatConnect gc;
            try
            {
                gc = (GreatConnect)Session["gc"];
            }
            catch (Exception ee)
            {
                var x = ee;
                txtResult.Value = "Press F5 please"; //vk 04.12
                return;
            }
			try
			{
                string sRes = "";

                PocketKnife.Info oInfo = new PocketKnife.Info();
                Reader oReader = (Reader)Application["DEFPROP"];

                string sUser = Request["hdnUser_"].ToUpper();
                string sPassword = Request["hdnPassword_"].ToUpper();
                bool bInputIsOK = true;

                bool bCheckInput = false;
                if (oReader.getProperty("XSS_c2s").ToLower() == "true") bCheckInput = true;
                if (bCheckInput)
                {
                    if ((!oInfo.InputIsOK(sUser)) || (!oInfo.InputIsOK(sPassword)))
                    {
                        bInputIsOK = false;
                    }
                }
                if (bInputIsOK)
                {
                    sRes = gc.CheckByConnect(Session, sUser, sPassword);
                    if (sRes == "")
                    {
                        sRes = gc.CheckUserBySql(Session, sUser);
                    }
                }
                else
                {
                    sRes = gc.ErrDesc(14, "");
                }
                txtResult.Value = sRes;
			}
			catch (Exception ee)
			{
                try
                {
                    txtResult.Value = gc.ErrDesc(10, ""); //vk 04.12
                }
                catch
                {
                    txtResult.Value = "Error while checking username and password: " + ee.Message; //vk 03.12
                }
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
