using PlacesToVisit.ServiceModel;
using PlacesToVisit.ServiceModel.Types;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;

namespace PlacesToVisit.ServiceInterface
{
    [Authenticate]
    public class UserPlaceService : Service
    {
        public object Get(AllUserPlaces request)
        {
            string userName = GetSession().UserName;
            var user = Db.Single<User>(x => x.UserName == userName);
            return new AllUserPlacesResponse
            {
                UserPlaces = Db.Select<UserPlace>(x => x.UserId == user.Id)
            };
        }

        public object Post(CreateUserPlace request)
        {
            string userName = GetSession().UserName;
            var user = Db.Single<User>(x => x.UserName == userName);
            var userPlace = request.ConvertTo<UserPlace>();
            userPlace.UserId = user.Id;
            Db.Insert(userPlace);
            userPlace.Id = Db.LastInsertId();
            return new CreateUserPlaceResponse
            {
                UserPlace = userPlace
            };
        }

        public object Put(UpdateUserPlace request)
        {
            if (!Db.Exists<UserPlace>(x => x.Id == request.Id))
            {
                throw HttpError.NotFound("Place not found");
            }
            var userPlace = Db.SingleById<UserPlace>(request.Id);
            userPlace.UserDescription = request.UserDescription;
            Db.Update(userPlace);
            return new UpdateUserPlaceResponse
            {
                UserPlace = userPlace
            };
        }
    }
}
