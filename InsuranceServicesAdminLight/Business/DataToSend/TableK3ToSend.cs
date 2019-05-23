using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuranceServicesAdminLight.Business.DataToSend
{
    public class TableK3ToSend
    {
        public string CarZoneOfRegistration { get; set; }
        public string IsLegalEntity { get; set; }
        public string InsuranceTypeOfCar { get; set; }
        public double Value { get; set; }
    }
}