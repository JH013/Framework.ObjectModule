using System.IO;
using System.Xml.Serialization;

namespace Framework.ObjectModule
{
    public class SerializeUtil
    {

        public static T Deserialize<T>(string xmlPath)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(xmlPath, FileMode.Open, FileAccess.Read))
            {
                var result = (T)xs.Deserialize(fs);
                return result;
            }
        }
    }
}
