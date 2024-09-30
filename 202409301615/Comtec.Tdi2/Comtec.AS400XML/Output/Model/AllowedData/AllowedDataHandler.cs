using Comtec.AS400XML.DL.Context;
using Comtec.BE.LogEx;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Comtec.AS400XML.Output.Model.AllowedData {
    public class AllowedDataHandler {
        // singleton
        private static AllowedDataHandler _instance;
        internal static AllowedDataHandler Instance {
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

            if (!Data.ContainsKey(dicKey)) {
                Data[dicKey] = new List<string>();

                AS400XmlContext context = new AS400XmlContext();
                string sql = $"select {columnName} from {tableName} where not ({columnName} is null) group by {columnName} order by {columnName}";

                string csv = "";
                foreach (var value in context.Database.SqlQueryRaw<string>(sql).ToList()) {
                    Data[dicKey].Add(value);
                    csv += value + ", ";
                }

                LogHelper.WriteInformation(MethodBase.GetCurrentMethod(), $"{dicKey} has {Data[dicKey].Count} entries {csv}");
            }

            return Data[dicKey];
        }

        /// </summary>
        internal void Init() {
            Data = new();
        }
    }
}