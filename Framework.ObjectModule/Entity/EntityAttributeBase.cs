namespace Framework.ObjectModule
{
    public class EntityAttributeBase
    {
        public string Id { get; set; }
        public string EntityId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeType { get; set; }
        public string AttributeSize { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsNotNull { get; set; }
    }
}
