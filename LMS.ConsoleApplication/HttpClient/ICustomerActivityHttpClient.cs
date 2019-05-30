using System;
using System.Threading.Tasks;

namespace LMS.ConsoleApplication.HttpClient
{
    public interface ICustomerActivityHttpClient
    {
     
        Task<string> GetCustomerActivity(Guid customerActivityGuid);
    }
}
