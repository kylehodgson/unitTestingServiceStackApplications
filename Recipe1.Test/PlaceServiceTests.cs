using System.Linq;
using NUnit.Framework;
using PlacesToVisit.ServiceInterface;
using PlacesToVisit.ServiceModel;
using PlacesToVisit.ServiceModel.Types;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Testing;

namespace Recipe1.Test
{
    [TestFixture]
    public class PlaceServiceTests
    {
        ServiceStackHost appHost;

        [TestFixtureSetUp]
        public void FixtureInit()
        {
            appHost = new BasicAppHost(typeof(PlaceService).Assembly)
            {
                ConfigureContainer = container =>
                {
                    container.Register<IDbConnectionFactory>(c =>
                        new OrmLiteConnectionFactory(
                            ":memory:", SqliteDialect.Provider));
                }
            }.Init();
        }

        [SetUp]
        public void TestInit()
        {
            using (var db = appHost.Container
                .Resolve<IDbConnectionFactory>().Open())
            {
                db.DropAndCreateTable<Place>();
                db.InsertAll(PlaceSeedData.GetSeedPlaces());
            }
        }

        [Test]
        public void ATestWillHaveTestFixtureSetupAndSetupFirst()
        {
            Assert.AreNotEqual(true,false);
        }

        [Test]
        public void ShouldAddNewPlaces()
        {
            var placeService = appHost.TryResolve<PlaceService>();
            var originalResponse = (AllPlacesToVisitResponse)placeService
                .Get(new AllPlacesToVisit());
            var startingCount = originalResponse
                .Places
                .Count;
            var melbourne = new CreatePlaceToVisit
            {
                Name = "Melbourne",
                Description = "A nice city to holiday"
            };

            placeService.Post(melbourne);
            var newResponse =
                (AllPlacesToVisitResponse)placeService
                    .Get(new AllPlacesToVisit());
            var newCount = newResponse
                .Places
                .Count;

            Assert.That(newCount == startingCount + 1);

            var newPlace = (PlaceToVisitResponse)placeService.Get(new PlaceToVisit
            {
                Id = startingCount + 1
            });
            Assert.That(newPlace.Place.Name == melbourne.Name);
        }

        [Test]
        public void ShouldUpdateExistingPlaces()
        {
            var placeService = appHost.Resolve<PlaceService>();
            var startingPlaces = ((AllPlacesToVisitResponse)placeService
                .Get(new AllPlacesToVisit()))
                .Places;
            var startingCount = startingPlaces.Count;

            var canberra = startingPlaces
                .First(c => c.Name.Equals("Canberra"));

            const string canberrasNewName = "Canberra, ACT";
            canberra.Name = canberrasNewName;
            placeService.Put(canberra.ConvertTo<UpdatePlaceToVisit>());

            var updatedPlaces = (AllPlacesToVisitResponse)placeService
                .Get(new AllPlacesToVisit());
            var updatedCanberra = updatedPlaces
                .Places
                .First(p => p.Id.Equals(canberra.Id));
            var updatedCount = updatedPlaces.Places.Count;

            Assert.That(updatedCanberra.Name == canberrasNewName);
            Assert.That(updatedCount == startingCount);
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            appHost.Dispose();
        }
    }
}
