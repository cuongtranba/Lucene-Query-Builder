using System;
using LuceneQueryBuilder.Test.ModelSample;
using NUnit.Framework;

namespace LuceneQueryBuilder.Test
{
    [TestFixture]
    public class LuceneQueryBuilderTest
    {
        private Address address;
        private Customer Customer;
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
                Region = "Duong7",
            };
            Customer = new Customer()
            {
                Address = address,
                FirstName = "Cuong",
                LastName = "Tran"
            };
        }
        [Test]
        public void EqualsTest()
        {
            var actual = LuceneBuilder.Create().WhereEquals(() => address.City).ToString();
            StringAssert.AreEqualIgnoringCase("City:\"HCM\"", actual);
        }
        [Test]
        public void LuceneSymbolSyntax()
        {
            var actual =
                LuceneBuilder.Create().WhereEquals(() => address.City)
                    .And()
                    .WhereEquals(() => address.Country)
                    .Or()
                    .WhereEquals(() => address.Line1)
                    .ToString();
            StringAssert.AreEqualIgnoringCase("City:\"HCM\" AND Country:\"VietNam\" OR Line1:\"Phuoc binh\"", actual);
        }
        [Test]
        public void Pharase()
        {
            var actual = LuceneBuilder.Create()
                .WhereEquals(() => address.City)
                .And()
                .WhereEquals(() => address.Country)
                .Or()
                .WhereEquals(() => address.Line1)
                .And()
                .Pharase(a => a.WhereEquals(() => address.Line2).And().WhereEquals(() => address.PostalCode)).ToString();

            StringAssert.AreEqualIgnoringCase("City:\"HCM\" AND Country:\"VietNam\" OR Line1:\"Phuoc binh\" AND (Line2:\"Quan9\" AND PostalCode:\"99999\")", actual);
        }
        [Test]
        public void PharaseAtBegin()
        {
            var actual = LuceneBuilder.Create()
                .Pharase(builder => builder.WhereEquals(() => address.PostalCode).And().WhereEquals(() => Customer.FirstName))
                .And()
                .WhereEquals(() => address.Country)
                .Or()
                .WhereEquals(() => address.Line1)
                .And()
                .Pharase(a => a.WhereEquals(() => address.Line2).And().WhereEquals(() => address.PostalCode)).ToString();

            StringAssert.AreEqualIgnoringCase("(PostalCode:\"99999\" AND FirstName:\"Cuong\") AND Country:\"VietNam\" OR Line1:\"Phuoc binh\" AND (Line2:\"Quan9\" AND PostalCode:\"99999\")", actual);
        }
        [Test]
        public void PharaseInPharase()
        {
            var actual = LuceneBuilder.Create()
                .Pharase(builder => builder
                            .WhereEquals(() => address.PostalCode)
                            .And()
                            .WhereEquals(() => Customer.FirstName)
                            .Or()
                            .Pharase(builder1 => builder1
                                        .WhereEquals(() => Customer.LastName)
                                        .Or()
                                        .WhereEquals(() => Customer.FirstName)
                                    )
                        )
                .And()
                .WhereEquals(() => address.Country)
                .Or()
                .WhereEquals(() => address.Line1)
                .And()
                .Pharase(a => a.WhereEquals(() => address.Line2).And().WhereEquals(() => address.PostalCode)).ToString();

            StringAssert.AreEqualIgnoringCase("(PostalCode:\"99999\" AND FirstName:\"Cuong\" OR (LastName:\"Tran\" OR FirstName:\"Cuong\")) AND Country:\"VietNam\" OR Line1:\"Phuoc binh\" AND (Line2:\"Quan9\" AND PostalCode:\"99999\")", actual);
        }

        [Test]
        public void StartWithTest()
        {
            var actual = LuceneBuilder.Create().StartWith(() => address.City).ToString();
            StringAssert.AreEqualIgnoringCase("City:HCM*", actual);
        }

        [Test]
        public void EndWithTest()
        {
            var actual = LuceneBuilder.Create().EndWith(() => address.City).ToString();
            StringAssert.AreEqualIgnoringCase("City:*HCM", actual);
        }

        [Test]
        public void NotTest()
        {
            var actual = LuceneBuilder.Create().Not(() => address.City).And().WhereEquals(() => address.Line1).ToString();
            StringAssert.AreEqualIgnoringCase("-City:HCM AND Line1:\"Phuoc binh\"", actual);
        }

        [Test]
        public void RangeTest()
        {
            var actual = LuceneBuilder.Create().Range(() => address.StateCode,1,2).ToString();
            StringAssert.AreEqualIgnoringCase("StateCode:[1 TO 2]", actual);
        }
    }
}
