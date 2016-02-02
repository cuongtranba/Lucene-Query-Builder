using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                                    .HaveValue(() => addressEmpty.City)
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
                            builder.WhereEquals(() => addressEmpty.PostalCode).And().HaveValue(() => addressEmpty.Region)).ToString();
                
            StringAssert.AreEqualIgnoringCase(String.Empty, actual);
        }

    }
}
