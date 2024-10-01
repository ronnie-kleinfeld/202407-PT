//vk 03.05
using System;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Comtec.TIS;
using System.IO;
//using System.Net; //vk 12.10
using System.Net.NetworkInformation; //vk 12.11
using System.Threading; //vk 01.12
//using System.Threading.Tasks;
//vk 01.22
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace DDSWeb
{
    //serialization
    //	[Serializable]
    //	internal sealed class GreatConnectSerializationHelper : IObjectReference 
    //	{
    //		public Object GetRealObject(StreamingContext context)
    //		{
    //			return GreatConnect.GetGreatConnect();
    //		}
    //	}
    //serialization till here

    /// <summary>
    /// Summary description for GreatConnect.
    /// </summary>
    [Serializable]
    public class GreatConnect //: ISerializable
    {
        bool bDebug;
        bool bLockLogon;
        bool bTimeOut;
        PocketKnife.Mq oMq1 = null;
        PocketKnife.Mq oMq2 = null;
        bool bXSS_s2c = false; //ik 2012-01-30
        string sSynchr = "";
        string sGuidForMq;
        string sDqLib;
        string sJob;
        string strLib;
        string sMqKey;
        int nLogonPassed = 0;
        string sAppLang;
        string sUserHostAddress;
        //string sApplicationPath;
        //string def_path;
        string err_path;
        string mappath;
        int nErrCode; //vk 07.05
        string sErrorFromSql; //vk 07.05
        //vk 11.05
        PocketKnife.Info pp = new PocketKnife.Info();
        public string Server_IP; //= System.Net.Dns.Resolve("").AddressList[0].ToString();
        string sSite; //vk 12.06
        string sConnStrAddition;
        bool bTestXml = false; //vk 07.13
        //vk 10.13
        string sMailFrom;
        string sMailTo;
        string sMailServer;
        string sMailSubj_First;
        string sMailSubj_Other;
        int nMaxSec_First;
        int nMaxSec_Other;
        Reader mDP;
        public string RandomNumber;
        public string RandomNumberPrev; //ntg 17.06.24 vladi change regarding "back" button in browser 
        //vk 03.22
        string sParamTo400_siteNo = "";
        string sParamTo400_actionCode = "";
        string sParamTo400_prm1;
        string sParamTo400_prm2;
        string sParamTo400_prm3;
        string sParamTo400_prm4;
        public string sParamTo400_user = "";

        //serialization
        //		private static readonly GreatConnect theOneObject = new GreatConnect();
        //		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        //		{
        //			info.SetType(typeof(GreatConnectSerializationHelper));
        //		}
        //		public Object GetRealObject(StreamingContext context)
        //		{
        //			return GreatConnect.GetGreatConnect();
        //		}
        //		public static GreatConnect GetGreatConnect()
        //		{
        //			return theOneObject;
        //		}
        //serialization till here

        public GreatConnect(Reader dp,
            System.Web.HttpRequest pReq,
            System.Web.HttpServerUtility pSer,
            System.Web.SessionState.HttpSessionState pSes,
            string pMapPath)
        {
            mDP = dp;
            mappath = pMapPath;
            if (dp.getProperty("DebugXml") == "true")
                if (System.IO.File.Exists(mappath + "\\test.xml") || System.IO.File.Exists(mappath + "\\test.htm"))
                    bTestXml = true; //vk 07.13
            sAppLang = dp.getProperty("Language").ToLower();
            //def_path = (string)Application["PATH"] + "\\Defprop.config";
            err_path = mappath + "\\" + dp.getProperty("ErrorFilePrefix") + "error.xml";
            //vk 11.05

            if (dp.getProperty("XSS_s2c").ToLower() == "true") bXSS_s2c = true;

            sUserHostAddress = pReq.UserHostAddress;
            //sApplicationPath = pReq.ApplicationPath;
            sSite = dp.getProperty("DebugSite");
            if (sSite == "")
                sSite = pReq.ApplicationPath.Substring(1); //vk 12.06
            sConnStrAddition = dp.getProperty("ConnStrAddition"); //vk 03.08
            //vk 10.13
            string t;
            sMailFrom = dp.getProperty("MailFrom");
            sMailTo = dp.getProperty("MailTo");
            sMailServer = dp.getProperty("MailServer");
            sMailSubj_First = dp.getProperty("MailSubj_First");
            sMailSubj_Other = dp.getProperty("MailSubj_Other");
            t = dp.getProperty("MaxSec_First");
            if (t.Trim() == "")
                nMaxSec_First = 0;
            else
                nMaxSec_First = int.Parse(t);
            t = dp.getProperty("MaxSec_Other");
            if (t.Trim() == "")
                nMaxSec_Other = 0;
            else
                nMaxSec_Other = int.Parse(t);
            if (dp.getProperty("CheckGateway") == "true")
            {
                //vk 01.12, thanks to ag
                foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
                {
                    IPInterfaceProperties prop = adapter.GetIPProperties();
                    foreach (GatewayIPAddressInformation Gateway in prop.GatewayAddresses)
                        if (Gateway.Address.ToString() != "0.0.0.0")
                            foreach (IPAddressInformation tmp in prop.UnicastAddresses)
                                if (tmp.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                {
                                    Server_IP = tmp.Address.ToString();
                                    return;
                                }
                }
                LogError("Server IP not found", pSes);
            }
            else
            {
                //vk 12.10, thanks to ib
                foreach (System.Net.IPAddress tmp in System.Net.Dns.GetHostEntry("").AddressList)
                    if (tmp.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        Server_IP = tmp.ToString();
                        return;
                    }
            }
        }
        public string ErrDesc(int err, string sWhy)
        {
            return ErrDesc(err.ToString(), true, sWhy); //vk 11.11
        }

        public string ErrDesc(string err, bool withcode, string sWhy) //vk 11.11, 10.13
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(err_path);
            XmlNode xnode = xdoc.DocumentElement.SelectSingleNode
                ("//lang[@id='" + sAppLang
                + "']/err[@id='" + err + "']");
            if (xnode == null)
                return err + " " + sWhy;
            else if (withcode)
                return err + " " + sWhy + " " + pp.AntiXSSHtml(xnode.InnerText, bXSS_s2c);
            else
                return pp.AntiXSSHtml(xnode.InnerText, bXSS_s2c);
        }
        public int ErrCode()
        {
            return nErrCode;
        }
        public bool LockLogon()
        {
            return bLockLogon;
        }
        public bool TimeOut()
        {
            return bTimeOut;
        }

        private string Send(string s, int nErr,
            System.Web.SessionState.HttpSessionState pSes)
        {
            if (bDebug)
                return "";
            try
            {
                oMq1.Send(s); //,"");
                if (oMq1.ErrorText() != "")
                {
                    nErrCode = nErr;
                    LogError("Send: " + nErr.ToString() + " " + oMq1.ErrorText(), pSes);
                    return ErrDesc(nErr, "") + ErrorFromSql();
                }
                return "";
            }
            catch (Exception ee)
            {
                LogError("Send", pSes, ee);
                return "Send: " + ee.Message + ErrorFromSql();
            }
        }
        private string Receive(ref string s, string sKey,
            int nErrorPos, int nErr, int nErrTimeout,
            System.Web.SessionState.HttpSessionState pSes,
            bool bSend99, bool bFirst)
        {
            int res;
            string err;
            DateTime d1, d2; //vk 10.13
            for (; ; )
            {
                bTimeOut = false;
                try
                {
                    d1 = DateTime.Now;
                    if (sKey == "")
                        s = oMq2.Receive(); //("");
                    else
                        s = oMq2.Receive(sKey + "\\0");
                    d2 = DateTime.Now;
                    if (oMq2.ErrorText() != "")
                    {
                        nErrCode = nErr;
                        LogError("Receive: " + nErr.ToString() + " " + oMq2.ErrorText(), pSes);
                        return ErrDesc(nErr, "") + ErrorFromSql();
                    }
                    if (s.Trim() == "" || oMq2.TimeOut())
                    {
                        if (bSend99) //vk 03.07
                        {
                            PocketKnife.Text oText = new PocketKnife.Text(100);
                            oText.Put(1, 5, "99001");
                            string e = Send(oText.FullText(), 1006, pSes); //vk 02.07
                        }
                        bTimeOut = true;
                        nErrCode = nErrTimeout;
                        LogError("Receive Timeout: " + nErrTimeout.ToString() + " " + oMq2.ErrorText(), pSes);
                        return ErrDesc(nErrTimeout, "") + ErrorFromSql();
                    }
                    int nMaxSec;
                    string sMailSubj;
                    if (bFirst)
                    {
                        nMaxSec = nMaxSec_First;
                        sMailSubj = sMailSubj_First;
                    }
                    else
                    {
                        nMaxSec = nMaxSec_Other;
                        sMailSubj = sMailSubj_Other;
                    }
                    if (nMaxSec > 0 && sMailServer != "" && sMailTo != "")
                    {
                        TimeSpan tsFact = d2.Subtract(d1);
                        TimeSpan tsMax = new TimeSpan(0, 0, nMaxSec);
                        //if (d2 > d1.AddSeconds(nMaxSec))
                        if (tsFact > tsMax)
                        {
                            if (bFirst || !(bool)pSes["MailSent"])
                            { //vk 10.13
                                PocketKnife.Info o = new PocketKnife.Info();
                                o.SendMail(sMailFrom, sMailTo,
                                    sMailSubj + ": " + tsFact.ToString(),
                                    "Session: " + pSes.SessionID, sMailServer);
                                o = null;
                                if (!bFirst)
                                    pSes["MailSent"] = true;
                            }
                        }
                    }
                    err = s.Substring(nErrorPos, 3);
                    break; //vk 01.12
                }
                catch (ThreadAbortException) //vk 01.12
                {
                    Thread.ResetAbort();
                    //neither break nor return, must continue with for
                }
                catch (Exception ee)
                {
                    LogError("Receive", pSes, ee);
                    return "Receive: " + ee.Message + ErrorFromSql();
                }
            }
            if (err != "000") LogError("Receive: " + err, pSes);
            try
            {
                res = Convert.ToInt32(err);
                if (res != 0)
                {
                    nErrCode = res;
                    return ErrDesc(res, "");
                }
                return "";
            }
            catch
            {
                nErrCode = 999;
                return ErrDesc(999, "");
            }
        }

        public string SendUsual(string s, string sScreen,
            System.Web.SessionState.HttpSessionState pSession,
            bool bLog_Conn)
        {
            if (bDebug)
                return "";
            string sRet = "";
            string sHeader = "";
            string sParam;
            sParam = "Log_XML";
            if (bLog_Conn)
                sParam = "Log_Conn"; //vk 09.07
            PocketKnife.Text oText = new PocketKnife.Text(100);
            oText.Put(1, 5, "80003");
            oText.Put(6, 3, 0);
            oText.Put(9, 13, sSynchr);
            oText.Put(40, 10, sJob);
            oText.Put(50, 10, strLib);
            oText.Put(60, 10, sDqLib); //mq
            sHeader = oText.FullText();
            for (; ; ) //vk 09.14
            {
                try
                {
                    LogXML("BEFORE SEND", sHeader + s, sParam, sScreen, pSession);
                    sRet = Send(sHeader + s, 1006, pSession);
                    LogXML("XML SENT", "", sParam, sScreen, pSession);
                    sErrorFromSql = ""; //vk 07.06
                    return sRet + ErrorFromSql();
                }
                catch (System.Threading.ThreadAbortException)
                {
                    System.Threading.Thread.ResetAbort(); //vk 09.14
                }
            }
        }
        private void LogError(string sMessage, System.Web.SessionState.HttpSessionState pSession) //vk 08.05
        {
            //Task t = new Task(() =>
            //{
            LogWriter lw = new LogWriter("", (string)pSession["Path"], sConnStrAddition);
            lw.SetLocalCounter(pSession);
            lw.WriteLine("ERROR", sUserHostAddress, sMessage, pSession, (string)pSession["User"], "", "", "", "", "");
            if (lw.ErrorText() != "")
                sErrorFromSql = "1030 " + lw.ErrorText(); //vk 07.06
            else
                sErrorFromSql = "";
            lw.Dispose();
            lw = null;
            //});
            //t.Start();
        }
        public void LogError(string sMessage, System.Web.SessionState.HttpSessionState pSession, Exception e) //vk 08.05, public vk 11.15
        {
            //Task t = new Task(() =>
            //{
            LogWriter lw = new LogWriter("", (string)pSession["Path"], sConnStrAddition);
            lw.SetLocalCounter(pSession);
            lw.WriteLine("ERROR", sUserHostAddress, sMessage, pSession, (string)pSession["User"], "", "", "", e);
            if (lw.ErrorText() != "")
                sErrorFromSql = "1030 " + lw.ErrorText(); //vk 07.06
            else
                sErrorFromSql = "";
            lw.Dispose();
            lw = null;
            //});
            //t.Start();
        }
        private string ErrorFromSql() //vk 07.06
        {
            if (sErrorFromSql == "")
                return "";
            else
                return "<br>" + pp.AntiXSSHtml(sErrorFromSql, bXSS_s2c);
        }
        public string ReceiveUsual(ref string s,
            System.Web.SessionState.HttpSessionState pSession)
        {
            if (bDebug)
            {
                s = "debug";
                return "";
            }
            for (; ; ) //vk 09.14
            {
                try
                {
                    string sRet = "";
                    LogXML("BEFORE GET", "", "Log_XML", pSession["ScreenNum"].ToString(), pSession);
                    sRet = Receive(ref s, sMqKey, 5, 1010, 1009, pSession, false, false);
                    //vk 05.05
                    if (s.Length > 100)
                        sSynchr = s.Substring(8, 13);
                    string sParam = "Log_XML";
                    //if (Param(s, "eps", "") != "")
                    //    sParam = "";
                    LogXML("XML GOT", s, sParam, pSession["ScreenNum"].ToString(), pSession);
                    if (s.Length > 100)
                        s = s.Substring(100);
                    return sRet;
                }
                catch (System.Threading.ThreadAbortException)
                {
                    System.Threading.Thread.ResetAbort(); //vk 09.14
                }
                catch
                {
                    return ""; //vk 08.05
                }
            }
        }
        public void Dispose(System.Web.SessionState.HttpSessionState pSession)
        {
            try
            {
                if (oMq1 != null) oMq1.Dispose();
            }
            catch { }
            try
            {
                if (oMq2 != null) oMq2.Dispose();
            }
            catch { }
            if (oMq2.ErrorText() != "")
                LogError("Error deleting MSMQ", pSession, oMq2.OriginalException()); //vk 09.09
            oMq1 = null;
            oMq2 = null;
        }
        public void Dispose()
        {
            try
            {
                if (oMq1 != null) oMq1.Dispose();
            }
            catch { }
            try
            {
                if (oMq2 != null) oMq2.Dispose();
            }
            catch { }
            oMq1 = null;
            oMq2 = null;
        }

        public void LogXML(string sWhat, string sXML,
            string sParam, string sScreen,
            System.Web.SessionState.HttpSessionState pSession) //vk 12.04
        {
            LogXML(sWhat, sXML, sParam, sScreen, pSession, (string)pSession["User"]);
        }
        public void LogXML(string sWhat, string sXML,
            string sParam, string sScreen,
            System.Web.SessionState.HttpSessionState pSession, string sUser) //vk 06.09
        {
            if (sParam != "")
            {
                //vk 10.19
                //if (mDP.getProperty(sParam) == "false")
                //    return;
                switch (mDP.getProperty(sParam))
                {
                    case "false":
                        return;
                    case "true":
                        break;
                    default:
                        if ((";" + mDP.getProperty(sParam).ToLower() + ";").IndexOf(";" + sUser.ToLower() + ";") >= 0)
                            break;
                        else
                            return;
                }
            }
            //Task t = new Task(() =>
            //{
            for (; ; ) //vk 08.14
            {
                try
                {
                    //Task t = new Task(() =>
                    //{
                    LogWriter lw = new LogWriter("", (string)pSession["Path"], sConnStrAddition);
                    lw.SetLocalCounter(pSession);
                    lw.WriteLine(sWhat, sUserHostAddress, sXML.Trim(), pSession, sUser,
                        sSynchr, "", strLib, sScreen, sParam);
                    if (lw.ErrorText() != "")
                        sErrorFromSql = "1030 " + lw.ErrorText(); //vk 07.06
                    else
                        sErrorFromSql = "";
                    lw.Dispose();
                    lw = null;
                    break;
                }
                catch (System.Threading.ThreadAbortException)
                {
                    System.Threading.Thread.ResetAbort(); //vk 08.14
                }
                catch
                {
                }
            }
            //});
            //t.Start();
        }

        public string CheckSiteBySql(System.Web.SessionState.HttpSessionState pSes)
        {
            bLockLogon = false;
            if (bTestXml) return ""; //vk 07.13
            PocketKnife.Sq oSq = null;
            try
            {
                string sSql = "select * from tblSite where sServerIP='" + Server_IP + "' and sSite='" + sSite + "';";
                oSq = new PocketKnife.Sq(
                    mDP.getProperty("SqlServer"), mDP.getProperty("DbForComm"), sSql,
                    mDP.getProperty("SqlUser"), pp.DecryptPassword(mDP.getProperty("SqlPass")), mDP.getProperty("ConnStrAddition"));
                if (oSq.ErrorText() != "")
                { //vk 07.06
                    bLockLogon = true;
                    string s = ErrDesc(1020, "");
                    nErrCode = 1020;
                    LogError("DoConnect: 1020 " + oSq.ErrorText(), pSes);
                    s += ErrorFromSql();
                    return s;
                }
                if (oSq.Receive("bActive") == null)
                { //vk 12.05
                    bLockLogon = true;
                    nErrCode = 2001;
                    LogError("DoConnect: 2001", pSes);
                    return ErrDesc(2001, "") + ErrorFromSql();
                }
                if (!(bool)oSq.Receive("bActive"))
                {
                    bLockLogon = true;
                    nErrCode = 2001;
                    LogError("DoConnect: 2001", pSes);
                    return ErrDesc(2001, "") + ErrorFromSql();
                }
                if (!(bool)oSq.Receive("bMayWork"))
                {
                    int nWhy = 0;
                    string sWhy = ""; //vk 09.13
                    if (oSq.HasField("nWhy"))
                    { //vk 03.08
                        object fWhy = oSq.Receive("nWhy");
                        if (fWhy == null) { }
                        else if (Convert.IsDBNull(fWhy)) { }
                        else
                            nWhy = (int)fWhy;
                        fWhy = null;
                        if (nWhy > 0)
                            sWhy = " (" + nWhy.ToString() + ")"; //vk 09.13
                    }
                    object dt = oSq.Receive("dEstimation");
                    bool bTimeGiven = true;
                    if (dt == null)
                        bTimeGiven = false;
                    else if (Convert.IsDBNull(dt))
                        bTimeGiven = false;
                    if (!bTimeGiven)
                    {
                        //if (nWhy > 0)
                        //    nErrCode = 2300 + nWhy;
                        //else
                        nErrCode = 2003;
                        LogError("DoConnect: " + nErrCode.ToString() + sWhy, pSes);
                        return ErrDesc(nErrCode, sWhy) + ErrorFromSql();
                    }
                    else
                    {
                        DateTime dt0 = (DateTime)dt;
                        //if (nWhy > 0)
                        //    nErrCode = 2200 + nWhy;
                        //else
                        nErrCode = 2002;
                        //vk 06.06
                        string er;
                        er = dt0.ToString(mDP.getProperty("DateTimeFormat"));
                        LogError("DoConnect: " + nErrCode.ToString() + sWhy + ", " + er, pSes);
                        return ErrDesc(nErrCode, sWhy) + er + ErrorFromSql();
                    }
                }
                return "";
            }
            catch (Exception ee) //vk 07.06
            {
                nErrCode = -1;
                return ee.Message;
            }
            finally
            {
                try { oSq.Dispose(); }
                catch { }
                oSq = null;
            }
        }
        public string CheckBrowser(System.Web.HttpRequest pReq)
        {
            //vk 12.20
            string userAgent = pReq.UserAgent;
            if (mDP.getProperty("AllowEdge").ToLower() == "false")
            {
                if (userAgent.IndexOf("Edg") <= -1) return "";
                return "Browser not supported";
            }
            if (mDP.getProperty("AllowBrowsers").ToLower() == "false") //vk 03.21
            {
                if (userAgent.IndexOf("MSIE") > -1) return "";
                if (userAgent.IndexOf("Trident") > -1) return "";
                return "Browser not supported";
            }
            return "";
        }
        public string CheckUserBySql(System.Web.SessionState.HttpSessionState pSes, string U) //vk 02.06
        {
            bLockLogon = false;
            PocketKnife.Sq oSq = null;
            if (mDP.getProperty("CheckUser") == "false") return "";
            try
            {
                string sSQL;
                sSQL = "select * from tblAccUsr where sUser='" + U + "' and sPermit=' ';";
                oSq = new PocketKnife.Sq(
                    mDP.getProperty("SqlServer"), mDP.getProperty("DbForCheckUser"), sSQL,
                    mDP.getProperty("SqlUser"), pp.DecryptPassword(mDP.getProperty("SqlPass")), mDP.getProperty("ConnStrAddition"));
                //LogXML("CHECK USER", sSQL, "Log_Conn", "", pSes); //vk 05.15
                if (oSq.ErrorText() != "")
                { //vk 07.06
                    bLockLogon = true;
                    string s = ErrDesc(1020, "");
                    nErrCode = 1020;
                    LogError("DoConnect: 1020 " + oSq.ErrorText(), pSes);
                    s += ErrorFromSql();
                    return s;
                }
                if (oSq.RowCount() == 0)
                {
                    nErrCode = 11;
                    return mDP.getProperty("CheckUserErrMsg");
                }
                else
                {
                    nErrCode = 0;
                    return "";
                }
            }
            catch (Exception ee) //vk 05.06
            {
                nErrCode = -1;
                return ee.Message;
            }
            finally
            {
                try { oSq.Dispose(); }
                catch { }
                oSq = null;
            }
        }
        public string CheckByConnect(System.Web.SessionState.HttpSessionState pSes,
            string U, string P)
        {
            bLockLogon = false;
            if (mDP.getProperty("ExactSecurity") != "true")
            { //vk 05.21
                U = U.ToUpper();
                P = P.ToUpper();
            }
            pSes["User"] = ""; //vk 04.12
            pSes["Password"] = ""; //vk 04.12
            string sCheckWay = mDP.getProperty("CheckByConnectWay");
            if (bTestXml) sCheckWay = "yalla_sa"; //vk 07.13
            switch (sCheckWay)
            {
                case "yalla_sa": //vk 01.06
                    pSes["User"] = U; //vk 04.12
                    return "";
                case "":
                case "as400":
                    if (U.Trim() == "" || P.Trim() == "")
                    {
                        //vk 02.06
                        nErrCode = 100;
                        return ErrDesc(nErrCode, "");
                    }
                    //PocketKnife.Dq oDq;
                    pSes["User"] = U; //vk 04.12
                    if (ExternalAgent(U))
                    {
                        pSes["Password"] = P; //vk 04.12
                        U = mDP.getProperty("ACUDFT");
                        P = pp.DecryptPassword(mDP.getProperty("ACUPWD"));
                    }
                    //oDq = new PocketKnife.Dq(mDP.getProperty("System"), U, P);
                    //string sDqError = oDq.ErrorText();
                    string sDqError = "";
                    try
                    {
                        DspWsServer.Service1 ws = new DspWsServer.Service1();
                        ws.Url = mDP.getProperty("WsPath");
                        sDqError = ws.As400("BD9D2C18-0449-4391-AB4A-E29CB9095598",
                            mDP.getProperty("System"), U, P); //vk 07.09
                        sDqError = pp.AntiXSSHtml(sDqError, bXSS_s2c);
                        ws.Dispose();
                        ws = null;
                    }
                    catch (Exception e)
                    {
                        sDqError = "Could not check the user:<br>" + e.Message; //vk 08.09
                    }
                    LogXML("CONN CHECK", (sDqError == "" ? "OK" : sDqError), "Log_Conn", "", pSes, U); //vk 06.09
                    if (sDqError != "")
                    {
                        //pSes["User"] = ""; //vk 04.12
                        //oDq.Dispose();
                        //oDq = null;
                        if (sDqError.Length < 9) //vk 06.06
                        {
                            pSes["User"] = ""; //vk 05.13
                            nErrCode = 1001;
                            LogError("DoConnect: " + nErrCode.ToString() + " " + sDqError, pSes);
                            return ErrDesc(nErrCode, "") + "<br>" + sDqError + ErrorFromSql();
                        }
                        else
                        { //vk 02.09
                            string codeCA = sDqError.Substring(0, 9);
                            string code = ";" + codeCA + ";";
                            if ((";" + mDP.getProperty("SecurityErrors_Allow") + ";").IndexOf(code) >= 0)
                            {
                                return "";
                            }
                            else if ((";" + mDP.getProperty("SecurityErrors_Block") + ";").IndexOf(code) >= 0)
                            {
                                pSes["User"] = ""; //vk 05.13
                                nErrCode = 10;
                                //vk 11.11, 12.13
                                string msg = ErrDesc(codeCA, false, "");
                                if (msg.Trim() == codeCA) msg = ErrDesc(nErrCode, "");
                                LogError("DoConnect: " + nErrCode.ToString() + " " + sDqError, pSes);
                                return msg + ErrorFromSql();
                            }
                            else
                            {
                                pSes["User"] = ""; //vk 05.13
                                nErrCode = 1001;
                                LogError("DoConnect: " + nErrCode.ToString() + " " + sDqError, pSes);
                                return ErrDesc(nErrCode, "") + "<br>" + sDqError + ErrorFromSql();
                            }
                        }
                    }
                    //oDq.Dispose();
                    //oDq = null;
                    return "";
                default:
                    nErrCode = 1101;
                    return "You must set the way of check by connect";
            }
        }
        public string CheckByIp(System.Web.SessionState.HttpSessionState pSes, System.Web.HttpRequest pReq) //vk 10.07
        {
            string sClientIp = mDP.getProperty("ClientIp");
            if (sClientIp == "") return "";
            foreach (string a in sClientIp.Split(','))
                if (a == pReq.UserHostAddress)
                    return "";
            nErrCode = 12;
            LogError("DoConnect: 12", pSes);
            return ErrDesc(12, "") + ErrorFromSql();
        }
        public bool ExternalAgent(string U) //vk 12.05
        {
            string sACULET = mDP.getProperty("ACULET").ToUpper();
            int nLetters = sACULET.Length;
            if (nLetters == 0) return false;
            switch (mDP.getProperty("ACUREL"))
            {
                case "EQ":
                    if (U.Length < nLetters) return false;
                    else return sACULET == U.Substring(0, nLetters);
                case "NE":
                    if (U.Length < nLetters) return true;
                    else return sACULET != U.Substring(0, nLetters);
                default: return false;
            }
        }

        private bool NotNum(string sParam) //vk 11.22
        {
            long tryp;
            if (sParam == null) return false; //vk 01.23
            if (sParam.Trim() == "") return false;
            return !long.TryParse(sParam.Trim(), out tryp);
        }

        public string DoConnect(
            System.Web.SessionState.HttpSessionState pSes,
            System.Web.HttpServerUtility pSer,
            System.Web.HttpRequest pReq,
            System.Web.HttpResponse pRes,
            string sSpecialCare, string sAdditionalData, string sAdditionalData2,
            string sCodeFromFile, string sIp)
        {
            string U = (string)pSes["User"]; //vk 04.12
            string P = (string)pSes["Password"]; //vk 04.12
            if (sIp != "") sUserHostAddress = sIp; //vk 03.06
            //pSes["Path"] = pSer.MapPath("");
            //pSes["ResolutionW"] = resolW;
            //pSes["ResolutionH"] = resolH;
            pSes["Kiosk"] = (sSpecialCare == "kiosk"); //vk 03.05
            pSes["Token"] = (sSpecialCare == "token" || sSpecialCare == "logonq" ? "true" : ""); //vk 05.10, 11.17
            //vk 09.06
            sGuidForMq = Guid.NewGuid().ToString();
            pSes["Guid"] = sGuidForMq;

            bLockLogon = false;
            if (nLogonPassed == 2)
            {
                nErrCode = 1004;
                LogError("DoConnect: 1004", pSes);
                return ErrDesc(1004, "") + ErrorFromSql();
            }
            if (mDP.getProperty("ExactSecurity") != "true")
            { //vk 04.22
                U = U.ToUpper();
                P = P.ToUpper();
            }

            //vk 01.09
            bDebug = false;
            if (mDP.getProperty("DebugXml") == "true")
            {
                PocketKnife.Fq f = null;
                try
                {
                    f = new PocketKnife.Fq(mappath + "\\test.xml", PocketKnife.Fq.FileType.Read);
                    bDebug = true;
                }
                catch { }
                finally
                {
                    if (f != null) f.Dispose();
                    f = null;
                }
                try
                {
                    f = new PocketKnife.Fq(mappath + "\\test.htm", PocketKnife.Fq.FileType.Read);
                    bDebug = true;
                }
                catch { }
                finally
                {
                    if (f != null) f.Dispose();
                    f = null;
                }
            }

            string SessionId = pSes.SessionID;
            PocketKnife.Text oText;
            string s = "";
            //bool bNotConn=false; //vk 06.05
            string sError = ""; //vk 06.05

            //vk 03.05
            //========================================
            oMq1 = new PocketKnife.Mq(
                mDP.getProperty("Mq_p2a"), "", false, 0);
            oMq2 = new PocketKnife.Mq(
                mDP.getProperty("Mq_a2p"), "", false, Convert.ToInt32(mDP.getProperty("MainDQTimeOut")));
            if (oMq1.ErrorText() != "" || oMq2.ErrorText() != "")
            {
                nErrCode = 2005;
                if (oMq1.ErrorText() != "") LogError("DoConnect: 2005(1) " + oMq1.ErrorText(), pSes);
                if (oMq2.ErrorText() != "") LogError("DoConnect: 2005(2) " + oMq2.ErrorText(), pSes);
                return ErrDesc(2005, "") + ErrorFromSql();
            }
            oText = new PocketKnife.Text(400);
            oText.Put(1, 5, "20002");
            oText.Put(6, 15, Server_IP);
            oText.Put(21, 10, mDP.getProperty("SiteID"));
            oText.Put(31, 38, sGuidForMq);
            oText.Put(69, 4, Convert.ToInt32(mDP.getProperty("MainDQTimeOut")));
            if (mDP.getProperty("ExactSecurity") != "true") //vk 04.22
                oText.Put(83, 10, U.ToUpper());
            else
                oText.Put(83, 10, U);
            oText.Put(103, 50, sSite);
            oText.Put(199, 10, sCodeFromFile); //vk 05.05
            oText.Put(219, 15, sUserHostAddress);
            oText.Put(400, 1, "9"); //vk 09.08
            switch (sSpecialCare)
            {
                case "sabra":
                    oText.Put(73, 10, sAdditionalData);
                    break;
                case "exter":
                    oText.Put(73, 10, mDP.getProperty("ACUDFT"));
                    if (mDP.getProperty("ExactSecurity") != "true") //vk 04.22
                        oText.Put(93, 10, P.ToUpper());
                    else
                        oText.Put(93, 10, P);
                    oText.Put(234, 10, mDP.getProperty("ACULIB")); //vk 12.05
                    break;
                case "token": //vk 11.06, 09.08
                    if (mDP.getProperty("ExactSecurity") != "true") //vk 04.22
                        oText.Put(244, 10, U.ToUpper());
                    else
                        oText.Put(244, 10, U);
                    oText.Put(254, 10, sAdditionalData2); //clerk
                    oText.Put(264, 32, sAdditionalData); //token
                    break;
                case "zehut": //vk 11.07
                    oText.Put(93, 10, sAdditionalData);
                    break;
                case "check": //vk 04.10
                    oText.Put(296, 2, "CH");
                    break;
            }

            //vk 01.22, 03.22, 09.22, 11.22
            if (sParamTo400_siteNo != "")
            {
                if (NotNum(sParamTo400_prm1)) { return ErrDesc(1102, ""); }
                if (NotNum(sParamTo400_prm2)) { return ErrDesc(1102, ""); }
                if (NotNum(sParamTo400_prm3)) { return ErrDesc(1102, ""); }
                oText.Put(301, 2, sParamTo400_siteNo);
                oText.Put(303, 1, sParamTo400_actionCode);
                oText.Put(304, 13, sParamTo400_prm1);
                oText.Put(317, 13, sParamTo400_prm2);
                oText.Put(330, 13, sParamTo400_prm3);
                oText.Put(343, 20, sParamTo400_prm4);
            }
            else if (pReq.QueryString["t"] == null && pReq.QueryString["siteNo"] != null && (mDP.getProperty("ParamTo400") == "URL" || mDP.getProperty("ParamTo400") == "API+URL"))
            {
                if (NotNum(pReq.QueryString["prm1"])) { return ErrDesc(1102, ""); }
                if (NotNum(pReq.QueryString["prm2"])) { return ErrDesc(1102, ""); }
                if (NotNum(pReq.QueryString["prm3"])) { return ErrDesc(1102, ""); }
                oText.Put(301, 2, pReq.QueryString["siteNo"].ToString().PadLeft(2, '0'));
                if (pReq.QueryString["actionCode"] == null)
                    oText.Put(303, 1, "".PadLeft(1, '0'));
                else
                    oText.Put(303, 1, pReq.QueryString["actionCode"].ToString().PadLeft(1, '0'));
                if (pReq.QueryString["prm1"] == null)
                    oText.Put(304, 13, "".PadLeft(13, '0'));
                else
                    oText.Put(304, 13, pReq.QueryString["prm1"].ToString().Trim().PadLeft(13, '0'));
                if (pReq.QueryString["prm2"] == null)
                    oText.Put(317, 13, "".PadLeft(13, '0'));
                else
                    oText.Put(317, 13, pReq.QueryString["prm2"].ToString().Trim().PadLeft(13, '0'));
                if (pReq.QueryString["prm3"] == null)
                    oText.Put(330, 13, "".PadLeft(13, '0'));
                else
                    oText.Put(330, 13, pReq.QueryString["prm3"].ToString().Trim().PadLeft(13, '0'));
                if (pReq.QueryString["prm4"] == null)
                    oText.Put(343, 20, "".PadRight(20, ' '));
                else
                    oText.Put(343, 20, pReq.QueryString["prm4"].ToString().PadRight(20, ' '));
            }

            string e;
            e = Send(oText.FullText(), 1002, pSes);
            if (e != "")
                return e;
            LogXML("CONN SENT", oText.FullText(), "Log_Conn", "", pSes);
            if (!bDebug)
                e = Receive(ref s, sGuidForMq, 66, 1003, 1005, pSes, true, true);
            if (e != "" || bTimeOut)
            {
                //return e;
                //bNotConn=true;
                sError = e;
                sDqLib = "";
                strLib = "";
                sMqKey = "";
                return e; //vk 01.07
            }
            else
            {
                if (sSpecialCare == "check") return ""; //vk 04.10
                LogXML("CONN GOT", s, "Log_Conn", "", pSes);
                s = s.PadRight(140); //moved here by vk 01.09
                sDqLib = s.Substring(84, 10).Trim();
                strLib = s.Substring(104, 10).Trim();
                if (s.Substring(122, 1) == "Y")
                    pSes["LastEntry"] = s.Substring(123, 14); //vk 11.07
                else
                    pSes["LastEntry"] = "";
                pSes["Job"] = strLib; //vk 05.07
                //PocketKnife.Info oInfo=new PocketKnife.Info();
                sMqKey = "";
                //oInfo=null;
            }
            //========================================

            // First stage of connection is OK.
            nLogonPassed = 1;
            try //vk 08.03
            {
                //vk 03.05
                //========================================
                if (oMq2 != null) oMq2.Dispose();
                //vk 02.18 from here
                oMq2 = new PocketKnife.Mq(strLib, "", false, 0);
                if (oMq2.Exists())
                {
                    LogError("DoConnect: MSMQ exists: " + strLib, pSes);
                    for (; ; )
                    {
                        string clear = oMq2.Receive();
                        if (clear == "") break;
                    }
                    if (oMq2.ErrorText() != "")
                    {
                        oMq2.Dispose(true);
                        oMq2 = new PocketKnife.Mq(strLib, "", true, Convert.ToInt32(mDP.getProperty("MainDQTimeOut")));
                    }
                    else
                    {
                        oMq2.SetTimeOut(Convert.ToInt32(mDP.getProperty("MainDQTimeOut")));
                    }
                }
                else
                {
                    oMq2 = new PocketKnife.Mq(strLib, "", true, Convert.ToInt32(mDP.getProperty("MainDQTimeOut")));
                }
                //vk 02.18 till here
                //oMq2 = new PocketKnife.Mq(
                //    strLib, "", true,
                //    Convert.ToInt32(mDP.getProperty("MainDQTimeOut")));
                if (oMq2.ErrorText() != "" && !bDebug)
                { //vk 12.06
                    nErrCode = 2005;
                    LogError("DoConnect: 2005(private) " + oMq2.ErrorText(), pSes);
                    return ErrDesc(2005, "") + ErrorFromSql();
                }
                oText = new PocketKnife.Text(100);
                oText.Put(1, 5, "80001");
                oText.Put(6, 3, 0);
                oText.Put(9, 13, 0); //synchr
                oText.Put(40, 10, ""); //job
                oText.Put(50, 10, strLib);
                oText.Put(60, 10, sDqLib); //mq
                //string e;
                e = Send(oText.FullText(), 1006, pSes);
                if (e != "")
                    return e;
                LogXML("CONN SENT", oText.FullText(), "Log_Conn", "", pSes);
                if (!bDebug)
                    e = Receive(ref s, sMqKey, 5, 1007, 1008, pSes, true, false);
                if (e != "")
                    return e;
                LogXML("CONN GOT", s, "Log_Conn", "", pSes);
                sSynchr = s.Substring(8, 13); //vk 11.05
                sJob = s.Substring(39, 10);
                oText.Put(1, 5, "80004");
                oText.Put(6, 3, 0);
                oText.Put(9, 13, sSynchr); //synchr
                oText.Put(40, 30, s.Substring(39, 30)); //job + mq + dq
                e = Send(oText.FullText(), 1006, pSes);
                if (e != "")
                    return e;
                LogXML("CONN SENT", oText.FullText(), "Log_Conn", "", pSes);
                oMq2.SetTimeOut(Convert.ToInt32(mDP.getProperty("WorkTimeOut")));
                //========================================
            }
            catch (Exception ee)
            {
                return ee.Message;
            }
            // Second stage of connection is OK.
            nLogonPassed = 2;
            System.Web.Security.FormsAuthentication.SetAuthCookie(U, false); //11.05
            //System.Web.Security.FormsAuthentication.FormsCookieName = mDP.getProperty("Site");
            if (mDP.getProperty("GuidInCookies") == "true")
            { //vk 08.09
                pRes.Cookies["IXSession"].Value = System.Guid.NewGuid().ToString();
                //pRes.Cookies["IXSession"].Expires = DateTime.Now; //rem vk 04.12
            }
            LogXML("CONNECTED", "", "Log_StartEnd", "", pSes);
            return "";
        }

        public void ParamTo400(System.Web.SessionState.HttpSessionState pSes, System.Web.HttpRequest pReq) //vk 03.22
        {
            sParamTo400_siteNo = "";
            sParamTo400_actionCode = "";
            sParamTo400_prm1 = "".PadLeft(13, '0');
            sParamTo400_prm2 = "".PadLeft(13, '0');
            sParamTo400_prm3 = "".PadLeft(13, '0');
            sParamTo400_prm4 = "".PadRight(20, ' ');
            sParamTo400_user = "";
            if (pReq.QueryString["t"] != null && (mDP.getProperty("ParamTo400") == "API" || mDP.getProperty("ParamTo400") == "API+URL"))
            {
                string t = pReq.QueryString["t"].ToString();
                LogXML("CONN+ 1", t, "Log_Conn", "", pSes);
                //vk 07.22
                PocketKnife.Info o = new PocketKnife.Info();
                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                         | SecurityProtocolType.Tls11
                                         | SecurityProtocolType.Tls12
                                         | SecurityProtocolType.Ssl3;
                    ServicePointManager.ServerCertificateValidationCallback = delegate (object sender1, X509Certificate pCertificate, X509Chain pChain, SslPolicyErrors pSSLPolicyErrors) { return true; };

                    using (HttpClient client = new HttpClient())
                        if (mDP.getProperty("ParamTo400_Way") == "h")
                        { //vk 12.23
                            client.DefaultRequestHeaders.Add("ApiKey", o.DecryptPassword(mDP.getProperty("ParamTo400_Key")));
                            using (HttpResponseMessage response = client.GetAsync(mDP.getProperty("ParamTo400_API") + t).Result)
                                GetResponse(response, pSes);
                        }
                        else
                        {
                            using (HttpContent inputContent = new StringContent("{\"app\":\"" + o.DecryptPassword(mDP.getProperty("ParamTo400_Key")) + "\",\"token\":\"" + t + "\"}", System.Text.Encoding.UTF8, "Application/JSON"))
                            using (HttpResponseMessage response = client.PostAsync(mDP.getProperty("ParamTo400_API"), inputContent).Result)
                                GetResponse(response, pSes);
                        }
                }
                catch (Exception e1)
                {
                    LogError("Could not retrieve values for AS/400", pSes, e1);
                }
                finally
                {
                    o = null;
                }
            }
        }
        public void GetResponse(HttpResponseMessage response, System.Web.SessionState.HttpSessionState pSes) //vk 12.23
        {
            ReturnValue ReturnValue = new ReturnValue();
            if (response.IsSuccessStatusCode)
            {
                var rt = response.Content.ReadAsStringAsync().Result;
                LogXML("CONN+ 2", rt, "Log_Conn", "", pSes);
                ReturnValue = JsonConvert.DeserializeObject<ReturnValue>(rt);
                if (ReturnValue.status != "success") //vk 05.22
                {
                    LogXML("CONN+ ERROR", "no success status", "Log_Conn", "", pSes);
                }
                else
                {
                    if (ReturnValue.data.data.siteNo == null)
                        sParamTo400_siteNo = "00"; //vk 05.22
                    else
                        sParamTo400_siteNo = ReturnValue.data.data.siteNo.PadLeft(2, '0');
                    if (ReturnValue.data.data.actionCode == null)
                        sParamTo400_actionCode = "0"; //vk 05.22
                    else
                        sParamTo400_actionCode = ReturnValue.data.data.actionCode.PadLeft(1, '0');
                    if (ReturnValue.data.data.prm1 != null)
                        sParamTo400_prm1 = ReturnValue.data.data.prm1.Trim().PadLeft(13, '0');
                    if (ReturnValue.data.data.prm2 != null)
                        sParamTo400_prm2 = ReturnValue.data.data.prm2.Trim().PadLeft(13, '0');
                    if (ReturnValue.data.data.prm3 != null)
                        sParamTo400_prm3 = ReturnValue.data.data.prm3.Trim().PadLeft(13, '0');
                    if (ReturnValue.data.data.prm4 != null)
                    { //vk 03.22+
                        Comtec.Tis.ConvCom oConv = new Comtec.Tis.ConvCom();
                        string sLang = "";
                        sParamTo400_prm4 = ReturnValue.data.data.prm4.Trim();
                        sParamTo400_prm4 = oConv.RevHeb(sParamTo400_prm4.Trim(), ref sLang);
                        sParamTo400_prm4 = sParamTo400_prm4.PadRight(20, ' ');
                        oConv = null;
                    }
                    if (ReturnValue.data.data.user != null)
                        sParamTo400_user = ReturnValue.data.data.user.Trim();
                }
            }
            else
            {
                //vk 03.22+
                var rt = response.Content.ReadAsStringAsync().Result;
                LogXML("CONN+ ERROR", rt, "Log_Conn", "", pSes);
            }
        }

        public bool FullLogon(
            System.Web.SessionState.HttpSessionState pSes,
            System.Web.HttpServerUtility pSer,
            System.Web.HttpRequest pReq,
            System.Web.HttpResponse pRes,
            string sIp,
            System.Web.UI.WebControls.Label lblMsg,
            System.Web.UI.HtmlControls.HtmlInputButton btnOk,
            System.Web.UI.HtmlControls.HtmlInputText txtUser,
            System.Web.UI.HtmlControls.HtmlInputText txtPassword,
            string U, string P,
            string sSpecialCare, string sAdditionalData, string sAdditionalData2, string sParam,
            bool bHideFields, bool bMustCheckByConnect, bool bMustCheckUserBySql, bool bMustCheckBrowser)
        {
            //Reader dp=(Reader)Application["DEFPROP"];
            string res = "";
            if (bMustCheckBrowser)
                res = CheckBrowser(pReq); //vk 12.20
            if (bMustCheckByConnect)
                res = CheckByConnect(pSes, U, P);
            if (res == "")
            {
                if (bMustCheckUserBySql)
                    res = CheckUserBySql(pSes, U); //vk 02.06
                if (res == "")
                {
                    res = DoConnect(pSes, pSer, pReq, pRes, sSpecialCare, sAdditionalData, sAdditionalData2, sParam, sIp);
                    if (res == "")
                    {
                        //if (sSpecialCare=="kiosk")
                        if (mDP.getProperty("SameWindow").ToLower() != "true" || (string)pSes["Token"] != "") //vk 08.07, 05.10
                            //pSer.Transfer("in0.aspx");
                            pRes.Redirect("in0.aspx", false);
                        else if (mDP.getProperty("Frame").ToLower() != "true")
                            //      pSer.Transfer("screen.aspx");
                            //pRes.Redirect("screen.aspx");
                            pRes.Redirect("in0.aspx?submit", false); //vk 07.23 - ntg 03.07.23
                        else
                            //      pSer.Transfer("http://localhost:52334/DDSWeb/frame.aspx"); //vk 05.10
                            pRes.Redirect("frame.aspx");
                        return true;
                    }
                }
            }
            if (ErrCode() == 42) res += " " + sParam;
            lblMsg.Text = res;
            if (sSpecialCare == "logonq")
                return false; //vk 11.17
            if (LockLogon() || bHideFields)
            {
                btnOk.Visible = false;
                txtUser.Visible = false;
                txtPassword.Visible = false;
                return false;
            }
            return true;
        }

        public string Param(string sXML, string sParam, string sDefault) //vk 10.07
        {
            StringReader sr = null;
            XmlTextReader rd;
            int i1, i2;
            try
            {
                i1 = sXML.IndexOf("<s ");
                if (i1 >= 0)
                {
                    i2 = sXML.IndexOf(">", i1);
                    if (i2 >= 0)
                    {
                        sr = new StringReader(sXML.Substring(i1, i2 - i1 + 1));
                        rd = new XmlTextReader(sr);
                        for (; rd.Read();)
                            if (rd.NodeType == XmlNodeType.Element)
                                if (rd.Name == "s")
                                {
                                    rd.MoveToAttribute(sParam);
                                    if (rd.Value == "")
                                        return sDefault;
                                    else
                                        return rd.Value;
                                }
                    }
                }
            }
            catch { }
            finally
            {
                rd = null;
                if (sr != null) sr.Dispose();
                sr = null;
            }
            return sDefault;
        }

        public string Synchr()
        {
            return sSynchr;
        }

        public void CloseSession(System.Web.SessionState.HttpSessionState pSes) //vk 12.11
        {
            pSes["StopSession"] = true;
            try
            {
                string s =
                    "<?xml version=\"1.0\" encoding=\"Windows-1255\"?><screen><s fil=\"          \" rec=\"          \"" +
                    " fld=\"          \" find=\"0000\" fcmd=\"##\"/><fields></fields></screen>";
                SendUsual(s, "", pSes, true);
            }
            catch (Exception ee)
            {
                LogError("Failed sending job end", pSes, ee);
            }
            //pSes.Abandon();
        }
    }

    //vk 01.22
    public class ReturnValue
    {
        public string version;
        public long created;
        public string message;
        public string status;
        public long code;
        public ReturnValue_Data data;
    }
    public class ReturnValue_Data
    {
        public string token;
        public ReturnValue_Data_Data data;
    }
    public class ReturnValue_Data_Data
    {
        public string siteNo;
        public string actionCode;
        public string prm1;
        public string prm2;
        public string prm3;
        public string prm4;
        public string user; //vk 03.22
    }
}
