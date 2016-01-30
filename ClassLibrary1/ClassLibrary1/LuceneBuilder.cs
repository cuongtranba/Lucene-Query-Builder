using System;
using System.Linq.Expressions;
using System.Text;

namespace LuceneQueryBuilder
{
    public class LuceneBuilder
    {
        internal LuceneLogicSymbol luceneLogicSymboy;
        internal LuceneField luceneField;

        internal new string ToString { get { return whereConditionBuilder.ToString(); } }

        internal StringBuilder whereConditionBuilder = new StringBuilder();

        public LuceneBuilder()
        {
            luceneLogicSymboy=new LuceneLogicSymbol(this);
            luceneField=new LuceneField(this);
        }

        public LuceneLogicSymbol HaveValue<TProp>(Expression<Func<TProp>> expression)
        {
            return luceneField.HaveValue(expression);
        }

        public LuceneLogicSymbol Pharase(Action<LuceneBuilder> action)
        {
            this.Add(action);
            return luceneLogicSymboy;
        }

        internal void Add(string syntax)
        {
            whereConditionBuilder.Append(" ");
            whereConditionBuilder.Append(syntax);
            whereConditionBuilder.Append(" ");
        }

        internal void Add(Property property)
        {
            whereConditionBuilder.Append(property.FieldName);
            whereConditionBuilder.Append(":");
            whereConditionBuilder.Append("\"");
            whereConditionBuilder.Append(property.Value);
            whereConditionBuilder.Append("\"");
        }

        internal void Add(Action<LuceneBuilder> action)
        {
            whereConditionBuilder.Append("(");
            action(this);
            whereConditionBuilder.Append(")");
        }
    }

}
