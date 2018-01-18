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
    [Authorize(Roles = "Admin")]
    public class BancoAlimentarsController : Controller
    {
        private gestaoCabazesEntities db = new gestaoCabazesEntities();

        // GET: BancoAlimentars
        public ActionResult Index()
        {
            var bancoAlimentar = db.BancoAlimentar.Include(b => b.Beneficiario).Include(b => b.Instituicao);
            return View(bancoAlimentar.ToList());
        }

        // GET: BancoAlimentars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BancoAlimentar bancoAlimentar = db.BancoAlimentar.Find(id);
            if (bancoAlimentar == null)
            {
                return HttpNotFound();
            }
            return View(bancoAlimentar);
        }

        // GET: BancoAlimentars/Create
        public ActionResult Create()
        {
            ViewBag.fk_idBeneficiario = new SelectList(db.Beneficiario, "idBeneficiario", "nomeBeneficiario");
            ViewBag.fk_idInstituicao = new SelectList(db.Instituicao, "idInstituicao", "nomeInstituicao");
            return View();
        }

        // POST: BancoAlimentars/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idRegisto,fk_idInstituicao,fk_idBeneficiario")] BancoAlimentar bancoAlimentar)
        {
            if (ModelState.IsValid)
            {
                db.BancoAlimentar.Add(bancoAlimentar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.fk_idBeneficiario = new SelectList(db.Beneficiario, "idBeneficiario", "nomeBeneficiario", bancoAlimentar.fk_idBeneficiario);
            ViewBag.fk_idInstituicao = new SelectList(db.Instituicao, "idInstituicao", "nomeInstituicao", bancoAlimentar.fk_idInstituicao);
            return View(bancoAlimentar);
        }

        // GET: BancoAlimentars/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BancoAlimentar bancoAlimentar = db.BancoAlimentar.Find(id);
            if (bancoAlimentar == null)
            {
                return HttpNotFound();
            }
            ViewBag.fk_idBeneficiario = new SelectList(db.Beneficiario, "idBeneficiario", "nomeBeneficiario", bancoAlimentar.fk_idBeneficiario);
            ViewBag.fk_idInstituicao = new SelectList(db.Instituicao, "idInstituicao", "nomeInstituicao", bancoAlimentar.fk_idInstituicao);
            return View(bancoAlimentar);
        }

        // POST: BancoAlimentars/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idRegisto,fk_idInstituicao,fk_idBeneficiario")] BancoAlimentar bancoAlimentar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bancoAlimentar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.fk_idBeneficiario = new SelectList(db.Beneficiario, "idBeneficiario", "nomeBeneficiario", bancoAlimentar.fk_idBeneficiario);
            ViewBag.fk_idInstituicao = new SelectList(db.Instituicao, "idInstituicao", "nomeInstituicao", bancoAlimentar.fk_idInstituicao);
            return View(bancoAlimentar);
        }

        // GET: BancoAlimentars/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BancoAlimentar bancoAlimentar = db.BancoAlimentar.Find(id);
            if (bancoAlimentar == null)
            {
                return HttpNotFound();
            }
            return View(bancoAlimentar);
        }

        // POST: BancoAlimentars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BancoAlimentar bancoAlimentar = db.BancoAlimentar.Find(id);
            db.BancoAlimentar.Remove(bancoAlimentar);
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
