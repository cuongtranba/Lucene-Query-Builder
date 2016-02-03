using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LuceneQueryBuilder
{
    public class LuceneBuilder : IBaseQueryFluent
    {
        private StringBuilder queryText = new StringBuilder();

        IFieldFluent ILogicSymbolFluent.And()
        {
            return Add(" " + SymbolSyntax.And + " ");
        }

        IFieldFluent ILogicSymbolFluent.Or()
        {
            return Add(" " + SymbolSyntax.Or + " ");
        }

        public ILogicSymbolFluent WhereEquals<TProp>(Expression<Func<TProp>> expression)
        {
            return Add(expression.GetPropertyNameAndValue());
        }

        public ILogicSymbolFluent Pharase(Action<IFieldFluent> action)
        {
            Add("(");
            action(this);
            Add(")");
            return this;
        }

        public ILogicSymbolFluent Not<TProp>(Expression<Func<TProp>> expression)
        {
            throw new NotImplementedException();
        }

        public ILogicSymbolFluent StartWith<TProp>(Expression<Func<TProp>> expression)
        {
            throw new NotImplementedException();
        }

        public ILogicSymbolFluent EndWith<TProp>(Expression<Func<TProp>> expression)
        {
            throw new NotImplementedException();
        }

        public static LuceneBuilder Create()
        {
            return new LuceneBuilder();
        }

        public override string ToString()
        {
            return queryText.ToString();
        }


        private LuceneBuilder Add(string value)
        {
            queryText.Append(value);
            return this;
        }

    }
}
