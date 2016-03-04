using System;
using LuceneQueryBuilder.Test.ModelSample;
using NUnit.Framework;

namespace LuceneQueryBuilder.Test
{
    [TestFixture]
    public class EmptyPropertyTest
    {
        [Test]
        public void RemoveSyntaxInPharaseWhenPropertyNull()
        {
            var addressEmpty = new Address()
            {
                Country = null,
                City = String.Empty,
                PostalCode = String.Empty
            };
            var actual = LuceneBuilder.Create()
                .Pharase(
                            a => a.Pharase(
                                    b => b
                                    .WhereEquals(() => addressEmpty.Country)
                                    .And()
                                    .WhereEquals(() => addressEmpty.City)
                                )
                        ).ToString();
            StringAssert.AreEqualIgnoringCase(String.Empty, actual);
        }
        [Test]
        public void RemoveSyntaxWhenPropertyNull()
        {
            var addressEmpty = new Address()
            {
                Country = null,
                City = String.Empty,
                PostalCode = String.Empty
            };
            var actual =
                LuceneBuilder.Create()
                    .WhereEquals(() => addressEmpty.City)
                    .And()
                    .Pharase(
                        builder =>
                            builder.WhereEquals(() => addressEmpty.PostalCode).And().WhereEquals(() => addressEmpty.Region)).ToString();
                
            StringAssert.AreEqualIgnoringCase(String.Empty, actual);
        }
        [Test]
        public void ShouldNotAppendValueIfEmpty()
        {
            var emptyAddress = new Address()
            {
                City = "",
                Line1 = "duong7,phuocbinh"
            };
            var expected = LuceneBuilder.Create().WhereEquals(() => emptyAddress.City).And().WhereEquals(() => emptyAddress.Line1).ToString();
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
            var expected = LuceneBuilder.Create().WhereEquals(() => emptyAddress.City).And().WhereEquals(() => emptyAddress.Line1).ToString();
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
                .WhereEquals(() => emptyAddress.PostalCode)
                .Or()
                .WhereEquals(() => emptyAddress.City)
                .And()
                .WhereEquals(() => emptyAddress.Line1).ToString();
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
                .WhereEquals(() => emptyAddress.PostalCode)
                .Or()
                .WhereEquals(() => emptyAddress.City)
                .And()
                .WhereEquals(() => emptyAddress.Line1)
                .ToString();
            StringAssert.AreEqualIgnoringCase("PostalCode:\"123\" And Line1:\"duong7,phuocbinh\"", expected);
        }
        [Test]
        public void RemovePharaseIfAllPropertiesIsEmpty()
        {
            var emptyAddress = new Address()
            {
                Line1 = null,
                Country = null,
                PostalCode = ""
            };
            var expected =
                LuceneBuilder.Create().Pharase(
                    builder =>
                        builder.WhereEquals(() => emptyAddress.Line1)
                            .And()
                            .WhereEquals(() => emptyAddress.Country)
                            .Or()
                            .WhereEquals(() => emptyAddress.PostalCode)).ToString();

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
                        builder.WhereEquals(() => emptyAddress.Line1)
                            .And()
                            .WhereEquals(() => emptyAddress.Country)
                            .Or()
                            .WhereEquals(() => emptyAddress.PostalCode)).ToString();

            StringAssert.AreEqualIgnoringCase("(Country:\"VietName\")", expected);
        }
    }
}
