using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassRecord.Models
{
    public class CustomPractitionerRegistration : BaseEntity
    {
        public CustomPractitionerRegistration()
        {
            PractitionerRegistrationWebServiceObject = new PractitionerRegister.PractitionerRegistrationObject();
        }
        public PractitionerRegister.PractitionerRegistrationObject PractitionerRegistrationWebServiceObject { get; set; }
    }
}
