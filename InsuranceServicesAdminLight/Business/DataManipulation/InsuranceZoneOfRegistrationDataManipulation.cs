using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuranceServicesAdminLight.Models;

namespace InsuranceServicesAdminLight.Business.DataManipulation
{
    public class InsuranceZoneOfRegistrationDataManipulation
    {
        static InsuranceServicesContext db = new InsuranceServicesContext();

        static public List<InsuranceZoneOfRegistration> GetInsuranceZoneOfRegistrations()
        {
            return db.InsuranceZoneOfRegistrations.ToList();
        }

        static public string GetInsuranceZoneOfRegistrationStr(int idInsuranceZoneOfReg)
        {
            return db.InsuranceZoneOfRegistrations
                     .Where(czor => czor.Id == idInsuranceZoneOfReg)
                     .Select(czor => czor.Name)
                     .FirstOrDefault();
        }
    }
}