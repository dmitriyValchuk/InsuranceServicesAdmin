namespace InsuranceServicesAdminLight.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class InsuranceServicesContext : DbContext
    {
        public InsuranceServicesContext()
            : base("name=InsuranceServicesContext")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<BonusMalu> BonusMalus { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<CarGlobalToInsuranceType> CarGlobalToInsuranceTypes { get; set; }
        public virtual DbSet<CarGlobalType> CarGlobalTypes { get; set; }
        public virtual DbSet<CarInsuranceType> CarInsuranceTypes { get; set; }
        public virtual DbSet<Chart> Charts { get; set; }
        public virtual DbSet<CityOfRegistration> CityOfRegistrations { get; set; }
        public virtual DbSet<CityOrCountryOfRegToZone> CityOrCountryOfRegToZones { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<ClientCar> ClientCars { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanyContractType> CompanyContractTypes { get; set; }
        public virtual DbSet<CompanyDetail> CompanyDetails { get; set; }
        public virtual DbSet<CompanyFeature> CompanyFeatures { get; set; }
        public virtual DbSet<CompanyFeatureToCompany> CompanyFeatureToCompanies { get; set; }
        public virtual DbSet<CompanyIMG> CompanyIMGs { get; set; }
        public virtual DbSet<CompanyMiddleman> CompanyMiddlemen { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<ContractFranchise> ContractFranchises { get; set; }
        public virtual DbSet<ContractType> ContractTypes { get; set; }
        public virtual DbSet<CountryOfRegistration> CountryOfRegistrations { get; set; }
        public virtual DbSet<DiscountByQuantity> DiscountByQuantities { get; set; }
        public virtual DbSet<DiscountForClientWithPrivilegy> DiscountForClientWithPrivilegies { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<DocumentType> DocumentTypes { get; set; }
        public virtual DbSet<Franchise> Franchises { get; set; }
        public virtual DbSet<ImageType> ImageTypes { get; set; }
        public virtual DbSet<InsuranceZoneOfRegistration> InsuranceZoneOfRegistrations { get; set; }
        public virtual DbSet<K1> K1 { get; set; }
        public virtual DbSet<K2> K2 { get; set; }
        public virtual DbSet<K3> K3 { get; set; }
        public virtual DbSet<K4> K4 { get; set; }
        public virtual DbSet<K5> K5 { get; set; }
        public virtual DbSet<K6> K6 { get; set; }
        public virtual DbSet<K7> K7 { get; set; }
        public virtual DbSet<Middleman> Middlemen { get; set; }
        public virtual DbSet<Privilege> Privileges { get; set; }
        public virtual DbSet<RegioneOfRegistration> RegioneOfRegistrations { get; set; }
        public virtual DbSet<RegioneOfRegistration1> RegioneOfRegistrations1 { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TSC> TSCs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BonusMalu>()
                .HasMany(e => e.Contracts)
                .WithRequired(e => e.BonusMalu)
                .HasForeignKey(e => e.IdBonusMalus)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Car>()
                .HasMany(e => e.ClientCars)
                .WithRequired(e => e.Car)
                .HasForeignKey(e => e.IdCar)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CarGlobalType>()
                .HasMany(e => e.CarGlobalToInsuranceTypes)
                .WithRequired(e => e.CarGlobalType)
                .HasForeignKey(e => e.IdGlobalType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CarInsuranceType>()
                .HasMany(e => e.BonusMalus)
                .WithOptional(e => e.CarInsuranceType)
                .HasForeignKey(e => e.IdCarInsuranceType);

            modelBuilder.Entity<CarInsuranceType>()
                .HasMany(e => e.Cars)
                .WithOptional(e => e.CarInsuranceType)
                .HasForeignKey(e => e.IdCarInsuranceType);

            modelBuilder.Entity<CarInsuranceType>()
                .HasMany(e => e.CarGlobalToInsuranceTypes)
                .WithRequired(e => e.CarInsuranceType)
                .HasForeignKey(e => e.IdInsuraceType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CarInsuranceType>()
                .HasMany(e => e.K1)
                .WithRequired(e => e.CarInsuranceType)
                .HasForeignKey(e => e.IdCarInsuranceType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CarInsuranceType>()
                .HasMany(e => e.K2)
                .WithRequired(e => e.CarInsuranceType)
                .HasForeignKey(e => e.IdCarInsuranceType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CarInsuranceType>()
                .HasMany(e => e.K3)
                .WithRequired(e => e.CarInsuranceType)
                .HasForeignKey(e => e.IdCarInsuranceType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CityOfRegistration>()
                .HasMany(e => e.CityOrCountryOfRegToZones)
                .WithOptional(e => e.CityOfRegistration)
                .HasForeignKey(e => e.IdCityOfRegistration);

            modelBuilder.Entity<CityOfRegistration>()
                .HasMany(e => e.TSCs)
                .WithRequired(e => e.CityOfRegistration)
                .HasForeignKey(e => e.IdCityOfRegistration)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Client>()
                .HasMany(e => e.ClientCars)
                .WithRequired(e => e.Client)
                .HasForeignKey(e => e.IdClient)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Client>()
                .HasMany(e => e.Contracts)
                .WithRequired(e => e.Client)
                .HasForeignKey(e => e.IdClient)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Client>()
                .HasMany(e => e.Documents)
                .WithRequired(e => e.Client)
                .HasForeignKey(e => e.IdClient)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ClientCar>()
                .HasMany(e => e.Contracts)
                .WithRequired(e => e.ClientCar)
                .HasForeignKey(e => e.IdClientCar)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.Charts)
                .WithRequired(e => e.Company)
                .HasForeignKey(e => e.IdCompany)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.CityOrCountryOfRegToZones)
                .WithRequired(e => e.Company)
                .HasForeignKey(e => e.IdCompany)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.CompanyDetails)
                .WithRequired(e => e.Company)
                .HasForeignKey(e => e.IdCompany)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.CompanyFeatureToCompanies)
                .WithRequired(e => e.Company)
                .HasForeignKey(e => e.IdCompany)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.CompanyIMGs)
                .WithRequired(e => e.Company)
                .HasForeignKey(e => e.IdCompany)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.CompanyMiddlemen)
                .WithRequired(e => e.Company)
                .HasForeignKey(e => e.IdCompany)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyContractType>()
                .HasMany(e => e.ContractFranchises)
                .WithRequired(e => e.CompanyContractType)
                .HasForeignKey(e => e.IdCompanyContractType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyFeature>()
                .HasMany(e => e.CompanyFeatureToCompanies)
                .WithRequired(e => e.CompanyFeature)
                .HasForeignKey(e => e.IdFeature)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyMiddleman>()
                .HasMany(e => e.BonusMalus)
                .WithRequired(e => e.CompanyMiddleman)
                .HasForeignKey(e => e.IdCompanyMiddleman)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyMiddleman>()
                .HasMany(e => e.CompanyContractTypes)
                .WithRequired(e => e.CompanyMiddleman)
                .HasForeignKey(e => e.IdCompanyMiddleman)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyMiddleman>()
                .HasMany(e => e.Contracts)
                .WithRequired(e => e.CompanyMiddleman)
                .HasForeignKey(e => e.IdCompanyMiddleman)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyMiddleman>()
                .HasMany(e => e.K1)
                .WithOptional(e => e.CompanyMiddleman)
                .HasForeignKey(e => e.IdCompanyMiddleman);

            modelBuilder.Entity<CompanyMiddleman>()
                .HasMany(e => e.K2)
                .WithRequired(e => e.CompanyMiddleman)
                .HasForeignKey(e => e.IdCompanyMiddleman)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyMiddleman>()
                .HasMany(e => e.K3)
                .WithRequired(e => e.CompanyMiddleman)
                .HasForeignKey(e => e.IdCompanyMiddleman)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyMiddleman>()
                .HasMany(e => e.K4)
                .WithRequired(e => e.CompanyMiddleman)
                .HasForeignKey(e => e.IdCompanyMiddleman)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ContractFranchise>()
                .HasMany(e => e.BonusMalus)
                .WithRequired(e => e.ContractFranchise)
                .HasForeignKey(e => e.IdContractFranchise)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ContractFranchise>()
                .HasMany(e => e.Contracts)
                .WithRequired(e => e.ContractFranchise)
                .HasForeignKey(e => e.IdContractFranchise)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ContractFranchise>()
                .HasMany(e => e.K2)
                .WithRequired(e => e.ContractFranchise)
                .HasForeignKey(e => e.IdContractFranchise)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ContractFranchise>()
                .HasMany(e => e.K4)
                .WithRequired(e => e.ContractFranchise)
                .HasForeignKey(e => e.IdContractFranchise)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ContractType>()
                .HasMany(e => e.CompanyContractTypes)
                .WithRequired(e => e.ContractType)
                .HasForeignKey(e => e.IdContractType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ContractType>()
                .HasMany(e => e.Contracts)
                .WithRequired(e => e.ContractType)
                .HasForeignKey(e => e.IdContractType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DiscountByQuantity>()
                .HasMany(e => e.Contracts)
                .WithRequired(e => e.DiscountByQuantity)
                .HasForeignKey(e => e.IdDiscountByQuantity)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DiscountForClientWithPrivilegy>()
                .HasMany(e => e.Contracts)
                .WithRequired(e => e.DiscountForClientWithPrivilegy)
                .HasForeignKey(e => e.IdDiscountForClientWithPrivilegies)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DocumentType>()
                .HasMany(e => e.DiscountForClientWithPrivilegies)
                .WithRequired(e => e.DocumentType)
                .HasForeignKey(e => e.IdDocumentType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DocumentType>()
                .HasMany(e => e.Documents)
                .WithRequired(e => e.DocumentType)
                .HasForeignKey(e => e.IdDocumentType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Franchise>()
                .HasMany(e => e.ContractFranchises)
                .WithRequired(e => e.Franchise)
                .HasForeignKey(e => e.IdFranchise)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ImageType>()
                .HasMany(e => e.CompanyIMGs)
                .WithRequired(e => e.ImageType)
                .HasForeignKey(e => e.IdImageType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InsuranceZoneOfRegistration>()
                .HasMany(e => e.BonusMalus)
                .WithRequired(e => e.InsuranceZoneOfRegistration)
                .HasForeignKey(e => e.IdInsuranceZoneOfReg)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InsuranceZoneOfRegistration>()
                .HasMany(e => e.CityOrCountryOfRegToZones)
                .WithOptional(e => e.InsuranceZoneOfRegistration)
                .HasForeignKey(e => e.IdInsuranceZoneOfReg);

            modelBuilder.Entity<InsuranceZoneOfRegistration>()
                .HasMany(e => e.K2)
                .WithRequired(e => e.InsuranceZoneOfRegistration)
                .HasForeignKey(e => e.IdInsuranceZoneOfReg)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InsuranceZoneOfRegistration>()
                .HasMany(e => e.K3)
                .WithRequired(e => e.InsuranceZoneOfRegistration)
                .HasForeignKey(e => e.IdInsuranceZoneOfReg)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InsuranceZoneOfRegistration>()
                .HasMany(e => e.K4)
                .WithRequired(e => e.InsuranceZoneOfRegistration)
                .HasForeignKey(e => e.IdInsuranceZoneOfReg)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<K1>()
                .HasMany(e => e.Contracts)
                .WithRequired(e => e.K1)
                .HasForeignKey(e => e.IdK1)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<K2>()
                .HasMany(e => e.Contracts)
                .WithRequired(e => e.K2)
                .HasForeignKey(e => e.IdK2)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<K3>()
                .HasMany(e => e.Contracts)
                .WithRequired(e => e.K3)
                .HasForeignKey(e => e.IdK3)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<K4>()
                .HasMany(e => e.Contracts)
                .WithRequired(e => e.K4)
                .HasForeignKey(e => e.IdK4)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<K5>()
                .HasMany(e => e.Contracts)
                .WithRequired(e => e.K5)
                .HasForeignKey(e => e.IdK5)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<K6>()
                .HasMany(e => e.Contracts)
                .WithRequired(e => e.K6)
                .HasForeignKey(e => e.IdK6)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<K7>()
                .HasMany(e => e.Contracts)
                .WithRequired(e => e.K7)
                .HasForeignKey(e => e.IdK7)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Middleman>()
                .HasMany(e => e.CompanyMiddlemen)
                .WithRequired(e => e.Middleman)
                .HasForeignKey(e => e.IdMiddleman)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Privilege>()
                .HasMany(e => e.Clients)
                .WithOptional(e => e.Privilege)
                .HasForeignKey(e => e.IdPrivileges);

            modelBuilder.Entity<RegioneOfRegistration>()
                .HasMany(e => e.CityOfRegistrations)
                .WithOptional(e => e.RegioneOfRegistration)
                .HasForeignKey(e => e.IdRegioneOfRegistration);

            modelBuilder.Entity<TSC>()
                .HasMany(e => e.CityOrCountryOfRegToZones)
                .WithOptional(e => e.TSC)
                .HasForeignKey(e => e.IdTSC);
        }
    }
}
