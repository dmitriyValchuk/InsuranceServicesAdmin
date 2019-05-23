using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuranceServicesAdminLight.Models;

namespace InsuranceServicesAdminLight.Business.DataManipulation
{
    public class FranchiseDataManipulation
    {
        static InsuranceServicesContext db = new InsuranceServicesContext();

        static public Franchise GetFranchise(int id)
        {
            return db.Franchises.Where(f => f.Id == id).FirstOrDefault();
        }

        static public List<Franchise> GetFranchises(List<int> idsFranchise)
        {
            return db.Franchises.Where(i => idsFranchise.Contains(i.Id)).ToList();
        }

    }
}