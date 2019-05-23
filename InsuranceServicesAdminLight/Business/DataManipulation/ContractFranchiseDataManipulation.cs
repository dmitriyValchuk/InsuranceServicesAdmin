using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuranceServicesAdminLight.Models;

namespace InsuranceServicesAdminLight.Business.DataManipulation
{
    public class ContractFranchiseDataManipulation
    {
        static InsuranceServicesContext db = new InsuranceServicesContext();

        static public List<int> GetFranchiseIds(int idCompanyContractType)
        {
            return db.ContractFranchises.Where(cf => cf.IdCompanyContractType == idCompanyContractType).Select(cf => cf.IdFranchise).ToList();
        }

        static public int GetId(int idCompanyContractType, int idFranchise)
        {
            return db.ContractFranchises
                     .Where(cf => cf.IdCompanyContractType == idCompanyContractType && cf.IdFranchise == idFranchise)
                     .Select(cf => cf.Id)
                     .FirstOrDefault();
        }
    }
}