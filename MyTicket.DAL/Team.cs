//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyTicket.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Team
    {
        public int Id { get; set; }
        public int PrincipalId { get; set; }
        public int ProductId { get; set; }
        public bool Idleader { get; set; }
        public bool IsActive { get; set; }
    }
}