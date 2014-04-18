using MassRecord.Models;
using MassRecord.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MassRecord.Repositories
{
    public class ClientAppointmentsRepository : BaseRepository, IRepository
    {
        public ClientAppointmentsRepository(BaseConfiguration baseConfiguration, MyViewModel myViewModel)
            : base(baseConfiguration, myViewModel)
        {
        }

        public void Add(object entity)
        {
            throw new NotImplementedException();
        }

        public void Edit(object entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(object entity)
        {
            throw new NotImplementedException();
        }

        public void ResetPassword(object entity)
        {
            throw new NotImplementedException();
        }

        List<object> IRepository.GetAllEntities(string[] fileLines)
        {
            throw new NotImplementedException();
        }
    }
}
