
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace ApplicationPerformance.Models
{

using System;
    using System.Collections.Generic;
    
public partial class Goal
{

    public int GoalID { get; set; }

    public string ManagerComment { get; set; }

    public string EmployeeComment { get; set; }

    public Nullable<int> ObjectiveID { get; set; }

    public int SystemUserID { get; set; }

    public int AppraisalID { get; set; }

    public Nullable<GoalStatus> GoalStatus { get; set; }

    public Nullable<int> Weight { get; set; }

    public Nullable<ManagerApproval> ManagerApproval { get; set; }

    public Nullable<EmployeeApproval> EmployeeApproval { get; set; }

    public Nullable<FinalApproval> FinalApproval { get; set; }

    public string ConfidentialTitle { get; set; }



    public virtual Appraisal Appraisal { get; set; }

    public virtual SystemUser SystemUser { get; set; }

    public virtual Objective Objective { get; set; }

}

}
