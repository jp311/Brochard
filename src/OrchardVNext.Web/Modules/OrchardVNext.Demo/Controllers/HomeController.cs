using System.Linq;
using Microsoft.AspNet.Mvc;
using OrchardVNext.ContentManagement;
using OrchardVNext.ContentManagement.Handlers;
using OrchardVNext.Data;
using OrchardVNext.Demo.Models;
using OrchardVNext.Test1;
using OrchardVNext.ContentManagement.Records;

namespace OrchardVNext.Demo.Controllers {
    public class HomeController : Controller {
        private readonly ITestDependency _testDependency;
        private readonly IContentStorageManager _contentStorageManager;
        private readonly IContentManager _contentManager;
        private readonly IContentIndexProvider _contentIndexProvider;

        public HomeController(ITestDependency testDependency,
            IContentStorageManager contentStorageManager,
            IContentManager contentManager,
            IContentIndexProvider contentIndexProvider) {
            _testDependency = testDependency;
            _contentStorageManager = contentStorageManager;
            _contentManager = contentManager;
            _contentIndexProvider = contentIndexProvider;
        }

        public ActionResult Index()
        {
            //var contentItem = new ContentItem
            //{
            //    VersionRecord = new ContentItemVersionRecord
            //    {
            //        ContentItemRecord = new ContentItemRecord(),
            //        Number = 1,
            //        Latest = true,
            //        Published = true
            //    }
            //};

            //contentItem.VersionRecord.ContentItemRecord.Versions.Add(contentItem.VersionRecord);

            //_contentStorageProvider.Store(contentItem);

            //var indexedRecordIds = _contentIndexProvider.GetByFilter(x => x.Id == 1);

            //var retrievedRecord = _contentStorageProvider.Get(contentItem.Id);

            //var indexedRetrievedRecords = _contentStorageProvider.GetMany(x => x.Id == 1);

            var contentItemA = _contentManager.New("Foo");
            contentItemA.As<TestContentPartA>().Line = "Orchard VNext Non Content Manager Direction";

            //_contentStorageManager.Store(contentItemA);

            var count = _contentStorageManager.Query<ContentItemRecord>().Records.Count();

            var result = _contentStorageManager
                .Query<ContentItemRecord, WhereLineIsNotNullIndex>();



                //    map => ci.As<TestContentPartA>().Line != "",
                //    sort => sort.OrderBy(sortOrder => sortOrder.Id))
                //.Reduce(reduce => reduce.Line == "foo");



            var contentItem = _contentManager.New("Foo");
            contentItem.As<TestContentPartA>().Line = "Orchard VNext";
            _contentManager.Create(contentItem);


            var retrieveContentItem = _contentManager.Get(contentItem.Id);
            var lineToSay = retrieveContentItem.As<TestContentPartA>().Line;


            return View("Index", _testDependency.SayHi(lineToSay));
        }
    }

    public class TestContentPartAHandler : ContentHandlerBase {
        public override void Activating(ActivatingContentContext context) {
            context.Builder.Weld<TestContentPartA>();
        }
    }
}
