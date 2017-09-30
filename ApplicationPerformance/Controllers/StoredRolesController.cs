using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApplicationPerformance.Models;

namespace ApplicationPerformance.Controllers
{
    public class StoredRolesController : Controller
    {
        private DatabasePMEntities db = new DatabasePMEntities();

        // GET: StoredRoles
        public ActionResult Index()
        {
            return View(db.StoredRoles.ToList());
        }

        // GET: StoredRoles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoredRole storedRole = db.StoredRoles.Find(id);
            if (storedRole == null)
            {
                return HttpNotFound();
            }
            return View(storedRole);
        }

        // GET: StoredRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StoredRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StoredRoleID,RoleName")] StoredRole storedRole)
        {
            if (ModelState.IsValid)
            {
                db.StoredRoles.Add(storedRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(storedRole);
        }

        // GET: StoredRoles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoredRole storedRole = db.StoredRoles.Find(id);
            if (storedRole == null)
            {
                return HttpNotFound();
            }
            return View(storedRole);
        }

        // POST: StoredRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StoredRoleID,RoleName")] StoredRole storedRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(storedRole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(storedRole);
        }

        // GET: StoredRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoredRole storedRole = db.StoredRoles.Find(id);
            if (storedRole == null)
            {
                return HttpNotFound();
            }
            return View(storedRole);
        }

        // POST: StoredRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StoredRole storedRole = db.StoredRoles.Find(id);
            db.StoredRoles.Remove(storedRole);
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
