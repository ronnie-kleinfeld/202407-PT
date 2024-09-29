using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Xml.XmlTemplate;

public class SingleChildElement1
{
    [XmlAttribute("attr1")]
    public string Attr1
    {
        get; set;
    }
}