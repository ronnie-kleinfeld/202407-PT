//vk 03.04
using System;
using System.Data;
using System.Data.SqlClient;
using Comtec.TIS;

namespace DDSWeb
{
    /// <summary>
    /// Summary description for ListFiller.
    /// </summary>
    public class ListFiller
    {
        DataSet dsRecords;
        SqlConnection conn;
        SqlDataAdapter dtAdapter;
        string sHtml = "";
        string sCurrentText = "";
        string sCurrentCode = "";
        public string Html()
        {
            return sHtml;
        }
        public string CurrentText()
        {
            return sCurrentText;
        }
        public string CurrentCode()
        {
            return sCurrentCode;
        }
        public ListFiller(Reader dp,
            System.Web.SessionState.HttpSessionState pSession,
            System.Web.HttpRequest pRequest,
            string txtDb, string txtTable, string txtWhere, string txtOrderBy,
            string txtCurrentText, string txtCurrentCode,
            string txtTextField, string txtCodeField, bool bAddEmptyLine, bool bMarkIfOne,
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
                s += " order by " + txtOrderBy; //sText";
                dtAdapter = new SqlDataAdapter(s, conn);
                dtAdapter.SelectCommand.CommandType = CommandType.Text;
                dtAdapter.Fill(dsRecords);
                sHtml = "\n";
                if (bAddEmptyLine) sHtml += "<OPTION value=\" \"></OPTION>\n";
                sCurrentText = txtCurrentText;
                foreach (DataRow dr in dsRecords.Tables[0].Rows)
                {
                    sHtml += "<OPTION value=\"" + ToHtml(dr[txtCodeField].ToString()) + "\"";
                    bool bSame;
                    if (txtCurrentCode == "")
                        bSame = txtCurrentText == dr[txtTextField].ToString() && txtCurrentText != "";
                    else
                        bSame = int.Parse(txtCurrentCode) == int.Parse(dr[txtCodeField].ToString()) && txtCurrentText == "";
                    if (
                        (dsRecords.Tables[0].Rows.Count == 1 && bMarkIfOne) || bSame
                    )
                    {
                        sHtml += " SELECTED";
                        if (sCurrentText == "") sCurrentText = dr[txtTextField].ToString();
                        if (sCurrentCode == "") sCurrentCode = dr[txtCodeField].ToString();
                    }
                    sHtml += ">" + ToHtml(dr[txtTextField].ToString()) + "</OPTION>\n";
                }
            }
            catch (Exception e)
            {
                //Task t = new Task(() =>
                //{
                LogWriter lw = new LogWriter("", sPath, dp.getProperty("ConnStrAddition"));
                lw.SetLocalCounter(pSession);
                lw.WriteLine("ERROR", (string)pSession["Station"], "ListFiller " + s0 + " " + s, pSession,
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
