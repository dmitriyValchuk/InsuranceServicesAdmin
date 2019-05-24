using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuranceServicesAdminLight.Models;

namespace InsuranceServicesAdminLight.Business.DataManipulation
{
    public class BonusMalusDataManipulation
    {
        static InsuranceServicesContext db = new InsuranceServicesContext();

        public static List<BonusMalus> GetMulti(int idCompanyMiddleman)
        {
            return db.BonusMalus.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman)
                              .OrderBy(k => k.InsuranceZoneOfRegistration.Name)
                              .ThenBy(k => k.IsLegalEntity)
                              .ThenBy(k => k.CarInsuranceType.Type)
                              .ThenBy(k => k.ContractFranchise.Franchise)
                              .ToList();
        }

        public static bool IsConditionExist(int idInsuranceZoneOfReg, bool isLegalEntity, int idCarInsuranceType, int idContractFranchise, int idCompanyContractType, int idCompanyMiddleman)
        {
            return db.K2.Where(k => k.IdInsuranceZoneOfReg == idInsuranceZoneOfReg
                                                            && k.IsLegalEntity == isLegalEntity
                                                            && k.IdCarInsuranceType == idCarInsuranceType
                                                            && k.IdContractFranchise == idContractFranchise
                                                            && k.IdCompanyMiddleman == idCompanyMiddleman).Count() == 0;
        }

        public static void Insert(BonusMalus newRecord)
        {
            db.BonusMalus.Add(newRecord);
            db.SaveChanges();
        }
    }
}