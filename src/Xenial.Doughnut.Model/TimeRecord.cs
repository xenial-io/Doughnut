using System;

using DevExpress.Xpo;

namespace Xenial.Doughnut.Model
{
    [Persistent("TimeRecord")]
    public class TimeRecord : DoughnutBaseObject
    {
        public TimeRecord(Session session) : base(session) { }

    }
}
