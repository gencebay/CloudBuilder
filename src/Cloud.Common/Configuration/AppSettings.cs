namespace Cloud.Common.Configuration
{
    public class AppSettingsCommon
    {
        public string Name { get; set; }
    }

    public class ServerSettings : AppSettingsCommon
    {
        public int DispatchInterval { get; set; }
    }

    public class ClientSettings : AppSettingsCommon
    {
        public string ApiBaseHostAddress { get; set; }
    }
}
