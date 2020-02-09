using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GenC.Entity;

namespace GenC.Web.Controllers
{
    public class AgendamentosController : Controller
    {
        private DbContextGenC db = new DbContextGenC();

        // GET: Agendamentos
        public ActionResult Index()
        {
            var agendamentos = db.Agendamentos.Include(a => a.Usuarios);
            return View(agendamentos.ToList());
        }

        // GET: Agendamentos/Details/5
        public ActionResult Details(int? id)
        {
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

        // GET: Agendamentos/Create
        public ActionResult Create()
        {
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Nome");
            return View();
        }

        // POST: Agendamentos/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdUsuario,DtAgendamento,DtCriacao,Endereco,CEP,Cidade,Estado,Titulo,Descricao")] Agendamentos agendamentos)
        {
            if (ModelState.IsValid)
            {
                db.Agendamentos.Add(agendamentos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Nome", agendamentos.IdUsuario);
            return View(agendamentos);
        }

        // GET: Agendamentos/Edit/5
        public ActionResult Edit(int? id)
        {
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
        public ActionResult Edit([Bind(Include = "Id,IdUsuario,DtAgendamento,DtCriacao,Endereco,CEP,Cidade,Estado,Titulo,Descricao")] Agendamentos agendamentos)
        {
            if (ModelState.IsValid)
            {
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
