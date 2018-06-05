namespace Framework.ObjectModule
{
    public class SqlServerConnectionProvider : ConnectionProviderBase
    {
        private static SqlServerConnectionProvider _instance = null;
        private static readonly object _lock = new object();

        SqlServerConnectionProvider()
        {
        }

        public static SqlServerConnectionProvider Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ?? (_instance = new SqlServerConnectionProvider());
                }
            }
        }

        const string TEMPLATE = "Data Source={0}; Initial Catalog={1}; User Id={2}; Password={3};";
        const string TEMPLATE2 = "Data Source={0}; User Id={1}; Password={2};";


        public override string GetConnectionString(string connName = null)
        {
            string connStr = string.Empty;
            if (_connDic != null && _connDic.Keys.Count > 0)
            {
                ConnectionInfo target = Resolve(connName);
                connStr = string.Format(TEMPLATE, target.DataSource, target.InitialCatalog, target.UserId, target.Password);
            }
            return connStr;
        }

        public override string GetConnectionStringWithoutIC(string connName = null)
        {
            string connStr = string.Empty;
            if (_connDic != null && _connDic.Keys.Count > 0)
            {
                ConnectionInfo target = Resolve(connName);
                connStr = string.Format(TEMPLATE2, target.DataSource, target.UserId, target.Password);
            }
            return connStr;
        }

        private ConnectionInfo Resolve(string connName = null)
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
            return target;
        }
    }
}
