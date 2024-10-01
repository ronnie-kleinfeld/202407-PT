using System;
using Comtec.TIS;
//using System.Threading.Tasks;

namespace DDSWeb
{
    public partial class frame : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((string)Session["User"] == "")
            {
                string x = System.Web.Security.FormsAuthentication.LoginUrl;
                Response.Redirect(x.Substring(x.LastIndexOf("/") + 1));
            }
            //vk 10.17 till here
            //Task t = new Task(() =>
            //{
                Reader dp = (Reader)Application["DEFPROP"];
                LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                lw.SetLocalCounter(Session);
                lw.WriteLine("FRAME", (string)Session["Station"], "",
                    Session, (string)Session["User"], "", "", "", "");
                lw.Dispose();
                lw = null;
            //});
            //t.Start();
        }
    }
}
