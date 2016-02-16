# Lucene Query Builder [![Build Status](https://travis-ci.org/cuongtranba/Lucene-Query-Builder.svg?branch=master)](https://travis-ci.org/cuongtranba/Lucene-Query-Builder) [![Build status](https://ci.appveyor.com/api/projects/status/q57j8mqtxyyj2p8q?svg=true)](https://ci.appveyor.com/project/herrylove72/lucene-query-builder-vxsik)



This library help reduce magic string when we create query clause by lucene
# How to get started
When first building the solution there will be external libraries that are missing since GitHub doesn't include DLLs. The best way to get these libraries into your solution is to use NuGet. However, since the project is now using NuGet Package Restore, manually installing the packages may not be necessary. Below lists the libraries that are required if manual installing is needed.

The libraries that are needed to build are the following:

NUnit
# Examples

Here's a couple of simple examples to give an idea of how LuceneQueryBuilder works:

### Testing if we create query base on object model property

``` City:"HCM" AND Country:"VietNam" OR Line1:"Phuoc binh" ```
```
        [Test]
        public void LuceneSymbolSyntax()
        {
            var value = luceneBuilder
                .WhereEquals(() => address.City)
                .And()
                .WhereEquals(() => address.Country)
                .Or()
                .WhereEquals((() => address.Line1))
                .ToString;
            StringAssert.AreEqualIgnoringCase("City:\"HCM\" AND Country:\"VietNam\" OR Line1:\"Phuoc binh\"", value);
        }
```
#####Start with ``` City:HCM*  ```

```
        [Test]
        public void StartWithTest()
        {
            var actual = LuceneBuilder.Create().StartWith(() => address.City).ToString();
            StringAssert.AreEqualIgnoringCase("City:HCM*", actual);
        }
```
#####End With ``` City:*HCM  ```

```
        [Test]
        public void EndWithTest()
        {
            var actual = LuceneBuilder.Create().EndWith(() => address.City).ToString();
            StringAssert.AreEqualIgnoringCase("City:*HCM", actual);
        }
```
#####In pharase ``` (PostalCode:"99999" AND FirstName:"Cuong") AND Country:"VietNam" OR Line1:"Phuoc binh" AND (Line2:"Quan9" AND PostalCode:"99999") ```

```
        [Test]
        public void PharaseAtBegin()
        {
            var actual = LuceneBuilder.Create()
                .Pharase(builder => builder.WhereEquals(() => address.PostalCode).And().WhereEquals(() => Customer.FirstName))
                .And()
                .WhereEquals(() => address.Country)
                .Or()
                .WhereEquals(() => address.Line1)
                .And()
                .Pharase(a => a.WhereEquals(() => address.Line2).And().WhereEquals(() => address.PostalCode)).ToString();

            StringAssert.AreEqualIgnoringCase("(PostalCode:\"99999\" AND FirstName:\"Cuong\") AND Country:\"VietNam\" OR Line1:\"Phuoc binh\" AND (Line2:\"Quan9\" AND PostalCode:\"99999\")", actual);
        }
```
#####Work well if have empty or null value 

```
        [Test]
        public void ShouldNotAppendValueIfNullAndBeforeSymbolSyntax()
        {
            var emptyAddress = new Address()
            {
                PostalCode = "123",
                City = null,
                Line1 = "duong7,phuocbinh"
            };
            var expected = LuceneBuilder.Create()
                .WhereEquals(() => emptyAddress.PostalCode)
                .Or()
                .WhereEquals(() => emptyAddress.City)
                .And()
                .WhereEquals(() => emptyAddress.Line1).ToString();
            StringAssert.AreEqualIgnoringCase("PostalCode:\"123\" AND Line1:\"duong7,phuocbinh\"", expected);
        }
```
### also many other functions ......
