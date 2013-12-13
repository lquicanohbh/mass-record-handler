using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassRecord.Models
{
    public class CustomClientResponse
    {
        public ClientDemographics.WebServiceResponse WebResponse { get; set; }
        public string ClientId { get; set; }
    }
}
