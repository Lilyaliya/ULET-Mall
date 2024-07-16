using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Tenancy.Models;

namespace Tenancy.Controllers
{
    public class RegistersController : Controller
    {
        public static int UserID { get; set; }
        public static string UserRole {get;set;}
        private TenancyEntities db = new TenancyEntities();
        //private ModelReg.RegLessee 

        // GET: Registers
        public ActionResult Index()
        {
            var register = db.Register.Include(r => r.Lessee).Include(r => r.Lessor);
            return View(register.ToList());
        }

        // GET: Registers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Register register = db.Register.Find(id);
            if (register == null)
            {
                return HttpNotFound();
            }
            return View(register);
        }

        // GET: Registers/Create/role
        public ActionResult Create()
        {
            //if (role == 1)//на вкладке арендатора
               // ViewBag.Id = new SelectList(db.Lessee, "Lessee_ID", "Organization");
            //else if (role == 2)//на квкладке арендодателя
               // ViewBag.Id = new SelectList(db.Lessor, "Lessor_ID", "Surname");
            return View();
        }
        public ActionResult CreateLessee()
        {
            UserRole = "Арендатор";
            ViewBag.Role = "Арендатор";
            return View();
        }
        public ActionResult CreateLessor()
        {
            UserRole = "Арендодатель";
            ViewBag.Role = "Арендодатель";
            return View();
        }
        //POST : REgisters/CreateLessee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateLessee([Bind(Include = "Id,Login,Password,Organization, BrandType, TradingProfile, Surname, Name, Email")] ModelReg.RegLessee regLessee)
        {
            if (ModelState.IsValid)
            {
                Register register = new Register();
                register.Login = regLessee.Login;
                register.Password = regLessee.Password;
                register.Role = "Арендатор";
                Register user = db.Register.Where(query => query.Login.Equals(register.Login) && query.Password.Equals(register.Password)).SingleOrDefault();
                if (user == null)
                {
                    register = db.Register.Add(register);
                    db.SaveChanges();
                    Lessee lessee = new Lessee();
                    lessee.Lessee_ID = register.Id;
                    lessee.Organization = regLessee.Organization;
                    lessee.BrandType = regLessee.BrandType;
                    lessee.TradingProfile = regLessee.TradingProfile;
                    lessee.Surname = regLessee.Surname;
                    lessee.Name = regLessee.Name;
                    lessee.Email = regLessee.Email;
                    lessee.ApplicationStatus = "В ОБРАБОТКЕ";
                    lessee = db.Lessee.Add(lessee);
                    db.SaveChanges();
                    ViewBag.Message = "Данные сохранены";
                    //return RedirectToAction("Index");
                    //return View(regLessor);
                    //return View("IndexLessor");
                }
                else
                {
                    ViewBag.Message = "Вы уже зарегистрированы в базе!";
                }
                return View(regLessee);
                
                //return RedirectToAction("Index");
            }
            return View(regLessee);
        }
        //POST : REgisters/CreateLessor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateLessor([Bind(Include = "Id,Login,Password,Surname, Name, SecondName")] ModelReg.RegLessor regLessor)
        {
            if (ModelState.IsValid)
            {
                Register register = new Register();
                register.Login = regLessor.Login;
                register.Password = regLessor.Password;
                register.Role = "Арендодатель";
                Register user = db.Register.Where(query => query.Login.Equals(register.Login) && query.Password.Equals(register.Password)).SingleOrDefault();
                if (user == null)
                {
                    register = db.Register.Add(register);
                    db.SaveChanges();
                    Lessor lessor = new Lessor();
                    lessor.Lessor_ID = register.Id;
                    lessor.Surname = regLessor.Surname;
                    lessor.Name = regLessor.Name;
                    lessor.SecondName = regLessor.SecondName;
                    lessor = db.Lessor.Add(lessor);
                    db.SaveChanges();
                    ViewBag.Message = "Данные сохранены";
                    //return RedirectToAction("Index");
                    //return View(regLessor);
                    //return View("IndexLessor");
                }
                else
                {
                    ViewBag.Message = "Вы уже зарегистрированы в базе!";
                }
                return View(regLessor);

            }
            return View(regLessor);

        }
        // POST: Registers/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Login,Password,Role")] Register register)
        {
            if (ModelState.IsValid)
            {
                register = db.Register.Add(register);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(register);
        }
        public ActionResult IndexLessee()
        {
            
            ViewBag.Role = "Lessee";
            return View();
        }
        public ActionResult IndexLessor()
        {
            ViewBag.Role = "Lessor";
            return View();
        }
        public ActionResult LoginLessor()
        {
            return View();
        }
        public ActionResult LoginLessee()
        {
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult SaveRegisterDetails(Register registerDetails)
        {
            //We check if the model state is valid or not. We have used DataAnnotation attributes.
            //If any form value fails the DataAnnotation validation the model state becomes invalid.
            if (ModelState.IsValid)
            {
                //create database context using Entity framework 
               
                    //If the model state is valid i.e. the form values passed the validation then we are storing the User's details in DB.
                    Register reglog = new Register();

                    //Save all details in RegitserUser object

                    reglog.Login = registerDetails.Login;
                    reglog.Password = registerDetails.Password;
                    reglog.Role = registerDetails.Role;
                    //reglog.Password = registerDetails.Password;

                    Register user = db.Register.Where(query => query.Login.Equals(reglog.Login) && query.Password.Equals(reglog.Password)).SingleOrDefault();
                    if (user == null)
                    {
                        db.Register.Add(reglog);
                        db.SaveChanges();
                        ViewBag.Message = "Данные сохранены";
                        return View("Register");
                    }
                    else
                        ViewBag.Message = "Вы уже зарегистрированы в базе!";
                    return View("Register", registerDetails);
                    //Calling the SaveDetails method which saves the details.
                


            }
            else
            {

                //If the validation fails, we are returning the model object with errors to the view, which will display the error messages.
                return View("Register", registerDetails);
            }
        }


        public ActionResult Login()
        {
            /*if (Role == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Role == "Lessee")
                return View("LoginLessee");
            else if (Role == "Lessor")
                return View("LoginLessor");*/
            
            return View();
        }

        //The login form is posted to this method.
        [HttpPost]
        public ActionResult Login(Register register)
        {
            //Checking the state of model passed as parameter.
            if (ModelState.IsValid)
            {

                //Validating the user, whether the user is valid or not.
                var isValidUser = IsValidUser(register);

                //If user is valid & present in database, we are redirecting it to Welcome page.
                if (isValidUser != null)
                {
                    FormsAuthentication.SetAuthCookie(register.Login, false);
                    UserID = isValidUser.Id;
                    LesseesController.StatusBook = null;
                    LesseesController.FailureDates = null;
                    ViewBag.Id = isValidUser.Id;
                    if (isValidUser.Role == "Арендатор")
                    {
                        ViewBag.Role = "Lessee";
                        UserRole = isValidUser.Role;
                        return RedirectToAction("IndexLessee");
                    }
                    else 
                    {
                        ViewBag.Role = "Lessor";
                        UserRole = isValidUser.Role;
                        return RedirectToAction("IndexLessor");
                    }
                    //return RedirectToAction("Index");
                }
                else
                {
                    //If the username and password combination is not present in DB then error message is shown.
                    ModelState.AddModelError("Failure", "Неправильно указаны учетные данные!");
                    return View();
                }
            }
            else
            {
                //If model state is not valid, the model with error message is returned to the View.
                return View(register);
            }
        }

        //function to check if User is valid or not
        public Register IsValidUser(Register model)
        {
           
                //Retireving the user details from DB based on username and password enetered by user.
                Register user = db.Register.Where(query => query.Login.Equals(model.Login) && query.Password.Equals(model.Password)).SingleOrDefault();

                //If user is present, then true is returned.
                if (user == null)
                    return null;
                //If user is not present false is returned.
                else
                    return user;
            
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            if (ViewBag.Role == "Lessor")
                return RedirectToAction("IndexLessor");
            else
                return RedirectToAction("IndexLessee");
        }

        // GET: Registers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Register register = db.Register.Find(id);
            if (register == null)
            {
                return HttpNotFound();
            }
            //ViewBag.Id = new SelectList(db.Lessee, "Lessee_ID", "Organization", register.Id);
            //ViewBag.Id = new SelectList(db.Lessor, "Lessor_ID", "Surname", register.Id);
            return View(register);
        }

        // POST: Registers/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Login,Password,Role")] Register register)
        {
            if (ModelState.IsValid)
            {
                db.Entry(register).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Lessee, "Lessee_ID", "Organization", register.Id);
            ViewBag.Id = new SelectList(db.Lessor, "Lessor_ID", "Surname", register.Id);
            return View(register);
        }

        // GET: Registers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Register register = db.Register.Find(id);
            if (register == null)
            {
                return HttpNotFound();
            }
            return View(register);
        }

        // POST: Registers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Register register = db.Register.Find(id);
            db.Register.Remove(register);
            db.SaveChanges();
            return RedirectToAction("Index");
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
