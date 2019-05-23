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
    }
}