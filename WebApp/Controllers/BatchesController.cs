using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class BatchesController : Controller
    {
       


        private smsEntities db = new smsEntities();

        // GET: Batches
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var batches = db.Batches.Include(b => b.Subject).Include(b => b.Teacher);
            return View(batches.ToList());
        }

        // GET: Batches/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Batch batch = db.Batches.Find(id);
            if (batch == null)
            {
                return HttpNotFound();
            }
            return View(batch);
        }

        // GET: Batches/Create
        public ActionResult Create()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "Name");
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "FirstName");

            var item = db.Students.ToList();
            BatchViewModel studentBatchViewModel = new BatchViewModel();
            studentBatchViewModel.AvailableStudents = item.Select(vm => new BatchStudentViewModel()
            {
                Id = vm.Id,
                Title = vm.FirstName + " " + vm.MiddleName + " " + vm.LastName,
                IsChecked = false
            }).ToList();
            return View(studentBatchViewModel);
        }

        // POST: Batches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BatchViewModel batchViewModel, Batch batch, BatchStudent batchStudent)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                List<BatchStudent> batchStudents = new List<BatchStudent>();
                batch.Name = batchViewModel.Name;
                batch.SubjectId = batchViewModel.SubjectId;
                batch.TeacherId = batchViewModel.TeacherId;
                batch.Date = batchViewModel.Date;
                batch.Time = batchViewModel.Time;
                db.Batches.Add(batch);
                db.SaveChanges();
                int batchId = batch.Id;

                foreach (var item in batchViewModel.AvailableStudents)
                {
                    if (item.IsChecked == true)
                    {
                        batchStudents.Add(new BatchStudent() { BatchId = batchId, StudentId = item.Id });
                    }
                }

                foreach (var item in batchStudents)
                {
                    db.BatchStudents.Add(item);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "Name", batch.SubjectId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "FirstName", batch.TeacherId);
            return View(batch);
        }

        // GET: Batches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BatchViewModel SBVM = new BatchViewModel();
            var bh = db.Batches.Include(s => s.BatchStudents.Select(e => e.Student)).AsNoTracking().SingleOrDefault(m => m.Id == id);
            var allStudents = db.Students.Select(vm => new BatchStudentViewModel()
            {
                Id = vm.Id,
                Title = vm.FirstName+ " " + vm.MiddleName + " " + vm.LastName,
                IsChecked = vm.BatchStudents.Any(x =>x.BatchId == bh.Id)
            }).ToList();
            SBVM.Name = bh.Name;
            SBVM.SubjectId = bh.SubjectId;
            SBVM.TeacherId = bh.TeacherId;
            SBVM.Date = bh.Date;
            SBVM.Time = bh.Time;
            SBVM.AvailableStudents = allStudents;
            Batch batch = db.Batches.Find(id);
            if (SBVM == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "Name", batch.SubjectId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "FirstName", batch.TeacherId);
            return View(SBVM);
        }

        // POST: Batches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BatchViewModel studentBatchViewModel, Batch batch, BatchStudent batchStudent)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                List<BatchStudent> batchStudents = new List<BatchStudent>();
                batch.Name = studentBatchViewModel.Name;
                batch.SubjectId = studentBatchViewModel.SubjectId;
                batch.TeacherId = studentBatchViewModel.TeacherId;
                batch.Date = studentBatchViewModel.Date;
                batch.Time = studentBatchViewModel.Time;
                db.Entry(batch).State = EntityState.Modified;
                db.SaveChanges();
                int batchId = batch.Id;

                foreach (var item in studentBatchViewModel.AvailableStudents)
                {
                    if (item.IsChecked == true)
                    {
                        batchStudents.Add(new BatchStudent() { BatchId = batchId, StudentId = item.Id });
                    }
                }

                var DataBaseTable = db.BatchStudents.Where(a => a.BatchId == batchId).ToList();
                var ResultList = DataBaseTable.Except(batchStudents).ToList();
                foreach (var item in ResultList)
                {
                    db.BatchStudents.Remove(item);                   
                }
                db.SaveChanges();

                var GetStudentsId = db.BatchStudents.Where(a => a.BatchId == batchId).ToList();
                foreach (var item in batchStudents)
                {
                    if (!GetStudentsId.Contains(item))
                    {
                        db.BatchStudents.Add(item);
                        
                    }
                }
                db.SaveChanges();
/*
                db.Entry(batch).State = EntityState.Modified;
                db.SaveChanges();*/

                return RedirectToAction("Index");
            }
            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "Name", batch.SubjectId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "FirstName", batch.TeacherId);
            return View(batch);
        }

        // GET: Batches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Batch batch = db.Batches.Find(id);
            if (batch == null)
            {
                return HttpNotFound();
            }
            return View(batch);
        }

        // POST: Batches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            db.BatchStudents.RemoveRange(db.BatchStudents.Where(x => x.BatchId == id));
            Batch batch = db.Batches.Find(id);
            db.Batches.Remove(batch);
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
