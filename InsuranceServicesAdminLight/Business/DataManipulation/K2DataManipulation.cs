using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuranceServicesAdminLight.Models;

namespace InsuranceServicesAdminLight.Business.DataManipulation
{
    public class K2DataManipulation
    {
        static InsuranceServicesContext db = new InsuranceServicesContext();

        static public List<K2> GetMulti(int idCompanyMiddleman)
        {
            return db.K2.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman).ToList();
        }

        static public bool IsConditionExist(int idInsuranceZoneOfReg, bool isLegalEntity, int idCarInsuranceType, int idContractFranchise, int idCompanyContractType, int idCompanyMiddleman)
        {
            return db.K2.Where(k => k.IdInsuranceZoneOfReg == idInsuranceZoneOfReg
                                                        && k.IsLegalEntity == isLegalEntity
                                                        && k.IdCarInsuranceType == idCarInsuranceType
                                                        && k.IdContractFranchise == ContractFranchiseDataManipulation.GetId(idCompanyContractType, idContractFranchise)
                                                        && k.IdCompanyMiddleman == idCompanyMiddleman).Count() == 0;
        }

        static public void Insert(K2 newRecord)
        {
            db.K2.Add(newRecord);
            db.SaveChanges();
        }
    }
}