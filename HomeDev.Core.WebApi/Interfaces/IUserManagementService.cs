using System.Threading.Tasks;

namespace HomeDev.Core.WebApi.Interfaces
{
    public interface IUserManagementService
    {
         Task<bool> IsValidUser(string username, string password);
    }
}