using System;
using System.Web.UI.HtmlControls;
//using System.Data.SqlClient;
//using System.Data.OleDb;
using Comtec.TIS;
//using System.Threading.Tasks;

namespace DDSWeb
{
    /// <summary>
    /// Summary description for Call.
    /// </summary>
    // Call.aspx?txtWs=Url&txtUrl=http://172.30.7.37/ESBGateway/Services/GetPaymentURL.asmx
    // Call.aspx?txtWs=Token&txtUrl=http://172.30.7.37/ESBGateway/Services/GetPaymentToken.asmx
    public class Call : System.Web.UI.Page
    {
        protected HtmlInputHidden txtResult;
        protected HtmlInputHidden txtResultU1;
        protected HtmlInputHidden txtResultU2;
        protected HtmlInputHidden txtResultU3;
        protected HtmlInputHidden txtResultU4;
        protected HtmlInputHidden txtResultU5;
        protected HtmlInputHidden txtResultU6;
        protected HtmlInputHidden txtResultU7;
        protected HtmlInputHidden txtResultU8;
        protected HtmlInputHidden txtResultU9;
        protected HtmlInputHidden txtResultV1;
        protected HtmlInputHidden txtResultV2;
        protected HtmlInputHidden txtResultV3;
        protected HtmlInputHidden txtResultV4;
        protected HtmlInputHidden txtResultV5;
        protected HtmlInputHidden txtResultV6;
        protected HtmlInputHidden txtResultV7;
        protected HtmlInputHidden txtResultV8;
        protected HtmlInputHidden txtResultV9;
        protected HtmlInputHidden txtResultW1;
        protected HtmlInputHidden txtResultW2;
        protected HtmlInputHidden txtResultW3;
        protected HtmlInputHidden txtResultW4;
        protected HtmlInputHidden txtResultW5;
        protected HtmlInputHidden txtResultW6;
        protected HtmlInputHidden txtResultW7;
        protected HtmlInputHidden txtResultW8;
        protected HtmlInputHidden txtResultW9;
        //protected HtmlInputHidden txtWhat;
        Reader dp;
        private void Page_Load(object sender, System.EventArgs e)
        {
            if ((string)Session["User"] == "")
            {
                string x = System.Web.Security.FormsAuthentication.LoginUrl;
                Response.Redirect(x.Substring(x.LastIndexOf("/") + 1));
            }
            //vk 10.17 till here
            //if (!Page.IsPostBack)
            //    return;
            dp = (Reader)Application["DEFPROP"];
            txtResultU1.Value = "";
            txtResultU2.Value = "";
            txtResultU3.Value = "";
            txtResultU4.Value = "";
            txtResultU5.Value = "";
            txtResultU6.Value = "";
            txtResultU7.Value = "";
            txtResultU8.Value = "";
            txtResultU9.Value = "";
            txtResultV1.Value = "";
            txtResultV2.Value = "";
            txtResultV3.Value = "";
            txtResultV4.Value = "";
            txtResultV5.Value = "";
            txtResultV6.Value = "";
            txtResultV7.Value = "";
            txtResultV8.Value = "";
            txtResultV9.Value = "";
            txtResultW1.Value = "";
            txtResultW2.Value = "";
            txtResultW3.Value = "";
            txtResultW4.Value = "";
            txtResultW5.Value = "";
            txtResultW6.Value = "";
            txtResultW7.Value = "";
            txtResultW8.Value = "";
            txtResultW9.Value = "";

            //txtResult.Value = "http://";
            //return;

            try
            {
                DspCallCG.Class1 o = new DspCallCG.Class1();
                switch (Request["txtWhatCG"])
                {
                    case "Url":
                    case "ESB1": //vk 04.21
                        if (dp.getProperty("WsApi").Trim() != "")
                            txtResult.Value = o.API("Url", Server.MapPath("Color//CG_1.xml"), dp, Request, Session, Server, txtResultV1, txtResultV2,
                            txtResultV3, txtResultV4, txtResultV5, txtResultV6, txtResultV7, txtResultV8, txtResultV9); //vk 01.20
                        else
                            txtResult.Value = o.GetUrl(dp, Request, Session, txtResultV1, txtResultV2);
                        break;
                    case "Token":
                    case "ESB2": //vk 04.21
                        if (dp.getProperty("WsApi").Trim() != "")
                            txtResult.Value = o.API("Token", Server.MapPath("Color//CG_2.xml"), dp, Request, Session, Server, txtResultV1, txtResultV2,
                            txtResultV3, txtResultV4, txtResultV5, txtResultV6, txtResultV7, txtResultV8, txtResultV9); //vk 01.20
                        else
                            txtResult.Value = o.GetToken(dp, Request, Session, txtResultV1, txtResultV2,
                            txtResultV3, txtResultV4, txtResultV5, txtResultV6, txtResultV7, txtResultV8, txtResultV9);
                        break;
                    default:
                        txtResult.Value = "ERROR: wrong parameter (" + Request["txtWhatCG"] + ")";
                        break;
                }
                //o.Dispose();
            }
            catch (Exception ee)
            {
                //Task t = new Task(() =>
                //{
                    LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                    lw.SetLocalCounter(Session);
                    lw.WriteLine("ERROR", (string)Session["Station"], "Call",
                        Session, (string)Session["User"], "", "", "", ee);
                    lw.Dispose();
                    lw = null;
                //});
                //t.Start();
                txtResult.Value = "ERROR: " + ee.Message;
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
