using System;

namespace Xenial.Doughnut.Model
{
    public static class ModelTypeList
    {
        public static Type[] ModelTypes { get; } = new[]
        {
            typeof(DoughnutBaseObject),

            typeof(TimeRecord),

            typeof(Customer),
            typeof(CustomerProject)
        };
    }
}
