using System.Text;

namespace Cloud.Common.Extensions
{
    public static class StringExtensions
    {
        public static string FromUtf8Bytes(this byte[] bytes)
        {
            return bytes == null ? null
                : Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        public static byte[] ToUtf8Bytes(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public static byte[] ToUtf8Bytes(this int intVal)
        {
            return ToUtf8Bytes(intVal.ToString());
        }

        public static T FromJson<T>(this string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public static string SerializeToString<T>(this T value)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }
    }
}
