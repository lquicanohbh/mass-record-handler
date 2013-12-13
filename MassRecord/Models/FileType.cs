using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MassRecord.Models
{
    public class FileType
    {
        public string Description { get; set; }
        public string Code { get; set; }
        public ICollection<FileAction> FileActions { get; set; }
    }
}
