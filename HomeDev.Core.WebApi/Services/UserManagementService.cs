using System.Threading.Tasks;
using HomeDev.Core.WebApi.Interfaces;

namespace HomeDev.Core.WebApi.Services
{
    public class UserManagementService : IUserManagementService
    {
        public Task<bool> IsValidUser(string username, string password)
        {
            return Task.FromResult(true);
        }
    }
}