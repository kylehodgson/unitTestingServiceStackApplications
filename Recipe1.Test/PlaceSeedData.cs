using System.Collections.Generic;
using PlacesToVisit.ServiceModel.Types;

namespace Recipe1.Test
{
    public class PlaceSeedData
    {
        public static List<Place> GetSeedPlaces()
        {
            var places = new List<Place>();
            places.Add(new Place
            {
                Name = "Canberra",
                Description = "Capital city of Australia"
            });
            places.Add(new Place
            {
                Name = "Sydney",
                Description = "Largest city in Australia"
            });

            places.Add(new Place
            {
                Name = "Ottawa",
                Description = "Capital city of Canada"
            });

            places.Add(new Place
            {
                Name = "Toronto",
                Description = "Most populated city in Canada"
            });

            return places;
        }
    }
}
