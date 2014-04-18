using MassRecord.Helpers;
using MassRecord.Models;
using MassRecord.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassRecord.Repositories
{
    public class PractitionerRegistrationRepository : BaseRepository, IRepository, IPractitionerRegistrationRepository
    {
        private PractitionerRegister.WebServices PractitionerRegistrationWebService;

        public PractitionerRegistrationRepository(BaseConfiguration baseConfiguration, MyViewModel myViewModel)
            : base(baseConfiguration, myViewModel)
        {
            PractitionerRegistrationWebService = new PractitionerRegister.WebServices();
        }
        public void Add(object entity)
        {
            throw new NotImplementedException();
        }

        public void Edit(object entity)
        {
            CustomPractitionerRegistration customPractitionerRegistration = (CustomPractitionerRegistration)entity;
            PractitionerRegistrationWebService.EditPractitionerRegisterCompleted +=
                (sender, e) => websvc_EditPractitionerRegisterCompleted(customPractitionerRegistration, sender, e);
            PractitionerRegistrationWebService.EditPractitionerRegisterAsync(_BaseConfiguration.SystemCode,
                _BaseConfiguration.UserName,
                _BaseConfiguration.Password,
                customPractitionerRegistration.PractitionerRegistrationWebServiceObject,
                ConvertHelper.ConvertStringToLong(customPractitionerRegistration.EntityId),
                true);
        }

        public void Delete(object entity)
        {
            throw new NotImplementedException();
        }

        public void ResetPassword(object entity)
        {
            throw new NotImplementedException();
        }

        public List<object> GetAllEntities(string[] fileLines)
        {
            var practList = new List<object>();
            for (int i = 0; i < fileLines.Length; i++)
            {
                var pract = new CustomPractitionerRegistration();
                var record = new List<string>();
                try
                {
                    record = fileLines[i].Split('|').ToList();
                }
                catch
                {
                    throw new IndexOutOfRangeException(String.Format("Incorrect number of parameters. Line {0}.", (i + 1)));
                }
                pract.EntityId = record[0];
                pract.PractitionerRegistrationWebServiceObject.Name = record[1];
                pract.PractitionerRegistrationWebServiceObject.PractitionerCategory = record[2];
                pract.PractitionerRegistrationWebServiceObject.Sex = record[3];
                pract.PractitionerRegistrationWebServiceObject.EthnicOrigin = record[4];
                pract.PractitionerRegistrationWebServiceObject.CategoriesForCoverage = record[5];
                pract.PractitionerRegistrationWebServiceObject.DateOfBirth = DateTime.Parse(record[6]);
                pract.PractitionerRegistrationWebServiceObject.DateOfBirthSpecified = true;
                pract.PractitionerRegistrationWebServiceObject.OfficeAddressStreet = record[7];
                pract.PractitionerRegistrationWebServiceObject.OfficeAddressCity = record[8];
                pract.PractitionerRegistrationWebServiceObject.OfficeAddressState = record[9];
                pract.PractitionerRegistrationWebServiceObject.OfficeAddressZipCode = record[10];
                pract.PractitionerRegistrationWebServiceObject.OfficeTelephone1 = record[11];
                practList.Add(pract);
            }
            return practList;
        }

        public void websvc_EditPractitionerRegisterCompleted(CustomPractitionerRegistration customPractitionerRegistration, object sender, PractitionerRegister.EditPractitionerRegisterCompletedEventArgs e)
        {
            ++Helpers.ProgressHelper.NumberOfClientsReturned;
            _MyViewModel.CurrentProgress = Helpers.ProgressHelper.CurrentProgress;
            if (e.Result.Status != 1)
                _MyViewModel.RecordsProcessed += String.Format("Entity {0}: {1}\r\n", customPractitionerRegistration.EntityId, e.Result.Message);
        }
    }
}
