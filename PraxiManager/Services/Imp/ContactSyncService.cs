using Azure.Core;
using MupiModel.Dtos.AppSettings;
using ICreatioContactService = CreatioManager.Services.IContactService;
using IMupiContactService = MupiBussines.Services.IContactService;

namespace PraxiManager.Services.Imp
{
    public class ContactSyncService : IContactSyncService
    {
        private readonly IMupiContactService _mupiCcontactService;
        private readonly ICreatioContactService _creatioContactService;
        private readonly MupiSetting _mupiSetting;

        public ContactSyncService(IMupiContactService mupiCcontactService, ICreatioContactService creatioContactService, MupiSetting mupiSetting)
        {
            _mupiCcontactService = mupiCcontactService;
            _creatioContactService = creatioContactService;
            _mupiSetting = mupiSetting;
        }

        public async Task SyncContactsFromMupi(int frequency, DateTime from, CancellationToken stoppingToken)
        {
            frequency = frequency < _mupiSetting.DefaultSyncFrequency ? _mupiSetting.DefaultSyncFrequency : frequency;
            from = from < _mupiSetting.DefaultSyncDate ? _mupiSetting.DefaultSyncDate : from;

            //Get contacts from mupi
            var contactsToSync =await _mupiCcontactService.GetContactsToSync(from);


            for (int i = 0; i < contactsToSync.Count(); i = i + 10) {

                var contactsVerified = await _creatioContactService.ExistsContacts(contactsToSync.Skip(i).Take(10).ToList());

                //sent contacts to creatio

                var resultAdd = await _creatioContactService.AddUpdateContacts(contactsVerified);

            }

                //Verify exists contact

       


           // await Task.Delay(frequency*60*1000,stoppingToken);
        }
    }
}
