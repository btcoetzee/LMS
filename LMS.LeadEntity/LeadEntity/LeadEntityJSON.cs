namespace LMS.LeadEntity.JSON   
{
    using Interface;
    using System;

    public class LeadEntityJSON : ILeadEntity
    {
        public IContext[] Context { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IProperty[] Properties { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ISegment[] Segments { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IResultCollection ResultCollection { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

  }
}
