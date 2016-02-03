using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LuceneQueryBuilder
{
    public interface IFieldFluent
    {
        ILogicSymbolFluent WhereEquals<TProp>(Expression<Func<TProp>> expression);
    }
}
