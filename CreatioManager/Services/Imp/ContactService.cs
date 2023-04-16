using CreatioManager.HttpClients;
using CreatioManager.Models;
using CreatioManager.Models.Batch;
using CreatioManager.Models.Result;
using MupiModel.Dtos;
using MupiModel.Dtos.Results;
using Newtonsoft.Json;

namespace CreatioManager.Services.Imp
{
    public class ContactService : IContactService
    {
        private readonly BatchClient _batchClient;

        public ContactService(BatchClient batchClient)
        {
            _batchClient = batchClient;
        }

        public async Task<List<ResultCreatioRequest<ContactDto>>> AddUpdateContacts(List<(ContactDto, bool)> contacts)
        {

            var batchRequests = await ShapeRequestsAddupdateContact(contacts);

            var resultRequest = _batchClient.RequestBatch(batchRequests);


            return new List<ResultCreatioRequest<ContactDto>>();
        }


        public async Task<List<(ContactDto, bool)>> ExistsContacts(List<ContactDto> contacts)
        {
            //Shape exists requests 
            var batchRequest = await ShapeRequestExistsContacts(contacts);

            var requestResponse = await _batchClient.RequestBatch(batchRequest);
            var creatioContacts = new List<Contact>();


            foreach (var elem in requestResponse.Responses)
            {
                var resultResponse = JsonConvert.DeserializeObject<ResultListEntity<Contact>>(elem.Body.ToString());
                creatioContacts.AddRange(resultResponse.Value);
            }



            var result = contacts.Select(x =>
            {
                var exists = false;
                if(creatioContacts.Any(y => y.UsrIDContacto == x.IdMupiContacto))
                {
                    exists = true;
                    var creatioContact = creatioContacts.First(y => y.UsrIDContacto == x.IdMupiContacto);
                    x.Id = creatioContact.Id;
                }
                return (x, exists);

            }).ToList();

            return result;
        }


        private async Task<IEnumerable<BatchRequest>> ShapeRequestExistsContacts(List<ContactDto> contacts)
        {
            var result = new List<BatchRequest>();
            int count = 1;
            foreach (var item in contacts)
            {
                var request = new BatchRequest
                {
                    Id = $"R{count}",
                    Method = BatchRequest.GET,
                    Url = Contact.GetUrlFindContactByIdContacto(item.IdMupiContacto),
                    Headers = new BatchHeaders(),
                    Body = new object()
                };
                result.Add(request);
                count++;
            }

            return result;

        }

        private async Task<IEnumerable<BatchRequest>> ShapeRequestsAddupdateContact(List<(ContactDto, bool)> contacts)
        {
            var result = new List<BatchRequest>();

            var count = 1;
            foreach (var elem in contacts)
            {
                var contact = elem.Item1;
                var existsOnCreatio = elem.Item2;
                var url = nameof(Contact);
                var httpVerb = BatchRequest.POST;
                if (existsOnCreatio)
                {
                    url += $"/{contact.Id}";
                    httpVerb = BatchRequest.PATCH;
                }


                var creatioContact = new Contact
                {
                    UsrIDContacto = contact.IdMupiContacto.ToString(),
                    Name = contact.Nombres,
                    Surname = contact.Apellidos,
                    Email = contact.Email,
                    JobTitle = "Azure test caao",
                    UsrSincronizar = false
                };


                var request = new BatchRequest
                {
                    Id = $"R{count}",
                    Method = httpVerb,
                    Url = url,
                    Headers = new BatchHeaders(),
                    Body = creatioContact
                };
                result.Add(request);
                count++;

            }

            return result;

        }

    }
}
