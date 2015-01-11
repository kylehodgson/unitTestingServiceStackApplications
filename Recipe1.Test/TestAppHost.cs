using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Funq;
using PlacesToVisit.ServiceInterface;
using PlacesToVisit.ServiceModel;
using ServiceStack;
using ServiceStack.Web;

namespace Recipe1.Test
{
    public class TestAppHost : AppSelfHostBase
    {
        public Action<Container> ConfigureContainer { get; set;  }

        public List<Action<IRequest, IResponse, object>> ResponseFilters { get; set;  }

        public TestAppHost()
            : base("Test Container for PlaceToVisit Service",
                typeof(PlaceService).Assembly)
        { }

        public override void Configure(Container container)
        {
            if (ConfigureContainer != null) 
                this.ConfigureContainer(container);

            if (ResponseFilters != null)
                this.GlobalResponseFilters.AddRange(ResponseFilters);
        }
    }
}
