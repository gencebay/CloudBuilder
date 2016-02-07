###Cross-Platform Cloud Optimized .NET Build Automation

####Prerequisites
> [DNVM](https://github.com/aspnet/Home), DNX (.NET Core CLR)
> [Docs](https://docs.asp.net/en/latest/)

####Installation

    dnvm install -r coreclr -a x64 latest
    git clone https://github.com/gencebay/CloudBuilder.git
    cd CloudBuilder
    dnu restore

####Execution
**Server and Socket Bootstrap**

    in CloudBuilder/src/Cloud.Server
    dnx web

**Console Client App Bootstrap**

    in CloudBuilder/src/Cloud.Client
    dnx run

**Dashboard App Bootstrap**

    in CloudBuilder/src/Cloud.Server.Web.Hosting
    dnx web

####Documentation
  