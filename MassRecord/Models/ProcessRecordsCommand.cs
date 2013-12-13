using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassRecord.Models
{
    public class ProcessRecordsCommand : ICustomCommand
    {
        private FileType _FileType;
        private FileAction _FileAction;


        public ProcessRecordsCommand(FileType fileType, FileAction fileAction)
        {
            _FileType = fileType;
            _FileAction = fileAction;
        }

        protected virtual void SetupTransformBlock()
        {

        }

        public void ExecuteCommand()
        {
            throw new NotImplementedException();
        }
    }
}
