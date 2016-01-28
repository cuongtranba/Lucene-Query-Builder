# Lucene-Query-Builder
This library help reduce magic string when we create query clause by lucene
# How to get started
When first building the solution there will be external libraries that are missing since GitHub doesn't include DLLs. The best way to get these libraries into your solution is to use NuGet. However, since the project is now using NuGet Package Restore, manually installing the packages may not be necessary. Below lists the libraries that are required if manual installing is needed.

The libraries that are needed to build are the following:

NUnit
#Examples

Here's a couple of simple examples to give an idea of how LuceneQueryBuilder works:

###Testing if we create query base on object model property

```
        [Test]
        public void LuceneSymbolSyntax()
        {
            var value = luceneBuilder
                .HaveValue(() => address.City)
                .And()
                .HaveValue(() => address.Country)
                .Or()
                .HaveValue((() => address.Line1))
                .ToString;
            StringAssert.AreEqualIgnoringCase("City:\"HCM\" AND Country:\"VietNam\" OR Line1:\"Phuoc binh\"", value);
        }
```


