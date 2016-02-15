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
        private List<string> listSyntax = new List<string>();

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
            return Add(BuildField(expression, SymbolSyntaxTemplate.WhereEquals));
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
            return Add(BuildField(expression, SymbolSyntaxTemplate.Not));
        }

        public ILogicSymbolFluent StartWith<TProp>(Expression<Func<TProp>> expression)
        {
            return Add(BuildField(expression, SymbolSyntaxTemplate.StartWith));
        }

        public ILogicSymbolFluent EndWith<TProp>(Expression<Func<TProp>> expression)
        {
            return Add(BuildField(expression, SymbolSyntaxTemplate.EndWith));
        }

        public ILogicSymbolFluent Range<TProp>(Expression<Func<TProp>> expression, object value1, object value2)
        {

            var body = expression.Body as MemberExpression;

            TProp value = expression.Compile()();


            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return Add(String.Empty); ;
            }
            return Add(String.Format("{0}:[{1} TO {2}]", body.Member.Name, value1, value2));
        }


        public static LuceneBuilder Create()
        {
            return new LuceneBuilder();
        }
        /// <summary>
        /// build query form listSyntax and will clear list syntax if it only contains begin pharase or end pharase
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (listSyntax.All(c => c == SymbolSyntaxTemplate.BeginPharase || c == SymbolSyntaxTemplate.EndPharase))
            {
                listSyntax.Clear();
            }
            foreach (var syntax in listSyntax)
            {
                queryText.Append(syntax);
            }
            return queryText.ToString();
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isOperator"></param>
        /// <returns></returns>
        private LuceneBuilder Add(string value, bool isOperator = false)
        {
            if (!listSyntax.IsEmpty() && (value == String.Empty && isOperator == false))
            {
                if (listSyntax.PeekLast() == SymbolSyntaxTemplate.BeginPharase)
                {
                    return this;
                }
                listSyntax.PopLast();
                return this;
            }
            if (isOperator && !listSyntax.IsEmpty())
            {
                if (listSyntax.PeekLast() == SymbolSyntaxTemplate.BeginPharase)
                {
                    return this;
                }

                if (listSyntax.PeekLast() == String.Empty)
                {
                    listSyntax.PopLast();
                    return this;
                }
            }
            listSyntax.Add(value);
            return this;
        }
        private string BuildField<TProp>(Expression<Func<TProp>> expression, string type)
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
