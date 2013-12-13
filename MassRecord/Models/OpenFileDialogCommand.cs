using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace MassRecord.Models
{
    public class OpenFileDialogCommand : ICustomCommand
    {
        private string _InitialDirectory;
        public string SelectedPath { get; private set; }

        public OpenFileDialogCommand(string InitialDirectory)
        {
            _InitialDirectory = InitialDirectory;
        }

        public void ExecuteCommand()
        {
            var dialog = InitializeOpenFileDialog();
            dialog.ShowDialog();
            SelectedPath = dialog.FileName;
        }

        private OpenFileDialog InitializeOpenFileDialog()
        {
            return new OpenFileDialog() { InitialDirectory = _InitialDirectory };
        }
    }
}
