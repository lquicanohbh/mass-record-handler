using MassRecord.Models;
using MassRecord.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassRecord.Repositories
{
    public interface IRepository
    {
        void Add(object entity);
        void Edit(object entity);
        void Delete(object entity);
        void ResetPassword(object entity);
        List<object> GetAllEntities(string[] fileLines);
    }
}
