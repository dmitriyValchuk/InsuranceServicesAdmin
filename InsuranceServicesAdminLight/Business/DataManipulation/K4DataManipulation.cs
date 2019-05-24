using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuranceServicesAdminLight.Models;

namespace InsuranceServicesAdminLight.Business.DataManipulation
{
    public class K4DataManipulation
    {
        static InsuranceServicesContext db = new InsuranceServicesContext();

        public static List<K4> GetMulti(int idCompanyMiddleman)
        {
            return db.K4.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman)
                      .OrderBy(k => k.InsuranceZoneOfRegistration.Name)
                      .ThenBy(k => k.IsLegalEntity)
                      .ThenBy(k => k.ContractFranchise.Franchise.Sum)
                      .ToList();
        }

        public static bool IsConditionExist(int idInsuranceZoneOfReg, bool isLegalEntity, int idContractFranchise, int idCompanyMiddleman)
        {
            return db.K4.Where(k => k.IdInsuranceZoneOfReg == idInsuranceZoneOfReg
                                                    && k.IsLegalEntity == isLegalEntity
                                                    && k.IdContractFranchise == idContractFranchise
                                                    && k.IdCompanyMiddleman == idCompanyMiddleman).Count() == 0;
        }

        public static void Insert(K4 newRecord)
        {
            db.K4.Add(newRecord);
            db.SaveChanges();
        }
    }
}