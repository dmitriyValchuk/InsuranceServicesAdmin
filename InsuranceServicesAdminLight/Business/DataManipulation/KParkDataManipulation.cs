using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuranceServicesAdminLight.Models;

namespace InsuranceServicesAdminLight.Business.DataManipulation
{
    public class KParkDataManipulation
    {
        static InsuranceServicesContext db = new InsuranceServicesContext();

        public static List<DiscountByQuantity> GetMulti()
        {
            return db.DiscountByQuantities
                          .OrderBy(k => k.TransportCountFrom)
                          .ThenBy(k => k.IsLegalEntity)
                          .ToList();
        }
    }
}