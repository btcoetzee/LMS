using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FetchCustomerActivity.Implementation.Context;

namespace FetchCustomerActivity.Implementation
{
    public class FetchCustomerActivityFromDb
    {
        public FetchCustomerActivityFromDb()
        {

        }
        public IList<Guid> getCustomerActivity(int count)
        {

            using (var context = new CustomerActivityContext())
            {
                var guidList =  (IList<Guid>) (from ca in context.BrandBuyClickList
                    orderby ca.CreationDate descending
                    select ca.CustomerActivity_ID).Take(count).ToList();

                return guidList;

        
            }
        }
    }
}
