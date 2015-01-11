using PlacesToVisit.ServiceModel;
using PlacesToVisit.ServiceModel.Types;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;

namespace PlacesToVisit.ServiceInterface
{
    public class PlaceService : Service
    {
        public object Get(PlaceToVisit request)
        {
            if (!Db.Exists<Place>(x => x.Id == request.Id))
            {
                throw HttpError.NotFound("Place not found");
            }
            return new PlaceToVisitResponse
            {
                Place = Db.SingleById<Place>(request.Id)
            };
        }

        public object Get(AllPlacesToVisit request)
        {
            return new AllPlacesToVisitResponse
            {
                Places = Db.Select<Place>().ToList()
            };
        }

        public object Post(CreatePlaceToVisit request)
        {
            var place = request.ConvertTo<Place>();
            Db.Insert(place);
            place.Id = Db.LastInsertId();
            return new PlaceToVisitResponse
            {
                Place = place
            };
        }

        public object Put(UpdatePlaceToVisit request)
        {
            if (!Db.Exists<Place>(x => x.Id == request.Id))
            {
                throw HttpError.NotFound("Place not found");
            }

            var place = request.ConvertTo<Place>();
            Db.Update<Place>(place);
            return new PlaceToVisitResponse
            {
                Place = place
            };
        }
    }
}
