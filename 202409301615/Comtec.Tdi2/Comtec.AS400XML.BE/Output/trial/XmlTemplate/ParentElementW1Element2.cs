using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Xml.XmlTemplate;

public class ParentElementW1Element2
{
    [XmlAttribute("attr1")]
    public string Attr1
    {
        get; set;
    }

    [XmlElement("singleChildElement2")]
    public SingleChildElement2 SingleChildElement2
    {
        get; set;
    }
}