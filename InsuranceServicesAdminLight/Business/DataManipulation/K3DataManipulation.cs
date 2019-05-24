using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuranceServicesAdminLight.Models;

namespace InsuranceServicesAdminLight.Business.DataManipulation
{
    public class K3DataManipulation
    {
        static InsuranceServicesContext db = new InsuranceServicesContext();

        static public List<K3> GetMulti(int idCompanyMiddleman)
        {
            return db.K3.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman).OrderBy(k => k.CarInsuranceType.Type)
                      .OrderBy(k => k.InsuranceZoneOfRegistration.Name)
                      .ThenBy(k => k.IsLegalEntity)
                      .ThenBy(k => k.CarInsuranceType.Type)
                      .ToList();
        }

        static public bool IsConditionExist(int idInsuranceZoneOfReg, bool isLegalEntity, int idCarInsuranceType, int idCompanyMiddleman)
        {
            return db.K3.Where(k => k.IdInsuranceZoneOfReg == idInsuranceZoneOfReg
                                                    && k.IsLegalEntity == isLegalEntity
                                                    && k.IdCarInsuranceType == idCarInsuranceType
                                                    && k.IdCompanyMiddleman == idCompanyMiddleman).Count() == 0;
        }

        static public void Insert(K3 newRecord)
        {
            db.K3.Add(newRecord);
            db.SaveChanges();
        }
    }
}