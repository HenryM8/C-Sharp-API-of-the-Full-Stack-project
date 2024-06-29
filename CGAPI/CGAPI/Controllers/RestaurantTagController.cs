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
    public class RestaurantTagController : ApiController
    {
        [HttpPost]
        [Route("api/AddRestaurantTag")]
        public int AddRestaurantTag(RestaurantTagEnt entidad)
        {
            using (var bd = new dbEntities())
            {
                Restaurant_tags tabla = new Restaurant_tags();
                tabla.restaurant_id = entidad.Restaurant_id;
                tabla.tag_id = entidad.Tag_id;
                bd.Restaurant_tags.Add(tabla);
                return bd.SaveChanges();
            }
        }

        [HttpGet]
        [Route("api/GetRestaurantTags")]
        public List<RestaurantTagEnt> GetRestaurantTags()
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Restaurant_tags
                             join r in bd.Restaurants on x.restaurant_id equals r.restaurant_id
                             join t in bd.Tags on x.tag_id equals t.tag_id
                             select new
                             {
                                 x.restaurant_tag_id,
                                 x.tag_id,
                                 x.restaurant_id,
                                 r.restaurant_name,
                                 t.tag_name,
                             }).ToList();

                List<RestaurantTagEnt> resp = new List<RestaurantTagEnt>();

                if (datos.Count > 0)
                {
                    foreach (var item in datos)
                    {
                        resp.Add(new RestaurantTagEnt
                        {
                            RestaurantTag_id = item.restaurant_tag_id,
                            Restaurant_id = item.restaurant_id,
                            Tag_id = item.tag_id,
                            Tag_name = item.tag_name,
                            Restaurant_name = item.restaurant_name,
                        });
                    }
                }

                return resp;
            }
        }

        [HttpGet]
        [Route("api/GetRestaurantTag")]
        public RestaurantTagEnt GetRestaurantTag(long q)
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Restaurant_tags
                             where x.restaurant_tag_id == q
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    RestaurantTagEnt resp = new RestaurantTagEnt();
                    resp.RestaurantTag_id = datos.restaurant_tag_id;
                    resp.Restaurant_id = datos.restaurant_id;
                    resp.Tag_id = datos.tag_id;
                    return resp;
                }

                return null;
            }
        }

        [HttpGet]
        [Route("api/GetRestaurantTagsByRestaurantId")]
        public List<RestaurantTagEnt> GetRestaurantTagsByRestaurantId(long q)
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Restaurant_tags
                             join r in bd.Restaurants on x.restaurant_id equals r.restaurant_id
                             join t in bd.Tags on x.tag_id equals t.tag_id
                             where x.restaurant_id == q
                             select new
                             {
                                 x.tag_id,
                                 x.restaurant_id,
                                 r.restaurant_name,
                                 t.tag_name,
                             }).ToList();

                List<RestaurantTagEnt> resp = new List<RestaurantTagEnt>();

                if (datos.Count > 0)
                {
                    foreach (var item in datos)
                    {
                        resp.Add(new RestaurantTagEnt
                        {
                            Restaurant_id = item.restaurant_id,
                            Tag_id = item.tag_id,
                            Tag_name = item.tag_name,
                            Restaurant_name = item.restaurant_name,
                        });
                    }
                }

                return resp;
            }
        }

        [HttpPut]
        [Route("api/ValidateRestaurantTagData")]
        public int ValidateRestaurantTagData(RestaurantTagEnt entidad)
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Restaurant_tags
                             where x.restaurant_tag_id == entidad.RestaurantTag_id
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    datos.restaurant_id = entidad.Restaurant_id;
                    datos.tag_id = entidad.Tag_id;
                    return bd.SaveChanges();
                }
                return 0;
            }
        }

    }
}
