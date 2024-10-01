using System;
using System.Web.UI;
using Comtec.TIS;
//using System.Threading.Tasks;

namespace DDSWeb
{
    public partial class CityStreet : Page
    {
        protected string sCities;
        protected string sStreets;
        protected string sCity;
        protected string sStreet;
        protected string sDb;
        protected string sHalves;
        protected string sColor;

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((string)Session["User"] == "")
            {
                string x = System.Web.Security.FormsAuthentication.LoginUrl;
                Response.Redirect(x.Substring(x.LastIndexOf("/") + 1));
            }
            //vk 10.17 till here
            string sCityCode;
            ListFiller lf;
            sDb = Request.Form["Modal_sDb"];
            sHalves = Request.Form["Modal_sHalves"];
            sColor = Request.Form["Modal_sColor"];

            try
            {
                if (Request.Form["Modal_sCityCode"] != "")
                {
                    lf = new ListFiller((Reader)Application["DEFPROP"], Session, Request,
                        Request.Form["Modal_sDb"], "tblCities",
                        "sCode='" + Request.Form["Modal_sCityCode"] + "'",
                        "sText", "", "", "sText", "sCode", false, true, (string)Application["PATH"]);
                    sCity = lf.CurrentText();
                    sCityCode = Request.Form["Modal_sCityCode"];
                    sCities = lf.Html();
                    lf = null;
                }
                else if (Request.Form["Modal_sCityName"] != "")
                {
                    sCity = Request.Form["Modal_sCityName"];
                    lf = new ListFiller((Reader)Application["DEFPROP"], Session, Request,
                        Request.Form["Modal_sDb"], "tblCities",
                        "sText like '" + sCity.Replace("'", "'+CHAR(39)+'") + "%'",
                        "sText", sCity, "", "sText", "sCode", false, true, (string)Application["PATH"]);
                    sCityCode = lf.CurrentCode();
                    sCities = lf.Html();
                    lf = null;
                }
                else
                {
                    sCity = "";
                    sCityCode = "";
                    sCities = "";
                }

                if (Request.Form["Modal_sStreetCode"] != "")
                {
                    lf = new ListFiller((Reader)Application["DEFPROP"], Session, Request,
                        Request.Form["Modal_sDb"], "tblStreets",
                        "sCode='" + Request.Form["Modal_sStreetCode"] + "' and sCity='" + sCityCode + "'",
                        "sText", "", "", "sText", "sCode", false, true, (string)Application["PATH"]);
                    sStreet = lf.CurrentText();
                    sStreets = lf.Html();
                    lf = null;
                }
                else if (Request.Form["Modal_sStreetName"] != "")
                {
                    sStreet = Request.Form["Modal_sStreetName"];
                    lf = new ListFiller((Reader)Application["DEFPROP"], Session, Request,
                        Request.Form["Modal_sDb"], "tblStreets",
                        "sText like '" + sStreet.Replace("'", "'+CHAR(39)+'") + "%' and sCity='" + sCityCode + "'",
                        "sText", sStreet, "", "sText", "sCode", false, true, (string)Application["PATH"]);
                    sStreets = lf.Html();
                    lf = null;
                }
                else
                {
                    sStreet = "";
                    sStreets = "";
                }

                //rem vk 02.09
                //sCities=ToHtml(sCities);
                //sStreets=ToHtml(sStreets);
                sCity = ToHtml(sCity);
                sStreet = ToHtml(sStreet);
            }
            catch (Exception ee)
            {
                //Task t = new Task(() =>
                //{
                    Reader dp = (Reader)Application["DEFPROP"];
                    LogWriter lw = new LogWriter("", (string)Application["PATH"], dp.getProperty("ConnStrAddition"));
                    lw.SetLocalCounter(Session);
                    lw.WriteLine("ERROR", (string)Session["Station"], "CityStreet " + sDb,
                        Session, (string)Session["User"], "", "", "", ee);
                    lw.Dispose();
                    lw = null;
                //});
                //t.Start();
            }
        }

        private string ToHtml(string s0)
        {
            string s = s0;
            s = s.Replace("&", "&amp;");
            s = s.Replace("\"", "&quot;");
            s = s.Replace("'", "&#39;");
            s = s.Replace("<", "&lt;");
            s = s.Replace(">", "&gt;");
            return s;
        }
    }
}