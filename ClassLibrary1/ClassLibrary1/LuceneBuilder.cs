using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LuceneQueryBuilder
{
    public class LuceneBuilder
    {
        public new string ToString { get { return whereConditionBuilder.ToString(); } }

        private StringBuilder whereConditionBuilder = new StringBuilder();

        public LuceneBuilder HaveValue<TProp>(Expression<Func<TProp>> expression)
        {
            var model = GetPropertyInfo(expression);
            return Add(model);
        }


        public LuceneBuilder And()
        {
            return Add(SymbolSyntax.And);
        }

        public LuceneBuilder Or()
        {
            return Add(SymbolSyntax.Or);
        }

        public LuceneBuilder Pharase()
        {
            return Add("");
        }

        public LuceneBuilder Not()
        {
            return Add(SymbolSyntax.Not);
        }

        private LuceneBuilderModel GetPropertyInfo<TProp>(Expression<Func<TProp>> expression)
        {
            var body = expression.Body as MemberExpression;

            TProp value = expression.Compile()();

            var model = new LuceneBuilderModel()
                        {
                            FieldName = body.Member.Name,
                            Value = value.ToString()
                        };
            return model;
        }


        private LuceneBuilder Add(LuceneBuilderModel model)
        {
            whereConditionBuilder.Append(model.FieldName);
            whereConditionBuilder.Append(":");
            whereConditionBuilder.Append("\"");
            whereConditionBuilder.Append(model.Value);
            whereConditionBuilder.Append("\"");
            return this;
        }


        private LuceneBuilder Add(string syntax)
        {
            whereConditionBuilder.Append(" ");
            whereConditionBuilder.Append(syntax);
            whereConditionBuilder.Append(" ");
            return this;
        }

        private LuceneBuilder Add(Action<LuceneBuilder> action)
        {
            whereConditionBuilder.Append("(");
            action(this);
            whereConditionBuilder.Append(")");
            return this;
        }

        public LuceneBuilder Pharase(Action<LuceneBuilder> action)
        {
            return Add(action);
        }

    }
}
