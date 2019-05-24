using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuranceServicesAdminLight.Models;

namespace InsuranceServicesAdminLight.Business.DataManipulation
{
    public class K5DataManipulation
    {
        static InsuranceServicesContext db = new InsuranceServicesContext();

        public static List<K5> GetMulti()
        {
            return db.K5.OrderBy(k => k.Period).ToList();
        }
    }
}