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
        public void HaveValueTest()
        {
            var expected = LuceneBuilder.Create().HaveValue(() => address.City).ToString();
            StringAssert.AreEqualIgnoringCase("City:\"HCM\"", expected);
        }
        [Test]
        public void ShouldNotAppendValueIfEmpty()
        {
            var emptyAddress=new Address()
            {
                City = "",
                Line1 = "duong7,phuocbinh"
            };
            var expected = LuceneBuilder.Create().HaveValue(() => emptyAddress.City).And().HaveValue(() => emptyAddress.Line1).ToString();
            StringAssert.AreEqualIgnoringCase("Line1:\"duong7,phuocbinh\"", expected);
        }

        [Test]
        public void ShouldNotAppendValueIfNull()
        {
            var emptyAddress = new Address()
            {
                City = null,
                Line1 = "duong7,phuocbinh"
            };
            var expected = LuceneBuilder.Create().HaveValue(() => emptyAddress.City).And().HaveValue(() => emptyAddress.Line1).ToString();
            StringAssert.AreEqualIgnoringCase("Line1:\"duong7,phuocbinh\"", expected);
        }

        [Test]
        public void ShouldNotAppendValueIfNullAndBeforeSymbolSyntax()
        {
            var emptyAddress = new Address()
            {
                PostalCode = "123",
                City = null,
                Line1 = "duong7,phuocbinh"
            };
            var expected = LuceneBuilder.Create()
                .HaveValue(() => emptyAddress.PostalCode)
                .Or()
                .HaveValue(() => emptyAddress.City)
                .And()
                .HaveValue(() => emptyAddress.Line1).ToString();
            StringAssert.AreEqualIgnoringCase("PostalCode:\"123\" AND Line1:\"duong7,phuocbinh\"", expected);
        }

        [Test]
        public void ShouldNotAppendValueIfNullAndAfterSymbolSyntax()
        {
            var emptyAddress = new Address()
            {
                PostalCode = "123",
                City = null,
                Line1 = "duong7,phuocbinh",
                Region = "Hanoi"
            };
            var expected = LuceneBuilder.Create()
                .HaveValue(() => emptyAddress.PostalCode)
                .Or()
                .HaveValue(() => emptyAddress.City)
                .And()
                .HaveValue(() => emptyAddress.Line1)
                .ToString();
            StringAssert.AreEqualIgnoringCase("PostalCode:\"123\" And Line1:\"duong7,phuocbinh\"", expected);
        }

        [Test]
        public void LuceneSymbolSyntax()
        {
            var expected =
                LuceneBuilder.Create().HaveValue(() => address.City)
                    .And()
                    .HaveValue(() => address.Country)
                    .Or()
                    .HaveValue(() => address.Line1)
                    .ToString();
            StringAssert.AreEqualIgnoringCase("City:\"HCM\" AND Country:\"VietNam\" OR Line1:\"Phuoc binh\"", expected);
        }

        [Test]
        public void Pharase()
        {
            var expected = LuceneBuilder.Create()
                .HaveValue(() => address.City)
                .And()
                .HaveValue(() => address.Country)
                .Or()
                .HaveValue(() => address.Line1)
                .And()
                .Pharase(a => a.HaveValue(() => address.Line2).And().HaveValue(() => address.PostalCode)).ToString();

            StringAssert.AreEqualIgnoringCase("City:\"HCM\" AND Country:\"VietNam\" OR Line1:\"Phuoc binh\" AND (Line2:\"Quan9\" AND PostalCode:\"99999\")", expected);
        }
        [Test]
        public void PharaseAtBegin()
        {
            var expected = LuceneBuilder.Create()
                .Pharase(builder => builder.HaveValue(() => address.PostalCode).And().HaveValue(() => Customer.FirstName))
                .And()
                .HaveValue(() => address.Country)
                .Or()
                .HaveValue(() => address.Line1)
                .And()
                .Pharase(a => a.HaveValue(() => address.Line2).And().HaveValue(() => address.PostalCode)).ToString();

            StringAssert.AreEqualIgnoringCase("(PostalCode:\"99999\" AND FirstName:\"Cuong\") AND Country:\"VietNam\" OR Line1:\"Phuoc binh\" AND (Line2:\"Quan9\" AND PostalCode:\"99999\")", expected);
        }
        [Test]
        public void PharaseInPharase()
        {
            var expected = LuceneBuilder.Create()
                .Pharase(builder =>builder
                            .HaveValue(() => address.PostalCode)
                            .And()
                            .HaveValue(() => Customer.FirstName)
                            .Or()
                            .Pharase(builder1 => builder1
                                        .HaveValue(() => Customer.LastName)
                                        .Or()
                                        .HaveValue(() => Customer.FirstName)
                                    )
                        )
                .And()
                .HaveValue(() => address.Country)
                .Or()
                .HaveValue(() => address.Line1)
                .And()
                .Pharase(a => a.HaveValue(() => address.Line2).And().HaveValue(() => address.PostalCode)).ToString();

            StringAssert.AreEqualIgnoringCase("(PostalCode:\"99999\" AND FirstName:\"Cuong\" OR (LastName:\"Tran\" OR FirstName:\"Cuong\")) AND Country:\"VietNam\" OR Line1:\"Phuoc binh\" AND (Line2:\"Quan9\" AND PostalCode:\"99999\")", expected);
        }
        [Test]
        public void RemovePharaseIfAllPropertiesIsEmpty()
        {
            var emptyAddress=new Address()
            {
                Line1 = null,
                Country = null,
                PostalCode = ""
            };
            var expected =
                LuceneBuilder.Create().Pharase(
                    builder =>
                        builder.HaveValue(() => emptyAddress.Line1)
                            .And()
                            .HaveValue(() => emptyAddress.Country)
                            .Or()
                            .HaveValue(() => emptyAddress.PostalCode)).ToString();

            StringAssert.AreEqualIgnoringCase(String.Empty, expected);
        }
        [Test]
        public void RemoveValueInPharaseIfPropertiesIsEmpty()
        {
            var emptyAddress = new Address()
            {
                Line1 = null,
                Country = "VietName",
                PostalCode = ""
            };
            var expected =
                LuceneBuilder.Create().Pharase(
                    builder =>
                        builder.HaveValue(() => emptyAddress.Line1)
                            .And()
                            .HaveValue(() => emptyAddress.Country)
                            .Or()
                            .HaveValue(() => emptyAddress.PostalCode)).ToString();

            StringAssert.AreEqualIgnoringCase("(Country:\"VietName\")", expected);
        }
    }
}
