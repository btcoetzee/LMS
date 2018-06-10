namespace LeadEntity.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    /// <summary>
    /// Defines other properties for the Lead
    /// Examples include "PriorBI", "VehicleCount", "QuotedBI", "DisplayedBrands"
    /// </summary>
    public interface IProperty
    {
        string Id { get; }
        object Value { get; }

    }
}
