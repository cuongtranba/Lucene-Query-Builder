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
        private Stack<string> stack = new Stack<string>();

        IFieldFluent ILogicSymbolFluent.And()
        {
            return Add(SymbolSyntaxTemplate.And, true);
        }

        IFieldFluent ILogicSymbolFluent.Or()
        {
            return Add(SymbolSyntaxTemplate.Or, true);
        }

        public ILogicSymbolFluent WhereEquals<TProp>(Expression<Func<TProp>> expression)
        {
            return Add(GetPropertyNameAndValue(expression, SymbolSyntaxTemplate.WhereEquals));
        }

        public ILogicSymbolFluent Pharase(Action<IFieldFluent> action)
        {
            Add("(", true);
            action(this);
            Add(")", true);
            return this;
        }

        public ILogicSymbolFluent Not<TProp>(Expression<Func<TProp>> expression)
        {
            return Add(GetPropertyNameAndValue(expression, SymbolSyntaxTemplate.Not));
        }

        public ILogicSymbolFluent StartWith<TProp>(Expression<Func<TProp>> expression)
        {
            return Add(GetPropertyNameAndValue(expression, SymbolSyntaxTemplate.StartWith));
        }

        public ILogicSymbolFluent EndWith<TProp>(Expression<Func<TProp>> expression)
        {
            return Add(GetPropertyNameAndValue(expression, SymbolSyntaxTemplate.EndWith));
        }

        public static LuceneBuilder Create()
        {
            return new LuceneBuilder();
        }

        public override string ToString()
        {
            return queryText.Append(stack.Pop()).ToString();
        }

        private LuceneBuilder Add(string value, bool isOperator = false)
        {
            if (isOperator == false)
            {
                stack.Push(value);
            }
            else
            {
                var previousValue = stack.Peek();
                if (previousValue == String.Empty)
                {
                    return this;
                }
                else
                {
                    queryText.Append(previousValue).Append(value);
                }
            }
            return this;
        }
        private string GetPropertyNameAndValue<TProp>(Expression<Func<TProp>> expression, string type)
        {
            var body = expression.Body as MemberExpression;

            TProp value = expression.Compile()();

            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return String.Empty;
            }
            var luceneField = new LuceneField()
            {
                FieldName = body.Member.Name,
                Value = value.ToString(),
                Template = type
            };
            return luceneField.ToString();
        }
    }

    public class LuceneField
    {
        public string FieldName { get; set; }
        public string Value { get; set; }
        public string Template { get; set; }

        public override string ToString()
        {
            return String.Format(Template, FieldName, Value);
        }
    }
}
