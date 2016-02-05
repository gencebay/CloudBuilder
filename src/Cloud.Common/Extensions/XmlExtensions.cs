using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Cloud.Common.Extensions
{
    public static class XmlExtensions
    {
        public static string CreateXmlMessage<T>(this T model)
        {
            XmlSerializer xsSubmit = new XmlSerializer(model.GetType());
            using (StringWriter sww = new StringWriter())
            using (XmlWriter writer = XmlWriter.Create(sww))
            {
                xsSubmit.Serialize(writer, model);
                return sww.ToString();
            }
        }
    }
}
