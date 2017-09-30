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
    public class RolesController : Controller
    {
        private DatabasePMEntities db = new DatabasePMEntities();

        // GET: Roles
        public ActionResult Index(string searchString)
        {
            var roles = db.Roles.Include(r => r.StoredRole).Include(r => r.SystemUser);

            if (!String.IsNullOrEmpty(searchString)) //allowing to search using either first or last name
            {//The search string value is received from a text box that you'll add to the Index view.
                roles = roles.Where(a => a.SystemUser.LastName.ToUpper().Contains(searchString)
                                       || a.StoredRole.RoleName.ToUpper().Contains(searchString)
                                       || a.SystemUser.FirstName.ToUpper().Contains(searchString));
            }

            return View(roles.ToList());
        }

        // GET: Roles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return PartialView(role);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            ViewBag.StoredRoleID = new SelectList(db.StoredRoles, "StoredRoleID", "RoleName");
            ViewBag.SystemUserID = new SelectList(db.SystemUsers, "SystemUserID", "LastName");
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoleID,SystemUserID,StoredRoleID")] Role role)
        {
            if (ModelState.IsValid)
            {
                db.Roles.Add(role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StoredRoleID = new SelectList(db.StoredRoles, "StoredRoleID", "RoleName", role.StoredRoleID);
            ViewBag.SystemUserID = new SelectList(db.SystemUsers, "SystemUserID", "LastName", role.SystemUserID);
            return View(role);
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            ViewBag.StoredRoleID = new SelectList(db.StoredRoles, "StoredRoleID", "RoleName", role.StoredRoleID);
            ViewBag.SystemUserID = new SelectList(db.SystemUsers, "SystemUserID", "LastName", role.SystemUserID);
            return View(role);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoleID,SystemUserID,StoredRoleID")] Role role)
        {
            if (ModelState.IsValid)
            {
                db.Entry(role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StoredRoleID = new SelectList(db.StoredRoles, "StoredRoleID", "RoleName", role.StoredRoleID);
            ViewBag.SystemUserID = new SelectList(db.SystemUsers, "SystemUserID", "LastName", role.SystemUserID);
            return View(role);
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Role role = db.Roles.Find(id);
            db.Roles.Remove(role);
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
