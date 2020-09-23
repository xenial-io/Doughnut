using System;

using DevExpress.Xpo;

namespace Xenial.Doughnut.Model
{
    [Persistent("Activity")]
    public class Activity : DoughnutBaseObject
    {
        public Activity(Session session) : base(session) { }

        private string name;
        [Persistent("Name")]
        public string Name { get => name; set => SetPropertyValue(ref name, value); }

        private decimal price;
        [Persistent("Price")]
        public decimal Price { get => price; set => SetPropertyValue(ref price, value); }
    }


    
}
