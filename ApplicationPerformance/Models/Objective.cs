
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
    
public partial class Objective
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Objective()
    {

        this.Goals = new HashSet<Goal>();

    }


    public int ObjectiveID { get; set; }

    public string Title { get; set; }

    public string ObjectiveDescription { get; set; }

    public Nullable<ObjectiveType> ObjectiveType { get; set; }

    public Nullable<Confidentiality> Confidentiality { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Goal> Goals { get; set; }

}

}
