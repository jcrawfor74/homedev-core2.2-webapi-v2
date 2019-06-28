using System;

namespace HomeDev.Core.WebApi.Interfaces
{
    public interface IApiSettings
    {
        string Environment { get; }
        string SeqUrl { get; }
        Guid ClientKey { get; }
        bool SwaggerEnabled { get; }
    }
}