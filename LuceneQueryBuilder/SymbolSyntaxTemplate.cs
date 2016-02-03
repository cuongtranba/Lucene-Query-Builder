using System.Collections.Generic;

namespace LuceneQueryBuilder
{
    public static class SymbolSyntaxTemplate
    {
        public const string And = " AND ";
        public const string Or = " OR ";
        public const string Not = "-{0}:{1}";
        public const string StartWith = "{0}:{1}*";
        public const string EndWith = "{0}:*{1}";
        public const string BeginPharase = "(";
        public const string EndPharase = ")";
        public const string Pharase = "({})";
        public static string WhereEquals = "{0}:\"{1}\"";
        public static List<string> SymbolSyntaxtList=new List<string>()
        {
            And,EndWith,Or,Not,StartWith
        };

    }
}
