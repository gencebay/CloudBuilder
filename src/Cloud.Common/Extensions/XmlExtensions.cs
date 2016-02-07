using Newtonsoft.Json;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Cloud.Common.Extensions
{
    public static class XmlExtensions
    {
        public static string CreateXml<T>(this T model)
        {
            if (model == null)
                throw new ArgumentNullException("Model required!");

            XmlSerializer xmlSerializer = new XmlSerializer(model.GetType());
            using (StringWriter sw = new StringWriter())
            using (XmlWriter writer = XmlWriter.Create(sw))
            {
                xmlSerializer.Serialize(writer, model);
                return sw.ToString();
            }
        }

        public static T FromXml<T>(this string serializedObject)
        {
            if (string.IsNullOrEmpty(serializedObject))
                throw new ArgumentNullException("Serialized object string required");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (TextReader tr = new StringReader(serializedObject))
            using (XmlReader reader = XmlReader.Create(tr))
            {
                var obj = xmlSerializer.Deserialize(reader);
                if (obj == null)
                    throw new InvalidCastException("Could not create an instance of specified object");

                return (T)obj;
            }
        }
    }
}
