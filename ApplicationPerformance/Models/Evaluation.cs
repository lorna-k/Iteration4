
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
    
public partial class Evaluation
{

    public int EvaluationID { get; set; }

    public Nullable<Rating> Rating { get; set; }

    public int SystemUserID { get; set; }

    public System.DateTime EvaluationDate { get; set; }

    public Nullable<int> CompleteObjectives { get; set; }

    public Nullable<int> TotalObjectives { get; set; }

    public string ManagerComment { get; set; }

    public string EmployeeComment { get; set; }



    public virtual SystemUser SystemUser { get; set; }

}

}
