﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InsuranceServicesAdminLight.Models;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.IO;

namespace InsuranceServicesAdminLight.Controllers
{
    public class ContractsController : Controller
    {
        InsuranceServicesContext db = new InsuranceServicesContext();
        
        enum ResponseType
        {
            Bad,
            Good            
        }

        class ResponseToClient
        {
            public ResponseType responseType { get; set; }
            public string responseText { get; set; }
        }

        const string GoodResponse = "Success";
        const string BadResponse = "Fail";
        
        public string GetCheckedCompany(string companyChecked)
        {
            ResponseToClient responseToClient = new ResponseToClient();

            var company = db.Companies.Where(c => c.Name == companyChecked).FirstOrDefault();

            JavaScriptSerializer js = new JavaScriptSerializer();

            if (company == null)
            {
                responseToClient.responseType = ResponseType.Bad;
                responseToClient.responseText = "Error! Company not found!";
                return js.Serialize(responseToClient);
            }
            else
            {
                var companyMiddleMan = db.CompanyMiddlemen.Where(ci => ci.IdCompany == company.Id);
                List<string> middlemen = new List<string>();

                if (companyMiddleMan == null)
                {
                    responseToClient.responseType = ResponseType.Bad;
                    responseToClient.responseText = "Error! Company hasn`t any middleman!";
                    return js.Serialize(responseToClient);
                }

                foreach (var cm in companyMiddleMan)
                {
                    var currentMiddleman = db.Middlemen.Where(m => m.Id == cm.IdMiddleman).FirstOrDefault();
                    middlemen.Add(currentMiddleman.FullName);
                }
                
                return js.Serialize(middlemen);
            }
        }

        [HttpPost]
        public string GetAllCompanies()
        {
            List<string> companiesNames = db.Companies.Select(c => c.Name).ToList();
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(companiesNames);            
        }

        public string GetConditionsForCoefK1(string companyName, string middlemanName)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            int idCompany = 0, idMiddleman = 0, idCompanyMiddleman = 0;

            ResponseToClient resultOfChekingCompanyMiddleman = GetCompanyMiddlemanData(companyName, middlemanName, ref idCompany, ref idMiddleman, ref idCompanyMiddleman);
            if (resultOfChekingCompanyMiddleman.responseType != ResponseType.Good)
                return js.Serialize(resultOfChekingCompanyMiddleman);

            var K1 = db.K1.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman);
            List<TableK1ToSend> K1Table = new List<TableK1ToSend>();

            var resultOfChekingExistingRows = InsertDataForK1(companyName, middlemanName, idCompanyMiddleman);
            if (resultOfChekingExistingRows != "Success!")
                return resultOfChekingExistingRows;

            K1 = db.K1.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman)
                      .OrderBy(k => k.CarInsuranceType.Type);

            
            foreach (var k in K1)
            {
                TableK1ToSend tempTableRow = new TableK1ToSend();
                tempTableRow.InsuranceTypeOfCar = db.CarInsuranceTypes.Where(cit => cit.Id == k.IdCarInsuranceType).Select(cit => cit.Type).FirstOrDefault();
                tempTableRow.Value = k.Value;
                K1Table.Add(tempTableRow);
            }

            
            return js.Serialize(K1Table);
        }

        public string GetConditionsForCoefK2(string companyName, string middlemanName)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            int idCompany = 0, idMiddleman = 0, idCompanyMiddleman = 0;

            ResponseToClient resultOfChekingCompanyMiddleman = GetCompanyMiddlemanData(companyName, middlemanName, ref idCompany, ref idMiddleman, ref idCompanyMiddleman);
            if (resultOfChekingCompanyMiddleman.responseType != ResponseType.Good)
                return js.Serialize(resultOfChekingCompanyMiddleman);

            var K2 = db.K2.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman);
            List<TableK2ToSend> K2Table = new List<TableK2ToSend>();

            var resultOfChekingExistingRows = InsertDataForK2(companyName, middlemanName, idCompanyMiddleman);
            if (resultOfChekingExistingRows != "Success!")
                return resultOfChekingExistingRows;

            K2 = db.K2.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman)
                      .OrderBy(k => k.InsuranceZoneOfRegistration.Name)
                      .ThenBy(k => k.IsLegalEntity)
                      .ThenBy(k => k.CarInsuranceType.Type)
                      .ThenBy(k => k.ContractFranchise.Franchise.Sum);

            foreach (var k in K2)
            {
                TableK2ToSend tempTableRow = new TableK2ToSend();
                tempTableRow.CarZoneOfRegistration = db.InsuranceZoneOfRegistrations
                                                        .Where(czor => czor.Id == k.IdInsuranceZoneOfReg)
                                                        .Select(czor => czor.Name)
                                                        .FirstOrDefault();
                tempTableRow.IsLegalEntity = k.IsLegalEntity ? "Юр" : "Фіз";
                tempTableRow.InsuranceTypeOfCar = db.CarInsuranceTypes
                                                    .Where(cit => cit.Id == k.IdCarInsuranceType)
                                                    .Select(cit => cit.Type)
                                                    .FirstOrDefault();
                tempTableRow.Franchise = db.Franchises.Where(i => i.Id == k.ContractFranchise.IdFranchise).Select(i => i.Sum).FirstOrDefault();
                //tempTableRow.Franchise = db.Franchises.Where(i => i.Id == k.ContractFranchise.IdFranchise && k.ContractFranchise.IdCompanyContractType == idContractType).Select(i => i.Sum).FirstOrDefault();
                tempTableRow.Value = k.Value;
                K2Table.Add(tempTableRow);
            }

            return js.Serialize(K2Table);
        }

        public string GetConditionsForCoefK3(string companyName, string middlemanName)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            int idCompany = 0, idMiddleman = 0, idCompanyMiddleman = 0;

            ResponseToClient resultOfChekingCompanyMiddleman = GetCompanyMiddlemanData(companyName, middlemanName, ref idCompany, ref idMiddleman, ref idCompanyMiddleman);
            if (resultOfChekingCompanyMiddleman.responseType != ResponseType.Good)
                return js.Serialize(resultOfChekingCompanyMiddleman);

            var K3 = db.K3.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman);
            List<TableK3ToSend> K3Table = new List<TableK3ToSend>();

            var resultOfChekingExistingRows = InsertDataForK3(companyName, middlemanName, idCompanyMiddleman);
            if (resultOfChekingExistingRows != "Success!")
                return resultOfChekingExistingRows;

            K3 = db.K3.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman)
                      .OrderBy(k => k.InsuranceZoneOfRegistration.Name)
                      .ThenBy(k => k.IsLegalEntity)
                      .ThenBy(k => k.CarInsuranceType.Type);

            foreach (var k in K3)
            {
                TableK3ToSend tempTableRow = new TableK3ToSend();
                tempTableRow.CarZoneOfRegistration = db.InsuranceZoneOfRegistrations
                                                        .Where(czor => czor.Id == k.IdInsuranceZoneOfReg)
                                                        .Select(czor => czor.Name)
                                                        .FirstOrDefault();
                tempTableRow.IsLegalEntity = k.IsLegalEntity ? "Юр" : "Фіз";
                tempTableRow.InsuranceTypeOfCar = db.CarInsuranceTypes
                                                    .Where(cit => cit.Id == k.IdCarInsuranceType)
                                                    .Select(cit => cit.Type)
                                                    .FirstOrDefault();
                tempTableRow.Value = k.Value;
                K3Table.Add(tempTableRow);
            }

            return js.Serialize(K3Table);
        }

        public string GetConditionsForCoefK4(string companyName, string middlemanName)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            int idCompany = 0, idMiddleman = 0, idCompanyMiddleman = 0;

            ResponseToClient resultOfChekingCompanyMiddleman = GetCompanyMiddlemanData(companyName, middlemanName, ref idCompany, ref idMiddleman, ref idCompanyMiddleman);
            if (resultOfChekingCompanyMiddleman.responseType != ResponseType.Good)
                return js.Serialize(resultOfChekingCompanyMiddleman);

            var K4 = db.K4.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman);
            List<TableK4ToSend> K4Table = new List<TableK4ToSend>();

            var resultOfChekingExistingRows = InsertDataForK4(companyName, middlemanName, idCompanyMiddleman);
            if (resultOfChekingExistingRows != "Success!")
                return resultOfChekingExistingRows;

            K4 = db.K4.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman)
                      .OrderBy(k => k.InsuranceZoneOfRegistration.Name)
                      .ThenBy(k => k.IsLegalEntity)
                      .ThenBy(k => k.ContractFranchise.Franchise.Sum);

            foreach (var k in K4)
            {
                TableK4ToSend tempTableRow = new TableK4ToSend();
                tempTableRow.CarZoneOfRegistration = db.InsuranceZoneOfRegistrations
                                                        .Where(czor => czor.Id == k.IdInsuranceZoneOfReg)
                                                        .Select(czor => czor.Name)
                                                        .FirstOrDefault();
                tempTableRow.IsLegalEntity = k.IsLegalEntity ? "Юр" : "Фіз";
                tempTableRow.Franchise = db.Franchises.Where(i => i.Id == k.ContractFranchise.IdFranchise).Select(i => i.Sum).FirstOrDefault();
                tempTableRow.Value = k.Value;
                K4Table.Add(tempTableRow);
            }

            return js.Serialize(K4Table);
        }

        public string GetConditionsForCoefK5()
        {
            var K5 = db.K5.ToList();
            List<TableK5ToSend> K5Table = new List<TableK5ToSend>();

            foreach (var k in K5)
            {
                TableK5ToSend tempTableRow = new TableK5ToSend();
                tempTableRow.Period = k.Period;
                tempTableRow.Value = k.Value;
                K5Table.Add(tempTableRow);
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(K5Table);
        }

        public string GetConditionsForCoefK6()
        {
            var K6 = db.K6.ToList();
            List<TableK6ToSend> K6Table = new List<TableK6ToSend>();

            foreach (var k in K6)
            {
                TableK6ToSend tempTableRow = new TableK6ToSend();
                tempTableRow.IsCheater = k.IsCheater.Value ? "Шахрай" : "Не шахрай";
                tempTableRow.Value = k.Value;
                K6Table.Add(tempTableRow);
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(K6Table);
        }

        public string GetConditionsForCoefK7()
        {
            var K7 = db.K7.ToList();
            List<TableK7ToSend> K7Table = new List<TableK7ToSend>();

            foreach (var k in K7)
            {
                TableK7ToSend tempTableRow = new TableK7ToSend();
                tempTableRow.Period = k.Period;
                tempTableRow.Value = k.Value;
                K7Table.Add(tempTableRow);
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(K7Table);
        }

        public string GetConditionsForCoefBM(string companyName, string middlemanName)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            int idCompany = 0, idMiddleman = 0, idCompanyMiddleman = 0;

            ResponseToClient resultOfChekingCompanyMiddleman = GetCompanyMiddlemanData(companyName, middlemanName, ref idCompany, ref idMiddleman, ref idCompanyMiddleman);
            if (resultOfChekingCompanyMiddleman.responseType != ResponseType.Good)
                return js.Serialize(resultOfChekingCompanyMiddleman);

            var BM = db.BonusMalus.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman);
            List<TableBMToSend> BMTable = new List<TableBMToSend>();

            var resultOfChekingExistingRows = InsertDataForK2(companyName, middlemanName, idCompanyMiddleman);
            if (resultOfChekingExistingRows != "Success!")
                return resultOfChekingExistingRows;

            BM = db.BonusMalus.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman)
                              .OrderBy(k => k.InsuranceZoneOfRegistration.Name)
                              .ThenBy(k => k.IsLegalEntity)
                              .ThenBy(k => k.CarInsuranceType.Type)
                              .ThenBy(k => k.ContractFranchise.Franchise);

            foreach (var k in BM)
            {
                TableBMToSend tempTableRow = new TableBMToSend();
                tempTableRow.CarZoneOfRegistration = db.InsuranceZoneOfRegistrations
                                                        .Where(czor => czor.Id == k.IdInsuranceZoneOfReg)
                                                        .Select(czor => czor.Name)
                                                        .FirstOrDefault();
                tempTableRow.IsLegalEntity = k.IsLegalEntity ? "Юр" : "Фіз";
                tempTableRow.InsuranceTypeOfCar = db.CarInsuranceTypes
                                                    .Where(cit => cit.Id == k.IdCarInsuranceType)
                                                    .Select(cit => cit.Type)
                                                    .FirstOrDefault();
                tempTableRow.Franchise = db.Franchises.Where(i => i.Id == k.ContractFranchise.IdFranchise).Select(i => i.Sum).FirstOrDefault();
                //tempTableRow.Franchise = db.Franchises.Where(i => i.Id == k.ContractFranchise.IdFranchise && k.ContractFranchise.IdCompanyContractType == idContractType).Select(i => i.Sum).FirstOrDefault();
                tempTableRow.Value = k.Value;
                BMTable.Add(tempTableRow);
            }

            return js.Serialize(BMTable);
        }

        public string GetConditionsForCoefKPark()
        {
            var KPark = db.DiscountByQuantities.ToList();
            List<TableKParkToSend> KParkTable = new List<TableKParkToSend>();

            foreach (var k in KPark)
            {
                TableKParkToSend tempTableRow = new TableKParkToSend();
                tempTableRow.IsLegalEntity = k.IsLegalEntity ? "Юр" : "Фіз";
                tempTableRow.TransportCountFrom = k.TransportCountFrom;
                tempTableRow.TransportCountTo = k.TransportCountTo;
                tempTableRow.Value = k.Value;
                KParkTable.Add(tempTableRow);
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(KParkTable);
        }

        public string RemoveDataFromTable(string coef, string companyName, string middlemanName, string data)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            int idCompany = 0, idMiddleman = 0, idCompanyMiddleman = 0;

            ResponseToClient resultOfChekingCompanyMiddleman = GetCompanyMiddlemanData(companyName, middlemanName, ref idCompany, ref idMiddleman, ref idCompanyMiddleman);
            if (resultOfChekingCompanyMiddleman.responseType != ResponseType.Good)
                return js.Serialize(resultOfChekingCompanyMiddleman);

            dynamic dataParsed = JsonConvert.DeserializeObject(data);

            ResponseToClient responseToClient = new ResponseToClient();

            switch (coef)
            {
                case "K1":
                    {
                        string currentInsuranceType = dataParsed.InsuranceTypeOfCar;
                        var recordForDel = db.K1.Where(k => k.CarInsuranceType.Type == currentInsuranceType
                                                         && k.IdCompanyMiddleman == idCompanyMiddleman).First();
                        if (recordForDel == null)
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Запис не знайдений в базі даних";
                        }
                        else
                        {
                            db.K1.Remove(recordForDel);
                            db.SaveChanges();
                            responseToClient.responseType = ResponseType.Good;
                            responseToClient.responseText = "Запис успішно видалений з бази даних";
                        }
                    }
                    break;
                case "K2":
                    {
                        string currentCarZone = dataParsed.CarZoneOfRegistration;
                        bool currentIsLegalEntity = dataParsed.IsLegalEntity == "Юр" ? true : false;
                        string currentInsuranceType = dataParsed.InsuranceTypeOfCar;
                        double currentFranchise = dataParsed.Franchise;

                        var recordForDel = db.K2.Where(k => k.InsuranceZoneOfRegistration.Name == currentCarZone
                                                         && k.IsLegalEntity == currentIsLegalEntity
                                                         && k.CarInsuranceType.Type == currentInsuranceType
                                                         && k.ContractFranchise.Franchise.Sum == currentFranchise
                                                         && k.IdCompanyMiddleman == idCompanyMiddleman).First();                        
                        if (recordForDel == null)
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Запис не знайдений в базі даних";
                        }
                        else
                        {
                            db.K2.Remove(recordForDel);
                            db.SaveChanges();
                            responseToClient.responseType = ResponseType.Good;
                            responseToClient.responseText = "Запис успішно видалений з бази даних";
                        }
                    }
                    break;
                case "K3":
                    {
                        string currentCarZone = dataParsed.CarZoneOfRegistration;
                        bool currentIsLegalEntity = dataParsed.IsLegalEntity == "Юр" ? true : false;
                        string currentInsuranceType = dataParsed.InsuranceTypeOfCar;

                        var recordForDel = db.K3.Where(k => k.InsuranceZoneOfRegistration.Name == currentCarZone
                                                         && k.IsLegalEntity == currentIsLegalEntity
                                                         && k.CarInsuranceType.Type == currentInsuranceType
                                                         && k.IdCompanyMiddleman == idCompanyMiddleman).First();
                        
                        if (recordForDel == null)
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Запис не знайдений в базі даних";
                        }
                        else
                        {
                            db.K3.Remove(recordForDel);
                            db.SaveChanges();
                            responseToClient.responseType = ResponseType.Good;
                            responseToClient.responseText = "Запис успішно видалений з бази даних";
                        }
                    }
                    break;
                case "K4":
                    {
                        string currentCarZone = dataParsed.CarZoneOfRegistration;
                        bool currentIsLegalEntity = dataParsed.IsLegalEntity == "Юр" ? true : false;
                        double currentFranchise = dataParsed.Franchise;

                        var recordForDel = db.K4.Where(k => k.InsuranceZoneOfRegistration.Name == currentCarZone
                                                         && k.IsLegalEntity == currentIsLegalEntity
                                                         && k.ContractFranchise.Franchise.Sum == currentFranchise
                                                         && k.IdCompanyMiddleman == idCompanyMiddleman).First();
                        
                        if (recordForDel == null)
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Запис не знайдений в базі даних";
                        }
                        else
                        {
                            db.K4.Remove(recordForDel);
                            db.SaveChanges();
                            responseToClient.responseType = ResponseType.Good;
                            responseToClient.responseText = "Запис успішно видалений з бази даних";
                        }
                    }
                    break;
                case "K5":
                    {
                        int currentPeriod = dataParsed.Period;

                        var recordForDel = db.K5.Where(k => k.Period == currentPeriod).First();
                        
                        if (recordForDel == null)
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Запис не знайдений в базі даних";
                        }
                        else
                        {
                            db.K5.Remove(recordForDel);
                            db.SaveChanges();
                            responseToClient.responseType = ResponseType.Good;
                            responseToClient.responseText = "Запис успішно видалений з бази даних";
                        }
                    }
                    break;
                case "K6":
                    {
                        bool currentIsCheater = dataParsed.IsCheater == "Шахрай" ? true : false;

                        var recordForDel = db.K6.Where(k => k.IsCheater == currentIsCheater).First();
                        
                        if (recordForDel == null)
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Запис не знайдений в базі даних";
                        }
                        else
                        {
                            db.K6.Remove(recordForDel);
                            db.SaveChanges();
                            responseToClient.responseType = ResponseType.Good;
                            responseToClient.responseText = "Запис успішно видалений з бази даних";
                        }
                    }
                    break;
                case "K7":
                    {
                        double currentPeriod = dataParsed.Period;

                        var recordForDel = db.K7.Where(k => k.Period == currentPeriod).First();
                        
                        if (recordForDel == null)
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Запис не знайдений в базі даних";
                        }
                        else
                        {
                            db.K7.Remove(recordForDel);
                            db.SaveChanges();
                            responseToClient.responseType = ResponseType.Good;
                            responseToClient.responseText = "Запис успішно видалений з бази даних";
                        }
                    }
                    break;
                case "BM":
                    {
                        string currentCarZone = dataParsed.CarZoneOfRegistration;
                        bool currentIsLegalEntity = dataParsed.IsLegalEntity == "Юр" ? true : false;
                        string currentInsuranceType = dataParsed.InsuranceTypeOfCar;
                        double currentFranchise = dataParsed.Franchise;

                        var recordForDel = db.BonusMalus.Where(k => k.InsuranceZoneOfRegistration.Name == currentCarZone
                                                         && k.IsLegalEntity == currentIsLegalEntity
                                                         && k.CarInsuranceType.Type == currentInsuranceType
                                                         && k.ContractFranchise.Franchise.Sum == currentFranchise
                                                         && k.IdCompanyMiddleman == idCompanyMiddleman).First();
                        
                        if (recordForDel == null)
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Запис не знайдений в базі даних";
                        }
                        else
                        {
                            db.BonusMalus.Remove(recordForDel);
                            db.SaveChanges();
                            responseToClient.responseType = ResponseType.Good;
                            responseToClient.responseText = "Запис успішно видалений з бази даних";
                        }
                    }
                    break;
                case "KPark":
                    {
                        bool currentIsLegalEntity = dataParsed.IsLegalEntity == "Юр" ? true : false;
                        int currentTransportCountFrom = dataParsed.TransportCountFrom;
                        int currentTransportCountTo = dataParsed.TransportCountTo;

                        var recordForDel = db.DiscountByQuantities.Where(k => k.IsLegalEntity == currentIsLegalEntity
                                                                            && k.TransportCountFrom == currentTransportCountFrom
                                                                            && k.TransportCountTo == currentTransportCountTo).First();
                        
                        if (recordForDel == null)
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Запис не знайдений в базі даних";
                        }
                        else
                        {
                            db.DiscountByQuantities.Remove(recordForDel);
                            db.SaveChanges();
                            responseToClient.responseType = ResponseType.Good;
                            responseToClient.responseText = "Запис успішно видалений з бази даних";
                        }
                    }
                    break;
                case "KPilg":
                    {
                        //return js.Serialize(GoodResponse);
                    }
                    break;
                default:
                    {
                        responseToClient.responseType = ResponseType.Bad;
                        responseToClient.responseText = "Невірно вказана назва коефіцієнту або коефіцієнт відсутній в базі даних";
                    }
                    break;
            }

            return js.Serialize(responseToClient);
        }

        [HttpPost]
        public string SaveSingleChangingInCoef()
        {
            int idCompany = 0, idMiddleman = 0, idCompanyMiddleman = 0;

            dynamic dataParsed = GetPotsRequestBody();
            string companyName = Convert.ToString(dataParsed.companyName);
            string middlemanName = Convert.ToString(dataParsed.middlemanName);
            string coef = Convert.ToString(dataParsed.coef);

            ResponseToClient responseToClient = new ResponseToClient();

            JavaScriptSerializer js = new JavaScriptSerializer();

            ResponseToClient resultOfChekingCompanyMiddleman = GetCompanyMiddlemanData(companyName, middlemanName, ref idCompany, ref idMiddleman, ref idCompanyMiddleman);
            if (resultOfChekingCompanyMiddleman.responseType != ResponseType.Good)
                return js.Serialize(resultOfChekingCompanyMiddleman);

            switch (coef)
            {
                case "K1":
                    {
                        string tempInsuranceTypeOfCar = dataParsed.data.InsuranceTypeOfCar;
                        var recordForChange = db.K1.Where(k => k.CarInsuranceType.Type == tempInsuranceTypeOfCar
                                                           && k.IdCompanyMiddleman == idCompanyMiddleman).First();

                        if (recordForChange != null)
                        {
                            recordForChange.Value = dataParsed.data.Value;
                            db.SaveChanges();
                        }
                        else
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = $"Запис з параметром {tempInsuranceTypeOfCar} не знайдено в базі даних";
                            return js.Serialize(responseToClient);
                        }

                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Запис для коєфіцента - {coef} успішно збережено до бази даних";
                    }
                    break;
                case "K2":
                    {
                        string tempCarZone = dataParsed.data.CarZoneOfRegistration;
                        bool tempIsLegalEntity = dataParsed.data.IsLegalEntity == "Юр" ? true : false;
                        string tempInsuranceType = dataParsed.data.InsuranceTypeOfCar;
                        double tempFranchise = dataParsed.data.Franchise;

                        var recordForChange = db.K2.Where(k => k.InsuranceZoneOfRegistration.Name == tempCarZone
                                                            && k.IsLegalEntity == tempIsLegalEntity
                                                            && k.CarInsuranceType.Type == tempInsuranceType
                                                            && k.ContractFranchise.Franchise.Sum == tempFranchise
                                                            && k.IdCompanyMiddleman == idCompanyMiddleman).First();
                        if (recordForChange != null)
                        {
                            recordForChange.Value = dataParsed.data.Value;
                            db.SaveChanges();
                        }
                        else
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = $"Запис з параметрами {tempCarZone}, {tempIsLegalEntity}, {tempInsuranceType}, {tempFranchise} не знайдено в базі даних";
                            return js.Serialize(responseToClient);
                        }
                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Запис для коєфіцента - {coef} успішно збережено до бази даних";
                    }
                    break;
                case "K3":
                    {
                        string tempCarZone = dataParsed.data.CarZoneOfRegistration;
                        bool tempIsLegalEntity = dataParsed.data.IsLegalEntity == "Юр" ? true : false;
                        string tempInsuranceType = dataParsed.data.InsuranceTypeOfCar;

                        var recordForChange = db.K3.Where(k => k.InsuranceZoneOfRegistration.Name == tempCarZone
                                                            && k.IsLegalEntity == tempIsLegalEntity
                                                            && k.CarInsuranceType.Type == tempInsuranceType
                                                            && k.IdCompanyMiddleman == idCompanyMiddleman).First();

                        if (recordForChange != null)
                        {
                            recordForChange.Value = dataParsed.data.Value;
                            db.SaveChanges();
                        }
                        else
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = $"Запис з параметрами: \n\t{tempCarZone} {tempIsLegalEntity} {tempInsuranceType} не знайдено в базі даних";
                            return js.Serialize(responseToClient);
                        }
                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Запис для коєфіцента - {coef} успішно збережено до бази даних";
                    }
                    break;
                case "K4":
                    {
                        string tempCarZone = dataParsed.data.CarZoneOfRegistration;
                        bool tempIsLegalEntity = dataParsed.data.IsLegalEntity == "Юр" ? true : false;
                        double tempFranchise = dataParsed.data.Franchise;

                        var recordForChange = db.K4.Where(k => k.InsuranceZoneOfRegistration.Name == tempCarZone
                                                            && k.IsLegalEntity == tempIsLegalEntity
                                                            && k.ContractFranchise.Franchise.Sum == tempFranchise
                                                            && k.IdCompanyMiddleman == idCompanyMiddleman).First();
                        if (recordForChange != null)
                        {
                            recordForChange.Value = dataParsed.data.Value;
                            db.SaveChanges();
                        }
                        else
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = $"Запис з параметрами: \n\t{tempCarZone} {tempIsLegalEntity} {tempFranchise} не знайдено в базі даних";
                            return js.Serialize(responseToClient);
                        }
                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Запис для коєфіцента - {coef} успішно збережено до бази даних";
                    }
                    break;
                case "K5":
                    {
                        int tempPeriod = dataParsed.data.Period;

                        var recordForChange = db.K5.Where(k => k.Period == tempPeriod).First();

                        if (recordForChange != null)
                        {
                            recordForChange.Value = dataParsed.data.Value;
                            db.SaveChanges();
                        }
                        else
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = $"Запис з параметром {tempPeriod} не знайдено в базі даних";
                            return js.Serialize(responseToClient);
                        }
                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Запис для коєфіцента - {coef} успішно збережено до бази даних";
                    }
                    break;
                case "K6":
                    {
                        bool tempIsCheater = dataParsed.data.IsCheater == "Шахрай" ? true : false;

                        var recordForChange = db.K6.Where(k => k.IsCheater == tempIsCheater).First();

                        if (recordForChange != null)
                        {
                            recordForChange.Value = dataParsed.data.Value;
                            db.SaveChanges();
                        }
                        else
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = $"Запис з параметром {tempIsCheater} не знайдено в базі даних";
                            return js.Serialize(responseToClient);
                        }
                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Запис для коєфіцента - {coef} успішно збережено до бази даних";
                    }
                    break;
                case "K7":
                    {
                        double tempPeriod = dataParsed.data.Period;

                        var recordForChange = db.K7.Where(k => k.Period == tempPeriod).First();

                        if (recordForChange != null)
                        {
                            recordForChange.Value = dataParsed.data.Value;
                            db.SaveChanges();
                        }
                        else
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = $"Запис з параметром {tempPeriod} не знайдено в базі даних";
                            return js.Serialize(responseToClient);
                        }
                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Запис для коєфіцента - {coef} успішно збережено до бази даних";
                    }
                    break;
                case "BM":
                    {
                        string tempCarZone = dataParsed.data.CarZoneOfRegistration;
                        bool tempIsLegalEntity = dataParsed.data.IsLegalEntity == "Юр" ? true : false;
                        string tempInsuranceType = dataParsed.data.InsuranceTypeOfCar;
                        double tempFranchise = dataParsed.data.Franchise;

                        var recordForChange = db.BonusMalus.Where(k => k.InsuranceZoneOfRegistration.Name == tempCarZone
                                                            && k.IsLegalEntity == tempIsLegalEntity
                                                            && k.CarInsuranceType.Type == tempInsuranceType
                                                            && k.ContractFranchise.Franchise.Sum == tempFranchise
                                                            && k.IdCompanyMiddleman == idCompanyMiddleman).First();

                        if (recordForChange != null)
                        {
                            recordForChange.Value = dataParsed.data.Value;
                            db.SaveChanges();
                        }
                        else
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = $"Запис з параметрами: {tempCarZone}, {tempIsLegalEntity}, {tempInsuranceType}, {tempFranchise} не знайдено в базі даних";
                            return js.Serialize(responseToClient);
                        }
                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Запис для коєфіцента - {coef} успішно збережено до бази даних";
                    }
                    break;
                case "KPark":
                    {
                        bool tempIsLegalEntity = dataParsed.data.IsLegalEntity == "Юр" ? true : false;
                        int tempTransportCountFrom = dataParsed.data.TransportCountFrom;
                        int tempTransportCountTo = dataParsed.data.TransportCountTo;

                        var recordForChange = db.DiscountByQuantities.Where(k => k.IsLegalEntity == tempIsLegalEntity
                                                                            && k.TransportCountFrom == tempTransportCountFrom
                                                                            && k.TransportCountTo == tempTransportCountTo).First();

                        if (recordForChange != null)
                        {
                            recordForChange.Value = dataParsed.data.Value;
                            db.SaveChanges();
                        }
                        else
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = $"Запис з параметрами: {tempIsLegalEntity}, {tempTransportCountFrom} - {tempTransportCountTo} не знайдено в базі даних";
                            return js.Serialize(responseToClient);
                        }
                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Запис для коєфіцента - {coef} успішно збережено до бази даних";
                    }
                    break;
                case "KPilg":
                    {
                        int countNewRecords = 0;

                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Записи для коєфіцента - {coef} у кількості {countNewRecords} успішно збережені до бази даних";
                    }
                    break;
                default:
                    {
                        responseToClient.responseType = ResponseType.Bad;
                        responseToClient.responseText = "Невірно вказана назва коефіцієнту або коефіцієнт відсутній в базі даних";
                    }
                    break;
            }

            return js.Serialize(responseToClient);
        }

        [HttpPost]
        public string SaveAllChangingInCoef()
        {
            int idCompany = 0, idMiddleman = 0, idCompanyMiddleman = 0;

            dynamic dataParsed = GetPotsRequestBody();
            string companyName = Convert.ToString(dataParsed.companyName);
            string middlemanName = Convert.ToString(dataParsed.middlemanName);
            string coef = Convert.ToString(dataParsed.coef);

            ResponseToClient responseToClient = new ResponseToClient();

            JavaScriptSerializer js = new JavaScriptSerializer();

            ResponseToClient resultOfChekingCompanyMiddleman = GetCompanyMiddlemanData(companyName, middlemanName, ref idCompany, ref idMiddleman, ref idCompanyMiddleman);
            if (resultOfChekingCompanyMiddleman.responseType != ResponseType.Good)
                return js.Serialize(resultOfChekingCompanyMiddleman);

            switch (coef)
            {
                case "K1":
                    {
                        int countNewRecords = 0;
                        string tempInsuranceTypeOfCar;
                        foreach(var dt in dataParsed.data)
                        {
                            tempInsuranceTypeOfCar = dt.InsuranceTypeOfCar;
                            var recordForChange = db.K1.Where(k => k.CarInsuranceType.Type == tempInsuranceTypeOfCar
                                                               && k.IdCompanyMiddleman == idCompanyMiddleman).First();
                            if (recordForChange != null)
                            {
                                if(recordForChange.Value != Convert.ToDouble(dt.Value))
                                {
                                    recordForChange.Value = dt.Value;
                                    db.SaveChanges();
                                    countNewRecords++;
                                } 
                            }
                            else
                            {
                                responseToClient.responseType = ResponseType.Bad;
                                responseToClient.responseText = $"Запис з параметром {tempInsuranceTypeOfCar} не знайдено в базі даних";
                                return js.Serialize(responseToClient);
                            }
                        }
                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Записи для коєфіцента - {coef} у кількості {countNewRecords} успішно збережені до бази даних";
                    }
                    break;
                case "K2":
                    {
                        int countNewRecords = 0;

                        string tempCarZone;
                        bool tempIsLegalEntity;
                        string tempInsuranceType;
                        double tempFranchise;

                        foreach (var dt in dataParsed.data)
                        {
                            tempCarZone = dt.CarZoneOfRegistration;
                            tempIsLegalEntity = dt.IsLegalEntity == "Юр" ? true : false;
                            tempInsuranceType = dt.InsuranceTypeOfCar;
                            tempFranchise = dt.Franchise;

                            var recordForChange = db.K2.Where(k => k.InsuranceZoneOfRegistration.Name == tempCarZone
                                                             && k.IsLegalEntity == tempIsLegalEntity
                                                             && k.CarInsuranceType.Type == tempInsuranceType
                                                             && k.ContractFranchise.Franchise.Sum == tempFranchise
                                                             && k.IdCompanyMiddleman == idCompanyMiddleman).First();
                            if (recordForChange != null)
                            {
                                if (recordForChange.Value != Convert.ToDouble(dt.Value))
                                {
                                    recordForChange.Value = dt.Value;
                                    db.SaveChanges();
                                    countNewRecords++;
                                }
                            }
                            else
                            {
                                responseToClient.responseType = ResponseType.Bad;
                                responseToClient.responseText = $"Запис з параметрами {tempCarZone}, {tempIsLegalEntity}, {tempInsuranceType}, {tempFranchise} не знайдено в базі даних";
                                return js.Serialize(responseToClient);
                            } 
                        }
                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Записи для коєфіцента - {coef} у кількості {countNewRecords} успішно збережені до бази даних";
                    }
                    break;
                case "K3":
                    {
                        int countNewRecords = 0;

                        string tempCarZone;
                        bool tempIsLegalEntity;
                        string tempInsuranceType;

                        foreach (var dt in dataParsed.data)
                        {
                            tempCarZone = dt.CarZoneOfRegistration;
                            tempIsLegalEntity = dt.IsLegalEntity == "Юр" ? true : false;
                            tempInsuranceType = dt.InsuranceTypeOfCar;

                            var recordForChange = db.K3.Where(k => k.InsuranceZoneOfRegistration.Name == tempCarZone
                                                             && k.IsLegalEntity == tempIsLegalEntity
                                                             && k.CarInsuranceType.Type == tempInsuranceType
                                                             && k.IdCompanyMiddleman == idCompanyMiddleman).First();
                            if (recordForChange != null)
                            {
                                if (recordForChange.Value != Convert.ToDouble(dt.Value))
                                {
                                    recordForChange.Value = dt.Value;
                                    db.SaveChanges();
                                    countNewRecords++;
                                }
                            }
                            else
                            {
                                responseToClient.responseType = ResponseType.Bad;
                                responseToClient.responseText = $"Запис з параметрами: \n\t{tempCarZone} {tempIsLegalEntity} {tempInsuranceType} не знайдено в базі даних";
                                return js.Serialize(responseToClient);
                            }
                        }
                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Записи для коєфіцента - {coef} у кількості {countNewRecords} успішно збережені до бази даних";
                    }
                    break;
                case "K4":
                    {
                        int countNewRecords = 0;

                        string tempCarZone;
                        bool tempIsLegalEntity;
                        double tempFranchise;

                        foreach (var dt in dataParsed.data)
                        {
                            tempCarZone = dt.CarZoneOfRegistration;
                            tempIsLegalEntity = dt.IsLegalEntity == "Юр" ? true : false;
                            tempFranchise = dt.Franchise;

                            var recordForChange = db.K4.Where(k => k.InsuranceZoneOfRegistration.Name == tempCarZone
                                                             && k.IsLegalEntity == tempIsLegalEntity
                                                             && k.ContractFranchise.Franchise.Sum == tempFranchise
                                                             && k.IdCompanyMiddleman == idCompanyMiddleman).First();
                            if (recordForChange != null)
                            {
                                if (recordForChange.Value != Convert.ToDouble(dt.Value))
                                {
                                    recordForChange.Value = dt.Value;
                                    db.SaveChanges();
                                    countNewRecords++;
                                }
                            }
                            else
                            {
                                responseToClient.responseType = ResponseType.Bad;
                                responseToClient.responseText = $"Запис з параметрами: \n\t{tempCarZone} {tempIsLegalEntity} {tempFranchise} не знайдено в базі даних";
                                return js.Serialize(responseToClient);
                            }
                        }
                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Записи для коєфіцента - {coef} у кількості {countNewRecords} успішно збережені до бази даних";
                    }
                    break;
                case "K5":
                    {
                        int countNewRecords = 0;

                        int tempPeriod;
                        foreach (var dt in dataParsed.data)
                        {
                            tempPeriod = dt.Period;

                            var recordForChange = db.K5.Where(k => k.Period == tempPeriod).First();

                            if (recordForChange != null)
                            {
                                if (recordForChange.Value != Convert.ToDouble(dt.Value))
                                {
                                    recordForChange.Value = dt.Value;
                                    db.SaveChanges();
                                    countNewRecords++;
                                }
                            }
                            else
                            {
                                responseToClient.responseType = ResponseType.Bad;
                                responseToClient.responseText = $"Запис з параметром {tempPeriod} не знайдено в базі даних";
                                return js.Serialize(responseToClient);
                            }
                        }
                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Записи для коєфіцента - {coef} у кількості {countNewRecords} успішно збережені до бази даних";
                    }
                    break;
                case "K6":
                    {
                        int countNewRecords = 0;

                        bool tempIsCheater;

                        foreach (var dt in dataParsed.data)
                        {
                            tempIsCheater = dt.IsCheater == "Шахрай" ? true : false;

                            var recordForChange = db.K6.Where(k => k.IsCheater == tempIsCheater).First();

                            if (recordForChange != null)
                            {
                                if (recordForChange.Value != Convert.ToDouble(dt.Value))
                                {
                                    recordForChange.Value = dt.Value;
                                    db.SaveChanges();
                                    countNewRecords++;
                                }
                            }
                            else
                            {
                                responseToClient.responseType = ResponseType.Bad;
                                responseToClient.responseText = $"Запис з параметром {tempIsCheater} не знайдено в базі даних";
                                return js.Serialize(responseToClient);
                            }
                        }
                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Записи для коєфіцента - {coef} у кількості {countNewRecords} успішно збережені до бази даних";
                    }
                    break;
                case "K7":
                    {
                        int countNewRecords = 0;

                        double tempPeriod;

                        foreach (var dt in dataParsed.data)
                        {
                            tempPeriod = dt.Period;

                            var recordForChange = db.K7.Where(k => k.Period == tempPeriod).First();

                            if (recordForChange != null)
                            {
                                if (recordForChange.Value != Convert.ToDouble(dt.Value))
                                {
                                    recordForChange.Value = dt.Value;
                                    db.SaveChanges();
                                    countNewRecords++;
                                }
                            }
                            else
                            {
                                responseToClient.responseType = ResponseType.Bad;
                                responseToClient.responseText = $"Запис з параметром {tempPeriod} не знайдено в базі даних";
                                return js.Serialize(responseToClient);
                            }
                        }
                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Записи для коєфіцента - {coef} у кількості {countNewRecords} успішно збережені до бази даних";
                    }
                    break;
                case "BM":
                    {
                        int countNewRecords = 0;

                        string tempCarZone;
                        bool tempIsLegalEntity;
                        string tempInsuranceType;
                        double tempFranchise;

                        foreach (var dt in dataParsed.data)
                        {
                            tempCarZone = dt.CarZoneOfRegistration;
                            tempIsLegalEntity = dt.IsLegalEntity == "Юр" ? true : false;
                            tempInsuranceType = dt.InsuranceTypeOfCar;
                            tempFranchise = dt.Franchise;

                            var recordForChange = db.BonusMalus.Where(k => k.InsuranceZoneOfRegistration.Name == tempCarZone
                                                             && k.IsLegalEntity == tempIsLegalEntity
                                                             && k.CarInsuranceType.Type == tempInsuranceType
                                                             && k.ContractFranchise.Franchise.Sum == tempFranchise
                                                             && k.IdCompanyMiddleman == idCompanyMiddleman).First();

                            if (recordForChange != null)
                            {
                                if (recordForChange.Value != Convert.ToDouble(dt.Value))
                                {
                                    recordForChange.Value = dt.Value;
                                    db.SaveChanges();
                                    countNewRecords++;
                                }
                            }
                            else
                            {
                                responseToClient.responseType = ResponseType.Bad;
                                responseToClient.responseText = $"Запис з параметрами: {tempCarZone}, {tempIsLegalEntity}, {tempInsuranceType}, {tempFranchise} не знайдено в базі даних";
                                return js.Serialize(responseToClient);
                            }
                        }
                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Записи для коєфіцента - {coef} у кількості {countNewRecords} успішно збережені до бази даних";
                    }
                    break;
                case "KPark":
                    {
                        int countNewRecords = 0;

                        bool tempIsLegalEntity;
                        int tempTransportCountFrom;
                        int tempTransportCountTo;

                        foreach (var dt in dataParsed.data)
                        {
                            tempIsLegalEntity = dt.IsLegalEntity == "Юр" ? true : false;
                            tempTransportCountFrom = dt.TransportCountFrom;
                            tempTransportCountTo = dt.TransportCountTo;

                            var recordForChange = db.DiscountByQuantities.Where(k => k.IsLegalEntity == tempIsLegalEntity
                                                                                && k.TransportCountFrom == tempTransportCountFrom
                                                                                && k.TransportCountTo == tempTransportCountTo).First();

                            if (recordForChange != null)
                            {
                                if (recordForChange.Value != Convert.ToDouble(dt.Value))
                                {
                                    recordForChange.Value = dt.Value;
                                    db.SaveChanges();
                                    countNewRecords++;
                                }
                            }
                            else
                            {
                                responseToClient.responseType = ResponseType.Bad;
                                responseToClient.responseText = $"Запис з параметрами: {tempIsLegalEntity}, {tempTransportCountFrom} - {tempTransportCountTo} не знайдено в базі даних";
                                return js.Serialize(responseToClient);
                            }
                        }
                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Записи для коєфіцента - {coef} у кількості {countNewRecords} успішно збережені до бази даних";
                    }
                    break;
                case "KPilg":
                    {
                        int countNewRecords = 0;

                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Записи для коєфіцента - {coef} у кількості {countNewRecords} успішно збережені до бази даних";
                    }
                    break;
                default:
                    {
                        responseToClient.responseType = ResponseType.Bad;
                        responseToClient.responseText = "Невірно вказана назва коефіцієнту або коефіцієнт відсутній в базі даних";
                    }
                    break;
            }

            return js.Serialize(responseToClient);
        }

        private dynamic GetPotsRequestBody()
        {
            System.IO.Stream request = Request.InputStream;
            request.Seek(0, SeekOrigin.Begin);
            string bodyData = new StreamReader(request).ReadToEnd();
            return JsonConvert.DeserializeObject(bodyData);
        }

        private string InsertDataForK1(string companyName, string middlemanName, int idCompanyMiddleman)
        {
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

            return "Success!";
        }

        private string InsertDataForK2(string companyName, string middlemanName, int idCompanyMiddleman)
        {
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
            var isLegal = new List<bool> { true, false };
            var insuranceTypeOfCar = db.CarInsuranceTypes.ToList();

            List<InsCarType> ict = new List<InsCarType>();
            List<InsZoneOfReg> izor = new List<InsZoneOfReg>();
            List<bool> isLegalBool = new List<bool>();
            List<Fran> fran = new List<Fran>();
            foreach (var i in insuranceZoneOfReg)
            {
                InsZoneOfReg curZoneOfReg = new InsZoneOfReg();
                curZoneOfReg.id = i.Id;
                curZoneOfReg.name = i.Name;
                izor.Add(curZoneOfReg);
            }
            foreach (var i in isLegal)
            {
                isLegalBool.Add(i);
            }
            foreach (var i in insuranceTypeOfCar)
            {
                InsCarType curIcsCarType = new InsCarType();
                curIcsCarType.id = i.Id;
                curIcsCarType.name = i.Type;
                ict.Add(curIcsCarType);
            }
            foreach (var i in franchise)
            {
                Fran curFran = new Fran();
                curFran.id = i.Id;
                curFran.sum = i.Sum;
                fran.Add(curFran);
            }

            foreach (var i in insuranceZoneOfReg)
            {
                foreach (var j in isLegal)
                {
                    foreach (var q in insuranceTypeOfCar)
                    {
                        foreach (var w in franchise)
                        {
                            var b = db.ContractFranchises
                                        .Where(cf => cf.IdCompanyContractType == idCompanyContractType && cf.IdFranchise == w.Id)
                                        .Select(i_d => i_d.Id)
                                        .FirstOrDefault();

                            if (db.K2.Where(k => k.IdInsuranceZoneOfReg == i.Id
                                                        && k.IsLegalEntity == j
                                                        && k.IdCarInsuranceType == q.Id
                                                        && k.IdContractFranchise == b
                                                        && k.IdCompanyMiddleman == idCompanyMiddleman).Count() == 0)
                            {
                                var newRowK2 = new K2()
                                {
                                    IdInsuranceZoneOfReg = i.Id,
                                    IsLegalEntity = j,
                                    IdCarInsuranceType = q.Id,
                                    IdContractFranchise = db.ContractFranchises.Where(idf => idf.IdFranchise == w.Id && idf.IdCompanyContractType == idCompanyContractType).Select(x => x.Id).FirstOrDefault(),
                                    Value = 0,
                                    IdCompanyMiddleman = idCompanyMiddleman,
                                };

                                db.K2.Add(newRowK2);
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
            return "Success!";
        }

        private string InsertDataForK3(string companyName, string middlemanName, int idCompanyMiddleman)
        {
            var K3 = db.K3.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman);
            List<TableK2ToSend> K2Table = new List<TableK2ToSend>();

            var insuranceZoneOfReg = db.InsuranceZoneOfRegistrations.ToList();
            var isLegal = new List<bool> { true, false };
            var insuranceTypeOfCar = db.CarInsuranceTypes.ToList();

            List<InsCarType> ict = new List<InsCarType>();
            List<InsZoneOfReg> izor = new List<InsZoneOfReg>();
            List<bool> isLegalBool = new List<bool>();
            foreach (var i in insuranceZoneOfReg)
            {
                InsZoneOfReg curZoneOfReg = new InsZoneOfReg();
                curZoneOfReg.id = i.Id;
                curZoneOfReg.name = i.Name;
                izor.Add(curZoneOfReg);
            }
            foreach (var i in isLegal)
            {
                isLegalBool.Add(i);
            }
            foreach (var i in insuranceTypeOfCar)
            {
                InsCarType curIcsCarType = new InsCarType();
                curIcsCarType.id = i.Id;
                curIcsCarType.name = i.Type;
                ict.Add(curIcsCarType);
            }

            foreach (var i in insuranceZoneOfReg)
            {
                foreach (var j in isLegal)
                {
                    foreach (var q in insuranceTypeOfCar)
                    {

                        if (db.K3.Where(k => k.IdInsuranceZoneOfReg == i.Id
                                                    && k.IsLegalEntity == j
                                                    && k.IdCarInsuranceType == q.Id
                                                    && k.IdCompanyMiddleman == idCompanyMiddleman).Count() == 0)
                        {
                            var newRowK3 = new K3()
                            {
                                IdInsuranceZoneOfReg = i.Id,
                                IsLegalEntity = j,
                                IdCarInsuranceType = q.Id,
                                Value = 0,
                                IdCompanyMiddleman = idCompanyMiddleman,
                            };

                            db.K3.Add(newRowK3);
                            db.SaveChanges();
                        }
                    }
                }
            }
            return "Success!";
        }

        private string InsertDataForK4(string companyName, string middlemanName, int idCompanyMiddleman)
        {
            var K4 = db.K4.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman);
            List<TableK2ToSend> K4Table = new List<TableK2ToSend>();

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
            var isLegal = new List<bool> { true, false };

            List<InsZoneOfReg> izor = new List<InsZoneOfReg>();
            List<bool> isLegalBool = new List<bool>();
            List<Fran> fran = new List<Fran>();
            foreach (var i in insuranceZoneOfReg)
            {
                InsZoneOfReg curZoneOfReg = new InsZoneOfReg();
                curZoneOfReg.id = i.Id;
                curZoneOfReg.name = i.Name;
                izor.Add(curZoneOfReg);
            }
            foreach (var i in isLegal)
            {
                isLegalBool.Add(i);
            }
            foreach (var i in franchise)
            {
                Fran curFran = new Fran();
                curFran.id = i.Id;
                curFran.sum = i.Sum;
                fran.Add(curFran);
            }

            foreach (var i in insuranceZoneOfReg)
            {
                foreach (var j in isLegal)
                {
                    foreach (var w in franchise)
                    {
                        var b = db.ContractFranchises
                                    .Where(cf => cf.IdCompanyContractType == idCompanyContractType && cf.IdFranchise == w.Id)
                                    .Select(i_d => i_d.Id)
                                    .FirstOrDefault();

                        if (db.K4.Where(k => k.IdInsuranceZoneOfReg == i.Id
                                                    && k.IsLegalEntity == j
                                                    && k.IdContractFranchise == b
                                                    && k.IdCompanyMiddleman == idCompanyMiddleman).Count() == 0)
                        {
                            var newRowK4 = new K4()
                            {
                                IdInsuranceZoneOfReg = i.Id,
                                IsLegalEntity = j,
                                IdContractFranchise = db.ContractFranchises.Where(idf => idf.IdFranchise == w.Id && idf.IdCompanyContractType == idCompanyContractType).Select(x => x.Id).FirstOrDefault(),
                                Value = 0,
                                IdCompanyMiddleman = idCompanyMiddleman,
                            };

                            db.K4.Add(newRowK4);
                            db.SaveChanges();
                        }
                    }
                }
            }
            return "Success!";
        }

        private string InsertDataForBM(string companyName, string middlemanName, int idCompanyMiddleman)
        {
            var BM = db.BonusMalus.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman);
            List<TableBMToSend> BMTable = new List<TableBMToSend>();

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
            var isLegal = new List<bool> { true, false };
            var insuranceTypeOfCar = db.CarInsuranceTypes.ToList();

            List<InsCarType> ict = new List<InsCarType>();
            List<InsZoneOfReg> izor = new List<InsZoneOfReg>();
            List<bool> isLegalBool = new List<bool>();
            List<Fran> fran = new List<Fran>();
            foreach (var i in insuranceZoneOfReg)
            {
                InsZoneOfReg curZoneOfReg = new InsZoneOfReg();
                curZoneOfReg.id = i.Id;
                curZoneOfReg.name = i.Name;
                izor.Add(curZoneOfReg);
            }
            foreach (var i in isLegal)
            {
                isLegalBool.Add(i);
            }
            foreach (var i in insuranceTypeOfCar)
            {
                InsCarType curIcsCarType = new InsCarType();
                curIcsCarType.id = i.Id;
                curIcsCarType.name = i.Type;
                ict.Add(curIcsCarType);
            }
            foreach (var i in franchise)
            {
                Fran curFran = new Fran();
                curFran.id = i.Id;
                curFran.sum = i.Sum;
                fran.Add(curFran);
            }

            foreach (var i in insuranceZoneOfReg)
            {
                foreach (var j in isLegal)
                {
                    foreach (var q in insuranceTypeOfCar)
                    {
                        foreach (var w in franchise)
                        {
                            var b = db.ContractFranchises
                                        .Where(cf => cf.IdCompanyContractType == idCompanyContractType && cf.IdFranchise == w.Id)
                                        .Select(i_d => i_d.Id)
                                        .FirstOrDefault();

                            if (db.BonusMalus.Where(k => k.IdInsuranceZoneOfReg == i.Id
                                                        && k.IsLegalEntity == j
                                                        && k.IdCarInsuranceType == q.Id
                                                        && k.IdContractFranchise == b
                                                        && k.IdCompanyMiddleman == idCompanyMiddleman).Count() == 0)
                            {
                                var newRowBM = new BonusMalus()
                                {
                                    IdInsuranceZoneOfReg = i.Id,
                                    IsLegalEntity = j,
                                    IdCarInsuranceType = q.Id,
                                    IdContractFranchise = db.ContractFranchises.Where(idf => idf.IdFranchise == w.Id && idf.IdCompanyContractType == idCompanyContractType).Select(x => x.Id).FirstOrDefault(),
                                    Value = 0,
                                    IdCompanyMiddleman = idCompanyMiddleman,
                                };

                                db.BonusMalus.Add(newRowBM);
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
            return "Success!";
        }

        private int GetMiddlemanId(string middlemanName)
        {
            return db.Middlemen.Where(mi => mi.FullName == middlemanName).Select(m => m.Id).FirstOrDefault();
        }

        private int GetCompanyId(string companyName)
        {
            return db.Companies.Where(ci => ci.Name == companyName).Select(c => c.Id).FirstOrDefault();
        }

        private int GetCompanyMiddlemanId(int idCompany, int idMiddleman)
        {
            return db.CompanyMiddlemen.Where(ci => ci.IdCompany == idCompany && ci.IdMiddleman == idMiddleman).Select(cm => cm.Id).FirstOrDefault();
        }

        private ResponseToClient GetCompanyMiddlemanData(string companyName, string middlemanName, ref int idCompany, ref int idMiddleman, ref int idCompanyMiddleman)
        {
            ResponseToClient responseToClient = new ResponseToClient();
            if (companyName == "")
            {
                responseToClient.responseType = ResponseType.Bad;
                responseToClient.responseText = "Не вказана назва компанії";
                return responseToClient;
                //return "Error! Company is empty";
            }

            idCompany = GetCompanyId(companyName);

            if (idCompany == 0)
            {
                responseToClient.responseType = ResponseType.Bad;
                responseToClient.responseText = "Не вдалося знайти компанію в базі даних";
                return responseToClient;
            }

            if (middlemanName == "")
            {
                responseToClient.responseType = ResponseType.Bad;
                responseToClient.responseText = "Не вказано ім'я посередника";
                return responseToClient;
            }

            idMiddleman = GetMiddlemanId(middlemanName);

            if (idMiddleman == 0)
            {
                responseToClient.responseType = ResponseType.Bad;
                responseToClient.responseText = "Не вдалося знайти посередника в базі даних";
                return responseToClient;
            }

            idCompanyMiddleman = GetCompanyMiddlemanId(idCompany, idMiddleman);

            if (idCompanyMiddleman == 0)
            {
                responseToClient.responseType = ResponseType.Bad;
                responseToClient.responseText = $"Для компанії \"{companyName}\" відсутній посередник \"{middlemanName}\"";
                return responseToClient;
            }

            responseToClient.responseType = ResponseType.Good;
            return responseToClient;
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

    public class TableK3ToSend
    {
        public string CarZoneOfRegistration { get; set; }
        public string IsLegalEntity { get; set; }
        public string InsuranceTypeOfCar { get; set; }
        public double Value { get; set; }
    }

    public class TableK4ToSend
    {
        public string CarZoneOfRegistration { get; set; }
        public string IsLegalEntity { get; set; }
        public double Franchise { get; set; }
        public double Value { get; set; }
    }

    public class TableK5ToSend
    {
        public int Period { get; set; }
        public double Value { get; set; }
    }

    public class TableK6ToSend
    {
        public string IsCheater { get; set; }
        public double? Value { get; set; }
    }

    public class TableK7ToSend
    {
        public double Period { get; set; }
        public double Value { get; set; }
    }

    public class TableBMToSend
    {
        public string CarZoneOfRegistration { get; set; }
        public string IsLegalEntity { get; set; }
        public string InsuranceTypeOfCar { get; set; }
        public double Franchise { get; set; }
        public double Value { get; set; }
    }

    public class TableKParkToSend
    {
        public string IsLegalEntity { get; set; }
        public int TransportCountFrom { get; set; }
        public int TransportCountTo { get; set; }
        public double Value { get; set; }
    }
    
}