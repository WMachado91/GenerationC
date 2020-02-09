using GenC.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GenC.Web.Controllers
{
    public class LoginController : baseController
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (!SessionAuth())
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Consulta");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(LoginViewModel usuario)
        {
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            var usuarioAcesso = db.Usuarios.Where(u => u.Email == usuario.Email).FirstOrDefault();

            if (usuarioAcesso == null)
            {
                return View(usuarioAcesso);
            }

            if (!VerifyHash(usuario.Senha, usuarioAcesso.Senha))
            {
                return View(usuarioAcesso);
            }

            SessionCookies(usuarioAcesso);

            return RedirectToAction("Index", "Consulta");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            try
            {
                RemoveCookies();
                return RedirectToAction("Index", "Login");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}