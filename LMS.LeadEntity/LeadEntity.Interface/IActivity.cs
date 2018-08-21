namespace LMS.LeadEntity.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Defines the LeadEntity Activity
    /// Examples include: "QuoteRef, "AnnualPremium", "DownPayment", "MonthlyInstallment", "Requote", "BuyOnlineClick", "BuyOnlineBrand", "BuyByPhoneClick", "BuyByPhoneBrand"
    /// </summary>
    public interface IActivity
    {
        string Id { get; }
        object Value { get; }

    }
}
