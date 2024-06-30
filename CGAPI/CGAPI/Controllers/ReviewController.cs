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
    public class ReviewController : ApiController
    {
        [HttpPost]
        [Route("api/AddReview")]
        public int AddReview(ReviewEnt entidad)
        {
            using (var bd = new dbEntities())
            {
                Reviews tabla = new Reviews();
                tabla.rating = entidad.Rating;
                tabla.review_content = entidad.Review_content;
                tabla.review_date = entidad.Review_date;
                tabla.is_visible = entidad.Is_visible;
                tabla.restaurant_id = entidad.Restaurant_id;
                tabla.user_id = entidad.User_id;
                bd.Reviews.Add(tabla);
                return bd.SaveChanges();
            }
        }

        [HttpGet]
        [Route("api/GetReviews")]
        public List<ReviewEnt> GetReviews()
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Reviews
                             join r in bd.Restaurants on x.restaurant_id equals r.restaurant_id
                             join u in bd.Users on x.user_id equals u.user_id
                             select new
                             {
                                 x.review_id,
                                 x.rating,
                                 x.review_content,
                                 x.review_date,
                                 x.is_visible,
                                 u.username,
                                 r.restaurant_name,
                             }).ToList();

                List<ReviewEnt> resp = new List<ReviewEnt>();

                if (datos.Count > 0)
                {
                    foreach (var item in datos)
                    {
                        resp.Add(new ReviewEnt
                        {
                            Review_id = item.review_id,
                            Rating = item.rating,
                            Review_content = item.review_content,
                            Review_date = item.review_date,
                            Is_visible = (bool)item.is_visible,
                            User = item.username,
                            Restaurant = item.restaurant_name
                        });
                    }
                }

                return resp;
            }
        }

        [HttpGet]
        [Route("api/GetReview")]
        public ReviewEnt GetReview(long q)
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Reviews
                             where x.review_id == q
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    ReviewEnt resp = new ReviewEnt();
                    resp.Review_id = q;
                    resp.Rating = datos.rating;
                    resp.Review_content = datos.review_content;
                    resp.Review_date = datos.review_date;
                    resp.Is_visible = (bool)datos.is_visible;
                    resp.User_id = datos.user_id;
                    resp.Restaurant_id = datos.restaurant_id;
                    return resp;
                }

                return null;
            }
        }

        [HttpGet]
        [Route("api/GetReviewsByRestaurantId")]
        public List<ReviewEnt> GetReviewsByRestaurantId(long q)
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Reviews
                             join r in bd.Restaurants on x.restaurant_id equals r.restaurant_id
                             join u in bd.Users on x.user_id equals u.user_id
                             where x.restaurant_id == q
                             && x.is_visible == true
                             select new
                             {
                                 x.review_id,
                                 x.review_content,
                                 x.review_date,
                                 x.rating,
                                 x.is_visible,
                                 x.user_id,
                                 u.username,
                                 u.url_image
                             }).ToList();

                List<ReviewEnt> resp = new List<ReviewEnt>();

                if (datos.Count > 0)
                {
                    foreach (var item in datos)
                    {
                        resp.Add(new ReviewEnt
                        {
                            Review_id = item.review_id,
                            Review_content = item.review_content,
                            Review_date = item.review_date,
                            Rating = item.rating,
                            Is_visible = (bool)item.is_visible,
                            User_id = item.user_id,
                            User = item.username,
                            UserImg = item.url_image,
                        });
                    }
                }

                return resp;
            }
        }


        [HttpPut]
        [Route("api/ChangeReviewVisibility")]
        public int ChangeReviewVisibility(ReviewEnt entidad)
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Reviews
                             where x.review_id == entidad.Review_id
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    datos.is_visible = !datos.is_visible;
                    return bd.SaveChanges();
                }
                return 0;
            }
        }

        [HttpGet]
        [Route("api/GetReviewsByUser")]
        public List<ReviewEnt> GetReviewsByUser(long q)
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Reviews
                             join r in bd.Restaurants on x.restaurant_id equals r.restaurant_id
                             join u in bd.Users on x.user_id equals u.user_id
                             where x.user_id == q
                             select new
                             {
                                 x.review_id,
                                 x.review_content,
                                 x.review_date,
                                 x.rating,
                                 x.is_visible,
                                 x.user_id,
                                 x.restaurant_id,
                                 r.restaurant_name,
                                 u.username,
                                 u.url_image
                             }).ToList();

                List<ReviewEnt> resp = new List<ReviewEnt>();

                if (datos.Count > 0)
                {
                    foreach (var item in datos)
                    {
                        resp.Add(new ReviewEnt
                        {
                            Review_id = item.review_id,
                            Review_content = item.review_content,
                            Review_date = item.review_date,
                            Rating = item.rating,
                            Is_visible = (bool)item.is_visible,
                            User_id = item.user_id,
                            User = item.username,
                            UserImg = item.url_image,
                            Restaurant = item.restaurant_name,
                            Restaurant_id = item.restaurant_id,
                        });
                    }
                }

                return resp;
            }
        }

        [HttpPut]
        [Route("api/ValidateReviewData")]
        public int ValidateReviewData(ReviewEnt entidad)
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Reviews
                             where x.review_id == entidad.Review_id
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    datos.is_visible = entidad.Is_visible;
                    datos.rating = entidad.Rating;
                    datos.review_content = entidad.Review_content;
                    datos.review_date = entidad.Review_date;
                    datos.restaurant_id = entidad.Restaurant_id;
                    return bd.SaveChanges();
                }
                return 0;
            }
        }

        [HttpDelete]
        [Route("api/DeleteReviewtData")]
        public int DeleteReviewtDataa(long q)
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Reviews
                             where x.review_id == q
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    bd.Reviews.Remove(datos);
                    return bd.SaveChanges();
                }

                return 0;
            }
        }
    }
}
