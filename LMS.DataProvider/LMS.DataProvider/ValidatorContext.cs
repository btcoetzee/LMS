using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;

namespace LMS.DataProvider
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.EntityFrameworkCore;
    public partial class ValidatorContext : DbContext
    {

        public DbSet<Validator> Validators { get; set; }

        public ValidatorContext()
        {
        }

        public ValidatorContext(DbContextOptions<ValidatorContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
               optionsBuilder.UseSqlServer(@"Server=USECVUT-SQL01;Database=PartnerData;Trusted_Connection=True;");
        }

    }
    [Table("Validator", Schema="dbo")]
    public class Validator
    {
        public int ValidatorID { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public string MethodName { get; set; }
        public string ClassName { get; set; }
        public string Parameters { get; set; }
    }
}
