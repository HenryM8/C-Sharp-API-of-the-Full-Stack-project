using CGAPI.Entities;
using CGAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CGAPI.Controllers
{
    [Authorize]
    public class RolController : ApiController
    {
        [HttpPost]
        [Route("api/AddRol")]
        public int AddRol(RolEnt entidad)
        {
            using (var bd = new dbEntities())
            {
                Roles tabla = new Roles();
                tabla.rol_name = entidad.Rol_name;
                bd.Roles.Add(tabla);
                return bd.SaveChanges();
            }
        }

        [HttpGet]
        [Route("api/GetRoles")]
        public List<RolEnt> GetRoles()
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Roles
                             select x).ToList();
                List<RolEnt> resp = new List<RolEnt>();

                if (datos.Count > 0)
                {
                    foreach (var item in datos)
                    {
                        resp.Add(new RolEnt
                        {
                            Rol_id = item.rol_id,
                            Rol_name = item.rol_name,
                        });
                    }
                }

                return resp;
            }
        }

        [HttpGet]
        [Route("api/GetRol")]
        public RolEnt GetRol(long q)
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Roles
                             where x.rol_id == q
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    RolEnt resp = new RolEnt();
                    resp.Rol_id = datos.rol_id;
                    resp.Rol_name = datos.rol_name;
                    return resp;
                }

                return null;
            }
        }

        [HttpPut]
        [Route("api/ValidateRolData")]
        public int ValidateRolData(RolEnt entidad)
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Roles
                             where x.rol_id == entidad.Rol_id
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    datos.rol_name = entidad.Rol_name;
                    return bd.SaveChanges();
                }
                return 0;
            }
        }
    }
}
