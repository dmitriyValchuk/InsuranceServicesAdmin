using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InsuranceServicesAdminLight.Models;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.IO;
using InsuranceServicesAdminLight.Business;
using InsuranceServicesAdminLight.Business.DataManipulation;
using InsuranceServicesAdminLight.Business.Conditions;
using InsuranceServicesAdminLight.Business.DataToSend;

namespace InsuranceServicesAdminLight.Controllers
{
    public class ContractsController : Controller
    {
        InsuranceServicesContext db = new InsuranceServicesContext();

        [HttpPost]
        public string GetAllCompanies()
        {
            List<string> companiesNames = CompanyDataManipulation.GetCompaniesName();
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(companiesNames);
        }

        public string GetCheckedCompany(string companyChecked)
        {
            ResponseToClient responseToClient = new ResponseToClient();

            Company company = CompanyDataManipulation.GetSingle(companyChecked);

            JavaScriptSerializer js = new JavaScriptSerializer();

            if (company == null)
            {
                responseToClient.responseType = ResponseType.Bad;
                responseToClient.responseText = "Не вдалося знайти компанію в базі даних";
                return js.Serialize(responseToClient);
            }

            List<string> middlemen = CompanyDataManipulation.GetCompanyMiddlemenName(company);
            if (middlemen == null)
            {
                responseToClient.responseType = ResponseType.Bad;
                responseToClient.responseText =  $"Для компанії {company.Name} відсутні посередники";
                return js.Serialize(responseToClient);
            }

            //foreach (var cm in companyMiddleMan)
            //{
            //    var currentMiddleman = db.Middlemen.Where(m => m.Id == cm.IdMiddleman).FirstOrDefault();
            //    middlemen.Add(currentMiddleman.FullName);
            //}
                
            return js.Serialize(middlemen);

        }

        public string GetConditionsForCoefK1(string companyName, string middlemanName)
        {
            return GetConditions.K1(companyName, middlemanName);
        }

        public string GetConditionsForCoefK2(string companyName, string middlemanName)
        {
            return GetConditions.K2(companyName, middlemanName);
        }

        public string GetConditionsForCoefK3(string companyName, string middlemanName)
        {
            return GetConditions.K3(companyName, middlemanName);
        }

        public string GetConditionsForCoefK4(string companyName, string middlemanName)
        {
            return GetConditions.K4(companyName, middlemanName);
        }

        public string GetConditionsForCoefK5(string companyName, string middlemanName)
        {
            return GetConditions.K5();
        }

        public string GetConditionsForCoefK6(string companyName, string middlemanName)
        {
            return GetConditions.K6();
        }

        public string GetConditionsForCoefK7(string companyName, string middlemanName)
        {
            return GetConditions.K7();
        }

        public string GetConditionsForCoefBM(string companyName, string middlemanName)
        {
            return GetConditions.BM(companyName, middlemanName);
        }

        public string GetConditionsForCoefKPark(string companyName, string middlemanName)
        {
            return GetConditions.KPark();
        }


        public string RemoveDataFromTable(string coef, string companyName, string middlemanName, string data, bool coefIsDependent)
        {
            int idCompany = 0, idMiddleman = 0, idCompanyMiddleman = 0;

            dynamic dataParsed = JsonConvert.DeserializeObject(data);

            bool currentCoefIsDependent = coefIsDependent;

            ResponseToClient responseToClient = new ResponseToClient();
            JavaScriptSerializer js = new JavaScriptSerializer();

            if (coefIsDependent)
            {
                ResponseToClient resultOfChekingCompanyMiddleman = GetCompanyMiddlemanData(companyName, middlemanName, ref idCompany, ref idMiddleman, ref idCompanyMiddleman);
                if (resultOfChekingCompanyMiddleman.responseType != ResponseType.Good)
                    return js.Serialize(resultOfChekingCompanyMiddleman);
            }

            switch (coef)
            {
                case "K1":
                    {
                        string currentInsuranceType = dataParsed.InsuranceTypeOfCar;
                        var recordForDel = db.K1.Where(k => k.CarInsuranceType.Type == currentInsuranceType
                                                         && k.IdCompanyMiddleman == idCompanyMiddleman);
                        if (!recordForDel.Any())
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Запис не знайдений в базі даних";
                        }
                        else
                        {
                            db.K1.Remove(recordForDel.First());
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
                                                         && k.IdCompanyMiddleman == idCompanyMiddleman);                        
                        if (!recordForDel.Any())
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Запис не знайдений в базі даних";
                        }
                        else
                        {
                            db.K2.Remove(recordForDel.First());
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
                                                         && k.IdCompanyMiddleman == idCompanyMiddleman);
                        
                        if (!recordForDel.Any())
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Запис не знайдений в базі даних";
                        }
                        else
                        {
                            db.K3.Remove(recordForDel.First());
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
                                                         && k.IdCompanyMiddleman == idCompanyMiddleman);
                        
                        if (!recordForDel.Any())
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Запис не знайдений в базі даних";
                        }
                        else
                        {
                            db.K4.Remove(recordForDel.First());
                            db.SaveChanges();
                            responseToClient.responseType = ResponseType.Good;
                            responseToClient.responseText = "Запис успішно видалений з бази даних";
                        }
                    }
                    break;
                case "K5":
                    {
                        int currentPeriod = dataParsed.Period;

                        var recordForDel = db.K5.Where(k => k.Period == currentPeriod);
                        
                        if (!recordForDel.Any())
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Запис не знайдений в базі даних";
                        }
                        else
                        {
                            db.K5.Remove(recordForDel.First());
                            db.SaveChanges();
                            responseToClient.responseType = ResponseType.Good;
                            responseToClient.responseText = "Запис успішно видалений з бази даних";
                        }
                    }
                    break;
                case "K6":
                    {
                        bool currentIsCheater = dataParsed.IsCheater == "Шахрай" ? true : false;

                        var recordForDel = db.K6.Where(k => k.IsCheater == currentIsCheater);
                        
                        if (!recordForDel.Any())
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Запис не знайдений в базі даних";
                        }
                        else
                        {
                            db.K6.Remove(recordForDel.First());
                            db.SaveChanges();
                            responseToClient.responseType = ResponseType.Good;
                            responseToClient.responseText = "Запис успішно видалений з бази даних";
                        }
                    }
                    break;
                case "K7":
                    {
                        double currentPeriod = dataParsed.Period;

                        var recordForDel = db.K7.Where(k => k.Period == currentPeriod);
                        
                        if (!recordForDel.Any())
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Запис не знайдений в базі даних";
                        }
                        else
                        {
                            db.K7.Remove(recordForDel.First());
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
                                                         && k.IdCompanyMiddleman == idCompanyMiddleman);
                        
                        if (!recordForDel.Any())
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Запис не знайдений в базі даних";
                        }
                        else
                        {
                            db.BonusMalus.Remove(recordForDel.First());
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
                                                                            && k.TransportCountTo == currentTransportCountTo);
                        
                        if (!recordForDel.Any())
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Запис не знайдений в базі даних";
                        }
                        else
                        {
                            db.DiscountByQuantities.Remove(recordForDel.First());
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
            bool coefIsDependent = Convert.ToBoolean(dataParsed.coefIsDependent);

            ResponseToClient responseToClient = new ResponseToClient();
            JavaScriptSerializer js = new JavaScriptSerializer();

            if (coefIsDependent)
            {
                ResponseToClient resultOfChekingCompanyMiddleman = GetCompanyMiddlemanData(companyName, middlemanName, ref idCompany, ref idMiddleman, ref idCompanyMiddleman);
                if (resultOfChekingCompanyMiddleman.responseType != ResponseType.Good)
                    return js.Serialize(resultOfChekingCompanyMiddleman);
            }

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

                        K5 recordForChange = new K5();
                        try
                        {
                            recordForChange = db.K5.Where(k => k.Period == tempPeriod).First();
                        }
                        catch
                        {
                            recordForChange.Period = tempPeriod;
                            recordForChange.Value = dataParsed.data.Value;
                            db.K5.Add(recordForChange);
                            db.SaveChanges();
                            responseToClient.responseType = ResponseType.Good;
                            responseToClient.responseText = $"Запис для коєфіцента - {coef} успішно збережено до бази даних";
                        }

                        try
                        {
                            recordForChange.Value = dataParsed.data.Value;
                            db.SaveChanges();
                        }
                        catch
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = $"Не вдалося зберегти період \"{tempPeriod}\" в базу даних";
                            return js.Serialize(responseToClient);
                        }

                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Запис для коєфіцента - {coef} успішно збережено до бази даних";
                        return js.Serialize(responseToClient);
                    }
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
                        return js.Serialize(responseToClient);
                    }
                case "K7":
                    {
                        double tempPeriod = dataParsed.data.Period;

                        K7 recordForChange = new K7();
                        try
                        {
                            recordForChange = db.K7.Where(k => k.Period == tempPeriod).First();
                        }
                        catch
                        {
                            recordForChange.Period = tempPeriod;
                            recordForChange.Value = dataParsed.data.Value;
                            db.K7.Add(recordForChange);
                            db.SaveChanges();
                            responseToClient.responseType = ResponseType.Good;
                            responseToClient.responseText = $"Запис для коєфіцента - {coef} успішно збережено до бази даних";
                        }

                        try
                        {
                            recordForChange.Value = dataParsed.data.Value;
                            db.SaveChanges();
                        }
                        catch
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = $"Не вдалося зберегти період \"{tempPeriod}\" в базу даних";
                            return js.Serialize(responseToClient);
                        }

                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Запис для коєфіцента - {coef} успішно збережено до бази даних";
                        return js.Serialize(responseToClient);
                    }
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
                            responseToClient.responseText = $"Запис з параметрами: {tempCarZone}, {tempIsLegalEntity}, {tempInsuranceType}, {tempFranchise} не вдалося зберегти в базі даних";
                            return js.Serialize(responseToClient);
                        }
                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Запис для коєфіцента - {coef} успішно збережено до бази даних";
                        return js.Serialize(responseToClient);
                    }
                case "KPark":
                    {
                        bool tempIsLegalEntity = dataParsed.data.IsLegalEntity == "Юр" ? true : false;
                        int tempTransportCountFrom = dataParsed.data.TransportCountFrom;
                        int tempTransportCountTo = dataParsed.data.TransportCountTo;

                        DiscountByQuantity recordForChange = new DiscountByQuantity();
                        try
                        {
                            recordForChange = db.DiscountByQuantities.Where(k => k.IsLegalEntity == tempIsLegalEntity
                                                                            && k.TransportCountFrom == tempTransportCountFrom
                                                                            && k.TransportCountTo == tempTransportCountTo).First();
                        }
                        catch
                        {
                            recordForChange.IsLegalEntity = tempIsLegalEntity;
                            recordForChange.TransportCountFrom = tempTransportCountFrom;
                            recordForChange.TransportCountTo = tempTransportCountTo;
                            recordForChange.Value = dataParsed.data.Value;
                            db.DiscountByQuantities.Add(recordForChange);
                            db.SaveChanges();
                            responseToClient.responseType = ResponseType.Good;
                            responseToClient.responseText = $"Запис для коєфіцента - {coef} успішно збережено до бази даних";
                        }

                        try
                        {
                            recordForChange.Value = dataParsed.data.Value;
                            db.SaveChanges();
                        }
                        catch
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = $"Запис з параметрами: {tempIsLegalEntity}, {tempTransportCountFrom} - {tempTransportCountTo} не вдалося зберегти в базі даних";
                            return js.Serialize(responseToClient);
                        }

                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Запис для коєфіцента - {coef} успішно збережено до бази даних";
                        return js.Serialize(responseToClient);
                    }
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
            bool coefIsDependent = Convert.ToBoolean(dataParsed.coefIsDependent);

            ResponseToClient responseToClient = new ResponseToClient();
            JavaScriptSerializer js = new JavaScriptSerializer();

            if (coefIsDependent)
            {
                ResponseToClient resultOfChekingCompanyMiddleman = GetCompanyMiddlemanData(companyName, middlemanName, ref idCompany, ref idMiddleman, ref idCompanyMiddleman);
                if (resultOfChekingCompanyMiddleman.responseType != ResponseType.Good)
                    return js.Serialize(resultOfChekingCompanyMiddleman);
            }

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
                        return js.Serialize(responseToClient);
                    }
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
                        return js.Serialize(responseToClient);
                    }
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
                        return js.Serialize(responseToClient);
                    }
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
                        return js.Serialize(responseToClient);
                    }
                case "K5":
                    {
                        int countNewRecords = 0;

                        int tempPeriod;
                        foreach (var dt in dataParsed.data)
                        {
                            tempPeriod = dt.Period;

                            K5 recordForChange = new K5();
                            try
                            {
                                recordForChange = db.K5.Where(k => k.Period == tempPeriod).First();
                            }
                            catch
                            {
                                recordForChange.Period = tempPeriod;
                                recordForChange.Value = dataParsed.data.Value;
                                db.K5.Add(recordForChange);
                                db.SaveChanges();
                                responseToClient.responseType = ResponseType.Good;
                                responseToClient.responseText = $"Запис для коєфіцента - {coef} успішно збережено до бази даних";
                            }

                            try
                            {
                                if (recordForChange.Value != Convert.ToDouble(dt.Value))
                                {
                                    recordForChange.Value = dt.Value;
                                    db.SaveChanges();
                                    countNewRecords++;
                                }
                            }
                            catch
                            {
                                responseToClient.responseType = ResponseType.Bad;
                                responseToClient.responseText = $"Не вдалося зберегти період \"{tempPeriod}\" в базу даних";
                                return js.Serialize(responseToClient);
                            }
                        }
                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Записи для коєфіцента - {coef} у кількості {countNewRecords} успішно збережені до бази даних";
                        return js.Serialize(responseToClient);
                    }
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
                        return js.Serialize(responseToClient);
                    }
                case "K7":
                    {
                        int countNewRecords = 0;

                        double tempPeriod;

                        foreach (var dt in dataParsed.data)
                        {
                            tempPeriod = dt.Period;

                            K7 recordForChange = new K7();
                            try
                            {
                                recordForChange = db.K7.Where(k => k.Period == tempPeriod).First();
                            }
                            catch
                            {
                                recordForChange.Period = tempPeriod;
                                recordForChange.Value = dataParsed.data.Value;
                                db.K7.Add(recordForChange);
                                db.SaveChanges();
                                responseToClient.responseType = ResponseType.Good;
                                responseToClient.responseText = $"Запис для коєфіцента - {coef} успішно збережено до бази даних";
                            }

                            try
                            {
                                recordForChange.Value = dt.Value;
                                db.SaveChanges();
                            }
                            catch
                            {
                                responseToClient.responseType = ResponseType.Bad;
                                responseToClient.responseText = $"Не вдалося зберегти період \"{tempPeriod}\" в базу даних";
                                return js.Serialize(responseToClient);
                            }

                            try
                            {
                                if (recordForChange.Value != Convert.ToDouble(dt.Value))
                                {
                                    recordForChange.Value = dt.Value;
                                    db.SaveChanges();
                                    countNewRecords++;
                                }
                            }
                            catch
                            {
                                responseToClient.responseType = ResponseType.Bad;
                                responseToClient.responseText = $"Не вдалося зберегти період \"{tempPeriod}\" в базу даних";
                                return js.Serialize(responseToClient);
                            }
                        }
                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Записи для коєфіцента - {coef} у кількості {countNewRecords} успішно збережені до бази даних";
                        return js.Serialize(responseToClient);
                    }
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
                        return js.Serialize(responseToClient);
                    }
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

                            DiscountByQuantity recordForChange = new DiscountByQuantity();
                            try
                            {
                                recordForChange = db.DiscountByQuantities.Where(k => k.IsLegalEntity == tempIsLegalEntity
                                                                                && k.TransportCountFrom == tempTransportCountFrom
                                                                                && k.TransportCountTo == tempTransportCountTo).First();
                            }
                            catch
                            {
                                recordForChange.IsLegalEntity = tempIsLegalEntity;
                                recordForChange.TransportCountFrom = tempTransportCountFrom;
                                recordForChange.TransportCountTo = tempTransportCountTo;
                                recordForChange.Value = dataParsed.data.Value;
                                db.DiscountByQuantities.Add(recordForChange);
                                db.SaveChanges();
                                responseToClient.responseType = ResponseType.Good;
                                responseToClient.responseText = $"Запис для коєфіцента - {coef} успішно збережено до бази даних";
                            }

                            try
                            {
                                if (recordForChange.Value != Convert.ToDouble(dt.Value))
                                {
                                    recordForChange.Value = dt.Value;
                                    db.SaveChanges();
                                    countNewRecords++;
                                }
                            }
                            catch
                            {
                                responseToClient.responseType = ResponseType.Bad;
                                responseToClient.responseText = $"Запис з параметрами: {tempIsLegalEntity}, {tempTransportCountFrom} - {tempTransportCountTo} не вдалося зберегти в базі даних";
                                return js.Serialize(responseToClient);
                            }
                        }
                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Записи для коєфіцента - {coef} у кількості {countNewRecords} успішно збережені до бази даних";

                        return js.Serialize(responseToClient);
                    }
                case "KPilg":
                    {
                        int countNewRecords = 0;

                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Записи для коєфіцента - {coef} у кількості {countNewRecords} успішно збережені до бази даних";
                        return "";
                    }
                default:
                    {
                        responseToClient.responseType = ResponseType.Bad;
                        responseToClient.responseText = "Невірно вказана назва коефіцієнту або коефіцієнт відсутній в базі даних";
                        return js.Serialize(responseToClient);
                    }
            }

            return js.Serialize(responseToClient);
        }

        //private string InsertDataForK1(string companyName, string middlemanName, int idCompanyMiddleman)
        //{
        //    var carInsuranceType = db.CarInsuranceTypes.ToList();
        //    if (carInsuranceType.Count == 0)
        //        return "Error! List car type is empty";

        //    foreach (var cit in carInsuranceType)
        //    {
        //        if (db.K1.Where(i => i.IdCarInsuranceType == cit.Id && i.IdCompanyMiddleman == idCompanyMiddleman).Count() == 0)
        //        {
        //            var newRowK1 = new K1()
        //            {
        //                IdCarInsuranceType = cit.Id,
        //                Value = 0,
        //                IdCompanyMiddleman = idCompanyMiddleman,
        //            };

        //            db.K1.Add(newRowK1);
        //            db.SaveChanges();
        //        }
        //    }

        //    return "Success!";
        //}

        //private string InsertDataForK2(string companyName, string middlemanName, int idCompanyMiddleman)
        //{
        //    var K2 = db.K2.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman);
        //    List<TableK2ToSend> K2Table = new List<TableK2ToSend>();

        //    int idContractType = db.ContractTypes.Where(n => n.Name == "ГО").Select(i => i.Id).FirstOrDefault();
        //    if (idContractType == 0)
        //        return "Error! Contract Type not found";

        //    int idCompanyContractType = db.CompanyContractTypes
        //        .Where(m => m.IdCompanyMiddleman == idCompanyMiddleman && m.IdContractType == idContractType)
        //        .Select(i => i.Id)
        //        .FirstOrDefault();
        //    if (idCompanyContractType == 0)
        //        return "Error! In this middlemant not exit this type of contract";

        //    var idsFranchise = db.ContractFranchises.Where(cct => cct.IdCompanyContractType == idCompanyContractType).Select(i => i.IdFranchise).ToList();
        //    if (idsFranchise.Count == 0)
        //        return "Error! This contract hasn`t franchise";

        //    var franchise = db.Franchises.Where(i => idsFranchise.Contains(i.Id)).ToList();
        //    if (franchise.Count == 0)
        //        return "Error! In Data Base not exist franchise for this conditions!";

        //    var insuranceZoneOfReg = db.InsuranceZoneOfRegistrations.ToList();
        //    var isLegal = new List<bool> { true, false };
        //    var insuranceTypeOfCar = db.CarInsuranceTypes.ToList();

        //    //
        //    List<InsCarType> ict = new List<InsCarType>();
        //    List<InsZoneOfReg> izor = new List<InsZoneOfReg>();
        //    List<bool> isLegalBool = new List<bool>();
        //    List<Fran> fran = new List<Fran>();
        //    foreach (var i in insuranceZoneOfReg)
        //    {
        //        InsZoneOfReg curZoneOfReg = new InsZoneOfReg();
        //        curZoneOfReg.id = i.Id;
        //        curZoneOfReg.name = i.Name;
        //        izor.Add(curZoneOfReg);
        //    }
        //    foreach (var i in isLegal)
        //    {
        //        isLegalBool.Add(i);
        //    }
        //    foreach (var i in insuranceTypeOfCar)
        //    {
        //        InsCarType curIcsCarType = new InsCarType();
        //        curIcsCarType.id = i.Id;
        //        curIcsCarType.name = i.Type;
        //        ict.Add(curIcsCarType);
        //    }
        //    foreach (var i in franchise)
        //    {
        //        Fran curFran = new Fran();
        //        curFran.id = i.Id;
        //        curFran.sum = i.Sum;
        //        fran.Add(curFran);
        //    }
        //    //

        //    foreach (var i in insuranceZoneOfReg)
        //    {
        //        foreach (var j in isLegal)
        //        {
        //            foreach (var q in insuranceTypeOfCar)
        //            {
        //                foreach (var w in franchise)
        //                {
        //                    var b = db.ContractFranchises
        //                                .Where(cf => cf.IdCompanyContractType == idCompanyContractType && cf.IdFranchise == w.Id)
        //                                .Select(i_d => i_d.Id)
        //                                .FirstOrDefault();

        //                    if (db.K2.Where(k => k.IdInsuranceZoneOfReg == i.Id
        //                                                && k.IsLegalEntity == j
        //                                                && k.IdCarInsuranceType == q.Id
        //                                                && k.IdContractFranchise == b
        //                                                && k.IdCompanyMiddleman == idCompanyMiddleman).Count() == 0)
        //                    {
        //                        var newRowK2 = new K2()
        //                        {
        //                            IdInsuranceZoneOfReg = i.Id,
        //                            IsLegalEntity = j,
        //                            IdCarInsuranceType = q.Id,
        //                            IdContractFranchise = db.ContractFranchises.Where(idf => idf.IdFranchise == w.Id && idf.IdCompanyContractType == idCompanyContractType).Select(x => x.Id).FirstOrDefault(),
        //                            Value = 0,
        //                            IdCompanyMiddleman = idCompanyMiddleman,
        //                        };

        //                        db.K2.Add(newRowK2);
        //                        db.SaveChanges();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return "Success!";
        //}

        //private string InsertDataForK3(string companyName, string middlemanName, int idCompanyMiddleman)
        //{
        //    var K3 = db.K3.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman);
        //    List<TableK3ToSend> K3Table = new List<TableK3ToSend>();

        //    var insuranceZoneOfReg = db.InsuranceZoneOfRegistrations.ToList();
        //    var isLegal = new List<bool> { true, false };
        //    var insuranceTypeOfCar = db.CarInsuranceTypes.ToList();

        //    List<InsCarType> ict = new List<InsCarType>();
        //    List<InsZoneOfReg> izor = new List<InsZoneOfReg>();
        //    List<bool> isLegalBool = new List<bool>();
        //    foreach (var i in insuranceZoneOfReg)
        //    {
        //        InsZoneOfReg curZoneOfReg = new InsZoneOfReg();
        //        curZoneOfReg.id = i.Id;
        //        curZoneOfReg.name = i.Name;
        //        izor.Add(curZoneOfReg);
        //    }
        //    foreach (var i in isLegal)
        //    {
        //        isLegalBool.Add(i);
        //    }
        //    foreach (var i in insuranceTypeOfCar)
        //    {
        //        InsCarType curIcsCarType = new InsCarType();
        //        curIcsCarType.id = i.Id;
        //        curIcsCarType.name = i.Type;
        //        ict.Add(curIcsCarType);
        //    }

        //    foreach (var i in insuranceZoneOfReg)
        //    {
        //        foreach (var j in isLegal)
        //        {
        //            foreach (var q in insuranceTypeOfCar)
        //            {

        //                if (db.K3.Where(k => k.IdInsuranceZoneOfReg == i.Id
        //                                            && k.IsLegalEntity == j
        //                                            && k.IdCarInsuranceType == q.Id
        //                                            && k.IdCompanyMiddleman == idCompanyMiddleman).Count() == 0)
        //                {
        //                    var newRowK3 = new K3()
        //                    {
        //                        IdInsuranceZoneOfReg = i.Id,
        //                        IsLegalEntity = j,
        //                        IdCarInsuranceType = q.Id,
        //                        Value = 0,
        //                        IdCompanyMiddleman = idCompanyMiddleman,
        //                    };

        //                    db.K3.Add(newRowK3);
        //                    db.SaveChanges();
        //                }
        //            }
        //        }
        //    }
        //    return "Success!";
        //}

        //private string InsertDataForK4(string companyName, string middlemanName, int idCompanyMiddleman)
        //{
        //    var K4 = db.K4.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman);
        //    List<TableK2ToSend> K4Table = new List<TableK2ToSend>();

        //    int idContractType = db.ContractTypes.Where(n => n.Name == "ГО").Select(i => i.Id).FirstOrDefault();
        //    if (idContractType == 0)
        //        return "Error! Contract Type not found";

        //    int idCompanyContractType = db.CompanyContractTypes
        //        .Where(m => m.IdCompanyMiddleman == idCompanyMiddleman && m.IdContractType == idContractType)
        //        .Select(i => i.Id)
        //        .FirstOrDefault();
        //    if (idCompanyContractType == 0)
        //        return "Error! In this middlemant not exit this type of contract";

        //    var idsFranchise = db.ContractFranchises.Where(cct => cct.IdCompanyContractType == idCompanyContractType).Select(i => i.IdFranchise).ToList();
        //    if (idsFranchise.Count == 0)
        //        return "Error! This contract hasn`t franchise";

        //    var franchise = db.Franchises.Where(i => idsFranchise.Contains(i.Id)).ToList();
        //    if (franchise.Count == 0)
        //        return "Error! In Data Base not exist franchise for this conditions!";

        //    var insuranceZoneOfReg = db.InsuranceZoneOfRegistrations.ToList();
        //    var isLegal = new List<bool> { true, false };

        //    List<InsZoneOfReg> izor = new List<InsZoneOfReg>();
        //    List<bool> isLegalBool = new List<bool>();
        //    List<Fran> fran = new List<Fran>();
        //    foreach (var i in insuranceZoneOfReg)
        //    {
        //        InsZoneOfReg curZoneOfReg = new InsZoneOfReg();
        //        curZoneOfReg.id = i.Id;
        //        curZoneOfReg.name = i.Name;
        //        izor.Add(curZoneOfReg);
        //    }
        //    foreach (var i in isLegal)
        //    {
        //        isLegalBool.Add(i);
        //    }
        //    foreach (var i in franchise)
        //    {
        //        Fran curFran = new Fran();
        //        curFran.id = i.Id;
        //        curFran.sum = i.Sum;
        //        fran.Add(curFran);
        //    }

        //    foreach (var i in insuranceZoneOfReg)
        //    {
        //        foreach (var j in isLegal)
        //        {
        //            foreach (var w in franchise)
        //            {
        //                var b = db.ContractFranchises
        //                            .Where(cf => cf.IdCompanyContractType == idCompanyContractType && cf.IdFranchise == w.Id)
        //                            .Select(i_d => i_d.Id)
        //                            .FirstOrDefault();

        //                if (db.K4.Where(k => k.IdInsuranceZoneOfReg == i.Id
        //                                            && k.IsLegalEntity == j
        //                                            && k.IdContractFranchise == b
        //                                            && k.IdCompanyMiddleman == idCompanyMiddleman).Count() == 0)
        //                {
        //                    var newRowK4 = new K4()
        //                    {
        //                        IdInsuranceZoneOfReg = i.Id,
        //                        IsLegalEntity = j,
        //                        IdContractFranchise = db.ContractFranchises.Where(idf => idf.IdFranchise == w.Id && idf.IdCompanyContractType == idCompanyContractType).Select(x => x.Id).FirstOrDefault(),
        //                        Value = 0,
        //                        IdCompanyMiddleman = idCompanyMiddleman,
        //                    };

        //                    db.K4.Add(newRowK4);
        //                    db.SaveChanges();
        //                }
        //            }
        //        }
        //    }
        //    return "Success!";
        //}

        //private string InsertDataForBM(string companyName, string middlemanName, int idCompanyMiddleman)
        //{
        //    var BM = db.BonusMalus.Where(i => i.IdCompanyMiddleman == idCompanyMiddleman);
        //    List<TableBMToSend> BMTable = new List<TableBMToSend>();

        //    int idContractType = db.ContractTypes.Where(n => n.Name == "ГО").Select(i => i.Id).FirstOrDefault();
        //    if (idContractType == 0)
        //        return "Error! Contract Type not found";

        //    int idCompanyContractType = db.CompanyContractTypes
        //        .Where(m => m.IdCompanyMiddleman == idCompanyMiddleman && m.IdContractType == idContractType)
        //        .Select(i => i.Id)
        //        .FirstOrDefault();
        //    if (idCompanyContractType == 0)
        //        return "Error! In this middlemant not exit this type of contract";

        //    var idsFranchise = db.ContractFranchises.Where(cct => cct.IdCompanyContractType == idCompanyContractType).Select(i => i.IdFranchise).ToList();
        //    if (idsFranchise.Count == 0)
        //        return "Error! This contract hasn`t franchise";

        //    var franchise = db.Franchises.Where(i => idsFranchise.Contains(i.Id)).ToList();
        //    if (franchise.Count == 0)
        //        return "Error! In Data Base not exist franchise for this conditions!";

        //    var insuranceZoneOfReg = db.InsuranceZoneOfRegistrations.ToList();
        //    var isLegal = new List<bool> { true, false };
        //    var insuranceTypeOfCar = db.CarInsuranceTypes.ToList();

        //    List<InsCarType> ict = new List<InsCarType>();
        //    List<InsZoneOfReg> izor = new List<InsZoneOfReg>();
        //    List<bool> isLegalBool = new List<bool>();
        //    List<Fran> fran = new List<Fran>();
        //    foreach (var i in insuranceZoneOfReg)
        //    {
        //        InsZoneOfReg curZoneOfReg = new InsZoneOfReg();
        //        curZoneOfReg.id = i.Id;
        //        curZoneOfReg.name = i.Name;
        //        izor.Add(curZoneOfReg);
        //    }
        //    foreach (var i in isLegal)
        //    {
        //        isLegalBool.Add(i);
        //    }
        //    foreach (var i in insuranceTypeOfCar)
        //    {
        //        InsCarType curIcsCarType = new InsCarType();
        //        curIcsCarType.id = i.Id;
        //        curIcsCarType.name = i.Type;
        //        ict.Add(curIcsCarType);
        //    }
        //    foreach (var i in franchise)
        //    {
        //        Fran curFran = new Fran();
        //        curFran.id = i.Id;
        //        curFran.sum = i.Sum;
        //        fran.Add(curFran);
        //    }

        //    foreach (var i in insuranceZoneOfReg)
        //    {
        //        foreach (var j in isLegal)
        //        {
        //            foreach (var q in insuranceTypeOfCar)
        //            {
        //                foreach (var w in franchise)
        //                {
        //                    var b = db.ContractFranchises
        //                                .Where(cf => cf.IdCompanyContractType == idCompanyContractType && cf.IdFranchise == w.Id)
        //                                .Select(i_d => i_d.Id)
        //                                .FirstOrDefault();

        //                    if (db.BonusMalus.Where(k => k.IdInsuranceZoneOfReg == i.Id
        //                                                && k.IsLegalEntity == j
        //                                                && k.IdCarInsuranceType == q.Id
        //                                                && k.IdContractFranchise == b
        //                                                && k.IdCompanyMiddleman == idCompanyMiddleman).Count() == 0)
        //                    {
        //                        var newRowBM = new BonusMalus()
        //                        {
        //                            IdInsuranceZoneOfReg = i.Id,
        //                            IsLegalEntity = j,
        //                            IdCarInsuranceType = q.Id,
        //                            IdContractFranchise = db.ContractFranchises.Where(idf => idf.IdFranchise == w.Id && idf.IdCompanyContractType == idCompanyContractType).Select(x => x.Id).FirstOrDefault(),
        //                            Value = 0,
        //                            IdCompanyMiddleman = idCompanyMiddleman,
        //                        };

        //                        db.BonusMalus.Add(newRowBM);
        //                        db.SaveChanges();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return "Success!";
        //}

        [HttpPost]
        public string AddNewRecordToCoef()
        {
            int idCompany = 0, idMiddleman = 0, idCompanyMiddleman = 0;

            dynamic dataParsed = GetPotsRequestBody();
            string companyName = Convert.ToString(dataParsed.companyName);
            string middlemanName = Convert.ToString(dataParsed.middlemanName);
            string coef = Convert.ToString(dataParsed.coef);
            bool coefIsDependent = Convert.ToBoolean(dataParsed.coefIsDependent);

            ResponseToClient responseToClient = new ResponseToClient();
            JavaScriptSerializer js = new JavaScriptSerializer();

            if (coefIsDependent) { 
                ResponseToClient resultOfChekingCompanyMiddleman = GetCompanyMiddlemanData(companyName, middlemanName, ref idCompany, ref idMiddleman, ref idCompanyMiddleman);
                if (resultOfChekingCompanyMiddleman.responseType != ResponseType.Good)
                    return js.Serialize(resultOfChekingCompanyMiddleman);
            }

            switch (coef)
            {
                case "Basic":
                    return js.Serialize(responseToClient.responseType = ResponseType.Good);
                case "K1":
                    { 
                        string insuranceTypeOfCar = Convert.ToString(dataParsed.data.InsuranceTypeOfCar);

                        if (dataParsed.data.InsuranceTypeOfCar == null || dataParsed.data.Value == null)
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Не вдалося отримати данні. Заповніть всі поля та спробуйте знову.";
                            return js.Serialize(responseToClient);
                        }

                        int idInsuranceTypeOfCar = GetIdInsuranceTypeOfCar(insuranceTypeOfCar);

                        if (idInsuranceTypeOfCar == 0)
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = $"Не вдалося знайти тип транспорту {insuranceTypeOfCar} в базі даних";
                            return js.Serialize(responseToClient);
                        }

                        double newValue = 0;
                        try
                        {
                            newValue = Convert.ToDouble(dataParsed.data.Value);
                        }
                        catch
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Виникла помилка при обробці поля \"Значення\" коєфіцієнта. Значення має бути числом.";
                            return js.Serialize(responseToClient);
                        }

                        var checkExist = db.K1.Where(k => k.IdCarInsuranceType == idInsuranceTypeOfCar
                                                    && k.Value == newValue && k.IdCompanyMiddleman == idCompanyMiddleman);

                        if (checkExist.Any())
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Запис з такою умовою вже існує.";
                        }

                        var checkExistSimilar = db.K1.Where(k => k.IdCarInsuranceType == idInsuranceTypeOfCar
                                                                && k.Value != newValue 
                                                                && k.IdCompanyMiddleman == idCompanyMiddleman);

                        if (checkExistSimilar.Any())
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Ми знайшли схожий запис з іншим значенням поля \"Значення\". Будь ласка оновіть значення в існуючому записі.";
                            return js.Serialize(responseToClient);
                        }

                        K1 newK1Record = new K1();
                        newK1Record.IdCarInsuranceType = idInsuranceTypeOfCar;
                        newK1Record.Value = newValue;
                        newK1Record.IdCompanyMiddleman = idCompanyMiddleman;

                        try
                        {
                            db.K1.Add(newK1Record);
                            db.SaveChanges();
                        }
                        catch
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Виникла неочікувана помилка. Будь ласка спробуйте ще.";
                            return js.Serialize(responseToClient);
                        }

                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Запис для коефіцієнту {dataParsed.coef} успішно збережений в базі даних";
                        return js.Serialize(responseToClient);
                    }
                case "K2":
                    {
                        string insuranceTypeOfCar, isLegalEntity, carZoneOfRegistration, franchise;
                        try
                        {
                            insuranceTypeOfCar = Convert.ToString(dataParsed.data.InsuranceTypeOfCar);
                            isLegalEntity = Convert.ToString(dataParsed.data.IsLegalEntity);
                            carZoneOfRegistration = Convert.ToString(dataParsed.data.CarZoneOfRegistration);
                            franchise = Convert.ToString(dataParsed.data.Franchise);
                        }
                        catch
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Помилка отримання даних. Спробуйте ще";
                            return js.Serialize(responseToClient);
                        }

                        K2 newK2Record = new K2();
                        try
                        {
                            newK2Record.IdCarInsuranceType = db.CarInsuranceTypes.Where(c => c.Type == insuranceTypeOfCar).Select(c => c.Id).First();
                            newK2Record.IsLegalEntity = isLegalEntity == "Юр" ? true : false;
                            newK2Record.IdInsuranceZoneOfReg = db.InsuranceZoneOfRegistrations.Where(i => i.Name == carZoneOfRegistration).Select(i => i.Id).First();
                            newK2Record.IdContractFranchise = db.ContractFranchises.Where(c => c.Franchise.Sum.ToString() == franchise).Select(c => c.Id).First();
                            newK2Record.Value = dataParsed.data.Value;
                        }
                        catch
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Виникли проблеми з отриманням даних з бази даних.";
                            return js.Serialize(responseToClient);
                        }

                        var checkExist = db.K2.Where(k => k.IdCarInsuranceType == newK2Record.IdCarInsuranceType
                                                       && k.IsLegalEntity == newK2Record.IsLegalEntity
                                                       && k.IdInsuranceZoneOfReg == newK2Record.IdInsuranceZoneOfReg
                                                       && k.IdContractFranchise == newK2Record.IdContractFranchise                                                       
                                                       && k.IdCompanyMiddleman == idCompanyMiddleman
                                                       && k.Value == newK2Record.Value);

                        if (checkExist.Any())
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Запис з такою умовою вже існує.";
                        }

                        var checkExistSimilar = db.K2.Where(k => k.IdCarInsuranceType == newK2Record.IdCarInsuranceType
                                                              && k.IsLegalEntity == newK2Record.IsLegalEntity
                                                              && k.IdInsuranceZoneOfReg == newK2Record.IdInsuranceZoneOfReg
                                                              && k.IdContractFranchise == newK2Record.IdContractFranchise
                                                              && k.IdCompanyMiddleman == idCompanyMiddleman
                                                              && k.Value != newK2Record.Value);

                        if (checkExistSimilar.Any())
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Ми знайшли схожий запис з іншим значенням поля \"Значення\". Будь ласка оновіть значення в існуючому записі.";
                            return js.Serialize(responseToClient);
                        }

                        try
                        {
                            db.K2.Add(newK2Record);
                            db.SaveChanges();
                        }
                        catch
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Виникла проблема з записом нового значення до бази даних.";
                        }

                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Запис для коефіцієнту {dataParsed.coef} успішно збережений в базі даних";
                        return js.Serialize(responseToClient);
                    }
                case "K3":
                    {
                        string insuranceTypeOfCar, isLegalEntity, carZoneOfRegistration;
                        try
                        {
                            insuranceTypeOfCar = Convert.ToString(dataParsed.data.InsuranceTypeOfCar);
                            isLegalEntity = Convert.ToString(dataParsed.data.IsLegalEntity);
                            carZoneOfRegistration = Convert.ToString(dataParsed.data.CarZoneOfRegistration);
                        }
                        catch
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Помилка отримання даних. Спробуйте ще";
                            return js.Serialize(responseToClient);
                        }

                        K3 newK3Record = new K3();
                        try
                        {
                            newK3Record.IdCarInsuranceType = db.CarInsuranceTypes.Where(c => c.Type == insuranceTypeOfCar).Select(c => c.Id).First();
                            newK3Record.IsLegalEntity = isLegalEntity == "Юр" ? true : false;
                            newK3Record.IdInsuranceZoneOfReg = db.InsuranceZoneOfRegistrations.Where(i => i.Name == carZoneOfRegistration).Select(i => i.Id).First();
                            newK3Record.Value = dataParsed.data.Value;
                        }
                        catch
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Виникли проблеми з отриманням даних з бази даних.";
                            return js.Serialize(responseToClient);
                        }

                        var checkExist = db.K3.Where(k => k.IdCarInsuranceType == newK3Record.IdCarInsuranceType
                                                       && k.IsLegalEntity == newK3Record.IsLegalEntity
                                                       && k.IdInsuranceZoneOfReg == newK3Record.IdInsuranceZoneOfReg
                                                       && k.IdCompanyMiddleman == idCompanyMiddleman
                                                       && k.Value == newK3Record.Value);

                        if (checkExist.Any())
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Запис з такою умовою вже існує.";
                        }

                        var checkExistSimilar = db.K3.Where(k => k.IdCarInsuranceType == newK3Record.IdCarInsuranceType
                                                              && k.IsLegalEntity == newK3Record.IsLegalEntity
                                                              && k.IdInsuranceZoneOfReg == newK3Record.IdInsuranceZoneOfReg
                                                              && k.IdCompanyMiddleman == idCompanyMiddleman
                                                              && k.Value != newK3Record.Value);

                        if (checkExistSimilar.Any())
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Ми знайшли схожий запис з іншим значенням поля \"Значення\". Будь ласка оновіть значення в існуючому записі.";
                            return js.Serialize(responseToClient);
                        }

                        try
                        {
                            db.K3.Add(newK3Record);
                            db.SaveChanges();
                        }
                        catch
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Виникла проблема з записом нового значення до бази даних.";
                        }

                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Запис для коефіцієнту {dataParsed.coef} успішно збережений в базі даних";
                        return js.Serialize(responseToClient);
                    }
                case "K4":
                    {
                        string carZoneOfRegistration, isLegalEntity, franchise;
                        try
                        {
                            carZoneOfRegistration = Convert.ToString(dataParsed.data.CarZoneOfRegistration);
                            isLegalEntity = Convert.ToString(dataParsed.data.IsLegalEntity);                            
                            franchise = Convert.ToString(dataParsed.data.Franchise);
                        }
                        catch
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Помилка отримання даних. Спробуйте ще";
                            return js.Serialize(responseToClient);
                        }

                        K4 newK4Record = new K4();
                        try
                        {
                            newK4Record.IdInsuranceZoneOfReg = db.InsuranceZoneOfRegistrations.Where(i => i.Name == carZoneOfRegistration).Select(i => i.Id).First();
                            newK4Record.IsLegalEntity = isLegalEntity == "Юр" ? true : false;
                            newK4Record.IdContractFranchise = db.ContractFranchises.Where(c => c.Franchise.Sum.ToString() == franchise).Select(c => c.Id).First();
                            newK4Record.Value = dataParsed.data.Value;
                        }
                        catch
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Виникли проблеми з отриманням даних з бази даних.";
                            return js.Serialize(responseToClient);
                        }

                        var checkExist = db.K2.Where(k => k.IdInsuranceZoneOfReg == newK4Record.IdInsuranceZoneOfReg
                                                       && k.IsLegalEntity == newK4Record.IsLegalEntity
                                                       && k.IdContractFranchise == newK4Record.IdContractFranchise
                                                       && k.IdCompanyMiddleman == idCompanyMiddleman
                                                       && k.Value == newK4Record.Value);

                        if (checkExist.Any())
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Запис з такою умовою вже існує.";
                        }

                        var checkExistSimilar = db.K2.Where(k => k.IdInsuranceZoneOfReg == newK4Record.IdInsuranceZoneOfReg
                                                              && k.IsLegalEntity == newK4Record.IsLegalEntity
                                                              && k.IdContractFranchise == newK4Record.IdContractFranchise
                                                              && k.IdCompanyMiddleman == idCompanyMiddleman
                                                              && k.Value != newK4Record.Value);

                        if (checkExistSimilar.Any())
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Ми знайшли схожий запис з іншим значенням поля \"Значення\". Будь ласка оновіть значення в існуючому записі.";
                            return js.Serialize(responseToClient);
                        }

                        try
                        {
                            db.K4.Add(newK4Record);
                            db.SaveChanges();
                        }
                        catch
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Виникла проблема з записом нового значення до бази даних.";
                        }

                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Запис для коефіцієнту {dataParsed.coef} успішно збережений в базі даних";
                        return js.Serialize(responseToClient);
                    }
                case "K5":
                    {
                        int period;
                        try
                        {
                            period = Convert.ToInt32(dataParsed.data.period);
                        }
                        catch
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Помилка отримання даних. Спробуйте ще";
                            return js.Serialize(responseToClient);
                        }

                        K5 newK5Record = new K5();
                        try
                        {
                            newK5Record.Period = period;
                            newK5Record.Value = dataParsed.data.Value;
                        }
                        catch
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Виникли проблеми з отриманням даних з бази даних.";
                            return js.Serialize(responseToClient);
                        }

                        var checkExist = db.K5.Where(k => k.Period == newK5Record.Period
                                                       && k.Value == newK5Record.Value);

                        if (checkExist.Any())
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Запис з такою умовою вже існує.";
                        }

                        var checkExistSimilar = db.K5.Where(k => k.Period == newK5Record.Period
                                                       && k.Value != newK5Record.Value);

                        if (checkExistSimilar.Any())
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Ми знайшли схожий запис з іншим значенням поля \"Значення\". Будь ласка оновіть значення в існуючому записі.";
                            return js.Serialize(responseToClient);
                        }

                        try
                        {
                            db.K5.Add(newK5Record);
                            db.SaveChanges();
                        }
                        catch
                        {
                            responseToClient.responseType = ResponseType.Bad;
                            responseToClient.responseText = "Виникла проблема з записом нового значення до бази даних.";
                        }

                        responseToClient.responseType = ResponseType.Good;
                        responseToClient.responseText = $"Запис для коефіцієнту {dataParsed.coef} успішно збережений в базі даних";
                        return js.Serialize(responseToClient);
                    }
            }

            responseToClient.responseType = ResponseType.Bad;
            responseToClient.responseText = "Виникли проблеми з назвою коефіцієнта.";
            return js.Serialize(responseToClient);
        }

        private int GetIdInsuranceTypeOfCar(string insuranceTypeOfCar)
        {
            return db.CarInsuranceTypes.Where(c => c.Type == insuranceTypeOfCar).Select(c => c.Id).First();
        }

        public string GetConditionsToAddK1Record()
        {
            Dictionary<string, List<string>> dataToSend = new Dictionary<string, List<string>>();
            List<string> carInsuranceTypes = db.CarInsuranceTypes.Select(t => t.Type).ToList();
            dataToSend.Add("InsuranceTypeOfCar", carInsuranceTypes);
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(dataToSend);
        }

        public string GetConditionsToAddK2Record(string companyName, string middlemanName)
        {
            Dictionary<string, List<string>> dataToSend = new Dictionary<string, List<string>>();

            List<string> carInsuranceZoneOfReg = db.InsuranceZoneOfRegistrations.Select(n => n.Name).ToList();
            List<string> isLegalEntity = new List<string>() { "Юр", "Фіз" };
            List<string> carInsuranceTypes = db.CarInsuranceTypes.Select(t => t.Type).ToList();

            int idCompany = 0, idMiddleman = 0, idCompanyMiddleman = 0;

            ResponseToClient responseToClient = new ResponseToClient();

            JavaScriptSerializer js = new JavaScriptSerializer();

            ResponseToClient resultOfChekingCompanyMiddleman = GetCompanyMiddlemanData(companyName, middlemanName, ref idCompany, ref idMiddleman, ref idCompanyMiddleman);
            if (resultOfChekingCompanyMiddleman.responseType != ResponseType.Good)
                return js.Serialize(resultOfChekingCompanyMiddleman);

            List<string> franchise = GetListFranchiseForCompanyMiddlemanAndContractType(idCompanyMiddleman, "ГО");

            dataToSend.Add("CarZoneOfRegistration", carInsuranceZoneOfReg);
            dataToSend.Add("IsLegalEntity", isLegalEntity);
            dataToSend.Add("InsuranceTypeOfCar", carInsuranceTypes);
            dataToSend.Add("Franchise", franchise);

            return js.Serialize(dataToSend);
        }

        public string GetConditionsToAddK3Record()
        {
            Dictionary<string, List<string>> dataToSend = new Dictionary<string, List<string>>();

            List<string> carInsuranceZoneOfReg = db.InsuranceZoneOfRegistrations.Select(n => n.Name).ToList();
            List<string> isLegalEntity = new List<string>() { "Юр", "Фіз" };
            List<string> carInsuranceTypes = db.CarInsuranceTypes.Select(t => t.Type).ToList();

            dataToSend.Add("CarZoneOfRegistration", carInsuranceZoneOfReg);
            dataToSend.Add("IsLegalEntity", isLegalEntity);
            dataToSend.Add("InsuranceTypeOfCar", carInsuranceTypes);

            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(dataToSend);
        }

        public string GetConditionsToAddK4Record(string companyName, string middlemanName)
        {
            Dictionary<string, List<string>> dataToSend = new Dictionary<string, List<string>>();

            List<string> carInsuranceZoneOfReg = db.InsuranceZoneOfRegistrations.Select(n => n.Name).ToList();
            List<string> isLegalEntity = new List<string>() { "Юр", "Фіз" };

            int idCompany = 0, idMiddleman = 0, idCompanyMiddleman = 0;

            ResponseToClient responseToClient = new ResponseToClient();

            JavaScriptSerializer js = new JavaScriptSerializer();

            ResponseToClient resultOfChekingCompanyMiddleman = GetCompanyMiddlemanData(companyName, middlemanName, ref idCompany, ref idMiddleman, ref idCompanyMiddleman);
            if (resultOfChekingCompanyMiddleman.responseType != ResponseType.Good)
                return js.Serialize(resultOfChekingCompanyMiddleman);

            List<string> franchise = GetListFranchiseForCompanyMiddlemanAndContractType(idCompanyMiddleman, "ГО");

            dataToSend.Add("CarZoneOfRegistration", carInsuranceZoneOfReg);
            dataToSend.Add("IsLegalEntity", isLegalEntity);
            dataToSend.Add("Franchise", franchise);

            return js.Serialize(dataToSend);
        }

        public string GetConditionsToAddK5Record()
        {
            Dictionary<string, List<string>> dataToSend = new Dictionary<string, List<string>>();

            //List<string> period = db.K5.Select(p => p.Period.ToString()).ToList();
            List<string> period = new List<string>()
            {
                "6", "7", "8", "9", "10", "11", "12"
            };

            dataToSend.Add("Period", period);

            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(dataToSend);
        }

        public string GetConditionsToAddK6Record()
        {
            Dictionary<string, List<string>> dataToSend = new Dictionary<string, List<string>>();

            List<string> IsCheater = new List<string>() { "Шахрай", "Не шахрай" };

            dataToSend.Add("IsCheater", IsCheater);

            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(dataToSend);
        }

        public string GetConditionsToAddK7Record()
        {
            Dictionary<string, List<string>> dataToSend = new Dictionary<string, List<string>>();

            //List<string> period = db.K7.Select(p => p.Period.ToString()).ToList();
            List<string> period = new List<string>()
            {
               "0,5", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"
            };

            dataToSend.Add("Period", period);

            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(dataToSend);
        }

        public string GetConditionsToAddBMRecord(string companyName, string middlemanName)
        {
            Dictionary<string, List<string>> dataToSend = new Dictionary<string, List<string>>();

            List<string> carInsuranceZoneOfReg = db.InsuranceZoneOfRegistrations.Select(n => n.Name).ToList();
            List<string> isLegalEntity = new List<string>() { "Юр", "Фіз" };
            List<string> carInsuranceTypes = db.CarInsuranceTypes.Select(t => t.Type).ToList();

            int idCompany = 0, idMiddleman = 0, idCompanyMiddleman = 0;

            ResponseToClient responseToClient = new ResponseToClient();

            JavaScriptSerializer js = new JavaScriptSerializer();

            ResponseToClient resultOfChekingCompanyMiddleman = GetCompanyMiddlemanData(companyName, middlemanName, ref idCompany, ref idMiddleman, ref idCompanyMiddleman);
            if (resultOfChekingCompanyMiddleman.responseType != ResponseType.Good)
                return js.Serialize(resultOfChekingCompanyMiddleman);

            List<string> franchise = GetListFranchiseForCompanyMiddlemanAndContractType(idCompanyMiddleman, "ГО");

            dataToSend.Add("CarZoneOfRegistration", carInsuranceZoneOfReg);
            dataToSend.Add("IsLegalEntity", isLegalEntity);
            dataToSend.Add("InsuranceTypeOfCar", carInsuranceTypes);
            dataToSend.Add("Franchise", franchise);

            return js.Serialize(dataToSend);
        }

        //SUPPORTING METHODS
        private List<string> GetListFranchiseForCompanyMiddlemanAndContractType(int idCompanyMiddleman, string contractType)
        {
            int idContractType = db.ContractTypes.Where(ct => ct.Name == contractType).Select(ct => ct.Id).First();

            List<int> idsCompanyContractTypes = db.CompanyContractTypes
                                         .Where(cct => cct.IdCompanyMiddleman == idCompanyMiddleman && cct.IdContractType == idContractType)
                                         .Select(cct => cct.Id)
                                         .ToList();
            List<string> franchise = db.ContractFranchises
                                        .Where(cf => idsCompanyContractTypes.Contains(cf.IdCompanyContractType))
                                        .Select(cf => cf.Franchise.Sum.ToString())
                                        .ToList();

            return franchise;
        }

        private TitlesToSend FillTitleToSend(string name, string titleUkr, string titleRus)
        {
            return new TitlesToSend
            {
                Name = name,
                TitleUkr = titleUkr,
                TitleRus = titleRus
            };
        }

        private dynamic GetPotsRequestBody()
        {
            System.IO.Stream request = Request.InputStream;
            request.Seek(0, SeekOrigin.Begin);
            string bodyData = new StreamReader(request).ReadToEnd();
            return JsonConvert.DeserializeObject(bodyData);
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

    //Supporting classes
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

    //public class TableK1ToSend
    //{
    //    public string InsuranceTypeOfCar { get; set; }
    //    public double Value { get; set; }
    //}

    //public class TableK2ToSend
    //{
    //    public string CarZoneOfRegistration { get; set; }
    //    public string IsLegalEntity { get; set; }
    //    public string InsuranceTypeOfCar { get; set; }
    //    public double Franchise { get; set; }
    //    public double Value { get; set; }
    //}

    //public class TableK3ToSend
    //{
    //    public string CarZoneOfRegistration { get; set; }
    //    public string IsLegalEntity { get; set; }
    //    public string InsuranceTypeOfCar { get; set; }
    //    public double Value { get; set; }
    //}

    //public class TableK4ToSend
    //{
    //    public string CarZoneOfRegistration { get; set; }
    //    public string IsLegalEntity { get; set; }
    //    public double Franchise { get; set; }
    //    public double Value { get; set; }
    //}

    //public class TableK5ToSend
    //{
    //    public int Period { get; set; }
    //    public double Value { get; set; }
    //}

    //public class TableK6ToSend
    //{
    //    public string IsCheater { get; set; }
    //    public double? Value { get; set; }
    //}

    //public class TableK7ToSend
    //{
    //    public double Period { get; set; }
    //    public double Value { get; set; }
    //}

    //public class TableBMToSend
    //{
    //    public string CarZoneOfRegistration { get; set; }
    //    public string IsLegalEntity { get; set; }
    //    public string InsuranceTypeOfCar { get; set; }
    //    public double Franchise { get; set; }
    //    public double Value { get; set; }
    //}

    //public class TableKParkToSend
    //{
    //    public string IsLegalEntity { get; set; }
    //    public int TransportCountFrom { get; set; }
    //    public int TransportCountTo { get; set; }
    //    public double Value { get; set; }
    //}

    //public class TitlesToSend
    //{
    //    public string Name { get; set; }
    //    public string TitleUkr { get; set; }
    //    public string TitleRus { get; set; }
    //}
    
}