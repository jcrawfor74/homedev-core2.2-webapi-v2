using HomeDev.Core.WebApi.Models;

namespace HomeDev.Core.WebApi.Interfaces
{
    public interface IAuthenticationService
    {
        bool IsAuthenticated(AuthTokenRequest request, out string token);
    }
}