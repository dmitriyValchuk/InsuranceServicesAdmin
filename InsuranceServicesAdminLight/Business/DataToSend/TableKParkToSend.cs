using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuranceServicesAdminLight.Business.DataToSend
{
    public class TableKParkToSend
    {
        public string IsLegalEntity { get; set; }
        public int TransportCountFrom { get; set; }
        public int TransportCountTo { get; set; }
        public double Value { get; set; }
    }
}