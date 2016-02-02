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
                    queryText.Append(value);
                }
                return queryText.ToString();
            }
        }

        internal StringBuilder queryText = new StringBuilder();
        private Stack<string> stack = new Stack<string>();
        private bool IsEmpty { get; set; }

        public LuceneBuilder()
        {
            IsEmpty = false;
            luceneLogicSymboy = new LuceneLogicSymbol(this);
            luceneField = new LuceneField(this);
        }

        public static LuceneBuilder Create()
        {
            return new LuceneBuilder();
        }

        public LuceneLogicSymbol WhereEquals<TProp>(Expression<Func<TProp>> expression)
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
            if (stack.IsEmpty() == false)
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
            if (stack.IsEmpty())
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
            if (stack.IsEmpty() == false)
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
