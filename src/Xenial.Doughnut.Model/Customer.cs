using System;

using DevExpress.Xpo;

namespace Xenial.Doughnut.Model
{
    [Persistent("Customer")]
    public class Customer : DoughnutBaseObject
    {
        public Customer(Session session) : base(session) { }

        private string _Name;
        [Persistent("Name")]
        public string Name { get => _Name; set => SetPropertyValue(ref _Name, value); }

        [Association]
        public XPCollection<CustomerProject> Projects => GetCollection<CustomerProject>();

        private CustomerProject _DefaultProject;
        [Persistent("DefaultProject")]
        public CustomerProject DefaultProject { get => _DefaultProject; set => SetPropertyValue(ref _DefaultProject, value); }
    }

    [Persistent("CustomerProject")]
    public class CustomerProject : DoughnutBaseObject
    {
        public CustomerProject(Session session) : base(session) { }

        private string _Name;
        [Persistent("Name")]
        public string Name { get => _Name; set => SetPropertyValue(ref _Name, value); }

        private Customer _Customer;
        [Persistent("Customer")]
        [Association]
        public Customer Customer
        {
            get => _Customer; set => SetPropertyValue(ref _Customer, value, customer =>
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
    }
}
