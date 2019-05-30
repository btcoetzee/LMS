using System.Linq;

namespace LMS.ConsoleApplication.Context
{
    public interface ICustomerActvityContext
    {

        IQueryable<CustomerActivityContext.BrandBuyClick> BrandBuyClickList { get; }
    }
}
