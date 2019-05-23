using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuranceServicesAdminLight.Models;

namespace InsuranceServicesAdminLight.Business.DataManipulation
{
    public class CarInsuranceTypeDataManipulation
    {
        static InsuranceServicesContext db = new InsuranceServicesContext();

        static public List<CarInsuranceType> GetMulti()
        {
            return db.CarInsuranceTypes.ToList();
        }
    }
}