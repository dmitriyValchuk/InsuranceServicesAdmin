using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InsuranceServicesAdminLight.Models;

namespace InsuranceServicesAdminLight.Controllers
{
    public class HomeController : Controller
    {
        InsuranceServicesContext db = new InsuranceServicesContext();

        public ActionResult Index()
        {
            var companies = db.Companies;
            //ViewBag.Companies = companies;
            return View(companies);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult CompanySearch(string companyName)
        {
            var company = db.Companies.Where(c => c.Name.Contains(companyName)).ToList();
            if (company.Count <= 0)
                return HttpNotFound();
            return PartialView(company);
        }
    }
}