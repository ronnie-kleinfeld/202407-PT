using Comtec.AS400XML.Output.Context;
using Comtec.BE.Helpers;
using Comtec.BE.LogEx;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json.Linq;

namespace Comtec.AS400XML.Output.Model.AllowedData {
    public class AllowedDataHandler {
        // singleton
        private static AllowedDataHandler _instance;
        public static AllowedDataHandler Instance {
            get {
                if (_instance == null) {
                    _instance = new AllowedDataHandler();
                    _instance.Init();
                }
                return _instance;
            }
        }

        private Dictionary<string, List<string>> Data;

        internal List<string> GetAllowedData(string tableName, string columnName) {
            string dicKey = $"{tableName}:{columnName}";

            if (Data.ContainsKey(dicKey)) {
                // this slows the process too much, must find a different way
                //AS400XmlContext context = new AS400XmlContext();
                //string sql = $"select count(distinct {columnName}) from {tableName} where {columnName} is not null";
                //int count = context.Database.SqlQueryRaw<int>(sql).ToList()[0];
                //if (Data[dicKey].Count != count) {
                //    Data[dicKey] = new List<string>();
                //    return GetAllowedData(tableName, columnName);
                //}
            } else {
                Data[dicKey] = new List<string>();

                AS400XmlContext context = new AS400XmlContext();
                string sql = $"select distinct {columnName} from {tableName} where {columnName} is not null order by {columnName}";

                string csv = "";
                foreach (var value in context.Database.SqlQueryRaw<string>(sql).ToList()) {
                    Data[dicKey].Add(value);
                    csv += $"[{value}] ";
                }

                LogHelper.WriteInformation(MethodBase.GetCurrentMethod(), $"{dicKey} has {Data[dicKey].Count} entries {csv}");
            }

            return Data[dicKey];
        }

        internal void Init() {
            Data = new();
        }
        public string ToString(string tableName, string columnName) {
            AS400XmlContext context = new AS400XmlContext();
            string sql = $"select distinct {columnName} from {tableName} where {columnName} is not null order by {columnName}";

            var results = context.Database
                .SqlQueryRaw<XmlAttributeCount>(@"SELECT " + columnName + " AS Attribute, COUNT(*) AS Count FROM " + tableName + " GROUP BY " + columnName + " ORDER BY Count DESC").ToList();

            StringBuilder sb = new StringBuilder();
            sb.Append(tableName);
            sb.Append(":");
            sb.Append(columnName);
            sb.Append("\t");
            foreach (var item in results) {
                sb.Append($" [{StringHelper.ReverseIfHebrew(item.Attribute)}:{item.Count}]");
            }

            return sb.ToString();
        }

        public string ToString() {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(ToString("ArXmlElement", "FicnXmlAttribute"));
            sb.AppendLine(ToString("ArXmlElement", "FtitXmlAttribute"));
            sb.AppendLine(ToString("ArXmlElement", "RclXmlAttribute"));
            sb.AppendLine(ToString("ArXmlElement", "Rx1XmlAttribute"));
            sb.AppendLine(ToString("ArXmlElement", "Rx2XmlAttribute"));
            sb.AppendLine(ToString("ArXmlElement", "Ry1XmlAttribute"));
            sb.AppendLine(ToString("ArXmlElement", "Ry2XmlAttribute"));
            sb.AppendLine(ToString("BXmlElement", "NmXmlAttribute"));
            sb.AppendLine(ToString("BXmlElement", "OpXmlAttribute"));
            sb.AppendLine(ToString("BXmlElement", "RoXmlAttribute"));
            sb.AppendLine(ToString("CXmlElement", "CgrXmlAttribute"));
            sb.AppendLine(ToString("CXmlElement", "FbuaXmlAttribute"));
            sb.AppendLine(ToString("CXmlElement", "FkXmlAttribute"));
            sb.AppendLine(ToString("CXmlElement", "FtXmlAttribute"));
            sb.AppendLine(ToString("CXmlElement", "LenXmlAttribute"));
            sb.AppendLine(ToString("CXmlElement", "PbgXmlAttribute"));
            sb.AppendLine(ToString("CXmlElement", "PicXmlAttribute"));
            sb.AppendLine(ToString("CXmlElement", "PntXmlAttribute"));
            sb.AppendLine(ToString("CXmlElement", "PszXmlAttribute"));
            sb.AppendLine(ToString("DXmlElement", "QtypXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "AliasXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "AprXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "BkgXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "BuaXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "ChbXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "ColXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "CryXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "DecXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "DfsXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "EdtXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "EmlXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "EwrXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "F4PXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "HebXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "HkyXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "IndXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "InpXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "LenXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "LinXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "LnkXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "LvlXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "ManXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "MaxXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "MinXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "NumXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "NumvXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "OnlyXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "PbgXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "PchXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "PchlXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "PclXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "PerXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "Pf4XmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "PfnXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "PhiXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "PhtXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "PicXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "PkvXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "PlcXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "PntXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "PrlXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "PslXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "PszXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "PtpXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "PulXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "PwdXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "PxkXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "PxmXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "PxnXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "PxrXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "QfkXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "RtpXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "SgnXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "TabXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "TchXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "TipXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "TypXmlAttribute"));
            //sb.AppendLine(ToString("FXmlElement", "ValXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "WdXmlAttribute"));
            sb.AppendLine(ToString("FXmlElement", "PfkXmlAttribute"));
            sb.AppendLine(ToString("LXmlElement", "FclXmlAttribute"));
            sb.AppendLine(ToString("LXmlElement", "IcnXmlAttribute"));
            sb.AppendLine(ToString("LXmlElement", "LkXmlAttribute"));
            sb.AppendLine(ToString("LXmlElement", "LnXmlAttribute"));
            sb.AppendLine(ToString("LXmlElement", "LonXmlAttribute"));
            sb.AppendLine(ToString("LXmlElement", "LtrXmlAttribute"));
            sb.AppendLine(ToString("RXmlElement", "BkgXmlAttribute"));
            sb.AppendLine(ToString("RXmlElement", "FicnXmlAttribute"));
            sb.AppendLine(ToString("RXmlElement", "FtitXmlAttribute"));
            sb.AppendLine(ToString("RXmlElement", "RclXmlAttribute"));
            sb.AppendLine(ToString("RXmlElement", "Rx1XmlAttribute"));
            sb.AppendLine(ToString("RXmlElement", "Rx2XmlAttribute"));
            sb.AppendLine(ToString("RXmlElement", "Ry1XmlAttribute"));
            sb.AppendLine(ToString("RXmlElement", "Ry2XmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "AdrXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "ArwXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "CliXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "DtkXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "DtrXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "DspaXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "FchXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "FcmdXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "FdspXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "FgrXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "FilXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "FindXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "FlangXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "FldXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "FlibXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "FlpXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "FlrXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "FnnXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "FortXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "FpcXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "FpsXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "FsidXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "GrkXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "JacketXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "MsgXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "PicXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "PlgcXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "PntXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "PsetXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "PxrXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "QendXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "QflxXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "QpxlXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "QstrXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "RecXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "SpoXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "SrgXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "StrcXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "VerXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "WaitXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "WcolXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "WcstXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "WinXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "WlinXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "WlstXmlAttribute"));
            sb.AppendLine(ToString("SXmlElement", "WsmlXmlAttribute"));
            sb.AppendLine(ToString("XXmlElement", "BkgXmlAttribute"));
            sb.AppendLine(ToString("XXmlElement", "XnXmlAttribute"));
            sb.AppendLine(ToString("XXmlElement", "XsXmlAttribute"));
            sb.AppendLine(ToString("YXmlElement", "BkgXmlAttribute"));
            sb.AppendLine(ToString("YXmlElement", "YnXmlAttribute"));
            sb.AppendLine(ToString("YXmlElement", "YsXmlAttribute"));
            sb.AppendLine(ToString("YXmlElement", "YtXmlAttribute"));
            sb.AppendLine(ToString("ZXmlElement", "ZclXmlAttribute"));
            sb.AppendLine(ToString("ZXmlElement", "ZlnXmlAttribute"));

            return sb.ToString();
        }
    }
}