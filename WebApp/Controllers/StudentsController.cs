using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class StudentsController : Controller
    {
        private smsEntities db = new smsEntities();

        // GET: Students
        public ActionResult Index()
        {
            /*check session*/
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            /*check session*/
            return View(db.Students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            /*check session*/
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            /*check session*/
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            /*check session*/
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            /*check session*/
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,MiddleName,LastName,RollNo,Mobile,DateOfBirth,Address")] Student student)
        {
            /*check session*/
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            /*check session*/
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            /*check session*/
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            /*check session*/
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,MiddleName,LastName,RollNo,Mobile,DateOfBirth,Address")] Student student)
        {
            /*check session*/
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            /*check session*/
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            /*check session*/
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            /*check session*/
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            /*check session*/
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            /*check session*/
            var result = db.Batches.Include(x => x.Teacher).Where(b => b.TeacherId == id).Count();
            if (result > 0)
            {
                return RedirectToAction("Index");
            }
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
