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
    public class TeachersController : Controller
    {
        private smsEntities db = new smsEntities();

        // GET: Teachers
        public ActionResult Index()
        {
            /*check session*/
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            /*check session*/
            return View(db.Teachers.ToList());
        }

        // GET: Teachers/Details/5
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
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // GET: Teachers/Create
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

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,MiddleName,LastName,Mobile,Address,DateOfBirth,JoinDate")] Teacher teacher)
        {
            /*check session*/
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            /*check session*/
            if (ModelState.IsValid)
            {
                db.Teachers.Add(teacher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(teacher);
        }

        // GET: Teachers/Edit/5
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
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,MiddleName,LastName,Mobile,Address,DateOfBirth,JoinDate")] Teacher teacher)
        {
            /*check session*/
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            /*check session*/
            if (ModelState.IsValid)
            {
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teacher);
        }

        // GET: Teachers/Delete/5
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
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Delete/5
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
            if (result>0)
            {
                return RedirectToAction("Index");
            }
            Teacher teacher = db.Teachers.Find(id);
            db.Teachers.Remove(teacher);
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
