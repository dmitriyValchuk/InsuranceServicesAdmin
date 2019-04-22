namespace InsuranceServicesAdminLight.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RegioneOfRegistrations")]
    public partial class RegioneOfRegistration1
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
