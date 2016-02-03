using LuceneQueryBuilder.Test.ModelSample;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneQueryBuilder.Test
{
    [TestFixture]
    class LuceneQueryBuilderNewTest
    {
        [Test]
        public void TestWhereEquals()
        {
            var address = new Address()
            {

                City = "HCM",
                Line1="HCM2"
            };

            var actual = LuceneBuilderNew.Create().WhereEquals(() => address.City).And().WhereEquals(()=>address.Line1).ToString();

            StringAssert.AreEqualIgnoringCase("City:\"HCM\" AND Line1:\"HCM2\"", actual);
        }
    }
}
