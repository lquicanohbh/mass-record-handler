using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassRecord.Models;

namespace MassRecord.Repositories
{
    public class ClientDemographicsRepository : BaseRepository<ClientDemographicsRepository>, IUpdatable
    {
        private CustomClientDemo _CustomClientDemo;
        private BaseConfiguration _Settings;
        public ClientDemographicsRepository(CustomClientDemo customClientDemo)
        {
            _CustomClientDemo = customClientDemo;
            _Settings = new BaseConfiguration();
        }
        public void Update()
        {
            try
            {
                var webSvc = new ClientDemographics.ClientDemographics();
                _CustomClientDemo.WebResponse = webSvc.UpdateClientDemographics(_Settings.SystemCode,
                    _Settings.UserName,
                    _Settings.Password,
                    _CustomClientDemo.ClientDemographics,
                    _CustomClientDemo.EntityId);

            }
            catch (Exception)
            {
                throw new Exception(String.Format("Unable to file {0} web service.", typeof(CustomClientDemo).ToString()));
            }
        }
    }
}
