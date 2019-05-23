using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuranceServicesAdminLight.Models;

namespace InsuranceServicesAdminLight.Business.DataManipulation
{
    public class ContractTypeDataManipulation
    {
        static InsuranceServicesContext db = new InsuranceServicesContext();

        static public int GetId(string name)
        {
            return db.ContractTypes.Where(n => n.Name == name).Select(i => i.Id).FirstOrDefault();
        }
    }
}