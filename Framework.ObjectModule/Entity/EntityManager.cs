using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using System;
using System.Collections.Generic;

namespace Framework.ObjectModule
{
    public class EntityManager
    {
        public EntityManager()
        {

        }

        private static Dictionary<string, Dictionary<string, EntityBase>> _datasourceInfo;

        /// <summary>
        /// 将ConnectionName和Table.xml进行映射
        /// </summary>
        /// <param name="connectionName"></param>
        /// <param name="tableXmlPath"></param>
        public static void Assemble(string connectionName, string tableXmlPath)
        {
            if (_datasourceInfo == null)
            {
                _datasourceInfo = new Dictionary<string, Dictionary<string, EntityBase>>();
            }
            Dictionary<string, ConnectionInfo> connDic = ConnectionProviderBase.GetAllConnectionInfos();
            if (connDic.ContainsKey(connectionName))
            {
                Dictionary<string, EntityBase> entityDic = ConvertTableXmlToEntityDic(tableXmlPath);
                if (!_datasourceInfo.ContainsKey(connectionName))
                {

                    _datasourceInfo.Add(connectionName, entityDic);
                }
                else
                {
                    foreach (var key in entityDic.Keys)
                    {
                        if (!_datasourceInfo[connectionName].ContainsKey(key))
                        {
                            _datasourceInfo[connectionName].Add(key, entityDic[key]);
                        }
                    }
                }
            }



        }

        /// <summary>
        /// 初始化，其中包括同步Table.xml信息到数据库，并且加载新的Entity信息到内存中
        /// </summary>
        public static void Init()
        {
            var dbSource = new Dictionary<string, Dictionary<string, EntityBase>>();
            InjectContainer.LoadConnection();
            Dictionary<string, ConnectionInfo> connDic = ConnectionProviderBase.GetAllConnectionInfos();
            if (connDic.Count > 0)
            {
                List<string> removeConns = new List<string>();
                foreach (var key in connDic.Keys)
                {
                    if (_datasourceInfo.ContainsKey(key))
                    {
                        var dbType = connDic[key].DBType;
                        IDBOperatorBase manager = InjectContainer.Resolve<IDBOperatorBase>(dbType.ToString());
                        if (!manager.DatabaseExist(key))
                        {
                            manager.CreateDatabase(key);
                        }
                        if (!manager.TableExist("EntityBase", key))
                        {
                            manager.CreateDefaultTable(key);
                        }
                        var entityDic = manager.QueryEntityDic(key);
                        dbSource.Add(key, entityDic);
                    }
                    else
                    {
                        removeConns.Add(key);
                    }
                }
                ConnectionProviderBase.DeleteConnections(removeConns);
            }
            Compare(dbSource, _datasourceInfo);
        }

        public static EntityBase GetEntity(string connectionName, string tableName)
        {
            return _datasourceInfo[connectionName][tableName];
        }

        #region private
        private static Dictionary<string, EntityBase> ConvertTableXmlToEntityDic(string tableXmlPath)
        {
            Dictionary<string, EntityBase> entityDic = new Dictionary<string, EntityBase>();
            Schemas schemas = SerializeUtil.Deserialize<Schemas>(tableXmlPath);
            if (schemas != null && schemas.Tables.Count > 0)
            {
                foreach (var schema in schemas.Tables)
                {
                    if (!entityDic.ContainsKey(schema.Name))
                    {
                        EntityBase entityBase = new EntityBase { EntityName = schema.Name };
                        foreach (var attr in schema.Attributes)
                        {
                            EntityAttributeBase attribute = new EntityAttributeBase
                            {
                                AttributeName = attr.AttributeName,
                                AttributeType = attr.AttributeType,
                                AttributeSize = attr.AttributeSize,
                                IsPrimaryKey = attr.IsPrimaryKey,
                                IsNotNull = attr.IsNotNull
                            };
                            if (!entityBase.Attributes.ContainsKey(attr.AttributeName))
                            {
                                entityBase.Attributes.Add(attr.AttributeName, attribute);
                            }
                        }
                        entityDic.Add(schema.Name, entityBase);
                    }
                }
            }
            return entityDic;
        }

        private static void Compare(Dictionary<string, Dictionary<string, EntityBase>> dbDic, Dictionary<string, Dictionary<string, EntityBase>> newDic)
        {
            Dictionary<string, ConnectionInfo> connDic = ConnectionProviderBase.GetAllConnectionInfos();

            foreach (var conn in newDic.Keys)
            {
                IDBOperatorBase manager = InjectContainer.Resolve<IDBOperatorBase>(connDic[conn].DBType.ToString());



                foreach (var entityName in newDic[conn].Keys)
                {
                    if (!dbDic[conn].ContainsKey(entityName))
                    {
                        //新增表
                        manager.CreateTable(newDic[conn][entityName], conn);
                        newDic[conn][entityName].Id = Guid.NewGuid().ToString();
                        manager.InsertIntoEntityBase(newDic[conn][entityName], conn);
                        foreach (var attr in newDic[conn][entityName].Attributes.Keys)
                        {
                            newDic[conn][entityName].Attributes[attr].Id = Guid.NewGuid().ToString();
                            newDic[conn][entityName].Attributes[attr].EntityId = newDic[conn][entityName].Id;
                            manager.InsertIntoEntityAttributeBase(newDic[conn][entityName].Attributes[attr], conn);
                        }
                    }
                    else
                    {
                        newDic[conn][entityName].Id = dbDic[conn][entityName].Id;
                        foreach (var attrName in newDic[conn][entityName].Attributes.Keys)
                        {
                            newDic[conn][entityName].Attributes[attrName].EntityId = newDic[conn][entityName].Id;
                            if (!dbDic[conn][entityName].Attributes.ContainsKey(attrName))
                            {
                                //新增字段
                                manager.CreateField(entityName, newDic[conn][entityName].Attributes[attrName], conn);
                                newDic[conn][entityName].Attributes[attrName].Id = Guid.NewGuid().ToString();
                                manager.InsertIntoEntityAttributeBase(newDic[conn][entityName].Attributes[attrName], conn);
                            }
                            else
                            {
                                newDic[conn][entityName].Attributes[attrName].Id = dbDic[conn][entityName].Attributes[attrName].Id;
                            }
                        }
                    }
                }
            }
            _datasourceInfo = newDic;
        }
        #endregion
    }
}
