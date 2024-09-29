using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.trial.XmlTemplate;

public class ListOfParentElementWElement1
{
    [XmlAttribute("attr1")]
    public string Attr1
    {
        get; set;
    }

    [XmlElement("singleChildElement1")]
    public SingleChildElement1 SingleChildElement1
    {
        get; set;
    }
}