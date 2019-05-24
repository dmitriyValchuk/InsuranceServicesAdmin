using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using InsuranceServicesAdminLight.Business.DataManipulation;
using InsuranceServicesAdminLight.Models;

namespace InsuranceServicesAdminLight.Business.Conditions
{
    public class ConditionGeneration
    {
        static InsuranceServicesContext db = new InsuranceServicesContext();
        static JavaScriptSerializer js = new JavaScriptSerializer();
        static ResponseToClient responseToClient = new ResponseToClient();

        static public string K1(int idCompanyMiddleman)
        {
            List<CarInsuranceType> carInsuranceTypes = CarInsuranceTypeDataManipulation.GetMulti();
            if(carInsuranceTypes == null)
            {
                responseToClient.responseType = ResponseType.Bad;
                responseToClient.responseText = "Не знайдено умови \"Тип транспорту\"";
                return js.Serialize(responseToClient);
            }

            foreach (var cit in carInsuranceTypes)
            {
                if (K1DataManipulation.IsConditionExist(cit, idCompanyMiddleman))
                {
                    var newRowK1 = new K1()
                    {
                        IdCarInsuranceType = cit.Id,
                        Value = 0,
                        IdCompanyMiddleman = idCompanyMiddleman,
                    };

                    try
                    {
                        K1DataManipulation.Insert(newRowK1);
                    }
                    catch
                    {
                        responseToClient.responseType = ResponseType.Bad;
                        responseToClient.responseText = $"Виникла помилка при генерації запису для умови \"{cit.Type}\"";
                        return js.Serialize(responseToClient);
                    }
                }
            }

            responseToClient.responseType = ResponseType.Good;
            responseToClient.responseText = "Генерація даних пройшла успішно.";
            return js.Serialize(responseToClient);
        }

        static public string K2(int idCompanyMiddleman)
        {
            int idContractType = ContractTypeDataManipulation.GetId("ГО");
            int idCompanyContractType = CompanyContractTypeDataManipulation.GetId(idCompanyMiddleman, idContractType);
            List<Franchise> franchise = FranchiseDataManipulation.GetFranchises(ContractFranchiseDataManipulation.GetFranchiseIds(idCompanyContractType));
            List<InsuranceZoneOfRegistration> insuranceZoneOfReg = InsuranceZoneOfRegistrationDataManipulation.GetInsuranceZoneOfRegistrations();
            List<bool> isLegal = new List<bool> { true, false };
            List<CarInsuranceType> insuranceTypeOfCar = CarInsuranceTypeDataManipulation.GetMulti();

            foreach (var izor in insuranceZoneOfReg)
            {
                foreach (var il in isLegal)
                {
                    foreach (var itoc in insuranceTypeOfCar)
                    {
                        foreach (var f in franchise)
                        {
                            var idContractFranchise = ContractFranchiseDataManipulation.GetId(idCompanyContractType, f.Id);

                            if (K2DataManipulation.IsConditionExist(izor.Id, il, itoc.Id, idContractFranchise, idCompanyContractType, idCompanyMiddleman))
                            {
                                var newRowK2 = new K2()
                                {
                                    IdInsuranceZoneOfReg = izor.Id,
                                    IsLegalEntity = il,
                                    IdCarInsuranceType = itoc.Id,
                                    IdContractFranchise = idContractFranchise,
                                    Value = 0,
                                    IdCompanyMiddleman = idCompanyMiddleman,
                                };

                                try
                                {
                                    K2DataManipulation.Insert(newRowK2);
                                }
                                catch
                                {
                                    responseToClient.responseType = ResponseType.Bad;
                                    responseToClient.responseText = $"Виникла помилка при генерації запису для умов: \"{izor.Name}\"; \"{il}\"; \"{itoc.Type}\"; \"{f.Sum}\".";
                                    return js.Serialize(responseToClient);
                                }
                            }
                        }
                    }
                }
            }

            responseToClient.responseType = ResponseType.Good;
            responseToClient.responseText = "Генерація даних пройшла успішно.";
            return js.Serialize(responseToClient);
        }
    }
}