using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuranceServicesAdminLight.Business
{
    public class ResponseToClient
    {
        public ResponseType responseType { get; set; }
        public string responseText { get; set; }
    }

    public enum ResponseType
    {
        Bad,
        Good
    }
}