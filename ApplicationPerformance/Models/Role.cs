
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
    
public partial class Role
{

    public int RoleID { get; set; }

    public Nullable<int> SystemUserID { get; set; }

    public Nullable<int> StoredRoleID { get; set; }



    public virtual StoredRole StoredRole { get; set; }

    public virtual SystemUser SystemUser { get; set; }

}

}