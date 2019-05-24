using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuranceServicesAdminLight.Models;

namespace InsuranceServicesAdminLight.Business.DataManipulation
{
    public class K6DataManipulation
    {
        static InsuranceServicesContext db = new InsuranceServicesContext();

        public static List<K6> GetMulti()
        {
            return db.K6.OrderBy(k => k.IsCheater).ToList();
        }
    }
}