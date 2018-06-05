using System.Collections.Generic;
using System.Xml.Serialization;

namespace Framework.ObjectModule
{
    [XmlRoot("Schemas")]
    public class Schemas
    {
        [XmlElement(ElementName = "Schema")]
        public List<Schema> Tables { get; set; }
    }

    public class Schema
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "Attribute")]
        public List<Attribute> Attributes { get; set; }
    }

    public class Attribute
    {
        [XmlAttribute(AttributeName = "AttributeName")]
        public string AttributeName { get; set; }

        [XmlAttribute(AttributeName = "AttributeType")]
        public string AttributeType { get; set; }

        [XmlAttribute(AttributeName = "AttributeSize")]
        public string AttributeSize { get; set; }

        [XmlAttribute(AttributeName = "IsPrimaryKey")]
        public bool IsPrimaryKey { get; set; }

        [XmlAttribute(AttributeName = "IsNotNull")]
        public bool IsNotNull { get; set; }
    }
}
