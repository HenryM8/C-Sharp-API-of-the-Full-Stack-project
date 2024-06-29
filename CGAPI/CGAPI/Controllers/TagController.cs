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
    public class TagController : ApiController
    {
        [HttpPost]
        [Route("api/AddTag")]
        public int AddTag(TagEnt entidad)
        {
            using (var bd = new dbEntities())
            {
                Tags tabla = new Tags();
                tabla.tag_name = entidad.Tag_name;
                bd.Tags.Add(tabla);
                return bd.SaveChanges();
            }
        }

        [HttpGet]
        [Route("api/GetTags")]
        public List<TagEnt> GetTags()
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Tags
                             select x).ToList();
                List<TagEnt> resp = new List<TagEnt>();

                if (datos.Count > 0)
                {
                    foreach (var item in datos)
                    {
                        resp.Add(new TagEnt
                        {
                            Tag_id = item.tag_id,
                            Tag_name = item.tag_name,
                        });
                    }
                }

                return resp;
            }
        }

        [HttpGet]
        [Route("api/GetTag")]
        public TagEnt GetTag(long q)
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Tags
                             where x.tag_id == q
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    TagEnt resp = new TagEnt();
                    resp.Tag_id = datos.tag_id;
                    resp.Tag_name = datos.tag_name;
                    return resp;
                }

                return null;
            }
        }

        [HttpPut]
        [Route("api/ValidateTagData")]
        public int ValidateTagData(TagEnt entidad)
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Tags
                             where x.tag_id == entidad.Tag_id
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    datos.tag_name = entidad.Tag_name;
                    return bd.SaveChanges();
                }
                return 0;
            }
        }
    }
}
