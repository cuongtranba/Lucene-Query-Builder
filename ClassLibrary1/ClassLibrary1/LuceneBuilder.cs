using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LuceneQueryBuilder
{
    public class LuceneBuilder<T> where T : class
    {
        public new string ToString { get { return whereConditionBuilder.ToString(); } }

        private StringBuilder whereConditionBuilder = new StringBuilder();
        private object modelBuilder;

        public LuceneBuilder<T> HaveValue<TProp>(Expression<Func<T, TProp>> expression)
        {
            var model = GetPropertyInfo(expression);
            return Add(model);
        }

        public LuceneBuilder<T> And()
        {
            return Add(SymbolSyntax.And);
        }

        public LuceneBuilder<T> Or()
        {
            return Add(SymbolSyntax.Or);
        }

        public LuceneBuilder<T> Pharase()
        {
            return Add("");
        }

        public LuceneBuilder<T> Not()
        {
            return Add(SymbolSyntax.Not);
        }

        private LuceneBuilderModel GetPropertyInfo<TProp>(Expression<Func<T, TProp>> expression)
        {
            var body = expression.Body as MemberExpression;

            if (body == null)
            {
                throw new ArgumentException("'expression' should be a member expression");
            }

            var propertyInfo = (PropertyInfo)body.Member;
            var propertyName = propertyInfo.Name;
            var value = modelBuilder.GetType().GetProperty(propertyName).GetValue(modelBuilder).ToString();
            var model = new LuceneBuilderModel()
                        {
                            FieldName = propertyName,
                            Value = value
                        };
            return model;
        }


        private LuceneBuilder<T> Add(LuceneBuilderModel model)
        {
            whereConditionBuilder.Append(model.FieldName);
            whereConditionBuilder.Append(":");
            whereConditionBuilder.Append("\"");
            whereConditionBuilder.Append(model.Value);
            whereConditionBuilder.Append("\"");
            return this;
        }


        private LuceneBuilder<T> Add(string syntax)
        {
            whereConditionBuilder.Append(" ");
            whereConditionBuilder.Append(syntax);
            whereConditionBuilder.Append(" ");
            return this;
        }

        private LuceneBuilder<T> Add(Action<LuceneBuilder<T>> action)
        {
            whereConditionBuilder.Append("(");
            action(this);
            whereConditionBuilder.Append(")");
            return this;
        }

        

        public LuceneBuilder<T> Create(object Object)
        {
            this.modelBuilder = Object;
            return this;
        }

        public LuceneBuilder<T> Pharase(Action<LuceneBuilder<T>> action)
        {
            return Add(action);
        }

    }
}
