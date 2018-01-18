using AtribuicaoCabazesipps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AtribuicaoCabazesipps.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AfterRegister()
        {
            gestaoCabazesEntities db = new gestaoCabazesEntities();
            return View(db.Instituicao.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Banco Alimentar";

            return View();
        }



        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}