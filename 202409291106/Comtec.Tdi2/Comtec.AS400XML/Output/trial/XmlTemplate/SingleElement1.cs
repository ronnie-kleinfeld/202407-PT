using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.trial.XmlTemplate;

public class SingleElement1
{
    [XmlAttribute("attr1")]
    public string Attr1
    {
        get; set;
    }

    [XmlAttribute("attr1a")]
    public string Attr1AXmlAttribute
    {
        get; set;
    } // maybe in class SingleElement1

    [XmlAttribute("attr1b")]
    public string Attr1BXmlAttribute
    {
        get; set;
    } // maybe in class SingleElement1
}