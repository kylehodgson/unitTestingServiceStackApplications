using PlacesToVisit.ServiceModel.Types;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlacesToVisit.ServiceModel
{
    [Route("/myplaces", Verbs = "GET")]
    public class AllUserPlaces
    {

    }

    [Route("/myplaces", Verbs = "POST")]
    public class CreateUserPlace
    {
        public long PlaceId { get; set; }
        public string UserDescription { get;set;}
    }

    [Route("/myplaces/{Id}", Verbs = "POST")]
    public class UpdateUserPlace
    {
        public long Id { get; set; }
        public string UserDescription { get;set;}
    }

    [Route("/myplaces/{Id}", Verbs = "DELETE")]
    public class DeleteUserPlace
    {
        public long Id { get; set; }
    }

    public class AllUserPlacesResponse
    {
        public List<UserPlace> UserPlaces { get; set; }
    }

    public class CreateUserPlaceResponse
    {
        public UserPlace UserPlace { get; set; }
    }

    public class UpdateUserPlaceResponse
    {
        public UserPlace UserPlace { get; set; }
    }
}
