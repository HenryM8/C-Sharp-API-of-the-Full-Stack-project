using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGAPI.Entities
{
    public class RestaurantEnt
    {
        public string Restaurant_name { get; set; }
        public string Address { get; set; }
        public string Phone_number { get; set; }
        public string Website { get; set; }
        public string Url_image { get; set; }
        public bool Is_visible { get; set; }
        public string Description { get; set; }
        public string Location_map { get; set; }
        public long Restaurant_id { get; set; }
        public long Reviews_count { get; set; }
        public decimal Stars_count { get; set; }
    }
}