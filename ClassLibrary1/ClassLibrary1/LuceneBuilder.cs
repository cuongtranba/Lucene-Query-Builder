using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LuceneQueryBuilder
{
    public class LuceneBuilder<T> where T: class 
    {
        public new string ToString { get { return whereConditionBuilder.ToString(); } }

        private StringBuilder whereConditionBuilder=new StringBuilder();

        public LuceneBuilder<T> HaveValue<TProp>(Expression<Func<T, TProp>> expression)
        {
            var body = expression.Body as MemberExpression;

            if (body == null)
            {
                throw new ArgumentException("'expression' should be a member expression");
            }

            var propertyInfo = (PropertyInfo)body.Member;

            var propertyType = propertyInfo.PropertyType;
            var propertyName = propertyInfo.Name;
            var propertyValue = expression.Compile();
            whereConditionBuilder.Append(propertyName);
            whereConditionBuilder.Append(":");
            whereConditionBuilder.Append(propertyValue);
            return this;
        }

    }
}
