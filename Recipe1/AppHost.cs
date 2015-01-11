using Funq;
using PlacesToVisit.ServiceInterface;
using PlacesToVisit.ServiceModel.Types;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Validation;

namespace Recipe1
{
    public class AppHost : AppHostBase
    {
        public AppHost()
            : base("Recipe 1 - Unit testing services and other components", 
            typeof(PlaceService).Assembly)
        {

        }

        public override void Configure(Container container)
        {
            var dbFactory = new OrmLiteConnectionFactory(
                "~/App_Data/db.sqlite".MapHostAbsolutePath(),
                SqliteDialect.Provider);
            container.Register<IDbConnectionFactory>(dbFactory);

            using (var db = dbFactory.OpenDbConnection())
            {
                db.DropAndCreateTable<User>();
                db.DropAndCreateTable<Place>();
                db.DropAndCreateTable<UserPlace>();
            }

            var userRep = new OrmLiteAuthRepository(dbFactory);
            container.Register<IUserAuthRepository>(userRep);

            Plugins.Add(new PostmanFeature());
            Plugins.Add(new CorsFeature());
            Plugins.Add(new ValidationFeature());
            Plugins.Add(
                new AuthFeature(() =>
                new AuthUserSession(),
                new IAuthProvider[] {
                new CredentialsAuthProvider(), 
            }));

            userRep.DropAndReCreateTables();
            CreateUsers(dbFactory, userRep);
            SeedPlaces(dbFactory);
        }

        private void CreateUsers(IDbConnectionFactory dbConnectionFactory, OrmLiteAuthRepository userRep)
        {
            var userAuth1 = userRep.CreateUserAuth(new UserAuth
            {
                Email = "darren.reid@reidsoninsdustries.net",
                DisplayName = "Darren",
                UserName = "darrenreid",
                FirstName = "Darren",
                LastName = "Reid",
                Roles = { RoleNames.Admin }

            }, "abc123");

            var user1 = userAuth1.ConvertTo<User>();
            CreateUserIfDoesntExist(dbConnectionFactory, user1);

            var userAuth2 = userRep.CreateUserAuth(new UserAuth
            {
                Email = "kyle.hodgson@reidsoninsdustries.net",
                DisplayName = "Kyle",
                UserName = "kylehodgson",
                FirstName = "Kyle",
                LastName = "Hodgson",
                Roles = { RoleNames.Admin }

            }, "123abc");

            var user2 = userAuth2.ConvertTo<User>();
            CreateUserIfDoesntExist(dbConnectionFactory, user2);
        }

        private void SeedPlaces(IDbConnectionFactory repository)
        {
            using (var db = repository.OpenDbConnection())
            {
                db.Insert(new Place
                {
                    Name = "Canberra",
                    Description = "Capital city of Australia"
                });

                db.Insert(new Place
                {
                    Name = "Toronto",
                    Description = "Provincial capital of Ontario"
                });

                db.Insert(new Place
                {
                    Name = "Auckland, New Zealand",
                    Description = "A city in the north island"
                });
            }
        }

        private void CreateUserIfDoesntExist(IDbConnectionFactory dbConnectionFactory, User user)
        {
            User existingUser = null;
            using (var db = dbConnectionFactory.OpenDbConnection())
            {
                existingUser = db.Single<User>(x => x.UserName == user.UserName);
            }
            if (existingUser != null)
            {
                return;
            }
            using (var db = dbConnectionFactory.OpenDbConnection())
            {
                db.Insert(user);
            }
        }
    }
}