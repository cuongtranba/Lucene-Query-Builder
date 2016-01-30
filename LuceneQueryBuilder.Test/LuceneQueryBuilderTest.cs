using LuceneQueryBuilder.Test.ModelSample;
using NUnit.Framework;

namespace LuceneQueryBuilder.Test
{
    [TestFixture]
    public class LuceneQueryBuilderTest
    {
        private Address address;
        private Customer Customer;
        private LuceneBuilder luceneBuilder;
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

            luceneBuilder = new LuceneBuilder();
        }

        [Test]
        public void HaveValueTest()
        {
            var value = luceneBuilder.HaveValue(() => address.City).ToString();
            StringAssert.AreEqualIgnoringCase("City:\"HCM\"", value);
        }
        [Test]
        public void ShouldNotAppendValueIfEmpty()
        {
            var emptyAddress=new Address()
            {
                City = "",
                Line1 = "duong7,phuocbinh"
            };
            var value = luceneBuilder.HaveValue(() => emptyAddress.City).And().HaveValue(() => emptyAddress.Line1).ToString();
            StringAssert.AreEqualIgnoringCase("Line1:\"duong7,phuocbinh\"", value);
        }


        [Test]
        public void LuceneSymbolSyntax()
        {
            var value =
                luceneBuilder.HaveValue(() => address.City)
                    .And()
                    .HaveValue(() => address.Country)
                    .Or()
                    .HaveValue(() => address.Line1)
                    .ToString();
            StringAssert.AreEqualIgnoringCase("City:\"HCM\" AND Country:\"VietNam\" OR Line1:\"Phuoc binh\"", value);
        }

        [Test]
        public void Pharase()
        {
            var value = luceneBuilder
                .HaveValue(() => address.City)
                .And()
                .HaveValue(() => address.Country)
                .Or()
                .HaveValue(() => address.Line1)
                .And()
                .Pharase(a => a.HaveValue(() => address.Line2).And().HaveValue(() => address.PostalCode)).ToString();

            StringAssert.AreEqualIgnoringCase("City:\"HCM\" AND Country:\"VietNam\" OR Line1:\"Phuoc binh\" AND (Line2:\"Quan9\" AND PostalCode:\"99999\")", value);
        }
        [Test]
        public void PharaseAtBegin()
        {
            var value = luceneBuilder
                .Pharase(builder => builder.HaveValue(() => address.PostalCode).And().HaveValue(() => Customer.FirstName))
                .And()
                .HaveValue(() => address.Country)
                .Or()
                .HaveValue(() => address.Line1)
                .And()
                .Pharase(a => a.HaveValue(() => address.Line2).And().HaveValue(() => address.PostalCode)).ToString();

            StringAssert.AreEqualIgnoringCase("(PostalCode:\"99999\" AND FirstName:\"Cuong\") AND Country:\"VietNam\" OR Line1:\"Phuoc binh\" AND (Line2:\"Quan9\" AND PostalCode:\"99999\")", value);
        }
        [Test]
        public void PharaseInPharase()
        {
            var value = luceneBuilder
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

            StringAssert.AreEqualIgnoringCase("(PostalCode:\"99999\" AND FirstName:\"Cuong\" OR (LastName:\"Tran\" OR FirstName:\"Cuong\")) AND Country:\"VietNam\" OR Line1:\"Phuoc binh\" AND (Line2:\"Quan9\" AND PostalCode:\"99999\")", value);
        }

        //[Test]
        //public void CreateQueryOnNestedObject()
        //{
        //    var value = luceneBuilder
        //        .HaveValue(() => address.City)
        //        .And()
        //        .HaveValue(() => address.Country)
        //        .Or()
        //        .HaveValue(() => address.Line1)
        //        .And()
        //        .Pharase(a => a.HaveValue(() => address.Line2).And().HaveValue(() => address.PostalCode))
        //        .And()
        //        .HaveValue(() => Customer.Address.Region)
        //        .ToString;
        //    StringAssert.AreEqualIgnoringCase("City:\"HCM\" AND Country:\"VietNam\" OR Line1:\"Phuoc binh\" AND (Line2:\"Quan9\" AND PostalCode:\"99999\") AND Region:\"Duong7\"", value);
        //}
    }
}
