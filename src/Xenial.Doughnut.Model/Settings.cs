using System;

using DevExpress.Xpo;

namespace Xenial.Doughnut.Model
{
    [Persistent("Settings")]
    [Singleton]
    public class Settings : DoughnutBaseObject
    {
        public Settings(Session session) : base(session) { }

        private string companyName;
        [Persistent("CompanyName")]
        public string CompanyName { get => companyName; set => SetPropertyValue(ref companyName, value); }

        private string ownerName;
        [Persistent("OwnerName")]
        public string OwnerName { get => ownerName; set => SetPropertyValue(ref ownerName, value); }

        private string companyEmail;
        [Persistent("CompanyEmail")]
        public string CompanyEmail { get => companyEmail; set => SetPropertyValue(ref companyEmail, value); }

        private string companyWebsite;
        [Persistent("CompanyWebsite")]
        public string CompanyWebsite { get => companyWebsite; set => SetPropertyValue(ref companyWebsite, value); }

        private string companyPhone;
        [Persistent("CompanyPhone")]
        public string CompanyPhone { get => companyPhone; set => SetPropertyValue(ref companyPhone, value); }

        private string companyTaxNumber;
        [Persistent("CompanyTaxNumber")]
        public string CompanyTaxNumber { get => companyTaxNumber; set => SetPropertyValue(ref companyTaxNumber, value); }

        private string companyVatNumber;
        [Persistent("CompanyVatNumber")]
        public string CompanyVatNumber { get => companyVatNumber; set => SetPropertyValue(ref companyVatNumber, value); }

        private string companyStreet;
        [Persistent("CompanyStreet")]
        public string CompanyStreet { get => companyStreet; set => SetPropertyValue(ref companyStreet, value); }

        private string companyPostalCode;
        [Persistent("CompanyPostalCode")]
        public string CompanyPostalCode { get => companyPostalCode; set => SetPropertyValue(ref companyPostalCode, value); }

        private string companyCity;
        [Persistent("CompanyCity")]
        public string CompanyCity { get => companyCity; set => SetPropertyValue(ref companyCity, value); }

        private Country companyCountry;
        [Persistent("CompanyCountry")]
        public Country CompanyCountry { get => companyCountry; set => SetPropertyValue(ref companyCountry, value); }

        private byte[] companyLogo;
        [Persistent("CompanyLogo")]
        public byte[] CompanyLogo { get => companyLogo; set => SetPropertyValue(ref companyLogo, value); }
    }
}
