using System.Xml.Serialization;

namespace Comtec.AS400XML.Output.trial.XmlTemplate
{
    [XmlRoot("xmlTemplate")]
    public class XmlTemplate
    {
        [XmlAttribute("attr1")]
        public string Attr1
        {
            get; set;
        }

        [XmlElement("singleElement1")]
        public SingleElement1 SingleElement1
        {
            get; set;
        }

        [XmlElement("singleElement2")]
        public SingleElement2 SingleElement2
        {
            get; set;
        }

        [XmlElement("listOfElements")]
        public List<ListOfElements> ListOfElements
        {
            get; set;
        }

        [XmlElement("parentElementEmpty1")]
        public ParentElementEmpty1 ParentElementEmpty1
        {
            get; set;
        }

        [XmlElement("parentElementW1Element1")]
        public ParentElementW1Element1 ParentElementW1Element1
        {
            get; set;
        }

        [XmlElement("parentElementW1Element2")]
        public ParentElementW1Element2 ParentElementW1Element2
        {
            get; set;
        }

        [XmlElement("parentElementW1Element3")]
        public ParentElementW1Element3 ParentElementW1Element3
        {
            get; set;
        }

        [XmlElement("listOfParentElementWElement1")]
        public List<ListOfParentElementWElement1> ListOfParentElementWElement1
        {
            get; set;
        }

        [XmlElement("listOfParentElementWElement2")]
        public List<ListOfParentElementWElement2> ListOfParentElementWElement2
        {
            get; set;
        }

        [XmlElement("listOfParentElementWElement3")]
        public List<ListOfParentElementWElement3> ListOfParentElementWElement3
        {
            get; set;
        }
    }
}