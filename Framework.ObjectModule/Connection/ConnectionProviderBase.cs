using System.Collections.Generic;
using System.Linq;

namespace Framework.ObjectModule
{
    public abstract class ConnectionProviderBase
    {
        public ConnectionProviderBase() { }

        protected static Dictionary<string, ConnectionInfo> _connDic;

        public static void Init(string dbxml)
        {
            TargetDbs targetDbs = SerializeUtil.Deserialize<TargetDbs>(dbxml);
            if (targetDbs != null)
            {
                _connDic = targetDbs.Infos.ToDictionary(item => item.ConnectionName, item => item);
            }
        }

        public static void DeleteConnections(List<string> connectionNames)
        {
            connectionNames.ForEach(c => _connDic.Remove(c));
        }

        public static Dictionary<string, ConnectionInfo> GetAllConnectionInfos()
        {
            return _connDic;
        }

        public static string GetDatabaseName(string connName = null)
        {
            string databaseName = string.Empty;
            if (_connDic != null && _connDic.Keys.Count > 0)
            {
                ConnectionInfo target;
                if (string.IsNullOrEmpty(connName))
                {
                    target = _connDic["Default"];
                }
                else
                {
                    target = _connDic[connName];
                }
                databaseName = target.InitialCatalog;
            }
            return databaseName;
        }

        public abstract string GetConnectionString(string connName = null);

        public abstract string GetConnectionStringWithoutIC(string connName = null);
    }
}
