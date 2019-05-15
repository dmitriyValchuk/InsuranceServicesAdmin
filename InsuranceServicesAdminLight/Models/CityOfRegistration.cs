namespace InsuranceServicesAdminLight.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CityOfRegistration")]
    public partial class CityOfRegistration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CityOfRegistration()
        {
            CityOrCountryOfRegToZones = new HashSet<CityOrCountryOfRegToZone>();
            TSCs = new HashSet<TSC>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int? IdRegioneOfRegistration { get; set; }

        public int? IdZoneOfRegistration { get; set; }

        public virtual RegioneOfRegistration RegioneOfRegistration { get; set; }

        public virtual InsuranceZoneOfRegistration InsuranceZoneOfRegistration { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CityOrCountryOfRegToZone> CityOrCountryOfRegToZones { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TSC> TSCs { get; set; }
    }
}
