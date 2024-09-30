using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.Xml.XmlTemplate;

public class ListOfParentElementWElement2
{
    [XmlAttribute("attr1")]
    public string Attr1
    {
        get; set;
    }

    [XmlElement("singleChildElement")]
    public SingleChildElement SingleChildElement
    {
        get; set;
    }
}