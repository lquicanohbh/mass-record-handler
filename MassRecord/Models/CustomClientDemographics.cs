using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassRecord.Models
{
    public class CustomClientDemographics : BaseEntity
    {
        public CustomClientDemographics()
        {
            ClientDemographicsWebServiceObject = new ClientDemographics.ClientDemographicsObject();
        }
        public ClientDemographics.ClientDemographicsObject ClientDemographicsWebServiceObject { get; set; }
    }
}
