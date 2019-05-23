using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuranceServicesAdminLight.Models;

namespace InsuranceServicesAdminLight.Business.DataManipulation
{
    public class CompanyContractTypeDataManipulation
    {
        static InsuranceServicesContext db = new InsuranceServicesContext();

        static public int GetId(int idCompanyMiddleman, int idContractType)
        {
            return  db.CompanyContractTypes
                      .Where(m => m.IdCompanyMiddleman == idCompanyMiddleman && m.IdContractType == idContractType)
                      .Select(i => i.Id)
                      .FirstOrDefault();
        }
    }
}