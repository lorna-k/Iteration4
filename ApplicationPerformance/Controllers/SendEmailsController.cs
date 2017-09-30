using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApplicationPerformance.Models;
using System.Web.Helpers;

namespace ApplicationPerformance.Controllers
{
    public class SendEmailsController : Controller
    {
        private DatabasePMEntities db = new DatabasePMEntities();

        // GET: SendEmails
        public ActionResult Index()
        {
            return View(db.SendEmails.ToList());
        }

        // GET: SendEmails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SendEmail sendEmail = db.SendEmails.Find(id);
            if (sendEmail == null)
            {
                return HttpNotFound();
            }
            return View(sendEmail);
        }

        // GET: SendEmails/Create
        public ActionResult Create()
        {
            
            return View();
        }



        // POST: SendEmails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SendEmail obj)
        {
            SystemUser user = (SystemUser)Session["CurrentUser"];
            try
            {
                //Configuring webMail class to send emails  
                //gmail smtp server  
                WebMail.SmtpServer = "smtp.gmail.com";
                //gmail port to send emails  
                WebMail.SmtpPort = 587;
                WebMail.SmtpUseDefaultCredentials = true;
                //sending emails with secure protocol  
                WebMail.EnableSsl = true;
                //EmailId used to send emails from application  
                WebMail.UserName = "olanyacolombus@gmail.com";
                WebMail.Password = "ky0b3r0nald";

                //Sender email address.  
                WebMail.From = "SenderGamilId@gmail.com";

                //Send email  
                obj.ToEmail = user.Email;
                WebMail.Send(to: obj.ToEmail, subject: obj.EmailSubject, body: obj.EmailBody, cc: obj.EmailCC, bcc: obj.EmailBCC, isBodyHtml: true);
                ViewBag.Status = "Email Sent Successfully.";
            }
            catch (Exception)
            {
                ViewBag.Status = "Problem while sending email, Please check details.";

            }
            return View();
        }






        // GET: SendEmails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SendEmail sendEmail = db.SendEmails.Find(id);
            if (sendEmail == null)
            {
                return HttpNotFound();
            }
            return View(sendEmail);
        }

        // POST: SendEmails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ToEmail,EmailBody,EmailSubject,EmailCC,EmailBCC")] SendEmail sendEmail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sendEmail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sendEmail);
        }

        // GET: SendEmails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SendEmail sendEmail = db.SendEmails.Find(id);
            if (sendEmail == null)
            {
                return HttpNotFound();
            }
            return View(sendEmail);
        }

        // POST: SendEmails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SendEmail sendEmail = db.SendEmails.Find(id);
            db.SendEmails.Remove(sendEmail);
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
