using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tenancy.Models;

namespace Tenancy.Controllers
{
    public class PremisesController : Controller
    {
        private TenancyEntities db = new TenancyEntities();

        // GET: Premises
        public ActionResult Index()
        {
            var premises = db.Premises.Include(p => p.Lessor);
            return View(premises.ToList());
        }

        // GET: Premises/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Premises premises = db.Premises.Find(id);
            if (premises == null)
            {
                return HttpNotFound();
            }
            return View(premises);
        }
        //GET :Premises/Floor/1
        public ActionResult Floor(int? floor)
        {
            if (floor == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var premises = db.Premises.Where(x => x.Floor == floor);
            if (premises == null)
            {
                return HttpNotFound();
            }
            return View(premises);
        }

        // GET: Premises/Create
        public ActionResult Create()
        {
            //ViewBag.Lessee_ID = new SelectList(db.Lessee, "Lessee_ID", "Organization");
            //ViewBag.Lessor_ID = new SelectList(db.Lessor, "Lessor_ID", "Surname");
            return View();
        }

        // POST: Premises/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommercialPremises_Code, PremisesCode,Floor,Square,UtilityRoom,Windows,Electricity,WaterSupply,Conditioner,Details,CostPerSM,ApplicationStatus,Lessee_ID,Lessor_ID,Type")] Premises premises)
        {
            if (ModelState.IsValid)
            {
                premises.Lessor_ID = RegistersController.UserID;
                premises.ApplicationStatus = "СВОБОДНО";
                db.Premises.Add(premises);
                db.SaveChanges();
                //db.SaveChanges(db.Premises);
                return RedirectToAction("PremisesList");
            }

            //ViewBag.Lessee_ID = new SelectList(db.Lessee, "Lessee_ID", "Organization", premises.Lessee_ID);
            //ViewBag.Lessor_ID = new SelectList(db.Lessor, "Lessor_ID", "Surname", premises.Lessor_ID);
            return View(premises);
        }

        // GET: Premises/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Premises premises = db.Premises.Find(id);
            if (premises == null)
            {
                return HttpNotFound();
            }
            //ViewBag.Lessee_ID = new SelectList(db.Lessee, "Lessee_ID", "Organization", premises.Lessee_ID);
            ViewBag.Lessor_ID = new SelectList(db.Lessor, "Lessor_ID", "Surname", premises.Lessor_ID);
            return View(premises);
        }

        // POST: Premises/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "СommercialPremises_Code,Floor,Square,UtilityRoom,Windows,Electricity,WaterSupply,Conditioner,Details,CostPerSM,ApplicationStatus,Lessee_ID,Lessor_ID,Type")] Premises premises)
        {
            if (ModelState.IsValid)
            {
                db.Entry(premises).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.Lessee_ID = new SelectList(db.Lessee, "Lessee_ID", "Organization", premises.Lessee_ID);
            ViewBag.Lessor_ID = new SelectList(db.Lessor, "Lessor_ID", "Surname", premises.Lessor_ID);
            return View(premises);
        }

        // GET: Premises/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Premises premises = db.Premises.Find(id);
            if (premises == null)
            {
                return HttpNotFound();
            }
            return View(premises);
        }
        public ActionResult PremisesList()
        {

            if (RegistersController.UserID == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int id = RegistersController.UserID;
            var premises = db.Premises.Where(x=>x.Lessor_ID == id);
            if (premises == null)
            {
                return HttpNotFound();
            }
            return View(premises);
        }

        // POST: Premises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Premises premises = db.Premises.Find(id);
            db.Premises.Remove(premises);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /*protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }*/
    }
}
