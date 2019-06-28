using System.Collections.Generic;
using Serilog;

namespace HomeDev.Core.WebApi.Interfaces
{
    public interface ISerilogConfigurationService
    {
         LoggerConfiguration Initialise(IEnumerable<ISerilogService> services);
    }
}