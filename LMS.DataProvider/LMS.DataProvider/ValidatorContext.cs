namespace LMS.DataProvider
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Common;
    public class ValidatorContext : DbContext
    {
        public DbSet<Validator> Validators { get; set; }
        public DbSet<LeadCollectorValidator> LeadCollectorValidators { get; set; }
        public DbSet<CampaignManagerValidator> CampaignManagerValidators { get; set; }
        public DbSet<CampaignValidator> CampaignValidators { get; set; }

        public ValidatorContext()
        {
        }

        public ValidatorContext(DbContextOptions<ValidatorContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=USECVUT-SQL01;Database=PartnerData;Trusted_Connection=True;");
            }
        }
    }
    [Table("Validator", Schema="dbo")]
    public class Validator
    {
        public int ValidatorId { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public string MethodName { get; set; }
        public string ClassName { get; set; }
    }

    [Table("LeadCollectorValidator", Schema = "dbo")]
    public class LeadCollectorValidator
    {
        public int LeadCollectorValidatorId { get; set; }
        public int ValidatorId { get; set; }
    }

    [Table("CampaignManagerValidator", Schema = "dbo")]
    public class CampaignManagerValidator
    {
        public int CampaignManagerValidatorId { get; set; }
        public int CampaignManagerId { get; set; }
        public int ValidatorId { get; set; }
    }

    [Table("CampaignValidator", Schema = "dbo")]
    public class CampaignValidator
    {
        public int CampaignValidatorId { get; set; }
        public int CampaignId { get; set; }
        public int ValidatorId { get; set; }
    }
}
