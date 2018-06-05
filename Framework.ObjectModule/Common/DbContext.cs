using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.ObjectModule
{
    public class DbContext : IDisposable
    {
        public DbContext(SqlCondition condition)
        {
            _condition = condition;
        }

        private SqlCondition _condition;

        public int Insert<T>(T data)
        {
            IDBOperatorBase service = InjectContainer.ResolveByName<IDBOperatorBase>(_condition.ConnectionName);
            return service.Insert<T>(_condition, data);
        }

        public int Insert<T>(List<T> datas)
        {
            IDBOperatorBase service = InjectContainer.ResolveByName<IDBOperatorBase>(_condition.ConnectionName);
            return service.Insert<T>(_condition, datas);
        }

        public List<T> Query<T>()
        {
            IDBOperatorBase service = InjectContainer.ResolveByName<IDBOperatorBase>(_condition.ConnectionName);
            return service.Query<T>(_condition);
        }

        public T SingleQuery<T>()
        {
            IDBOperatorBase service = InjectContainer.ResolveByName<IDBOperatorBase>(_condition.ConnectionName);
            return service.Query<T>(_condition).FirstOrDefault();
        }

        public int Delete()
        {
            IDBOperatorBase service = InjectContainer.ResolveByName<IDBOperatorBase>(_condition.ConnectionName);
            return service.Delete(_condition);
        }

        public int Update<T>(T data)
        {
            IDBOperatorBase service = InjectContainer.ResolveByName<IDBOperatorBase>(_condition.ConnectionName);
            return service.Update(_condition, data);
        }

        public void Dispose()
        {
        }
    }
}
