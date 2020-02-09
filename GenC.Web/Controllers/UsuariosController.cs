using GenC.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GenC.Web.Controllers
{
    public class UsuariosController: baseController
    {
        public ActionResult Create()
        {
            ViewBag.ChaveGoogle = GoogleMapsKey;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuarios Usuarios)
        {

            if (ModelState.IsValid)
            {
                var existeUsuario = db.Usuarios.Any(u => u.Email == Usuarios.Email || u.CPF == Usuarios.CPF);
                if (!existeUsuario)
                {
                    Usuarios.Senha = ComputeHash(Usuarios.Senha, null);

                    db.Usuarios.Add(Usuarios);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Login");
                }                
            }

            return View(Usuarios);
        }
    }
}