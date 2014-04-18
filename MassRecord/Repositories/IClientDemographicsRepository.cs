using MassRecord.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassRecord.Repositories
{
    public interface IClientDemographicsRepository
    {
        void websvc_UpdateClientDemographicsCompleted(CustomClientDemographics customClientDemographics, object sender, ClientDemographics.UpdateClientDemographicsCompletedEventArgs e);
    }
}
