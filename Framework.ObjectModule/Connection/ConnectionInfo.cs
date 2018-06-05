using System.Collections.Generic;
using System.Xml.Serialization;

namespace Framework.ObjectModule
{
    [XmlRoot("TargetDbs")]
    public class TargetDbs
    {
        [XmlElement(ElementName = "ConnectionInfo")]
        public List<ConnectionInfo> Infos { get; set; }
    }

    public class ConnectionInfo
    {
        [XmlAttribute(AttributeName = "ConnectionName")]
        public string ConnectionName { get; set; }

        [XmlAttribute(AttributeName = "DBType")]
        public DBType DBType { get; set; }

        [XmlAttribute(AttributeName = "DataSource")]
        public string DataSource { get; set; }

        [XmlAttribute(AttributeName = "InitialCatalog")]
        public string InitialCatalog { get; set; }

        [XmlAttribute(AttributeName = "UserId")]
        public string UserId { get; set; }

        [XmlAttribute(AttributeName = "Password")]
        public string Password { get; set; }
    }

    public enum DBType
    {
        [XmlEnum(Name = "SqlServer")]
        SqlServer = 0,
        [XmlEnum(Name = "Oracle")]
        Oracle = 1,
        [XmlEnum(Name = "MySql")]
        MySql = 2
    }
}
