using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuranceServicesAdminLight.Models;

namespace InsuranceServicesAdminLight.Business.DataManipulation
{
    public class K1DataManipulation
    {
        static InsuranceServicesContext db = new InsuranceServicesContext();

        static public List<K1> GetMulti(int idCompanyMiddleman)
        {
            return db.K1.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman).OrderBy(k => k.CarInsuranceType.Type).ToList();
        }

        static public bool IsConditionExist(CarInsuranceType carInsuranceType, int idCompanyMiddleman)
        {
            return db.K1.Where(i => i.IdCarInsuranceType == carInsuranceType.Id && i.IdCompanyMiddleman == idCompanyMiddleman).Count() == 0;
        }

        static public void Insert(K1 newRecord)
        {
            db.K1.Add(newRecord);
            db.SaveChanges();
        }
    }
}