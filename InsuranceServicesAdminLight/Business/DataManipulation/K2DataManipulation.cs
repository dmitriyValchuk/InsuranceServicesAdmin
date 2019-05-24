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
            return db.K2.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman).OrderBy(k => k.InsuranceZoneOfRegistration.Name)
                      .ThenBy(k => k.IsLegalEntity)
                      .ThenBy(k => k.CarInsuranceType.Type)
                      .ThenBy(k => k.ContractFranchise.Franchise.Sum)
                      .ToList();
        }

        static public bool IsConditionExist(int idInsuranceZoneOfReg, bool isLegalEntity, int idCarInsuranceType, int idContractFranchise, int idCompanyContractType, int idCompanyMiddleman)
        {
            var a = db.K2.Where(k => k.IdInsuranceZoneOfReg == idInsuranceZoneOfReg
                                                        && k.IsLegalEntity == isLegalEntity
                                                        && k.IdCarInsuranceType == idCarInsuranceType
                                                        && k.IdContractFranchise == idContractFranchise
                                                        && k.IdCompanyMiddleman == idCompanyMiddleman).Count() == 0;
            return db.K2.Where(k => k.IdInsuranceZoneOfReg == idInsuranceZoneOfReg
                                                        && k.IsLegalEntity == isLegalEntity
                                                        && k.IdCarInsuranceType == idCarInsuranceType
                                                        && k.IdContractFranchise == idContractFranchise
                                                        && k.IdCompanyMiddleman == idCompanyMiddleman).Count() == 0;
        }

        static public void Insert(K2 newRecord)
        {
            db.K2.Add(newRecord);
            db.SaveChanges();
        }
    }
}