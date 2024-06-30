using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGAPI.Entities
{
    public class ReviewEnt
    {
        public long Review_id { get; set; }
        public decimal Rating { get; set; }
        public string Review_content { get; set; }
        public DateTime Review_date { get; set; }
        public bool Is_visible { get; set; }
        public long Restaurant_id { get; set; }
        public long User_id { get; set; }
        public string User { get; set; }
        public string UserImg { get; set; }
        public string Restaurant { get; set; }
    }
}