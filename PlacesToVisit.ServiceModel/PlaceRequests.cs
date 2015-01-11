using System.Security.Cryptography;
using PlacesToVisit.ServiceModel.Types;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlacesToVisit.ServiceModel
{
    [Route("/places/{Id}", Verbs = "GET")]
    public class PlaceToVisit
    {
        public long Id { get; set; }
    }

    [Route("/places", Verbs = "GET")]
    public class AllPlacesToVisit
    {

    }

    [Route("/places", Verbs = "POST")]
    public class CreatePlaceToVisit
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    [Route("/places/{Id}", Verbs = "PUT")]
    public class UpdatePlaceToVisit
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    [Route("/places/{Id}", Verbs = "DELETE")]
    public class DeletePlaceToVisit
    {
        public long Id { get; set; }
    }

    public class PlaceToVisitResponse
    {
        public Place Place { get; set; }
    }

    public class AllPlacesToVisitResponse
    {
        public List<Place> Places { get; set; }
    }

    public class CreatePlaceToVisitResponse
    {
        public Place Place { get; set; }
    }

    public class UpdatePlaceToVisitResponse
    {
        public Place Place { get; set; }
    }
}
