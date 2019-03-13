using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FetchCustomerActivity.Implementation.Context
{
    public interface ICustomerActvityContext
    {

        IQueryable<CustomerActivityContext.BrandBuyClick> BrandBuyClickList { get; }
    }
}
