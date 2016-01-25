using System;
using LuceneQueryBuilder.Test.ModelSample;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using LuceneQueryBuilder;
namespace LuceneQueryBuilder.Test
{
    [TestFixture]
    public class Equal
    {
        private Address address;
        private ILuceneBuilder luceneBuilder;
        [SetUp]
        public void Init()
        {
            address=new Address()
            {
                City = "HCM",
                Country = "VietNam",
                Line1 = "Phuoc binh",
                Line2 = "Quan9",
                PostalCode = "99999",
                Region = "Duong7"
            };
            luceneBuilder = new LuceneBuilder();
        }

        [Test]
        public void Equal_DoesMatchEqual()
        {
            var builder=luceneBuilder.
        }
    }
}
