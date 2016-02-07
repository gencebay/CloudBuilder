using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Cloud.Common.Extensions
{
    public static class JsonExtensions
    {
        public static string CreateJson<T>(this T model)
        {
            if (model == null)
                throw new ArgumentNullException("Model required!");

            return JsonConvert.SerializeObject(model);
        }
    }
}
