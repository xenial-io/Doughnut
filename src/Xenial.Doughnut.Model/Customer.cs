using System;

using DevExpress.Xpo;

namespace Xenial.Doughnut.Model
{
    [Persistent("Customer")]
    public class Customer : DoughnutBaseObject
    {
        public Customer(Session session) : base(session) { }

        private string name;
        [Persistent("Name")]
        public string Name { get => name; set => SetPropertyValue(ref name, value); }

        [Association]
        public XPCollection<CustomerProject> Projects => GetCollection<CustomerProject>();

        private CustomerProject defaultProject;
        [Persistent("DefaultProject")]
        public CustomerProject DefaultProject { get => defaultProject; set => SetPropertyValue(ref defaultProject, value); }
    }

    [Persistent("CustomerProject")]
    public class CustomerProject : DoughnutBaseObject
    {
        public CustomerProject(Session session) : base(session) { }

        private string name;
        [Persistent("Name")]
        public string Name { get => name; set => SetPropertyValue(ref name, value); }

        private Customer customer;
        [Persistent("Customer")]
        [Association]
        public Customer Customer
        {
            get => customer; set => SetPropertyValue(ref customer, value, customer =>
            {
                if (customer != null && Session.IsNewObject(customer))
                {
                    if (customer.DefaultProject == null && customer.Projects.Count == 1)
                    {
                        customer.DefaultProject = this;
                    }
                }
            });
        }

        private Activity? defaultActivity;
        [Persistent("DefaultActivity")]
        public Activity? DefaultActivity { get => defaultActivity; set => SetPropertyValue(ref defaultActivity, value); }
    }
}
