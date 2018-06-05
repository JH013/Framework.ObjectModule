using System.Collections.Generic;

namespace Framework.ObjectModule
{
    public interface IDBOperatorBase
    {
        #region 框架初始化需要调用的方法，外部不可用

        /// <summary>
        /// 判断数据库是否存在
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        bool DatabaseExist(string connectionName = null);
        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="connectionName"></param>
        void CreateDatabase(string connectionName = null);
        /// <summary>
        /// 判断表是否存在
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        bool TableExist(string tableName, string connectionName = null);
        /// <summary>
        /// 创建缺省表EntityBase和EntityAttributeBase
        /// </summary>
        /// <param name="connectionName"></param>
        void CreateDefaultTable(string connectionName = null);
        /// <summary>
        /// 创建表，用于初始化时比对Table.xml创建相应的表
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="connectionName"></param>
        void CreateTable(EntityBase entity, string connectionName = null);
        /// <summary>
        /// 新增字段，用于初始化时比对Table.xml向相应表中新增字段
        /// </summary>
        /// <param name="entityName">表名</param>
        /// <param name="attributeBase">字段信息</param>
        /// <param name="connectionName"></param>
        void CreateField(string entityName, EntityAttributeBase attributeBase, string connectionName = null);
        /// <summary>
        /// 向EntityBase表插入数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        int InsertIntoEntityBase(EntityBase entity, string connectionName = null);
        /// <summary>
        /// 向EntityAttributeBase表出入数据
        /// </summary>
        /// <param name="attributeBase"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        int InsertIntoEntityAttributeBase(EntityAttributeBase attributeBase, string connectionName = null);
        /// <summary>
        /// 从数据库中加载数据生成Entity字典
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        Dictionary<string, EntityBase> QueryEntityDic(string connectionName = null);

        #endregion

        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        int Insert<T>(SqlCondition condition, T data);
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        int Insert<T>(SqlCondition condition, List<T> datas);

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <returns></returns>
        List<T> Query<T>(SqlCondition condition);

        int Delete(SqlCondition condition);

        int Update<T>(SqlCondition condition, T data);
    }
}
