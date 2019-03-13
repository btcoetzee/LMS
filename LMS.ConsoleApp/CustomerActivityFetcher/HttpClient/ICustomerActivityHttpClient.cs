using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CustomerActivityFetcher.HttpClient
{
    public interface ICustomerActivityHttpClient
    {
     
        Task<string> GetCustomerActivity(Guid customerActivityGuid);
    }
}
