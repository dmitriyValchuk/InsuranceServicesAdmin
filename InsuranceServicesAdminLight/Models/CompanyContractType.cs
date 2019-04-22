namespace InsuranceServicesAdminLight.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CompanyContractType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CompanyContractType()
        {
            ContractFranchises = new HashSet<ContractFranchise>();
        }

        public int Id { get; set; }

        public int IdCompanyMiddleman { get; set; }

        public int IdContractType { get; set; }

        public virtual CompanyMiddleman CompanyMiddleman { get; set; }

        public virtual ContractType ContractType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContractFranchise> ContractFranchises { get; set; }
    }
}
