using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuranceServicesAdminLight.Models;
using InsuranceServicesAdminLight.Business.DataManipulation;
using InsuranceServicesAdminLight.Business.DataToSend;
using System.Web.Script.Serialization;

namespace InsuranceServicesAdminLight.Business.Conditions
{
    public class GetConditions
    {
        static InsuranceServicesContext db = new InsuranceServicesContext();
        static JavaScriptSerializer js = new JavaScriptSerializer();
        static ResponseToClient responseToClient = new ResponseToClient();

        static public string K1(string companyName, string middlemanName)
        {
            int idCompany, idMiddleman, idCompanyMiddleman;

            try
            {
                idCompany = CompanyDataManipulation.GetId(companyName);
                idMiddleman = MiddlemanDataManipulation.GetId(middlemanName);
                idCompanyMiddleman = CompanyMiddlemanDataManipulation.GetId(companyName, middlemanName);
            }
            catch
            {
                responseToClient.responseType = ResponseType.Bad;
                responseToClient.responseText = "Виникли проблеми з отриманням даних.";
                return js.Serialize(responseToClient);
            }

            List<K1> K1Records = K1DataManipulation.GetMulti(idCompanyMiddleman);
            List<TableK1ToSend> K1Table = new List<TableK1ToSend>();

            //Uncomment for auto generate conditions
            //ConditionGeneration.K1(idCompanyMiddleman);
            //K1Records = K1DataManipulation.GetMulti(idCompanyMiddleman);

            foreach (var k in K1Records)
            {
                TableK1ToSend tempTableRow = new TableK1ToSend();
                tempTableRow.InsuranceTypeOfCar = db.CarInsuranceTypes.Where(cit => cit.Id == k.IdCarInsuranceType).Select(cit => cit.Type).FirstOrDefault();
                tempTableRow.Value = k.Value;
                K1Table.Add(tempTableRow);
            }

            List<TitlesToSend> titles = new List<TitlesToSend>();
            titles.Add(FillTitleToSend(name: "InsuranceTypeOfCar", titleUkr: "Тип транспорту", titleRus: "Тип транспорта"));
            titles.Add(FillTitleToSend(name: "Value", titleUkr: "Значення", titleRus: "Значение"));

            Dictionary<string, object> dataToSend = new Dictionary<string, object>();

            dataToSend.Add("titles", titles);
            dataToSend.Add("data", K1Table);

            return js.Serialize(dataToSend);
        }

        static public string K2(string companyName, string middlemanName)
        {
            int idCompany, idMiddleman, idCompanyMiddleman;

            try
            {
                idCompany = CompanyDataManipulation.GetId(companyName);
                idMiddleman = MiddlemanDataManipulation.GetId(middlemanName);
                idCompanyMiddleman = CompanyMiddlemanDataManipulation.GetId(companyName, middlemanName);
            }
            catch
            {
                responseToClient.responseType = ResponseType.Bad;
                responseToClient.responseText = "Виникли проблеми з отриманням даних.";
                return js.Serialize(responseToClient);
            }

            List<K2> K2Records = K2DataManipulation.GetMulti(idCompanyMiddleman);
            List<TableK2ToSend> K2Table = new List<TableK2ToSend>();

            //Uncomment for auto generate conditions
            ConditionGeneration.K2(idCompanyMiddleman);
            K2Records = K2DataManipulation.GetMulti(idCompanyMiddleman);

            K2Records = K2DataManipulation.GetMulti(idCompanyMiddleman);

            foreach (var k in K2Records)
            {
                TableK2ToSend tempTableRow = new TableK2ToSend();
                tempTableRow.CarZoneOfRegistration = InsuranceZoneOfRegistrationDataManipulation.GetInsuranceZoneOfRegistrationStr(k.IdInsuranceZoneOfReg);
                tempTableRow.IsLegalEntity = k.IsLegalEntity ? "Юр" : "Фіз";
                tempTableRow.InsuranceTypeOfCar = CarInsuranceTypeDataManipulation.GetCarInsuranceTypeStr(k.IdCarInsuranceType);
                tempTableRow.Franchise = FranchiseDataManipulation.GetFranchiseSum(k.ContractFranchise.IdFranchise);
                tempTableRow.Value = k.Value;
                K2Table.Add(tempTableRow);
            }

            List<TitlesToSend> titles = new List<TitlesToSend>();
            titles.Add(FillTitleToSend(name: "CarZoneOfRegistration", titleUkr: "Зона регестрації", titleRus: "Зона регистрации"));
            titles.Add(FillTitleToSend(name: "IsLegalEntity", titleUkr: "Юр / Фіз", titleRus: "Юр / Физ"));
            titles.Add(FillTitleToSend(name: "InsuranceTypeOfCar", titleUkr: "Тип транспорту", titleRus: "Тип транспорта"));
            titles.Add(FillTitleToSend(name: "Franchise", titleUkr: "Франшиза", titleRus: "Франшиза"));
            titles.Add(FillTitleToSend(name: "Value", titleUkr: "Значення", titleRus: "Значение"));

            Dictionary<string, object> dataToSend = new Dictionary<string, object>();

            dataToSend.Add("titles", titles);
            dataToSend.Add("data", K2Table);

            return js.Serialize(dataToSend);
        }

        static private TitlesToSend FillTitleToSend(string name, string titleUkr, string titleRus)
        {
            return new TitlesToSend
            {
                Name = name,
                TitleUkr = titleUkr,
                TitleRus = titleRus
            };
        }
    }
}