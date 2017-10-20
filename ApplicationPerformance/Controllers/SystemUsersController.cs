using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApplicationPerformance.Models;
using System.Web.Security;
using System.IO;

namespace ApplicationPerformance.Controllers
{
    public class SystemUsersController : Controller
    {
        private DatabasePMEntities db = new DatabasePMEntities();

        private int totalcompleteGoals;
        private int totalincompleteGoals;
        private int totalnotapprovedGoals;


        #region Properties
        public int TotalcompleteGoals
        {
            get
            {
                return totalcompleteGoals;
            }

            set
            {
                totalcompleteGoals = value;
            }
        }

        public int TotalincompleteGoals
        {
            get
            {
                return totalincompleteGoals;
            }

            set
            {
                totalincompleteGoals = value;
            }
        }

        public int TotalnotapprovedGoals
        {
            get
            {
                return totalnotapprovedGoals;
            }

            set
            {
                totalnotapprovedGoals = value;
            }
        }
        #endregion

        /*Method Flow
         Login Get-- Login Post (uses isValid)
            RolesSelect Get-- RolesSelect Post
                if admin -- Index
                if manager-- ManagersEmployees
                if employee-- SelectedEmployee (Goals)*/


        #region Log in and out methods

        /**Login Get Method**/
        [HttpGet]
        public ActionResult LogIn()
        {/*When the user attempts to use the application, 
            the first thing they encounter is a login page
            The get method determines what view the user sees when the 
            page is first loaded
            The View it returns is under Views/SystemUsers and is called
            LogIn.
            */

            //the for statement below makes sure that objectives that exceed the archive date are placed in archive history
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
                        Goal goal = db.Goals.Find(item.GoalID);
                        db.Goals.Remove(goal);
                    }
                }
                else
                {

                    item.Archive = ApplicationPerformance.Models.Archive.Archived;
                    db.Entry(item).State = EntityState.Modified;
                }

            }
            return View();
        }






        /**Login Post Method**/
        [HttpPost]
        public ActionResult LogIn(ApplicationPerformance.Models.SystemUser user)
        {/*The post method allows the user to get feedback for example
            when a button is pressed. */

            Session["AdminCheck"] = null;

            if (ModelState.IsValid)//checks if the you have any model errors
            {
                if (isValid(user.UserName, user.Password))/*checks to see that the 
                                                                username and password
                                                                entered acually exist*/
                {

                    FormsAuthentication.SetAuthCookie(user.UserName, false);/*what keeps the user logged 
                                                            in whenever they want to access the page,
                                                            should usually be done only with the remember
                                                            me functionality but this is yet to be
                                                            implemented*/


                    Session["User"] = user;/*creates a session that allows us to set privileges
                                                  based on the user who has logged in*/


                    SystemUser selectedUser = null;  //create a user that we will give values further down
                    SystemUser newUser = null;
                    List<SystemUser> newUsers = null;//creates a list of users 

                    //get list of all employees
                    var employees = //returns all the employees currently stored in the database
                                       from a in db.SystemUsers
                                       select a;
                    //user comparing the employees list to, has the value from the login
                    selectedUser = (SystemUser)Session["User"];

                    if (Session["User"] != null)//if the user has in fact logged in
                    {
                        /*Find the employee from the database where the username is the same as the 
                        logged in user's username*/
                        employees = employees.Where(a => a.UserName.ToUpper().Equals(selectedUser.UserName.ToUpper()));

                        //converts the var list to a list of type System Object.
                        newUsers = employees.Cast<SystemUser>().ToList();


                        newUser = newUsers.First();
                        int numberOfRoles = newUser.Roles.Count();

                        if (numberOfRoles > 1)/*If more than one role, direct to page
                                                which allows the user to select
                                                their roles*/
                        {

                            return RedirectToAction("RolesSelect", "SystemUsers");/*page with button for each
                                                                                       assigned role */


                        }
                        else  //otherwise if the user has only one role
                        {


                            bool containsAdmin = newUser.Roles.Any(p => p.StoredRoleID == 1);
                            if (containsAdmin)
                            {/*the redirect method will include, the name of the controller method, then the 
                                the controller class the method's in and then parameters if the method
                                takes any parameters*/
                                Session["admin"] = newUser;
                                return RedirectToAction("Index", "SystemUsers");/*admin page*/
                            }

                            bool containsManager = newUser.Roles.Any(p => p.StoredRoleID == 3);
                            if (containsManager)
                            {
                                Session["manager"] = newUser;
                                return RedirectToAction("ManagerEmployees", "SystemUsers", new { id = newUser.SystemUserID });  /*Redirect to the manager
                                                                                                            page which shows manager details
                                                                                                            and employees under than manager*/
                            }

                            bool containsEmployee = newUser.Roles.Any(p => p.StoredRoleID == 2);

                            if (containsEmployee)
                            {

                                Session["employee"] = newUser;
                                return RedirectToAction("SelectedEmployee", "Goals", new { id = newUser.SystemUserID });

                            }
                        }
                    }


                }
                else //if the login issues were encountered. 
                {
                    ModelState.AddModelError("", "Login Data is incorrect");
                }
            }

            return View(user);
        }





        /**Checks if the username and password actually exist in the system**/
        private bool isValid(string username, string password)
        {
            var crypto = new SimpleCrypto.PBKDF2();

            bool isValid = false;
            using (var db = new DatabasePMEntities())
            {
                var user = db.SystemUsers.FirstOrDefault(u => u.UserName == username);
                if (user != null)
                {
                    if (user.Password == crypto.Compute(password, user.PasswordSalt))
                    {

                        isValid = true;

                    }
                }
            }
            return isValid;
        }




        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("LogIn", "SystemUsers");
        }



        #endregion



        #region Select Role methods


        /**Select your role Get Method**/
        [HttpGet]
        public ActionResult RolesSelect()
        {
            SystemUser selectedUser = null;
            SystemUser newUser = null;
            List<SystemUser> newUsers = null;


            //get list of all employees
            var employees = //db.Users.Include(a => a.LastName);
                               from a in db.SystemUsers
                               select a;


            //user comparing the var list to, has the value from the login
            selectedUser = (SystemUser)Session["User"];
            //   Session["EmployeeUser"] = user;
            if (Session["User"] != null)
            {

                employees = employees.Where(a => a.UserName.ToUpper().Equals(selectedUser.UserName.ToUpper()));

                newUsers = employees.Cast<SystemUser>().ToList();

                newUser = newUsers.First();


                bool containsAdmin = newUser.Roles.Any(p => p.StoredRoleID == 1);
                bool containsEmployee = newUser.Roles.Any(p => p.StoredRoleID == 2);
                bool containsManager = newUser.Roles.Any(p => p.StoredRoleID == 3);



                if (containsAdmin)
                {
                    Session["admin"] = newUser;
                }


                if (containsManager)
                {
                    Session["manager"] = newUser;
                }


                if (containsEmployee)
                {
                    Session["employee"] = newUser;
                }

            }


            return View();
        }



        /**Select your role Post Method**/
        [HttpPost]
        public ActionResult RolesSelect(ApplicationPerformance.Models.SystemUser user, string button)
        {
            SystemUser selectedUser = null;//create a user that we will give values further down
            SystemUser newUser = null;
            List<SystemUser> newUsers = null;//creates a list of users 
                                             //get list of all employees
            var employees = //returns all the employees currently stored in the database
                               from a in db.SystemUsers
                               select a;
            //user comparing the employees list to, has the value from the login
            selectedUser = (SystemUser)Session["User"];
            employees = employees.Where(a => a.UserName.ToUpper().Equals(selectedUser.UserName.ToUpper()));

            //converts the var list to a list of type System Object.
            newUsers = employees.Cast<SystemUser>().ToList();

            newUser = newUsers.First();



            if (ModelState.IsValid)
            {
                if (button == "1")
                {

                    Session["admin"] = newUser;

                    return RedirectToAction("Index", "SystemUsers");
                }
                else if (button == "2")
                {

                    Session["manager"] = newUser;
                    //  Session["ChosenManager"] = user;
                    return RedirectToAction("ManagerEmployees", "SystemUsers", new { id = newUser.SystemUserID });
                }
                else if (button == "3")
                {
                    Session["employee"] = newUser;

                    return RedirectToAction("SelectedEmployee", "Goals", new { id = newUser.SystemUserID });


                }
                else
                {
                    ModelState.AddModelError("", "Incorrect role selected");
                }


            }
            else
            {
                ModelState.AddModelError("", "Incorrect role selected");
            }


            return View(user);

        }


        #endregion






        #region Manager notifications methods


        public ActionResult Approval() //Shows list of objectives that haven't been approved by a specific manager
        {


            return View(ApprovalGoals().ToList());
        }



        public IQueryable<Goal> ApprovalGoals() //Shows list of objectives that haven't been approved by a specific manager
        {
            SystemUser user = (SystemUser)Session["manager"];


            DateTime currentDate = DateTime.Now;
            //  IncompleteList();
            var goals =
                  from a in db.Goals //grabs all users currently in the database
                  select a;
            //goals = goals.Where(a => a.SystemUser.AssignedManager.ToUpper().Equals(user.LastName.ToUpper()));
            goals = goals.Where(a => a.SystemUser.AssignedManager.ToUpper().Equals(user.LastName.ToUpper())
            && a.ManagerApproval.ToString().ToUpper() != "APPROVED"
            && a.Appraisal.AppraisalEndDate >= currentDate
            );


            TotalnotapprovedGoals = goals.Count();
            return goals;
        }




        public ActionResult Incomplete() //shows list of incomplete objectives of all employees under a specific manager
        {


            return View(IncompleteGoals().ToList());

        }

        public IQueryable<Goal> IncompleteGoals()
        {
            SystemUser user = (SystemUser)Session["manager"];


            DateTime currentDate = DateTime.Now;
            //  IncompleteList();
            var goals =
                  from a in db.Goals //grabs all users currently in the database
                  select a;
            //goals = goals.Where(a => a.SystemUser.AssignedManager.ToUpper().Equals(user.LastName.ToUpper()));
            goals = goals.Where(a => a.SystemUser.AssignedManager.ToUpper().Equals(user.LastName.ToUpper())
            && a.GoalStatus.ToString().ToUpper() != "COMPLETE"
            && a.Appraisal.AppraisalEndDate >= currentDate
            );
            TotalincompleteGoals = goals.Count();
            return goals;
        }




        public ActionResult Complete() //shows list of incomplete objectives of all employees under a specific manager
        {


            return View(CompleteGoals().ToList());
        }




        public IQueryable<Goal> CompleteGoals() //shows list of incomplete objectives of all employees under a specific manager
        {
            SystemUser user = (SystemUser)Session["manager"];


            DateTime currentDate = DateTime.Now;
            //  IncompleteList();
            var goals =
                  from a in db.Goals //grabs all users currently in the database
                  select a;
            //goals = goals.Where(a => a.SystemUser.AssignedManager.ToUpper().Equals(user.LastName.ToUpper()));
            goals = goals.Where(a => a.SystemUser.AssignedManager.ToUpper().Equals(user.LastName.ToUpper())
            && a.GoalStatus.ToString().ToUpper() == "COMPLETE"
            && a.Appraisal.AppraisalEndDate >= currentDate
            );


            TotalcompleteGoals = goals.Count();
            return goals;
        }



        #endregion


        #region Manager Page methods

        public ActionResult ManagerEmployees(int? id, string searchString)
        {
            IncompleteGoals();
            ApprovalGoals();
            CompleteGoals();
            List<SystemUser> newUsers = null;//creates a list of users 
                                             // SystemUser user = null;//creates a user object that shall be assigned values
            var employee =
                       from a in db.SystemUsers //grabs all users currently in the database
                       select a;
            var selectedEmployee =
                       from b in db.SystemUsers //grabs all users currently in the database
                       select b;
            SystemUser user = db.SystemUsers.Find(id);//assigns the logged in user's details

            ViewBag.ManagerLastName = user.LastName;
            ViewBag.ManagerFirstName = user.FirstName;
            ViewBag.ManagerJobTitle = user.JobTitle;
            ViewBag.ManagerID = user.SystemUserID;



            if (Session["User"] != null)//if the user is in fact logged in
            {
                // add all the users who's firstname will be the same as the logged in user's. 
                //should be only one user. 

                employee = employee.Where(a => a.UserName.ToUpper().Equals(user.UserName.ToUpper()));

                //tempdata is mainly for redirects. Whilst sessions remain active, must be explicitly closed. 


            }
            //converts the var list to a list of type System Object.
            newUsers = employee.Cast<SystemUser>().ToList();
            SystemUser newUser = newUsers.First();
            selectedEmployee = selectedEmployee.Where(b => b.AssignedManager.ToUpper().Equals(newUser.LastName.ToUpper()));
            int numberOfEmployees = selectedEmployee.Count();

            //returns size of a manager's team
            ViewBag.NumberOfEmployees = numberOfEmployees;





            List<SystemUser> assignedUsers = null;
            assignedUsers = selectedEmployee.Cast<SystemUser>().ToList();

            //To get total number of objectives and the objectives that have not been signed off. 


            int objectivecount = 0;
            // int notAppObjectiveCount = 0;
            var goals = /*from c in */db.Goals; //grabs all goals currently in the database
                                                // select c;


            ViewBag.NumberOfObjectives = objectivecount;

            ViewBag.NumberOfCompleteGoals = TotalcompleteGoals;
            ViewBag.NumberOfIncompleteGoals = TotalincompleteGoals;

            ViewBag.NumberNotApprovalGoals = TotalnotapprovedGoals;
            //   ViewBag.NumberOfNotAppObj = notAppObjectiveCount;

            //searching functionality

            if (!String.IsNullOrEmpty(searchString)) //allowing to search using either first or last name
            {//The search string value is received from a text box that you'll add to the Index view.
                selectedEmployee = selectedEmployee.Where(a => a.LastName.ToUpper().Contains(searchString)
                                       || a.FirstName.ToUpper().Contains(searchString));
            }
            return View(selectedEmployee.ToList());//returns the individual employee

        }





        public ActionResult MoveToManagerPage() //moves to the manager page once the user has logged in and wants to switch between pages
        {
            SystemUser newUser = (SystemUser)Session["manager"];

            return RedirectToAction("ManagerEmployees", "SystemUsers", new { id = newUser.SystemUserID });

        }

        #endregion



        #region Admin page methods
        // GET: SystemUsers
        public ActionResult Index(string searchString)
        {
            var systemUsers = from a in db.SystemUsers
                                  //grabs all users currently in the database
                              select a;

            if (!String.IsNullOrEmpty(searchString)) //allowing to search using either first or last name
            {//The search string value is received from a text box that you'll add to the Index view.
                systemUsers = systemUsers.Where(a => a.LastName.ToUpper().Contains(searchString)
                                       || a.AssignedManager.ToUpper().Contains(searchString)
                                       || a.FirstName.ToUpper().Contains(searchString));

            }
            return View(systemUsers);
        }






        // GET: SystemUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemUser systemUser = db.SystemUsers.Find(id);
            if (systemUser == null)
            {
                return HttpNotFound();
            }
            return View(systemUser);
        }


        public string DisplayName(SystemUser user)
        {

            return user.FirstName + " " + user.LastName;

        }

        // GET: SystemUsers/Create
        public ActionResult Create()
        {
            var rolesmanager = //returns all the employees currently stored in the database
                              from a in db.Roles
                              select a;
            rolesmanager = rolesmanager.Where(a => a.StoredRoleID.ToString().Equals("3"));

            List<Role> roles = null;

            roles = rolesmanager.Cast<Role>().ToList();



            var assignedManager = from firstList in roles
                                  group firstList by firstList.SystemUser.LastName into newList
                                  let m = newList.FirstOrDefault()
                                  select m;

            ViewBag.AssignedManager = new SelectList(assignedManager.ToList(), "SystemUser.LastName", "SystemUser.LastName");


            return View();
        }





        // POST: SystemUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SystemUserID,LastName,FirstName,UserName,Password,AssignedManager,ManagerPosition,JobTitle,Email,SystemUserImage,ImageFile")] SystemUser systemUser)
        {
            if (ModelState.IsValid)
            {
                using (var db = new DatabasePMEntities())//create a new instance of the main db context
                {

                    var crypto = new SimpleCrypto.PBKDF2();
                    var encrpPass = crypto.Compute(systemUser.Password);//encrypt the password


                    //  var sysUser = db.SystemUsers.Create();//create new system user
                    //assign the values received from the user to the syUser
                    //  sysUser.FirstName = systemUser.FirstName;
                    //  sysUser.LastName = systemUser.LastName;
                    //  sysUser.UserName = systemUser.UserName;
                    //  sysUser.Email = systemUser.Email;
                    //sysUser.JobTitle = systemUser.JobTitle;

                    systemUser.Password = encrpPass;
                    systemUser.PasswordSalt = crypto.Salt;
                    //  sysUser.SystemUserID = systemUser.SystemUserID;

                    //beginning of Image functionality//
                    string fileName = Path.GetFileNameWithoutExtension(systemUser.ImageFile.FileName);
                    string extension = Path.GetExtension(systemUser.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                    systemUser.SystemUserImage = "~/Content/ProfileImages/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Content/ProfileImages/"), fileName);
                    systemUser.ImageFile.SaveAs(fileName);

                    //if (systemUser != null)
                    //{
                    //db.SystemUsers.Add(systemUser);
                    //db.SaveChanges();
                    //}



                    //End Image Functionality//

                    db.SystemUsers.Add(systemUser);
                    db.SaveChanges();
                    // ModelState.Clear();
                    return RedirectToAction("Index");
                }
            }
            var rolesmanager = //returns all the employees currently stored in the database.
                             from a in db.Roles
                             select a;
            rolesmanager = rolesmanager.Where(a => a.StoredRoleID.ToString().Equals("3"));

            List<Role> roles = null;

            roles = rolesmanager.Cast<Role>().ToList();



            var assignedManager = from firstList in roles
                                  group firstList by firstList.SystemUser.LastName into newList
                                  let m = newList.FirstOrDefault()
                                  select m;

            ViewBag.AssignedManager = new SelectList(assignedManager.ToList(), "SystemUser.LastName", "SystemUser.LastName");


            return View(systemUser);
        }





        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemUser systemUser = db.SystemUsers.Find(id);
            if (systemUser == null)
            {
                return HttpNotFound();
            }

            var rolesmanager = //returns all the employees currently stored in the database
                             from a in db.Roles
                             select a;
            rolesmanager = rolesmanager.Where(a => a.StoredRoleID.ToString().Equals("3"));

            List<Role> roles = null;

            roles = rolesmanager.Cast<Role>().ToList();



            var assignedManager = from firstList in roles
                                  group firstList by firstList.SystemUser.LastName into newList
                                  let m = newList.FirstOrDefault()
                                  select m;

            ViewBag.AssignedManager = new SelectList(assignedManager.ToList(), "SystemUser.LastName", "SystemUser.LastName");

            return View(systemUser);
        }

        // POST: SystemUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SystemUserID,LastName,FirstName,UserName,Password,PasswordSalt,AssignedManager,ManagerPosition,JobTitle,Email,SystemUserImage,ImageFile")] SystemUser systemUser)
        {


            //var currentPerson = db.SystemUsers.FirstOrDefault(p => p.SystemUserID == systemUser.SystemUserID);
            //if (currentPerson == null)
            //    return HttpNotFound();

            //systemUser.Password = currentPerson.Password;
            //systemUser.PasswordSalt = currentPerson.PasswordSalt;

            if (systemUser.ImageFile != null)
            {

                string fileName = Path.GetFileNameWithoutExtension(systemUser.ImageFile.FileName);
                string extension = Path.GetExtension(systemUser.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                systemUser.SystemUserImage = "~/Content/ProfileImages/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Content/ProfileImages/"), fileName);
                systemUser.ImageFile.SaveAs(fileName);
            }

            if (ModelState.IsValid)
            {
                string manager = systemUser.AssignedManager;


                db.Entry(systemUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var rolesmanager = //returns all the employees currently stored in the database
                             from a in db.Roles
                             select a;
            rolesmanager = rolesmanager.Where(a => a.StoredRoleID.ToString().Equals("3"));

            List<Role> roles = null;

            roles = rolesmanager.Cast<Role>().ToList();



            var assignedManager = from firstList in roles
                                  group firstList by firstList.SystemUser.LastName into newList
                                  let m = newList.FirstOrDefault()
                                  select m;

            ViewBag.AssignedManager = new SelectList(assignedManager.ToList(), "SystemUser.LastName", "SystemUser.LastName");


            return View(systemUser);
        }

        // GET: SystemUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemUser systemUser = db.SystemUsers.Find(id);
            if (systemUser == null)
            {
                return HttpNotFound();
            }
            return View(systemUser);
        }

        // POST: SystemUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SystemUser systemUser = db.SystemUsers.Find(id);
            db.SystemUsers.Remove(systemUser);
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


        #endregion



        public ActionResult Rating(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemUser systemUser = db.SystemUsers.Find(id);
            if (systemUser == null)
            {
                return HttpNotFound();
            }

            var rolesmanager = //returns all the employees currently stored in the database
                             from a in db.Roles
                             select a;
            rolesmanager = rolesmanager.Where(a => a.StoredRoleID.ToString().Equals("3"));

            List<Role> roles = null;

            roles = rolesmanager.Cast<Role>().ToList();



            var assignedManager = from firstList in roles
                                  group firstList by firstList.SystemUser.LastName into newList
                                  let m = newList.FirstOrDefault()
                                  select m;

            ViewBag.AssignedManager = new SelectList(assignedManager.ToList(), "SystemUser.LastName", "SystemUser.LastName");

            return View(systemUser);
        }




        // POST: SystemUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rating([Bind(Include = "SystemUserID,LastName,FirstName,UserName,Password,PasswordSalt,AssignedManager,ManagerPosition,JobTitle,Email,UserImage,EmployeeRating")] SystemUser systemUser)
        {
            if (ModelState.IsValid)
            {
                string manager = systemUser.AssignedManager;
                db.Entry(systemUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var rolesmanager = //returns all the employees currently stored in the database
                             from a in db.Roles
                             select a;
            rolesmanager = rolesmanager.Where(a => a.StoredRoleID.ToString().Equals("3"));

            List<Role> roles = null;

            roles = rolesmanager.Cast<Role>().ToList();



            var assignedManager = from firstList in roles
                                  group firstList by firstList.SystemUser.LastName into newList
                                  let m = newList.FirstOrDefault()
                                  select m;

            ViewBag.AssignedManager = new SelectList(assignedManager.ToList(), "SystemUser.LastName", "SystemUser.LastName");

            return View(systemUser);
        }

    }
}
