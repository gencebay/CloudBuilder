using Cloud.Common.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace Cloud.Common.Contracts
{
    public interface IBaseApi
    {
        IOptions<AppSettingsCommon> AppSettings { get; }
        IServiceProvider Resolver { get; }
        string Echo(string message);
    }
}