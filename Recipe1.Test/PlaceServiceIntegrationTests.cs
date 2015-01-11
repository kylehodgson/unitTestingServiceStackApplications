using System;
using System.Net;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;
using PlacesToVisit.ServiceInterface;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using PlacesToVisit.ServiceInterface.DataRepository;
using PlacesToVisit.ServiceModel;

namespace Recipe2.Test
{
    [TestFixture]
    public class PlaceServiceIntegrationTests
    {
        ServiceStackHost appHost;
        PlaceService targetPlaceService;

        [TestFixtureSetUp]
        public void SetUpTest()
        {
            appHost = new BasicAppHost(typeof(PlaceService).Assembly)
            {
                ConfigureContainer = container =>
                {
                    container.Register<IDbConnectionFactory>(c =>
                        new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider));
                    container.RegisterAutoWiredAs<PlacesToVisitRepository, IPlacesToVisitRepository>();
                }
            }.Init();

            targetPlaceService = appHost.TryResolve<PlaceService>();
        }

        [SetUp]
        public void Init()
        {
            using (var db = appHost.Container.Resolve<IDbConnectionFactory>().Open())
            {
                db.DropAndCreateTable<Place>();
                db.InsertAll(PlaceSeedData.GetSeedPlaces());
            }
        }

        [Test]
        public void ShouldAddNewPlaces()
        {
            var startingCount = targetPlaceService.Get(new AllPlacesToVisitRequest()).Places.Count;
            var melbourne = new CreatePlaceToVisit
            {
                Name = "Melbourne",
                Description = "A nice city to holiday"
            };

            targetPlaceService.Post(melbourne);
            var newCount = targetPlaceService.Get(new AllPlacesToVisitRequest()).Places.Count;

            Assert.That(newCount == startingCount + 1);

            var newPlace = targetPlaceService.Get(new PlaceToVisitRequest
            {
                Id = startingCount + 1
            });
            Assert.That(newPlace.Place.Name == melbourne.Name);
        }

        [Test]
        public void ShouldUpdateExistingPlaces()
        {
            var startingPlaces = targetPlaceService.Get(new AllPlacesToVisitRequest()).Places;
            var startingCount = startingPlaces.Count;

            var canberra = startingPlaces.SafeWhere(c => c.Name.Equals("Canberra")).FirstNonDefault();

            const string canberrasNewName = "Canberra, ACT";
            canberra.Name = canberrasNewName;
            targetPlaceService.Put(canberra.ConvertTo<UpdatePlaceToVisit>());

            var updatedCanberra = targetPlaceService.Get(new AllPlacesToVisitRequest())
                .Places.SafeWhere(p => p.Id.Equals(canberra.Id)).FirstNonDefault();

            Assert.That(updatedCanberra.Name == canberrasNewName);
            Assert.That(startingPlaces.Count == startingCount);
        }

        [TestFixtureTearDown]
        public void Teardown()
        {
            appHost.Dispose();
        }
    }
}
