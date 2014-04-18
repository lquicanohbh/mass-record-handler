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
using MassRecord.Models.Interfaces;
using MassRecord.Repositories;
using System.Threading;


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

        private double _CurrentProgress;

        public double CurrentProgress
        {
            get { return _CurrentProgress; }
            set
            {
                _CurrentProgress = value;
                RaisePropertyChanged("CurrentProgress");
            }
        }


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

        private ValueName _SelectedAction;
        public ValueName SelectedAction
        {
            get { return _SelectedAction; }
            set
            {
                _SelectedAction = value;
                RaisePropertyChanged("SelectedAction");
            }
        }

        private BaseConfiguration BaseConfiguration { get; set; }
        #endregion

        #region Constructors and Initializers
        public MyViewModel()
        {
            RegisterCommands();
            InitializeComboBoxes();
            InitializeConfiguration();
        }
        private void RegisterCommands()
        {
            OpenFileCommand = new RelayCommand(OpenFileDialog);
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
                },
                new FileType()
                {
                    Description = "Practitioner Registration",
                    Code = "PRACT",
                    FileActions = new List<FileAction>()
                    {
                        new FileAction() { Action = CustomAction.None},
                        new FileAction() { Action = CustomAction.Add},
                        new FileAction() {Action = CustomAction.Edit}
                    }
                }
            };
            FileTypes = FileTypes.OrderBy(x => x.Description).ToList();
        }
        private void InitializeConfiguration()
        {
            BaseConfiguration = new BaseConfiguration
            {
                SystemCode = "LIVE",
                UserName = "LQUICANO",
                Password = "tinchair719"
            };
        }
        #endregion

        #region Methods
        public FileAction GetAction()
        {
            return new FileAction();
        }
        #endregion

        private void OpenFileDialog()
        {
            var dialog = new OpenFileDialog() { InitialDirectory = DefaultPath };
            try
            {
                dialog.ShowDialog();
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Unable to execute OpenFileDialog");
            }
            SelectedPath = dialog.FileName;
        }

        private void ProcessFile()
        {
            ResetValues();
            var records = ReadAllRecords();
            ProgressHelper.TotalClients = records.Count;
            foreach (var record in records)
            {
                IRepository entityRep = RepositoryFactory.GetRepository(BaseConfiguration, this, _SelectedFileType.Code);
                switch ((CustomAction)_SelectedAction.Value)
                {
                    case CustomAction.Add:
                        entityRep.Add(record);
                        break;
                    case CustomAction.Edit:
                        entityRep.Edit(record);
                        break;
                    case CustomAction.Delete:
                        entityRep.Delete(record);
                        break;
                    case CustomAction.ResetPassword:
                        entityRep.ResetPassword(record);
                        break;
                    default:
                        break;
                }
            }
        }

        private List<object> ReadAllRecords()
        {
            IRepository repository = RepositoryFactory.GetRepository(BaseConfiguration, this, _SelectedFileType.Code);
            return repository.GetAllEntities(IOHelper.ReadFile(_SelectedPath));
        }

        private void ResetValues()
        {
            ProgressHelper.ResetAll();
        }
    }
}
