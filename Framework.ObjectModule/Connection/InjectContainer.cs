using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using System.Collections.Generic;

namespace Framework.ObjectModule
{
    public class InjectContainer
    {
        private static IWindsorContainer _container;
        public static void LoadConnection()
        {
            _container = new WindsorContainer(new XmlInterpreter("Connection\\component.xml"));
        }

        public static T Resolve<T>(string dbType)
        {
            return _container.Resolve<T>(dbType);
        }

        public static T ResolveByName<T>(string connectionName)
        {
            Dictionary<string, ConnectionInfo> connDic = ConnectionProviderBase.GetAllConnectionInfos();
            return _container.Resolve<T>(connDic[connectionName].DBType.ToString());
        }
    }
}
