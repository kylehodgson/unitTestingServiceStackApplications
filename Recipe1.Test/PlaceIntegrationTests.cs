using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PlacesToVisit.ServiceInterface;
using PlacesToVisit.ServiceModel;
using PlacesToVisit.ServiceModel.Types;
using Recipe_1;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Web;

namespace Recipe1.Test
{
    [TestFixture]
    public class PlaceIntegrationTests
    {
        const string URLBase = "http://localhost:28193/";
        ServiceStackHost appHost;
        
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            var responseFilters = new List<Action<IRequest, IResponse, object>> 
                                      {AppHost.ETagResponseFilter};

            appHost = new TestAppHost
            {
                ConfigureContainer = container =>
                {
                    container.Register<IDbConnectionFactory>(
                        new OrmLiteConnectionFactory(":memory:",
                                                     SqliteDialect.Provider));
                },
                ResponseFilters = responseFilters
            }
            .Init()
            .Start(URLBase);
        }

        [SetUp]
        public void TestSetUp()
        {
            var dbFactory = appHost.Resolve<IDbConnectionFactory>();
            using (var db = dbFactory.OpenDbConnection())
            {
                db.DropAndCreateTable<Place>();
                db.InsertAll(PlaceSeedData.GetSeedPlaces());
            }
        }

        [Test]
        public void ShouldAddNewPlaces()
        {
            var client = new JsonServiceClient(URLBase);
            client.ResponseFilter = resp => 
                                    Assert.IsNotNull(resp.Headers[HttpHeaders.ETag]);

            var testPlace = new CreatePlaceToVisit
            {
                Name = "Halifax",
                Description = "Very friendly Atlantic city"
            };

            var response = client.Post<CreatePlaceToVisitResponse>(testPlace);
            Assert.AreEqual(testPlace.Name,response.Place.Name);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            appHost.Dispose();
        }
    }
}
