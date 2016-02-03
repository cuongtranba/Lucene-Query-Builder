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
        ILogicSymbolFluent Pharase(Action<IFieldFluent> action);
        ILogicSymbolFluent Not<TProp>(Expression<Func<TProp>> expression);
        ILogicSymbolFluent StartWith<TProp>(Expression<Func<TProp>> expression);
        ILogicSymbolFluent EndWith<TProp>(Expression<Func<TProp>> expression);
    }
}
