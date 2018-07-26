namespace LMS.LeadEntity.Interface
{
    using System;

    // The Contex, Property and Segment Members can be set
    public interface ILeadEntity: ILeadEntityImmutable
    {
        new IContext[] Context { get; set; }

        new IProperty[] Properties { get; set; }

        new ISegment[] Segments { get; set; }

        IResultCollection ResultCollection { get; set; }

    }
}
