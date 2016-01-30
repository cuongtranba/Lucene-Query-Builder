namespace LuceneQueryBuilder
{
    public class LuceneLogicSymbol
    {
        private LuceneBuilder luceneBuilder;

        public LuceneLogicSymbol(LuceneBuilder luceneBuilder)
        {
            this.luceneBuilder = luceneBuilder;
        }

        public LuceneField And()
        {
            luceneBuilder.Add(SymbolSyntax.And);
            return luceneBuilder.luceneField;
        }

        public LuceneField Or()
        {
            luceneBuilder.Add(SymbolSyntax.Or);
            return luceneBuilder.luceneField;
        }

    

        public override string ToString()
        {
            return luceneBuilder.ToString;
        }

    }
}