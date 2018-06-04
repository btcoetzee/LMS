namespace LeadEntity.Interface
{
    using System;

    public interface ILeadEntity
    {
        // Is Lead Entity Valid
        bool isValid();

        // Add elements for observers to see
        void AddObserverableAspects();

        // Remove elements not required for observers
        void RemoveObserverableAspects();

    }
}
