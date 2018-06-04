namespace LeadEntity
{
    using Interface;
    using System;

    public class LeadEntity : ILeadEntity
    {

        public class Rootobject
        {
            public Lead Lead { get; set; }
        }

        public class Lead
        {
            public Context[] Context { get; set; }
            public Property[] Properties { get; set; }
            public Segment[] Segments { get; set; }
            public Activity[] Activity { get; set; }
        }

        public class Context
        {
            public string Id { get; set; }
            public object Value { get; set; }
        }

        public class Property
        {
            public string Id { get; set; }
            public object Value { get; set; }
        }

        public class Segment
        {
            public string Type { get; set; }
        }

        public class Activity
        {
            public string Type { get; set; }
            public DateTime Timestamp { get; set; }
        }

        void ILeadEntity.AddObserverableAspects()
        {
            throw new NotImplementedException();
        }

        bool ILeadEntity.isValid()
        {
            throw new NotImplementedException();
        }

        void ILeadEntity.RemoveObserverableAspects()
        {
            throw new NotImplementedException();
        }
    }
}
