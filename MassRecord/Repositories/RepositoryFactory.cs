using MassRecord.Models;
using MassRecord.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassRecord.Repositories
{
    public class RepositoryFactory
    {
        public static IRepository GetRepository(BaseConfiguration baseConfiguration, MyViewModel myViewModel, string repositoryName)
        {
            IRepository rep;
            switch (repositoryName)
            {
                case "DEMO":
                    rep = new ClientDemographicsRepository(baseConfiguration, myViewModel);
                    break;
                case "APPT":
                    rep = new ClientAppointmentsRepository(baseConfiguration, myViewModel);
                    break;
                case "PRACT":
                    rep = new PractitionerRegistrationRepository(baseConfiguration, myViewModel);
                    break;
                default:
                    rep = null;
                    break;
            }
            return rep;
        }
    }
}
