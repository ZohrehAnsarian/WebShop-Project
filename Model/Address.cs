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
    
    public partial class Address
    {
        public int Id { get; set; }
        public string AddressLine { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string WebSite { get; set; }
        public string MapURL { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public Nullable<int> CustomerId { get; set; }
    }
}
