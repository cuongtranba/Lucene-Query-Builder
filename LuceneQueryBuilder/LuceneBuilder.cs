using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LuceneQueryBuilder
{
    public class LuceneBuilder
    {
        internal LuceneLogicSymbol luceneLogicSymboy;
        internal LuceneField luceneField;

        internal new string ToString
        {
            get
            {
                var listValue = stack.ToList();
                listValue.Reverse();
                foreach (var value in listValue)
                {
                    whereConditionBuilder.Append(value);
                }
                return whereConditionBuilder.ToString();
            }
        }

        internal StringBuilder whereConditionBuilder = new StringBuilder();
        private Stack<string> stack = new Stack<string>();
        private bool IsEmpty { get; set; }

        public LuceneBuilder()
        {
            IsEmpty = false;
            luceneLogicSymboy = new LuceneLogicSymbol(this);
            luceneField = new LuceneField(this);
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
            if (stack.Count > 0)
            {
                var valueBefore = stack.Peek().Trim();
                if (SymbolSyntax.SymbolSyntaxtList.Contains(valueBefore))
                {
                    stack.Pop();
                }
                else if (valueBefore == SymbolSyntax.BeginPharase)
                {
                    return;
                }
            }
            if (stack.Count == 0)
            {
                return;
            }
            stack.Push(String.Format(" {0} ", syntax));
        }

        internal void Add(Property property)
        {
            if (property != null)
            {
                stack.Push(String.Format("{0}:\"{1}\"", property.FieldName, property.Value));
            }
        }

        internal void Add(Action<LuceneBuilder> action)
        {
            stack.Push("(");
            action(this);
            if (stack.Count > 0)
            {
                var valueBefore = stack.Peek().Trim();
                if (SymbolSyntax.SymbolSyntaxtList.Contains(valueBefore))
                {
                    stack.Pop();
                }
                else if (valueBefore == SymbolSyntax.BeginPharase)
                {
                    stack.Pop();
                    return;
                }
            }
            stack.Push(")");
        }
    }

}
