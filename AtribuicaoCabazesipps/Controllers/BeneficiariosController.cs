using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AtribuicaoCabazesipps.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace AtribuicaoCabazesipps.Controllers
{
    [Authorize]
    public class BeneficiariosController : Controller
    {
        private bancoAlimentarCabazesEntidades db = new bancoAlimentarCabazesEntidades();

        // GET: Beneficiarios
        public ActionResult Index()
        {
            if (User.IsInRole("Instituicao"))
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                var beneficiarios = db.Beneficiario.Where(f => f.Familia.Instituicao.IdUser.Equals(user.Id)).ToList();
                return View(beneficiarios);
            }
            else if (User.IsInRole("Admin"))
            {
                var beneficiario = db.Beneficiario.Include(b => b.Familia);
                return View(beneficiario.ToList());
            }
            return null;        
        }

        public ActionResult ByFamilia(int id)
        {
            var beneficiarios = db.Beneficiario.Where(m => m.IdFamilia.Equals(id)).ToList();
            ViewBag.Familia = db.Familia.Where(f => f.Id == id).FirstOrDefault();
            return View(beneficiarios);
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
        public ActionResult Create(int id = 0)
        {
            if (id == 0)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                ViewBag.IdFamilia = new SelectList(db.Familia.Where(f => f.Instituicao.IdUser.Equals(user.Id)).ToList(), "Id", "Nome");
            }
            else{
                var fam = db.Familia.Where(f => f.Id == id).First();
                ViewBag.Familia = fam;
                fam.NumeroMembros = fam.NumeroMembros + 1;
                db.Entry(fam).State = EntityState.Modified;
                db.SaveChanges();
            }         
            
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
                try
                {
                    db.SaveChanges();
                    var numeroMembrosRequerido = db.Familia.Where(f => f.Id == beneficiario.IdFamilia).First().NumeroMembros;
                    var numeroDeMembrosRegistados = db.Beneficiario.Count(b => b.IdFamilia == beneficiario.Familia.Id) + 1;

                    if (numeroMembrosRequerido > numeroDeMembrosRegistados)
                    {
                        TempData["idFamilia"] = beneficiario.IdFamilia;
                        TempData["MessageBeneficiario"] = null;
                        return RedirectToAction("Create");
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    TempData["MessageBeneficiario"] = "Atenção, dados introduzidos já existentes na bases de dados!";
                    TempData["idFamilia"] = db.Familia.Max(item => item.Id);
                    return RedirectToAction("Create");
                }
                
                
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
            Session["idFamilia"] = beneficiario.IdFamilia;
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
        public ActionResult Edit([Bind(Include = "Id,Nome,NIF,BI,Telefone")] Beneficiario beneficiario)
        {
            if (ModelState.IsValid)
            {
                beneficiario.IdFamilia = (int)Session["idFamilia"];
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
            var fam = db.Familia.Where(f => f.Id == beneficiario.IdFamilia).First();
            ViewBag.Familia = fam;
            if(fam.NumeroMembros != 0) { 
            fam.NumeroMembros = fam.NumeroMembros - 1;
            db.Entry(fam).State = EntityState.Modified;
            db.SaveChanges();
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
