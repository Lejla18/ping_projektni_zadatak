using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Inspekcijska_kontrola_proizvoda.Models;
using PagedList;

namespace Inspekcijska_kontrola_proizvoda.Controllers
{
    public class ProizvodController : Controller
    {
        private ping_inspekcijska_kontrola_proizvodaEntities db = new ping_inspekcijska_kontrola_proizvodaEntities();




        //Ispis svih proizvoda
        public ActionResult Details(string option, string search, int? pageNumber)
        {
            
            if (option == "Proizvodjac")
            {
                return View(db.Proizvod.Where(x => x.Izbrisan == false && (x.Proizvodjac.StartsWith(search) || search == null)).ToList().OrderBy(x => x.NazivProizvoda).ToPagedList(pageNumber ?? 1, 10));
            }
            else if (option == "SerijskiBroj")
            {
                return View(db.Proizvod.Where(x => x.Izbrisan == false && (x.SerijskiBroj.StartsWith(search) || search == null)).ToList().OrderBy(x => x.NazivProizvoda).ToPagedList(pageNumber ?? 1, 10));
            }
            else if(option == "NazivProizvoda")
            {
                return View(db.Proizvod.Where(x=> x.Izbrisan == false && (x.NazivProizvoda.StartsWith(search) || search == null)).ToList().OrderBy(x => x.NazivProizvoda).ToPagedList(pageNumber ?? 1, 10));
            }

            else return View(db.Proizvod.Where(x => x.Izbrisan == false).ToList().OrderBy(x => x.NazivProizvoda).ToPagedList(pageNumber ?? 1, 10)); 
        }



        // GET: Kreiranje proizvoda
        public ActionResult Create()
        {
            return View();
        }


        // POST: Kreiranje proizvoda
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProzvodID,NazivProizvoda,Proizvodjac,SerijskiBroj,ZemljaPorijekla,Opis,Izbrisan")] Proizvod proizvod)
        {
            if (ModelState.IsValid)
            {   
                
                db.Proizvod.Add(proizvod);
                db.SaveChanges();
                ViewBag.uspjesno_dodan = true;
                return PartialView("partial_view_created");

            }

            
         return View(proizvod);
                  
        }


        // GET: Izmjena proizvoda
        public ActionResult Edit(int? id)
        {
            
            Proizvod proizvod = db.Proizvod.Find(id);
            return View(proizvod);
        }

        
        // POST : Izmjena proizvoda

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProizvodID,NazivProizvoda,Proizvodjac,SerijskiBroj,ZemljaPorijekla,Opis")] Proizvod proizvod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proizvod).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.uspjesno_editovan = true;
                return PartialView("partial_view_edited");
            }
            return View(proizvod);
        }


    

        // GET: Brisanje proizvoda
        public ActionResult Delete(int? id)
        {
            
            Proizvod proizvod = db.Proizvod.Find(id);
            return View(proizvod);
        }



        // POST: Brisanje proizvoda

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Proizvod proizvod = db.Proizvod.Find(id);
            proizvod.Izbrisan = true;
            ViewBag.uspjesno_izbrisan = true;
            db.SaveChanges();
            return PartialView("partial_view_deleted");
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
