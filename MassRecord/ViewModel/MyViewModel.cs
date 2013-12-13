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

        #region Properties
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

        private string _DefaultPath;
        public string DefaultPath
        {
            get { return _DefaultPath; }
            set { _DefaultPath = value; }
        }

        public FileAction SelectedAction { get; set; }
        #endregion

        #region Constructors and Initializers
        public MyViewModel()
        {
            RegisterCommands();
            InitializeComboBoxes();
        }
        private void RegisterCommands()
        {
            OpenFileCommand = new RelayCommand(new OpenFileDialogCommand(DefaultPath).ExecuteCommand);
            ProcessFileCommand = new RelayCommand(ProcessFile);
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
        #endregion

        #region Methods
        public FileAction GetAction()
        {
            return new FileAction();
        }
        #endregion

        private void ProcessFile()
        {
            TransformBlock<CustomClientDemo, CustomClientDemo> transformBlock =
                new TransformBlock<CustomClientDemo, CustomClientDemo>(
                    clientDemo =>
                    {
                        var webSvc = new ClientDemographics.ClientDemographics().UpdateClientDemographics(
                            "LIVE", "ODBC", "hotwire2011", clientDemo.ClientDemographics, clientDemo.EntityId);
                        clientDemo.WebResponse = webSvc;
                        return clientDemo;

                    },
                    new ExecutionDataflowBlockOptions
                    {
                        MaxDegreeOfParallelism = 20
                    });

            ActionBlock<CustomClientDemo> notificationBlock = new ActionBlock<CustomClientDemo>(
                webSvcResponse =>
                {
                    RecordsProcessed += String.Format("Client {0}: {1}\r\n", webSvcResponse.EntityId, webSvcResponse.WebResponse.Message);
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
                        EntityId = tempObj[0],
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
    }
}
