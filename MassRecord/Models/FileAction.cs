using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MassRecord.Helpers;

namespace MassRecord.Models
{
    public class FileAction
    {
        public CustomAction Action { get; set; }
    }
    public enum CustomAction
    {
        [Description("None")]
        None = 0,

        [Description("Add")]
        Add = 1,

        [Description("Edit")]
        Edit = 2,

        [Description("Delete")]
        Delete = 3,

        [Description("Reset Password")]
        ResetPassword = 4
    }
}
