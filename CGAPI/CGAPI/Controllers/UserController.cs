using CGAPI.App_Start;
using CGAPI.Entities;
using CGAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace CGAPI.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        generals model = new generals();
        TokenGenerator tokenGenerator = new TokenGenerator();

        [HttpPost]
        [Route("api/AddUser")]
        [AllowAnonymous]
        public int AddUser(UserEnt entidad)
        {
            using (var bd = new dbEntities())
            {
                Users tabla = new Users();
                tabla.mail = entidad.Mail;
                tabla.password = entidad.Password;
                tabla.username = entidad.Username;
                tabla.url_image = entidad.Url_image;
                tabla.rol_id = entidad.Rol_id;
                tabla.register_date = entidad.Register_date;
                tabla.date_recovery = entidad.Date_recovery;
                tabla.use_recovery_password = entidad.Use_recovery_password;
                bd.Users.Add(tabla);
                return bd.SaveChanges();
            }
        }

        [HttpGet]
        [Route("api/GetUsers")]
        public List<UserEnt> GetUsers()
        {
            using (var bd = new dbEntities())
            {
                var datos = (from u in bd.Users
                             join r in bd.Roles on u.rol_id equals r.rol_id
                             select new
                             {
                                 u.user_id,
                                 u.mail,
                                 u.username,
                                 u.url_image,
                                 u.rol_id,
                                 u.register_date,
                                 r.rol_name,
                                 reviews = u.Reviews.Count(),
                             }).ToList();
                List<UserEnt> resp = new List<UserEnt>();

                if (datos.Count > 0)
                {
                    foreach (var item in datos)
                    {
                        resp.Add(new UserEnt
                        {
                            User_id = item.user_id,
                            Mail = item.mail,
                            Username = item.username,
                            Url_image = item.url_image,
                            Rol_id = item.rol_id,
                            Register_date = (DateTime)item.register_date,
                            Rol_name = item.rol_name,
                            Reviews = item.reviews,
                        });
                    }
                }

                return resp;
            }
        }

        [HttpGet]
        [Route("api/GetUser")]
        public UserEnt GetUser(long q)
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Users
                             where x.user_id == q
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    UserEnt resp = new UserEnt();
                    resp.User_id = datos.user_id;
                    resp.Mail = datos.mail;
                    resp.Username = datos.username;
                    resp.Url_image = datos.url_image;
                    resp.Rol_id = datos.rol_id;
                    resp.Register_date = (DateTime)datos.register_date;
                    return resp;
                }

                return null;
            }
        }

        [HttpPost]
        [Route("api/Login")]
        [AllowAnonymous]
        public UserEnt Login(UserEnt entidad)
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Users
                             join r in bd.Roles on x.rol_id equals r.rol_id
                             where x.username == entidad.Username
                             && x.password == entidad.Password
                             select new
                             {
                                 x.user_id,
                                 x.username,
                                 x.mail,
                                 x.url_image,
                                 x.rol_id,
                                 x.register_date,
                                 x.date_recovery,
                                 x.use_recovery_password,
                                 r.rol_name
                             }).FirstOrDefault();

                if (datos != null)
                {
                    // Asegúrate de que use_recovery_password tenga un valor antes de compararlo con true
                    if (datos.use_recovery_password.HasValue && datos.use_recovery_password.Value && datos.date_recovery < DateTime.Now)
                    {
                        return null;
                    }

                    UserEnt resp = new UserEnt();
                    resp.User_id = datos.user_id;
                    resp.Username = datos.username;
                    resp.Mail = datos.mail;
                    resp.Url_image = datos.url_image;
                    resp.Rol_id = datos.rol_id;
                    resp.Register_date = (DateTime)datos.register_date;
                    resp.Rol_name = datos.rol_name;
                    resp.Token = tokenGenerator.GenerateTokenJwt(datos.mail);
                    return resp;
                }

                return null;
            }
        }


        [HttpPut]
        [Route("api/ValidateUserData")]
        public int ValidateUserData(UserEnt entidad)
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Users
                             where x.user_id == entidad.User_id
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    datos.url_image = entidad.Url_image;
                    datos.username = entidad.Username;
                    datos.mail = entidad.Mail;
                    return bd.SaveChanges();
                }
                return 0;
            }
        }

        [HttpPost]
        [Route("api/RecoveryPassword")]
        [AllowAnonymous]
        public bool RecoveryPassword(UserEnt entidad)
        {
            using (var bd = new dbEntities())
            {
                //el usuario es válido?
                var datos = (from x in bd.Users
                             where x.mail == entidad.Mail
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    //generar una contraseña temporal
                    string password = model.CreatePassword();

                    //actualizar la contraseña temporal en la bd
                    datos.password = password;
                    datos.use_recovery_password = true;
                    datos.date_recovery = DateTime.Now.AddMinutes(15);
                    bd.SaveChanges();

                    //correo
                    StringBuilder mensaje = new StringBuilder();
                    mensaje.Append("<html><head></head><body>");
                    mensaje.Append("Estimado(a) " + datos.username + "<br/><br/>");
                    mensaje.Append("Se ha generado la siguiente contraseña temporal para que ingrese al sistema: <b>" + password + "</b><br/><br/>");
                    mensaje.Append("La contraseña generado tiene 15 minutos de validez, por favor ingrese al sistema para establecerla <br/><br/>");
                    mensaje.Append("Este correo se ha generado de manera automática del el sistema <br/><br/>");
                    mensaje.Append("Muchas Gracias <br/><br/>");
                    mensaje.Append("</body></html>");

                    //enviar correo
                    model.SendEmail(datos.mail, "Recuperar Contraseña", mensaje.ToString());
                    return true;
                }

            }
            return false;
        }

        [HttpPut]
        [Route("api/ChangePassword")]
        public int ChangePassword(UserEnt entidad)
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Users
                             where x.user_id == entidad.User_id
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    datos.password = entidad.New_password;
                    datos.use_recovery_password = false;
                    datos.date_recovery = DateTime.Now;
                    return bd.SaveChanges();
                }
                return 0;
            }
        }

        [HttpPut]
        [Route("api/UpdateUserImage")]
        public void UpdateUserImage(UserEnt entidad)
        {
            using (var bd = new dbEntities())
            {
                var datos = (from x in bd.Users
                             where x.user_id == entidad.User_id
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
