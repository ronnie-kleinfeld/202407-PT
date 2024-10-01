//vk 03.04
using System;
using System.Data;
using System.Data.SqlClient;
using Comtec.TIS;
//using System.Threading.Tasks;

namespace DDSWeb
{
    /// <summary>
    /// Summary description for ValueReceiver.
    /// </summary>
    public class ValueReceiver
    {
        DataSet dsRecords;
        SqlConnection conn;
        SqlDataAdapter dtAdapter;
        string sAnswer = "";
        string sAnswer1 = "";
        string sAnswer2 = "";
        string sAnswer3 = "";
        public string Answer()
        {
            return sAnswer;
        }
        public string Answer1()
        {
            return sAnswer1;
        }
        public string Answer2()
        {
            return sAnswer2;
        }
        public string Answer3()
        {
            return sAnswer3;
        }
        public ValueReceiver(Reader dp,
            System.Web.SessionState.HttpSessionState pSession,
            System.Web.HttpRequest pRequest,
            string txtDb, string txtTable, string txtWhere,
            string txtField, string txtField1, string txtField2, string txtField3,
            string sPath)
        {
            string s0 = "";
            string s = "";
            try
            {
                PocketKnife.Info pp = new PocketKnife.Info();

                if (dp.getProperty("SqlUser") == "") //ntg 10.07.24 vladi's change regarding sql migration for bituah haklai
                    s0 = "Data Source=" + dp.getProperty("SqlServer") +
                        ";Initial Catalog=" + txtDb +
                        ";Integrated Security=true;" + dp.getProperty("ConnStrAddition");
                else
                    s0 = "Data Source=" + dp.getProperty("SqlServer") +
                    ";Initial Catalog=" + txtDb +
                    ";User ID=" + dp.getProperty("SqlUser") +
                    ";Password=" + pp.DecryptPassword(dp.getProperty("SqlPass")) + ";" + dp.getProperty("ConnStrAddition"); //vk 03.08

                conn = new SqlConnection(s0);
                conn.Open();
                dsRecords = new DataSet();
                s = "select * from " + txtTable;
                if (txtWhere != "") s += " where " + txtWhere;
                dtAdapter = new SqlDataAdapter(s, conn);
                dtAdapter.SelectCommand.CommandType = CommandType.Text;
                dtAdapter.Fill(dsRecords);
                if (dsRecords.Tables[0].Rows.Count > 0)
                {
                    sAnswer = ToHtml(dsRecords.Tables[0].Rows[0][txtField].ToString());
                    if (txtField1 != "") sAnswer1 = ToHtml(dsRecords.Tables[0].Rows[0][txtField1].ToString());
                    if (txtField2 != "") sAnswer2 = ToHtml(dsRecords.Tables[0].Rows[0][txtField2].ToString());
                    if (txtField3 != "") sAnswer3 = ToHtml(dsRecords.Tables[0].Rows[0][txtField3].ToString());
                }
            }
            catch (Exception e)
            {
                //Task t = new Task(() =>
                //{
                LogWriter lw = new LogWriter("", sPath, dp.getProperty("ConnStrAddition"));
                lw.SetLocalCounter(pSession);
                lw.WriteLine("ERROR", (string)pSession["Station"], "ValueReceiver " + s0 + " " + s, pSession,
                    (string)pSession["User"], "", "", "", e);
                lw.Dispose();
                lw = null;
                //});
                //t.Start();
            }
            finally
            {
                try { conn.Close(); }
                catch { }
                try { conn.Dispose(); }
                catch { }
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
