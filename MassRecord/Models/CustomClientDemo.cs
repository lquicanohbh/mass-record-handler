using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassRecord.Models.Interfaces;

namespace MassRecord.Models
{
    public class CustomClientDemo : BaseEntity
    {
        public ClientDemographics.ClientDemographicsObject ClientDemographics { get; set; }
        public ClientDemographics.WebServiceResponse WebResponse { get; set; }
    }
}
