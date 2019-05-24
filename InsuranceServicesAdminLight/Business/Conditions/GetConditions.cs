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

        public static string K1(string companyName, string middlemanName)
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

        public static string K2(string companyName, string middlemanName)
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
            //ConditionGeneration.K2(idCompanyMiddleman);
            //K2Records = K2DataManipulation.GetMulti(idCompanyMiddleman);

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

        public static string K3(string companyName, string middlemanName)
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

            List<K3> K3Records = K3DataManipulation.GetMulti(idCompanyMiddleman);
            List<TableK3ToSend> K3Table = new List<TableK3ToSend>();

            //Uncomment for auto generate conditions
            //ConditionGeneration.K3(idCompanyMiddleman);
            //K3Records = K3DataManipulation.GetMulti(idCompanyMiddleman);

            foreach (var k in K3Records)
            {
                TableK3ToSend tempTableRow = new TableK3ToSend();
                tempTableRow.CarZoneOfRegistration = InsuranceZoneOfRegistrationDataManipulation.GetInsuranceZoneOfRegistrationStr(k.IdInsuranceZoneOfReg);
                tempTableRow.IsLegalEntity = k.IsLegalEntity ? "Юр" : "Фіз";
                tempTableRow.InsuranceTypeOfCar = CarInsuranceTypeDataManipulation.GetCarInsuranceTypeStr(k.IdCarInsuranceType);
                tempTableRow.Value = k.Value;
                K3Table.Add(tempTableRow);
            }

            List<TitlesToSend> titles = new List<TitlesToSend>();
            titles.Add(FillTitleToSend(name: "CarZoneOfRegistration", titleUkr: "Зона регестрації", titleRus: "Зона регистрации"));
            titles.Add(FillTitleToSend(name: "IsLegalEntity", titleUkr: "Юр / Фіз", titleRus: "Юр / Физ"));
            titles.Add(FillTitleToSend(name: "InsuranceTypeOfCar", titleUkr: "Тип транспорту", titleRus: "Тип транспорта"));
            titles.Add(FillTitleToSend(name: "Value", titleUkr: "Значення", titleRus: "Значение"));

            Dictionary<string, object> dataToSend = new Dictionary<string, object>();

            dataToSend.Add("titles", titles);
            dataToSend.Add("data", K3Table);

            return js.Serialize(dataToSend);
        }

        public static string K4(string companyName, string middlemanName)
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

            List<K4> K4Records = K4DataManipulation.GetMulti(idCompanyMiddleman);
            List<TableK4ToSend> K4Table = new List<TableK4ToSend>();

            //Uncomment for auto generate conditions
            //ConditionGeneration.K4(idCompanyMiddleman);
            //K4Records = K4DataManipulation.GetMulti(idCompanyMiddleman);

            foreach (var k in K4Records)
            {
                TableK4ToSend tempTableRow = new TableK4ToSend();
                tempTableRow.CarZoneOfRegistration = InsuranceZoneOfRegistrationDataManipulation.GetInsuranceZoneOfRegistrationStr(k.IdInsuranceZoneOfReg);
                tempTableRow.IsLegalEntity = k.IsLegalEntity ? "Юр" : "Фіз";
                tempTableRow.Franchise = FranchiseDataManipulation.GetFranchiseSum(k.ContractFranchise.IdFranchise);
                tempTableRow.Value = k.Value;
                K4Table.Add(tempTableRow);
            }

            List<TitlesToSend> titles = new List<TitlesToSend>();
            titles.Add(FillTitleToSend(name: "CarZoneOfRegistration", titleUkr: "Зона регестрації", titleRus: "Зона регистрации"));
            titles.Add(FillTitleToSend(name: "IsLegalEntity", titleUkr: "Юр / Фіз", titleRus: "Юр / Физ"));
            titles.Add(FillTitleToSend(name: "Franchise", titleUkr: "Франшиза", titleRus: "Франшиза"));
            titles.Add(FillTitleToSend(name: "Value", titleUkr: "Значення", titleRus: "Значение"));

            Dictionary<string, object> dataToSend = new Dictionary<string, object>();

            dataToSend.Add("titles", titles);
            dataToSend.Add("data", K4Table);

            return js.Serialize(dataToSend);
        }

        public static string K5()
        {
            List<K5> K5Records = K5DataManipulation.GetMulti();
            List<TableK5ToSend> K5Table = new List<TableK5ToSend>();

            foreach (var k in K5Records)
            {
                TableK5ToSend tempTableRow = new TableK5ToSend();
                tempTableRow.Period = k.Period;
                tempTableRow.Value = k.Value;
                K5Table.Add(tempTableRow);
            }

            List<TitlesToSend> titles = new List<TitlesToSend>();
            titles.Add(FillTitleToSend(name: "Period", titleUkr: "Період страхування", titleRus: "Период страхования"));
            titles.Add(FillTitleToSend(name: "Value", titleUkr: "Значення", titleRus: "Значение"));

            Dictionary<string, object> dataToSend = new Dictionary<string, object>();

            dataToSend.Add("titles", titles);
            dataToSend.Add("data", K5Table);

            return js.Serialize(dataToSend);
        }

        public static string K6()
        {
            List<K6> K6Records = K6DataManipulation.GetMulti();
            List<TableK6ToSend> K6Table = new List<TableK6ToSend>();

            foreach (var k in K6Records)
            {
                TableK6ToSend tempTableRow = new TableK6ToSend();
                tempTableRow.IsCheater = k.IsCheater.Value ? "Шахрай" : "Не шахрай";
                tempTableRow.Value = k.Value;
                K6Table.Add(tempTableRow);
            }

            List<TitlesToSend> titles = new List<TitlesToSend>();
            titles.Add(FillTitleToSend(name: "IsCheater", titleUkr: "Шахрай", titleRus: "Мошенник"));
            titles.Add(FillTitleToSend(name: "Value", titleUkr: "Значення", titleRus: "Значение"));

            Dictionary<string, object> dataToSend = new Dictionary<string, object>();

            dataToSend.Add("titles", titles);
            dataToSend.Add("data", K6Table);

            return js.Serialize(dataToSend);
        }

        public static string K7()
        {
            List<K7> K7Records = K7DataManipulation.GetMulti();
            List<TableK7ToSend> K7Table = new List<TableK7ToSend>();

            foreach (var k in K7Records)
            {
                TableK7ToSend tempTableRow = new TableK7ToSend();
                tempTableRow.Period = k.Period;
                tempTableRow.Value = k.Value;
                K7Table.Add(tempTableRow);
            }

            List<TitlesToSend> titles = new List<TitlesToSend>();
            titles.Add(FillTitleToSend(name: "Period", titleUkr: "Період страхування", titleRus: "Период страхования"));
            titles.Add(FillTitleToSend(name: "Value", titleUkr: "Значення", titleRus: "Значение"));

            Dictionary<string, object> dataToSend = new Dictionary<string, object>();

            dataToSend.Add("titles", titles);
            dataToSend.Add("data", K7Table);

            return js.Serialize(dataToSend);
        }

        public static string BM(string companyName, string middlemanName)
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

            List<BonusMalus> BMRecords = BonusMalusDataManipulation.GetMulti(idCompanyMiddleman);
            List<TableBMToSend> BMTable = new List<TableBMToSend>();

            //Uncomment for auto generate conditions
            //ConditionGeneration.BM(idCompanyMiddleman);
            //BMRecords = BMDataManipulation.GetMulti(idCompanyMiddleman);

            BMRecords = BonusMalusDataManipulation.GetMulti(idCompanyMiddleman);

            foreach (var k in BMRecords)
            {
                TableBMToSend tempTableRow = new TableBMToSend();
                tempTableRow.CarZoneOfRegistration = InsuranceZoneOfRegistrationDataManipulation.GetInsuranceZoneOfRegistrationStr(k.IdInsuranceZoneOfReg);
                tempTableRow.IsLegalEntity = k.IsLegalEntity ? "Юр" : "Фіз";
                tempTableRow.InsuranceTypeOfCar = CarInsuranceTypeDataManipulation.GetCarInsuranceTypeStr((int)k.IdCarInsuranceType);
                tempTableRow.Franchise = FranchiseDataManipulation.GetFranchiseSum(k.ContractFranchise.IdFranchise);
                tempTableRow.Value = k.Value;
                BMTable.Add(tempTableRow);
            }

            List<TitlesToSend> titles = new List<TitlesToSend>();
            titles.Add(FillTitleToSend(name: "CarZoneOfRegistration", titleUkr: "Зона регестрації", titleRus: "Зона регистрации"));
            titles.Add(FillTitleToSend(name: "IsLegalEntity", titleUkr: "Юр / Фіз", titleRus: "Юр / Физ"));
            titles.Add(FillTitleToSend(name: "InsuranceTypeOfCar", titleUkr: "Тип транспорту", titleRus: "Тип транспорта"));
            titles.Add(FillTitleToSend(name: "Franchise", titleUkr: "Франшиза", titleRus: "Франшиза"));
            titles.Add(FillTitleToSend(name: "Value", titleUkr: "Значення", titleRus: "Значение"));

            Dictionary<string, object> dataToSend = new Dictionary<string, object>();

            dataToSend.Add("titles", titles);
            dataToSend.Add("data", BMTable);

            return js.Serialize(dataToSend);
        }

        public static string KPark()
        {
            var KPark = KParkDataManipulation.GetMulti();
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

            List<TitlesToSend> titles = new List<TitlesToSend>();
            titles.Add(FillTitleToSend(name: "IsLegalEntity", titleUkr: "Юр / Фіз", titleRus: "Юр / Физ"));
            titles.Add(FillTitleToSend(name: "TransportCountFrom", titleUkr: "Кількість ТЗ від", titleRus: "Количество ТС от"));
            titles.Add(FillTitleToSend(name: "TransportCountTo", titleUkr: "Кількість ТЗ до", titleRus: "Количество ТС до"));
            titles.Add(FillTitleToSend(name: "Value", titleUkr: "Значення", titleRus: "Значение"));

            Dictionary<string, object> dataToSend = new Dictionary<string, object>();

            dataToSend.Add("titles", titles);
            dataToSend.Add("data", KParkTable);

            return js.Serialize(dataToSend);
        }

        static TitlesToSend FillTitleToSend(string name, string titleUkr, string titleRus)
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