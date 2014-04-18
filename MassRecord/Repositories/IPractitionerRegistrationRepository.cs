using MassRecord.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassRecord.Repositories
{
    public interface IPractitionerRegistrationRepository
    {
        void websvc_EditPractitionerRegisterCompleted(CustomPractitionerRegistration customPractitionerRegistration, object sender, PractitionerRegister.EditPractitionerRegisterCompletedEventArgs e);
    }
}
