using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGAPI.Entities
{
    public class UserEnt
    {
        public long User_id { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Url_image { get; set; }
        public long Rol_id { get; set; }
        public DateTime Register_date { get; set; }
        public string Rol_name { get; set; }
        public int Reviews { get; set; }
        public bool Use_recovery_password { get; set; }
        public DateTime Date_recovery { get; set; }
        public string Token { get; set; }
        public string New_password { get; set; }
        public string Confirm_new_password { get; set; }
    }
}