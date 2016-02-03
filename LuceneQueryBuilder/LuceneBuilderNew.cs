using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LuceneQueryBuilder
{
    public class LuceneBuilderNew : IBaseQueryFluent
    {
        private StringBuilder queryText = new StringBuilder();

        IFieldFluent ILogicSymbolFluent.And()
        {
            return Add(" " + SymbolSyntax.And + " ");
        }

        public ILogicSymbolFluent WhereEquals<TProp>(Expression<Func<TProp>> expression)
        {
            return Add(expression.GetPropertyNameAndValue());
        }

        public static LuceneBuilderNew Create()
        {
            return new LuceneBuilderNew();
        }

        public override string ToString()
        {
            return queryText.ToString();
        }


        private LuceneBuilderNew Add(string value)
        {
            queryText.Append(value);
            return this;
        }

    }
}
