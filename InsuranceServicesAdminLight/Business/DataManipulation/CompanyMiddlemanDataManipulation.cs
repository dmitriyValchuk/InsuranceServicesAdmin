using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuranceServicesAdminLight.Models;

namespace InsuranceServicesAdminLight.Business.DataManipulation
{
    public class CompanyMiddlemanDataManipulation
    {
        static InsuranceServicesContext db = new InsuranceServicesContext();

        static public int GetId(string companyName, string middlemanName)
        {
            int idCompany = CompanyDataManipulation.GetId(companyName);
            int idMiddleman = MiddlemanDataManipulation.GetId(middlemanName);

            return db.CompanyMiddlemen.Where(cm => cm.IdCompany == idCompany && cm.IdMiddleman == idMiddleman)
                                      .Select(cm => cm.Id)
                                      .FirstOrDefault();
        }
    }
}