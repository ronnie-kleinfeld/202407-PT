using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.trial.XmlTemplate;

public class ParentElementW1Element3
{
    [XmlAttribute("attr1")]
    public string Attr1
    {
        get; set;
    }

    [XmlElement("listOfChildElements")]
    public List<ListOfChildElements> ListOfChildElements
    {
        get; set;
    }
}