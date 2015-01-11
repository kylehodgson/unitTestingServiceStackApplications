using PlacesToVisit.ServiceModel;
using PlacesToVisit.ServiceModel.Types;
using ServiceStack;
using ServiceStack.OrmLite;

namespace PlacesToVisit.ServiceInterface
{
    public class UserService : Service
    {
        public object Get(SearchUser request)
        {
            var response = new SearchUserResponse();

            response.Users = Db.Select<User>(x =>
                    x.UserName.Contains(request.SearchQuery) ||
                    x.DisplayName.Contains(request.SearchQuery) ||
                    x.RealName.Contains(request.SearchQuery));

            return response;
        }
    }
}
