using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Framework.ObjectModule
{
    public class SqlServerDBOperator : IDBOperatorBase
    {
        #region 框架初始化需要调用的方法，外部不可用
        public bool DatabaseExist(string connectionName = null)
        {
            string databaseName = ConnectionProviderBase.GetDatabaseName(connectionName);
            string sqlTemplate = "SELECT * FROM master.dbo.sysdatabases WHERE name='{0}'";
            string sqlText = string.Format(sqlTemplate, databaseName);
            string connStr = SqlServerConnectionProvider.Instance.GetConnectionStringWithoutIC(connectionName);
            object result = SqlServerBaseOrder.ExecuteScalar(connStr, sqlText);
            if (result != null && string.Equals(result as string, databaseName))
            {
                return true;
            }
            return false;
        }

        public void CreateDatabase(string connectionName = null)
        {
            string databaseName = ConnectionProviderBase.GetDatabaseName(connectionName);
            string sqlTemplate = "CREATE DATABASE {0}";
            string sqlText = string.Format(sqlTemplate, databaseName);
            string connStr = SqlServerConnectionProvider.Instance.GetConnectionStringWithoutIC(connectionName);
            SqlServerBaseOrder.ExecuteNonQuery(connStr, sqlText);
        }

        public bool TableExist(string tableName, string connectionName = null)
        {
            string sqlTemplate = "SELECT COUNT(*) FROM dbo.sysobjects WHERE name = '{0}'";
            string sqlText = string.Format(sqlTemplate, tableName);
            string connStr = SqlServerConnectionProvider.Instance.GetConnectionString(connectionName);
            object result = SqlServerBaseOrder.ExecuteScalar(connStr, sqlText);
            return (int)result > 0 ? true : false;
        }

        public void CreateDefaultTable(string connectionName = null)
        {
            string sqlText1 = @"CREATE TABLE EntityBase(Id nvarchar(256) primary key, EntityName nvarchar(256))";
            string sqlText2 = @"CREATE TABLE EntityAttributeBase  
(  
  Id nvarchar(256) primary key,  
  EntityId  nvarchar(256),  
  AttributeName nvarchar(256),  
  AttributeType nvarchar(256),  
  AttributeSize nvarchar(256),
  IsPrimaryKey bit,
  IsNotNull bit
)";
            string connStr = SqlServerConnectionProvider.Instance.GetConnectionString(connectionName);
            SqlServerBaseOrder.ExecuteNonQuery(connStr, sqlText1);
            SqlServerBaseOrder.ExecuteNonQuery(connStr, sqlText2);
        }

        public void CreateTable(EntityBase entity, string connectionName = null)
        {
            string sqlTemplate = @"CREATE TABLE {0} ({1})";
            List<string> attrList = new List<string>();
            foreach (var attr in entity.Attributes.Keys)
            {
                var temp = entity.Attributes[attr].AttributeName + " " + entity.Attributes[attr].AttributeType;
                if (!string.IsNullOrEmpty(entity.Attributes[attr].AttributeSize) && int.Parse(entity.Attributes[attr].AttributeSize) > 0)
                {
                    temp += string.Format("({0})", entity.Attributes[attr].AttributeSize);
                }
                if (entity.Attributes[attr].IsPrimaryKey)
                {
                    temp += " PRIMARY KEY";
                }
                if (entity.Attributes[attr].IsNotNull && !entity.Attributes[attr].IsPrimaryKey)
                {
                    temp += " NOT NULL";
                }
                attrList.Add(temp);
            }
            string sqlText = string.Format(sqlTemplate, entity.EntityName, string.Join(",", attrList.ToArray()));
            SqlServerBaseOrder.ExecuteNonQuery(SqlServerConnectionProvider.Instance.GetConnectionString(connectionName), sqlText);
        }

        public void CreateField(string entityName, EntityAttributeBase attributeBase, string connectionName = null)
        {
            string sqlText = "ALTER TABLE {0} ADD {1} {2}";
            string temp = string.Format(sqlText, entityName, attributeBase.AttributeName, attributeBase.AttributeType);
            if (!string.IsNullOrEmpty(attributeBase.AttributeSize) && int.Parse(attributeBase.AttributeSize) > 0)
            {
                temp += string.Format("({0})", attributeBase.AttributeSize);
            }
            if (attributeBase.IsPrimaryKey)
            {
                temp += " PRIMARY KEY";
            }
            if (attributeBase.IsNotNull && !attributeBase.IsPrimaryKey)
            {
                temp += " NOT NULL";
            }
            SqlServerBaseOrder.ExecuteNonQuery(SqlServerConnectionProvider.Instance.GetConnectionString(connectionName), temp);
        }

        public int InsertIntoEntityBase(EntityBase entity, string connectionName = null)
        {
            string sqlText = "INSERT INTO EntityBase(Id, EntityName)VALUES(@Id, @EntityName)";
            SqlParameter[] paras = new SqlParameter[]{
                    new SqlParameter("@Id", SqlDbType.NVarChar,256),
                    new SqlParameter("@EntityName", SqlDbType.NVarChar,256)
               };
            paras[0].Value = entity.Id;
            paras[1].Value = entity.EntityName;
            string connStr = SqlServerConnectionProvider.Instance.GetConnectionString(connectionName);
            return SqlServerBaseOrder.ExecuteNonQuery(connStr, sqlText, paras);
        }

        public int InsertIntoEntityAttributeBase(EntityAttributeBase attributeBase, string connectionName = null)
        {
            string sqlText = "INSERT INTO EntityAttributeBase(Id, EntityId, AttributeName, AttributeType, AttributeSize, IsPrimaryKey, IsNotNull)VALUES(@Id, @EntityId, @AttributeName, @AttributeType, @AttributeSize, @IsPrimaryKey, @IsNotNull)";
            SqlParameter[] paras = new SqlParameter[]{
                    new SqlParameter("@Id", SqlDbType.NVarChar,256),
                    new SqlParameter("@EntityId", SqlDbType.NVarChar,256),
                    new SqlParameter("@AttributeName", SqlDbType.NVarChar,256),
                    new SqlParameter("@AttributeType", SqlDbType.NVarChar,256),
                    new SqlParameter("@AttributeSize", SqlDbType.NVarChar,256),
                    new SqlParameter("@IsPrimaryKey", SqlDbType.Bit),
                    new SqlParameter("@IsNotNull", SqlDbType.Bit)
               };
            paras[0].Value = attributeBase.Id;
            paras[1].Value = attributeBase.EntityId;
            paras[2].Value = attributeBase.AttributeName;
            paras[3].Value = attributeBase.AttributeType;
            paras[4].Value = string.IsNullOrEmpty(attributeBase.AttributeSize) ? (object)DBNull.Value : attributeBase.AttributeSize;
            paras[5].Value = attributeBase.IsPrimaryKey;
            paras[6].Value = attributeBase.IsNotNull;
            string connStr = SqlServerConnectionProvider.Instance.GetConnectionString(connectionName);
            return SqlServerBaseOrder.ExecuteNonQuery(connStr, sqlText, paras);
        }

        public Dictionary<string, EntityBase> QueryEntityDic(string connectionName = null)
        {
            Dictionary<string, EntityBase> dic = new Dictionary<string, EntityBase>();

            List<EntityBase> entities = SqlServerBaseOrder.ExecuteReader<EntityBase>(SqlServerConnectionProvider.Instance.GetConnectionString(connectionName), "SELECT * FROM EntityBase");
            List<EntityAttributeBase> attributes = SqlServerBaseOrder.ExecuteReader<EntityAttributeBase>(SqlServerConnectionProvider.Instance.GetConnectionString(connectionName), "SELECT * FROM EntityAttributeBase");

            foreach (EntityBase entity in entities)
            {
                if (!dic.ContainsKey(entity.EntityName))
                {
                    foreach (EntityAttributeBase attr in attributes)
                    {
                        if (string.Equals(entity.Id, attr.EntityId))
                        {
                            entity.Attributes.Add(attr.AttributeName, attr);
                        }
                    }
                    dic.Add(entity.EntityName, entity);
                }
            }

            return dic;
        }
        #endregion

        public int Insert<T>(SqlCondition condition, T data)
        {
            EntityBase entity = EntityManager.GetEntity(condition.ConnectionName, condition.TableName);
            if (entity == null)
            {
                throw new Exception("Invalid connection or table.");
            }
            List<string> attrList = new List<string>();
            List<string> paramList = new List<string>();
            foreach (var attr in entity.Attributes.Keys)
            {
                attrList.Add(attr);
                paramList.Add(string.Format("@{0}", attr));
            }
            string sqlTemplate = "INSERT INTO {0}({1})VALUES({2})";
            string sqlText = string.Format(sqlTemplate, condition.TableName, string.Join(",", attrList), string.Join(",", paramList));
            List<SqlParameter> paras = new List<SqlParameter> { };

            foreach (var attr in entity.Attributes.Keys)
            {
                paras.Add(this.Convert<T>(entity.Attributes[attr], data));
            }
            string connStr = SqlServerConnectionProvider.Instance.GetConnectionString(condition.ConnectionName);
            return SqlServerBaseOrder.ExecuteNonQuery(connStr, sqlText, paras.ToArray());
        }

        public int Insert<T>(SqlCondition condition, List<T> datas)
        {
            DataTable dt = new DataTable();
            EntityBase entity = EntityManager.GetEntity(condition.ConnectionName, condition.TableName);
            if (entity == null)
            {
                throw new Exception("Invalid connection or table.");
            }
            Hashtable mappings = new Hashtable();
            foreach (var attr in entity.Attributes.Keys)
            {
                dt.Columns.Add(attr, typeof(T).GetProperty(attr).PropertyType);
                mappings.Add(attr, attr);
            }
            foreach (var data in datas)
            {
                DataRow dr = dt.NewRow();
                foreach (var attr in entity.Attributes.Keys)
                {
                    dr[attr] = data.GetType().GetProperty(attr).GetValue(data);
                }
                dt.Rows.Add(dr);
            }
            string connStr = SqlServerConnectionProvider.Instance.GetConnectionString(condition.ConnectionName);
            return SqlServerBaseOrder.ExecuteBulkCopy(connStr, condition.TableName, dt, mappings);
        }

        public List<T> Query<T>(SqlCondition condition)
        {
            EntityBase entity = EntityManager.GetEntity(condition.ConnectionName, condition.TableName);
            if (entity == null)
            {
                throw new Exception("Invalid connection or table.");
            }
            List<string> attrList = new List<string>();
            foreach (var attr in entity.Attributes.Keys)
            {
                if ((condition.FilterWay == FilterWay.FilterOut && !condition.FilterColumns.Contains(attr)) ||
                    (condition.FilterWay == FilterWay.Keep && condition.FilterColumns.Count == 0) ||
                    (condition.FilterWay == FilterWay.Keep && condition.FilterColumns.Contains(attr)))
                {
                    attrList.Add(attr);
                }
            }
            string sqlTemplate = "SELECT {0} FROM {1}";
            string sqlText = string.Format(sqlTemplate, string.Join(",", attrList), condition.TableName);
            if (condition.Expression != null)
            {
                sqlText += condition.Expression.GenerateSqlText();
            }
            if (condition.OrderColumns != null && condition.OrderColumns.Count > 0)
            {
                sqlText = sqlText + string.Format(" ORDER BY {0} ", string.Join(",", condition.OrderColumns)) + condition.OrderBy.ToString();
            }
            string connStr = SqlServerConnectionProvider.Instance.GetConnectionString(condition.ConnectionName);
            return SqlServerBaseOrder.ExecuteReader<T>(connStr, sqlText);
        }

        private SqlParameter Convert<T>(EntityAttributeBase attributeBase, T data)
        {
            var propValue = data.GetType().GetProperty(attributeBase.AttributeName).GetValue(data);
            SqlParameter parameter = new SqlParameter(attributeBase.AttributeName, propValue ?? DBNull.Value);
            switch (attributeBase.AttributeType)
            {
                case "nvarchar":
                    parameter.SqlDbType = SqlDbType.NVarChar;
                    break;
                case "varchar":
                    parameter.SqlDbType = SqlDbType.VarChar;
                    break;
                case "int":
                    parameter.SqlDbType = SqlDbType.Int;
                    break;
                case "bigint":
                    parameter.SqlDbType = SqlDbType.BigInt;
                    break;
                case "bit":
                    parameter.SqlDbType = SqlDbType.Bit;
                    break;
                case "float":
                    parameter.SqlDbType = SqlDbType.Float;
                    break;
                case "double":
                    parameter.SqlDbType = SqlDbType.Decimal;
                    break;
                case "ntext":
                    parameter.SqlDbType = SqlDbType.NText;
                    break;
            }
            if (!string.IsNullOrEmpty(attributeBase.AttributeSize) && int.Parse(attributeBase.AttributeSize) > 0)
            {
                parameter.Size = int.Parse(attributeBase.AttributeSize);
            }
            return parameter;
        }

        public int Delete(SqlCondition condition)
        {
            string sqlTemplate = "DELETE FROM {0}";
            string sqlText = string.Format(sqlTemplate, condition.TableName);
            if (condition.Expression != null)
            {
                sqlText += condition.Expression.GenerateSqlText();
            }
            string connStr = SqlServerConnectionProvider.Instance.GetConnectionString(condition.ConnectionName);
            return SqlServerBaseOrder.ExecuteNonQuery(connStr, sqlText);
        }

        public int Update<T>(SqlCondition condition, T data)
        {
            EntityBase entity = EntityManager.GetEntity(condition.ConnectionName, condition.TableName);
            if (entity == null)
            {
                throw new Exception("Invalid connection or table.");
            }
            List<string> attrList = new List<string>();
            List<string> expressionList = new List<string>();
            foreach (var attr in entity.Attributes.Keys)
            {
                if ((condition.FilterWay == FilterWay.FilterOut && !condition.FilterColumns.Contains(attr)) ||
                    (condition.FilterWay == FilterWay.Keep && condition.FilterColumns.Count == 0) ||
                    (condition.FilterWay == FilterWay.Keep && condition.FilterColumns.Contains(attr)))
                {
                    attrList.Add(attr);
                    expressionList.Add(string.Format("{0} = @{0}", attr));
                }
            }
            string sqlTemplate = "UPDATE {0} SET {1}";
            string sqlText = string.Format(sqlTemplate, condition.TableName, string.Join(",", expressionList));
            if (condition.Expression != null)
            {
                sqlText += condition.Expression.GenerateSqlText();
            }
            List<SqlParameter> paras = new List<SqlParameter> { };
            foreach (var attr in attrList)
            {
                paras.Add(this.Convert<T>(entity.Attributes[attr], data));
            }
            string connStr = SqlServerConnectionProvider.Instance.GetConnectionString(condition.ConnectionName);
            return SqlServerBaseOrder.ExecuteNonQuery(connStr, sqlText, paras.ToArray());
        }
    }
}
