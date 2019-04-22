using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InsuranceServicesAdminLight.Models;
using System.Web.Script.Serialization;

namespace InsuranceServicesAdminLight.Controllers
{
    public class ContractsController : Controller
    {
        InsuranceServicesContext db = new InsuranceServicesContext();

        public string GetCheckedCompany(string companyChecked)
        {
            //var companyFromJS = new JavaScriptSerializer().Deserialize<string>(companyChecked.ToString());
            var company = db.Companies.Where(c => c.Name == companyChecked).FirstOrDefault();
            if (company == null)
            {
                return "Error! Company not found!";
            }
            else
            {
                var companyMiddleMan = db.CompanyMiddlemen.Where(ci => ci.IdCompany == company.Id);
                List<string> middlemen = new List<string>();
                if (companyMiddleMan == null)
                    return "Error! Company hasn`t any middleman!";
                foreach (var cm in companyMiddleMan)
                {
                    var currentMiddleman = db.Middlemen.Where(m => m.Id == cm.IdMiddleman).FirstOrDefault();
                    middlemen.Add(currentMiddleman.FullName);
                }

                JavaScriptSerializer js = new JavaScriptSerializer();
                return js.Serialize(middlemen);

            }
        }

        [HttpPost]
        public string GetAllCompanies()
        {
            InsuranceServicesContext db = new InsuranceServicesContext();
            var companies = db.Companies.ToArray();
            List<CompanyToSend> compList = new List<CompanyToSend>();

            foreach (var c in companies)
            {
                CompanyToSend cts = new CompanyToSend();
                cts.Id = c.Id;
                cts.Name = c.Name;
                cts.Code = c.Code;
                compList.Add(cts);
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(compList);
        }

        public string GetConditionsForCoefK1(string companyName, string middlemanName)
        {
            int idCompany, idMiddleman, idCompanyMiddleman;
            if (companyName == "")
                return "Error! Company is empty";

            idCompany = db.Companies.Where(ci => ci.Name == companyName).Select(c => c.Id).FirstOrDefault();

            if (idCompany == 0)
                return "Error! Company not found";

            if (middlemanName == "")
                return "Error! Middleman is empty";

            idMiddleman = db.Middlemen.Where(mi => mi.FullName == middlemanName).Select(m => m.Id).FirstOrDefault();

            if (idMiddleman == 0)
                return "Error! Middleman not found";

            idCompanyMiddleman = db.CompanyMiddlemen.Where(ci => ci.IdCompany == idCompany && ci.IdMiddleman == idMiddleman).Select(cm => cm.Id).FirstOrDefault();

            if (idCompanyMiddleman == 0)
                return "Error! Company hasn`t that middleman";

            var K1 = db.K1.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman);
            List<TableK1ToSend> K1Table = new List<TableK1ToSend>();

            var carInsuranceType = db.CarInsuranceTypes.ToList();
            if (carInsuranceType.Count == 0)
                return "Error! List car type is empty";

            foreach (var cit in carInsuranceType)
            {
                if (db.K1.Where(i => i.IdCarInsuranceType == cit.Id && i.IdCompanyMiddleman == idCompanyMiddleman).Count() == 0)
                {
                    var newRowK1 = new K1()
                    {
                        IdCarInsuranceType = cit.Id,
                        Value = 0,
                        IdCompanyMiddleman = idCompanyMiddleman,
                    };

                    db.K1.Add(newRowK1);
                    db.SaveChanges();
                }
            }

            K1 = db.K1.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman);

            foreach (var k in K1)
            {
                TableK1ToSend tempTableRow = new TableK1ToSend();
                tempTableRow.InsuranceTypeOfCar = db.CarInsuranceTypes.Where(cit => cit.Id == k.IdCarInsuranceType).Select(cit => cit.Type).FirstOrDefault();
                tempTableRow.Value = k.Value;
                K1Table.Add(tempTableRow);
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(K1Table);
        }

        public string GetConditionsForCoefK2(string companyName, string middlemanName)
        {
            //companyName = "ТАС";
            //middlemanName = "Жильченкова Олена";
            int idCompany, idMiddleman, idCompanyMiddleman;
            if (companyName == "")
                return "Error! Company is empty";

            idCompany = db.Companies.Where(ci => ci.Name == companyName).Select(c => c.Id).FirstOrDefault();

            if (idCompany == 0)
                return "Error! Company not found";

            if (middlemanName == "")
                return "Error! Middleman is empty";

            idMiddleman = db.Middlemen.Where(mi => mi.FullName == middlemanName).Select(m => m.Id).FirstOrDefault();

            if (idMiddleman == 0)
                return "Error! Middleman not found";

            idCompanyMiddleman = db.CompanyMiddlemen.Where(ci => ci.IdCompany == idCompany && ci.IdMiddleman == idMiddleman).Select(cm => cm.Id).FirstOrDefault();

            if (idCompanyMiddleman == 0)
                return "Error! Company hasn`t that middleman";

            var K2 = db.K2.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman);
            List<TableK2ToSend> K2Table = new List<TableK2ToSend>();

            int idContractType = db.ContractTypes.Where(n => n.Name == "ГО").Select(i => i.Id).FirstOrDefault();
            if (idContractType == 0)
                return "Error! Contract Type not found";

            int idCompanyContractType = db.CompanyContractTypes
                .Where(m => m.IdCompanyMiddleman == idCompanyMiddleman && m.IdContractType == idContractType)
                .Select(i => i.Id)
                .FirstOrDefault();
            if (idCompanyContractType == 0)
                return "Error! In this middlemant not exit this type of contract";

            var idsFranchise = db.ContractFranchises.Where(cct => cct.IdCompanyContractType == idCompanyContractType).Select(i => i.IdFranchise).ToList();
            if (idsFranchise.Count == 0)
                return "Error! This contract hasn`t franchise";

            var franchise = db.Franchises.Where(i => idsFranchise.Contains(i.Id)).ToList();
            if (franchise.Count == 0)
                return "Error! In Data Base not exist franchise for this conditions!";

            var insuranceZoneOfReg = db.InsuranceZoneOfRegistrations.ToList();
            var isLegal = new List<bool> { true, false };//db.K2.Select(l => l.IsLegalEntity).ToList();
            var insuranceTypeOfCar = db.CarInsuranceTypes.ToList();

            //Block for test start
            //List<InsCarType> ict = new List<InsCarType>();
            //List<InsZoneOfReg> izor = new List<InsZoneOfReg>();
            //List<bool> isLegalBool = new List<bool>();
            //List<Fran> fran = new List<Fran>();
            //foreach (var i in insuranceZoneOfReg)
            //{
            //    InsZoneOfReg curZoneOfReg = new InsZoneOfReg();
            //    curZoneOfReg.id = i.Id;
            //    curZoneOfReg.name = i.Name;
            //    izor.Add(curZoneOfReg);
            //}
            //foreach (var i in isLegal)
            //{
            //    isLegalBool.Add(i);
            //}
            //foreach (var i in insuranceTypeOfCar)
            //{
            //    InsCarType curIcsCarType = new InsCarType();
            //    curIcsCarType.id = i.Id;
            //    curIcsCarType.name = i.Type;
            //    ict.Add(curIcsCarType);
            //}
            //foreach (var i in franchise)
            //{
            //    Fran curFran = new Fran();
            //    curFran.id = i.Id;
            //    curFran.sum = i.Sum;
            //    fran.Add(curFran);
            //}
            //var z = izor;
            //var t = ict;
            //var l = isLegalBool;
            //var f = fran;

            //var someVar = 10;
            //Block for test end

            ////List<List<K2>> check = new List<List<K2>>();

            ////foreach (var i in insuranceZoneOfReg)
            ////{
            ////    foreach (var j in isLegal)
            ////    {
            ////        foreach (var q in insuranceTypeOfCar)
            ////        {
            ////            foreach (var w in franchise)
            ////            {
            ////                if (db.K2.Where(k => k.IdInsuranceZoneOfReg == i.Id
            ////                                            && k.IsLegalEntity == j
            ////                                            && k.IdCarInsuranceType == q.Id
            ////                                            && k.ContractFranchise.IdFranchise == w.Id
            ////                                            && k.IdCompanyMiddleman == idMiddleman).Count() == 0)
            ////                {
            ////                    var newRowK2 = new K2()
            ////                    {
            ////                        IdInsuranceZoneOfReg = i.Id,
            ////                        IsLegalEntity = j,
            ////                        IdCarInsuranceType = q.Id,
            ////                        IdContractFranchise = db.ContractFranchises.Where(idf => idf.IdFranchise == w.Id).Select(x => x.Id).FirstOrDefault(),
            ////                        Value = 0,
            ////                        IdCompanyMiddleman = idCompanyMiddleman,
            ////                    };

            ////                    db.K2.Add(newRowK2);
            ////                    db.SaveChanges();
            ////                }
            ////            }
            ////        }
            ////    }
            ////}

            //var control = check;

            //for (int i = 0; i < insuranceZoneOfReg.Count; i++)
            //{
            //    for (int j = 0; j < isLegal.Count; j++)
            //    {
            //        for (int q = 0; q < insuranceTypeOfCar.Count; q++)
            //        {
            //            for (int w = 0; w < franchise.Count; w++)
            //            {
            //                check.Add(db.K2.Where(k => k.IdCarInsuranceType == insuranceTypeOfCar[q].Id).ToList());
            //                var a = 0;
            //var z = insuranceZoneOfReg[i].Id;
            //var t = insuranceTypeOfCar[q].Id;
            //var l = isLegal[j];
            //var f = franchise[w].Id;
            //var a = db.K2.Where(k => k.IdInsuranceZoneOfReg == insuranceZoneOfReg[i].Id
            //                            && k.IsLegalEntity == isLegal[j]
            //                            && k.IdCarInsuranceType == insuranceTypeOfCar[q].Id
            //                            && k.ContractFranchise.IdFranchise == franchise[w].Id
            //                            && k.IdCompanyMiddleman == idMiddleman).Count() == 0;

            //bool isTrue = db.K2.Where(k => k.IdInsuranceZoneOfReg == insuranceZoneOfReg[i].Id
            //                            && k.IsLegalEntity == isLegal[j]
            //                            && k.IdCarInsuranceType == insuranceTypeOfCar[q].Id
            //                            && k.ContractFranchise.IdFranchise == franchise[w].Id
            //                            && k.IdCompanyMiddleman == idMiddleman).Count() == 0;
            //if (isTrue)
            //{
            //    var newRowK2 = new K2()
            //    {
            //        IdInsuranceZoneOfReg = insuranceZoneOfReg[i].Id,
            //        IsLegalEntity = isLegal[j],
            //        IdCarInsuranceType = insuranceTypeOfCar[q].Id,
            //        IdContractFranchise = db.ContractFranchises.Where(idf => idf.IdFranchise == franchise[w].Id).Select(x => x.Id).FirstOrDefault(),
            //        Value = 0,
            //        IdCompanyMiddleman = idCompanyMiddleman,
            //    };

            //    db.K2.Add(newRowK2);
            //    db.SaveChanges();
            //}
            //            }
            //        }
            //    }
            //}

            ////K2 = db.K2.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman)
            ////    .OrderBy(i => i.IdInsuranceZoneOfReg)
            ////    .ThenBy(i => i.IsLegalEntity)
            ////    .ThenBy(i => i.CarInsuranceType)
            ////    .ThenBy(i => i.ContractFranchise);
            foreach (var k in K2)
            {
                TableK2ToSend tempTableRow = new TableK2ToSend();
                tempTableRow.CarZoneOfRegistration = db.InsuranceZoneOfRegistrations
                                                        .Where(czor => czor.Id == k.IdInsuranceZoneOfReg)
                                                        .Select(czor => czor.Name)
                                                        .FirstOrDefault();
                tempTableRow.IsLegalEntity = k.IsLegalEntity.ToString();
                tempTableRow.InsuranceTypeOfCar = db.CarInsuranceTypes
                                                    .Where(cit => cit.Id == k.IdCarInsuranceType)
                                                    .Select(cit => cit.Type)
                                                    .FirstOrDefault();
                tempTableRow.Franchise = db.Franchises.Where(i => i.Id == k.ContractFranchise.IdFranchise).Select(i => i.Sum).FirstOrDefault();
                tempTableRow.Value = k.Value;
                K2Table.Add(tempTableRow);
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(K2Table);
        }
    }

    //Block test class start
    public class InsCarType
    {
        public int id { get; set; }
        public string name { get; set; }
    }
    public class InsZoneOfReg
    {
        public int id { get; set; }
        public string name { get; set; }
    }
    public class Fran
    {
        public int id { get; set; }
        public double sum { get; set; }
    }
    //Block test class end

    public class TableK1ToSend
    {
        public string InsuranceTypeOfCar { get; set; }
        public double Value { get; set; }
    }

    public class TableK2ToSend
    {
        public string CarZoneOfRegistration { get; set; }
        public string IsLegalEntity { get; set; }
        public string InsuranceTypeOfCar { get; set; }
        public double Franchise { get; set; }
        public double Value { get; set; }
    }

    public class CompanyToSend
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Code { get; set; }
    }
}