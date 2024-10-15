using Comtec.AS400XML.Output.Context;
using Comtec.AS400XML.Output.Model;
using Comtec.AS400XML.Output.Model.Screen;
using Comtec.BE.Config;
using Comtec.BE.Helpers;
using Comtec.BE.LogEx;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace Comtec.AS400XML.Output {
    public class AS400XmlParser {
        private StringBuilder _elements;
        private Dictionary<string, Dictionary<string, StringBuilder>> _attributes;

        public AS400XmlParser() {
        }

        public int DeserializeDirectoriesInDirectory<T>(string directory, string searchPattern) where T : ScreenOutputXmlRoot {
            int counter = 0;
            counter += DeserializeFilesInDirectory<T>(directory, SearchOption.TopDirectoryOnly);

            string[] xmlDirectories = Directory.GetDirectories(directory, searchPattern);

            foreach (var xmlDirectory in xmlDirectories) {
                try {
                    LogHelper.WriteInformation(MethodBase.GetCurrentMethod(), $"Directory {xmlDirectory} Started");

                    counter += DeserializeFilesInDirectory<T>(xmlDirectory);

                    LogHelper.WriteInformation(MethodBase.GetCurrentMethod(), $"Directory {xmlDirectory} Finished");
                    LogHelper.WriteInformation(MethodBase.GetCurrentMethod(), "========================================================================");
                } catch (Exception ex) {
                    LogHelper.WriteFatal(MethodBase.GetCurrentMethod(), $"An error occurred while processing directory {xmlDirectory}: {ex.Message}");
                }
            }

            return counter;
        }

        public int DeserializeFilesInDirectory<T>(string directory, SearchOption searchOption = SearchOption.AllDirectories) where T : ScreenOutputXmlRoot {
            string[] xmlFiles = Directory.GetFiles(directory, "*.xml", searchOption);
            int counter = 0;

            foreach (string xmlFilePath in xmlFiles) {
                try {
                    counter++;
                    LogHelper.WriteInformation(MethodBase.GetCurrentMethod(), $"File {xmlFilePath}");

                    AS400XmlOutputModel as400XmlOutput = DeserializeFilePath<T>(xmlFilePath);

                    if (AppConfigHandler.Data.AS400Xml.WriteToDatabase) {
                        AS400XmlContext as400XmlContext = new AS400XmlContext();
                        if (as400XmlContext.AS400XmlOutput.Count(x => x.Hash == as400XmlOutput.Hash) == 0) {
                            as400XmlContext.AS400XmlOutput.Add(as400XmlOutput);
                            as400XmlContext.SaveChanges();
                        }
                    }
                } catch (Microsoft.Data.SqlClient.SqlException ex) {
                    if (ex.Message.Contains("Invalid column name")) {
                        LogHelper.WriteFatal(MethodBase.GetCurrentMethod(), $"RONNIE: Possible missing column in table. Maybe added an attribute to the XmlFileModel but forgot to add-migration to the Entity Framework. (InnerException message:{ex.Message})");
                    }
                } catch (Exception ex) {
                    LogHelper.WriteFatal(MethodBase.GetCurrentMethod(), $"An error occurred while processing file {xmlFilePath}: {ex.Message} {ex.InnerException?.Message} {ex.InnerException?.InnerException?.Message}");
                }
            }

            return counter;
        }

        public AS400XmlOutputModel DeserializeFilePath<T>(string xmlFilePath) where T : ScreenOutputXmlRoot {
            string rawXml = File.ReadAllText(xmlFilePath);

            AS400XmlOutputModel as400XmlOutput = DeserializeString<T>(rawXml);
            as400XmlOutput.XmlFilePath = xmlFilePath;
            as400XmlOutput.GetFullPath = Path.GetFullPath(xmlFilePath);
            as400XmlOutput.RawXml = rawXml;
            as400XmlOutput.Hash = HashHelper.OneWayHash(rawXml);

            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };
            as400XmlOutput.Json = JsonConvert.SerializeObject(as400XmlOutput, jsonSerializerSettings);

            return as400XmlOutput;
        }

        public AS400XmlOutputModel DeserializeString<T>(string xmlString) where T : ScreenOutputXmlRoot {
            AS400XmlOutputModel result = new AS400XmlOutputModel();

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.UnknownAttribute += Serializer_UnknownAttribute;
            serializer.UnknownElement += Serializer_UnknownElement;
            serializer.UnknownNode += Serializer_UnknownNode;
            serializer.UnreferencedObject += Serializer_UnreferencedObject;

            _elements = new StringBuilder();
            _attributes = new Dictionary<string, Dictionary<string, StringBuilder>>();

            using (StringReader reader = new StringReader(xmlString)) {
                T screenXmlRoot = (T)serializer.Deserialize(reader);
                if (AppConfigHandler.Data.AS400Xml.ValidateAllowedData) {
                    screenXmlRoot?.IsValidXmlElement();
                }

                result.ScreenXmlRoot = screenXmlRoot;
            }

            if (_elements.Length > 0) {
                LogHelper.WriteError(MethodBase.GetCurrentMethod(), _elements.ToString());
            }

            StringBuilder sb = new StringBuilder();
            foreach (var item in _attributes.OrderBy(x => x.Key)) {
                sb.AppendLine($"// missing code in class {item.Key}");
                sb.AppendLine($"public class {item.Key} {{");
                foreach (var attribute in item.Value) {
                    sb.AppendLine(attribute.Value.ToString());
                }
                sb.AppendLine("}}");
            }

            if (sb.Length > 0) {
                LogHelper.WriteError(MethodBase.GetCurrentMethod(), "\r\n" + sb.ToString());
            }

            return result;
        }

        private void Serializer_UnreferencedObject(object? sender, UnreferencedObjectEventArgs e) {
            LogHelper.WriteError(MethodBase.GetCurrentMethod(), $"Serializer_UnreferencedObject {sender} {e}");
        }

        private void Serializer_UnknownNode(object? sender, XmlNodeEventArgs e) {
            //LogHelper.WriteError(MethodBase.GetCurrentMethod(), $"Serializer_UnknownNode {sender} {e}");
        }

        private void Serializer_UnknownElement(object? sender, XmlElementEventArgs e) {
            _elements.AppendLine($"// unknown element in line {e.LineNumber} of xml: {e.Element.OuterXml} check parent element to find parent class");
        }

        private void Serializer_UnknownAttribute(object? sender, XmlAttributeEventArgs e) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"// unknown attribute in line {e.LineNumber} of xml: {e.Attr.OuterXml}");

            string className = e.ObjectBeingDeserialized.ToString().Split('.').Last();
            string attributeName = $"{StringHelper.ToProperCase(e.Attr.Name)}XmlAttribute";

            if (!_attributes.ContainsKey(className)) {
                _attributes[className] = new Dictionary<string, StringBuilder>();
            }
            if (!_attributes[className].ContainsKey(e.Attr.Name)) {
                _attributes[className][e.Attr.Name] = new StringBuilder();

                _attributes[className][e.Attr.Name].AppendLine($"    [XmlAttribute(\"{e.Attr.Name}\"), MaxLength(1000), AS400XmlAllowedValues]");
                _attributes[className][e.Attr.Name].AppendLine($"    public string? {attributeName} {{");
                _attributes[className][e.Attr.Name].AppendLine($"        get; set;");
                _attributes[className][e.Attr.Name].AppendLine($"    }}");
            }
        }
    }
}