using MassRecord.Models;
using MassRecord.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassRecord.Repositories
{
    public abstract class BaseRepository
    {
        public BaseRepository(BaseConfiguration baseConfiguration, MyViewModel myViewModel)
        {
            _BaseConfiguration = baseConfiguration;
            _MyViewModel = myViewModel;
        }
        protected BaseConfiguration _BaseConfiguration { get; set; }
        protected MyViewModel _MyViewModel { get; set; }
    }
}
