using Comtec.AS400XML;
using Comtec.AS400XML.Output.Model.Screen;
using Comtec.BE.Config;
using Comtec.BE.LogEx;
using System.Reflection;

namespace Sample.AS400XML {
    internal class Program {
        static void Main(string[] args) {
            AppConfigHandler.Init();
            LogHelper.Init(AppConfigHandler.Data.Log);

            AS400XmlParser as400XmlParser = new AS400XmlParser();

            // ream xml files from directory
            //string xmlDirectory = "C:\\Work_Tfs";
            //string xmlDirectory = "C:\\Work_Git\\Tdi2\\AS400Xml\\ronnie.k";
            //string xmlDirectory = "\\\\172.17.17.2\\ComDisplay_Sites\\TdiBth";
            //string xmlDirectory = "\\\\Com-files01\\user\\DOC_TIS\\GENERAL\\TIS\\TDI תיעוד מערכת";
            //string xmlDirectory = "C:\\Work_Git\\Tdi2\\AS400XmlFiles\\TDI_xml";
            string xmlDirectory = "C:/Work_Git/Tdi2/AS400XmlFiles";

            int counter = as400XmlParser.DeserializeFilesInDirectory<ScreenXmlRoot>(xmlDirectory);
            LogHelper.WriteInformation(MethodBase.GetCurrentMethod(), $"{counter} files");


            // read xml file path
            string xmlFilePath = "./Output/trial/page1FromAsaf.xml";
            //string xmlFilePath = "./Output/Template/XmlTemplateWithAttributes.xml";
            //string xmlFilePath = "/Output/trial/sample.xml";
            //string xmlFilePath = "./Output/Template/XmlTemplate/XmlTemplate.xml";
            //string xmlFilePath = "./Output/Data/xml2.xml";
            //string xmlFilePath = "C:/Work_Git/Tdi2/AS400Xml/ronnie.k/tmp-0fxwrgcx0ovtyhqtqxbl5zgo-InputFile.xml";

            //XmlFileModel xmlFileModel = as400XmlParser.DeserializeFilePath<ScreenXmlRoot>(xmlFilePath);
            //LogHelper.WriteInformation(MethodBase.GetCurrentMethod(), xmlFileModel.JsonIndented);

            // read xml string
            string str =
                "<?xml version='1.0' encoding='utf-8'?>\r\n<screen>\r\n    <s fil='HFFLNAVFM ' rec='RLISTDDS  ' fpc='0006' flib='CRMVNL    ' cli='055' fsid='L' dtk='20221102' dtr='20221102' ver='003' fnn='1  1       11     1  1 1  ' adr='R' qpxl='00000' qflx='Q' qstr='06' qend='24'/>\r\n    <cmd>\r\n        <c fk='01' ft='F1  =אבה רדלופ' len='00020'/>\r\n        <c fk='13' ft='F13 =בושיח' len='00020'/>\r\n        <c fk='24' ft='F24 =הרימש אלל אצ' cgr='1X' len='00020'/>\r\n    </cmd>\r\n    <folder>\r\n        <l lk='51' ltr='N' lon='Y' ln='             םייללכ םיטרפ' icn='0001'/>\r\n        <l lk='52' ltr='N' lon='Y' ln='                 בכר יטרפ' icn='0003'/>\r\n        <l lk='53' ltr='N' lon='Y' ln='              יוסיכה יטרפ' icn='0003'/>\r\n        <l lk='54' ltr='N' lon='Y' ln='               םיגהנ יטרפ' icn='0011'/>\r\n        <l lk='55' ltr='Y' lon='Y' ln='              +יחוטיב רבע' icn='0004'/>\r\n        <l lk='56' ltr='N' lon='Y' ln='          תובחרהו םייוסיכ' icn='0005'/>\r\n        <l lk='57' ltr='N' lon='Y' ln='          םינוגימ/םירזיבא' icn='0002'/>\r\n        <l lk='58' ltr='N' lon='Y' ln='            תופסותו תוחנה' icn='0014'/>\r\n        <l lk='59' ltr='N' lon='Y' ln='                   דובעיש' icn='0007'/>\r\n        <l lk='60' ltr='N' lon='Y' ln='              הריכמ םוכיס' icn='0009'/>\r\n        <l lk='80' ltr='N' lon='Y' ln='           תויוליעפ תמישר'/>\r\n    </folder></screen>\r\n";

            //AS400XmlOutputModel as400XmlOutputModel = as400XmlParser.DeserializeString<ScreenXmlRoot>(str);
            //LogHelper.WriteInformation(MethodBase.GetCurrentMethod(), as400XmlOutputModel.JsonIndented);



            LogHelper.WriteInformation(MethodBase.GetCurrentMethod(), $"File {xmlFilePath} Finished");
            LogHelper.WriteInformation(MethodBase.GetCurrentMethod(), "========================================================================");
        }
    }
}