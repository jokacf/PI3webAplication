using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AtribuicaoCabazesipps.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace AtribuicaoCabazesipps.Controllers
{
    [Authorize]
    public class FamiliasController : Controller
    {
        private bancoAlimentarCabazesEntidades db = new bancoAlimentarCabazesEntidades();

        // GET: Familias
        public ActionResult Index()
        {
            
            if (User.IsInRole("Instituicao"))
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                var familias = db.Familia.Where(f => f.Instituicao.IdUser.Equals(user.Id)).ToList();
                return View(familias);
            }else if (User.IsInRole("Admin"))
            {
                var familia = db.Familia.Include(f => f.Instituicao);
                return View(familia.ToList());
            }
            return null;
        }


        // GET: Familias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Familia familia = db.Familia.Find(id);
            if (familia == null)
            {
                return HttpNotFound();
            }
            return View(familia);
        }

        // GET: Familias/Create
        public ActionResult Create()
        {
            ViewBag.IdInstituicao = new SelectList(db.Instituicao, "Id", "Nome");
            return View();
        }

        // POST: Familias/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public ActionResult CreatePost()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var instituicao = db.Instituicao.Where(f => f.IdUser.Equals(user.Id)).First();

            var nomeFamilia = Request["Nome"];
            var nomeResponsavel = Request["NomeResponsavel"];
            var telefoneResponsavel = Request["TelefoneResponsavel"];
            var nifResponsavel = Request["NIFResponsavel"];
            var biResponsavel = Request["BIResponsavel"];
            var numeroMembros = Request["NumeroMembros"];

            if (ModelState.IsValid)
            {
                Familia familia = new Familia();
                familia.Nome = nomeFamilia;
                familia.NomeResponsavel = nomeResponsavel;
                familia.TelefoneResponsavel = int.Parse(telefoneResponsavel);
                familia.NIFResponsavel = int.Parse(nifResponsavel);
                familia.BIResponsavel = int.Parse(biResponsavel);
                familia.NumeroMembros = int.Parse(numeroMembros);
                familia.IdInstituicao = instituicao.Id;
                db.Familia.Add(familia);
                db.SaveChanges();

                Beneficiario beneficiario = new Beneficiario();
                beneficiario.Nome = nomeResponsavel;
                beneficiario.NIF = int.Parse(nifResponsavel);
                beneficiario.BI = int.Parse(biResponsavel);
                beneficiario.Telefone = int.Parse(telefoneResponsavel);
                beneficiario.IdFamilia = db.Familia.Max(item => item.Id); ;
                db.Beneficiario.Add(beneficiario);
                db.SaveChanges();

                ViewBag.Instituicao = new SelectList(db.Instituicao, "Id", "Nome", familia.IdInstituicao);
                TempData["idFamilia"] = familia.Id;
                TempData["NomeFamilia"] = familia.Nome;
                return RedirectToAction("Create","Beneficiarios");
            }
            return null;
            
        }

        // GET: Familias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Familia familia = db.Familia.Find(id);
            if (familia == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdInstituicao = new SelectList(db.Instituicao, "Id", "Nome", familia.IdInstituicao);
            return View(familia);
        }

        // POST: Familias/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,NomeResponsavel,TelefoneResponsavel,NIFResponsavel,BIResponsavel,NumeroMembros,IdInstituicao")] Familia familia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(familia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdInstituicao = new SelectList(db.Instituicao, "Id", "Nome", familia.IdInstituicao);
            return View(familia);
        }

        // GET: Familias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Familia familia = db.Familia.Find(id);
            if (familia == null)
            {
                return HttpNotFound();
            }
            return View(familia);
        }

        // POST: Familias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Familia familia = db.Familia.Find(id);
            db.Familia.Remove(familia);
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
