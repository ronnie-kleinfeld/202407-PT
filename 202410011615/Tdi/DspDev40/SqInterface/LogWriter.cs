using System;
using System.Data;
using System.Data.SqlClient;
using Comtec.Tis;

using Comtec.TIS;
using System.Text; //vk 10.15

namespace DDSWeb
{
    /// <summary>
    /// Summary description for LogWriter.
    /// </summary>
    public class LogWriter
    {
        SqlConnection conn;
        System.Collections.ArrayList coll;
        string sTableName;
        ConvCom m_conv;
        string m_sLang;
        //string sNow;
        System.DateTime dNow;
        int LongString = 3500; //vk 07.07
        string sSite;
        string sVersion;
        Reader rd;
        Reader rdVer;
        long nLocalCounter; //vk 08.05
        //string sErrorText = ""; //vk 11.05
        Exception eException = null; //vk 10.15
        long nHoursForLog; //vk 01.07
        string[] indlist = { "TAR", "NOIN", "LTXT", "STXT" }; //vk 02.14
        string m_sPath; //vk 10.15
        StringBuilder sb;

        class MyField
        {
            public string sName;
            public object oValue;
        }
        public LogWriter(string sDb, string sPath, string sConnStrAddition, bool bTables) //vk 11.05
        {
            try
            {
                m_sPath = sPath; //vk 10.15
                rd = new Reader(sPath + "\\Defprop.config"); //vk 11.05
                PocketKnife.Info pp = new PocketKnife.Info();
                if (sDb == "") sDb = rd.getProperty("DbForLog");

                if (rd.getProperty("SqlUserTables") == "") //ntg 10.07.24 vladi's change regarding sql migration for bituah haklai
                    conn = new SqlConnection(
                    "Data Source=" + rd.getProperty("SqlServer") +
                    ";Initial Catalog=" + sDb.Trim() +
                    ";Integrated Security=true;" + sConnStrAddition
                    );
                else
                    conn = new SqlConnection(
                    "Data Source=" + rd.getProperty("SqlServer") +
                    ";Initial Catalog=" + sDb.Trim() +
                    ";User ID=" + rd.getProperty("SqlUserTables") +
                    ";Password=" + pp.DecryptPassword(rd.getProperty("SqlPassTables")) + ";" + sConnStrAddition
                    ); //vk 03.08
                conn.Open();
                m_conv = new ConvCom();
                pp = null;
                //vk 07.08
                string s = rd.getProperty("LogMaxString");
                if (s != "")
                    LongString = Int32.Parse(s);
            }
            catch (Exception ee)
            {
                eException = ee;
                //vk 10.15
                sb = new StringBuilder();
                sb.AppendLine("connecting to DB: " + sDb);
                TextLog(ee, sb.ToString());
                sb = null;
            }
        }
        public LogWriter(string sDb, string sPath, string sConnStrAddition)
        {
            try
            {
                m_sPath = sPath; //vk 10.15
                rd = new Reader(sPath + "\\Defprop.config"); //vk 11.05
                rdVer = new Reader(sPath + "\\Version.txt");
                PocketKnife.Info pp = new PocketKnife.Info();
                if (sDb == "") sDb = rd.getProperty("DbForLog");
                string sConn = "";
                if (rd.getProperty("SqlUserTables") == "") //ntg 10.07.24 vladi's change regarding sql migration for bituah haklai
                    sConn = "Data Source=" + rd.getProperty("SqlServer") +
                    ";Initial Catalog=" + sDb.Trim() +
                    ";Integrated Security=true;" + sConnStrAddition;
                else
                    sConn = "Data Source=" + rd.getProperty("SqlServer") +
                    ";Initial Catalog=" + sDb +
                    ";User ID=" + rd.getProperty("SqlUser") +
                    ";Password=" + pp.DecryptPassword(rd.getProperty("SqlPass")) + ";" + sConnStrAddition;

                //";Trusted_Connection=True";
                //PocketKnife.Info o = new PocketKnife.Info();
                //bool bImpersonated = o.impersonateValidUser(rd, "SQL_");

                conn = new SqlConnection(sConn); //vk 03.08
                sConn = "";
                conn.Open();
                m_conv = new ConvCom();
                sSite = MyTrim(rd.getProperty("Site"), 50);
                sVersion = MyTrim(rdVer.getProperty("AppVersion"), 50);
                pp = null;
                dNow = System.DateTime.Now; //vk 08.05
                //vk 01.07
                string s = rd.getProperty("HoursForLog");
                if (s == "")
                    nHoursForLog = 0;
                else
                    nHoursForLog = Int64.Parse(s);
                //vk 07.08
                s = rd.getProperty("LogMaxString");
                if (s != "")
                    LongString = Int32.Parse(s);
            }
            catch (Exception ee)
            {
                eException = ee;
                //vk 10.15
                sb = new StringBuilder();
                sb.AppendLine("connecting to DB: " + sDb);
                TextLog(ee, sb.ToString());
                sb = null;
            }
        }

        //public void MakeTable(System.Web.SessionState.HttpSessionState pSes, System.Web.HttpRequest pReq,
        //    string sTable, string sThisUser, string sUsualUser, string sAddUsers)
        //{ //vk 09.14 compatibility
        //    MakeTable(pSes, pReq, sTable, sThisUser, sUsualUser, sAddUsers, false);
        //}
        public void MakeTable(System.Web.SessionState.HttpSessionState pSes, System.Web.HttpRequest pReq,
            string sTable, string sThisUser, string sUsualUser, string sAddUsers) //vk 01.05 //ik 12.11
        {
            string stCommand = "CREATE TABLE [dbo].[" + sTable + "] (";
            //string Collate="";
            //if (Collate1.Trim()!="") Collate = "COLLATE " + Collate1;
            if (rd.getProperty("TablesWithId").ToLower() == "true")
                stCommand += "Id Int Identity(1, 1),"; //vk 08.22

            SqlCommand cmd = null;
            switch (sTable)
            {
                case "tblFlex":
                    stCommand
                    += " [FLKOD] [numeric](2, 0) NOT NULL,"
                    + " [FLTBNO] [numeric](3, 0) NOT NULL,"
                    + " [FLNOIN] [numeric](9, 0) NOT NULL,"
                    + " [FLSTXT] [nchar] (20) NULL,"
                    + " [FLLTXT] [nchar] (60) NULL,"
                    + " [CLSLNK] [numeric](3, 0) NULL,"
                    + " [ICONID] [numeric](3, 0) NULL,"
                    + " [CLSNUM] [numeric](4, 0) NULL,"
                    + " [FIELD3] [numeric](3, 0) NULL,"
                    + " [FIELD4] [nchar] (30) NULL,"
                    + " [FLSTRV] [nchar] (10) NULL,"
                    + " [FLLTRV] [nchar] (30) NULL,"
                    + " [FLANAF] [numeric](3, 0) NULL,"
                    + " [FLMHOZ] [numeric](2, 0) NULL,"
                    + " [FLLIDR] [numeric](2, 0) NULL,"
                    + " [FLTART] [numeric](8, 0) NULL,"
                    + " [MATBEAPC] [numeric](2, 0) NULL,"
                    + " [FLKYML] [char] (1) NULL,"
                    + " [FLMLL1] [nchar] (25) NULL,"
                    + " [FLMLL2] [nchar] (25) NULL,"
                    + " [FLMLL3] [nchar] (25) NULL,"
                    + " [FLFIL] [char] (100) NULL,"
                    + " [FLTAR] [int] NOT NULL,"
                    + " [FLFRDT] [int] NOT NULL,"
                    + " [FLTODT] [int] NOT NULL)"; //nchar vk 05.05; 3 fields vk 07.05; not null vk 02.14
                    break;
                case "tblHelp":
                    stCommand
                    += " [HPANAF] [numeric](3, 0) NOT NULL,"
                    + " [HPVERS] [numeric](4, 0) NOT NULL,"
                    + " [HPFIELD] [numeric](9, 0) NOT NULL,"
                    + " [HPTYPE] [numeric](2, 0) NOT NULL,"
                    + " [HPNUMB] [numeric](2, 0) NOT NULL,"
                    + " [HPTEXT] [nchar] (150) NULL)";
                    break;
                case "tblCities":
                    stCommand
                    += " [sCode] [char] (5) NOT NULL,"
                    + " [sText] [nvarchar] (60) NULL)";
                    break;
                case "tblStreets":
                    stCommand
                    += " [sCode] [char] (5) NOT NULL,"
                    + " [sCity] [char] (5) NOT NULL,"
                    + " [sText] [nvarchar] (60) NULL)";
                    break;
                case "tb156": //vk 12.05
                    stCommand
                    += " [U56MNFN] [decimal](3, 0) NOT NULL,"
                    + " [U56IZR] [decimal](4, 0) NOT NULL,"
                    + " [U56DGM] [decimal](7, 0) NOT NULL,"
                    + " [U56TRN] [decimal](1, 0) NOT NULL,"
                    + " [U56SMK] [decimal](5, 0) NULL,"
                    + " [U56NAME] [nvarchar] (80) NULL)";
                    break;
                case "tblAccUsr": //vk 02.06, 05.13
                    stCommand
                    += " [sUser] [varchar] (10) NOT NULL,"
                    + " [sPermit] [char] (1) NOT NULL,"
                    + " [sWebByUser] [char] (1) NOT NULL)";
                    break;
                case "tblConverse": //ik 01.10
                    stCommand
                    += " [Company] [int] NOT NULL,"
                    + " [Lob] [int] NOT NULL,"
                    + " [Activity] [int] NOT NULL,"
                    + " [TafnitBufPosition] [int] NOT NULL,"
                    + " [TafnitBufPosition2] [int] NOT NULL,"
                    + " [TafnitBufPosition3] [int] NOT NULL,"
                    + " [NodeNameSource] [varchar](50) NULL,"
                    + " [NodeName] [varchar](50) NOT NULL,"
                    + " [FieldDescription] [nvarchar](50) NULL,"
                    + " [TafnitFieldType] [char](1) NULL,"
                    + " [TafnitLeftRight] [char](1) NULL,"
                    + " [TafnitSize] [int] NULL,"
                    + " [TafnitDecimal] [int] NULL,"
                    + " [Direction] [char](1) NOT NULL,"
                    + " [Translation] [int] NULL,"
                    + " [ExternalFieldType] [char](1) NULL,"
                    + " [ExternalLeftRight] [char](1) NULL,"
                    + " [ExternalSize] [int] NULL,"
                    + " [ExternalDecimal] [int] NULL,"
                    + " [ExternalPosition] [int] NULL,"
                    + " [TafnitHebrew] [char](1) NULL,"
                    + " [Thinning] [char](1) NULL)";
                    break;
                case "tblFlexBuffer": //vk 04.15
                    stCommand
                    += " [Interface] [char](1) NOT NULL,"
                    + " [System] [char](1) NOT NULL,"
                    + " [SubSystem] [int] NOT NULL,"
                    + " [Action] [int] NOT NULL,"
                    + " [TafnitBufPosition] [int] NOT NULL,"
                    + " [TafnitBufPosition2] [int] NOT NULL,"
                    + " [TafnitBufPosition3] [int] NOT NULL,"
                    + " [NodeNameSource] [varchar](50) NULL,"
                    + " [NodeName] [varchar](50) NOT NULL,"
                    + " [FieldDescription] [nvarchar](50) NULL,"
                    + " [TafnitFieldType] [char](1) NULL,"
                    + " [TafnitLeftRight] [char](1) NULL,"
                    + " [TafnitSize] [int] NULL,"
                    + " [TafnitDecimal] [int] NULL,"
                    + " [Direction] [char](1) NOT NULL,"
                    + " [Translation] [int] NULL,"
                    + " [ExternalFieldType] [char](1) NULL,"
                    + " [ExternalLeftRight] [char](1) NULL,"
                    + " [ExternalSize] [int] NULL,"
                    + " [ExternalDecimal] [int] NULL,"
                    + " [ExternalPosition] [int] NULL,"
                    + " [TafnitHebrew] [char](1) NULL,"
                    + " [Thinning] [char](1) NULL,"
                    + " [CobolName] [char](36) NULL)";
                    break;
                case "tblVendor": //ik 12.11
                    stCommand
                    += " [VendorCode] [nvarchar](3) NOT NULL,"
                    + " [VendorDesc] [nvarchar](12) NULL)";
                    break;
                case "tblCategory": //ik 12.11
                    stCommand
                    += " [VendorCode] [nvarchar](3) NOT NULL,"
                    + " [CategoryDesc] [nvarchar](24) NULL,"
                    + " [CategoryCode] [nvarchar](10) NOT NULL)";
                    break;
                case "tblModel": //ik 12.11
                    stCommand
                    += " [ModelCode] [int] NOT NULL,"
                    + " [ModelDesc] [nvarchar](24) NULL,"
                    + " [CategoryCode] [nvarchar](10) NULL,"
                    + " [ModelFirstYear] [int] NULL,"
                    + " [ModelLastYear] [int] NULL)";
                    break;
                case "tb398": //vk 09.24
                    stCommand
                    += " [U398LDR] [decimal](2, 0) NOT NULL,"
                    + " [U398TAV] [decimal](3, 0) NOT NULL,"
                    + " [U398TBL] [decimal](3, 0) NOT NULL,"
                    + " [U398DAT] [decimal](8, 0) NOT NULL,"
                    + " [U398RAM] [decimal](1, 0) NOT NULL,"
                    + " [U398K01] [decimal](5, 0) NOT NULL,"
                    + " [U398K02] [decimal](5, 0) NOT NULL,"
                    + " [U398K03] [decimal](5, 0) NOT NULL,"
                    + " [U398NAM] [nvarchar](40) NULL,"
                    + " [U398NMA] [nvarchar](40) NULL,"
                    + " [U398NMT] [nvarchar](30) NULL,"
                    + " [U398RM1] [nvarchar](15) NULL,"
                    + " [U398RM2] [nvarchar](15) NULL,"
                    + " [U398RM3] [nvarchar](15) NULL,"
                    + " [U398FIL] [nvarchar](200) NULL)";
                    break;
            }

            //vk 06.23 - ntg 06.07.23 - added this if
            if (rd.getProperty("Before_" + sTable) != "")
            {
                try
                {
                    cmd = new SqlCommand(rd.getProperty("Before_" + sTable), conn);
                    cmd.CommandType = CommandType.Text;
                    Exec(cmd);
                }
                catch (Exception e)
                {
                    LogError(e.Message, pSes, pReq);
                }
            }

            if (sTable == "tblFlex")
            {
                foreach (string v in indlist)
                {
                    try
                    {
                        //vk 02.14
                        cmd = new SqlCommand(
                            "IF EXISTS (SELECT * FROM sys.indexes "
                            + "WHERE object_id = OBJECT_ID(N'[dbo].[tblFlex]') "
                            + "AND name = N'IX_tblFlex_" + v + "') "
                            + "DROP INDEX [IX_tblFlex_" + v + "] "
                            + "ON [dbo].[tblFlex] WITH ( ONLINE = OFF )", conn);
                        cmd.CommandType = CommandType.Text;
                        Exec(cmd);
                    }
                    catch
                    {
                    }
                }
            }
            try
            {
                cmd = new SqlCommand("DROP TABLE [dbo].[" + sTable + "]", conn);
                cmd.CommandType = CommandType.Text;
                Exec(cmd);
            }
            catch
            {
            }
            try
            {
                cmd = new SqlCommand(stCommand, conn);
                cmd.CommandType = CommandType.Text;
                Exec(cmd);
            }
            catch (Exception e)
            {
                LogError(e.Message, pSes, pReq);
                //vk 05.13
                if (cmd != null) cmd.Dispose();
                cmd = null;
                return;
            }
            //vk 11.05
            try
            {
                cmd = new SqlCommand("GRANT SELECT ON [dbo].[" + sTable + "] TO [" + sUsualUser + "]", conn);
                cmd.CommandType = CommandType.Text;
                Exec(cmd);

                //ik 12.11
                string[] arrUsers = sAddUsers.Split(';');
                foreach (string sUser in arrUsers)
                {
                    string sCurUser = sUser.Trim();
                    if (sCurUser.Length > 0)
                    {
                        cmd = new SqlCommand("if not exists (select * from dbo.sysusers where name ='" + sCurUser + "' and uid < 16382) EXEC sp_grantdbaccess '" + sCurUser + "', '" + sCurUser + "'", conn);
                        cmd.CommandType = CommandType.Text;
                        Exec(cmd);
                        cmd = new SqlCommand("GRANT SELECT ON [dbo].[" + sTable + "] TO [" + sCurUser + "]", conn);
                        cmd.CommandType = CommandType.Text;
                        Exec(cmd);
                    }
                }
            }
            catch (Exception e)
            {
                LogError(e.Message, pSes, pReq);
                //vk 05.13
                if (cmd != null) cmd.Dispose();
                cmd = null;
                return;
            }
            try
            {
                cmd = new SqlCommand("GRANT REFERENCES, SELECT, INSERT ON [dbo].[" + sTable + "] TO [" + sThisUser + "]", conn);
                cmd.CommandType = CommandType.Text;
                Exec(cmd);
            }
            catch (Exception e)
            {
                LogError(e.Message, pSes, pReq);
            }
            if (cmd != null) cmd.Dispose();
            cmd = null;
        }
        public void MakeKeys(System.Web.SessionState.HttpSessionState pSes, System.Web.HttpRequest pReq,
            string sTable, bool bNew) //vk 02.14
        {
            bool bMustDo = true; //vk 09.24
            string stCommand = "ALTER TABLE [dbo].[" + sTable + "] ADD CONSTRAINT [PK_" + sTable + "] PRIMARY KEY CLUSTERED (";

            SqlCommand cmd = null;
            switch (sTable)
            {
                case "tblFlex":
                    stCommand += "[FLKOD] ASC, [FLTBNO] ASC, [FLNOIN] ASC";
                    if (bNew)
                        stCommand += ", [FLTAR] ASC";
                    break;
                case "tblHelp":
                    stCommand += "[HPANAF] ASC, [HPVERS] ASC, [HPFIELD] ASC, [HPTYPE] ASC, [HPNUMB] ASC";
                    break;
                case "tblCities":
                    stCommand += "[sCode] ASC";
                    bMustDo = false; //vk 09.24
                    break;
                case "tblStreets":
                    stCommand += "[sCode] ASC, [sCity] ASC";
                    bMustDo = false; //vk 09.24
                    break;
                case "tb156": //vk 12.05
                    stCommand += "[U56MNFN] ASC, [U56IZR] ASC, [U56DGM] ASC, [U56TRN] ASC";
                    break;
                case "tblAccUsr": //vk 02.06, 05.13
                    stCommand += "[sUser] ASC";
                    break;
                case "tblConverse": //ik 01.10
                    stCommand += "[Company] ASC, [Lob] ASC, [Activity] ASC, [Direction] ASC, [NodeName] ASC";
                    break;
                case "tblFlexBuffer": //vk 04.15
                    stCommand += "[Interface] ASC, [System] ASC, [SubSystem] ASC, [Direction] ASC, [NodeName] ASC";
                    break;
                case "tblVendor": //ik 12.11
                    stCommand += "[VendorCode] ASC";
                    break;
                case "tblCategory": //ik 12.11
                    stCommand += "[VendorCode] ASC, [CategoryCode] ASC";
                    break;
                case "tblModel": //ik 12.11
                    stCommand += "[ModelCode] ASC";
                    break;
                case "tb398": //vk 09.24
                    stCommand += "[U398LDR] ASC, [U398TAV] ASC, [U398TBL] ASC, [U398DAT] ASC, [U398RAM] ASC, [U398K01] ASC, [U398K02] ASC, [U398K03] ASC";
                    break;
            }
            stCommand += ")WITH (PAD_INDEX  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]";
            if (bMustDo) //vk 09.24
            {
                try
                {
                    cmd = new SqlCommand(stCommand, conn);
                    cmd.CommandType = CommandType.Text;
                    Exec(cmd);
                }
                catch (Exception e)
                {
                    LogError(e.Message, pSes, pReq);
                    //vk 05.13
                    if (cmd != null) cmd.Dispose();
                    cmd = null;
                    return;
                }
            }
            if (sTable == "tblFlex")
            {
                foreach (string v in indlist)
                {
                    if (bNew || (v != "TAR" && v != "NOIN"))
                    {
                        stCommand = "CREATE NONCLUSTERED INDEX [IX_tblFlex_" + v + "] ON [dbo].[tblFlex] (";
                        stCommand += "[FLKOD] ASC, [FLTBNO] ASC";
                        if (bNew)
                            stCommand += ", [FLTAR] ASC";
                        if (v != "TAR" && bNew)
                            stCommand += ", [FLFRDT] ASC, [FLTODT] ASC";
                        if (v != "TAR")
                            stCommand += ", [FL" + v + "] ASC";
                        stCommand += ")WITH (PAD_INDEX  = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]";
                        try
                        {
                            cmd = new SqlCommand(stCommand, conn);
                            cmd.CommandType = CommandType.Text;
                            Exec(cmd);
                        }
                        catch (Exception e)
                        {
                            LogError(e.Message, pSes, pReq);
                            //vk 05.13
                            if (cmd != null) cmd.Dispose();
                            cmd = null;
                            return;
                        }
                    }
                }
            }

          
            //vk 06.23 - ntg 06.07.23 - added this if
            if (rd.getProperty("After_" + sTable) != "")
            {
                try
                {
                    cmd = new SqlCommand(rd.getProperty("After_" + sTable), conn);
                    cmd.CommandType = CommandType.Text;
                    Exec(cmd);
                }
                catch (Exception e)
                {
                    LogError(e.Message, pSes, pReq);
                }
            }


            if (cmd != null) cmd.Dispose();
            cmd = null;
        }
        public void ClearTable(System.Web.SessionState.HttpSessionState pSes, System.Web.HttpRequest pReq,
            string sTable, string sWhere) //vk 09.06
        {
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("DELETE FROM [dbo].[" + sTable + "] WHERE " + sWhere, conn);
                cmd.CommandType = CommandType.Text;
                Exec(cmd);
            }
            catch (Exception e)
            {
                LogError(e.Message, pSes, pReq);
            }
            if (cmd != null) cmd.Dispose();
            cmd = null;
        }
        public void SetLocalCounter(System.Web.SessionState.HttpSessionState pSession)
        {
            nLocalCounter = Convert.ToInt64(pSession["LocalCounter"]);
            nLocalCounter += 1;
            pSession["LocalCounter"] = nLocalCounter;
        }
        public void SetLocalCounter(ref long pLocalCounter)
        {
            pLocalCounter += 1;
            nLocalCounter = pLocalCounter;
        }
        //public void SetLocalCounter(System.Web.SessionState.HttpSessionState pSession)
        //{
        //    nLocalCounter = Convert.ToInt64(pSession["LocalCounter"]) + 1;
        //    pSession["LocalCounter"] = nLocalCounter;
        //    ?SetFieldValue("nLocalCounter", nLocalCounter);
        //}
        private bool MustWriteLine(string sParam, string sUser) //vk 10.19
        {
            if (sParam == "")
                return true;
            switch (rd.getProperty(sParam))
            {
                case "false":
                    return false;
                case "true":
                    return true;
                default:
                    return (";" + rd.getProperty(sParam).ToLower() + ";").IndexOf(";" + sUser.ToLower() + ";") >= 0;
            }
        }
        public void WriteLine(
            string sEvent, string sHost, string sXml,
            System.Web.SessionState.HttpSessionState pSession,
            string sUser, string sSynchr, string sMq, string sDq, Exception e) //vk 09.04
        {
            //if (rd.getProperty("Log_Error") != "true") return;
            if (!MustWriteLine("Log_Error", (string)pSession["User"])) return;
            try
            {
                InitOnce("tblLog1", true, pSession.SessionID);
                WriteLineOnce(sEvent, MyTrim(sHost, 50), sUser, sSynchr, sDq, "");
                SetFieldValue("sExSource", "-");
                //SetLocalCounter(pSession);
                SetFieldValue("nLocalCounter", nLocalCounter);
                SubmitOnce();
                if (eException != null) throw eException; //vk 10.15

                WriteLong("err", sXml, pSession.SessionID);
                //vk 08.06
                int i = 0;
                for (Exception ee = e; ee != null; ee = ee.InnerException)
                {
                    i++;
                    //vk 01.08
                    if (ee.StackTrace != null)
                        WriteLong("st" + i.ToString(), ee.StackTrace, pSession.SessionID);
                    if (ee.Message != null)
                        WriteLong("ms" + i.ToString(), ee.Message, pSession.SessionID); //vk 01.07
                    if (ee.Source != null)
                        WriteLong("so" + i.ToString(), ee.Source, pSession.SessionID); //vk 01.07
                }
                WriteLong("xml", (string)pSession["PageXML"], pSession.SessionID); //vk 12.11
                if (eException != null) throw eException; //vk 10.15
            }
            catch (Exception ee)
            {
                eException = ee;
                //vk 10.15, 08.21
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("session: " + pSession.SessionID);
                sb.AppendLine("host: " + sHost);
                sb.AppendLine("user: " + sUser);
                sb.AppendLine("event: " + sEvent);
                sb.AppendLine("text: " + sXml);
                sb.AppendLine("dq: " + sDq);
                sb.AppendLine("exception: ");
                sb.AppendLine(ExceptionText(e));
                TextLog(ee, sb.ToString());
                sb = null;
            }
        }
        public void WriteLine(
            string sEvent, string sHost, string sXml,
            System.Web.SessionState.HttpSessionState pSession,
            string sUser, string sSynchr, string sMq, string sDq, string sParam) //vk 12.04
        {
            //if (sParam != "" && rd.getProperty(sParam) != "true") return;
            if (!MustWriteLine(sParam, (string)pSession["User"])) return;
            try
            {
                InitOnce("tblLog1", true, pSession.SessionID);
                WriteLineOnce(sEvent, MyTrim(sHost, 50), sUser, sSynchr, sDq, "");
                //SetLocalCounter(pSession);
                SetFieldValue("nLocalCounter", nLocalCounter);
                SubmitOnce();
                if (eException != null) throw eException; //vk 10.15
                if (sXml != "")
                    WriteLong(sEvent.Contains("ERROR") || sParam == "Log_Error" ? "err" : "xml", sXml, pSession.SessionID);
                if (sEvent.Contains("ERROR") || sParam == "Log_Error")
                    WriteLong("xml", (string)pSession["PageXML"], pSession.SessionID); //vk 12.11
                if (eException != null) throw eException; //vk 10.15
            }
            catch (Exception ee)
            {
                eException = ee;
                //vk 10.15
                sb = new StringBuilder();
                sb.AppendLine("session: " + pSession.SessionID);
                sb.AppendLine("host: " + sHost);
                sb.AppendLine("user: " + sUser);
                sb.AppendLine("event: " + sEvent);
                sb.AppendLine("text: " + sXml);
                sb.AppendLine("dq: " + sDq);
                TextLog(ee, sb.ToString());
                sb = null;
            }
        }
        public void WriteLine(
            string sEvent, string sHost, string sXml,
            System.Web.SessionState.HttpSessionState pSession,
            string sUser, string sSynchr, string sMq, string sDq, string sScreen, string sParam) //vk 08.05
        {
            //if (sParam != "" && rd.getProperty(sParam) != "true") return;
            if (!MustWriteLine(sParam, (string)pSession["User"])) return;
            try
            {
                InitOnce("tblLog1", true, pSession.SessionID);
                WriteLineOnce(sEvent, MyTrim(sHost, 50), sUser, sSynchr, sDq, sScreen);
                //SetLocalCounter(pSession);
                SetFieldValue("nLocalCounter", nLocalCounter);
                SubmitOnce();
                if (eException != null) throw eException; //vk 10.15
                if (sXml != "")
                    WriteLong(sEvent.Contains("ERROR") || sParam == "Log_Error" ? "err" : "xml", sXml, pSession.SessionID);
                if (sEvent.Contains("ERROR") || sParam == "Log_Error")
                    WriteLong("xml", (string)pSession["PageXML"], pSession.SessionID); //vk 12.11
                if (eException != null) throw eException; //vk 10.15
            }
            catch (Exception ee)
            {
                eException = ee;
                //vk 10.15
                sb = new StringBuilder();
                sb.AppendLine("session: " + pSession.SessionID);
                sb.AppendLine("host: " + sHost);
                sb.AppendLine("user: " + sUser);
                sb.AppendLine("event: " + sEvent);
                sb.AppendLine("text: " + sXml);
                sb.AppendLine("dq: " + sDq);
                TextLog(ee, sb.ToString());
                sb = null;
            }
        }
        public void WriteLine(string sEvent, Exception e, System.Web.SessionState.HttpSessionState pSession) //vk 12.11
        {
            //if (rd.getProperty("Log_Error") != "true") return;
            if (!MustWriteLine("Log_Error", (string)pSession["User"])) return;
            try
            {
                InitOnce("tblLog1", true, "Application");
                WriteLineOnce(sEvent, "", "", "", "", "");
                SetFieldValue("sExSource", "-");
                SetLocalCounter(pSession);
                SubmitOnce();
                if (eException != null) throw eException; //vk 10.15

                //vk 08.06
                int i = 0;
                for (Exception ee = e; ee != null; ee = ee.InnerException)
                {
                    i++;
                    //vk 01.08
                    if (ee.StackTrace != null)
                        WriteLong("st" + i.ToString(), ee.StackTrace, "Application");
                    if (ee.Message != null)
                        WriteLong("ms" + i.ToString(), ee.Message, "Application"); //vk 01.07
                    if (ee.Source != null)
                        WriteLong("so" + i.ToString(), ee.Source, "Application"); //vk 01.07
                }
                if (eException != null) throw eException; //vk 10.15
            }
            catch (Exception ee)
            {
                eException = ee;
                //vk 10.15
                sb = new StringBuilder();
                sb.AppendLine("event: " + sEvent);
                sb.AppendLine("exception: ");
                sb.AppendLine(ExceptionText(e));
                TextLog(ee, sb.ToString());
                sb = null;
            }
        }
        public void WriteLine(string sEvent, string sXml, System.Web.SessionState.HttpSessionState pSession) //vk 12.11
        {
            //if (rd.getProperty("Log_StartEnd") != "true") return;
            if (!MustWriteLine("Log_StartEnd", (string)pSession["User"])) return;
            try
            {
                //InitOnce("tblLog1", true, "Application");
                InitOnce("tblLog1", true, pSession.SessionID); //ntg 30.04.24 vladi change regarding CG functionallity
                WriteLineOnce(sEvent, "", "", "", "", "");
                //SetLocalCounter(pSession);
                SetFieldValue("nLocalCounter", nLocalCounter);
                SubmitOnce();
                if (eException != null) throw eException; //vk 10.15
                if (sXml != "")
                    //WriteLong(sEvent.Contains("ERROR") ? "err" : "xml", sXml, "Application");
                    WriteLong(sEvent.Contains("ERROR") ? "err" : "xml", sXml, pSession.SessionID); //ntg 30.04.24 vladi change regarding CG functionallity
                if (eException != null) throw eException; //vk 10.15
            }
            catch (Exception ee)
            {
                eException = ee;
                //vk 10.15
                sb = new StringBuilder();
                sb.AppendLine("event: " + sEvent);
                sb.AppendLine("text: " + sXml);
                TextLog(ee, sb.ToString());
                sb = null;
            }
        }
        public void WriteLine(string sEvent, string sXml)
        {
            if (rd.getProperty("Log_StartEnd") != "true") return;
            try
            {
                InitOnce("tblLog1", true, "Application");
                WriteLineOnce(sEvent, "", "", "", "", "");
                //SetLocalCounter(pSession);
                SetFieldValue("nLocalCounter", nLocalCounter);
                SubmitOnce();
                if (eException != null) throw eException; //vk 10.15
                if (sXml != "")
                    WriteLong(sEvent.Contains("ERROR") ? "err" : "xml", sXml, "Application");
                if (eException != null) throw eException; //vk 10.15
            }
            catch (Exception ee)
            {
                eException = ee;
                //vk 10.15
                sb = new StringBuilder();
                sb.AppendLine("event: " + sEvent);
                sb.AppendLine("text: " + sXml);
                TextLog(ee, sb.ToString());
                sb = null;
            }
        }
        void TextLog(Exception e, string sText) //vk 10.15
        {
            //try
            //{
            string sPlace = m_sPath + "\\TextLog\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            PocketKnife.Fq oFq = new PocketKnife.Fq(sPlace, PocketKnife.Fq.FileType.Append);
            oFq.Send("===> " + dNow.ToString("dd/MM/yyyy HH:mm:ss"));
            oFq.Send(sText);
            oFq.Send("SQL exception:");
            oFq.Send(ExceptionText(e));
            oFq.Send("");
            oFq.Dispose();
            oFq = null;
            //}
            //catch (Exception ee) { ee = ee; }
        }
        string ExceptionText(Exception e) //vk 10.15
        {
            try
            {
                StringBuilder sb = new StringBuilder(); //local variable to avoid conflicts with the module-level one
                bool bInner = false;
                for (Exception ee = e; ee != null; ee = ee.InnerException)
                {
                    if (bInner) sb.AppendLine("and its inner exception:");
                    if (ee.Source != null) sb.AppendLine(ee.Source);
                    if (ee.Message != null) sb.AppendLine(ee.Message);
                    if (ee.StackTrace != null) sb.AppendLine(ee.StackTrace);
                    bInner = true;
                }
                return sb.ToString();
            }
            finally
            {
                sb = null;
            }
        }
        public void WriteLine_Tap(System.Web.SessionState.HttpSessionState pSes, System.Web.HttpRequest pReq,
            string sTable, string sData, string flr, bool bNew) //vk 01.05
        {
            string s = "*";
            eException = null;
            try
            {
                switch (sTable)
                {
                    case "tblCities":
                    case "tblStreets":
                        s = sData.Substring(30, 30).Trim();
                        if (s == "")
                            return; //vk 09.06
                        break;
                }
                InitOnce_Simple(sTable, false, false);
                int nShift;
                switch (sTable)
                {
                    case "tblFlex":
                        nShift = (bNew ? 8 : 0); //vk 07.05
                        string rus = sData.Substring(177 + nShift, 3);
                        SetFieldValue("FLKOD", nz(sData.Substring(0, 2).Trim()));
                        SetFieldValue("FLTBNO", nz(sData.Substring(2, 3).Trim()));
                        SetFieldValue("FLNOIN", nz(sData.Substring(5 + nShift, 9).Trim()));
                        SetFieldValue("FLSTXT", AdjustLang(sData.Substring(14 + nShift, 40), flr, rus, 10, true, false)); //vk 05.20
                        SetFieldValue("FLLTXT", AdjustLang(sData.Substring(54 + nShift, 120), flr, rus, 30, true, false)); //vk 05.20
                        SetFieldValue("CLSLNK", sData.Substring(174 + nShift, 3).Trim());
                        SetFieldValue("ICONID", nz(rus.Trim()));
                        SetFieldValue("CLSNUM", nz(sData.Substring(180 + nShift, 4).Trim()));
                        SetFieldValue("FIELD3", nz(sData.Substring(184 + nShift, 3).Trim()));
                        SetFieldValue("FIELD4", AdjustLang(sData.Substring(187 + nShift, 120), flr, rus, 30, true, false));
                        SetFieldValue("FLSTRV", AdjustLang(sData.Substring(307 + nShift, 40), flr, rus, 10, true, false));
                        SetFieldValue("FLLTRV", AdjustLang(sData.Substring(347 + nShift, 120), flr, rus, 30, true, false));
                        SetFieldValue("FLANAF", nz(sData.Substring(467 + nShift, 3).Trim()));
                        SetFieldValue("FLMHOZ", nz(sData.Substring(470 + nShift, 2).Trim()));
                        SetFieldValue("FLLIDR", nz(sData.Substring(472 + nShift, 2).Trim()));
                        SetFieldValue("FLTART", nz(sData.Substring(474 + nShift, 8).Trim()));
                        SetFieldValue("FLKYML", nz(sData.Substring(482 + nShift, 1).Trim()));
                        SetFieldValue("FLMLL1", AdjustLang(sData.Substring(483 + nShift, 100), flr, rus, 25, false, false));
                        SetFieldValue("FLMLL2", AdjustLang(sData.Substring(583 + nShift, 100), flr, rus, 25, false, false));
                        SetFieldValue("FLMLL3", AdjustLang(sData.Substring(683 + nShift, 100), flr, rus, 25, false, false));
                        if (bNew)
                        {
                            SetFieldValue("FLTAR", nz(sData.Substring(5, 8).Trim()));
                            SetFieldValue("FLFRDT", nz(sData.Substring(783 + 8, 8).Trim()));
                            SetFieldValue("FLTODT", nz(sData.Substring(783 + 16, 8).Trim()));
                            SetFieldValue("FLFIL", sData.Substring(783 + 24, 100).Trim());
                        }
                        else
                        {
                            //vk 02.14
                            SetFieldValue("FLTAR", "99999999");
                            SetFieldValue("FLFRDT", "00000000");
                            SetFieldValue("FLTODT", "99999999");
                            SetFieldValue("FLFIL", sData.Substring(783, 100).Trim());
                        }
                        break;
                    case "tblHelp":
                        SetFieldValue("HPANAF", nz(sData.Substring(0, 3).Trim()));
                        SetFieldValue("HPVERS", nz(sData.Substring(3, 4).Trim()));
                        SetFieldValue("HPFIELD", nz(sData.Substring(7, 9).Trim()));
                        SetFieldValue("HPTYPE", nz(sData.Substring(16, 2).Trim()));
                        SetFieldValue("HPNUMB", nz(sData.Substring(18, 2).Trim()));
                        SetFieldValue("HPTEXT", AdjustLang(sData.Substring(20, 75), flr, "000", 75, true, false));
                        break;
                    case "tblCities":
                        SetFieldValue("sCode", sData.Substring(5, 5));
                        SetFieldValue("sText", AdjustLang(s.Replace("`", "'"), flr, "000", 30, false, false));
                        break;
                    case "tblStreets":
                        SetFieldValue("sCity", sData.Substring(5, 5));
                        SetFieldValue("sCode", sData.Substring(10, 5));
                        SetFieldValue("sText", AdjustLang(s.Replace("`", "'"), flr, "000", 30, false, false));
                        break;
                    case "tb156": //vk 12.05
                        SetFieldValue("U56MNFN", nz(sData.Substring(5, 3).Trim()));
                        SetFieldValue("U56IZR", nz(sData.Substring(8, 4).Trim()));
                        SetFieldValue("U56DGM", nz(sData.Substring(12, 7).Trim()));
                        SetFieldValue("U56TRN", nz(sData.Substring(19, 1).Trim()));
                        SetFieldValue("U56SMK", nz(sData.Substring(30, 5).Trim()));
                        SetFieldValue("U56NAME", AdjustLang(sData.Substring(57, 40), flr, "000", 40, true, false));
                        break;
                    case "tblAccUsr": //vk 02.06
                        SetFieldValue("sUser", sData.Substring(0, 10).Trim());
                        SetFieldValue("sPermit", sData.Substring(10, 1));
                        SetFieldValue("sWebByUser", sData.Substring(11, 1)); //vk 05.13
                        break;
                    case "tblMemory": //vk 09.06
                        SetFieldValue("FLGUID", pSes["Guid"]);
                        SetFieldValue("FLKOD", nz(sData.Substring(0, 2).Trim()));
                        SetFieldValue("FLTBNO", nz(sData.Substring(2, 3).Trim()));
                        SetFieldValue("FLNOIN", nz(sData.Substring(5, 9).Trim()));
                        SetFieldValue("FLMTXT", AdjustLang(sData.Substring(14, 60), flr, "000", 60, true, false));
                        SetFieldValue("FLDEL", nz(sData.Substring(74, 1).Trim()));
                        break;
                    case "tblConverse": //ik 01.10
                    case "tblFlexBuffer": //vk 04.15
                        if (sTable == "tblConverse")
                        {
                            nShift = 0;
                            SetFieldValue("Company", nz(sData.Substring(0, 3).Trim()));
                            SetFieldValue("Lob", nz(sData.Substring(3, 3).Trim()));
                            SetFieldValue("Activity", nz(sData.Substring(6, 3).Trim()));
                        }
                        else
                        {
                            nShift = -1;
                            SetFieldValue("Interface", sData.Substring(0, 1));
                            SetFieldValue("System", sData.Substring(1, 1));
                            SetFieldValue("SubSystem", nz(sData.Substring(2, 3).Trim()));
                            SetFieldValue("Action", nz(sData.Substring(5, 3).Trim()));
                            SetFieldValue("CobolName", sData.Substring(202, 36));
                        }
                        SetFieldValue("TafnitBufPosition", nz(sData.Substring(9 + nShift, 5).Trim()));
                        SetFieldValue("TafnitBufPosition2", nz(sData.Substring(14 + nShift, 5).Trim()));
                        SetFieldValue("TafnitBufPosition3", nz(sData.Substring(19 + nShift, 5).Trim()));
                        SetFieldValue("NodeNameSource", sData.Substring(24 + nShift, 50).Trim());
                        SetFieldValue("NodeName", sData.Substring(74 + nShift, 50).Trim());
                        SetFieldValue("FieldDescription", AdjustLang(sData.Substring(124 + nShift, 50), flr, "000", 50, true, false));
                        SetFieldValue("TafnitFieldType", sData.Substring(174 + nShift, 1).Trim());
                        SetFieldValue("TafnitLeftRight", sData.Substring(175 + nShift, 1).Trim());
                        SetFieldValue("TafnitSize", nz(sData.Substring(176 + nShift, 5).Trim()));
                        SetFieldValue("TafnitDecimal", nz(sData.Substring(181 + nShift, 2).Trim()));
                        SetFieldValue("Direction", sData.Substring(183 + nShift, 1).Trim());
                        SetFieldValue("Translation", nz(sData.Substring(184 + nShift, 3).Trim()));
                        SetFieldValue("ExternalFieldType", sData.Substring(187 + nShift, 1).Trim());
                        SetFieldValue("ExternalLeftRight", sData.Substring(188 + nShift, 1).Trim());
                        SetFieldValue("ExternalSize", nz(sData.Substring(189 + nShift, 5).Trim()));
                        SetFieldValue("ExternalDecimal", nz(sData.Substring(194 + nShift, 2).Trim()));
                        SetFieldValue("ExternalPosition", nz(sData.Substring(196 + nShift, 5).Trim()));
                        SetFieldValue("TafnitHebrew", sData.Substring(201 + nShift, 1).Trim()); //vk 05.10
                        SetFieldValue("Thinning", sData.Substring(202 + nShift, 1).Trim()); //vk 06.10
                        break;
                    case "tblVendor": //ik 12.11
                        SetFieldValue("VendorCode", sData.Substring(0, 3).Trim());
                        SetFieldValue("VendorDesc", AdjustLang(sData.Substring(3, 12), flr, "000", 12, true, false));
                        break;
                    case "tblCategory": //ik 12.11
                        SetFieldValue("VendorCode", sData.Substring(0, 3).Trim());
                        SetFieldValue("CategoryDesc", AdjustLang(sData.Substring(3, 24), flr, "000", 24, true, false));
                        SetFieldValue("CategoryCode", sData.Substring(27, 10).Trim());
                        break;
                    case "tblModel": //ik 12.11
                        SetFieldValue("ModelCode", nz(sData.Substring(0, 8).Trim()));
                        SetFieldValue("ModelDesc", AdjustLang(sData.Substring(8, 24), flr, "000", 24, true, false));
                        SetFieldValue("CategoryCode", sData.Substring(32, 10).Trim());
                        SetFieldValue("ModelFirstYear", nz(sData.Substring(42, 4).Trim()));
                        SetFieldValue("ModelLastYear", nz(sData.Substring(46, 4).Trim()));
                        break;
                    case "tb398": //vk 09.24
                        SetFieldValue("U398LDR", nz(sData.Substring(0, 2).Trim()));
                        SetFieldValue("U398TAV", nz(sData.Substring(2, 3).Trim()));
                        SetFieldValue("U398TBL", nz(sData.Substring(5, 3).Trim()));
                        SetFieldValue("U398DAT", nz(sData.Substring(8, 8).Trim()));
                        SetFieldValue("U398RAM", nz(sData.Substring(16, 1).Trim()));
                        SetFieldValue("U398K01", nz(sData.Substring(17, 5).Trim()));
                        SetFieldValue("U398K02", nz(sData.Substring(22, 5).Trim()));
                        SetFieldValue("U398K03", nz(sData.Substring(27, 5).Trim()));
                        SetFieldValue("U398NAM", AdjustLang(sData.Substring(32, 40), flr, "000", 40, true, false));
                        SetFieldValue("U398NMA", AdjustLang(sData.Substring(72, 40), flr, "000", 40, true, false));
                        SetFieldValue("U398NMT", AdjustLang(sData.Substring(112, 30), flr, "000", 30, true, false));
                        SetFieldValue("U398RM1", AdjustLang(sData.Substring(142, 15), flr, "000", 15, true, false));
                        SetFieldValue("U398RM2", AdjustLang(sData.Substring(157, 15), flr, "000", 15, true, false));
                        SetFieldValue("U398RM3", AdjustLang(sData.Substring(172, 15), flr, "000", 15, true, false));
                        SetFieldValue("U398FIL", AdjustLang(sData.Substring(187, 200), flr, "000", 200, true, false));
                        break;
                }
                SubmitOnce(pSes, pReq);
            }
            catch (Exception e)
            {
                LogError(e.Message, pSes, pReq);
            }
        }
        public string ErrorText()
        {
            if (eException != null)
                return eException.Message;
            else
                return "";
        }
        public Exception ErrorItself() //vk 11.15
        {
            return eException;
        }
        private string AdjustLang(string val, string flr, string rus, int nMaxlen, bool bTrim, bool bCombo)
        {
            string s;
            //rem vk 08.22
            //if (rus != "000")
            //{
            //    PocketKnife.Info o = new PocketKnife.Info();
            //    s = o.FromUni(val); //vk 03.05
            //    o = null;
            //}
            //else
            //{
            if (val.Length > nMaxlen)
                val = val.Substring(0, nMaxlen);
            if (flr != "0")
                s = val;
            else if (!bCombo)
                s = m_conv.RevHeb(val, ref m_sLang);
            else //vk 02.07
            {
                PocketKnife.Info o = new PocketKnife.Info();
                if (bTrim)
                    s = o.Heb11(val.Trim());
                else
                    s = o.Heb11(val);
                o = null;
            }
            //}
            if (bTrim)
                s = s.Trim();
            return s;
        }
        private string nz(string val)
        {
            if (val.Trim() == "")
                return "0";
            else
                return val.Trim();
        }

        private void WriteLineOnce(
            string sEvent, string sHost, string sUser, string sSynchr, string sDq, string sScreen)
        {
            try
            {
                SetFieldValue("sEvent", sEvent);
                SetFieldValue("sStation", sHost);
                SetFieldValue("sUser", sUser);
                SetFieldValue("sVersion", sVersion);
                SetFieldValue("sSite", sSite);
                SetFieldValue("sSynchr", sSynchr);
                //SetFieldValue("sMq",sMq);
                SetFieldValue("sDq", sDq);
                SetFieldValue("sScreen", sScreen); //vk 08.05
            }
            catch (Exception e) { eException = e; }
        }
        private void WriteLongOnce(
            string sWhat, string sString, string sSession, int nContinue)
        {
            try
            {
                InitOnce("tblLog2", false, sSession);
                SetFieldValue("sWhat", sWhat);
                SetFieldValue("sString", sString);
                SetFieldValue("nContinue", nContinue);
                //nLocalCounter = Convert.ToInt64(pSession["LocalCounter"]) + 1;
                //pSession["LocalCounter"] = nLocalCounter;
                SetFieldValue("nLocalCounter", nLocalCounter);
                SubmitOnce();
            }
            catch (Exception e) { eException = e; }
        }
        private void WriteLong(
            string sWhat, string sString, string sSession)
        {
            string s;
            int i;
            try
            {
                if (sString.Length <= LongString)
                {
                    WriteLongOnce(sWhat, sString, sSession, 0);
                    return;
                }
                for (i = 1; sString.Length > 0; i++)
                {
                    if (sString.Length > LongString)
                    {
                        s = sString.Substring(0, LongString);
                        sString = sString.Substring(LongString);
                    }
                    else
                    {
                        s = sString;
                        sString = "";
                    }
                    WriteLongOnce(sWhat, s, sSession, i);
                }
            }
            catch (Exception e) { eException = e; }
        }
        private void InitOnce(
            string sTable, bool bTakeTime, string sSession) //bTakeTime vk 09.04
        {
            string s = sTable; //vk 01.07
            if (nHoursForLog > 0)
            {
                s += "_";
                s += dNow.DayOfWeek.ToString().Substring(0, 3);
                s += (dNow.Hour / nHoursForLog).ToString();
            }
            InitOnce_Simple(s, bTakeTime, true);
            try
            {
                SetFieldValue("dTime", dNow); //vk 08.05
                if (sSession != "*") SetFieldValue("sSession", sSession);
            }
            catch (Exception e) { eException = e; }
        }
        private void InitOnce_Simple(
            string sTable, bool bTakeTime, bool bLog) //vk 01.05
        {
            try
            {
                if (bTakeTime) dNow = System.DateTime.Now; //vk 08.05
                coll = new System.Collections.ArrayList();
                sTableName = sTable;
            }
            catch (Exception e) { eException = e; }
        }
        private void SetFieldValue(string sName, object oValue) //vk 05.05
        {
            MyField f = new MyField();
            f.sName = sName;
            f.oValue = oValue;
            coll.Add(f);
        }
        private void SubmitOnce(System.Web.SessionState.HttpSessionState pSes, System.Web.HttpRequest pReq)
        {
            try
            {
                string s1 = "";
                string s2 = "";
                foreach (MyField f in coll)
                {
                    if (f.oValue != null) //vk 12.05
                    {
                        s1 += "," + f.sName;
                        s2 += ",@" + f.sName;
                    }
                }
                SqlCommand cmd = new SqlCommand(
                    "insert into " + sTableName + " (" + s1.Substring(1) +
                    ") values (" + s2.Substring(1) + ")", conn);
                foreach (MyField f in coll)
                {
                    if (f.oValue != null) //vk 12.05
                        cmd.Parameters.AddWithValue("@" + f.sName, f.oValue);
                }
                Exec(cmd);
            }
            catch (Exception e)
            {
                LogError(e.Message, pSes, pReq);
            }

        }
        private void SubmitOnce()
        {
            try
            {
                string s1 = "";
                string s2 = "";
                foreach (MyField f in coll)
                {
                    if (f.oValue != null) //vk 12.05
                    {
                        s1 += "," + f.sName;
                        s2 += ",@" + f.sName;
                    }
                }
                SqlCommand cmd = new SqlCommand(
                    "insert into " + sTableName + " (" + s1.Substring(1) +
                    ") values (" + s2.Substring(1) + ")", conn);
                foreach (MyField f in coll)
                {
                    if (f.oValue != null) //vk 12.05
                        cmd.Parameters.AddWithValue("@" + f.sName, f.oValue);
                }
                Exec(cmd);
            }
            catch (Exception e) { eException = e; }
        }
        private string MyTrim(string s, int l)
        {
            try { return s.Substring(0, l); }
            catch { return s; }
        }
        public void Dispose() //vk 01.05
        {
            try { conn.Close(); }
            catch { }
            try { conn.Dispose(); }
            catch { }
            rd = null;
            rdVer = null;
        }
        private void LogError(string sMessage,
            System.Web.SessionState.HttpSessionState pSes, System.Web.HttpRequest pReq) //vk 12.05
        {
            eException = new Exception(sMessage);
        }
        private void Exec(SqlCommand cmd) //vk 09.12
        {
            for (; ; )
            {
                try
                {
                    cmd.ExecuteNonQuery();
                    return;
                }
                catch (System.Threading.ThreadAbortException)
                {
                    System.Threading.Thread.ResetAbort();
                }
                catch (System.InvalidOperationException)
                { //vk 09.14
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    else
                        return;
                }
            }
        }
    }
}
