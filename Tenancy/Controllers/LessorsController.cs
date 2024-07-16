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
    public class LessorsController : Controller
    {
        private TenancyEntities db = new TenancyEntities();

        // GET: Lessees у которых статус заявки "В ОБРАБОТКЕ"
        public ActionResult Index()
        {
            /*var list = db.Lessee.ToList();
            list = list.Where(x=>x.ApplicationStatus == "В ОБРАБОТКЕ").ToList();*/
            return View();
        }

        // GET: Lessors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lessor lessor = db.Lessor.Find(id);
            if (lessor == null)
            {
                return HttpNotFound();
            }
            return View(lessor);
        }
        public ActionResult AvialableLessees()
        {
            var list = db.Lessee.Where(x => x.ApplicationStatus == "В ОБРАБОТКЕ");
            return View(list);
        }
        public ActionResult AvialablePremises(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list = db.Premises.Where(x=>x.Lessor_ID ==id);
            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // GET: Lessors/Create
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult ChangeStatus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
                Lessee lessee = db.Lessee.Find(id);
                if (lessee != null)
                {
                    lessee.ApplicationStatus = "СТАДИЯ ФОРМИРОВАНИЯ ДОГОВОРА";
                    if (RegistersController.UserRole == "Арендодатель")
                        lessee.ApplicationDescription = RegistersController.UserID.ToString();
                    db.Entry(lessee).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("AvialableLessees");
                   // return View(lessor);
                }
            return View("AvialableLessees");
        }
        // POST: Lessors/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Lessor_ID,Surname,Name,SecondName,FileNameOwner,FIleData")] Lessor lessor)
        {
            if (ModelState.IsValid)
            {
                db.Lessor.Add(lessor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lessor);
        }

        // GET: Lessors/Edit/
        public ActionResult Edit()
        {
            //RegistersController.UserID
            if (RegistersController.UserID == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lessor lessor = db.Lessor.Find(RegistersController.UserID);
            if (lessor == null)
            {
                return HttpNotFound();
            }
            return View(lessor);
        }

        // POST: Lessors/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Lessor_ID,Surname,Name,SecondName,FileNameOwner,FIleData")] Lessor lessor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lessor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lessor);
        }

        // GET: Lessors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lessor lessor = db.Lessor.Find(id);
            if (lessor == null)
            {
                return HttpNotFound();
            }
            return View(lessor);
        }

        // POST: Lessors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lessor lessor = db.Lessor.Find(id);
            db.Lessor.Remove(lessor);
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
