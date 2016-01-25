using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneQueryBuilder
{
    public class LuceneBuilder : ILuceneBuilder
    {

        private object inputObject;

        private string WhereCondition { get; set; }

        public ILuceneBuilder Setup(object inputObject)
        {
            this.inputObject = inputObject;
            return this;
        }

        public ILuceneBuilder Field()
        {
            throw new NotImplementedException();
        }

        public ILuceneBuilder And()
        {
            throw new NotImplementedException();
        }

        public ILuceneBuilder Or()
        {
            throw new NotImplementedException();
        }

        public ILuceneBuilder Equal()
        {
            throw new NotImplementedException();
        }

        public ILuceneBuilder Not()
        {
            throw new NotImplementedException();
        }

        public ILuceneBuilder StartWith()
        {
            throw new NotImplementedException();
        }

        public ILuceneBuilder EndWith()
        {
            throw new NotImplementedException();
        }

        public ILuceneBuilder Between()
        {
            throw new NotImplementedException();
        }

        public string Build()
        {
            return WhereCondition;
        }
    }
}
