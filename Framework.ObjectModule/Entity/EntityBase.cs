using System.Collections.Generic;

namespace Framework.ObjectModule
{
    public class EntityBase
    {
        public EntityBase()
        {
            Attributes = new Dictionary<string, EntityAttributeBase>();
        }
        public string Id { get; set; }
        public string EntityName { get; set; }
        public Dictionary<string, EntityAttributeBase> Attributes { get; set; }
    }
}
