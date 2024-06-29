using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGAPI.Entities
{
    public class RestaurantTagEnt
    {
        public long RestaurantTag_id { get; set; }
        public long Tag_id { get; set; }
        public long Restaurant_id { get; set; }
        public string Restaurant_name { get; set; }
        public string Tag_name { get; set; }
    }
}