namespace LeadEntity.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Defines the LeadEntity Context
    /// Examples include: "ActivityID, "IdentityID", "SessionID", "QuotedProduct", "OtherProducts", "SessionSequence"
    /// </summary>
    public interface IContext
    {
        string Id { get; }
        object Value { get; }

    }
}
