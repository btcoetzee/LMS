using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LMS.ConsoleApplication.Context
{
    public class CustomerActivityContext : DbContext, ICustomerActvityContext
    {

        public CustomerActivityContext()
        {
        }

        public CustomerActivityContext(DbContextOptions<CustomerActivityContext> options)
       : base(options)
        {
        }
        /// <summary>
        /// Connection string for connecting Database
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Server=USECVUT-SQL01.inspopusa.lan;Database=Product;Trusted_Connection=True;");
            }
        }
        /// <summary>
        /// Return the ApprovedLeads
        /// </summary>
        private DbSet<BrandBuyClick> BrandBuyClickSet { get; set; }
        public IQueryable<BrandBuyClick> BrandBuyClickList => BrandBuyClickSet;

        /// <summary>
        /// ApprovedLeads class contains all coloums of table
        /// </summary>
        [Table("BrandBuyClick", Schema = "Motor")]
        public class BrandBuyClick
        {
            public int Brand_ID { get; set; }
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public Guid CustomerActivity_ID { get; set; }
            public DateTime CreationDate { get; set; }
            public int BrandBuyClickID { get; set; }
            public string BuyTypeXML { get; set; }
        }
    }

}
