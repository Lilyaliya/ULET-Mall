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
    public class LesseesController : Controller
    {
        public static string StatusBook { get; set; }
        public static string FailureDates { get; set; }
        private TenancyEntities db = new TenancyEntities();

        // GET: Lessees
        public ActionResult Index()
        {
            return View(db.Lessee.ToList());
        }

        // GET: Lessees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lessee lessee = db.Lessee.Find(id);
            if (lessee == null)
            {
                return HttpNotFound();
            }
            return View(lessee);
        }
        //Companies/Floor?
        public ActionResult Companies(int? floor)
        {
            //var lessees = db.Lessee.Where(x=>x.СommercialPremises_Code != 0);
            ViewBag.Floor = floor;
            var reserved = from el in db.Premises
                           where el.Lessee_ID != 0 && el.Floor == floor && el.ApplicationStatus == "ЗАНЯТО"
                           select el;
            List<Lessee> companies = new List<Lessee>();
            foreach (var el in reserved)
            {
                Lessee ind = db.Lessee.Find(el.Lessee_ID);
                ind.СommercialPremises_Code = el.PremisesCode;
                if (ind != null)
                    companies.Add(ind);

            }
            //var res = from el in db.Lessee
                      
            return View(companies);
        }
        // GET: Lessees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Lessees/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Lessee_ID,Organization,Trademarks,BrandType,TradingProfile,ProductProfile,FileNameSlide,FIleData,Surname,Name,SecondName,СommercialPremises_Code,Email,Website,Phone,ApplicationStatus,ApplicationDescription,RentalStartDate,RentalEndDate")] Lessee lessee)
        {
            if (ModelState.IsValid)
            {
                db.Lessee.Add(lessee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lessee);
        }

        // GET: Lessees/Edit/5
        /*public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lessee lessee = db.Lessee.Find(id);
            if (lessee == null)
            {
                return HttpNotFound();
            }
            return View(lessee);
        }*/
        public ActionResult Edit()
        {
            //int Lessee_ID = RegistersController.UserID;
            int Lessee_ID = RegistersController.UserRole == "Арендатор" ? RegistersController.UserID : 0;
            if (Lessee_ID == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lessee lessee = db.Lessee.Find(Lessee_ID);
            if (lessee == null)
            {
                return HttpNotFound();
            }
            //return View(lessee);
            int Lessor_ID;
            List<Premises> list = null;
            if (lessee != null)
            {
                bool yes = int.TryParse(lessee.ApplicationDescription, out Lessor_ID);
                if (yes)
                {
                    list = db.Premises.Where(x => x.Lessor_ID == Lessor_ID && x.ApplicationStatus == "СВОБОДНО").ToList();
                }
                Premises requestedPremise = db.Premises.FirstOrDefault(x => x.ApplicationStatus == "ЗАЯВКА ОТПРАВЛЕНА" && x.Lessee_ID == Lessee_ID);
                if (requestedPremise != null)
                {
                    StatusBook = "В ОБРАБОТКЕ";
                    return View("WaitingForFeedback");
                }
            }
            return View(lessee);
        }

        // POST: Lessees/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Lessee_ID,Organization,Trademarks,BrandType,TradingProfile,ProductProfile,FileNameSlide,FIleData,Surname,Name,SecondName,СommercialPremises_Code,Email,Website,Phone,ApplicationStatus,ApplicationDescription,RentalStartDate,RentalEndDate")] Lessee lessee)
        {
            if (ModelState.IsValid)
            {
                if (lessee.RentalStartDate != null && lessee.RentalEndDate != null)
                    FailureDates = null;
                db.Entry(lessee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lessee);
        }

        // GET: Lessees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lessee lessee = db.Lessee.Find(id);
            if (lessee == null)
            {
                return HttpNotFound();
            }
            return View(lessee);
        }

        // POST: Lessees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lessee lessee = db.Lessee.Find(id);
            db.Lessee.Remove(lessee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult AvialablePremises()
        {
            int Lessee_Id = RegistersController.UserRole == "Арендатор"? RegistersController.UserID:0;
            if (Lessee_Id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Lessee lessee = db.Lessee.Find(Lessee_Id);
            int Lessor_ID;
            List<Premises> list = null;
            if (lessee != null)
            {
                bool yes = int.TryParse(lessee.ApplicationDescription, out Lessor_ID);
                if (yes)
                {
                    list = db.Premises.Where(x => x.Lessor_ID == Lessor_ID && x.ApplicationStatus == "СВОБОДНО").ToList();
                }
                Premises requestedPremise = db.Premises.FirstOrDefault(x => x.ApplicationStatus == "ЗАЯВКА ОТПРАВЛЕНА" && x.Lessee_ID == Lessee_Id);
                if (requestedPremise != null)
                {
                    StatusBook = "В ОБРАБОТКЕ";
                    return View("WaitingForFeedback");
                }
            }
            return View(list);
        }
        public ActionResult BookPremise(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int currLessee = RegistersController.UserID;
            Lessee elem = db.Lessee.Find(currLessee);
            if (elem.RentalStartDate == null || elem.RentalEndDate == null)
            {
                FailureDates = "В заявке не указаны сроки аренды!";
                //ViewBag.Message =  "В заявке не указаны сроки аренды!";
                return RedirectToAction("AvialablePremises");
            }
            FailureDates = null;
            Premises premise = db.Premises.Find(id);
            if (premise != null)
            {
                premise.ApplicationStatus = "ЗАЯВКА ОТПРАВЛЕНА";
                if (RegistersController.UserRole == "Арендатор")
                {
                    premise.Lessee_ID = RegistersController.UserID;
                    elem.СommercialPremises_Code = premise.СommercialPremises_Code;
                }
                db.Entry(premise).State = EntityState.Modified;
                db.SaveChanges();
                StatusBook = "В ОБРАБОТКЕ";
                return RedirectToAction("WaitingForFeedBack");
                // return View(lessor);
            }
            return View("WaitingForFeedBack");
            //return View();
        }

        public ActionResult WaitingForFeedback()
        {
            //if (StatusBook == "В ОБРАБОТКЕ")

            return View();
        }
       /* protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }*/
    }
}
