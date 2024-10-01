//vk 09.16
//vk 09.16
using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using Comtec.TIS;
using System.Security.Cryptography.X509Certificates;
//using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DDSWeb;
//using System.Runtime.Serialization.Json;
//using System.Xml.Linq;
//using System.Xml.XPath;
using System.Net;
using System.Net.Security;
using System.Xml;
using System.IO;
using System.Reflection; //vk 06.21
//using Microsoft.Web.Services2.Security.X509;
//using System.Threading.Tasks;

namespace DspCallCG
{
    public class Class1
    {
        private X509Certificate cert; //vk 10.16
        private void WriteLog(string sEvent, string sXml,
            Reader dp,
            System.Web.HttpRequest pReq,
            System.Web.SessionState.HttpSessionState pSes,
            Exception e)
        {
            try
            {
                //Task t = new Task(() =>
                //{
                LogWriter lw = new LogWriter("", (string)pSes["Path"], dp.getProperty("ConnStrAddition"));
                lw.SetLocalCounter(pSes);
                lw.WriteLine(sEvent, (string)pSes["Station"], sXml,
                    pSes, (string)pSes["User"], "", "", "", e);
                lw.Dispose();
                lw = null;
                //});
                //t.Start();
            }
            catch (Exception)
            {
            }
        }
        private void WriteLog(string sEvent, string sXml,
            Reader dp,
            System.Web.HttpRequest pReq,
            System.Web.SessionState.HttpSessionState pSes)
        {
            try
            {
                //Task t = new Task(() =>
                //{
                LogWriter lw = new LogWriter("", (string)pSes["Path"], dp.getProperty("ConnStrAddition"));
                lw.SetLocalCounter(pSes);
                lw.WriteLine(sEvent, (string)pSes["Station"], sXml,
                    pSes, (string)pSes["User"], "", "", "", "");
                lw.Dispose();
                lw = null;
                //});
                //t.Start();
            }
            catch (Exception)
            {
            }
        }
        private X509Certificate Certificate(Reader dp,
            System.Web.HttpRequest pReq,
            System.Web.SessionState.HttpSessionState pSes) //vk 10.16
        {
            cert = null;
            try
            {
                WriteLog("CG SECURITY 1", "", dp, pReq, pSes);
                X509Store computerCaStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                WriteLog("CG SECURITY 2", "", dp, pReq, pSes);
                computerCaStore.Open(OpenFlags.ReadWrite);
                WriteLog("CG SECURITY 3", "", dp, pReq, pSes);
                X509CertificateCollection col =
                    computerCaStore.Certificates.Find(X509FindType.FindBySubjectName, dp.getProperty("CG_Cert"), false);
                WriteLog("CG SECURITY 4", "", dp, pReq, pSes);
                //X509Certificate cert = col[0];

                //WriteLog("CG SECURITY 1", "", dp, pReq, pSes);
                //X509CertificateStore store =
                //    X509CertificateStore.LocalMachineStore(X509CertificateStore.MyStore);
                //WriteLog("CG SECURITY 2", "", dp, pReq, pSes);
                //store.OpenRead();
                //WriteLog("CG SECURITY 3", "", dp, pReq, pSes);
                //X509CertificateCollection col =
                //    (X509CertificateCollection)store.FindCertificateBySubjectString(dp.getProperty("CG_Cert"));
                //WriteLog("CG SECURITY 4", "", dp, pReq, pSes);
                cert = col[0];
            }
            catch (Exception ex)
            {
                WriteLog("CG SECURITY ERROR", ex.Message, dp, pReq, pSes, ex);
            }
            return cert;
        }
        public string API(string sWhat, string sXmlFile, Reader dp,
            System.Web.HttpRequest pReq,
            System.Web.SessionState.HttpSessionState pSes,
            System.Web.HttpServerUtility pSer,
            HtmlInputHidden v1, HtmlInputHidden v2, HtmlInputHidden v3,
            HtmlInputHidden v4, HtmlInputHidden v5, HtmlInputHidden v6,
            HtmlInputHidden v7, HtmlInputHidden v8, HtmlInputHidden v9) //vk 01.20
        {
            bool ESB = dp.getProperty("WsApi") == "ESB"; //vk 04.21
            PocketKnife.Fq g = new PocketKnife.Fq(sXmlFile, PocketKnife.Fq.FileType.Read);
            string sXmlQ = g.Receive();
            string sXmlA = ""; //moved here vk 04.21
            if (ESB)
            {
                int nPos = sXmlQ.IndexOf("int_in=");
                if (nPos > 0)
                    sXmlQ = sXmlQ.Substring(nPos + 7);
            }
            switch (sWhat)
            {
                case "Url":
                    pSes["CG_Transaction"] = "";
                    break;
                case "Token":
                    sXmlQ = sXmlQ.Replace("{@trans@}", (string)pSes["CG_Transaction"]);
                    break;
            }
            if (((string)pSes["XmlFil"]).Trim() == "DCCGWINF") //vk 03.21 universal window by Mark
                sXmlQ = sXmlQ.Replace("{@whichcancel@}", "2");
            else
                sXmlQ = sXmlQ.Replace("{@whichcancel@}", "");
            sXmlQ = sXmlQ.Replace("{{terminal}}", pReq["Hi1_value"]);
            sXmlQ = sXmlQ.Replace("{{mpi_mid}}", dp.getProperty("Mid"));
            sXmlQ = sXmlQ.Replace("{{uniqueid}}", Guid.NewGuid().ToString());
            sXmlQ = sXmlQ.Replace("{@site@}", (string)pSes["Url"]);
            g.Dispose();
            g = null;
            if (!ESB)
            {
                sXmlQ = sXmlQ.Replace("{{cguser}}", dp.getProperty("CG_User"));
                PocketKnife.Info i = new PocketKnife.Info();
                sXmlQ = sXmlQ.Replace("{{cgpassword}}", i.DecryptPassword(dp.getProperty("CG_Pass")));
                i = null;
            }
            WriteLog("CG REQUEST " + sWhat, sXmlQ, dp, pReq, pSes);

            if (ESB)
            {
                //vk+ib 06.21
                Assembly asm;
                Type t;
                Object obj;
                MethodInfo method;
                PropertyInfo property_;
                Object result;

                asm = Assembly.LoadFrom(pSer.MapPath("bin\\CGBankHaPoalim.dll"));
                t = asm.GetType("CGBankHaPoalim.CGBankHaPoalim", true, true);
                obj = Activator.CreateInstance(t);
                property_ = t.GetProperty("Timeout");
                property_.SetValue(obj, 100000);
                property_ = t.GetProperty("User");
                property_.SetValue(obj, dp.getProperty("CG_User"));
                PocketKnife.Info i = new PocketKnife.Info();
                property_ = t.GetProperty("Password");
                property_.SetValue(obj, i.DecryptPassword(dp.getProperty("CG_Pass")));
                i = null;
                method = t.GetMethod("callGatewayForAuthenticateTrans");
                result = method.Invoke(obj, new Object[] { sXmlQ });
                sXmlA = (string)result;
                property_ = t.GetProperty("ErrorCode");
                int err = (int)property_.GetValue(obj);
                if (err != 0)
                {
                    v1.Value = err.ToString();
                    property_ = t.GetProperty("ErrorDesk");
                    v2.Value = "GetPayment" + sWhat + ": " + (string)property_.GetValue(obj);
                }
                //CGBankHaPoalim.CGBankHaPoalim o = new CGBankHaPoalim.CGBankHaPoalim();
                //o.Timeout = 100000;
                //o.User = dp.getProperty("CG_User");
                //o.Password = i.DecryptPassword(dp.getProperty("CG_Pass"));
                //sXmlA = o.callGatewayForAuthenticateTrans(sXmlQ);
                //if (o.ErrorCode != 0)
                //{
                //    v1.Value = o.ErrorCode.ToString();
                //    v2.Value = "GetPayment" + sWhat + ": " + o.ErrorDesk;
                //}
            }
            else
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                     | SecurityProtocolType.Tls11
                                     | SecurityProtocolType.Tls12
                                     | SecurityProtocolType.Ssl3;
                ServicePointManager.ServerCertificateValidationCallback = delegate (object sender1, X509Certificate pCertificate, X509Chain pChain, SslPolicyErrors pSSLPolicyErrors) { return true; };

                StreamWriter myWriter = null;
                HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(dp.getProperty("WsApi"));
                objRequest.Method = "POST";
                objRequest.ContentLength = sXmlQ.Length;
                objRequest.ContentType = "application/x-www-form-urlencoded";
                try
                {
                    myWriter = new StreamWriter(objRequest.GetRequestStream());
                    myWriter.Write(sXmlQ);
                }
                catch (Exception e)
                {
                    v1.Value = "999";
                    v2.Value = "GetPayment" + sWhat + ": " + e.Message;
                    return "ERROR";
                }
                finally
                {
                    myWriter.Close();
                }
                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    sXmlA = sr.ReadToEnd();
                    sr.Close();
                }
            }
            WriteLog("CG RESPONSE " + sWhat, sXmlA, dp, pReq, pSes);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sXmlA);
            switch (sWhat)
            {
                case "Url":
                    string sToken = FromXml(doc, "token");
                    pSes["CG_Transaction"] = sToken;
                    v1.Value = FromXml(doc, "status");
                    v2.Value = FromXml(doc, "statusText");
                    if (sToken == "")
                        return "ERROR:" + v2.Value + " (" + v1.Value + ")";
                    else
                        return FromXml(doc, "mpiHostedPageUrl");
                case "Token":
                    string s = FromXml(doc, "cardId");
                    //string s0 = FromXml(doc,"message");
                    //string cc = doc.GetElementsByTagName("creditCompany")[0].Attributes["code"].Value;
                    //     v1.Value = FromXml(doc, "result");//--rem ntg 30.04.24 vladi change regarding CG functionallity
                    //     v2.Value = FromXml(doc, "message");//--rem ntg 30.04.24 vladi change regarding CG functionallity
                    v1.Value = FromXml_Last(doc, "result"); //ntg 30.04.24 vladi change regarding CG functionallity
                    v2.Value = FromXml_Last(doc, "message");//ntg 30.04.24 vladi change regarding CG functionallity
                    //v3.Value = doc.GetElementsByTagName("cardBrand")[0].Attributes["code"].Value;
                    //v4.Value = doc.GetElementsByTagName("cardAcquirer")[0].Attributes["code"].Value;
                    //v5.Value = cc.PadLeft(2, '0').Substring(0, 1); //vk 03.17
                    //vk 07.20
                    string sTokef4 = FromXml(doc, "cardExpiration").PadLeft(4, '0');
                    string sTokef8;
                    switch (sTokef4.Substring(0, 2))
                    {
                        case "02":
                            if (int.Parse(sTokef4.Substring(2, 2)) % 4 == 0)
                                sTokef8 = "29";
                            else
                                sTokef8 = "28";
                            break;
                        case "04":
                        case "06":
                        case "09":
                        case "11":
                            sTokef8 = "30";
                            break;
                        default:
                            sTokef8 = "31";
                            break;
                    }
                    sTokef8 = sTokef8 + "/" + sTokef4.Substring(0, 2) + "/20" + sTokef4.Substring(2, 2);
                    v6.Value = sTokef8;
                    //v7.Value = doc.GetElementsByTagName("extendedCardType")[0].Attributes["code"].Value;
                    v7.Value = FromXml(doc, "id"); //vk 05.21
                    v8.Value = s;
                    //v9.Value = doc.GetElementsByTagName("cardType")[0].Attributes["code"].Value;
                    if (s == "")
                        return "ERROR:" + v2.Value + " (" + v1.Value + ")";
                    else
                    {
                        char p = dp.getProperty("CreditFiller").PadRight(1).ToCharArray()[0];
                        return s.Substring(s.Length - 4).PadLeft(16, p);
                    }
                default:
                    return "ERROR";
            }
        }
        private string FromXml(XmlDocument doc, string sElement) //vk 04.20
        {
            try
            {
                return doc.GetElementsByTagName(sElement)[0].InnerText;
            }
            catch
            {
                return "";
            }
        }
        //ntg 30.04.24 vladi change regarding CG functionallity - new function
        private string FromXml_Last(XmlDocument doc, string sElement)
        {
            try
            {
                return doc.GetElementsByTagName(sElement)[doc.GetElementsByTagName(sElement).Count - 1].InnerText;
            }
            catch
            {
                return "";
            }
        }

        public string GetUrl(Reader dp,
            System.Web.HttpRequest pReq,
            System.Web.SessionState.HttpSessionState pSes,
            HtmlInputHidden v1, HtmlInputHidden v2)
        {
            pSes["CG_Transaction"] = "";
            GetPaymentURL.GetPaymentURL_ESBGW eSBGateway = new GetPaymentURL.GetPaymentURL_ESBGW();
            eSBGateway.Timeout = 100000;
            eSBGateway.Url = dp.getProperty("WsUrl");
            WriteLog("CG URL TERMINAL", pReq["Hi1_value"], dp, pReq, pSes);
            if (dp.getProperty("CG_Cert") != "")
            {
                WriteLog("CG URL 1", "", dp, pReq, pSes);
                cert = Certificate(dp, pReq, pSes);
                WriteLog("CG URL 2", cert.ToString(), dp, pReq, pSes);
                eSBGateway.ClientCertificates.Add(cert);
                WriteLog("CG URL 3", "", dp, pReq, pSes);
                //eSBGateway.ClientCertificates.Add(X509Certificate.CreateFromCertFile(dp.getProperty("CG_Cert"))); //vk 09.16
            }
            WriteLog("CG URL 4", "", dp, pReq, pSes);
            GetPaymentURL.ashrait ashrait = new GetPaymentURL.ashrait();
            GetPaymentURL.ashraitRequest request = new GetPaymentURL.ashraitRequest();
            GetPaymentURL.ashraitRequestDoDeal doDeal = new GetPaymentURL.ashraitRequestDoDeal();
            doDeal.total = "0";
            doDeal.currency = "USD";
            doDeal.transactionType = "Debit";
            doDeal.creditType = "RegularCredit";
            doDeal.transactionCode = "Phone";
            doDeal.terminalNumber = pReq["Hi1_value"].PadLeft(7, '0'); //dp.getProperty("AbsTerminal"); // "0962832";
            doDeal.firstPayment = "";
            doDeal.periodicalPayment = "";
            doDeal.numberOfPayments = "";
            doDeal.user = "request identifier";
            doDeal.mid = dp.getProperty("Mid"); // "16";
            doDeal.uniqueid = "";
            doDeal.mpiValidation = "Token";
            doDeal.description = "";
            doDeal.email = "test@creditguard.co.il";
            doDeal.userData1 = "";
            request.doDeal = doDeal;
            ashrait.request = request;
            GetPaymentURL.ESBServiceRequestMetadata eSBServiceRequestMetadata = new GetPaymentURL.ESBServiceRequestMetadata();
            GetPaymentURL.Filter filter = new GetPaymentURL.Filter();
            filter.Code = "";
            filter.Value = "";
            eSBServiceRequestMetadata.Filter = filter;
            GetPaymentURL.Id id = new GetPaymentURL.Id();
            id.Code = "";
            id.Value = "";
            eSBServiceRequestMetadata.Id = id;
            GetPaymentURL.Process process = new GetPaymentURL.Process();
            process.ProcessCode = "";
            GetPaymentURL.Id id_33 = new GetPaymentURL.Id();
            id_33.Code = "";
            id_33.Value = "";
            process.Id = id_33;
            eSBServiceRequestMetadata.Process = process;
            eSBServiceRequestMetadata.CallingAppInstanceTrackingId = "";
            eSBServiceRequestMetadata.Version = "";
            eSBServiceRequestMetadata.Interface = "";
            eSBServiceRequestMetadata.Caller = "";
            eSBServiceRequestMetadata.ActivityTime = new System.DateTime(0);
            eSBServiceRequestMetadata.ActivityTimeSpecified = false;
            eSBServiceRequestMetadata.ServiceInstanceId = "";
            eSBServiceRequestMetadata.Workspace = "";
            eSBServiceRequestMetadata.CorrelationId = "";
            GetPaymentURL.ESBServiceResponseMetadata ESBServiceResponseMetadata;
            GetPaymentURL.ashrait1 requestResponseServiceResult = eSBGateway.GetPaymentURL_ReqResSvc(ashrait, eSBServiceRequestMetadata, out ESBServiceResponseMetadata);
            if (requestResponseServiceResult == null)
            {
                string sCode;
                string sDescr;
                try
                {
                    sCode = ESBServiceResponseMetadata.ResponseCode;
                    sDescr = ESBServiceResponseMetadata.ResponseDescription;
                }
                catch (Exception)
                {
                    sCode = "999";
                    sDescr = "No result";
                }
                WriteLog("CG URL ERR", sCode + " " + sDescr, dp, pReq, pSes);
                v1.Value = sCode;
                v2.Value = "GetPaymentURL: " + sDescr;
                return "ERROR";
            }
            else
            {
                string s = requestResponseServiceResult.response.doDeal.mpiHostedPageUrl;
                WriteLog("CG URL", s, dp, pReq, pSes);
                string s0 = requestResponseServiceResult.response.message;
                WriteLog("CG URL MSG", s0, dp, pReq, pSes);
                pSes["CG_Transaction"] = requestResponseServiceResult.response.doDeal.token;
                v1.Value = requestResponseServiceResult.response.result;
                v2.Value = s0;
                return s;
            }
        }
        public string GetToken(Reader dp,
            System.Web.HttpRequest pReq,
            System.Web.SessionState.HttpSessionState pSes,
            HtmlInputHidden v1, HtmlInputHidden v2, HtmlInputHidden v3,
            HtmlInputHidden v4, HtmlInputHidden v5, HtmlInputHidden v6,
            HtmlInputHidden v7, HtmlInputHidden v8, HtmlInputHidden v9)
        {
            GetPaymentToken.GetPaymentToken_ESBGW eSBGateway = new GetPaymentToken.GetPaymentToken_ESBGW();
            eSBGateway.Timeout = 100000;
            eSBGateway.Url = dp.getProperty("WsToken");
            WriteLog("CG TOKEN BEFORE", "", dp, pReq, pSes);
            if (dp.getProperty("CG_Cert") != "")
            {
                WriteLog("CG TOKEN 1", "", dp, pReq, pSes);
                cert = Certificate(dp, pReq, pSes);
                WriteLog("CG TOKEN 2", cert.ToString(), dp, pReq, pSes);
                eSBGateway.ClientCertificates.Add(cert);
                WriteLog("CG TOKEN 3", "", dp, pReq, pSes);
                //eSBGateway.ClientCertificates.Add(X509Certificate.CreateFromCertFile(dp.getProperty("CG_Cert"))); //vk 09.16
            }
            WriteLog("CG TOKEN 4", "", dp, pReq, pSes);
            GetPaymentToken.PaymentToken paymentToken = new GetPaymentToken.PaymentToken();
            GetPaymentToken.PaymentTokenRequest request = new GetPaymentToken.PaymentTokenRequest();
            request.mpiTransactionId = (string)pSes["CG_Transaction"]; // "0254c254-4681-45b6-9547-b6c0114f39ea";
            request.mid = dp.getProperty("Mid"); // "805";
            request.terminalNumber = pReq["Hi1_value"].PadLeft(7, '0'); // dp.getProperty("AbsTerminal"); // "0962832";
            paymentToken.Request = request;
            GetPaymentToken.ESBServiceRequestMetadata eSBServiceRequestMetadata = new GetPaymentToken.ESBServiceRequestMetadata();
            GetPaymentToken.Filter filter = new GetPaymentToken.Filter();
            filter.Code = "";
            filter.Value = "";
            eSBServiceRequestMetadata.Filter = filter;
            GetPaymentToken.Id id = new GetPaymentToken.Id();
            id.Code = "";
            id.Value = "";
            eSBServiceRequestMetadata.Id = id;
            GetPaymentToken.Process process = new GetPaymentToken.Process();
            process.ProcessCode = "";
            GetPaymentToken.Id id_18 = new GetPaymentToken.Id();
            id_18.Code = "";
            id_18.Value = "";
            process.Id = id_18;
            eSBServiceRequestMetadata.Process = process;
            eSBServiceRequestMetadata.CallingAppInstanceTrackingId = "";
            eSBServiceRequestMetadata.Version = "";
            eSBServiceRequestMetadata.Interface = "";
            eSBServiceRequestMetadata.Caller = "";
            eSBServiceRequestMetadata.ActivityTime = new System.DateTime(0);
            eSBServiceRequestMetadata.ActivityTimeSpecified = false;
            eSBServiceRequestMetadata.ServiceInstanceId = "";
            eSBServiceRequestMetadata.Workspace = "";
            eSBServiceRequestMetadata.CorrelationId = "";
            GetPaymentToken.ESBServiceResponseMetadata ESBServiceResponseMetadata;
            GetPaymentToken.PaymentToken1 requestResponseServiceResult = eSBGateway.GetPaymentToken_ReqResSvc(paymentToken, eSBServiceRequestMetadata, out ESBServiceResponseMetadata);
            if (requestResponseServiceResult == null)
            {
                string sCode;
                string sDescr;
                try
                {
                    sCode = ESBServiceResponseMetadata.ResponseCode;
                    sDescr = ESBServiceResponseMetadata.ResponseDescription;
                }
                catch (Exception)
                {
                    sCode = "999";
                    sDescr = "No result";
                }
                WriteLog("CG TOKEN ERR", sCode + " " + sDescr, dp, pReq, pSes);
                v1.Value = sCode;
                v2.Value = "GetPaymentToken: " + sDescr;
                return "ERROR";
            }
            else
            {
                string s = requestResponseServiceResult.response.inquireTransactions.row.cardId;
                //WriteLog("CG TOKEN", s, dp, pReq, pSes);
                string s0 = requestResponseServiceResult.response.inquireTransactions.row.errorText;
                //WriteLog("CG TOKEN MSG", s0, dp, pReq, pSes);
                string cc = requestResponseServiceResult.response.inquireTransactions.row.cgGatewayResponseXML.ashrait.response.doDeal.creditCompanyCode;
                v1.Value = requestResponseServiceResult.response.result;
                v2.Value = s0;
                v3.Value = requestResponseServiceResult.response.inquireTransactions.row.cgGatewayResponseXML.ashrait.response.doDeal.cardBrandCode;
                v4.Value = requestResponseServiceResult.response.inquireTransactions.row.cgGatewayResponseXML.ashrait.response.doDeal.cardAcquirerCode;
                v5.Value = cc.PadLeft(2, '0').Substring(0, 1); //vk 03.17
                v6.Value = requestResponseServiceResult.response.inquireTransactions.row.cgGatewayResponseXML.ashrait.response.doDeal.cardExpiration;
                v7.Value = requestResponseServiceResult.response.inquireTransactions.row.cgGatewayResponseXML.ashrait.response.doDeal.extendedCardType;
                v8.Value = s;
                v9.Value = requestResponseServiceResult.response.inquireTransactions.row.cgGatewayResponseXML.ashrait.response.doDeal.cardType;
                try
                {
                    //vk 03.17
                    string log = "";
                    log += "v1/result=" + requestResponseServiceResult.response.result + ";";
                    log += "v2/errorText=" + s0 + ";";
                    log += "v3/cardBrandCode=" + requestResponseServiceResult.response.inquireTransactions.row.cgGatewayResponseXML.ashrait.response.doDeal.cardBrandCode + ";";
                    log += "v4/cardAcquirerCode=" + requestResponseServiceResult.response.inquireTransactions.row.cgGatewayResponseXML.ashrait.response.doDeal.cardAcquirerCode + ";";
                    //log += "v5/creditCompanyCode=" + requestResponseServiceResult.response.inquireTransactions.row.cgGatewayResponseXML.ashrait.response.doDeal.creditCompanyCode + ";";
                    log += "v5/creditCompanyCode=" + cc.PadLeft(2, '0').Substring(0, 1) + "/" + cc + ";";
                    log += "v6/cardExpiration=" + requestResponseServiceResult.response.inquireTransactions.row.cgGatewayResponseXML.ashrait.response.doDeal.cardExpiration + ";";
                    log += "v7/extendedCardType=" + requestResponseServiceResult.response.inquireTransactions.row.cgGatewayResponseXML.ashrait.response.doDeal.extendedCardType + ";";
                    log += "v8/cardId=" + s + ";";
                    log += "v9/cardType=" + requestResponseServiceResult.response.inquireTransactions.row.cgGatewayResponseXML.ashrait.response.doDeal.cardType + ";";
                    WriteLog("CG TOKEN DATA", log, dp, pReq, pSes);
                }
                catch (Exception e)
                {
                    WriteLog("CG TOKEN DATA ERROR", "Error writing log", dp, pReq, pSes, e);
                }
                char p = dp.getProperty("CreditFiller").PadRight(1).ToCharArray()[0];
                return s.Substring(s.Length - 4).PadLeft(16, p);
            }
        }
    }
}
