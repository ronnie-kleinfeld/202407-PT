using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.trial.XmlTemplate;

public class SingleChildElement
{
    [XmlAttribute("attr1")]
    public string Attr1
    {
        get; set;
    }
}