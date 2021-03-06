
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
    using System.ComponentModel;
    using System.Web;

    public partial class SystemUser
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public SystemUser()
    {

        this.Roles = new HashSet<Role>();

        this.Goals = new HashSet<Goal>();

        this.Responses = new HashSet<Response>();

        this.Evaluations = new HashSet<Evaluation>();

    }


    public int SystemUserID { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public string Password { get; set; }

    public string PasswordSalt { get; set; }

    public string AssignedManager { get; set; }

    public string JobTitle { get; set; }

    public string Email { get; set; }

    public string UserName { get; set; }

    public string SystemUserImage { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Role> Roles { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Goal> Goals { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Response> Responses { get; set; }


        [DisplayName("Upload File")]
        public HttpPostedFileBase ImageFile { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Evaluation> Evaluations { get; set; }

}

}
