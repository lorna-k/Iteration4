using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApplicationPerformance.Models;

//Testing version controll

namespace ApplicationPerformance.Controllers
{
    public class AppraisalsController : Controller
    {
        private DatabasePMEntities db = new DatabasePMEntities();

        // GET: Appraisals
        public ActionResult Index()
        {
            return View(db.Appraisals.ToList());
        }

        //new comment 2.0

        // GET: Appraisals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appraisal appraisal = db.Appraisals.Find(id);
            if (appraisal == null)
            {
                return HttpNotFound();
            }
            return View(appraisal);
        }

        // GET: Appraisals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Appraisals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppraisalID,AppraisalEndDate,AppraisalStartDate,AppraisalStatus")] Appraisal appraisal)
        {
            if (ModelState.IsValid)
            {
                db.Appraisals.Add(appraisal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(appraisal);
        }

        // GET: Appraisals/Edit/5.
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appraisal appraisal = db.Appraisals.Find(id);
            if (appraisal == null)
            {
                return HttpNotFound();
            }
            return View(appraisal);
        }

        // POST: Appraisals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AppraisalID,AppraisalEndDate,AppraisalStartDate,AppraisalStatus")] Appraisal appraisal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appraisal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appraisal);
        }

        // GET: Appraisals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appraisal appraisal = db.Appraisals.Find(id);
            if (appraisal == null)
            {
                return HttpNotFound();
            }
            return View(appraisal);
        }

        // POST: Appraisals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appraisal appraisal = db.Appraisals.Find(id);
            db.Appraisals.Remove(appraisal);
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
