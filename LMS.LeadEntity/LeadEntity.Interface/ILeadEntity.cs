namespace LMS.LeadEntity.Interface
{
    using System;

    public interface ILeadEntity
    {
        IContext[] Context { get; set; }

        IProperty[] Properties { get; set; }

        ISegment[] Segments { get; set; }
        IResults[] Results { get; set; }

        // Is Lead Entity Valid
        bool isValid();

        //// Add elements for observers to see
        //void AddObserverableAspects();

        //// Remove elements not required for observers
        //void RemoveObserverableAspects();

    }
}
