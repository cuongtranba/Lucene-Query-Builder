using System.Collections.Generic;

namespace LuceneQueryBuilder
{
    public static class SymbolSyntax
    {
        public const string And = "AND";
        public const string Or = "OR";
        public const string Not = "-";
        public const string StartWith = "*";
        public const string EndWith = "*";
        public const string BeginPharase = "(";
        public const string EndPharase = ")";
        public static List<string> SymbolSyntaxtList=new List<string>()
        {
            And,EndWith,Or,Not,StartWith
        };
    }
}
