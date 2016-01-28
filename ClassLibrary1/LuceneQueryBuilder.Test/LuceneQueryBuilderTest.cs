using System;
using LuceneQueryBuilder.Test.ModelSample;
using NUnit.Framework;
namespace LuceneQueryBuilder.Test
{
    [TestFixture]
    public class LuceneQueryBuilderTest
    {
        private Address address;
        private LuceneBuilder<Address> luceneBuilder;
        [SetUp]
        public void Init()
        {
            address = new Address()
            {
                City = "HCM",
                Country = "VietNam",
                Line1 = "Phuoc binh",
                Line2 = "Quan9",
                PostalCode = "99999",
                Region = "Duong7"
            };
            luceneBuilder = new LuceneBuilder<Address>();
        }
        
        [Test]
        public void HaveValueTest()
        {
            var value = luceneBuilder.Create(address).HaveValue(c => c.City).ToString;
            StringAssert.AreEqualIgnoringCase("City:\"HCM\"", value);
        }

        [Test]
        public void LuceneSymbolSyntax()
        {
            var value = luceneBuilder.Create(address)
                .HaveValue(c => c.City)
                .And()
                .HaveValue(c=>c.Country)
                .Or()
                .HaveValue(c=>c.Line1)
                .ToString;
            StringAssert.AreEqualIgnoringCase("City:\"HCM\" AND Country:\"VietNam\" OR Line1:\"Phuoc binh\"", value);
        }

        [Test]
        public void Pharase()
        {
            var value = luceneBuilder.Create(address)
                .HaveValue(c => c.City)
                .And()
                .HaveValue(c => c.Country)
                .Or()
                .HaveValue(c => c.Line1)
                .And()
                .Pharase(a => a.HaveValue(c => c.Line2).And().HaveValue(c => c.PostalCode))
                .ToString;
            StringAssert.AreEqualIgnoringCase("City:\"HCM\" AND Country:\"VietNam\" OR Line1:\"Phuoc binh\" AND (Line2:\"Quan9\" AND PostalCode:\"99999\")", value);
        }
    }
}
