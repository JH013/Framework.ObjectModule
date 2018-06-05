using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.ObjectModule
{
    public class SqlCondition
    {
        public SqlCondition(string tableName, string connectionName = null)
        {
            _tableName = tableName;
            _connectionName = connectionName ?? "Default";
            _filterColumns = new List<string>();
            _filterWay = FilterWay.FilterOut;
        }

        private string _tableName;
        private string _connectionName;
        private List<string> _filterColumns;
        private FilterWay _filterWay;
        private List<string> _orderColumns;
        private OrderBy _orderBy;
        private Expression _expression;

        /// <summary>
        /// 操作的表名
        /// </summary>
        public string TableName
        {
            get
            {
                return _tableName;
            }
        }

        /// <summary>
        /// 连接名
        /// </summary>
        public string ConnectionName
        {
            get
            {
                return _connectionName;
            }
        }

        /// <summary>
        /// 操作的列名，根据FilterWay判断是过滤掉还是留下
        /// </summary>
        public List<string> FilterColumns
        {
            get
            {
                return _filterColumns;
            }
        }

        /// <summary>
        /// 判断Columns属性是过滤掉的还是留下的
        /// </summary>
        public FilterWay FilterWay
        {
            get
            {
                return _filterWay;
            }
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        public List<string> OrderColumns
        {
            get
            {
                return _orderColumns;
            }
        }

        public OrderBy OrderBy
        {
            get
            {
                return _orderBy;
            }
        }

        /// <summary>
        /// 约束表达式
        /// </summary>
        public Expression Expression
        {
            get
            {
                return _expression;
            }
        }

        /// <summary>
        /// 设置过滤器,对查询结果的过滤
        /// </summary>
        /// <param name="columnNames"></param>
        /// <param name="filterWay"></param>
        public SqlCondition SetFilter(FilterWay filterWay, params string[] columnNames)
        {
            if (columnNames != null && columnNames.Length > 0)
            {
                _filterColumns = columnNames.ToList();
                _filterWay = filterWay;
            }
            return this;
        }

        /// <summary>
        /// 设置排序规则
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="orderColumns"></param>
        public SqlCondition SetOrderBy(OrderBy orderBy, params string[] orderColumns)
        {
            if (orderColumns != null && orderColumns.Length > 0)
            {
                _orderColumns = orderColumns.ToList();
                _orderBy = orderBy;
            }
            return this;
        }

        /// <summary>
        /// 设置约束
        /// </summary>
        /// <param name="expression"></param>
        public SqlCondition SetContraints(Expression expression)
        {
            _expression = expression;
            return this;
        }
    }

    public enum FilterWay
    {
        FilterOut = 0,
        Keep = 1
    }

    public enum OrderBy
    {
        ASC = 0,
        DESC = 1
    }
}
