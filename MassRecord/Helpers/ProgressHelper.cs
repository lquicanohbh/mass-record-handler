using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassRecord.Helpers
{
    static public class ProgressHelper
    {
        static public double TotalClients = 1;
        static public int NumberOfClientsReturned = 0;
        static public double CurrentProgress
        {
            get
            {
                return (NumberOfClientsReturned / TotalClients) * 100;
            }
        }
        static public void ResetAll()
        {
            TotalClients = 0;
            NumberOfClientsReturned = 0;
        }
    }
}
