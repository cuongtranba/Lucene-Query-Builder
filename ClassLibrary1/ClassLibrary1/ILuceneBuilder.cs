namespace LuceneQueryBuilder
{
    public interface ILuceneBuilder
    {
        ILuceneBuilder Setup(object inputObject);
        ILuceneBuilder Field();
        ILuceneBuilder And();
        ILuceneBuilder Or();
        ILuceneBuilder Equal();
        ILuceneBuilder Not();
        ILuceneBuilder StartWith();
        ILuceneBuilder EndWith();
        ILuceneBuilder Between();
        string Build();
    }
}
