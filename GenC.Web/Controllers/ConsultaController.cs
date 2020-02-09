using GenC.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GenC.Web.Controllers
{
    public class ConsultaController:baseController
    {
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

        [HttpPost]
        public ActionResult cadastrarEvento(Agendamentos agendamento)
        {
            if (!SessionAuth())
                return Json(new { mensagem = "Conectar-se novamente" });

            if (!ModelState.IsValid)
                return Json(agendamento);

            int usuarioId = Current_user();

            var mensagemRetorno = "";

            try
            {
                Usuarios usuarioLogado = db.Usuarios.Where(u => u.Id == usuarioId).FirstOrDefault();
                agendamento.IdUsuario = usuarioLogado.Id;
                agendamento.Usuarios = usuarioLogado;
                db.Agendamentos.Add(agendamento);
                db.SaveChanges();
                mensagemRetorno = "Evento registrado";
            }
            catch (Exception ex)
            {
                mensagemRetorno = ex.Message.ToString();
            }

            return Json(new { mensagem = mensagemRetorno });

        }
        public ActionResult buscarEventos(string pesquisa)
        {
            if (!SessionAuth())
            {
                return RedirectToAction("Index", "Login");
            }

            int UsuariosId = Current_user();

            if (!String.IsNullOrEmpty(pesquisa))
            {
                var devices = db.Agendamentos.Where(d => d.Usuarios.Id == UsuariosId && d.Titulo.Contains(pesquisa) || d.Usuarios.Id == UsuariosId && d.Descricao.Contains(pesquisa));
                return Json(devices.ToList(), JsonRequestBehavior.AllowGet);
            }

            else
            {
                var devices = db.Agendamentos.Where(d => d.Usuarios.Id == UsuariosId);
                return Json(devices.ToList(), JsonRequestBehavior.AllowGet);
            }


        }
    }
}