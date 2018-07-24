namespace LMS.LeadEntity.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// The members of the classes that inherit from this interface should not 
    /// be set/updated by the functions manipulating the object.
    /// The component that have created this lead entity would have assigned
    /// all the values for the members below - the LMS should not be updating 
    /// these members.
    /// </summary>
    public interface ILeadEntityImmutable
    {
        IContext[] Context { get;  }

        IProperty[] Properties { get; }

        ISegment[] Segments { get; }
    }
}
