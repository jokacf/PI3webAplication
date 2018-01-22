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
    public class FamiliasController : Controller
    {
        private bancoAlimentarCabazesEntidades db = new bancoAlimentarCabazesEntidades();

        // GET: Familias
        public ActionResult Index()
        {
            if (User.IsInRole("Instituicao"))
            {
                var familias = db.Familia.Where(m => m.Instituicao.Id.Equals(m.IdInstituicao)).ToList();
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
        public ActionResult Create([Bind(Include = "Id,Nome,NomeResponsavel,TelefoneResponsavel,NIFResponsavel,BIResponsavel,NumeroMembros")] Familia familia)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                var id = user.Id.ToString();
                var instituicao = db.Instituicao.Where(f => f.IdUser.Equals(id)).First();
                familia.IdInstituicao = instituicao.Id;
                db.Familia.Add(familia);
                db.SaveChanges();
                TempData["IdFamilia"] = familia.Id;
                return RedirectToAction("Create","Beneficiarios");
            }

            ViewBag.IdInstituicao = new SelectList(db.Instituicao, "Id", "Nome", familia.IdInstituicao);
            return View(familia);
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
