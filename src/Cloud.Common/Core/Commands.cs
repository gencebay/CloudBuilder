using Cloud.Common.Extensions;

namespace Cloud.Common.Core
{
    public enum CommandType
    {
        Unset = 0,
        Quit = 1,
        Build = 2,
        Test = 3
    }

    public static class CommandsAsBytes
    {
        public readonly static byte[] Quit = "QUIT".ToUtf8Bytes();
        public readonly static byte[] Build = "BUILD".ToUtf8Bytes();
        public readonly static byte[] Test = "TEST".ToUtf8Bytes();
    }
}
