﻿namespace LMS.LeadEntity.Interface
{
    using System;
    using System.Collections.Generic;

    // The Contex, Property and Segment Members can be set
    public interface ILeadEntity: ILeadEntityImmutable
    {
        new IContext[] Context { get; set; }

        new IProperty[] Properties { get; set; }

        new ISegment[] Segments { get; set; }

        IResultCollection ResultCollection { get; set; }

        List<string> ErrorList { get; set; }

    }
}
