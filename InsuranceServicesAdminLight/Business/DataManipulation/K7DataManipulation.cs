using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuranceServicesAdminLight.Models;

namespace InsuranceServicesAdminLight.Business.DataManipulation
{
    public class K7DataManipulation
    {
        static InsuranceServicesContext db = new InsuranceServicesContext();

        public static List<K7> GetMulti()
        {
            return db.K7.OrderBy(k => k.Period).ToList();
        }
    }
}