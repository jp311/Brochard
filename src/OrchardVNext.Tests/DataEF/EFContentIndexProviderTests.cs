//using OrchardVNext.Data;
//using OrchardVNext.Data.EF;
//using OrchardVNext.ContentManagement.Records;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Collections.Concurrent;
//using Xunit;
//using Moq;

//namespace OrchardVNext.Tests.DataEF {
//    public class EFContentIndexProviderTests {
//        [Fact]
//        public void ShouldStoreDocument() {
//            FakeDataContext context = new FakeDataContext();

//            IContentIndexProvider provider = new EFContentIndexProvider(
//                context, null);

//            var document = new DocumentRecord() { Id = 1 };

//            provider.Store(document.Id, document);

//            Assert.Equal(1, context.Data.Count);
//        }

//        [Fact]
//        public void ShouldReturnObjectOnQuery() {
//            FakeDataContext context = new FakeDataContext();

//            IContentIndexProvider provider = new EFContentIndexProvider(
//                context, null);

//            var document = new DocumentRecord() { Id = 1 };

//            provider.Store(document.Id, document);

//            var queryDocument = provider.Query<DocumentRecord>(q => q.Id == 1);

//            Assert.Same(document, queryDocument);
//        }

//    }

//}
