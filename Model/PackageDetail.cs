//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class PackageDetail
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int PackageId { get; set; }
        public int Number { get; set; }
    
        public virtual Package Package { get; set; }
        public virtual Product Product { get; set; }
    }
}
