using System;
using System.Collections.Generic;
using System.Linq;
using LMS.ConsoleApplication.Context;

namespace LMS.ConsoleApplication
{
    public class FetchCustomerActivityFromDb
    {
        public FetchCustomerActivityFromDb()
        {

        }
        public IList<Guid> GetCustomerActivity(int count)
        {

            using (var context = new CustomerActivityContext())
            {
                var guidList1 = (IList<Guid>) (context.BrandBuyClickList.Where(p => p.CustomerActivity_ID != Guid.Empty)
                    .OrderByDescending(c => c.CreationDate).Select(d => d.CustomerActivity_ID).Distinct().Take(count)
                    .ToList());
                //var guidList =  (IList<Guid>) (from ca in context.BrandBuyClickList
                //    orderby ca.CreationDate descending
                //    select  ca.CustomerActivity_ID).Distinct().Take(count).ToList()
                //where ca.CustomerActivity_ID<> Guid.Empty;

                return guidList1;

        
            }
        }
    }
}
