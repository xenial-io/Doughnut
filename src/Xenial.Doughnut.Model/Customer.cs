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
        private string street;
        [Persistent("Street")]
        public string Street { get => street; set => SetPropertyValue(ref street, value); }

        private string zipCode;
        [Persistent("ZipCode")]
        public string ZipCode { get => zipCode; set => SetPropertyValue(ref zipCode, value); }

        private string city;
        [Persistent("City")]
        public string City { get => city; set => SetPropertyValue(ref city, value); }

        // private Country _Country;
        // [Persistent("Country")]
        // public Country Country { get => _Country; set => SetPropertyValue(ref _Country, value); }

        private string email;
        [Persistent("Email")]
        public string Email { get => email; set => SetPropertyValue(ref email, value); }

        private string website;
        [Persistent("Website")]
        public string Website { get => website; set => SetPropertyValue(ref website, value); }

        private string phone;
        [Persistent("Phone")]
        public string Phone { get => phone; set => SetPropertyValue(ref phone, value); }
        private string comment;
        [Persistent("Comment")]
        [Size(SizeAttribute.Unlimited)]
        public string Comment { get => comment; set => SetPropertyValue(ref comment, value); }

        private string taxNumber;
        [Persistent("TaxNumber")]
        public string TaxNumber { get => taxNumber; set => SetPropertyValue(ref taxNumber, value); }

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
