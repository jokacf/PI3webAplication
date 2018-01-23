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
    public class BeneficiariosController : Controller
    {
        private bancoAlimentarCabazesEntidades db = new bancoAlimentarCabazesEntidades();

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
                    ViewBag.Familia = db.Familia.Where(f => f.Id == idFamilia).First();
                }
                catch (Exception e)
                {

                }
            ViewBag.IdFamilia = new SelectList(db.Familia, "Id", "Nome");
            return View();
        }

        // POST: Beneficiarios/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,NIF,BI,Telefone,IdFamilia")] Beneficiario beneficiario)
        {
            if (ModelState.IsValid)
            {
                db.Beneficiario.Add(beneficiario);
                db.SaveChanges();
                var numeroMembrosRequerido = db.Familia.Where(f => f.Id == beneficiario.IdFamilia).First().NumeroMembros;
                var numeroDeMembrosRegistados = db.Beneficiario.Count(b => b.IdFamilia == beneficiario.Familia.Id);

                if (numeroMembrosRequerido > numeroDeMembrosRegistados)
                {
                    TempData["idFamilia"] = beneficiario.IdFamilia;                
                    return RedirectToAction("Create");
                }
                return RedirectToAction("Index");
            }

            ViewBag.IdFamilia = new SelectList(db.Familia, "idFamilia", "nomeFamilia", beneficiario.IdFamilia);
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
            ViewBag.IdFamilia = new SelectList(db.Familia, "Id", "Nome", beneficiario.IdFamilia);
            return View(beneficiario);
        }

        // POST: Beneficiarios/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,NIF,BI,Telefone,IdFamilia")] Beneficiario beneficiario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beneficiario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdFamilia = new SelectList(db.Familia, "Id", "Nome", beneficiario.IdFamilia);
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
