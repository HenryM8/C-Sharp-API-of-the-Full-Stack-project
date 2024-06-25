using CGAPI.Entities;
using CGAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CGAPI.Controllers
{
    [Authorize]
    public class RestaurantController : ApiController
    {
        [HttpPost]
        [Route("api/AddRestaurant")]
        public long RegistrarRestaurants(RestaurantEnt entidad)
        {
            using (var bd = new dbEntities())
            {
                Restaurants tabla = new Restaurants();
                tabla.restaurant_name = entidad.Restaurant_name;
                tabla.address = entidad.Address;
                tabla.phone_number = entidad.Phone_number;
                tabla.website = entidad.Website;
                tabla.url_image = entidad.Url_image;
                tabla.description = entidad.Description;
                tabla.location_map = entidad.Location_map;
                tabla.is_visible = entidad.Is_visible;
                bd.Restaurants.Add(tabla);
                bd.SaveChanges();

                return tabla.restaurant_id;
            }
        }

        [HttpGet]
        [Route("api/GetRestaurants")]
        public List<RestaurantEnt> GetRestaurants()
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Restaurants
                             join r in bd.Reviews on x.restaurant_id equals r.restaurant_id into Union
                             from r in Union.DefaultIfEmpty()
                             select new
                             {
                                 x.restaurant_id,
                                 x.restaurant_name,
                                 x.address,
                                 x.phone_number,
                                 x.website,
                                 x.url_image,
                                 x.description,
                                 x.is_visible,
                                 x.location_map,
                                 reviews_count = x.Reviews.Count(),
                                 stars_count = x.Reviews.Sum(s => s.rating) != null ? x.Reviews.Sum(s => s.rating) / x.Reviews.Count() : 0
                             }
                             ).Distinct().ToList();

                List<RestaurantEnt> resp = new List<RestaurantEnt>();

                if (datos.Count > 0)
                {
                    foreach (var item in datos)
                    {
                        resp.Add(new RestaurantEnt
                        {
                            Restaurant_id = item.restaurant_id,
                            Restaurant_name = item.restaurant_name,
                            Address = item.address,
                            Phone_number = item.phone_number,
                            Website = item.website,
                            Url_image = item.url_image,
                            Description = item.description,
                            Is_visible = (bool)item.is_visible,
                            Location_map = item.location_map,
                            Reviews_count = item.reviews_count,
                            Stars_count = item.stars_count,
                        });
                    }
                }

                return resp;
            }
        }

        [HttpGet]
        [Route("api/GetRestaurant")]
        public RestaurantEnt GetRestaurant(long q)
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Restaurants
                             join r in bd.Reviews on x.restaurant_id equals r.restaurant_id into Union
                             from r in Union.DefaultIfEmpty()
                             where x.restaurant_id == q
                             select new
                             {
                                 x.restaurant_id,
                                 x.restaurant_name,
                                 x.address,
                                 x.phone_number,
                                 x.website,
                                 x.url_image,
                                 x.description,
                                 x.is_visible,
                                 x.location_map,
                                 stars_count = x.Reviews.Sum(s => s.rating) != null ? x.Reviews.Sum(s => s.rating) / x.Reviews.Count() : 0
                             }).FirstOrDefault();

                if (datos != null)
                {
                    RestaurantEnt resp = new RestaurantEnt();
                    resp.Restaurant_id = datos.restaurant_id;
                    resp.Restaurant_name = datos.restaurant_name;
                    resp.Address = datos.address;
                    resp.Phone_number = datos.phone_number;
                    resp.Website = datos.website;
                    resp.Url_image = datos.url_image;
                    resp.Description = datos.description;
                    resp.Is_visible = (bool)datos.is_visible;
                    resp.Location_map = datos.location_map;
                    resp.Stars_count = datos.stars_count;
                    return resp;
                }

                return null;
            }
        }

        [HttpPut]
        [Route("api/ChangeRestaurantVisibility")]
        public int ChangeRestaurantVisibility(RestaurantEnt entidad)
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Restaurants
                             where x.restaurant_id == entidad.Restaurant_id
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    datos.is_visible = !datos.is_visible;
                    return bd.SaveChanges();
                }
                return 0;
            }
        }

        [HttpPut]
        [Route("api/ValidateRestaurantData")]
        public int ValidateUserData(RestaurantEnt entidad)
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Restaurants
                             where x.restaurant_id == entidad.Restaurant_id
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    datos.restaurant_name = entidad.Restaurant_name;
                    datos.address = entidad.Address;
                    datos.phone_number = entidad.Phone_number;
                    datos.description = entidad.Description;
                    datos.website = entidad.Website;
                    datos.location_map = entidad.Location_map;
                    return bd.SaveChanges();
                }
                return 0;
            }
        }

        [HttpPut]
        [Route("api/UpdateRestaurantImage")]
        public void UpdateRestaurantImage(RestaurantEnt entidad)
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Restaurants
                             where x.restaurant_id == entidad.Restaurant_id
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    datos.url_image = entidad.Url_image;
                    bd.SaveChanges();
                }
            }
        }
    }
}
