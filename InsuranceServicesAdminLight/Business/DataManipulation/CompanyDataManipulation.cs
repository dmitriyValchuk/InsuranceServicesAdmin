using InsuranceServicesAdminLight.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuranceServicesAdminLight.Business.DataManipulation
{
    public class CompanyDataManipulation
    {
        static InsuranceServicesContext db = new InsuranceServicesContext();

        static public int GetId(string name)
        {
            return db.Companies.Where(c => c.Name == name).Select(c => c.Id).FirstOrDefault();
        }

        static public Company GetSingle(string name)
        {     
            return db.Companies.Where(c => c.Name == name).FirstOrDefault();
        }

        static public Company GetSingle(int id)
        {
            return db.Companies.Where(c => c.Id == id).FirstOrDefault();
        }

        static public List<Company> GetMulti(string name)
        {
            return db.Companies.Where(c => c.Name == name).ToList();
        }

        static public List<Company> GetMulti(int id)
        {
            return db.Companies.Where(c => c.Id == id).ToList();
        }

        static public List<string> GetCompaniesName()
        {
            return db.Companies.Select(c => c.Name).ToList();
        }

        static public List<string> GetCompanyMiddlemenName(Company company)
        {
            var idMiddlemen = db.CompanyMiddlemen.Where(ci => ci.IdCompany == company.Id).Select(cm => cm.IdMiddleman);
            List<string> middlemen = new List<string>();

            foreach(var id in idMiddlemen)
            {
                middlemen.Add(MiddlemanDataManipulation.GetName(id));
            }

            return middlemen;
        }

        static public int GetCompanyMiddlemanId(string companyName, string middlemanName)
        {
            int idCompanyMiddleman = db.CompanyMiddlemen.Where(cm => cm.IdCompany == CompanyDataManipulation.GetId(companyName) 
                                                                  && cm.IdMiddleman == MiddlemanDataManipulation.GetId(middlemanName))
                                                        .Select(cm => cm.Id).FirstOrDefault();
            return idCompanyMiddleman;
        }
    }
}