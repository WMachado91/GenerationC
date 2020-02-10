using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GenC.Entity;
using Newtonsoft.Json;

namespace GenC.Web.Controllers
{
    public class AgendamentosController : baseController
    {
        // GET: Agendamentos
        public ActionResult Index()
        {
            if (!SessionAuth())
            {
                return RedirectToAction("Index", "Login");
            }

            int UsuariosId = Current_user();
            var agendamentos = db.Agendamentos.Where(d => d.Usuarios.Id == UsuariosId).OrderByDescending(d => d.DtAgendamento).ToList();
            ViewBag.ChaveGoogle = GoogleMapsKey;

            return View(agendamentos);
        }

        // GET: Agendamentos/Details/5
        public ActionResult Details(int? id)
        {
            if (!SessionAuth())
            {
                return RedirectToAction("Index", "Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var usuarioLogado = Current_user();
            Agendamentos agendamentos = db.Agendamentos.Where(u => u.IdUsuario == usuarioLogado && u.Id == id).FirstOrDefault();

            if (agendamentos == null)
            {
                return HttpNotFound();
            }
            //var jsonRetorno = JsonConvert.SerializeObject(agendamentos);
            return Json(JsonConvert.SerializeObject(agendamentos, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }), JsonRequestBehavior.AllowGet);
            //Json(agendamentos, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            if (!SessionAuth())
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Agendamentos agendamentos)
        {
            if (!SessionAuth())
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                var usuarioLogado = db.Usuarios.Find(Current_user());
                agendamentos.IdUsuario = usuarioLogado.Id;
                db.Agendamentos.Add(agendamentos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agendamentos);
        }

        // GET: Agendamentos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!SessionAuth())
            {
                return RedirectToAction("Index", "Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agendamentos agendamentos = db.Agendamentos.Find(id);
            if (agendamentos == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Nome", agendamentos.IdUsuario);
            return View(agendamentos);
        }

        // POST: Agendamentos/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Agendamentos agendamentos)
        {
            if (!SessionAuth())
            {
                return RedirectToAction("Index", "Login");
            }
            if (ModelState.IsValid)
            {
                var usuarioLogado = db.Usuarios.Find(Current_user());
                agendamentos.IdUsuario = usuarioLogado.Id;
                db.Entry(agendamentos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Nome", agendamentos.IdUsuario);
            return View(agendamentos);
        }

        // GET: Agendamentos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!SessionAuth())
            {
                return RedirectToAction("Index", "Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agendamentos agendamentos = db.Agendamentos.Find(id);
            if (agendamentos == null)
            {
                return HttpNotFound();
            }
            return View(agendamentos);
        }

        // POST: Agendamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Agendamentos agendamentos = db.Agendamentos.Find(id);
            db.Agendamentos.Remove(agendamentos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
