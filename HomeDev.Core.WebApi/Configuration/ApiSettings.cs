using System;
using HomeDev.Core.WebApi.Interfaces;

namespace HomeDev.Core.WebApi.Configuration
{
    public class ApiSettings : IApiSettings
    {

        public string Environment { get; set; }

        public string SeqUrl { get; set; }

        public Guid ClientKey { get; set; }

        public bool SwaggerEnabled { get; set; }
    }
}