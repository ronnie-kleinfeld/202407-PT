using Comtec.TIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DDSWeb
{
    public partial class cg_ok : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Reader dp = (Reader)Application["DEFPROP"];
            LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
            lw.SetLocalCounter(Session);
            lw.WriteLine("CG OK", Request.QueryString.ToString(), Session);
            lw.Dispose();
            lw = null;
        }
    }
}