using MupiModel.Dtos;
using MupiModel.Dtos.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatioManager.Services
{
    public interface IContactService
    {


        public Task<List<ResultCreatioRequest<ContactDto>>> AddUpdateContacts(List<(ContactDto,bool)> contacts);

        public Task<List<(ContactDto,bool)>> ExistsContacts(List<ContactDto> contacts);
    }
}
