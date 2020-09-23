
using DevExpress.Xpo;

namespace Xenial.Doughnut.Model
{
    [Persistent("Country")]
    public class Country : DoughnutBaseObject
    {
        public Country(Session session) : base(session) { }

        [PersistentAlias("Concat([CountryAbbreviationISO_2],' - ', [CountryName])")]
        public string FullName => $"{CountryAbbreviationISO_2} - {CountryName}";

        private string _CountryName;
        [Persistent("CountryName")]
        public string CountryName { get => _CountryName; set => SetPropertyValue(ref _CountryName, value); }

        private string _CountryAbbreviationISO_2;
        [Persistent("CountryAbbreviationISO_2")]
        [Size(2)]
        [Indexed(Unique = true)]
        public string CountryAbbreviationISO_2 { get => _CountryAbbreviationISO_2; set => SetPropertyValue(ref _CountryAbbreviationISO_2, value); }

        private string _CountryAbbreviationISO_3;
        [Persistent("CountryAbbreviationISO_3")]
        [Size(3)]
        [Indexed(Unique = true)]
        public string CountryAbbreviationISO_3 { get => _CountryAbbreviationISO_3; set => SetPropertyValue(ref _CountryAbbreviationISO_3, value); }

    }
}
