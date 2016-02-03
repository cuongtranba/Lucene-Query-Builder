using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LuceneQueryBuilder
{
    public static class Extention
    {
        public static bool IsEmpty<T>(this Stack<T> stack)
        {
            if (stack.Count == 0)
            {
                return true;
            }
            return false;
        }

        public static string GetPropertyNameAndValue<TProp>(this Expression<Func<TProp>> expression)
        {
            var body = expression.Body as MemberExpression;

            TProp value = expression.Compile()();

            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return string.Empty;
            }
            return string.Format("{0}:\"{1}\"", body.Member.Name, value.ToString());
        }
    }
}
