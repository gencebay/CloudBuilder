using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Cloud.Common.Extensions
{
    public static class JsonExtensions
    {
        public static string CreateJson<T>(this T model)
        {
            if (model == null)
                throw new ArgumentNullException("Model required!");

            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.SerializeObject(model, settings);
        }

        public static string CreateJson(this object model)
        {
            if (model == null)
                throw new ArgumentNullException("Model required!");

            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.SerializeObject(model, settings);
        }
    }
}
