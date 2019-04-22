using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using InsuranceServicesAdminLight.Models;

namespace InsuranceServicesAdminLight
{
    /// <summary>
    /// Сводное описание для TestService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    // [System.Web.Script.Services.ScriptService]
    public class TestService : System.Web.Services.WebService
    {

        [WebMethod]
        public void GetAllCompanies()
        {
            InsuranceServicesContext db = new InsuranceServicesContext();
            var companies = db.Companies.ToArray();
            List<CompanyToSend> compList = new List<CompanyToSend>();

            foreach(var c in companies)
            {
                CompanyToSend cts = new CompanyToSend();
                cts.Id = c.Id;
                cts.Name = c.Name;
                cts.Code = c.Code;
                compList.Add(cts);
            }
            //List<Company> comp = new List<Company>();
            
            //foreach(var c in companies)
            //{
            //    Company tempCompany = new Company();
            //    tempCompany.Id = c.Id;
            //    tempCompany.Name = c.Name;
            //    tempCompany.Code = c.Code;
            //    comp.Add(tempCompany);
            //}
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(compList));            
        }
    }

    public class CompanyToSend
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Code { get; set; }
    }

}
