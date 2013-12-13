using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MassRecord.Helpers;
using MassRecord.Models;
using Microsoft.Win32;
using System.Threading.Tasks.Dataflow;
using System.Threading.Tasks;
using System.IO;


namespace MassRecord.ViewModel
{
    public class MyViewModel : ViewModelBase
    {
        #region ComboBoxes
        public ICollection<FileType> FileTypes { get; private set; }
        public ICollection<ValueName> FileActions { get; private set; }
        #endregion

        public static RelayCommand OpenFileCommand { get; set; }
        public static RelayCommand ProcessFileCommand { get; set; }

        private string _RecordsProcessed;

        public string RecordsProcessed
        {
            get { return _RecordsProcessed; }
            set
            {
                _RecordsProcessed = value;
                RaisePropertyChanged("RecordsProcessed");
            }
        }


        private string _SelectedPath;

        public string SelectedPath
        {
            get { return _SelectedPath; }
            set
            {
                _SelectedPath = value;
                RaisePropertyChanged("SelectedPath");
            }
        }

        private FileType _SelectedFileType;

        public FileType SelectedFileType
        {
            get { return _SelectedFileType; }
            set
            {
                _SelectedFileType = value;
                RaisePropertyChanged("SelectedFileType");
                this.FileActions = EnumHelper.GetItems<CustomAction>(_SelectedFileType.FileActions.Select(x => x.Action).ToList()).ToList();
                RaisePropertyChanged("FileActions");
            }
        }

        public FileAction GetAction()
        {
            return new FileAction();
        }

        public FileAction SelectedAction { get; set; }

        private string _DefaultPath;

        public string DefaultPath
        {
            get { return _DefaultPath; }
            set { _DefaultPath = value; }
        }

        public MyViewModel()
        {
            RegisterCommands();
            InitializeComboBoxes();
        }

        private void InitializeComboBoxes()
        {
            this.FileTypes = new List<FileType>()
            {
                new FileType() 
                {
                    Description = "Client Demographics",
                    Code = "DEMO",
                    FileActions = new List<FileAction>()
                    {
                        new FileAction() { Action = CustomAction.None},
                        new FileAction() { Action = CustomAction.Edit}
                    }
                },
                new FileType()
                {
                    Description = "Client Appointments",
                    Code = "APPT",
                    FileActions = new List<FileAction>()
                    {
                        new FileAction() { Action = CustomAction.None},
                        new FileAction() { Action = CustomAction.Add},
                        new FileAction() { Action = CustomAction.Edit},
                        new FileAction() { Action = CustomAction.Delete}
                    }
                },
                new FileType()
                {
                    Description = "User Definition",
                    Code = "USER",
                    FileActions = new List<FileAction>()
                    {
                        new FileAction() { Action = CustomAction.None},
                        new FileAction() { Action = CustomAction.Add},
                        new FileAction() { Action = CustomAction.Edit},
                        new FileAction() { Action = CustomAction.Delete},
                        new FileAction() {Action = CustomAction.ResetPassword}
                    }
                }
            };
        }

        private void RegisterCommands()
        {
            OpenFileCommand = new RelayCommand(ExecuteOpenFileDialog);
            ProcessFileCommand = new RelayCommand(ProcessFile);
        }

        private void ProcessFile()
        {
            TransformBlock<CustomClientDemo, CustomClientResponse> transformBlock =
                new TransformBlock<CustomClientDemo, CustomClientResponse>(
                    clientDemo =>
                    {
                        var webSvc = new ClientDemographics.ClientDemographics().UpdateClientDemographics(
                            "LIVE", "ODBC", "hotwire2011", clientDemo.ClientDemographics, clientDemo.ClientId);
                        var response = new CustomClientResponse { ClientId = clientDemo.ClientId };
                        response.WebResponse = webSvc;
                        return response;
                    },
                    new ExecutionDataflowBlockOptions
                    {
                        MaxDegreeOfParallelism = 20
                    });

            ActionBlock<CustomClientResponse> notificationBlock = new ActionBlock<CustomClientResponse>(
                webSvcResponse =>
                {
                    RecordsProcessed += String.Format("Client {0}: {1}\r\n", webSvcResponse.ClientId, webSvcResponse.WebResponse.Message);
                },
                new ExecutionDataflowBlockOptions
                {
                    TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext()
                });

            transformBlock.LinkTo(notificationBlock);

            var clients = getAllClients();
            foreach (var client in clients)
            {
                transformBlock.Post(client);
            }
        }
        public List<CustomClientDemo> getAllClients()
        {
            try
            {
                string[] allText = File.ReadAllLines(_SelectedPath);
                var clientList = new List<CustomClientDemo>();
                for (int i = 0; i < allText.Length; i++)
                {
                    var tempObj = allText[i].Split('|');
                    clientList.Add(new CustomClientDemo
                    {
                        ClientId = tempObj[0],
                        ClientDemographics = new ClientDemographics.ClientDemographicsObject()
                        {
                            Alias9 = tempObj[1]
                        }
                    });
                }
                return clientList;
            }
            catch (Exception)
            {
                throw new ArgumentOutOfRangeException("Incorrect number of parameters.");
            }
        }
        private void ExecuteOpenFileDialog()
        {
            var dialog = new OpenFileDialog { InitialDirectory = DefaultPath };
            dialog.ShowDialog();
            SelectedPath = dialog.FileName;
        }
    }
}
