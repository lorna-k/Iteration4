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
    public class EvaluationsController : Controller
    {
        private DatabasePMEntities db = new DatabasePMEntities();

        //Testing new code


        #region Old index method
        //public ActionResult Index()
        //{
        //    SystemUser user = (SystemUser)Session["CurrentUser"];

        //    ViewBag.evalLastName = user.LastName;
        //    ViewBag.evalFirstName = user.FirstName;
        //    var evaluations = db.Evaluations.Include(e => e.SystemUser);


        //    evaluations = evaluations.Where(a => a.SystemUser.SystemUserID.Equals(user.SystemUserID));
        //    var systemusers = from a in db.SystemUsers //grabs all users currently in the database
        //                      select a;

        //    systemusers = systemusers.Where(a => a.LastName.Equals(user.AssignedManager));
        //    SystemUser assignedmanager = (SystemUser)systemusers.First();

        //    ViewBag.AssignedManagerID = assignedmanager.SystemUserID;
        //    ViewBag.EvalID = user.SystemUserID;

        //    return View(evaluations.ToList());

        //}

        //Added chart to evaluation method 
        #endregion



        #region Individual Employee's evaluations
        int Jan;
        int Feb;
        int Mar;
        int Apr;
        int May;
        int Jun;
        int Jul;
        int Aug;
        int Sep;
        int Oct;
        int Nov;
        int Dec;

        int janEvaluationsValue;
        int janEvaluationsCount;

        int febEvaluationsValue;
        int febEvaluationsCount;

        int marEvaluationsValue;
        int marEvaluationsCount;


        int aprEvaluationsValue;
        int aprEvaluationsCount;

        int mayEvaluationsValue;
        int mayEvaluationsCount;

        int junEvaluationsValue;
        int junEvaluationsCount;

        int julEvaluationsValue;
        int julEvaluationsCount;

        int augEvaluationsValue;
        int augEvaluationsCount;

        int sepEvaluationsValue;
        int sepEvaluationsCount;

        int octEvaluationsValue;
        int octEvaluationsCount;

        int novEvaluationsValue;
        int novEvaluationsCount;

        int decEvaluationsValue;
        int decEvaluationsCount;
        #endregion

        #region All employee evaluations variables
        int empJan;
        int empFeb;
        int empMar;
        int empApr;
        int empMay;
        int empJun;
        int empJul;
        int empAug;
        int empSep;
        int empOct;
        int empNov;
        int empDec;

        int empJanEvaluationsValue;
        int empJanEvaluationsCount;

        int empFebEvaluationsValue;
        int empFebEvaluationsCount;

        int empMarEvaluationsValue;
        int empMarEvaluationsCount;


        int empAprEvaluationsValue;
        int empAprEvaluationsCount;

        int empMayEvaluationsValue;
        int empMayEvaluationsCount;

        int empJunEvaluationsValue;
        int empJunEvaluationsCount;

        int empJulEvaluationsValue;
        int empJulEvaluationsCount;

        int empAugEvaluationsValue;
        int empAugEvaluationsCount;

        int empSepEvaluationsValue;
        int empSepEvaluationsCount;

        int empOctEvaluationsValue;
        int empOctEvaluationsCount;

        int empNovEvaluationsValue;
        int empNovEvaluationsCount;

        int empDecEvaluationsValue;
        int empDecEvaluationsCount;

        #endregion





        // GET: Evaluations
        public ActionResult Index(string searchString, DateTime? start, DateTime? end)

        {
            SystemUser user = (SystemUser)Session["CurrentUser"];

            ViewBag.evalLastName = user.LastName;
            ViewBag.evalFirstName = user.FirstName;
            var evaluations = db.Evaluations.Include(e => e.SystemUser);

            #region Specfic employee's evaluation

            if (Session["CurrentUser"] != null)
            {
                //  user = (SystemUser)Session["CurrentUser"];//assigns the logged in user's details
                //Grabs all the user's appraisals
                if (start == null || end == null)
                {

                    evaluations = evaluations.Where(a => a.SystemUser.SystemUserID.Equals(user.SystemUserID));
                }
                else
                {
                    evaluations = evaluations
                                       .Where(x =>
                                        x.EvaluationDate >= start
                                       && x.EvaluationDate <= end
                                       && x.SystemUser.SystemUserID.Equals(user.SystemUserID))
                                       .OrderByDescending(x => x.EvaluationID)
                                       ;
                }

            }

            //searching functionality
            if (!String.IsNullOrEmpty(searchString)) //allowing to search using either first or last name
            {//The search string value is received from a text box that you'll add to the Index view.

                evaluations = evaluations.Where(a => /*a.Appraisal.AppraisalEndDate.ToString().Contains(searchString)   //filter by date*/
                                         a.ManagerComment.ToUpper().Contains(searchString)
                                        );
            }


            //evaluations = evaluations.Where(a => a.SystemUser.SystemUserID.Equals(user.SystemUserID));
            var systemusers = from a in db.SystemUsers //grabs all users currently in the database
                              select a;

            systemusers = systemusers.Where(a => a.LastName.Equals(user.AssignedManager));
            SystemUser assignedmanager = (SystemUser)systemusers.First();

            ViewBag.AssignedManagerID = assignedmanager.SystemUserID;
            ViewBag.EvalID = user.SystemUserID;



            // goodEvaluations = goodEvaluations.Where(a => a.Rating.Equals(ApplicationPerformance.Models.Rating.Good));
            foreach (var item in evaluations)
            {

                if (item.EvaluationDate.Month == 1)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        janEvaluationsValue += 3;
                        janEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        janEvaluationsValue += 2;
                        janEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        janEvaluationsValue += 1;
                        janEvaluationsCount += 1;
                    }
                    Jan = janEvaluationsValue / janEvaluationsCount;


                    ViewBag.Jan = Jan;
                }


                else if (item.EvaluationDate.Month == 2)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        febEvaluationsValue += 3;
                        febEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        febEvaluationsValue += 2;
                        febEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        febEvaluationsValue += 1;
                        febEvaluationsCount += 1;
                    }
                    Feb = febEvaluationsValue / febEvaluationsCount;


                    ViewBag.Feb = Feb;

                }


                else if (item.EvaluationDate.Month == 3)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        marEvaluationsValue += 3;
                        marEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        marEvaluationsValue += 2;
                        marEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        marEvaluationsValue += 1;
                        marEvaluationsCount += 1;
                    }
                    Mar = marEvaluationsValue / marEvaluationsCount;

                    ViewBag.Mar = Mar;

                }



                else if (item.EvaluationDate.Month == 4)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        aprEvaluationsValue += 3;
                        aprEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        aprEvaluationsValue += 2;
                        aprEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        aprEvaluationsValue += 1;
                        aprEvaluationsCount += 1;
                    }
                    Apr = aprEvaluationsValue / aprEvaluationsCount;

                    ViewBag.Apr = Apr;

                }


                else if (item.EvaluationDate.Month == 5)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        mayEvaluationsValue += 3;
                        mayEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        mayEvaluationsValue += 2;
                        mayEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        mayEvaluationsValue += 1;
                        mayEvaluationsCount += 1;
                    }
                    May = mayEvaluationsValue / mayEvaluationsCount;

                    ViewBag.May = May;

                }



                else if (item.EvaluationDate.Month == 6)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        junEvaluationsValue += 3;
                        junEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        junEvaluationsValue += 2;
                        junEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        junEvaluationsValue += 1;
                        junEvaluationsCount += 1;
                    }
                    Jun = junEvaluationsValue / junEvaluationsCount;

                    ViewBag.Jun = Jun;

                }
                else if (item.EvaluationDate.Month == 7)
                {

                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        julEvaluationsValue += 3;
                        julEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        julEvaluationsValue += 2;
                        julEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        julEvaluationsValue += 1;
                        julEvaluationsCount += 1;
                    }
                    Jul = julEvaluationsValue / julEvaluationsCount;

                    ViewBag.Jul = Jul;

                }
                else if (item.EvaluationDate.Month == 8)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        augEvaluationsValue += 3;
                        augEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        augEvaluationsValue += 2;
                        augEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        augEvaluationsValue += 1;
                        augEvaluationsCount += 1;
                    }
                    Aug = augEvaluationsValue / augEvaluationsCount;

                    ViewBag.Aug = Aug;

                }
                else if (item.EvaluationDate.Month == 9)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        sepEvaluationsValue += 3;
                        sepEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        sepEvaluationsValue += 2;
                        sepEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        sepEvaluationsValue += 1;
                        sepEvaluationsCount += 1;
                    }
                    Sep = sepEvaluationsValue / sepEvaluationsCount;

                    ViewBag.Sep = Sep;


                }
                else if (item.EvaluationDate.Month == 10)
                {

                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        octEvaluationsValue += 3;
                        octEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        octEvaluationsValue += 2;
                        octEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        octEvaluationsValue += 1;
                        octEvaluationsCount += 1;
                    }
                    Oct = octEvaluationsValue / octEvaluationsCount;

                    ViewBag.Oct = Oct;

                }
                else if (item.EvaluationDate.Month == 11)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        novEvaluationsValue += 3;
                        novEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        novEvaluationsValue += 2;
                        novEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        novEvaluationsValue += 1;
                        novEvaluationsCount += 1;
                    }
                    Nov = novEvaluationsValue / novEvaluationsCount;

                    ViewBag.Nov = Nov;

                }
                else if (item.EvaluationDate.Month == 12)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        decEvaluationsValue += 3;
                        decEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        decEvaluationsValue += 2;
                        decEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        decEvaluationsValue += 1;
                        decEvaluationsCount += 1;
                    }
                    Dec = decEvaluationsValue / decEvaluationsCount;

                    ViewBag.Dec = Dec;


                }


            }
            if (ViewBag.Jan == null)
            {
                ViewBag.Jan = 0;
            }
            if (ViewBag.Feb == null)
            {
                ViewBag.Feb = 0;
            }
            if (ViewBag.Mar == null)
            {
                ViewBag.Mar = 0;
            }
            if (ViewBag.Apr == null)
            {
                ViewBag.Apr = 0;
            }
            if (ViewBag.May == null)
            {
                ViewBag.May = 0;
            }
            if (ViewBag.Jun == null)
            {
                ViewBag.Jun = 0;
            }
            if (ViewBag.Jul == null)
            {
                ViewBag.Jul = 0;
            }
            if (ViewBag.Aug == null)
            {
                ViewBag.Aug = 0;
            }
            if (ViewBag.Sep == null)
            {
                ViewBag.Sep = 0;
            }
            if (ViewBag.Oct == null)
            {
                ViewBag.Oct = 0;
            }
            if (ViewBag.Nov == null)
            {
                ViewBag.Nov = 0;
            }
            if (ViewBag.Dec == null)
            {
                ViewBag.Dec = 0;
            }



            #endregion


            #region All evaluations method
            var empEvaluations = db.Evaluations.Include(e => e.SystemUser);
            if (start == null || end == null)
            {

                empEvaluations = empEvaluations.Where(a => a.SystemUser.AssignedManager.Equals(user.AssignedManager));
            }
            else
            {
                empEvaluations = empEvaluations
                                       .Where(x =>
                                        x.EvaluationDate >= start
                                       && x.EvaluationDate <= end
                                       && x.SystemUser.LastName.Equals(user.AssignedManager)
                                      )
                                       .OrderByDescending(x => x.EvaluationID)
                                       ;

            }

            //empEvaluations = empEvaluations.Where(a => a.SystemUser.SystemUserID.Equals(user.SystemUserID));

            //int two = empEvaluations.Count();


            foreach (var item in empEvaluations)
            {

                if (item.EvaluationDate.Month == 1)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        empJanEvaluationsValue += 3;
                        empJanEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        empJanEvaluationsValue += 2;
                        empJanEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        empJanEvaluationsValue += 1;
                        empJanEvaluationsCount += 1;
                    }
                    empJan = empJanEvaluationsValue / empJanEvaluationsCount;

                    ViewBag.empJan = empJan;

                }


                else if (item.EvaluationDate.Month == 2)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        empFebEvaluationsValue += 3;
                        empFebEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        empFebEvaluationsValue += 2;
                        empFebEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        empFebEvaluationsValue += 1;
                        empFebEvaluationsCount += 1;
                    }
                    empFeb = empFebEvaluationsValue / empFebEvaluationsCount;

                    ViewBag.empFeb = empFeb;

                }


                else if (item.EvaluationDate.Month == 3)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        empMarEvaluationsValue += 3;
                        empMarEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        empMarEvaluationsValue += 2;
                        empMarEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        empMarEvaluationsValue += 1;
                        empMarEvaluationsCount += 1;
                    }
                    empMar = empMarEvaluationsValue / empMarEvaluationsCount;

                    ViewBag.empMar = empMar;

                }



                else if (item.EvaluationDate.Month == 4)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        empAprEvaluationsValue += 3;
                        empAprEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        empAprEvaluationsValue += 2;
                        empAprEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        empAprEvaluationsValue += 1;
                        empAprEvaluationsCount += 1;
                    }
                    empApr = empAprEvaluationsValue / empAprEvaluationsCount;

                    ViewBag.empApr = empApr;

                }


                else if (item.EvaluationDate.Month == 5)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        empMayEvaluationsValue += 3;
                        empMayEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        empMayEvaluationsValue += 2;
                        empMayEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        empMayEvaluationsValue += 1;
                        empMayEvaluationsCount += 1;
                    }
                    empMay = empMayEvaluationsValue / empMayEvaluationsCount;

                    ViewBag.empMay = empMay;

                }



                else if (item.EvaluationDate.Month == 6)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        empJunEvaluationsValue += 3;
                        empJunEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        empJunEvaluationsValue += 2;
                        empJunEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        empJunEvaluationsValue += 1;
                        empJunEvaluationsCount += 1;
                    }
                    empJun = empJunEvaluationsValue / empJunEvaluationsCount;

                    ViewBag.empJun = empJun;

                }
                else if (item.EvaluationDate.Month == 7)
                {

                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        empJulEvaluationsValue += 3;
                        empJulEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        empJulEvaluationsValue += 2;
                        empJulEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        empJulEvaluationsValue += 1;
                        empJulEvaluationsCount += 1;
                    }
                    empJul = empJulEvaluationsValue / empJulEvaluationsCount;

                    ViewBag.empJul = empJul;

                }
                else if (item.EvaluationDate.Month == 8)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        empAugEvaluationsValue += 3;
                        empAugEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        empAugEvaluationsValue += 2;
                        empAugEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        empAugEvaluationsValue += 1;
                        empAugEvaluationsCount += 1;
                    }
                    empAug = empAugEvaluationsValue / empAugEvaluationsCount;

                    ViewBag.empAug = empAug;

                }
                else if (item.EvaluationDate.Month == 9)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        empSepEvaluationsValue += 3;
                        empSepEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        empSepEvaluationsValue += 2;
                        empSepEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        empSepEvaluationsValue += 1;
                        empSepEvaluationsCount += 1;
                    }
                    empSep = empSepEvaluationsValue / empSepEvaluationsCount;

                    ViewBag.empSep = empSep;

                }
                else if (item.EvaluationDate.Month == 10)
                {

                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        empOctEvaluationsValue += 3;
                        empOctEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        empOctEvaluationsValue += 2;
                        empOctEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        empOctEvaluationsValue += 1;
                        empOctEvaluationsCount += 1;
                    }
                    empOct = empOctEvaluationsValue / empOctEvaluationsCount;

                    ViewBag.empOct = empOct;

                }
                else if (item.EvaluationDate.Month == 11)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        empNovEvaluationsValue += 3;
                        empNovEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        empNovEvaluationsValue += 2;
                        empNovEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        empNovEvaluationsValue += 1;
                        empNovEvaluationsCount += 1;
                    }
                    empNov = empNovEvaluationsValue / empNovEvaluationsCount;

                    ViewBag.empNov = empNov;

                }
                else if (item.EvaluationDate.Month == 12)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        empDecEvaluationsValue += 3;
                        empDecEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        empDecEvaluationsValue += 2;
                        empDecEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        empDecEvaluationsValue += 1;
                        empDecEvaluationsCount += 1;
                    }
                    empDec = empDecEvaluationsValue / empDecEvaluationsCount;

                    ViewBag.empDec = empDec;


                }


            }



            if (ViewBag.empJan == null)
            {
                ViewBag.empJan = 0;
            }
            if (ViewBag.empFeb == null)
            {
                ViewBag.empFeb = 0;
            }
            if (ViewBag.empMar == null)
            {
                ViewBag.empMar = 0;
            }
            if (ViewBag.empApr == null)
            {
                ViewBag.empApr = 0;
            }
            if (ViewBag.empMay == null)
            {
                ViewBag.empMay = 0;
            }
            if (ViewBag.empJun == null)
            {
                ViewBag.empJun = 0;
            }
            if (ViewBag.empJul == null)
            {
                ViewBag.empJul = 0;
            }
            if (ViewBag.empAug == null)
            {
                ViewBag.empAug = 0;
            }
            if (ViewBag.empSep == null)
            {
                ViewBag.empSep = 0;
            }
            if (ViewBag.empOct == null)
            {
                ViewBag.empOct = 0;
            }
            if (ViewBag.empNov == null)
            {
                ViewBag.empNov = 0;
            }
            if (ViewBag.empDec == null)
            {
                ViewBag.empDec = 0;
            }
            #endregion

            return View(evaluations.ToList());

        }





        public ActionResult MyChart()
        {

            var evaluations = db.Evaluations;



            int Jan = 0;
            int Feb = 0;
            int Mar = 0;
            int Apr = 0;
            int May = 0;
            int Jun = 0;
            int Jul = 0;
            int Aug = 0;
            int Sep = 0;
            int Oct = 0;
            int Nov = 0;
            int Dec = 0;

            int janEvaluationsValue = 0;
            int janEvaluationsCount = 0;

            int febEvaluationsValue = 0;
            int febEvaluationsCount = 0;

            int marEvaluationsValue = 0;
            int marEvaluationsCount = 0;


            int aprEvaluationsValue = 0;
            int aprEvaluationsCount = 0;

            int mayEvaluationsValue = 0;
            int mayEvaluationsCount = 0;

            int junEvaluationsValue = 0;
            int junEvaluationsCount = 0;

            int julEvaluationsValue = 0;
            int julEvaluationsCount = 0;

            int augEvaluationsValue = 0;
            int augEvaluationsCount = 0;

            int sepEvaluationsValue = 0;
            int sepEvaluationsCount = 0;

            int octEvaluationsValue = 0;
            int octEvaluationsCount = 0;

            int novEvaluationsValue = 0;
            int novEvaluationsCount = 0;

            int decEvaluationsValue = 0;
            int decEvaluationsCount = 0;

            //int evaluationsValue = 0;
            //int evaluationsCount = 0;
            //  goodEvaluations = goodEvaluations.Where(a => a.Rating.Equals(ApplicationPerformance.Models.Rating.Good));
            foreach (var item in evaluations)
            {

                if (item.EvaluationDate.Month == 1)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        janEvaluationsValue += 3;
                        janEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        janEvaluationsValue += 2;
                        janEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        janEvaluationsValue += 1;
                        janEvaluationsCount += 1;
                    }
                    Jan = janEvaluationsValue / janEvaluationsCount;
                }


                else if (item.EvaluationDate.Month == 2)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        febEvaluationsValue += 3;
                        febEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        febEvaluationsValue += 2;
                        febEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        febEvaluationsValue += 1;
                        febEvaluationsCount += 1;
                    }
                    Feb = febEvaluationsValue / febEvaluationsCount;
                }


                else if (item.EvaluationDate.Month == 3)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        marEvaluationsValue += 3;
                        marEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        marEvaluationsValue += 2;
                        marEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        marEvaluationsValue += 1;
                        marEvaluationsCount += 1;
                    }
                    Mar = marEvaluationsValue / marEvaluationsCount;
                }



                else if (item.EvaluationDate.Month == 4)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        aprEvaluationsValue += 3;
                        aprEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        aprEvaluationsValue += 2;
                        aprEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        aprEvaluationsValue += 1;
                        aprEvaluationsCount += 1;
                    }
                    Apr = aprEvaluationsValue / aprEvaluationsCount;
                }


                else if (item.EvaluationDate.Month == 5)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        mayEvaluationsValue += 3;
                        mayEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        mayEvaluationsValue += 2;
                        mayEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        mayEvaluationsValue += 1;
                        mayEvaluationsCount += 1;
                    }
                    May = mayEvaluationsValue / mayEvaluationsCount;
                }



                else if (item.EvaluationDate.Month == 6)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        junEvaluationsValue += 3;
                        junEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        junEvaluationsValue += 2;
                        junEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        junEvaluationsValue += 1;
                        junEvaluationsCount += 1;
                    }
                }
                else if (item.EvaluationDate.Month == 7)
                {

                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        julEvaluationsValue += 3;
                        julEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        julEvaluationsValue += 2;
                        julEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        julEvaluationsValue += 1;
                        julEvaluationsCount += 1;
                    }
                }
                else if (item.EvaluationDate.Month == 8)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        augEvaluationsValue += 3;
                        augEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        augEvaluationsValue += 2;
                        augEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        augEvaluationsValue += 1;
                        augEvaluationsCount += 1;
                    }
                }
                else if (item.EvaluationDate.Month == 9)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        sepEvaluationsValue += 3;
                        sepEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        sepEvaluationsValue += 2;
                        sepEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        sepEvaluationsValue += 1;
                        sepEvaluationsCount += 1;
                    }

                }
                else if (item.EvaluationDate.Month == 10)
                {

                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        octEvaluationsValue += 3;
                        octEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        octEvaluationsValue += 2;
                        octEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        octEvaluationsValue += 1;
                        octEvaluationsCount += 1;
                    }
                }
                else if (item.EvaluationDate.Month == 11)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        novEvaluationsValue += 3;
                        novEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        novEvaluationsValue += 2;
                        novEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        novEvaluationsValue += 1;
                        novEvaluationsCount += 1;
                    }
                }
                else if (item.EvaluationDate.Month == 12)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        decEvaluationsValue += 3;
                        decEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        decEvaluationsValue += 2;
                        decEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        decEvaluationsValue += 1;
                        decEvaluationsCount += 1;
                    }

                }


            }


            SystemUser user = (SystemUser)Session["CurrentUser"];

            var empEvaluations = from a in db.Evaluations
                                     //grabs all users currently in the database
                                 select a;


            empEvaluations = empEvaluations.Where(a => a.SystemUser.SystemUserID.Equals(user.SystemUserID));

            int two = empEvaluations.Count();
            int empJan = 0;
            int empFeb = 0;
            int empMar = 0;
            int empApr = 0;
            int empMay = 0;
            int empJun = 0;
            int empJul = 0;
            int empAug = 0;
            int empSep = 0;
            int empOct = 0;
            int empNov = 0;
            int empDec = 0;

            int empJanEvaluationsValue = 0;
            int empJanEvaluationsCount = 0;

            int empFebEvaluationsValue = 0;
            int empFebEvaluationsCount = 0;

            int empMarEvaluationsValue = 0;
            int empMarEvaluationsCount = 0;


            int empAprEvaluationsValue = 0;
            int empAprEvaluationsCount = 0;

            int empMayEvaluationsValue = 0;
            int empMayEvaluationsCount = 0;

            int empJunEvaluationsValue = 0;
            int empJunEvaluationsCount = 0;

            int empJulEvaluationsValue = 0;
            int empJulEvaluationsCount = 0;

            int empAugEvaluationsValue = 0;
            int empAugEvaluationsCount = 0;

            int empSepEvaluationsValue = 0;
            int empSepEvaluationsCount = 0;

            int empOctEvaluationsValue = 0;
            int empOctEvaluationsCount = 0;

            int empNovEvaluationsValue = 0;
            int empNovEvaluationsCount = 0;

            int empDecEvaluationsValue = 0;
            int empDecEvaluationsCount = 0;

            //int evaluationsValue = 0;
            //int evaluationsCount = 0;
            //  goodEvaluations = goodEvaluations.Where(a => a.Rating.Equals(ApplicationPerformance.Models.Rating.Good));
            foreach (var item in empEvaluations)
            {

                if (item.EvaluationDate.Month == 1)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        empJanEvaluationsValue += 3;
                        empJanEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        empJanEvaluationsValue += 2;
                        empJanEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        empJanEvaluationsValue += 1;
                        empJanEvaluationsCount += 1;
                    }
                    empJan = empJanEvaluationsValue / empJanEvaluationsCount;
                }


                else if (item.EvaluationDate.Month == 2)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        empFebEvaluationsValue += 3;
                        empFebEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        empFebEvaluationsValue += 2;
                        empFebEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        empFebEvaluationsValue += 1;
                        empFebEvaluationsCount += 1;
                    }
                    empFeb = empFebEvaluationsValue / empFebEvaluationsCount;
                }


                else if (item.EvaluationDate.Month == 3)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        empMarEvaluationsValue += 3;
                        empMarEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        empMarEvaluationsValue += 2;
                        empMarEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        empMarEvaluationsValue += 1;
                        empMarEvaluationsCount += 1;
                    }
                    empMar = empMarEvaluationsValue / empMarEvaluationsCount;
                }



                else if (item.EvaluationDate.Month == 4)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        empAprEvaluationsValue += 3;
                        empAprEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        empAprEvaluationsValue += 2;
                        empAprEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        empAprEvaluationsValue += 1;
                        empAprEvaluationsCount += 1;
                    }
                    empApr = empAprEvaluationsValue / empAprEvaluationsCount;
                }


                else if (item.EvaluationDate.Month == 5)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        empMayEvaluationsValue += 3;
                        empMayEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        empMayEvaluationsValue += 2;
                        empMayEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        empMayEvaluationsValue += 1;
                        empMayEvaluationsCount += 1;
                    }
                    empMay = empMayEvaluationsValue / empMayEvaluationsCount;
                }



                else if (item.EvaluationDate.Month == 6)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        empJunEvaluationsValue += 3;
                        empJunEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        empJunEvaluationsValue += 2;
                        empJunEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        empJunEvaluationsValue += 1;
                        empJunEvaluationsCount += 1;
                    }
                }
                else if (item.EvaluationDate.Month == 7)
                {

                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        empJulEvaluationsValue += 3;
                        empJulEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        empJulEvaluationsValue += 2;
                        empJulEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        empJulEvaluationsValue += 1;
                        empJulEvaluationsCount += 1;
                    }
                }
                else if (item.EvaluationDate.Month == 8)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        empAugEvaluationsValue += 3;
                        empAugEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        empAugEvaluationsValue += 2;
                        empAugEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        empAugEvaluationsValue += 1;
                        empAugEvaluationsCount += 1;
                    }
                }
                else if (item.EvaluationDate.Month == 9)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        empSepEvaluationsValue += 3;
                        empSepEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        empSepEvaluationsValue += 2;
                        empSepEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        empSepEvaluationsValue += 1;
                        empSepEvaluationsCount += 1;
                    }

                }
                else if (item.EvaluationDate.Month == 10)
                {

                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        empOctEvaluationsValue += 3;
                        empOctEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        empOctEvaluationsValue += 2;
                        empOctEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        empOctEvaluationsValue += 1;
                        empOctEvaluationsCount += 1;
                    }
                }
                else if (item.EvaluationDate.Month == 11)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        empNovEvaluationsValue += 3;
                        empNovEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        empNovEvaluationsValue += 2;
                        empNovEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        empNovEvaluationsValue += 1;
                        empNovEvaluationsCount += 1;
                    }
                }
                else if (item.EvaluationDate.Month == 12)
                {
                    if (item.Rating.ToString().ToUpper() == "GOOD")
                    {
                        empDecEvaluationsValue += 3;
                        empDecEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "AVERAGE")
                    {
                        empDecEvaluationsValue += 2;
                        empDecEvaluationsCount += 1;
                    }
                    if (item.Rating.ToString().ToUpper() == "POOR")
                    {
                        empDecEvaluationsValue += 1;
                        empDecEvaluationsCount += 1;
                    }

                }


            }


            //if (evaluation.Rating= Rating.Good)
            //{

            //} //Evaluation evaluation= new Evaluation();
            string[] yval = { "2", "1", "3", "2", "1" };

            new Chart(width: 800, height: 200, theme: ChartTheme.Vanilla)
                .AddSeries(
                chartType: "line",
                xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" },
                yValues: new[] { Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec }
                      )
                .AddSeries(
                chartType: "line",
                //xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" },
                yValues: new[] { empJan, empFeb, empMar, empApr, empMay, empJun, empJul, empAug, empSep, empOct, empNov, empDec }

                )
                .Write("png");
            return null;

        }






        // GET: Evaluations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evaluation evaluation = db.Evaluations.Find(id);
            if (evaluation == null)
            {
                return HttpNotFound();
            }
            return View(evaluation);
        }





        // GET: Evaluations/Create
        public ActionResult Create(int? id)
        {
            SystemUser user = db.SystemUsers.Find(id); //creates a user object that shall be assigned values

            //SystemUser user = (SystemUser)Session["CurrentUser"];
            Session["CurrentUser"] = user;


            var systemusers = from a in db.SystemUsers //grabs all users currently in the database
                              select a;

            systemusers = systemusers.Where(a => a.LastName.Equals(user.AssignedManager));
            SystemUser assignedmanager = (SystemUser)systemusers.First();

            ViewBag.AssignedManagerID = assignedmanager.SystemUserID;
            ViewBag.EvalID = user.SystemUserID;

            ViewBag.FirstName = user.FirstName;
            ViewBag.LastName = user.LastName;



            var goals = from a in db.Goals //grabs all users currently in the database
                        select a; //.Where(a => a.GoalStatus.Equals(ApplicationPerformance.Models.GoalStatus.Complete));
                                  // goalComplete = goalComplete.Where(a => a.GoalStatus.ToString().ToUpper().Equals("COMPLETE"));

            goals = goals.Where(a => a.SystemUserID.Equals(user.SystemUserID));
            List<string> incompleteGoals = new List<string>();

            int complete = 0;


            foreach (var item in goals)
            {
                if (item.GoalStatus.ToString().ToUpper() != "COMPLETE")
                {
                    if (item.Appraisal.AppraisalEndDate >= DateTime.Now.Date)
                    {
                        incompleteGoals.Add(item.Objective.Title);
                    }
                }

                if (item.GoalStatus.ToString().ToUpper() == "COMPLETE")
                {
                    if (item.Appraisal.AppraisalEndDate >= DateTime.Now.Date)
                    {
                        complete += 1;
                    }
                }
            }
            var incomplete =
                      from a in db.Goals //grabs all users currently in the database
                      select a;


            //incomplete = incomplete.Where(a => a.GoalStatus.Equals(ApplicationPerformance.Models.GoalStatus.Incomplete));

            ViewBag.InCompleteGoalsList = incompleteGoals;
            //ViewBag.InCompleteGoals = incompleteGoals.Count();



            ViewBag.CompleteGoals = complete;
            ViewBag.InCompleteGoals = incompleteGoals.Count();


            Session["CompleteGoals"] = complete;

            int totalCount = complete + incompleteGoals.Count();
            ViewBag.TotalGoals = totalCount;
            Session["NumberOfGoals"] = totalCount;


            ViewBag.SystemUserID = new SelectList(db.SystemUsers, "SystemUserID", "LastName");
            return View();
        }





        // POST: Evaluations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EvaluationID,Rating,SystemUserID,EvaluationDate,CompleteObjectives,TotalObjectives,ManagerComment,EmployeeComment")] Evaluation evaluation)
        {
            SystemUser user = (SystemUser)Session["CurrentUser"];

            ViewBag.EvalID = user.SystemUserID;
            evaluation.SystemUserID = user.SystemUserID;

            if (ModelState.IsValid)
            {
                evaluation.CompleteObjectives = (int)Session["CompleteGoals"];
                evaluation.TotalObjectives = (int)Session["NumberOfGoals"];
                db.Evaluations.Add(evaluation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SystemUserID = new SelectList(db.SystemUsers, "SystemUserID", "LastName", evaluation.SystemUserID);
            return View(evaluation);
        }

        // GET: Evaluations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evaluation evaluation = db.Evaluations.Find(id);
            if (evaluation == null)
            {
                return HttpNotFound();
            }
            ViewBag.SystemUserID = new SelectList(db.SystemUsers, "SystemUserID", "LastName", evaluation.SystemUserID);
            return View(evaluation);
        }

        // POST: Evaluations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EvaluationID,Rating,SystemUserID,EvaluationDate,CompleteObjectives,TotalObjectives,ManagerComment,EmployeeComment")] Evaluation evaluation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(evaluation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SystemUserID = new SelectList(db.SystemUsers, "SystemUserID", "LastName", evaluation.SystemUserID);
            return View(evaluation);
        }

        // GET: Evaluations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evaluation evaluation = db.Evaluations.Find(id);
            if (evaluation == null)
            {
                return HttpNotFound();
            }
            return View(evaluation);
        }

        // POST: Evaluations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Evaluation evaluation = db.Evaluations.Find(id);
            db.Evaluations.Remove(evaluation);
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
