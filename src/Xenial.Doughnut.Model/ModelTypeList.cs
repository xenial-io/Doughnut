using System;

namespace Xenial.Doughnut.Model
{
    public static class ModelTypeList
    {
        public static Type[] ModelTypes { get; } = new[]
        {
            typeof(DoughnutBaseObject),

            typeof(Country),
            
            typeof(Settings),

            typeof(Activity),

            typeof(TimeRecord),
            typeof(TimeRecordEntry),

            typeof(Customer),
            typeof(CustomerProject)
        };
    }
}
