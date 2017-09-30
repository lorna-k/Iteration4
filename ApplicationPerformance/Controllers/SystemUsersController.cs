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
            return View();
        }
        // SimpleCrypto will be installed to encrypt and decrypt the password




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

                    SystemUser selectedUser = null;//create a user that we will give values further down
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
                        /* int numberOfUsers = newUsers.Count();*//*count how many users are in the list, 
                                                             will be changed to how many roles 
                                                     have been assigned*/

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
                            // int adminRole = newUsers.FindIndex(item => item.RoleID == 1);//if their role is admin

                            bool containsAdmin = newUser.Roles.Any(p => p.StoredRoleID == 1);
                            if (containsAdmin)
                            {/*the redirect method will include, the name of the controller method, then the 
                                the controller class the method's in and then parameters if the method
                                takes any parameters*/
                                Session["admin"] = newUser;//later used for comments
                                return RedirectToAction("Index", "SystemUsers");/*show list of users in the system
                                                                                may need to change this*/
                            }
                            // int managerRole = newUsers.FindIndex(item => item.RoleID == 2);//if their role is manager
                            bool containsManager = newUser.Roles.Any(p => p.StoredRoleID == 3);
                            if (containsManager)
                            {
                                Session["manager"] = newUser;//later used for comments
                                return RedirectToAction("ManagerEmployees", "SystemUsers", new { id = newUser.SystemUserID });  /*Redirect to the manager
                                                                                                            page which shows manager details
                                                                                                            and employees under than manager*/
                            }
                            //  int employeeRole = newUsers.FindIndex(item => item.RoleID == 3);////if their role is employee
                            bool containsEmployee = newUser.Roles.Any(p => p.StoredRoleID == 2);

                            if (containsEmployee)
                            {
                                // Session["Employee"] = user.RoleID;//create a session for employee because this is used later on
                                //in the ... method
                                Session["employee"] = newUser;//later used for comments
                                return RedirectToAction("SelectedEmployee", "Goals", new { id = newUser.SystemUserID });
                                // return RedirectToAction("EmployeeDetails", "SystemUsers", new { id = newUser.SystemUserID });/* redirected to the page with the employee's
                                // details and their objective for the appraisal
                                //This method is in the Appraisals Controller*/
                            }
                        }
                    }


                }
                else //if the login issues were encountered. 
                {
                    ModelState.AddModelError("", "Login Data is incorrect");
                }
            }

            return View(user);//displays the user's details once they've been assigned
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




        /**Select your role Get Method**/
        [HttpGet]
        public ActionResult RolesSelect()
        {
            Session["employee"] = null;
            Session["manager"] = null;
            Session["admin"] = null;
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

                //converts the var list to a list of type System Object.
                //  newUsers = employees.Cast<SystemUser>().ToList();
                newUsers = employees.Cast<SystemUser>().ToList();
                /* int numberOfUsers = newUsers.Count();*//*count how many users are in the list, 
                                                     will be changed to how many roles 
                                             have been assigned*/

                newUser = newUsers.First();


                // int index = newUsers.FindIndex(item => item.RoleID == 1);
                bool containsAdmin = newUser.Roles.Any(p => p.StoredRoleID == 1);
                bool containsEmployee = newUser.Roles.Any(p => p.StoredRoleID == 2);
                bool containsManager = newUser.Roles.Any(p => p.StoredRoleID == 3);


                //  if (index >= 0)
                if (containsAdmin)
                {
                    Session["ValidAdmin"] = newUser;
                }
                // int secondindex = newUsers.FindIndex(item => item.RoleID == 2);
                if (containsManager)
                {
                    Session["ValidManager"] = newUser;
                }
                //   int thirdindex = newUsers.FindIndex(item => item.RoleID == 3);
                if (containsEmployee)
                {
                    Session["ValidEmployee"] = newUser;
                }

            }

            //  ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName");
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
                    // Session["Admin"] = user.RoleID;
                    Session["admin"] = newUser;
                    //   Session["AdminUserID"] = user.SystemUserID;
                    return RedirectToAction("Index", "SystemUsers");
                }
                else if (button == "2")
                {
                    //  Session["Manager"] = user.RoleID;
                    //   Session["ManagerUserID"] = user.SystemUserID;
                    Session["manager"] = newUser;
                    Session["ChosenManager"] = user;
                    return RedirectToAction("ManagerEmployees", "SystemUsers", new { id = newUser.SystemUserID });
                }
                else if (button == "3")
                {
                    Session["employee"] = newUser;
                    //  Session["Employee"] = user.RoleID;
                    return RedirectToAction("SelectedEmployee", "Goals", new { id = newUser.SystemUserID });
                    //  return RedirectToAction("EmployeeDetails", "SystemUsers", new { id = newUser.SystemUserID });

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
            //   ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName");

            return View(user);

        }



        public ActionResult MoveToManagerPage()
        {
            SystemUser newUser = (SystemUser)Session["manager"];
         
            return RedirectToAction("ManagerEmployees", "SystemUsers", new { id = newUser.SystemUserID });  
                                                                            
        }







        public List<string> ApprovalList()
        {
            SystemUser user = (SystemUser)Session["manager"];


            var goals =
                       from a in db.Goals //grabs all users currently in the database
                       select a;

            goals = goals.Where(a => a.SystemUser.AssignedManager.ToUpper().Equals(user.LastName.ToUpper()));

            List<string> approvalList = new List<string>();
            //Maneger has not signed off 
            int managerApproval = 0;

            foreach (var item in goals)
            {
                if (item.ManagerApproval.ToString().ToUpper() != "APPROVED")
                {
                    if (item.Appraisal.AppraisalEndDate >= DateTime.Now.Date)
                    {
                        managerApproval += 1;
                        approvalList.Add(item.SystemUser.FirstName + " " + item.SystemUser.LastName + ": " + item.Objective.Title);
                    }
                }
            }

            ViewBag.ListApprovalGoals = approvalList;
            return approvalList;
        }
        




        public ActionResult Approval()
        {
            ApprovalList();
            return View();
        }



        public ActionResult Incomplete()
        {
            IncompleteList();
            return View();
        }


        public List<string> IncompleteList()
        {
            SystemUser user = (SystemUser)Session["manager"];


            var goals =
                       from a in db.Goals //grabs all users currently in the database
                       select a;

            goals = goals.Where(a => a.SystemUser.AssignedManager.ToUpper().Equals(user.LastName.ToUpper()));

            List<string> incompleteGoalList = new List<string>();

            int incompleteGoalCount = 0;
            foreach (var item in goals)
            {
                if (item.GoalStatus.ToString().ToUpper() != "COMPLETE" || item.GoalStatus.ToString().ToUpper() == null)
                {
                    if (item.Appraisal.AppraisalEndDate >= DateTime.Now.Date)
                    {
                        incompleteGoalCount += 1;
                        incompleteGoalList.Add(item.SystemUser.FirstName + " " + item.SystemUser.LastName + ": " + item.Objective.Title);
                    }
                }
            }





            //  int completeGoalCount = goalComplete.Count();



            //   ViewBag.NumberOfCompleteGoals = completeGoalCount;
            // Session["CompleteGoals"] = completeGoalCount;
            ViewBag.ListOfIncompleteGoals = incompleteGoalList;
            return incompleteGoalList;
        }




        public ActionResult Complete()
        {
            CompleteList();
            return View();
        }





        public List<string> CompleteList()
        {
            SystemUser user = (SystemUser)Session["manager"];

            int ba = 0;
            var goals =
                       from a in db.Goals //grabs all users currently in the database
                       select a;

            goals = goals.Where(a => a.SystemUser.AssignedManager.ToUpper().Equals(user.LastName.ToUpper()));

            List<string> completeGoalList = new List<string>();

            int completeGoalCount = 0;
            foreach (var item in goals)
            {
                if (item.GoalStatus.ToString().ToUpper() == "COMPLETE")
                {
                    if (item.Appraisal.AppraisalEndDate >= DateTime.Now.Date)
                    {
                        completeGoalCount += 1;
                        completeGoalList.Add(item.SystemUser.FirstName + " " + item.SystemUser.LastName + ": " + item.Objective.Title);
                    }
                }
            }





            //  int completeGoalCount = goalComplete.Count();



            //   ViewBag.NumberOfCompleteGoals = completeGoalCount;
            // Session["CompleteGoals"] = completeGoalCount;
            ViewBag.ListOfCompleteGoals = completeGoalList;
            return completeGoalList;
        }






        //public ActionResult ManagerEmployees(int? id, string searchString)
        //{

        //    List<SystemUser> newUsers = null;//creates a list of users 
        //                                     // SystemUser user = null;//creates a user object that shall be assigned values
        //    var employee =
        //               from a in db.SystemUsers //grabs all users currently in the database
        //               select a;
        //    var selectedEmployee =
        //               from b in db.SystemUsers //grabs all users currently in the database
        //               select b;
        //    SystemUser user = db.SystemUsers.Find(id);//assigns the logged in user's details

        //    ViewBag.ManagerLastName = user.LastName;
        //    ViewBag.ManagerFirstName = user.FirstName;
        //    ViewBag.ManagerJobTitle = user.JobTitle;
        //    ViewBag.ManagerID = user.SystemUserID;



        //    if (Session["User"] != null)//if the user is in fact logged in
        //    {
        //        // add all the users who's firstname will be the same as the logged in user's. 
        //        //should be only one user. 

        //        employee = employee.Where(a => a.UserName.ToUpper().Equals(user.UserName.ToUpper()));

        //        //tempdata is mainly for redirects. Whilst sessions remain active, must be explicitly closed. 


        //    }
        //    //converts the var list to a list of type System Object.
        //    newUsers = employee.Cast<SystemUser>().ToList();
        //    SystemUser newUser = newUsers.First();
        //    selectedEmployee = selectedEmployee.Where(b => b.AssignedManager.ToUpper().Equals(newUser.LastName.ToUpper()));
        //    int numberOfEmployees = selectedEmployee.Count();

        //    //returns size of a manager's team
        //    ViewBag.NumberOfEmployees = numberOfEmployees;





        //    List<SystemUser> assignedUsers = null;
        //    assignedUsers = selectedEmployee.Cast<SystemUser>().ToList();

        //    //To get total number of objectives and the objectives that have not been signed off. 


        //    int objectivecount = 0;
        //    int notAppObjectiveCount = 0;
        //    var goals = /*from c in */db.Goals; //grabs all goals currently in the database
        //               // select c;

        //    List<Goal> assignedgoals = null;
        //    assignedgoals = goals.Cast<Goal>().ToList();
        //    //  string test = emptest.Objective.Title;
        //    SystemUser emp;
        //    Goal item;
        //    for (int i = 0; i < selectedEmployee.Count(); i++)
        //    {
        //        emp = assignedUsers.ElementAt(i);
        //       // var emp = selectedEmployee.ElementAt(i);
        //        for (int j = 0; j < goals.Count(); j++)
        //        {
        //            item = assignedgoals.ElementAt(j);
        //            if (item.Appraisal.AppraisalEndDate >= DateTime.Now.Date)
        //            {
        //                if (emp.SystemUserID.Equals(item.SystemUserID))
        //                {
        //                    objectivecount += 1;
        //                    if (item.ManagerApproval.ToString().ToUpper() != "APPROVED")
        //                    {
        //                        notAppObjectiveCount += 1;
        //                    }
        //                }
        //            }

        //        }

        //    }
        //    ViewBag.NumberOfObjectives = objectivecount;

        //    ViewBag.NumberOfNotAppObj = notAppObjectiveCount;

        //    //searching functionality

        //    if (!String.IsNullOrEmpty(searchString)) //allowing to search using either first or last name
        //    {//The search string value is received from a text box that you'll add to the Index view.
        //        selectedEmployee = selectedEmployee.Where(a => a.LastName.ToUpper().Contains(searchString)
        //                               || a.FirstName.ToUpper().Contains(searchString));
        //    }
        //    return View(selectedEmployee.ToList());//returns the individual employee

        //}



        public ActionResult ManagerEmployees(int? id, string searchString)
        {

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
            int notAppObjectiveCount = 0;
            var goals = /*from c in */db.Goals; //grabs all goals currently in the database
                                                // select c;

            List<Goal> assignedgoals = null;
            assignedgoals = goals.Cast<Goal>().ToList();
            //  string test = emptest.Objective.Title;
            SystemUser emp;
            Goal item;
            for (int i = 0; i < selectedEmployee.Count(); i++)
            {
                emp = assignedUsers.ElementAt(i);
                // var emp = selectedEmployee.ElementAt(i);
                for (int j = 0; j < goals.Count(); j++)
                {
                    item = assignedgoals.ElementAt(j);
                    if (item.Appraisal.AppraisalEndDate >= DateTime.Now.Date)
                    {
                        if (emp.SystemUserID.Equals(item.SystemUserID))
                        {
                            objectivecount += 1;
                            if (item.ManagerApproval.ToString().ToUpper() != "APPROVED")
                            {
                                notAppObjectiveCount += 1;
                            }
                        }
                    }

                }

            }
            ViewBag.NumberOfObjectives = objectivecount;

            ViewBag.NumberOfCompleteGoals = CompleteList().Count();
            ViewBag.NumberOfIncompleteGoals = IncompleteList().Count();

            ViewBag.NumberNotApprovalGoals = ApprovalList().Count();

            ViewBag.NumberOfNotAppObj = notAppObjectiveCount;

            //searching functionality

            if (!String.IsNullOrEmpty(searchString)) //allowing to search using either first or last name
            {//The search string value is received from a text box that you'll add to the Index view.
                selectedEmployee = selectedEmployee.Where(a => a.LastName.ToUpper().Contains(searchString)
                                       || a.FirstName.ToUpper().Contains(searchString));
            }
            return View(selectedEmployee.ToList());//returns the individual employee

        }





        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("LogIn", "SystemUsers");
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
        public ActionResult Edit([Bind(Include = "SystemUserID,LastName,FirstName,UserName,Password,PasswordSalt,AssignedManager,ManagerPosition,JobTitle,Email,SystemUserImage")] SystemUser systemUser)
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
    }
}
