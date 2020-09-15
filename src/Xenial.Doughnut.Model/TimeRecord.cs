using System;

using DevExpress.Xpo;

namespace Xenial.Doughnut.Model
{
    [Persistent("TimeRecord")]
    public class TimeRecord : DoughnutBaseObject
    {
        public TimeRecord(Session session) : base(session) { }

        [Association, Aggregated]
        public XPCollection<TimeRecordEntry> TimeRecordEntries => GetCollection<TimeRecordEntry>();

        private Customer? customer;
        [Persistent("Customer")]
        public Customer? Customer
        {
            get => customer; set => SetPropertyValue(ref customer, value, c =>
            {
                if (c is null)
                {
                    CustomerProject = null;
                }
                else
                {
                    if (CustomerProject is null)
                    {
                        CustomerProject = c.DefaultProject;
                    }
                }
            });
        }

        private CustomerProject? customerProject;
        [Persistent("CustomerProject")]
        public CustomerProject? CustomerProject
        {
            get => customerProject; set => SetPropertyValue(ref customerProject, value, p =>
            {
                if (p is not null && Activity is null)
                {
                    Activity = p.DefaultActivity;
                }
            });
        }

        private Activity? activity;
        [Persistent("Activity")]
        public Activity? Activity { get => activity; set => SetPropertyValue(ref activity, value); }

        private string description = string.Empty;
        [Persistent("Description")]
        public string Description { get => description; set => SetPropertyValue(ref description, value); }

    }

    [Persistent("TimeRecordEntry")]
    public class TimeRecordEntry : DoughnutBaseObject
    {
        public TimeRecordEntry(Session session) : base(session) { }

        private TimeRecord timeRecord;
        [Persistent("TimeRecord")]
        [Association]
        public TimeRecord TimeRecord { get => timeRecord; set => SetPropertyValue(ref timeRecord, value); }

        private DateTime? startDate;
        [Persistent("StartDate")]
        public DateTime? StartDate { get => startDate; set => SetPropertyValue(ref startDate, value); }

        private DateTime? stopDate;
        [Persistent("StopDate")]
        public DateTime? StopDate { get => stopDate; set => SetPropertyValue(ref stopDate, value); }




    }
}
