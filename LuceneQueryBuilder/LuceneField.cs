using System;
using System.Linq.Expressions;

namespace LuceneQueryBuilder
{
    public class LuceneField
    {
        private LuceneBuilder luceneBuilder;

        public override string ToString()
        {
            return luceneBuilder.ToString;
        }

        private Property GetPropertyInfo<TProp>(Expression<Func<TProp>> expression)
        {
            var body = expression.Body as MemberExpression;

            TProp value = expression.Compile()();

            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return null;
            }
            var model = new Property()
            {
                FieldName = body.Member.Name,
                Value = value.ToString()
            };
            return model;
        }

        public LuceneField(LuceneBuilder luceneBuilder)
        {
            this.luceneBuilder = luceneBuilder;
        }

        public LuceneLogicSymbol WhereEquals<TProp>(Expression<Func<TProp>> expression)
        {
            luceneBuilder.Add(GetPropertyInfo(expression));
            return luceneBuilder.luceneLogicSymboy;
        }
        public LuceneLogicSymbol Pharase(Action<LuceneBuilder> action)
        {
            luceneBuilder.Add(action);
            return luceneBuilder.luceneLogicSymboy;
        }

        public LuceneLogicSymbol Not<TProp>(Expression<Func<TProp>> expression)
        {
            return luceneBuilder.luceneLogicSymboy;
        }

        internal LuceneLogicSymbol StartWith<TProp>(Expression<Func<TProp>> expression)
        {
            throw new NotImplementedException();
        }

        internal LuceneLogicSymbol EndWith<TProp>(Expression<Func<TProp>> expression)
        {
            throw new NotImplementedException();
        }
    }
    class Property
    {
        public string FieldName { get; set; }
        public string Value { get; set; }
    }

}