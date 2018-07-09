namespace LMS.LeadEntity.JSON   
{
    using Interface;
    using System;

    public class LeadEntityJSON : ILeadEntity
    {
        public IContext[] Context { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IProperty[] Properties { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ISegment[] Segments { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IResults[] Results { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        void AddObserverableAspects()
        {
            throw new NotImplementedException();
        }

        bool ILeadEntity.isValid()
        {
            throw new NotImplementedException();
        }

        void RemoveObserverableAspects()
        {
            throw new NotImplementedException();
        }
    }
}
