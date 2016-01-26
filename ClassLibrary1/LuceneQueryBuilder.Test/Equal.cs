using System;
using LuceneQueryBuilder.Test.ModelSample;
using NUnit.Framework;
namespace LuceneQueryBuilder.Test
{
    [TestFixture]
    public class Equal
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
        public void Equal_DoesMatchEqual()
        {
            var value = luceneBuilder.HaveValue(c => c.City).ToString;
            StringAssert.Contains("City:HCM",value);
        }
    }
}
