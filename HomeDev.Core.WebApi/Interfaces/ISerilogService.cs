using Serilog;

namespace HomeDev.Core.WebApi.Interfaces
{
    public interface ISerilogService
    {
         void Initialise(IApiSettings apiSettings, LoggerConfiguration loggerConfiguration);
    }
}