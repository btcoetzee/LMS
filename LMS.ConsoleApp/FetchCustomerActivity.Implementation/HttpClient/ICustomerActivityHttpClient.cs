using System;
using System.Threading.Tasks;

namespace FetchCustomerActivity.Implementation.HttpClient
{
    public interface ICustomerActivityHttpClient
    {
     
        Task<string> GetCustomerActivity(Guid customerActivityGuid);
    }
}
