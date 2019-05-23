using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuranceServicesAdminLight.Models;

namespace InsuranceServicesAdminLight.Business.DataManipulation
{
    public class MiddlemanDataManipulation
    {
        static InsuranceServicesContext db = new InsuranceServicesContext();

        static public int GetId(string name)
        {
            return db.Middlemen.Where(m => m.FullName == name).Select(m => m.Id).FirstOrDefault();
        }

        static public string GetName(int id)
        {
            return db.Middlemen.Where(m => m.Id == id).Select(m => m.FullName).FirstOrDefault();
        }
    }
}