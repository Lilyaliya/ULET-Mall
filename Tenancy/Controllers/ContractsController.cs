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
    public class ContractsController : Controller
    {
        private TenancyEntities db = new TenancyEntities();

        // GET: Contracts
        public ActionResult Index()
        {
            var contract = db.Contract.Include(c => c.Lessee).Include(c => c.Lessor).Include(c => c.Premises);
            return View(contract.ToList());
        }

        // GET: Contracts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = db.Contract.Find(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // GET: Contracts/Create
        public ActionResult Create()
        {
            //ViewBag.Lessor_ID = new SelectList(db.Lessor, "Lessor_ID", "Surname");
            var listTemp = db.Premises.Where(x=>x.Lessor_ID == RegistersController.UserID && x.ApplicationStatus == "ЗАЯВКА ОТПРАВЛЕНА");
            //var listLessees = db.Lessee.Where(x=>x.);
            List<Lessee> lessees = new List<Lessee>();
            foreach (var el in listTemp)
            {
                Lessee elem = db.Lessee.Find(el.Lessee_ID);
                if (elem != null)
                    lessees.Add(elem);
            }
            var temp = new SelectList(lessees, "Lessee_ID", "Organization");
            ViewBag.Lessee_ID = temp;
            ViewBag.СommercialPremises_Code = new SelectList(listTemp, "СommercialPremises_Code", "СommercialPremises_Code");

            //List<Premises> currPremise = new List<Premises>() { listTemp.FirstOrDefault(x => x.Lessee_ID == ) };
            return View();
        }
        public ActionResult AvialableContracts()
        {
            /*int Lessor_Id = RegistersController.UserID;
            if (Lessor_Id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var contracts = db.Contract.Where (x=>x.Lessor_ID == Lessor_Id);
            if (contracts == null)
            {
                return HttpNotFound();
            }
            return View(contracts);*/
            var listTemp = db.Premises.Where(x => x.Lessor_ID == RegistersController.UserID && x.ApplicationStatus == "ЗАЯВКА ОТПРАВЛЕНА");
            //var listLessees = db.Lessee.Where(x=>x.);
            List<Lessee> lessees = new List<Lessee>();
            foreach (var el in listTemp)
            {
                Lessee elem = db.Lessee.Find(el.Lessee_ID);
                if (elem != null)
                    lessees.Add(elem);
            }
            List<Contract> result = new List<Contract>();
            foreach (var e in lessees)
            {
                Contract item = new Contract();
                item.Lessee = e;
                item.Lessor = db.Lessor.Find(RegistersController.UserID);
                item.Premises = db.Premises.Find(e.СommercialPremises_Code);
                item.Lessee_ID = e.Lessee_ID;
                item.Lessor_ID = RegistersController.UserID;
                item.RentalStartDate = (DateTime)e.RentalStartDate;
                item.RentalEndDate = (DateTime)e.RentalEndDate;
                item.СommercialPremises_Code = e.СommercialPremises_Code;
                result.Add(item);
            }
            //List<Premises> currPremise = new List<Premises>() { listTemp.FirstOrDefault(x => x.Lessee_ID == ) };
            return View(result);
        }
        // POST: Contracts/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContractCode,Lessee_ID,Lessor_ID,СommercialPremises_Code,RentalStartDate,RentalEndDate,FileNameContract,FileDataContract,FileNameLessee,FileDataLessee")] Contract contract)
        {
            if (ModelState.IsValid)
            {
                contract.Lessor_ID = RegistersController.UserID;
                var lessee = db.Lessee.Find(contract.Lessee_ID);
                contract.RentalStartDate = (DateTime)lessee.RentalStartDate;
                contract.RentalEndDate = (DateTime)lessee.RentalEndDate;
                db.Contract.Add(contract);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Lessor_ID = new SelectList(db.Lessee, "Lessee_ID", "Organization", contract.Lessee_ID);
            var listTemp = db.Premises.Where(x => x.Lessor_ID == RegistersController.UserID && x.ApplicationStatus == "ЗАЯВКА ОТПРАВЛЕНА");
            Premises currPremise = listTemp.FirstOrDefault(x=>x.Lessee_ID == contract.Lessee_ID);
            if (currPremise != null)
            {
                ViewBag.СommercialPremises_Code = currPremise.СommercialPremises_Code;
                contract.СommercialPremises_Code = currPremise.СommercialPremises_Code;
            }
            //ViewBag.Lessor_ID = new SelectList(db.Lessor, "Lessor_ID", "Surname", contract.Lessor_ID);
            //ViewBag.СommercialPremises_Code = new SelectList(db.Premises, "СommercialPremises_Code", "СommercialPremises_Code", contract.СommercialPremises_Code).SelectedValue;

            return View(contract);
        }

        // GET: Contracts/Edit/5
        public ActionResult Edit(int? id)
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
            //List<Contract> result = new List<Contract>();
                Contract item = new Contract();
                item.Lessee = lessee;
                item.Lessor = db.Lessor.Find(RegistersController.UserID);
                item.Premises = db.Premises.Find(lessee.СommercialPremises_Code);
                item.Lessee_ID = lessee.Lessee_ID;
                item.Lessor_ID = RegistersController.UserID;
                item.RentalStartDate = (DateTime)lessee.RentalStartDate;
                item.RentalEndDate = (DateTime)lessee.RentalEndDate;
                item.СommercialPremises_Code = lessee.СommercialPremises_Code;
                if (db.Contract.Find(item.ContractCode) == null)
                {
                    item = db.Contract.Add(item);
                    db.SaveChanges();
                    //db.Entry(item).State = EntityState.Modified;
                    item.Premises.ApplicationStatus = "ЗАНЯТО";
                    item.Lessee.ApplicationStatus = "ДОГОВОР ЗАКЛЮЧЕН";
                    item.Lessee.ApplicationDescription = item.ContractCode.ToString();
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();

                }
                else { ViewBag.Message = "Вы уже заключили договор!"; }
                    return RedirectToAction("AvialableContracts");
        }

        // POST: Contracts/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContractCode,Lessee_ID,Lessor_ID,СommercialPremises_Code,RentalStartDate,RentalEndDate,FileNameContract,FileDataContract,FileNameLessee,FileDataLessee")] Contract contract)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contract).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Lessee_ID = new SelectList(db.Lessee, "Lessee_ID", "Organization", contract.Lessee_ID);
            ViewBag.Lessor_ID = new SelectList(db.Lessor, "Lessor_ID", "Surname", contract.Lessor_ID);
            ViewBag.СommercialPremises_Code = new SelectList(db.Premises, "СommercialPremises_Code", "Electricity", contract.СommercialPremises_Code);
            return View(contract);
        }

        // GET: Contracts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = db.Contract.Find(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contract contract = db.Contract.Find(id);
            db.Contract.Remove(contract);
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
