using System.Threading.Tasks;

namespace HomeDev.Core.WebApi.Extensions
{
    public static class TaskExtension
    {
        public static T AwaitResult<T>(this Task<T> task)
        {
            return task.GetAwaiter().GetResult();
        }
    }
}