using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.ViewModels;
using System.Data.Entity;



namespace WebApp.Controllers
{
    public class ReportController : Controller
    {
        private smsEntities db = new smsEntities();
        // GET: Report
        public ActionResult BatchStudent(int? batchId)
        {
            /*check session*/
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            /*check session*/

            Batch batch = new Batch();

            ViewBag.BatchId = new SelectList(db.Batches, "Id", "Name");
            //ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "FirstName");

            if (batchId != null && batchId > 0)
            {
                batch = db.Batches.Include(b => b.Subject).Include(b => b.Teacher).Where(w => w.Id == batchId).FirstOrDefault();
            }

            return View(batch);
        }

        // GET: Report
        public ActionResult BatchWiseTeacherStudent(int? batchId)
        {
            /*check session*/
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            /*check session*/
            Batch batch = new Batch();

            ViewBag.BatchId = new SelectList(db.Batches, "Id", "Name");
            //ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "FirstName");

            if (batchId != null && batchId > 0)
            {
                batch = db.Batches.Include(b => b.Subject).Include(b => b.Teacher).Where(w => w.Id == batchId).FirstOrDefault();
            }

            return View(batch);
        }

        // GET: Report
        public ActionResult WeeklyBatch(DateTime? start, DateTime? end)
        {
            /*check session*/
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            /*check session*/
            Batch batch = new Batch();

            ViewBag.BatchId = new SelectList(db.Batches, "Id", "Name");
            //ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "FirstName");
            if (start != null && end != null)
            {
                batch = db.Batches.Include(b => b.Subject).Include(b => b.Teacher).Where(x => x.Date >= start && x.Date <= end).FirstOrDefault();              
                if (batch == null)
                {
                    return HttpNotFound();
                }
            }


            return View(batch);
        }
    }
}