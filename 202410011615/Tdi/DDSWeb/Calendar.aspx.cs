using System;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DDSWeb
{
    public partial class Calendar : System.Web.UI.Page
    {
        string language = "";
        string[] arrlanguage ={"en-US", "he-IL"};
        //GreatConnect gc; //= (GreatConnect)Session["gc"];

        protected void Page_Load(object sender, EventArgs e)
        {
            //gc = (GreatConnect)Session["gc"];
            string value = Request["date"];
            if (!Page.IsPostBack)
            {
                language = arrlanguage[Convert.ToInt16(Request["language"])];
                //gc.LogXML("Cal", "language=" + language, "", Session["ScreenNum"].ToString(), Session);
                if (language != null)
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
                }

                Populate_MonthList();
                Populate_YearList();

                if (value.Length > 0) {
                    setDate(value);
                }
                //----------------
                //Get to day date.
                //----------------
                ToDay.Value = Calendar1.TodaysDate.ToString("dd/MM/yyyy");
                //gc.LogXML("Cal", "TodaysDate=" + ToDay.Value, "", Session["ScreenNum"].ToString(), Session);
            }
        }
        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            if (language != null)
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
            }
            TextBox1.Value = Calendar1.SelectedDate.ToString("dd/MM/yyyy");
            //gc.LogXML("Cal", "SelectedDate=" + TextBox1.Value, "", Session["ScreenNum"].ToString(), Session);
            //setDate(Calendar1.SelectedDate.ToShortDateString());
        }

        protected void Calendar1_TodaysDate(object sender, EventArgs e)
        {
            setDate(Calendar1.TodaysDate.ToString("dd/MM/yyyy"));
        }
        private void setDrpCal(int month,int year)
        {
            drpCalMonth.SelectedValue = month.ToString();
            drpCalYear.SelectedValue = year.ToString();
        }
        private void setDate(String value)
        {
            DateTime dt = DateTime.ParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            Calendar1.SelectedDate = dt;
            Calendar1.VisibleDate = dt;
            TextBox1.Value = value;
            //gc.LogXML("Cal", "sD" + TextBox1.Value, "", Session["ScreenNum"].ToString(), Session);

            setDrpCal(dt.Month, dt.Year);
        }
        protected void Set_Calendar(object Sender, EventArgs e)
        {
            int year = int.Parse(drpCalYear.SelectedValue);
            int month = int.Parse(drpCalMonth.SelectedValue);

            string value = Calendar1.SelectedDate.Day.ToString().PadLeft(2, '0') + "/" + month.ToString().PadLeft(2, '0') + "/" + year;
            DateTime dt = DateTime.ParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            Calendar1.SelectedDate = dt;
            Calendar1.VisibleDate = dt;
            TextBox1.Value = value;
            //gc.LogXML("Cal", "S_C" + TextBox1.Value, "", Session["ScreenNum"].ToString(), Session);
            //TextBox1.Value = Calendar1.SelectedDate.ToShortDateString();
        }
        protected void Populate_MonthList()
        {
            var dtf =Thread.CurrentThread.CurrentUICulture.DateTimeFormat;
            for (int i = 1; i <= 12; i++)
            {
                drpCalMonth.Items.Add(new ListItem(dtf.GetMonthName(i), i.ToString()));
            }
            drpCalMonth.Items.FindByValue(DateTime.Now.Month.ToString()).Selected = true;
        }

        protected void Populate_YearList()
        {
            //vk 06.20
            Comtec.TIS.Reader dp = (Comtec.TIS.Reader)Application["DEFPROP"];
            string s = dp.getProperty("YearsTo");
            int n = 20;
            if (s.Trim() != "")
                n = int.Parse(s);

            for (int i = 1900; i <= DateTime.Now.Year + n; i++)
            {
                drpCalYear.Items.Add(i.ToString());
            }
            drpCalYear.Items.FindByValue(DateTime.Now.Year.ToString()).Selected = true;
        }
    }
}