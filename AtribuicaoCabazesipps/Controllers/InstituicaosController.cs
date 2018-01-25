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
    public class InstituicaosController : Controller
    {
        private bancoAlimentarCabazesEntidades db = new bancoAlimentarCabazesEntidades();

        // GET: Instituicaos
        public ActionResult Index()
        {
            return View(db.Instituicao.ToList());
        }

        // GET: Instituicaos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instituicao instituicao = db.Instituicao.Find(id);
            if (instituicao == null)
            {
                return HttpNotFound();
            }
            return View(instituicao);
        }

        // GET: Instituicaos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Instituicaos/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,Nif,Telefone")] Instituicao instituicao)
        {
            if (ModelState.IsValid)
            {
                var id = (string)Session["lastIdregistered"];
                
                //db.Instituicao.Add(instituicao);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(instituicao);
        }

        // GET: Instituicaos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instituicao instituicao = db.Instituicao.Find(id);
            Session["IdtoEdit"] = instituicao.IdUser;
            if (instituicao == null)
            {
                return HttpNotFound();
            }
            return View(instituicao);
        }

        // POST: Instituicaos/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Nif,Telefone")] Instituicao instituicao)
        {
            if (ModelState.IsValid)
            {
                var id = (string)Session["IdtoEdit"];
                //a partir do index enviar o id do user da familia selecionada
                instituicao.IdUser = id;
                db.Entry(instituicao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(instituicao);
        }

        // GET: Instituicaos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Instituicao instituicao = db.Instituicao.Find(id);
            if (instituicao == null)
            {
                return HttpNotFound();
            }
            return View(instituicao);
        }

        // POST: Instituicaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Instituicao instituicao = db.Instituicao.Find(id);
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(instituicao.IdUser);
            db.Instituicao.Remove(instituicao);
            db.AspNetUsers.Remove(aspNetUsers);
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
