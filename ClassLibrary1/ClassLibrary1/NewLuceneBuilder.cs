using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LuceneQueryBuilder
{
    public class NewLuceneBuilder
    {
        public new string ToString { get { return whereConditionBuilder.ToString(); } }
        private StringBuilder whereConditionBuilder = new StringBuilder();

        public NewLuceneBuilder HaveValue<T>(Expression<Func<T>> e)
        {
            var member = (MemberExpression)e.Body;
            whereConditionBuilder.Append(member.Member.Name);
            T value = e.Compile()();
            whereConditionBuilder.Append(":");
            whereConditionBuilder.Append(value.ToString());
            return this;
        }
    }
}
