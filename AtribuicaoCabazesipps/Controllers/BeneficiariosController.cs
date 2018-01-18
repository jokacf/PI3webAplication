using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AtribuicaoCabazesipps.Models;

namespace AtribuicaoCabazesipps.Controllers
{
    [Authorize]
    public class BeneficiariosController : Controller
    {
        private gestaoCabazesEntities db = new gestaoCabazesEntities();

        // GET: Beneficiarios
        public ActionResult Index()
        {
            var beneficiario = db.Beneficiario.Include(b => b.Familia);
            return View(beneficiario.ToList());
        }

        // GET: Beneficiarios/Details/5

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beneficiario beneficiario = db.Beneficiario.Find(id);
            if (beneficiario == null)
            {
                return HttpNotFound();
            }
            return View(beneficiario);
        }

        // GET: Beneficiarios/Create
       
        public ActionResult Create()
        {
            try
            {
                int idFamilia = (int)TempData["idFamilia"];
                ViewBag.Familia = db.Familia.Where(f => f.idFamilia == idFamilia).First();
            }
            catch (Exception e)
            {

            }         
            ViewBag.fk_idFamilia = new SelectList(db.Familia, "idFamilia", "nomeFamilia");
            return View();
        }

        // POST: Beneficiarios/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idBeneficiario,nomeBeneficiario,nifBeneficiario,biBeneficiario,telefoneBeneficiario,fk_idFamilia")] Beneficiario beneficiario)
        {
            if (ModelState.IsValid)
            {
                db.Beneficiario.Add(beneficiario);
                db.SaveChanges();
                var numeroMembrosRequerido = db.Familia.Where(f => f.idFamilia == beneficiario.fk_idFamilia).First().numeroMembros - 1;
                var numeroDeMembrosRegistados = db.Beneficiario.Count(b => b.fk_idFamilia == beneficiario.Familia.idFamilia);
                if (numeroMembrosRequerido > numeroDeMembrosRegistados)
                {
                    TempData["idFamilia"] = beneficiario.fk_idFamilia;
                    return RedirectToAction("Create");
                }
                return RedirectToAction("Index");
            }

            ViewBag.fk_idFamilia = new SelectList(db.Familia, "idFamilia", "nomeFamilia", beneficiario.fk_idFamilia);
            return View(beneficiario);
        }

        // GET: Beneficiarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beneficiario beneficiario = db.Beneficiario.Find(id);
            if (beneficiario == null)
            {
                return HttpNotFound();
            }
            ViewBag.fk_idFamilia = new SelectList(db.Familia, "idFamilia", "nomeFamilia", beneficiario.fk_idFamilia);
            return View(beneficiario);
        }

        // POST: Beneficiarios/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idBeneficiario,nomeBeneficiario,nifBeneficiario,biBeneficiario,telefoneBeneficiario,fk_idFamilia")] Beneficiario beneficiario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beneficiario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.fk_idFamilia = new SelectList(db.Familia, "idFamilia", "nomeFamilia", beneficiario.fk_idFamilia);
            return View(beneficiario);
        }

        // GET: Beneficiarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beneficiario beneficiario = db.Beneficiario.Find(id);
            if (beneficiario == null)
            {
                return HttpNotFound();
            }
            return View(beneficiario);
        }

        // POST: Beneficiarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Beneficiario beneficiario = db.Beneficiario.Find(id);
            db.Beneficiario.Remove(beneficiario);
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
