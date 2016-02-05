using Cloud.Common.Extensions;

namespace Cloud.Common.Core
{
    public static class Commands
    {
        public readonly static byte[] Quit = "QUIT".ToUtf8Bytes();
        public readonly static byte[] Build = "BUILD".ToUtf8Bytes();
        public readonly static byte[] Test = "TEST".ToUtf8Bytes();
    }
}
