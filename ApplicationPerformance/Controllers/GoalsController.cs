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
    public class GoalsController : Controller
    {
        private DatabasePMEntities db = new DatabasePMEntities();



        #region Archived Goals
        public ActionResult Archives()
        {
            var archivedgoals =
                     from a in db.Goals //grabs all users currently in the database
                     select a;


            archivedgoals = archivedgoals.Where(a => a.Archive.ToString().Equals(ApplicationPerformance.Models.Archive.Archived.ToString()));
            return View(archivedgoals.ToList());
        }

        #endregion



        #region Error page
        public ActionResult NoItem()
        {
            return View();
        }
        #endregion


        #region Admin's Goal Management Page
        // GET: Goals for selected user
        public ActionResult Index(int? id)
        {
            return View(FindGoals(id).ToList());



        }


        #endregion



        #region History Goals

        public ActionResult History(string searchString, DateTime? start, DateTime? end)
        {
            //   SystemUser user = null;//creates a user object that shall be assigned values
            SystemUser user = (SystemUser)Session["CurrentUser"];

            ViewBag.ID = user.SystemUserID;


            List<Goal> appraisalGoals = null;//creates a list to store certain appraisal details
            var goals = db.Goals.Include(g => g.Appraisal).Include(g => g.Objective).Include(g => g.SystemUser);



            ViewBag.start = start;
            ViewBag.end = end;

            if (Session["User"] != null)
            {
                //  user = (SystemUser)Session["CurrentUser"];//assigns the logged in user's details
                //Grabs all the user's appraisals
                if (start == null || end == null)
                {

                    goals = goals.Where(a => a.SystemUser.UserName.ToUpper().Equals(user.UserName.ToUpper()));
                }
                else
                {
                    goals = goals
                                       .Where(x =>
                                        x.Appraisal.AppraisalEndDate >= start
                                       && x.Appraisal.AppraisalEndDate <= end
                                       && x.SystemUser.UserName.ToUpper().Equals(user.UserName.ToUpper()))
                                       .OrderByDescending(x => x.ObjectiveID)
                                       ;
                }

            }
            //changes the appraisal list from type var to type Appraisal so that it can be used later on
            appraisalGoals = goals.Cast<Goal>().ToList();

            //create a session variable to keep track of...
            Session["CurrentAppraisals"] = appraisalGoals;






            //searching functionality
            if (!String.IsNullOrEmpty(searchString)) //allowing to search using either first or last name
            {//The search string value is received from a text box that you'll add to the Index view.

                goals = goals.Where(a => /*a.Appraisal.AppraisalEndDate.ToString().Contains(searchString)   //filter by date*/
                                         a.Objective.Title.ToString().ToUpper().Contains(searchString)
                                        || a.Objective.ObjectiveType.ToString().ToUpper().Contains(searchString));
            }



            return View(goals.ToList());
        }



        #endregion



        #region Complete and Incomplete List
        public ActionResult Complete()
        {
            CompleteList();
            return View();
        }

        public List<string> CompleteList()
        {
            SystemUser user = (SystemUser)Session["CurrentUser"];
            var goals = FindGoals(user.SystemUserID);
            int completeGoalCount = 0;

            List<string> completeGoalList = new List<string>();
            foreach (var item in goals)
            {
                if (item.GoalStatus.ToString().ToUpper() == "COMPLETE")
                {
                    if (item.Appraisal.AppraisalEndDate >= DateTime.Now.Date)
                    {
                        completeGoalCount += 1;
                        completeGoalList.Add(item.Objective.Title);
                    }
                }
            }


            //  int completeGoalCount = goalComplete.Count();



            //ViewBag.NumberOfCompleteGoals = completeGoalCount;
            //   Session["CompleteGoals"] = completeGoalCount;
            ViewBag.ListOfCompleteGoals = completeGoalList;
            return completeGoalList;
        }




        public List<string> InCompleteList()
        {
            SystemUser user = (SystemUser)Session["CurrentUser"];
            var goals = FindGoals(user.SystemUserID);
            int incompleteGoalCount = 0;

            List<string> incompleteGoalList = new List<string>();
            foreach (var item in goals)
            {
                if (item.GoalStatus.ToString().ToUpper() != "COMPLETE" || item.GoalStatus.ToString().ToUpper() == null)
                {
                    if (item.Appraisal.AppraisalEndDate >= DateTime.Now.Date)
                    {
                        incompleteGoalCount += 1;
                        incompleteGoalList.Add(item.Objective.Title);
                    }
                }
            }


            //  int completeGoalCount = goalComplete.Count();



            //  ViewBag.NumberOfCompleteGoals = incompleteGoalCount;
            // Session["CompleteGoals"] = incompleteGoalCount;
            ViewBag.ListOfCompleteGoals = incompleteGoalList;
            return incompleteGoalList;
        }

        #endregion




        #region Display Employee's Objectives


        public IQueryable<Goal> FindGoals(int? id)
        {
            if (id == null)
            {
                RedirectToAction("NoItem", "Goals");//remember to add controller for this view


            }

            SystemUser user = db.SystemUsers.Find(id); //creates a user object that shall be assigned values
            Session["CurrentUser"] = user;
            if (user == null)
            {
                RedirectToAction("NoItem", "Goals");
            }


            ViewBag.LastName = user.LastName;
            ViewBag.FirstName = user.FirstName;
            ViewBag.ID = user.SystemUserID;




            if (user.SystemUserImage == null)
            {
                ViewBag.Image = "~/Content/ProfileImages/default-image.png";
            }
            else
            {
                ViewBag.Image = user.SystemUserImage;
            }

            var assignedManager =
                       from a in db.SystemUsers //grabs all users currently in the database
                       select a;


            assignedManager = assignedManager.Where(a => a.LastName.ToUpper().Equals(user.AssignedManager.ToUpper()));

            List<SystemUser> assignedManagers = assignedManager.Cast<SystemUser>().ToList();
            SystemUser AssignedManagerUser = assignedManagers.First();

            ViewBag.AssignedManagerID = AssignedManagerUser.SystemUserID;



            var goals = db.Goals.Include(g => g.Appraisal).Include(g => g.Objective).Include(g => g.SystemUser);


            goals = goals.Where(a => a.SystemUserID.ToString().Equals(user.SystemUserID.ToString()));

            return goals;

        }
        public ActionResult SelectedEmployee(int? id, string searchString)
        {


            List<Goal> appraisalGoals = null;//creates a list to store certain appraisal details


            //changes the appraisal list from type var to type Appraisal so that it can be used later on

            var goals = FindGoals(id);

            appraisalGoals = goals.Cast<Goal>().ToList(); ;
            //create a session variable to keep track of...
            Session["CurrentAppraisals"] = appraisalGoals;


            int count = 0;


            foreach (var item in goals)
            {

                if (item.Appraisal.AppraisalEndDate >= DateTime.Now.Date)
                {
                    count += 1;
                }

            }
            ViewBag.NumberOfGoals = count;
            Session["NumberOfGoals"] = count;



            int completeGoalCount = 0;

            foreach (var item in goals)
            {
                if (item.GoalStatus.ToString().ToUpper() == "COMPLETE")
                {
                    if (item.Appraisal.AppraisalEndDate >= DateTime.Now.Date)
                    {
                        completeGoalCount += 1;
                    }
                }
            }






            ViewBag.NumberOfCompleteGoals = completeGoalCount;
            Session["CompleteGoals"] = completeGoalCount;



            int incomplete = count - completeGoalCount;
            Session["InCompleteGoals"] = incomplete;
            ViewBag.OngoingGoals = incomplete;






            //Maneger has not signed off 
            int managerApproval = 0;

            foreach (var item in goals)
            {
                if (item.ManagerApproval.ToString().ToUpper() != "APPROVED")
                {
                    if (item.Appraisal.AppraisalEndDate >= DateTime.Now.Date)
                    {
                        managerApproval += 1;
                    }
                }
            }

            ViewBag.ManNotApproved = managerApproval;
            Session["ManNotApproved"] = managerApproval;

            //Employee has not signed off 
            int employeeApproval = 0;

            foreach (var item in goals)
            {
                if (item.EmployeeApproval.ToString().ToUpper() != "APPROVED")
                {
                    if (item.Appraisal.AppraisalEndDate >= DateTime.Now.Date)
                    {
                        employeeApproval += 1;
                    }
                }
            }

            ViewBag.EmpNotApproved = employeeApproval;
            Session["EmpNotApproved"] = employeeApproval;




            //Comments notifications 


            int employeeCommentCount = 0;


            foreach (var item in goals)
            {
                if (item.Appraisal.AppraisalEndDate >= DateTime.Now.Date)


                {
                    if (item.EmployeeComment == null)
                    {

                        employeeCommentCount += 0;
                    }
                    else
                    {
                        if (item.EmployeeComment.ToString() != "")
                        {
                            employeeCommentCount += 1;
                        }
                    }
                }

            }
            ViewBag.NumberOfEmpComments = employeeCommentCount;
            Session["NumberOfEmpComments"] = employeeCommentCount;




            int managerCommentCount = 0;


            foreach (var item in goals)
            {

                if (item.Appraisal.AppraisalEndDate >= DateTime.Now.Date)


                {
                    if (item.ManagerComment == null)
                    {

                        managerCommentCount += 0;
                    }
                    else
                    {
                        if (item.ManagerComment.ToString() != "")
                        {
                            managerCommentCount += 1;
                        }
                    }
                }

            }
            ViewBag.NumberOfManComments = managerCommentCount;
            Session["NumberOfManComments"] = managerCommentCount;


            //searching functionality
            if (!String.IsNullOrEmpty(searchString)) //allowing to search using either first or last name
            {//The search string value is received from a text box that you'll add to the Index view.
                goals = goals.Where(a => a.GoalStatus.ToString().ToUpper().Contains(searchString)
                                       || a.Objective.Title.ToString().ToUpper().Contains(searchString)
                                       || a.Objective.ObjectiveType.ToString().ToUpper().Contains(searchString));
            }

            return View(goals.ToList());
        }

        #endregion




        #region create goals methods


        // GET: Goals/CreateUnique
        public ActionResult Create()
        {
            SystemUser user = (SystemUser)Session["CurrentUser"];//assigns a user to the current user
            ViewBag.ID = user.SystemUserID;

            ViewBag.AppraisalID = new SelectList(db.Appraisals, "AppraisalID", "AppraisalEndDate");
            ViewBag.ObjectiveID = new SelectList(db.Objectives, "ObjectiveID", "Title");
            ViewBag.SystemUserID = new SelectList(db.SystemUsers, "SystemUserID", "LastName");
            return View();
        }

        // POST: Goals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GoalID,Weight,ManagerComment,EmployeeComment,ObjectiveID,SystemUserID,AppraisalID,GoalStatus")] Goal goal)
        {

            SystemUser user = (SystemUser)Session["CurrentUser"];//assigns a user to the current user
            goal.SystemUserID = user.SystemUserID;
            if (ModelState.IsValid)
            {
                db.Goals.Add(goal);
                db.SaveChanges();
                return RedirectToAction("SelectedEmployee", new { id = user.SystemUserID });
            }
            ViewBag.ID = user.SystemUserID;
            ViewBag.AppraisalID = new SelectList(db.Appraisals, "AppraisalID", "AppraisalEndDate", goal.AppraisalID);
            ViewBag.ObjectiveID = new SelectList(db.Objectives, "ObjectiveID", "Title", goal.ObjectiveID);
            ViewBag.SystemUserID = new SelectList(db.SystemUsers, "SystemUserID", "LastName", goal.SystemUserID);
            return View(goal);
        }





        // GET: Goals/CreateUnique
        public ActionResult CreateUnique()
        {
            SystemUser user = (SystemUser)Session["CurrentUser"];//assigns a user to the current user
            ViewBag.ID = user.SystemUserID;

            ViewBag.AppraisalID = new SelectList(db.Appraisals, "AppraisalID", "AppraisalEndDate");
            ViewBag.ObjectiveID = new SelectList(db.Objectives, "ObjectiveID", "Title");
            ViewBag.SystemUserID = new SelectList(db.SystemUsers, "SystemUserID", "LastName");
            return View();
        }

        // POST: Goals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUnique([Bind(Include = "GoalID,Weight,ManagerComment,EmployeeComment,ObjectiveID,SystemUserID,AppraisalID,GoalStatus,ObjectiveDescription,Title,ObjectiveType,Confidentiality")]Objective objective, Goal goal)
        {

            SystemUser user = (SystemUser)Session["CurrentUser"];//assigns a user to the current user
            goal.SystemUserID = user.SystemUserID;
            if (ModelState.IsValid)
            {
                db.Goals.Add(goal);
                db.SaveChanges();
                return RedirectToAction("SelectedEmployee", new { id = user.SystemUserID });
            }

            ViewBag.ID = user.SystemUserID;
            ViewBag.AppraisalID = new SelectList(db.Appraisals, "AppraisalID", "AppraisalEndDate", goal.AppraisalID);
            ViewBag.ObjectiveID = new SelectList(db.Objectives, "ObjectiveID", "Title", goal.ObjectiveID);
            ViewBag.SystemUserID = new SelectList(db.SystemUsers, "SystemUserID", "LastName", goal.SystemUserID);
            return View(goal);
        }




        // GET: Goals/CreateUnique
        public ActionResult CreateMultiple()
        {

            ViewBag.AppraisalID = new SelectList(db.Appraisals, "AppraisalID", "AppraisalEndDate");
            var objectives = db.Objectives.Select(c => new {
                ObjectiveID = c.ObjectiveID,
                ObjectiveTitle = c.Title
            }).ToList();
            ViewBag.ObjectiveID = new MultiSelectList(objectives, "ObjectiveID", "ObjectiveTitle");




            //comment here
            var users = //returns all the employees currently stored in the database
                   from a in db.SystemUsers
                   select a;
            //user comparing the employees list to, has the value from the login
            SystemUser user = (SystemUser)Session["CurrentUser"];//assigns a user to the current user

            users = users.Where(a => a.AssignedManager.ToUpper().Equals(user.LastName.ToUpper()));





            var systemUsers = db.SystemUsers.Select(c => new {
                SystemUserID = c.SystemUserID,
                LastName = c.FirstName + " " + c.LastName
            }).ToList();


            ViewBag.SystemUserID = new MultiSelectList(systemUsers, "SystemUserID", "LastName");
            return View();
        }

        // POST: Goals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMultiple(int[] systemUserID, int[] objectiveID, [Bind(Include = "GoalID,Weight,ManagerComment,EmployeeComment,ObjectiveID,SystemUserID,AppraisalID,GoalStatus")] Goal goal)
        {
            //    [Bind(Include = "GoalID,Weight,ManagerComment,EmployeeComment,ObjectiveID,SystemUserID,AppraisalID,GoalStatus")]
            //Goal goal,

            // SystemUser user = (SystemUser)Session["CurrentUser"];//assigns a user to the current user
            for (int z = 0; z < systemUserID.Count(); z++)
            {
                goal.SystemUserID = systemUserID.ElementAt(z); // user.SystemUserID;
                for (int a = 0; a < objectiveID.Count(); a++)

                {

                    goal.ObjectiveID = objectiveID.ElementAt(a);
                    if (ModelState.IsValid)
                    {
                        db.Goals.Add(goal);
                        db.SaveChanges();
                        //return RedirectToAction("SelectedEmployee", new { id = 56/*user.SystemUserID*/ });
                    }

                }
            }

            if (ModelState.IsValid)
            {

                return RedirectToAction("SelectedEmployee", new { id = 56/*user.SystemUserID*/ });
            }

            var objectives = db.Objectives.Select(c => new {
                ObjectiveID = c.ObjectiveID,
                ObjectiveTitle = c.Title
            }).ToList();
            ViewBag.ObjectiveID = new MultiSelectList(objectives, "ObjectiveID", "ObjectiveTitle");
            ViewBag.AppraisalID = new SelectList(db.Appraisals, "AppraisalID", "AppraisalEndDate", goal.AppraisalID);
            //  ViewBag.ObjectiveID = new MultiSelectList(db.Objectives, "ObjectiveID", "Title", goal.ObjectiveID);

            //comment here
            var users = //returns all the employees currently stored in the database
                   from a in db.SystemUsers
                   select a;
            //user comparing the employees list to, has the value from the login
            SystemUser user = (SystemUser)Session["CurrentUser"];//assigns a user to the current user



            users = users.Where(a => a.AssignedManager.ToUpper().Equals(user.LastName.ToUpper()));




            var systemUsers = users.Select(c => new {
                SystemUserID = c.SystemUserID,
                LastName = c.FirstName + " " + c.LastName
            }).ToList();


            ViewBag.SystemUserID = new MultiSelectList(systemUsers, "SystemUserID", "LastName");

            //            ViewBag.SystemUserID = new SelectList(db.SystemUsers, "SystemUserID", "LastName", goal.SystemUserID);
            return View(goal);
        }






        #endregion



        #region Edit goals

        // GET: Goals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Goal goal = db.Goals.Find(id);
            if (goal == null)
            {
                return HttpNotFound();
            }

            SystemUser user = (SystemUser)Session["CurrentUser"];//assigns a user to the current user
            if (user == null)
            {
                user = db.SystemUsers.Find(goal.SystemUserID);
            }
            ViewBag.ID = user.SystemUserID;
            ViewBag.AppraisalID = new SelectList(db.Appraisals, "AppraisalID", "AppraisalEndDate", goal.AppraisalID);
            ViewBag.ObjectiveID = new SelectList(db.Objectives, "ObjectiveID", "Title", goal.ObjectiveID);
            ViewBag.SystemUserID = new SelectList(db.SystemUsers, "SystemUserID", "LastName", goal.SystemUserID);
            return View(goal);
        }

        // POST: Goals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GoalID,Weight,ManagerComment,EmployeeComment,ObjectiveID,SystemUserID,AppraisalID,GoalStatus,ManagerApproval,EmployeeApproval")] Goal goal)
        {
            SystemUser user = (SystemUser)Session["CurrentUser"];//assigns a user to the current user
            if (user == null)
            {
                user = db.SystemUsers.Find(goal.SystemUserID);
            }
            goal.SystemUserID = user.SystemUserID;
            if (ModelState.IsValid)
            {
                db.Entry(goal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SelectedEmployee", new { id = user.SystemUserID });
            }


            ViewBag.ID = user.SystemUserID;
            ViewBag.AppraisalID = new SelectList(db.Appraisals, "AppraisalID", "AppraisalEndDate", goal.AppraisalID);
            ViewBag.ObjectiveID = new SelectList(db.Objectives, "ObjectiveID", "Title", goal.ObjectiveID);
            ViewBag.SystemUserID = new SelectList(db.SystemUsers, "SystemUserID", "LastName", goal.SystemUserID);
            return View(goal);
        }


        #endregion



        #region Delete Goals
        // GET: Goals/Delete/5
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Goal goal = db.Goals.Find(id);
            if (goal == null)
            {
                return HttpNotFound();
            }
            return View(goal);
        }

        // POST: Goals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            SystemUser user = (SystemUser)Session["CurrentUser"];

            ViewBag.ID = user.SystemUserID;

            foreach (var item in db.Goals)
            {
                if (item.Appraisal.AppraisalEndDate < DateTime.Now.Date && item.GoalStatus == ApplicationPerformance.Models.GoalStatus.Complete)
                {
                    if (item.Archive != ApplicationPerformance.Models.Archive.HistoryArchive)
                    {
                        item.Archive = ApplicationPerformance.Models.Archive.HistoryArchive;
                        db.Entry(item).State = EntityState.Modified;

                    }
                }
                if (item.Appraisal.AppraisalEndDate != null)
                {
                    DateTime currentDate = (DateTime)item.Appraisal.AppraisalEndDate;
                    DateTime futureDate = currentDate.AddYears(5);

                    if (DateTime.Now > futureDate)
                    {
                        Goal goal = db.Goals.Find(id);
                        db.Goals.Remove(goal);
                    }
                }
                else
                {

                    item.Archive = ApplicationPerformance.Models.Archive.Archived;
                    db.Entry(item).State = EntityState.Modified;
                }

            }
            //   Goal goal = db.Goals.Find(id);


            // db.Goals.Remove(goal);
            db.SaveChanges();
            return RedirectToAction("SelectedEmployee", new { id = user.SystemUserID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        #endregion




        #region approve goals methods

        public void ManagerApproveAll()
        {
            SystemUser manager = (SystemUser)Session["manager"];
            SystemUser user = (SystemUser)Session["CurrentUser"];
            var goals =
                      from a in db.Goals //grabs all users currently in the database
                      select a;
            goals = goals.Where(a => a.SystemUser.AssignedManager.ToUpper().Equals(manager.LastName.ToUpper())
            && a.ManagerApproval.ToString().ToUpper() != "APPROVED"
            && a.SystemUser.SystemUserID.Equals(user.SystemUserID)

                    );

            foreach (var item in goals)
            {
                item.ManagerApproval = ApplicationPerformance.Models.ManagerApproval.Approved;
                db.Entry(item).State = EntityState.Modified;
            }
            db.SaveChanges();
        }

        public ActionResult ManagerApproves()
        {
            SystemUser user = (SystemUser)Session["CurrentUser"];
            ManagerApproveAll();
            return RedirectToAction("SelectedEmployee", new { id = user.SystemUserID });
        }




        public void EmployeeApproveAll()
        {
            SystemUser manager = (SystemUser)Session["manager"];
            SystemUser user = (SystemUser)Session["CurrentUser"];
            var goals =
                      from a in db.Goals //grabs all users currently in the database
                      select a;
            goals = goals.Where(a => a.SystemUser.AssignedManager.ToUpper().Equals(manager.LastName.ToUpper())
            && a.EmployeeApproval.ToString().ToUpper() != "APPROVED"
            && a.SystemUser.SystemUserID.Equals(user.SystemUserID)

                    );

            foreach (var item in goals)
            {
                item.EmployeeApproval = ApplicationPerformance.Models.EmployeeApproval.Approved;
                db.Entry(item).State = EntityState.Modified;
            }
            db.SaveChanges();
        }

        public ActionResult EmployeeApproves()
        {
            SystemUser user = (SystemUser)Session["CurrentUser"];
            ManagerApproveAll();
            return RedirectToAction("SelectedEmployee", new { id = user.SystemUserID });
        }


        #endregion




        // GET: Goals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Goal goal = db.Goals.Find(id);
            if (goal == null)
            {
                return HttpNotFound();
            }
            return View(goal);
        }

    }
}
