using MassRecord.Helpers;
using MassRecord.Models;
using MassRecord.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassRecord.Repositories
{
    public class ClientDemographicsRepository : BaseRepository, IRepository, IClientDemographicsRepository
    {
        private ClientDemographics.ClientDemographics ClientDemographicsWebService;

        public ClientDemographicsRepository(BaseConfiguration baseConfiguration, MyViewModel myViewModel)
            : base(baseConfiguration, myViewModel)
        {
            ClientDemographicsWebService = new ClientDemographics.ClientDemographics();
        }

        public void Add(object entity)
        {
            throw new NotImplementedException();
        }

        public void Edit(object entity)
        {
            CustomClientDemographics customClientDemographics = (CustomClientDemographics)entity;
            ClientDemographicsWebService.UpdateClientDemographicsCompleted +=
                (sender, e) => websvc_UpdateClientDemographicsCompleted(customClientDemographics, sender, e);
            ClientDemographicsWebService.UpdateClientDemographicsAsync(
                _BaseConfiguration.SystemCode,
                _BaseConfiguration.UserName,
                _BaseConfiguration.Password,
                customClientDemographics.ClientDemographicsWebServiceObject,
                customClientDemographics.EntityId
                );
        }

        public void Delete(object entity)
        {
            throw new NotImplementedException();
        }

        public void ResetPassword(object entity)
        {
            throw new NotImplementedException();
        }

        public List<object> GetAllEntities(string[] fileLines)
        {
            var clientList = new List<object>();
            for (int i = 0; i < fileLines.Length; i++)
            {
                var client = new CustomClientDemographics();
                var record = new List<string>();
                try
                {
                    record = fileLines[i].Split('|').ToList();
                }
                catch
                {
                    throw new IndexOutOfRangeException(String.Format("Incorrect number of parameters. Line {0}.", (i + 1)));
                }
                client.EntityId = record[0];
                client.ClientDemographicsWebServiceObject.Alias4 = record[1];
                clientList.Add(client);
            }
            return clientList;
        }

        public void websvc_UpdateClientDemographicsCompleted(CustomClientDemographics customClientDemographics, object sender, ClientDemographics.UpdateClientDemographicsCompletedEventArgs e)
        {
            ++Helpers.ProgressHelper.NumberOfClientsReturned;
            _MyViewModel.CurrentProgress = Helpers.ProgressHelper.CurrentProgress;
            if (e.Result.Status != 1)
                _MyViewModel.RecordsProcessed += String.Format("Entity {0}: {1}\r\n", customClientDemographics.EntityId, e.Result.Message);
        }
    }
}
